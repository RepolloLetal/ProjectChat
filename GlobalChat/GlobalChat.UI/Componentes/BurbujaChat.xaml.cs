using GlobalChat.Business.Dtos;

namespace GlobalChat.UI.Componentes;

public partial class BurbujaChat : ContentView
{
    public MensajeDto Mensaje { get; set; } = new MensajeDto();
    public BurbujaChat(MensajeDto men, bool derecha = false)
	{
		InitializeComponent();
        Mensaje = men;
        DatosGridBurbuja.BindingContext = Mensaje;
        if (derecha)
        {
            HorizontalOptions = LayoutOptions.End;
            LblMen.HorizontalOptions = LayoutOptions.Start;
            BorderMensaje.BackgroundColor = Color.FromArgb("#E5C0E9");
        }
    }
}