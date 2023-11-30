using GlobalChat.Business.Dtos;
using GlobalChat.UI.Menus;
using GlobalChat.UI.Modelos;
using GlobalChat.UI.VentanasEmergentes;
using System.Net.Http.Json;
using System.Text.Json;

namespace GlobalChat.UI
{
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
            flyoutMenu.elementosMenu.SelectionChanged += OnSelectionChanged;
        }

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault() as ElementoMenu;
            if (item != null)
            {
                Detail = (Page)Activator.CreateInstance(item.PaginaObjetivo);
            }
        }

        private async void FlyoutPage_Loaded(object sender, EventArgs e)
        {
            await ServicioAPI.ReLogin(Navigation);
        }
    }
}
