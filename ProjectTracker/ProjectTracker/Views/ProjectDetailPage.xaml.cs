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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Tasks.Count == 0)
                viewModel.LoadProjectTasksCommand.Execute(null);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            Project project = args.SelectedItem as Project;
            if (project == null)
                return;

            // Manually deselect item
            ProjectsListView.SelectedItem = null;
        }

        private async Task AddTaskButton_Clicked(object sender, EventArgs e)
        {
            AddProjectTaskViewModel addTaskViewModel = new AddProjectTaskViewModel();
            addTaskViewModel.ProjectID = viewModel.ProjectID;
            await Navigation.PushAsync(new AddProjectTaskPage(addTaskViewModel));
        }
    }

    public class ProjectDetailViewModel : INotifyPropertyChanged
    {
        public string ProjectID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PercentCompleted { get; set; }

        public ObservableCollection<ProjectTask> Tasks { get; set; }

        ProjectRepository projectRepository;
        ProjectTaskRepository projectTaskRepository;
        public Command LoadProjectTasksCommand { get; set; }

        public ProjectDetailViewModel(Project project)
        {
            projectTaskRepository = new ProjectTaskRepository();

            this.ProjectID = project.Id;
            this.Name = project.Name;
            this.Description = project.Description;
            this.Tasks = new ObservableCollection<ProjectTask>();

            LoadProjectTasksCommand = new Command(async () => await loadProjectTasksAsync());
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


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
