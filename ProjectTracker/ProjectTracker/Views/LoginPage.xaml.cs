using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectTracker.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (usernameEntry.Text.ToLower() == "jackson")
            {
                GlobalConfig.CurrentUser = new User
                {
                    Id = "C45981AF-3EC5-42EA-98B4-F540C5B5C4AA",
                    FirstName = "Jackson",
                    LastName = "Smith",
                    EmailAddress = "jsmith@sektek.com",
                    Password = "demo",
                    PictureURL = "https://jsekdevstorage.blob.core.windows.net/project/user1.jpg",
                    Title = "Project Manager",
                    Skills = "Projects,Management,Teams",
                };
            }
            else
            {
                GlobalConfig.HomeScreenProjects = new List<Project>();

                GlobalConfig.CurrentUser = new User
                {
                    Id = "E11BD95C-D2F5-4086-B2C8-97882FEC5870",
                    FirstName = "Susan",
                    LastName = "Bell",
                    EmailAddress = "sbell@sektek.com",
                    Password = "demo",
                    PictureURL = "https://jsekdevstorage.blob.core.windows.net/project/user2.jpg",
                    Title = "Field Technician",
                    Skills = "Fiber, Programming, Network"
                };
            }
            App.Current.MainPage = new RootPage();
        }
    }

    class LoginViewModel : INotifyPropertyChanged
    {

        public LoginViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
