using GlobalChat.UI.Menus;
using GlobalChat.UI.VentanasEmergentes;

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
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.PaginaObjetivo));
                if (!((IFlyoutPageController)this).ShouldShowSplitMode)
                    IsPresented = false;
            }
        }

        private void ComprobarSesion()
        {
            if (ServicioPersistencia.SesionIniciada)
            {

            }
            else
            {
                Navigation.PushModalAsync(new VentanaLogin());
            }
        }

        private void FlyoutPage_Loaded(object sender, EventArgs e)
        {
           ComprobarSesion();
        }
    }

}
