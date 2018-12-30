using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeohashCross.Services;
using GeohashCross.ViewModels;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeohashCross.Views
{
    public partial class TestsPage : ContentPage
    {
        HomePageViewModel VM = new HomePageViewModel();
        public TestsPage()
        {
            BindingContext = VM;
            InitializeComponent();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var granted = await GetPermissions();
                if (!granted)
                    return;
                await SetupLocations();
            });
        }

        private async Task SetupLocations()
        {
            await VM.UpdateCurrentLocation();
        }

        void UseNorthPole(object sender, System.EventArgs e)
        {
            EndLat.Text = VM.TrueNorthPole.ToString();
            EndLon.Text = VM.TrueNorthPole.Longitude.ToString();
            Log.Text += $"\nSet end location to north pole: ({VM.TrueNorthPole.Latitude},{VM.TrueNorthPole.Longitude})";
        }


        void Calculate(object sender, System.EventArgs e)
        {
            Log.Text += "\nAttempting to calculate";

            var startLat = Convert.ToDouble(StartLat.Text);
            var startLon = Convert.ToDouble(StartLong.Text);
            var startLoc = new Location(startLat, startLon);

            var endLat = Convert.ToDouble(EndLat.Text);
            var endLon = Convert.ToDouble(EndLon.Text);
            var endLoc = new Location(endLat, endLon);

            var bearing = VM.GetBearing(startLoc, endLoc);
            Bearing.Text = bearing.ToString();
            Log.Text += $"\ncalculate bearing as {bearing}";
        }

        void UseCurrent(object sender, System.EventArgs e)
        {
            var loc = VM.CurrentLocation;
            StartLat.Text = loc.Latitude.ToString();
            StartLong.Text = loc.Longitude.ToString();
            Log.Text += $"\nUsing real current location: ({loc.Latitude}, {loc.Longitude})";


        }

        private async Task<bool> GetPermissions()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await DisplayAlert("Need location", "GeohashCross needs your location to find the nearest hash", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                if (results.ContainsKey(Permission.Location))
                    status = results[Permission.Location];
            }

            if (status == PermissionStatus.Granted)
            {
                return true;
            }

            else if (status != PermissionStatus.Unknown)
            {
                await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
            }
            return false;
        }

        void CrashClicked(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Analytics.TrackEvent(AnalyticsManager.PageOpened, new Dictionary<string, string>
            {
                {"Page", GetType().Name}
            });
        }

        void ErrorClicked(object sender, System.EventArgs e)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}
