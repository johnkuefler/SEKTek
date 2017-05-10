using ProjectTracker;
using ProjectTracker.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ProjectTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            GlobalConfig.Initialize();

            SetMainPage();
        }

        public static void SetMainPage()
        {
            if (GlobalConfig.CurrentUser == null)
            {
                Current.MainPage = new LoginPage();
            }
            else
            {
                Current.MainPage = new RootPage();
            }
        }
    }
}
