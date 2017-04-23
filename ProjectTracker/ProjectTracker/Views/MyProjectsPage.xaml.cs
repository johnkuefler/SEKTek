using ProjectTracker.Helpers;
using ProjectTracker.Models;
using ProjectTracker.Services;
using ProjectTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

    public partial class MyProjectsPage : ContentPage
    {
        MyProjectsViewModel viewModel;

        public MyProjectsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MyProjectsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            Project project = args.SelectedItem as Project;
            if (project == null)
                return;

            await Navigation.PushAsync(new ProjectDetailPage(new ProjectDetailViewModel(project)));

            // Manually deselect item
            ProjectsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Projects.Count == 0)
                viewModel.LoadProjectsCommand.Execute(null);
        }


    }

    class MyProjectsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<Project> Projects { get; set; }
        public Command LoadProjectsCommand { get; set; }

        private ProjectRepository projectRepository;
        public MyProjectsViewModel()
        {
            this.Projects = new ObservableCollection<Project>();

            projectRepository = new ProjectRepository();

            LoadProjectsCommand = new Command(async () => await loadProjectsAsync());
        }

        private async Task loadProjectsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Projects.Clear();
                IEnumerable<Project> items = await projectRepository.GetByCriteria(rec => true);
                foreach (Project item in items)
                {
                    Projects.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
