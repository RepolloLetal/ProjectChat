using GlobalChat.Business.Dtos;
using Microsoft.Maui.Controls.Shapes;

namespace GlobalChat.UI.Componentes;

public partial class ContactoComp : ContentView
{
    public ContactoCompletoDto ContactoCompleto { get; set; } = new ContactoCompletoDto();

    public ContactoComp()
	{
		InitializeComponent();
        DatosGridComp.BindingContext = ContactoCompleto;
    }
}