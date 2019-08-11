using Plugin.Permissions;
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
using GeohashCross.Models;
using Location = GeohashCross.Models.Location;
using LocationExtensions = GeohashCross.Models.LocationExtensions;
using GeohashCross.Converters;

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
            TheMap.UiSettings.MyLocationButtonEnabled = false;//Don't show the my location button because I've implemented my own
            TheMap.UiSettings.ZoomControlsEnabled = false;
        }



        private async Task<bool> GetPermissions()
        {
            Debug.WriteLine("Gettings permission 1");
            var success = await Device.InvokeOnMainThreadAsync(async () =>
            {
                Debug.WriteLine("Gettings permission 2");

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
                    Debug.WriteLine("Gettings permission 3");
                    return true;
                }

                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
                Debug.WriteLine("Gettings permission 3");
                return false;
            });
            Debug.WriteLine("Gettings permission 4");

            return success;

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



        bool initialised;
        bool FirstUse
        {
            get => Xamarin.Essentials.Preferences.Get(Keys.FirstUse, true);
            set => Xamarin.Essentials.Preferences.Set(Keys.FirstUse, value);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            VM.GeohashLocations.CollectionChanged += GeohashLocations_CollectionChanged;
            VM.OnBoardingViewModel.PropertyChanged += OnBoardingViewModel_PropertyChanged;

            if (FirstUse)
            {
                FirstUse = false;
                VM.OnBoardingViewModel.IsVisible = true;

                return;
            }
            else
            {
                VM.OnBoardingViewModel.IsVisible = false;
            }






        }

        private async void GeohashLocations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null || e.NewItems.Count == 0)
            {
                return;
            }


            CameraUpdate update;
            if (e.NewItems?.Count == 1)
            {
                var loc = e.NewItems[0] as Location;


                update = CameraUpdateFactory.NewPosition(new Position(loc.Latitude, loc.Longitude));
            }
            else
            {
                var newLocs = new List<Location>();
                foreach (var item in e.NewItems)
                {
                    newLocs.Add(item as Location);
                }
                var locationBounds = LocationExtensions.GetBounds(newLocs);
                var bounds = new Bounds(locationBounds.SouthWest.ToPosition(), locationBounds.NorthEast.ToPosition());

                update = CameraUpdateFactory.NewBounds(bounds, 100);
            }

            await TheMap.AnimateCamera(update);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            VM.OnBoardingViewModel.PropertyChanged -= OnBoardingViewModel_PropertyChanged;
            VM.GeohashLocations.CollectionChanged -= GeohashLocations_CollectionChanged;
            VM.StopLocationService();
        }

        private async void OnBoardingViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine("PropertyChanged" + e.PropertyName);
            if (e.PropertyName == "IsVisible" && VM.OnBoardingViewModel.IsVisible == false)
            {
                var granted = await GetPermissions();
                if (granted)
                {
                    VM.LocationPermissionGranted = true;
                    VM.StartLocationService();
                }
            }

            
        }


    }
}
