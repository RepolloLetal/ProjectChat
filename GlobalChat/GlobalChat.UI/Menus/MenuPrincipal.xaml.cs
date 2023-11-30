namespace GlobalChat.UI.Menus;

public partial class MenuPrincipal : ContentPage
{
	public MenuPrincipal()
	{
		InitializeComponent();
        ServicioAPI.UsuarioIniSesion += ServicioAPI_UsuarioIniSesion;
    }

    private void ServicioAPI_UsuarioIniSesion(object? sender, EventArgs e)
    {
        LabelNombre.Text = ServicioAPI.Usuario.Nombre;
        //TODO: Posible carga de imagen
    }
}