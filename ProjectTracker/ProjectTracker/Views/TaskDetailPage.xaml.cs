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

        public TaskDetailPage(ProjectTask projectTask, string projectColor)
        {
            viewModel = new TaskDetailViewModel(projectTask, projectColor);

            InitializeComponent();
            BindingContext = viewModel;
        }

        private void CommentsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (GlobalConfig.CurrentUser.GetRole() != "Admin")
            {
                addResourceButton.IsVisible = false;
            }


            viewModel.LoadProjectCommentsCommand.Execute(null);

            List<User> resources = await viewModel.TaskRepository.GetAssignedResources(viewModel.TaskID);

            ResourcesStackLayout.Children.Clear();

            foreach (User resource in resources)
            {
                ResourcesStackLayout.Children.Add(new CircleImage { Source = resource.PictureURL, HeightRequest = 50, WidthRequest = 50 });
            }

            PercentPicker.SelectedIndex = PercentPicker.Items.IndexOf(viewModel.PercentComplete + "%");
        }

        private async Task AddResource_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTaskResourcePage(viewModel.TaskID, viewModel.ProjectID));
        }

        private async Task UpdatePercent_Clicked(object sender, EventArgs e)
        {
            decimal percent = Convert.ToDecimal(((string)PercentPicker.SelectedItem).Replace("%", ""));



            await viewModel.UpdatePercentComplete(percent);

            //ProjectTaskRepository temp = new ProjectTaskRepository();
            //ProjectTask task = await temp.Find(viewModel.TaskID);
            //task.PercentComplete = percent;

            viewModel.PercentComplete = percent;

            this.PercentCompleteLabel.Text = viewModel.PercentCompleteDisplay;
        }


        private async Task AddComment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCommentPage(viewModel.TaskID));
        }
    }

    public class TaskDetailViewModel : INotifyPropertyChanged
    {
        public ProjectTaskRepository TaskRepository;
        public string ProjectID { get; set; }
        public string TaskID { get; set; }
        public string ProjectColor { get; set; }
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


        public ObservableCollection<CommentListItemModel> Comments { get; set; }
        public Command LoadProjectCommentsCommand { get; set; }

        public TaskDetailViewModel(ProjectTask task, string projectColor)
        {
            TaskRepository = new ProjectTaskRepository();
            this.Name = task.Name;
            this.PercentComplete = task.PercentComplete;
            this.Description = task.Description;
            this.DueDate = task.DueDate;
            this.TaskID = task.Id;
            this.ProjectID = task.ProjectID;
            this.ProjectColor = projectColor;
            this.Comments = new ObservableCollection<CommentListItemModel>();
            LoadProjectCommentsCommand = new Command(async () => await loadProjectCommentsAsync());
        }


        public async Task UpdatePercentComplete(decimal newPercentComplete)
        {
            ProjectTask task = await TaskRepository.Find(this.TaskID);
            decimal oldPercent = task.PercentComplete;
            this.PercentComplete = newPercentComplete;
            task.PercentComplete = newPercentComplete;

           //await TaskRepository.Update(task);

            await TaskRepository.AddTaskComment(GlobalConfig.CurrentUser.Id, this.TaskID, "Updated from " + oldPercent + "% to " + newPercentComplete + "% complete");

            LoadProjectCommentsCommand.Execute(null);
        }

        private async Task loadProjectCommentsAsync()
        {
            this.Comments.Clear();
            IEnumerable<ProjectTaskComment> comments = await TaskRepository.GetComments(this.TaskID);
            foreach (ProjectTaskComment comment in comments.OrderByDescending(x => x.DateTime))
            {
                this.Comments.Add(new CommentListItemModel
                {
                    Comment = comment.Comment,
                    PictureURL = comment.GetUser().PictureURL,
                    DateDisplay = comment.DateTime.ToString()
                });
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class CommentListItemModel
    {
        public string PictureURL { get; set; }
        public string Comment { get; set; }
        public string DateDisplay { get; set; }
    }
}
