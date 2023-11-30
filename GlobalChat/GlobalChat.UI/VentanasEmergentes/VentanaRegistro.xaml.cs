using GlobalChat.Business.Dtos;
using System.Net.Http.Json;
using System.Text.Json;

namespace GlobalChat.UI.VentanasEmergentes;


public partial class VentanaRegistro : ContentPage
{
    public UsuarioDto NuevoUsuario { get; set; } = new UsuarioDto();
	public VentanaRegistro()
	{
        InitializeComponent();
        GridDatos.BindingContext = NuevoUsuario;
	}

    private void CrearCuenta_Clicked(object sender, EventArgs e)
    {
        bool correcto = true;
        if (string.IsNullOrEmpty(NuevoUsuario.Nombre))
        {
            correcto = false;
            DisplayAlert("Error", "Tienes que rellenar el nombre!", "OK");
        }
        if (string.IsNullOrEmpty(NuevoUsuario.NombreLogin))
        {
            correcto = false;
            DisplayAlert("Error", "Tienes que rellenar el nombre de login!", "OK");
        }
        if (string.IsNullOrEmpty(NuevoUsuario.Password))
        {
            correcto = false;
            DisplayAlert("Error", "Tienes que rellenar la contraseña!", "OK");
        }
        if (correcto)
        {
            Task.Run(CrearUsuarioAsync);
        }
    }

    private async Task CrearUsuarioAsync()
    {
        HttpResponseMessage respuesta = await ServicioAPI.Cliente.PostAsJsonAsync("api/Usuario/RegistrarUsuario", NuevoUsuario, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        if (respuesta.IsSuccessStatusCode)
        {
            string respuestaStr = await respuesta.Content.ReadAsStringAsync();
            PeticionDto<UsuarioDto> petUsu = JsonSerializer.Deserialize<PeticionDto<UsuarioDto>>(respuestaStr, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new PeticionDto<UsuarioDto>();
            if (petUsu.PeticionCorrecta)
            {
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

    private void IniciaSesion_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }
}