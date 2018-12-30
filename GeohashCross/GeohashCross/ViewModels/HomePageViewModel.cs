using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GeohashCross.Models;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using Plugin.Permissions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeohashCross.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel()
        {
            try
            {
                Compass.ReadingChanged += Compass_ReadingChanged;
                Compass.Start(SensorSpeed.UI);
            }
            catch (Exception ex)
            {
                HeadingMagneticNorth = 30;
                Crashes.TrackError(ex);
            }





        }

        Location _CurrentLocation;
        public Location CurrentLocation
        {
            get
            {
                Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");
                return _CurrentLocation;
            }
            set
            {
                Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

                _CurrentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(Distance));
                OnPropertyChanged(nameof(ImHere));

            }
        }

        bool _ShowAdvanced = true;
        public bool ShowAdvanced
        {
            get
            {
                return _ShowAdvanced;
            }
            set
            {
                _ShowAdvanced = value;
                OnPropertyChanged(nameof(ShowAdvanced));
            }
        }


        Location _TappedLocation;
        public Location TappedLocation
        {
            get
            {
                Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

                return _TappedLocation;
            }
            set
            {
                Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

                _TappedLocation = value;
                InvokeLoadHashLocation();
            }
        }
        HashData _HashData;
        public HashData HashData
        {
            get
            {
                Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

                return _HashData;
            }
            set
            {
                Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

                _HashData = value;
                OnPropertyChanged(nameof(HashData));
                OnPropertyChanged(nameof(Distance));
            }
        }

        public ObservableRangeCollection<Location> Locations = new ObservableRangeCollection<Location>();
        public ObservableRangeCollection<Location> LocationsToDisplay = new ObservableRangeCollection<Location>();

        Double? _Distance
        {
            get
            {
                if (_CurrentLocation == null || _HashData == null || _HashData.NearestHashLocation == null)
                    return null;
                var distance = Xamarin.Essentials.Location.CalculateDistance(_CurrentLocation, _HashData.NearestHashLocation, DistanceUnits.Kilometers);
                return distance;
            }
        }

        public string Distance
        {
            get
            {
                var distance = _Distance;
                if (!distance.HasValue)
                    return "Calculating...";
                if (distance < 1)
                    return (distance.Value * 1000).ToString("N2") + "m";
                else
                    return _Distance.Value.ToString("N2") + "Km";
            }
        }

        //Thanks to http://www.movable-type.co.uk/scripts/latlong.html
        public double Bearing
        {
            get
            {


                if (_CurrentLocation == null || _HashData == null || _HashData.NearestHashLocation == null)
                    return 0;


                var bearing = GetBearing(_CurrentLocation, _HashData.NearestHashLocation);

                return bearing;


            }
        }

        public double BearingToTrueNorth
        {
            get
            {
                var bearing =  HeadingMagneticNorth - TrueNorth;

                return bearing;
            }
        }

        public double GetBearing(Location currentLocation, Location destination)
        {
            var lat1 = currentLocation.Latitude.ToRadians();
            var lon1 = currentLocation.Longitude.ToRadians();
            var lat2 = destination.Latitude.ToRadians();
            var lon2 = destination.Longitude.ToRadians();

            var dlon = lon2 - lon1;


            var y = Math.Sin(dlon) * Math.Cos(lat2);
            var x = (Math.Cos(lat1) * Math.Sin(lat2)) -
                                 (Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dlon));

            var bearing = Math.Atan2(y, x);
            var bearingInDegrees = bearing.ToDegrees();

            var normalisedBearing = (bearingInDegrees + 360) % 360;

            return normalisedBearing;

        }




        public double HeadingMagneticNorth
        {
            get;
            set;

        }

        public Location TrueNorthPole { get; } = new Location(90, 0);
        public Location MagneticNorthPole { get; } = new Location(86, 175.346);

        public double MagneticNorth
        {
            get
            {
                if (_CurrentLocation == null)
                    return 0;
                var bearing = GetBearing(_CurrentLocation, MagneticNorthPole);
                return bearing;
            }
        }

        //A complicated way of returning 0
        public double TrueNorth
        {
            get
            {
                if (_CurrentLocation == null)
                    return 0;
                var bearing = GetBearing(_CurrentLocation, TrueNorthPole);
                return bearing;
            }
        }

        public double NorthRelativeToHeading
        {
            get
            {
                return 0 - HeadingMagneticNorth;
            }
        }

        public double Desire
        {
            get
            {
                var desire = Bearing - HeadingMagneticNorth;
                desire = desire.FixDegreesRange().FixDegreesRange();
                return desire;
            }
        }

        void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            HeadingMagneticNorth = e.Reading.HeadingMagneticNorth;
        }


        public bool ImHere
        {
            get
            {
                var distance = _Distance;

                if (!distance.HasValue)
                {
                    return false;
                }

                if (distance.Value < 0.005)//5 metres
                {
                    return true;
                }
                if (_CurrentLocation.Accuracy.HasValue && _CurrentLocation.Accuracy.Value <= 50)//Accuracy greater than 10 metres
                {
                    if (distance * 1000 < _CurrentLocation.Accuracy.Value)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        DateTime _Date = DateTime.Today;
        public DateTime Date
        {
            get
            {
                Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

                return _Date;
            }
            set
            {
                Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

                if (value != _Date)
                {
                    Debug.WriteLine("Date changed");
                    _Date = value;
                    //Locations.Clear();
                    InvokeLoadHashLocation();
                }
                else
                {
                    _Date = value;
                }
            }
        }

        internal async Task Refresh()
        {

            Locations.Clear();
            LocationsToDisplay.Clear();
            TappedLocation = null;
            _Date = DateTime.Today;
            OnPropertyChanged(nameof(Date));
            await LoadHashLocation();
        }

        bool TimerInitiatedUpdateLocation()
        {

            InvokeUpdateCurrentLocation();
            return true;
        }

        public async void InvokeUpdateCurrentLocation()
        {

            await UpdateCurrentLocation();
        }

        public async Task<Response<Location>> UpdateCurrentLocation()
        {

            var loc = await Geolocation.GetLocationAsync();
            CurrentLocation = loc;

            if (!Initialised)
            {
                SetUpTimer();
            }
            OnPropertyChanged(nameof(Desire));
            OnPropertyChanged(nameof(Bearing));
            OnPropertyChanged(nameof(HeadingMagneticNorth));
            OnPropertyChanged(nameof(NorthRelativeToHeading));
            OnPropertyChanged(nameof(TrueNorth));
            return new Response<Location>(loc, true, "Location Loaded successfully");
        }


        private bool Initialised = false;
        private void SetUpTimer()
        {

            try
            {
                Initialised = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), TimerInitiatedUpdateLocation);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {this.GetType().Name} \n {ex}\n{ex.StackTrace}");
                Crashes.TrackError(ex);
            }
        }

        public async void InvokeLoadHashLocation()
        {

            try
            {
                var loc = await LoadHashLocation();
                if (loc.Success == false)
                {
                    await Application.Current.MainPage.DisplayAlert("Couldn't load hash", loc.Message, "Okay");
                }
            }
            catch (Exception ex)
            {

                Crashes.TrackError(ex);

                Debug.WriteLine($"Error in invoke load hash location{ex}\n{ex.StackTrace}");
            }
        }
        public async Task<HashData> LoadHashLocation()
        {

            var loc = TappedLocation ?? _CurrentLocation;

            var hashData = await Hasher.GetHashData(Date, loc);



            if (hashData.Success)
            {

                var existingPins = Locations.Where(x => x.Latitude == hashData.NearestHashLocation.Latitude
                                                                && x.Longitude == hashData.NearestHashLocation.Longitude).ToList();
                Locations.RemoveRange(existingPins);
                Locations.Add(hashData.NearestHashLocation);
                LocationsToDisplay.Add(hashData.NearestHashLocation);
                if (ShowNeighbours)
                {
                    LocationsToDisplay.AddRange(hashData.NearestHashLocation.GetNeighbours());
                }

            }

            HashData = hashData;

            return hashData;
        }

        bool _ShowNeighbours;
        public bool ShowNeighbours
        {
            get
            {
                return _ShowNeighbours;
            }
            set
            {
                _ShowNeighbours = value;
                InvokeUpdateNeighbouringPins();
            }
        }

        bool _DarkNavEnabled = false;
        public bool DarkNavEnabled
        {
            get
            {
                return _DarkNavEnabled;
            }
            set
            {
                _DarkNavEnabled = value;
                OnPropertyChanged(nameof(DarkNavEnabled));
            }
        }

        private async void InvokeUpdateNeighbouringPins()
        {
            try
            {
                await UpdateNeighbouringPins();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error adding pins \n{ex}\n{ex.StackTrace}");
            }
        }

        private async Task UpdateNeighbouringPins()
        {
            if (ShowNeighbours)
            {
                foreach (var loc in Locations)
                {
                    LocationsToDisplay.AddRange(loc.GetNeighbours());
                }
            }
            else
            {
                var removeMe = LocationsToDisplay.Where(x => Locations.Where(y => y.Latitude == x.Latitude && y.Longitude == x.Longitude).Count() == 0).ToList();
                LocationsToDisplay.RemoveRange(removeMe, System.Collections.Specialized.NotifyCollectionChangedAction.Remove);
            }

        }

        public Location GetGlobal()
        {
            var global = _HashData.GlobalHash;
            //LocationsToDisplay.Add(global);
            return global;
        }
    }
}
