using GlobalChat.Business.Dtos;
using GlobalChat.UI.Modelos;
using GlobalChat.UI.VentanasEmergentes;
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
        GridContra.BindingContext = Usuario;
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
                DatosGuardado datosGuardado = new DatosGuardado()
                {
                    SesionIniciada = true,
                    Usuario = petUsu.Value.NombreLogin,
                    Clave = petUsu.Value.Password
                };
                ServicioPersistencia.NuevaSesionIniciada(datosGuardado);
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

    private void EditarContra_Clicked(object sender, EventArgs e)
    {
        if(passVieja.Text == ServicioAPI.Usuario.Password)
        {
            ActualizarContraAsync();
        }
        else
        {
            DisplayAlert("Error", "Error al conectar con el servidor.", "OK");
        }
        BorderContra.IsVisible = false;
    }
    private void MostrarEditarContra_Clicked(object sender, EventArgs e)
    {
        BorderContra.IsVisible = true;
    }

    private async Task ActualizarContraAsync()
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
                DatosGuardado datosGuardado = new DatosGuardado()
                {
                    SesionIniciada = true,
                    Usuario = petUsu.Value.NombreLogin,
                    Clave = petUsu.Value.Password
                };
                ServicioPersistencia.NuevaSesionIniciada(datosGuardado);
                CargarDatos();
                await DisplayAlert("Info", "Datos actualizados correctamente.", "OK");
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

    private async void EliminarCuenta_Clicked(object sender, EventArgs e)
    {
        bool acepto = await DisplayAlert("Eliminar cuenta", "¿Seguro que quieres eliminar tu cuenta?", "Sí", "No");
        if(acepto)
        {
            EliminarCuentaAsync();
        }
    }

    private async Task EliminarCuentaAsync()
    {
        PeticionDto<UsuarioDto> peticionEdi = new PeticionDto<UsuarioDto>() { TokenPeticion = ServicioAPI.TokenUsuario, Value = Usuario };
        HttpResponseMessage respuesta = await ServicioAPI.Cliente.PostAsJsonAsync("api/Usuario/EliminarUsuario", peticionEdi, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        if (respuesta.IsSuccessStatusCode)
        {
            string respuestaStr = await respuesta.Content.ReadAsStringAsync();
            PeticionDto<UsuarioDto> petUsu = JsonSerializer.Deserialize<PeticionDto<UsuarioDto>>(respuestaStr, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new PeticionDto<UsuarioDto>();
            if (petUsu.PeticionCorrecta)
            {
                await Navigation.PushModalAsync(new VentanaLogin());
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

    private void AcercaDe_Clicked(object sender, EventArgs e)
    {
        BorderAcercaDe.IsVisible = true;
    }
}