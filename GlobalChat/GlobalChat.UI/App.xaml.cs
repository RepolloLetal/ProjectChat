namespace GlobalChat.UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            NavigationPage main = new NavigationPage(new MainPage());
            NavigationPage.SetTitleView(main, null);
            NavigationPage.SetHasNavigationBar(main, false);
            NavigationPage.SetHasBackButton(main, false);
            MainPage = main;
        }
    }
}
