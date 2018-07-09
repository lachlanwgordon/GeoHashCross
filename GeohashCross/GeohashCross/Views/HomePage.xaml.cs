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
            var hashLoc = VM.LoadHashLocation();
            if (hashLoc == null)
            {
                await DisplayAlert("Error", "Could not load DJIA for today, please check internet connection", "Okay");
            }
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
            VM.LocationsToDisplay.CollectionChanged += PinLocations_CollectionChanged;
            //var pins = TheMap.Pins as ObservableCollection<Pin>;
            //VM.PinLocations.CollectionChanged += pins.CollectionChanged;
            
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
                            Label = loc.TimestampUtc == DateTime.Today ? "Today's Hash" : loc.TimestampUtc.ToString("yyyy-MM-dd"),
                            Position = new Xamarin.Forms.GoogleMaps.Position(loc.Latitude, loc.Longitude),
                            Icon = loc.TimestampUtc == DateTime.Today ? BitmapDescriptorFactory.DefaultMarker(Color.Red) : BitmapDescriptorFactory.DefaultMarker(Color.Yellow)
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
    }
}