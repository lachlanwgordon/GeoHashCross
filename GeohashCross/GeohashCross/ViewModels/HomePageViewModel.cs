using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Geo;
using Geo.Geomagnetism;
using GeohashCross.Converters;
using GeohashCross.Models;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Location = GeohashCross.Models.Location;

namespace GeohashCross.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {

        public static HomePageViewModel Instance {get;set;}
        //TODO: Services in this section should be handled with IOC
        //IDistanceCalculator DistanceCalculator = new DistanceCalculator();

        //TODO: Backing fields for observable properties, these can probably be eliminated with FODY
        bool darkNavEnabled;
        bool locationPermissionGranted;
        bool isSatteliteView;
        HashLocation hashLocation;
        Location currentLocation;
        DateTime date = DateTime.Today;

        public OnBoardingViewModel OnBoardingViewModel { get; set; } = new OnBoardingViewModel();
        public ObservableRangeCollection<HashLocation> GeohashLocations { get; set; } = new ObservableRangeCollection<HashLocation>();
        public bool ShowNeighbours { get; set; }
        public Location TappedLocation { get; set; }





        public ICommand DarkNavCommand => new Command(ExecuteToggleDarkNavCommand);
        public ICommand GlobalHashCommand => new Command(ExecuteGlobalHashCommand);
        public ICommand SatteliteCommand => new Command(ExecuteSatteliteCommand);
        public ICommand ToggleNeighboursCommand => new Command(ExecuteToggleNeighboursCommand);
        public ICommand ResetCommand => new Command(async () => await ExecuteResetCommand());
        public ICommand MapTappedCommand => new Command(async (obj) => await ExecuteMapTappedCommand(obj));
        public ICommand DateChangedCommand => new Command(async () => await LoadHashLocation());
        public ICommand LoadHashLocationCommand => new Command(async () => await LoadHashLocation());


        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        //This is the direction to the hash that the needle should show. It updates as the device is rotated and accounts for magnetic declination
        public float TargetNeedleDirection
        {
            get
            {
                if (currentLocation == null || HashLocation == null)
                    return 0;

                var displayBearing = Bearing - HeadingTrueNorth;

                return (float)displayBearing;
            }
        }

        public static float StaticNeedleDirection => Instance.TargetNeedleDirection;

        public bool LocationPermissionGranted
        {
            get
            {
                return locationPermissionGranted;
            }
            set
            {
                locationPermissionGranted = value;
                OnPropertyChanged(nameof(LocationPermissionGranted));
                if (value == true)
                {
                    StartLocationService();
                }
            }
        }

        public bool IsSatteliteView
        {
            get
            {
                return isSatteliteView;
            }
            set
            {
                isSatteliteView = value;
                OnPropertyChanged(nameof(IsSatteliteView));
            }
        }

        public Location CurrentLocation
        {
            get
            {
                return currentLocation;
            }
            set
            {
                var oldValue = currentLocation;
                currentLocation = value;
                if(oldValue == null)
                {
                    Debug.WriteLine("Am I main thread?");
                    LoadHashLocationCommand.Execute(null);
                }
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(Distance));
                
            }
        }

        public HashLocation HashLocation
        {
            get
            {
                return hashLocation;
            }
            set
            {
                hashLocation = value;
                OnPropertyChanged(nameof(HashLocation));
                OnPropertyChanged(nameof(Distance));
            }
        }


        public Double? Distance
        {
            get
            {
                if (CurrentLocation == null || HashLocation == null)
                    return null;
                //var distance = DistanceCalculator.CalculateDistance(CurrentLocation, HashLocation);
                return 0;// distance;
            }
        }

        public bool DarkNavEnabled
        {
            get
            {
                return darkNavEnabled;
            }
            set
            {
                darkNavEnabled = value;
                OnPropertyChanged(nameof(DarkNavEnabled));
            }
        }




        private void ExecuteToggleNeighboursCommand(object obj)
        {
            ShowNeighbours = !ShowNeighbours;
            if (ShowNeighbours)
            {
                var neighbours = new List<HashLocation>();
                foreach (var hash in GeohashLocations)
                {
                    if (!hash.IsNeighbour)
                    {
                        neighbours.AddRange(hash.Neighbours);
                    }
                }
                GeohashLocations.AddRange(neighbours);
            }
            else
            {
                var neighbours = GeohashLocations.Where(x => x.IsNeighbour).ToList();

                foreach (var loc in neighbours)
                {
                    GeohashLocations.Remove(loc);
                }

            }
        }

        private void ExecuteSatteliteCommand()
        {
            IsSatteliteView = !IsSatteliteView;
        }

        private void ExecuteGlobalHashCommand()
        {
        }















        //This bearing is relative to true north and doesn't use declination
        //Thanks to http://www.movable-type.co.uk/scripts/latlong.html
        public double GetBearingRelativeToTrueNorth(Location currentLocation, Location destination)
        {
            var lat1 = currentLocation.Latitude.ToRadians();
            var lon1 = currentLocation.Longitude.ToRadians();
            var lat2 = destination.Latitude.ToRadians();
            var lon2 = destination.Longitude.ToRadians();

            var dlon = lon2 - lon1;

            var y = Math.Sin(dlon) * Math.Cos(lat2);
            var x = (Math.Cos(lat1) * Math.Sin(lat2)) - (Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dlon));

            var bearing = Math.Atan2(y, x);
            var bearingInDegrees = bearing.ToDegrees();

            var normalisedBearing = (bearingInDegrees + 360) % 360;

            return normalisedBearing;
        }

        void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            HeadingMagneticNorth = e.Reading.HeadingMagneticNorth;
        }

        bool LocationServiceShouldContinue;
        public void StartLocationService()
        {
            LocationServiceShouldContinue = true;
            Device.StartTimer(TimeSpan.FromSeconds(4), TimerInitiatedUpdateLocation);
        }

        public void StopLocationService()
        {
            LocationServiceShouldContinue = false;
        }



        bool TimerInitiatedUpdateLocation()
        {
            Task.Run(async () =>
            {
                var locationRequest = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromMilliseconds(2500));
                var loc = await Geolocation.GetLocationAsync(locationRequest);
                if(loc != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //CurrentLocation = loc.ToGCLocation();
                    });
                }

            });


            return LocationServiceShouldContinue;
        }


        public async Task<HashLocation> LoadHashLocation()
        {
            if(HomePageViewModel.Instance == null)
            {
                HomePageViewModel.Instance = this;
            }

            var loc = TappedLocation ?? CurrentLocation;
            if (loc == null)
            {
                return null;
            }


            var hashData = await Hasher.GetHashData(Date, loc);

            if (hashData.Success)
            {
                var location = hashData.Data;

                GeohashLocations.Add(location);
                if (ShowNeighbours)
                {
                    GeohashLocations.AddRange(location.Neighbours);
                }

            }

            HashLocation = hashData.Data;

            return hashData.Data;
        }






        private async Task ExecuteMapTappedCommand(object obj)
        {
            var loc = obj as Location;
            TappedLocation = loc;
            await LoadHashLocation();
        }

        public async Task ExecuteResetCommand()
        {
            try
            {
                Date = DateTime.Today;
                TappedLocation = null;
                ShowNeighbours = false;
                GeohashLocations.Clear();

                await LoadHashLocation();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }


        public void ExecuteToggleDarkNavCommand()
        {
            //Make Dark nav visible/invible
            DarkNavEnabled = !DarkNavEnabled;

            //Start compass, because compass is only visible in DarkMode
            try
            {
                if (DarkNavEnabled)
                {
                    Compass.ReadingChanged += Compass_ReadingChanged;
                    Compass.Start(SensorSpeed.Fastest);
                }
                else
                {
                    Compass.ReadingChanged -= Compass_ReadingChanged;
                    Compass.Stop();
                }
            }
            catch (Exception ex)
            {
                HeadingMagneticNorth = 30;
                Crashes.TrackError(ex);
            }
        }

















        //Everything below here needs to be refactored into a direction calculating class




        //This is the difference between True north and magnetic north in degress
        public double Declination
        {
            get
            {
                if (CurrentLocation == null)
                    return 0;
                var calc = new WmmGeomagnetismCalculator();
                var declination = calc.TryCalculate(new Coordinate(CurrentLocation.Latitude, CurrentLocation.Longitude), DateTime.Now);
                return declination.Declination;
            }
        }


        public double TrueNorthNeedleDirection
        {
            get
            {
                var headingMag = HeadingMagneticNorth;//If I'm facing magnetic east this will read 90
                var magNeedleDirection = 0 - headingMag;//This will read 0 when facing magEast but needs to correct for declination
                var trueNeedleDirection = magNeedleDirection - Declination;
                return trueNeedleDirection;
            }
        }

        //Direction to show N on display
        public double MagneticNorthNeedleDirection
        {
            get
            {
                var headingMag = HeadingMagneticNorth;//If I'm facing magnetic east this will read 90
                var magNeedleDirection = 0 - headingMag;//This will read 0 when facing magEast but needs to correct for declination
                return magNeedleDirection;
            }
        }




        public double Bearing
        {
            get
            {
                if (CurrentLocation == null || HashLocation == null)
                    return 0;
                var bearing = GetBearingRelativeToTrueNorth(CurrentLocation, HashLocation);//Relative to true north ignoring heading
                return bearing;
            }
        }

        //Direction the devic is facing relative to magnetic north
        public double HeadingMagneticNorth
        {
            get;
            set;
        }

        public double HeadingTrueNorth
        {
            get
            {
                var headingTrueNorth = HeadingMagneticNorth + Declination;

                if (headingTrueNorth > 360)
                    headingTrueNorth -= 360;
                return headingTrueNorth;
            }
        }

    }
}
