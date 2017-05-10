using ProjectTracker.Models;
using ProjectTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#if __ANDROID__
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
#endif 

namespace ProjectTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPageDetail : ContentPage
    {
        RootPageDetailViewModel viewModel;
        public RootPageDetail()
        {
            InitializeComponent();
            viewModel = new RootPageDetailViewModel();
            BindingContext = viewModel;
        }

        private async void FavoritesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Project project = e.SelectedItem as Project;
            if (project == null)
                return;

            await Navigation.PushAsync(new ProjectDetailPage(new ProjectDetailViewModel(project)));

            // Manually deselect item
            FavoritesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            if (GlobalConfig.HomeScreenProjects.Count > 0)
            {
                noFavoritesStack.IsVisible = false;
            }

#if __ANDROID__
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                    status = results[Permission.Location];
                }
#endif


            if (viewModel.FavoriteProjects.Count == 0)
                viewModel.LoadFavoritesCommand.Execute(null);

        }
    }

    class RootPageDetailViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Project> FavoriteProjects { get; set; }
        public ICommand LoadFavoritesCommand => new Command(loadFavoriteProjectsAsync);

        public RootPageDetailViewModel()
        {
            FavoriteProjects = new ObservableCollection<Project>();
        }

        private async void loadFavoriteProjectsAsync()
        {
            foreach (Project p in GlobalConfig.HomeScreenProjects)
            {
                await p.GetCompletionPercentage();
                this.FavoriteProjects.Add(p);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
