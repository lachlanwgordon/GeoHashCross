using GeohashCross.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace GeohashCross.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
        HomePageViewModel VM { get; set; } = new HomePageViewModel();
        public HomePage()
        {
            BindingContext = VM;
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var granted = await GetPermissions();
                if (!granted)
                    return;
                SetupUI();
                await SetupLocations();
            });



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
            var hashPos = new Position(hashLoc.NearestHashLocation.Latitude, hashLoc.NearestHashLocation.Longitude);
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

        private void SetupUI()
        {
            //MainMap = new Map();
            //MainMap.MapClicked += MapClicked;
            //TheGrid.Children.Add(MainMap);
            //Grid.SetRow(MainMap, 6);
            //Grid.SetColumnSpan(MainMap, 3);
            TheMap.UiSettings.MyLocationButtonEnabled = true;
            TheMap.UiSettings.ZoomControlsEnabled = true;
            VM.LocationsToDisplay.CollectionChanged += PinLocations_CollectionChanged;
            //var pins = TheMap.Pins as ObservableCollection<Pin>;
            //VM.PinLocations.CollectionChanged += pins.CollectionChanged;


            TheMap.UiSettings.CompassEnabled = true;
            TheMap.UiSettings.MapToolbarEnabled = true;
            TheMap.UiSettings.IndoorLevelPickerEnabled = true;
            //TheMap.MapType = MapType.Satellite;
            if(DeviceDisplay.ScreenMetrics.Height == 1792 || DeviceDisplay.ScreenMetrics.Height == 2436 || DeviceDisplay.ScreenMetrics.Height == 2688)
            {
                TheStack.Margin = new Thickness(20, 50, 20, 0);
            }
            Debug.WriteLine(DeviceInfo.Model);
            Debug.WriteLine(DeviceDisplay.ScreenMetrics.Height);


        }
        //public Map MainMap;
        async void PinLocations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {

                if (VM.LocationsToDisplay?.Count == 0)
                {
                    TheMap.Pins.Clear();
                    return;
                }
                Position? lastPos = null;
                if(e.OldItems != null)
                {

                    foreach (var item in e.OldItems)
                    {
                        var loc = item as Location;
                        var pin = TheMap.Pins.FirstOrDefault(x => x.Position.Latitude == loc.Latitude && x.Position.Longitude == loc.Longitude);
                        if (pin != null)
                        {
                            TheMap.Pins.Remove(pin);
                        }
                    }
                }

                if(e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        var loc = item as Location;
                        var pin = new Xamarin.Forms.GoogleMaps.Pin
                        {
                            Label = loc.Timestamp == DateTime.Today ? "Today's Hash" : loc.Timestamp.ToString("yyyy-MM-dd"),
                            Position = new Xamarin.Forms.GoogleMaps.Position(loc.Latitude, loc.Longitude),
                            Icon = loc.Timestamp == DateTime.Today ? BitmapDescriptorFactory.DefaultMarker(Color.Red) : BitmapDescriptorFactory.DefaultMarker(Color.Yellow)
                        };
                        TheMap.Pins.Add(pin);

                        if(VM.Locations.Any(x => x.Latitude == loc.Latitude && x.Longitude == loc.Longitude))
                        {
                            lastPos = new Position(loc.Latitude, loc.Longitude);
                        }
                    }
                }
                

                if (lastPos.HasValue)
                {
                    await TheMap.AnimateCamera(CameraUpdateFactory.NewPosition(lastPos.Value), TimeSpan.FromSeconds(1));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Pin Collection changed \n{ex}\n{ex.StackTrace}");
            }
        }

        private async void RefreshClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("refresh");
            await VM.Refresh(); }

        private void MapClicked(object sender, MapClickedEventArgs e)
        {
            Debug.WriteLine("map clicked");
            VM.TappedLocation = new Location(e.Point.Latitude, e.Point.Longitude);
        }

        private void ShowMoreClicked(object sender, EventArgs e)
        {
            VM.ShowAdvanced = !VM.ShowAdvanced;



        }

        private async void YouMadeItClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await DisplayActionSheet("Congratulations\nWould you like to take a photo?", "Cancel", null, "Screen shot", "Photo", "Both");



            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}