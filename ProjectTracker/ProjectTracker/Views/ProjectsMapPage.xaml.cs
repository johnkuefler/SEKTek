using ProjectTracker.Models;
using ProjectTracker.Services;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ProjectTracker.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectsMapPage : ContentPage
    {
        public ProjectsMapPage()
        {
            InitializeComponent();
        }


        protected override async void OnAppearing()
        {
            // middle of Pittsburg
            //37.413079, -94.7048571

            if (GlobalConfig.MapProject == null)
            {
                projectMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(37.413079, -94.7048571), Distance.FromMiles(0.5)));

                ProjectRepository repository = new ProjectRepository();
                IEnumerable<Project> projects = await repository.GetByCriteria(x => true);

                foreach (Project p in projects)
                {
                    projectMap.Pins.Add(new Pin
                    {
                        Label = p.Name + " - " + p.Description,
                        Type = PinType.SavedPin,
                        Address = p.Address1,
                        Position = new Position((double)p.Latitude, (double)p.Longitude)
                    });
                }
            }
            else
            {
                projectMap.MoveToRegion(
                  MapSpan.FromCenterAndRadius(
                      new Position((double)GlobalConfig.MapProject.Latitude, (double)GlobalConfig.MapProject.Longitude), Distance.FromMiles(0.3)));


                projectMap.Pins.Add(new Pin
                {
                    Label = GlobalConfig.MapProject.Name + " - " + GlobalConfig.MapProject.Description,
                    Type = PinType.SavedPin,
                    Address = GlobalConfig.MapProject.Address1,
                    Position = new Position((double)GlobalConfig.MapProject.Latitude, (double)GlobalConfig.MapProject.Longitude)
                });

                GlobalConfig.MapProject = null;
            }
        }
    }
}
