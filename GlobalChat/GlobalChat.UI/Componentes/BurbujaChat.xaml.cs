using GlobalChat.Business.Dtos;

namespace GlobalChat.UI.Componentes;

public partial class BurbujaChat : ContentView
{
    public MensajeDto BurbujaMensaje { get; set; } = new MensajeDto();
    public BurbujaChat()
	{
		InitializeComponent();
        DatosGridBurbuja.BindingContext = BurbujaMensaje;
    }
}