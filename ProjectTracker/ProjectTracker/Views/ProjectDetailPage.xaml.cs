using ImageCircle.Forms.Plugin.Abstractions;
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
    public partial class ProjectDetailPage : ContentPage
    {
        ProjectDetailViewModel viewModel;

        public ProjectDetailPage()
        {
            InitializeComponent();
        }

        public ProjectDetailPage(ProjectDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Tasks.Count == 0)
                viewModel.LoadProjectTasksCommand.Execute(null);

            //if (viewModel.Tasks.Count == 0)
            //    viewModel.LoadProjectResourcesCommand.Execute(null);


            ProjectRepository projectRepository = new ProjectRepository();
            List<User> resources = await projectRepository.GetProjectResources(viewModel.ProjectID);

            ResourcesStackLayout.Children.Clear();

            foreach (User resource in resources)
            {
                ResourcesStackLayout.Children.Add(new CircleImage { Source = resource.PictureURL, HeightRequest = 50, WidthRequest = 50 });
            }
        }

        async void OnProjectTaskItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            ProjectTask projectTask = args.SelectedItem as ProjectTask;
            if (projectTask == null)
                return;

            // Manually deselect item
            ProjectTasksListView.SelectedItem = null;


            // Load task detail page
            await Navigation.PushAsync(new TaskDetailPage(projectTask));
        }

        async void OnProjectResourceItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            User resource = args.SelectedItem as User;
            if (resource == null)
                return;

            // Manually deselect item
            ProjectTasksListView.SelectedItem = null;


            // Load users task page
        }

        private async Task AddTaskButton_Clicked(object sender, EventArgs e)
        {
            AddProjectTaskViewModel addTaskViewModel = new AddProjectTaskViewModel();
            addTaskViewModel.ProjectID = viewModel.ProjectID;
            await Navigation.PushAsync(new AddProjectTaskPage(addTaskViewModel));
        }

        private async Task AddResourceButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FindResourcesPage(viewModel.ProjectID));
        }
    }

    public class ProjectDetailViewModel : INotifyPropertyChanged
    {
        public string ProjectID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PercentCompleted { get; set; }
        public string Color { get; set; }

        public ObservableCollection<ProjectTask> Tasks { get; set; }
        //public ObservableCollection<User> Resources { get; set; }

        ProjectRepository projectRepository;
        ProjectTaskRepository projectTaskRepository;
        public Command LoadProjectTasksCommand { get; set; }
        // public Command LoadProjectResourcesCommand { get; set; }
        public ProjectDetailViewModel(Project project)
        {
            projectTaskRepository = new ProjectTaskRepository();
            projectRepository = new ProjectRepository();

            this.ProjectID = project.Id;
            this.Name = project.Name;
            this.Color = project.Color;
            this.Description = project.Description;
            this.Tasks = new ObservableCollection<ProjectTask>();
            // this.Resources = new ObservableCollection<User>();
            this.PercentCompleted = project.GetCompletionPercentage();

            LoadProjectTasksCommand = new Command(async () => await loadProjectTasksAsync());
            // LoadProjectResourcesCommand = new Command(async () => await loadProjectResourcesAsync());
        }

        private async Task loadProjectTasksAsync()
        {
            Tasks.Clear();
            IEnumerable<ProjectTask> items = await projectTaskRepository.GetByCriteria(rec => rec.ProjectID == this.ProjectID);

            foreach (ProjectTask item in items)
            {
                Tasks.Add(item);
            }
        }

        //private async Task loadProjectResourcesAsync()
        //{
        //    Resources.Clear();
        //    List<User> resources = await projectRepository.GetProjectResources(this.ProjectID);
        //    foreach (User item in resources)
        //    {
        //        Resources.Add(item);
        //    }
        //}


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
