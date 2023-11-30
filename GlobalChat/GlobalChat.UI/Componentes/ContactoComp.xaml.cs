using GlobalChat.Business.Dtos;
using Microsoft.Maui.Controls.Shapes;

namespace GlobalChat.UI.Componentes;

public partial class ContactoComp : ContentView
{
    public ContactoCompletoDto ContactoCompleto { get; set; }

    public ContactoComp(ContactoCompletoDto contactoCompleto)
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
    }
}