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
    public partial class FindResourcesPage : ContentPage
    {
        FindResourcesViewModel viewModel;

        public FindResourcesPage(string projectID)
        {
            InitializeComponent();
            viewModel = new FindResourcesViewModel(projectID);

            BindingContext = viewModel;
        }

        private async Task searchResultsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            User user = e.SelectedItem as User;
            if (user == null)
                return;

            ProjectRepository projectRepository = new ProjectRepository();
            await projectRepository.AddProjectResource(viewModel.ProjectID, user.Id);

            Project p = await projectRepository.Find(viewModel.ProjectID);
            await Navigation.PushAsync(new ProjectDetailPage(new ProjectDetailViewModel(p)));

            // Manually deselect item
            SearchResultsListView.SelectedItem = null;
        }

        private async Task searchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                await viewModel.SearchAsync(e.NewTextValue);
            }
            else
            {
                viewModel.SearchResults.Clear();
            }
        }
    }

    class FindResourcesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<User> SearchResults { get; set; }
        public string ProjectID { get; set; }

        private UserRepository userRepository;
        private ProjectRepository projectRepository;

        public FindResourcesViewModel(string projectID)
        {
            this.ProjectID = projectID;
            userRepository = new UserRepository();
            projectRepository = new ProjectRepository();

            this.SearchResults = new ObservableCollection<User>();
        }


        // todo: get suggested
        public async Task<List<User>> GetSuggestedResources()
        {
            throw new NotImplementedException();
        }

        public async Task SearchAsync(string searchTerm)
        {
            List<User> usersAlreadyInProject = new List<User>();
            usersAlreadyInProject = await projectRepository.GetProjectResources(ProjectID);

            List<User> searchResults = await userRepository.GetByCriteria(rec => rec.FirstName.ToLower().Contains(searchTerm.ToLower()) || rec.LastName.ToLower().Contains(searchTerm.ToLower()) || rec.Skills.ToLower().Contains(searchTerm.ToLower()));

            this.SearchResults.Clear();
            foreach (User u in searchResults)
            {
                if (!usersAlreadyInProject.Any(x => x.Id == u.Id))
                {
                    this.SearchResults.Add(u);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
