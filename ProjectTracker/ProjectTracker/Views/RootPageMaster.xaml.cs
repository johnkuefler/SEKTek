using ProjectTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPageMaster : ContentPage
    {
        public ListView ListView => ListViewMenuItems;

        public RootPageMaster()
        {
            InitializeComponent();
            BindingContext = new RootPageMasterViewModel();
        }



        class RootPageMasterViewModel : INotifyPropertyChanged
        {
            public string UserFullName { get; set; }
            public string UserEmail { get; set; }
            public string UserImageURL { get; set; }

            public ObservableCollection<RootPageMenuItem> MenuItems { get; }
            public RootPageMasterViewModel()
            {
                UserFullName = GlobalConfig.CurrentUser.FullName;
                UserEmail = GlobalConfig.CurrentUser.EmailAddress;
                UserImageURL = GlobalConfig.CurrentUser.PictureURL;

                MenuItems = new ObservableCollection<RootPageMenuItem>(new[]
                {
                    new RootPageMenuItem { Title = "My Projects", TargetType = typeof(MyProjectsPage) },
                    new RootPageMenuItem { Title = "Project Map", TargetType = typeof(ProjectsMapPage) },
                    new RootPageMenuItem { Title = "Admin", TargetType = typeof(AdminPage) },
                    new RootPageMenuItem { Title = "Sign Out", TargetType = typeof(SignOutPage) },
                });
            }
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }
    }
}
