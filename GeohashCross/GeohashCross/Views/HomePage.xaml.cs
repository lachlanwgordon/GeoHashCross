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
using SkiaSharp;
using SkiaSharp.Views.Forms;
using static GeohashCross.Views.HomePage;
using GeohashCross.Models;
using Microsoft.AppCenter.Analytics;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;

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
            UpdateCanvas();
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
            TheMap.UiSettings.MyLocationButtonEnabled = true;
            TheMap.UiSettings.ZoomControlsEnabled = true;
            VM.LocationsToDisplay.CollectionChanged += PinLocations_CollectionChanged;

            TheMap.UiSettings.CompassEnabled = true;
            TheMap.UiSettings.MapToolbarEnabled = true;
            TheMap.UiSettings.IndoorLevelPickerEnabled = true;
            //TheMap.MapType = MapType.Satellite;
            if (DeviceDisplay.MainDisplayInfo.Height == 1792 || DeviceDisplay.MainDisplayInfo.Height == 2436 || DeviceDisplay.MainDisplayInfo.Height == 2688)//Notched phones
            {
                LightFrame.Margin = new Thickness(20, 50, 20, 0);
                TheDarkFrame.Margin = new Thickness(10, 50, 10, 0);
                TheGrid.RowDefinitions.Remove(TheGrid.RowDefinitions.LastOrDefault());
                TheGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(84, GridUnitType.Absolute) });
            }
            else
            {
                LightFrame.Margin = new Thickness(20, 30, 20, 0);
                TheDarkFrame.Margin = new Thickness(10, 30, 10, 0);
            }
            Debug.WriteLine(DeviceInfo.Model);
            Debug.WriteLine(DeviceDisplay.MainDisplayInfo.Height);

            Device.StartTimer(TimeSpan.FromSeconds(1f / 5), UpdateCanvas);

        }

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
                if (e.OldItems != null)
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

                if (e.NewItems != null)
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

                        if (VM.Locations.Any(x => x.Latitude == loc.Latitude && x.Longitude == loc.Longitude))
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
                Crashes.TrackError(ex);
            }
        }

        private async void RefreshClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("refresh");
            await VM.Refresh();
        }

        private void MapClicked(object sender, MapClickedEventArgs e)
        {
            Debug.WriteLine("map clicked");
            VM.TappedLocation = new Location(e.Point.Latitude, e.Point.Longitude);
        }

        private void ShowMoreClicked(object sender, EventArgs e)
        {
            VM.ShowAdvanced = !VM.ShowAdvanced;
        }

        bool SatteliteView = false;
        void SatteliteClicked(object sender, System.EventArgs e)
        {
            try
            {
                SatteliteView = !SatteliteView;
                TheMap.MapType = SatteliteView ? MapType.Satellite : MapType.Street;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in satelite changed \n{ex}\n{ex.StackTrace}");
                Crashes.TrackError(ex);

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

        void DarkNavClicked(object sender, System.EventArgs e)
        {
            VM.DarkNavEnabled = !VM.DarkNavEnabled;
        }




        void Handle_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
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
            canvas.DrawText("T", 0, -100, Paint.BlackPaint);
            canvas.Restore();
        }

        bool UpdateCanvas()
        {
            skiaView.InvalidateSurface();
            return true;
        }

        public static class Paint
        {
            public static SKPaint BlackPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Black
            };

            public static SKPaint RedPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Red
            };


            public static SKPaint BluePaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
               Color = SKColors.Blue
            };


            public static SKPath NeedlyPath = SKPath.ParseSvgPathData("M 0 -80 C 0 -30 20 -30 5 -20 L 10 10 C 5 7.5 -5 7.5 -10 10 L -5 -20 C -20 -30 0 -30 0 -80");

            public static SKPaint WhiteFill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                 Color = SKColors.White,
            };

            public static SKPaint GreyFill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.DimGray,
            };

            public static SKPaint WhiteStrokePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.White,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Round,
                IsAntialias = true
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Analytics.TrackEvent(AnalyticsManager.PageOpened, new Dictionary<string, string>
            {
                {"Page", GetType().Name}
            });
        }

        void GlobalHashClicked(object sender, System.EventArgs e)
        {
            var location = VM.GetGlobal();
        }
    }
}