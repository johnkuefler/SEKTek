﻿using ProjectTracker;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ProjectTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            GlobalConfig.Initialize();
            SetMainPage();
        }

        public static void SetMainPage()
        {
            Current.MainPage = new RootPage();
        }
    }
}
