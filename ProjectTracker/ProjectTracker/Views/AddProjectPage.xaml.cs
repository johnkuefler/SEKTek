using ProjectTracker.Models;
using ProjectTracker.Services;
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
    public partial class AddProjectPage : ContentPage
    {
        public AddProjectPage()
        {
            InitializeComponent();
            BindingContext = new AddProjectViewModel();
        }

        private async Task CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new RootPage();
        }
    }

    class AddProjectViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public ICommand SaveProjectCommand => new Command(saveProjectDataAsync);


        private readonly ProjectRepository projectRepository;

        public AddProjectViewModel()
        {
            projectRepository = new ProjectRepository();
        }

        private async void saveProjectDataAsync()
        {
            // validate address

            Project newProject = new Project
            {
                Address1 = this.Address1,
                Address2 = this.Address2,
                City = this.City,
                State = this.State,
                Description = this.Description,
                Name = this.Name,
                Latitude = 0,
                Longitude = 0,
                Zip = this.Zip
            };

            await projectRepository.Add(newProject);

            App.Current.MainPage = new RootPage();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
