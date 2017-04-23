namespace ProjectTracker.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            LoadApplication(new ProjectTracker.App());
        }
    }
}