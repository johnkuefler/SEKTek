using ProjectTracker.Helpers;
using ProjectTracker.Models;
using ProjectTracker.Services;

using Xamarin.Forms;

namespace ProjectTracker.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
    }
}

