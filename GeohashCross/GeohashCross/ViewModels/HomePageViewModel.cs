using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Geo;
using Geo.Geomagnetism;
using GeohashCross.Models;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using Plugin.Permissions;
using TrueNorth.Geographic;
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
                Compass.Start(SensorSpeed.Fastest);
            }
            catch (Exception ex)
            {
                HeadingMagneticNorth = 30;
                Crashes.TrackError(ex);
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

        Location _CurrentLocation;
        public Location CurrentLocation
        {
            get
            {
                return _CurrentLocation;
            }
            set
            {
                _CurrentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(Declination));
                OnPropertyChanged(nameof(Distance));
                OnPropertyChanged(nameof(ImHere));
            }
        }

        bool _ShowAdvanced = false;
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
                Task.Run(LoadHashLocation);
            }
        }

        HashData _HashData;
        public HashData HashData
        {
            get
            {
                return _HashData;
            }
            set
            {
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

        //This is the difference between True north and magnetic north in degress
        public double Declination
        {
            get
            {
                if (_CurrentLocation == null)
                    return 0;
                var calc = new WmmGeomagnetismCalculator();
                var declination = calc.TryCalculate(new Coordinate(_CurrentLocation.Latitude, _CurrentLocation.Longitude), DateTime.Now);
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


        //This is the direction to the hash that the needle should show. It updates as the device is rotated and accounts for magnetic declination
        public double TargetNeedleDirection
        {
            get
            {
                if (_CurrentLocation == null || _HashData == null || _HashData.NearestHashLocation == null)
                    return 0;
                var headingMag = HeadingMagneticNorth;
                var headingTrue = HeadingTrueNorth;
                //Bearing
                var bearing = GetBearingRelativeToTrueNorth(_CurrentLocation, _HashData.NearestHashLocation);//Relative to true north ignoring heading



                var displayBearing =  Bearing - HeadingTrueNorth;

                return displayBearing;
            }
        }

        public double Bearing
        {
            get
            {
                if (_CurrentLocation == null || _HashData == null || _HashData.NearestHashLocation == null)
                    return 0;
                var bearing = GetBearingRelativeToTrueNorth(_CurrentLocation, _HashData.NearestHashLocation);//Relative to true north ignoring heading
                return bearing;
            }
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
            OnPropertyChanged(nameof(HeadingMagneticNorth));
            OnPropertyChanged(nameof(HeadingTrueNorth));
            OnPropertyChanged(nameof(Declination));
            OnPropertyChanged(nameof(TrueNorthNeedleDirection));
            OnPropertyChanged(nameof(MagneticNorthNeedleDirection));
            OnPropertyChanged(nameof(TargetNeedleDirection));
            OnPropertyChanged(nameof(Bearing));
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
                return _Date;
            }
            set
            {
                if (value != _Date)
                {
                    Debug.WriteLine("Date changed");
                    _Date = value;
                }
                else
                {
                    _Date = value;
                }
                OnPropertyChanged(nameof(Date));
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

            Task.Run(UpdateCurrentLocation);
            return true;
        }


        public async Task<Response<Location>> UpdateCurrentLocation()
        {

            var loc = await Geolocation.GetLocationAsync();
            CurrentLocation = loc;

            if (!Initialised)
            {
                SetUpTimer();
            }
            OnPropertyChanged(nameof(HeadingMagneticNorth));
            OnPropertyChanged(nameof(HeadingTrueNorth));
            OnPropertyChanged(nameof(Declination));
            OnPropertyChanged(nameof(TargetNeedleDirection));
            OnPropertyChanged(nameof(TrueNorthNeedleDirection));
            OnPropertyChanged(nameof(MagneticNorthNeedleDirection));


            OnPropertyChanged(nameof(TrueNorth));
            return new Response<Location>(loc, true, "Location Loaded successfully");
        }


        private bool Initialised = false;
        private void SetUpTimer()
        {

            try
            {
                Initialised = true;
                Device.StartTimer(TimeSpan.FromSeconds(3), TimerInitiatedUpdateLocation);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {this.GetType().Name} \n {ex}\n{ex.StackTrace}");
                Crashes.TrackError(ex);
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
                UpdateNeighbouringPins();
                OnPropertyChanged(nameof(ShowNeighbours));
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

        private void UpdateNeighbouringPins()
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

        public async Task ChangeDate()
        {
            await LoadHashLocation();
        }

        public ICommand ShowMoreCommand => new Command(ToggleShowMore);
        public ICommand ResetCommand => new Command(Reset);
        public ICommand DarkNavCommand => new Command(ToggleDarkNav);

        public async void Reset()
        {
            Date = DateTime.Today;
            ShowNeighbours = false;
            LocationsToDisplay.Clear();
            Locations.Clear();
            await LoadHashLocation();
        }

        public void ToggleShowMore()
        {
            ShowAdvanced = !ShowAdvanced;
        }

        public void ToggleDarkNav()
        {
            DarkNavEnabled = !DarkNavEnabled;
        }
    }
}
