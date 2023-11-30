using GlobalChat.Business.Dtos;
using GlobalChat.UI.Modelos;
using System.Net.Http.Json;
using System.Text.Json;

namespace GlobalChat.UI.VentanasEmergentes;

public partial class VentanaLogin : ContentPage
{
    public UsuarioDto UsuarioLogin { get; set; } = new UsuarioDto();

    public VentanaLogin()
	{
		InitializeComponent();
        GridDatos.BindingContext = UsuarioLogin;
    }

    private async void IniciarSesion_Clicked(object sender, EventArgs e)
    {
        bool correcto = true;
        if (string.IsNullOrEmpty(UsuarioLogin.NombreLogin))
        {
            correcto = false;
            await DisplayAlert("Error", "Tienes que rellenar el nombre de login!", "OK");
        }
        if (string.IsNullOrEmpty(UsuarioLogin.Password))
        {
            correcto = false;
            await DisplayAlert("Error", "Tienes que rellenar la contraseña!", "OK");
        }
        if (correcto)
        {
            await LoginUsuarioAsync();
        }
    }

    private void Registar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new VentanaRegistro());
    }

    private async Task LoginUsuarioAsync()
    {
        HttpResponseMessage respuesta = await ServicioAPI.Cliente.PostAsJsonAsync("api/Usuario/LoginUsuario", UsuarioLogin, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        if (respuesta.IsSuccessStatusCode)
        {
            string respuestaStr = await respuesta.Content.ReadAsStringAsync();
            PeticionDto<UsuarioDto> petUsu = JsonSerializer.Deserialize<PeticionDto<UsuarioDto>>(respuestaStr, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new PeticionDto<UsuarioDto>();
            if (petUsu.PeticionCorrecta)
            {
                ServicioAPI.UsuarioLoggeado(petUsu);
                DatosGuardado datosGuardado = new DatosGuardado()
                {
                    SesionIniciada = true,
                    Usuario = petUsu.Value.NombreLogin,
                    Clave = petUsu.Value.Password
                };
                ServicioPersistencia.NuevaSesionIniciada(datosGuardado);
                await Navigation.PopModalAsync();
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