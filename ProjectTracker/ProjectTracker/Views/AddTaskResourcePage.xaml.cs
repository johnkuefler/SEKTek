using ProjectTracker.Models;
using ProjectTracker.Services;
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

namespace ProjectTracker.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTaskResourcePage : ContentPage
    {
        AddTaskResourceViewModel viewModel;
        public AddTaskResourcePage(string taskID, string projectID)
        {
            InitializeComponent();
            viewModel = new AddTaskResourceViewModel(taskID, projectID);
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.AvailableResources.Count == 0)
                viewModel.LoadAvailableResourcesCommand.Execute(null);
        }

        private async Task AvailableResourcesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            User resource = e.SelectedItem as User;
            if (resource == null)
                return;

            // Manually deselect item
            AvailableResourcesListView.SelectedItem = null;


            await viewModel.AddResource(resource);

            // Load task detail page
            await Navigation.PopAsync();

        }
    }

    class AddTaskResourceViewModel : INotifyPropertyChanged
    {
        public Command LoadAvailableResourcesCommand { get; set; }
        public ObservableCollection<User> AvailableResources { get; set; }
        public string TaskID { get; set; }
        public string ProjectID { get; set; }

        private ProjectRepository  projectRepository;
        private ProjectTaskRepository projectTaskRepository;

        public AddTaskResourceViewModel(string taskID, string projectID)
        {
            this.TaskID = taskID;
            this.ProjectID = projectID;
            projectRepository = new ProjectRepository();
            projectTaskRepository = new ProjectTaskRepository();
            this.AvailableResources = new ObservableCollection<User>();
            LoadAvailableResourcesCommand = new Command(async () => await loadAvailableResourcesAsync());
        }


        public async Task AddResource(User u)
        {
            await projectTaskRepository.AddTaskResource(TaskID, u.Id);
        }

        private async Task loadAvailableResourcesAsync()
        {
            this.AvailableResources.Clear();

            List<User> usersAssignedToTask = await projectTaskRepository.GetAssignedResources(this.TaskID);

            List<User> resources = await projectRepository.GetProjectResources(ProjectID);
            foreach (User resource in resources)
            {
                if (!usersAssignedToTask.Any(x => x.Id == resource.Id))
                {
                    this.AvailableResources.Add(resource);
                }
            }
        }
 
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
