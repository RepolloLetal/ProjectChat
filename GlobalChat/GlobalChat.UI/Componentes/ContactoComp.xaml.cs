using GlobalChat.Business.Dtos;
using Microsoft.Maui.Controls.Shapes;
using System.Net.Http.Json;
using System.Text.Json;

namespace GlobalChat.UI.Componentes;

public partial class ContactoComp : ContentView
{
    public ContactoCompletoDto ContactoCompleto { get; set; }
    public event EventHandler? Selected;
    private bool canEdit;

    public ContactoComp(ContactoCompletoDto contactoCompleto, bool canEdit = false)
	{
		InitializeComponent();
        ContactoCompleto = contactoCompleto;
        if (contactoCompleto.Favorito)
        {
            img.Source = "favoritolleno.png";
        }
        else
        {
            img.Source = "favoritovacio.png";
        }
        DatosGridComp.BindingContext = ContactoCompleto;
        this.canEdit = canEdit;
        if(!canEdit)
        {
            imgBrr.IsVisible = false;
        }
    }

    private void Img_Clicked(object sender, EventArgs e)
    {
        if (canEdit)
        {
            ContactoCompleto.Favorito = !ContactoCompleto.Favorito;
            if (ContactoCompleto.Favorito)
            {
                img.Source = "favoritolleno.png";
            }
            else
            {
                img.Source = "favoritovacio.png";
            }
            EditarContacto();
        }
    }

    private async void ImgBrr_Clicked(object sender, EventArgs e)
    {
        bool acepto = await Application.Current.MainPage.DisplayAlert("Eliminar contacto", "¿Seguro que quieres eliminar el contacto?", "Sí", "No");
        if (canEdit && acepto)
        {
            ContactoDto eliminar = new ContactoDto();
            eliminar.IdUsuarioA = ServicioAPI.Usuario.Id;
            eliminar.IdUsuarioB = ContactoCompleto.IdOtroUsuario;
            PeticionDto<ContactoDto> peNuevoCon = new PeticionDto<ContactoDto>() { TokenPeticion = ServicioAPI.TokenUsuario, Value = eliminar };
            HttpResponseMessage respuesta = await ServicioAPI.Cliente.PostAsJsonAsync("api/Contactos/EliminarContacto", peNuevoCon, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            if (respuesta.IsSuccessStatusCode)
            {
                string respuestaStr = await respuesta.Content.ReadAsStringAsync();
                PeticionDto<ContactoDto> petUsu = JsonSerializer.Deserialize<PeticionDto<ContactoDto>>(respuestaStr, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new PeticionDto<ContactoDto>();
                if (petUsu.PeticionCorrecta)
                {
                    await Application.Current.MainPage.DisplayAlert("Info", "Contacto eliminado correctamente", "OK");
                    (Parent as StackLayout).Remove(this);
                }
                else if (petUsu.ErrorPorToken)
                {
                    await ServicioAPI.ReLogin(Navigation);
                    await Application.Current.MainPage.DisplayAlert("Error", "Error en la comunicacion con el servidor, intentelo de nuevo.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", petUsu.MensajeError, "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Error al conectar con el servidor.", "OK");
            }
        }
    }

    private async void EditarContacto()
    {
        ContactoDto editar = new ContactoDto();
        editar.IdUsuarioA = ServicioAPI.Usuario.Id;
        editar.IdUsuarioB = ContactoCompleto.IdOtroUsuario;
        editar.Favorito = ContactoCompleto.Favorito;
        PeticionDto<ContactoDto> peNuevoCon = new PeticionDto<ContactoDto>() { TokenPeticion = ServicioAPI.TokenUsuario, Value = editar };
        HttpResponseMessage respuesta = await ServicioAPI.Cliente.PostAsJsonAsync("api/Contactos/EditarContacto", peNuevoCon, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        if (respuesta.IsSuccessStatusCode)
        {
            string respuestaStr = await respuesta.Content.ReadAsStringAsync();
            PeticionDto<ContactoCompletoDto> petUsu = JsonSerializer.Deserialize<PeticionDto<ContactoCompletoDto>>(respuestaStr, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new PeticionDto<ContactoCompletoDto>();
            if (petUsu.PeticionCorrecta)
            {
                ContactoCompleto = petUsu.Value;
            }
            else if (petUsu.ErrorPorToken)
            {
                await ServicioAPI.ReLogin(Navigation);
                await Application.Current.MainPage.DisplayAlert("Error", "Error en la comunicacion con el servidor, intentelo de nuevo.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", petUsu.MensajeError, "OK");
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Error al conectar con el servidor.", "OK");
        }
    }

    private void Contacto_Clicked(object sender, EventArgs e)
    {
        Selected?.Invoke(ContactoCompleto, e);
    }
}