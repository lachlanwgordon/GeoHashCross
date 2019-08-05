﻿using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter.Analytics;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;

namespace GeohashCross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public async void MyPositionClicked(object sender, EventArgs e)
        {
            try
            {

                var pos = VM.CurrentLocation;

                var update = CameraUpdateFactory.NewPosition(new Position(pos.Latitude, pos.Longitude));

                await TheMap.AnimateCamera(update, TimeSpan.FromMilliseconds(200));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public HomePage()
        {
            InitializeComponent();
        }

        public async Task Init()
        {

            try
            {
                TheMap.UiSettings.MyLocationButtonEnabled = false;//Don't show the my location button because I've implemented my own

                var granted = await GetPermissions();
                if (!granted)
                    return;
                await SetupLocations();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex}\n{ex.StackTrace}");
                Crashes.TrackError(ex);
                await Shell.Current.DisplayAlert("Error", "An error has occured, probably because the hash isn't available yet.", "Okay");
            }
        }

        private async Task SetupLocations()
        {
            var currentLoc = await VM.UpdateCurrentLocation();
            if (currentLoc == null)
            {
                await DisplayAlert("Error", "Could not get current location. Please tap a location on the map", "Okay");
                return;
            }
            TheMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(currentLoc.Data.Latitude, currentLoc.Data.Longitude), new Distance(50)));

            var hashLoc = await VM.LoadHashLocation();
            if (hashLoc == null)
            {
                await DisplayAlert("Error", "Could not load DJIA for today, please check internet connection", "Okay");
                return;
            }
            var hashPos = new Position(hashLoc.Latitude, hashLoc.Longitude);
            var myPos = new Position(currentLoc.Data.Latitude, currentLoc.Data.Longitude);
            var bounds = new Bounds(myPos, hashPos);
            var update = CameraUpdateFactory.NewBounds(bounds, 50);
            await TheMap.AnimateCamera(update);
        }

        private async Task<bool> GetPermissions()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    //await DisplayAlert("Allow access to location", "GeohashCross works much better with ", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                if (results.ContainsKey(Permission.Location))
                    status = results[Permission.Location];
            }

            if (status == PermissionStatus.Granted)
            {
                VM.LocationPermissionGranted = true;
                return true;
            }

            else if (status != PermissionStatus.Unknown)
            {
                await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
            }
            return false;
        }



        async void TheMap_InfoWindowClicked(object sender, InfoWindowClickedEventArgs e)
        {

            if (Device.RuntimePlatform == Device.iOS)
            {
                var maps = await Shell.Current.DisplayActionSheet("Open in maps", "cancel", null, "Google Maps", "Apple Maps");//In current preview of Shell you must call actionsheets and alerts like this.
                if (maps == "Google Maps")
                {
                    var uri = new Uri($"https://google.com/maps/place/{e.Pin.Position.Latitude},{e.Pin.Position.Longitude}");
                    Device.OpenUri(uri);

                }
                else if (maps == "Apple Maps")
                {
                    await Xamarin.Essentials.Map.OpenAsync(e.Pin.Position.Latitude, e.Pin.Position.Longitude);
                }
            }

            else
            {
                await Xamarin.Essentials.Map.OpenAsync(e.Pin.Position.Latitude, e.Pin.Position.Longitude);
            }

        }


        private async void YouMadeItClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await DisplayActionSheet("Congratulations\nWould you like to take a photo?", "Cancel", null, "Screen shot", "Photo", "Both");
            }
            catch (Exception ex)
            {

                Crashes.TrackError(ex);
            }
        }

        


        bool initialised;
        bool FirstUse
        {
            get => Xamarin.Essentials.Preferences.Get(Keys.FirstUse, true);
            set => Xamarin.Essentials.Preferences.Set(Keys.FirstUse, value);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();


            Analytics.TrackEvent(AnalyticsManager.PageOpened, new Dictionary<string, string>
            {
                {"Page", GetType().Name}
            });

            if (FirstUse)
            {
                await Shell.Current.DisplayAlert("Onboarding Time", "how to", "Okay");
                FirstUse = false;
            }

            if (!initialised)
            {
                await Init();
                initialised = true;

            }

        }

        async void DateChanged(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            try
            {
                await VM.ChangeDate();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }



    }
}
