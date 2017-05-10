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
    public partial class AddCommentPage : ContentPage
    {
        private AddCommentViewModel viewModel;
        public AddCommentPage(string taskID)
        {
            InitializeComponent();
            viewModel = new AddCommentViewModel(taskID);
            BindingContext = viewModel;
        }

        public async Task AddComment_Clicked(object sender, EventArgs e)
        {
            await viewModel.AddComment(commentEditor.Text);
            await Navigation.PopAsync();
        }
    }

    class AddCommentViewModel : INotifyPropertyChanged
    {
        private ProjectTaskRepository taskRepository;
        public string TaskID { get; set; }

        public AddCommentViewModel(string taskID)
        {
            this.TaskID = taskID;
            taskRepository = new ProjectTaskRepository();
        }

        public async Task AddComment(string commentText)
        {
            await taskRepository.AddTaskComment(GlobalConfig.CurrentUser.Id, TaskID, commentText);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
