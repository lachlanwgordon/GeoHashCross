using GeohashCross.Resources;
using GeohashCross.Services;
using GeohashCross.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace GeohashCross
{
	public partial class App : Application
	{
		public App ()
		{
            InitializeComponent();
            MainPage = new AppShell();
		}

		protected override async void OnStart ()
		{
            // Handle when your app starts
            await AnalyticsManager.Initialize();
            await DB.Initialize();
            if (!APIKeys.MapsKeyInitialized)
            {
                await Shell.Current.DisplayAlert("Maps API Key", "Please add a google maps API Key to APIKeys.cs", "Okay");
            }
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
