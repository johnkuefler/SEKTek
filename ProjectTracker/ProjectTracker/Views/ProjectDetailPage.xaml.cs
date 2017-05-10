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

            if (GlobalConfig.CurrentUser.GetRole() != "Admin")
            {
                addResourceBtn.IsVisible = false;
                addTaskBtn.IsVisible = false;
            }

            await viewModel.LoadProjectTasksAsync();

            ProjectRepository projectRepository = new ProjectRepository();
            List<User> resources = await projectRepository.GetProjectResources(viewModel.ProjectID);

            ResourcesStackLayout.Children.Clear();

            foreach (User resource in resources)
            {
                ResourcesStackLayout.Children.Add(new CircleImage { Source = resource.PictureURL, HeightRequest = 50, WidthRequest = 50 });
            }

            this.percentCompletedLabel.Text = viewModel.PercentCompleted;
        }

        async void OnProjectTaskItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            ProjectTask projectTask = args.SelectedItem as ProjectTask;
            if (projectTask == null)
                return;

            // Manually deselect item
            ProjectTasksListView.SelectedItem = null;


            // Load task detail page
            await Navigation.PushAsync(new TaskDetailPage(projectTask, viewModel.Color));
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

        private void FavoriteIcon_Tapped(object sender, EventArgs e)
        {
            if (!GlobalConfig.HomeScreenProjects.Any(x => x.Id == viewModel.Project.Id))
            {
                GlobalConfig.HomeScreenProjects.Add(viewModel.Project);
            }

            DisplayAlert("Favorite Added", "Project added to favorites", "Ok");
        }

        private async void MapIcon_Tapped(object sender, EventArgs e)
        {
            GlobalConfig.MapProject = viewModel.Project;
            await Navigation.PushAsync(new ProjectsMapPage());
        }
    }

    public class ProjectDetailViewModel : INotifyPropertyChanged
    {
        public Project Project;
        public string ProjectID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PercentCompleted { get; set; }
        public string Color { get; set; }

        public ObservableCollection<ProjectTask> Tasks { get; set; }

        ProjectRepository projectRepository;
        ProjectTaskRepository projectTaskRepository;
        public Command LoadProjectTasksCommand { get; set; }
        public ProjectDetailViewModel(Project project)
        {
            this.Project = project;
            projectTaskRepository = new ProjectTaskRepository();
            projectRepository = new ProjectRepository();

            this.ProjectID = project.Id;
            this.Name = project.Name;
            this.Color = project.Color;
            this.Description = project.Description;
            this.Tasks = new ObservableCollection<ProjectTask>();
            LoadProjectTasksCommand = new Command(async () => await LoadProjectTasksAsync());
        }

        public async Task LoadProjectTasksAsync()
        {
            Tasks.Clear();
            IEnumerable<ProjectTask> items = await projectTaskRepository.GetByCriteria(rec => rec.ProjectID == this.ProjectID);

            List<ProjectTaskAssignment> assignments = await projectTaskRepository.GetTaskAssignmentsForUser(GlobalConfig.CurrentUser.Id);

            foreach (ProjectTask item in items.OrderBy(x => x.DueDate))
            {
                if (GlobalConfig.CurrentUser.GetRole() == "Admin")
                {
                    Tasks.Add(item);
                }
                else
                {
                    if (assignments.Any(x=>x.ProjectTaskID == item.Id))
                    {
                        Tasks.Add(item);
                    }
                }
            }

            await Project.GetCompletionPercentage();
            this.PercentCompleted = Project.PercentComplete;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
