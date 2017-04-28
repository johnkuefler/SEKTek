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
    public partial class AddProjectTaskPage : ContentPage
    {
        AddProjectTaskViewModel viewModel;

        public AddProjectTaskPage()
        {
            InitializeComponent();
        }

        public AddProjectTaskPage(AddProjectTaskViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        private async Task CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

    public class AddProjectTaskViewModel : INotifyPropertyChanged
    {
        public string ProjectID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public List<User> Resources { get; set; }

        ProjectTaskRepository projectTaskRepository;

        public ICommand AddProjectTaskCommand { get; set; }

        public AddProjectTaskViewModel()
        {
            projectTaskRepository = new ProjectTaskRepository();

            AddProjectTaskCommand = new Command(async () => await saveProjectTaskAsync());
        }

        private async Task saveProjectTaskAsync()
        {
            ProjectTask projectTask = new ProjectTask
            {
                Name = this.Name,
                Description = this.Description,
                PercentComplete = 0,
                DueDate = DueDate,
                ProjectID = this.ProjectID
            };

            await projectTaskRepository.Add(projectTask);

            App.Current.MainPage = new RootPage();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
