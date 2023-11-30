namespace GlobalChat.UI.VentanasEmergentes;

public partial class VentanaLogin : ContentPage
{
	public VentanaLogin()
	{
		InitializeComponent();
	}

    private void IniciarSesion_Clicked(object sender, EventArgs e)
    {
		Navigation.PopModalAsync();
    }

    private void Registar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new VentanaRegistro());
    }
}