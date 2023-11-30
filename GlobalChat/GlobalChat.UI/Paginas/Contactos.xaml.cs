using GlobalChat.Business.Dtos;
using GlobalChat.UI.Componentes;
using System.Net.Http.Json;
using System.Text.Json;

namespace GlobalChat.UI.Paginas;

public partial class Contactos : ContentPage
{
	List<ContactoCompletoDto> contactosCompletos = new List<ContactoCompletoDto>();
	public Contactos()
	{
		InitializeComponent();
		CargarContactos();
    }

	public async void CargarContactos()
	{
        PeticionDto<int> peticionEdi = new PeticionDto<int>() { TokenPeticion = ServicioAPI.TokenUsuario, Value = ServicioAPI.Usuario.Id };
        HttpResponseMessage respuesta = await ServicioAPI.Cliente.PostAsJsonAsync("api/Contactos/ObtenerListaContactos", peticionEdi, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        if (respuesta.IsSuccessStatusCode)
        {
            string respuestaStr = await respuesta.Content.ReadAsStringAsync();
            PeticionDto<List<ContactoCompletoDto>> petContactos = JsonSerializer.Deserialize<PeticionDto<List<ContactoCompletoDto>>>(respuestaStr, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new PeticionDto<List<ContactoCompletoDto>>();
            if (petContactos.PeticionCorrecta)
            {
                contactosCompletos = petContactos.Value;
                MostrarContactos();
            }
            else if (petContactos.ErrorPorToken)
            {
                await ServicioAPI.ReLogin(Navigation);
                await DisplayAlert("Error", "Error en la comunicacion con el servidor, intentelo de nuevo.", "OK");
            }
            else
            {
                await DisplayAlert("Error", petContactos.MensajeError, "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Error al conectar con el servidor.", "OK");
        }
    }

    public void MostrarContactos()
    {
        elementosMenu.Clear();
        foreach (var contacto in contactosCompletos)
        {
            ContactoComp comp = new ContactoComp(contacto);
            elementosMenu.Add(comp);
        }
    }

    private async void AgregarContacto_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("Agregar usuario", "Indica el nombre del usuario a agregar");
        if (!string.IsNullOrEmpty(result))
        {
            NuevoContactoDto nuevo = new NuevoContactoDto();
            nuevo.IdUsuarioActual = ServicioAPI.Usuario.Id;
            nuevo.NombreContacto = result;
            PeticionDto<NuevoContactoDto> peNuevoCon = new PeticionDto<NuevoContactoDto>() { TokenPeticion = ServicioAPI.TokenUsuario, Value = nuevo };
            HttpResponseMessage respuesta = await ServicioAPI.Cliente.PostAsJsonAsync("api/Contactos/AgregarContacto", peNuevoCon, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            if (respuesta.IsSuccessStatusCode)
            {
                string respuestaStr = await respuesta.Content.ReadAsStringAsync();
                PeticionDto<ContactoDto> petUsu = JsonSerializer.Deserialize<PeticionDto<ContactoDto>>(respuestaStr, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new PeticionDto<ContactoDto>();
                if (petUsu.PeticionCorrecta)
                {
                    await DisplayAlert("Info", "Contacto Agregado.", "OK");
                    CargarContactos();
                }
                else if (petUsu.ErrorPorToken)
                {
                    await ServicioAPI.ReLogin(Navigation);
                    await DisplayAlert("Error", "Error en la comunicacion con el servidor, intentelo de nuevo.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", petUsu.MensajeError, "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Error al conectar con el servidor.", "OK");
            }
        }
    }
}