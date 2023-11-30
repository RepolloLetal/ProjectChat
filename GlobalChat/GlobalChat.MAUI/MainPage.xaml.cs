using GlobalChat.MAUI.Menus;

namespace GlobalChat.MAUI
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
    }
}