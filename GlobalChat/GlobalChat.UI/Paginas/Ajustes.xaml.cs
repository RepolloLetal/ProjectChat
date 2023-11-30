using GlobalChat.Business.Dtos;
using System.Net.Http.Json;
using System.Text.Json;

namespace GlobalChat.UI.Paginas;

public partial class Ajustes : ContentPage
{
	public UsuarioDto Usuario { get; set; }
    public bool editando;

	public Ajustes()
	{
        Usuario = new UsuarioDto();
        CargarDatos();
        InitializeComponent();
        GridDatos.BindingContext = Usuario;
    }

	private void CargarDatos()
	{
		Usuario.Id = ServicioAPI.Usuario.Id;
		Usuario.Nombre = ServicioAPI.Usuario.Nombre;
		Usuario.NombreLogin = ServicioAPI.Usuario.NombreLogin;
		Usuario.Password = ServicioAPI.Usuario.Password;
    }

    private void EditarPerfil_Clicked(object sender, EventArgs e)
    {
        if (editando)
        {
            EntryNombre.IsReadOnly = true;
            EntryNombreLogin.IsReadOnly = true;
            BtnEditarPerfil.Text = "Editar perfil";
            ConfirmarCambios();
        }
        else
        {
            EntryNombre.IsReadOnly = false;
            EntryNombreLogin.IsReadOnly = false;
            BtnEditarPerfil.Text = "Confirmar cambios";
        }
        editando = !editando;
    }

    private async void ConfirmarCambios()
    {
        PeticionDto<UsuarioDto> peticionEdi = new PeticionDto<UsuarioDto>() { TokenPeticion = ServicioAPI.TokenUsuario, Value = Usuario };
        HttpResponseMessage respuesta = await ServicioAPI.Cliente.PostAsJsonAsync("api/Usuario/EditarUsuario", peticionEdi, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        if (respuesta.IsSuccessStatusCode)
        {
            string respuestaStr = await respuesta.Content.ReadAsStringAsync();
            PeticionDto<UsuarioDto> petUsu = JsonSerializer.Deserialize<PeticionDto<UsuarioDto>>(respuestaStr, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new PeticionDto<UsuarioDto>();
            if (petUsu.PeticionCorrecta)
            {
                petUsu.TokenPeticion = ServicioAPI.TokenUsuario;
                ServicioAPI.UsuarioLoggeado(petUsu);
                CargarDatos();
                await DisplayAlert("Info", "Datos actualizados correctamente.", "OK");
            }
            else if(petUsu.ErrorPorToken)
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