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
    public partial class TaskDetailPage : ContentPage
    {
        TaskDetailViewModel viewModel;

        public TaskDetailPage(ProjectTask projectTask)
        {
            viewModel = new TaskDetailViewModel(projectTask);

            InitializeComponent();
            BindingContext = new TaskDetailViewModel(projectTask);
        }

        private void CommentsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Comments.Count == 0)
            {
                viewModel.LoadProjectCommentsCommand.Execute(null);
            }

            List<User> resources = await viewModel.TaskRepository.GetAssignedResources(viewModel.TaskID);

            ResourcesStackLayout.Children.Clear();

            foreach (User resource in resources)
            {
                ResourcesStackLayout.Children.Add(new CircleImage { Source = resource.PictureURL, HeightRequest = 50, WidthRequest = 50 });
            }
        }

        private async Task AddResource_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTaskResourcePage(viewModel.TaskID, viewModel.ProjectID));
        }

        private async Task UpdatePercentButton_Clicked(object sender, EventArgs e)
        {

        }
    }

    class TaskDetailViewModel : INotifyPropertyChanged
    {
        public ProjectTaskRepository TaskRepository;
        public string ProjectID { get; set; }
        public string TaskID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PercentComplete { get; set; }
        public string PercentCompleteDisplay
        {
            get
            {
                return "Percent completed: " + PercentComplete + "%";
            }
        }
        public DateTime DueDate { get; set; }
        public string DueDateDisplay
        {
            get
            {
                return "Date due: " + DueDate.ToString("yyyy-MM-dd");
            }
        }


        public ObservableCollection<ProjectTaskComment> Comments { get; set; }
        public Command LoadProjectCommentsCommand { get; set; }

        public TaskDetailViewModel(ProjectTask task)
        {
            TaskRepository = new ProjectTaskRepository();
            this.Name = task.Name;
            this.PercentComplete = task.PercentComplete;
            this.Description = task.Description;
            this.DueDate = task.DueDate;
            this.TaskID = task.Id;
            this.ProjectID = task.ProjectID;
            this.Comments = new ObservableCollection<ProjectTaskComment>();
            LoadProjectCommentsCommand = new Command(async () => await loadProjectCommentsAsync());
        }

        private async Task loadProjectCommentsAsync()
        {
            this.Comments = new ObservableCollection<ProjectTaskComment>(await TaskRepository.GetComments(TaskID));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
