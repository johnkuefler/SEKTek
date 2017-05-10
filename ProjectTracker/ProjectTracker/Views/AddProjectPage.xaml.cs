using ProjectTracker.Exceptions;
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
    public partial class AddProjectPage : ContentPage
    {
        private AddProjectViewModel viewModel;
        public AddProjectPage()
        {
            InitializeComponent();
            viewModel = new AddProjectViewModel();
            BindingContext = viewModel;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new RootPage();
        }

        private async Task SaveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string color = colorPicker.Items[colorPicker.SelectedIndex];
                await viewModel.SaveProjectDataAsync(color);
                App.Current.MainPage = new RootPage();
            }
            catch (InvalidAddressException ex)
            {
                await DisplayAlert("Error", "Invalid address entered", "Ok");
            }
        }
    }

    public class AddProjectViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }


        private ProjectRepository projectRepository;

        public AddProjectViewModel()
        {
            projectRepository = new ProjectRepository();
        }

        public async Task SaveProjectDataAsync(string color)
        {
            // validate address

            APIAddressResource selectedAddressResource = null;
            this.Color = color;
            try
            {
                Address newAddress = new Address();
                newAddress.Address1 = this.Address1;
                newAddress.Address2 = this.Address2;
                newAddress.City = this.City;
                newAddress.Province = this.State;
                newAddress.PostalCode = this.Zip;

                APIAddressResource apiAddress = await AddressService.ReturnValidatedAddress(newAddress);
                if (apiAddress.address != null)
                {
                    selectedAddressResource = apiAddress;
                }
                else
                {
                    throw new InvalidAddressException();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidAddressException();
            }

            Project newProject = new Project
            {
                Color = this.Color,
                Address1 = this.Address1,
                Address2 = this.Address2,
                City = this.City,
                State = this.State,
                Description = this.Description,
                Name = this.Name,
                Latitude = 0,
                Longitude = 0,
                Zip = this.Zip
            };
            newProject.Latitude = Convert.ToDecimal(selectedAddressResource.point.coordinates[0]);
            newProject.Longitude = Convert.ToDecimal(selectedAddressResource.point.coordinates[1].ToString());

            await projectRepository.Add(newProject);

        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
