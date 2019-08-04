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
using SkiaSharp;
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
            Device.StartTimer(TimeSpan.FromSeconds(1f / 5), UpdateCanvas);
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
            Debug.WriteLine($"When am I clicked? {e.Pin.Position.Latitude},{e.Pin.Position.Longitude}");

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

        private void MapClicked(object sender, MapClickedEventArgs e)
        {
            Debug.WriteLine("map clicked");
            VM.TappedLocation = new Location(e.Point.Latitude, e.Point.Longitude);
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

        void Handle_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            if (!VM.DarkNavEnabled)
                return;


            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            var height = e.Info.Height;
            var width = e.Info.Width;

            //Setup canvas with transforms based at the center
            canvas.Translate(width / 2, height / 2);
            canvas.Scale(width / 220);

            canvas.Save();

            //Markers around circle
            for (int angle = 0; angle < 360; angle += 15)
            {
                canvas.DrawCircle(0, -90, angle % 90 == 0 ? 5 : 2, Paint.WhiteFill);
                canvas.RotateDegrees(15);
            }

            canvas.Restore();


            //Target Needle
            canvas.Save();
            canvas.RotateDegrees((int)VM.TargetNeedleDirection);
            canvas.DrawPath(Paint.NeedlyPath, Paint.BluePaint);
            canvas.DrawPath(Paint.NeedlyPath, Paint.WhiteStrokePaint);
            canvas.Restore();



            //MagneticNorth
            canvas.Save();
            canvas.RotateDegrees((int)VM.MagneticNorthNeedleDirection);
            canvas.DrawText("M", 0, -100, Paint.RedPaint);
            canvas.Restore();


            //TrueNorth
            canvas.Save();
            canvas.RotateDegrees((int)VM.TrueNorthNeedleDirection);
            canvas.DrawText("T", 0, -100, BlackPaint);
            canvas.Restore();
        }

        bool UpdateCanvas()
        {
            skiaView.InvalidateSurface();
            return true;
        }

        public static SKPaint BlackPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black
        };

        bool initialised;
        bool FirstUse
        {
            get => Xamarin.Essentials.Preferences.Get(Keys.FirstUse, false);
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

        public async void GlobalClicked(object sender, EventArgs e)
        {
            try
            {
                var loc = VM.HashData.GlobalHash;
                var address = await Xamarin.Essentials.Geocoding.GetPlacemarksAsync(loc);
                var pin = new Xamarin.Forms.GoogleMaps.Pin
                {
                    Label = loc.Timestamp == DateTime.Today ? "Today's Global Hash" : "Global Hash for " + loc.Timestamp.ToString("yyyy-MM-dd"),
                    Position = new Xamarin.Forms.GoogleMaps.Position(loc.Latitude, loc.Longitude),
                    Icon = BitmapDescriptorFactory.DefaultMarker(Color.Green),
                    Address = $"{address.FirstOrDefault().Locality ?? address.FirstOrDefault().SubLocality }"
                };
                TheMap.Pins.Add(pin);
                var lastPos = new Position(loc.Latitude, loc.Longitude);
                await TheMap.AnimateCamera(CameraUpdateFactory.NewPosition(lastPos), TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }


    }
}