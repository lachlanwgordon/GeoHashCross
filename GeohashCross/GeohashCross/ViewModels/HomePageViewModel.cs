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
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeohashCross.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public ICommand ToggleNeighbours
        {
            get
            {
                return new Command(() =>
                {
                    ShowNeighbours = !ShowNeighbours;
                    UpdateShowNeighbours();

                    

                });
            }
        }

        private void UpdateShowNeighbours()
        {
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


                //Do not convert to RemoveRange, there is a bug that means you end up with no pins displayed even if some are in collection
                foreach (var loc in neighbours)
                {
                    GeohashLocations.Remove(loc);
                }

                Debug.WriteLine(GeohashLocations.Count);
            }
        }



        public ObservableRangeCollection<HashLocation> GeohashLocations
        {
            get;
            set;
        } = new ObservableRangeCollection<HashLocation>();


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

            GeohashLocations.Clear();
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

            try
            {
                Debug.WriteLine("About to get Location");
                Location loc = null;
                int failCount = 0;
                while(loc == null)
                {
                    if (failCount > 0)
                        await Task.Delay(1000);
                    failCount++;
                    var locationRequest = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromMilliseconds(failCount * 1000));
                    Debug.WriteLine($"try location for {failCount * 1000}ms");
                    loc = await Geolocation.GetLocationAsync(locationRequest);

                }



                Debug.WriteLine("Got Location");
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return new Response<Location>(new Location(), false, "Location Loaded successfully");

            }
        }


        private bool Initialised = false;
        private void SetUpTimer()
        {

            try
            {
                if (Initialised)
                    return;
                Initialised = true;
                Debug.WriteLine("!!!!!!!!!!!!!!!STARTING A TIMER");
                Device.StartTimer(TimeSpan.FromSeconds(5), TimerInitiatedUpdateLocation);
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
            if(loc == null)
            {
                return new HashData { Success = false } ;
            }


            var hashData = await Hasher.GetHashData(Date, loc);

            if (hashData.Success)
            {
                var location = new HashLocation(hashData);
                GeohashLocations.Add(location);
                if(ShowNeighbours)
                {
                    GeohashLocations.AddRange(location.Neighbours);
                }

            }

            HashData = hashData;

            return hashData;
        }

        public bool ShowNeighbours { get; set; }

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


        public async Task ChangeDate()
        {
            await LoadHashLocation();
        }

        public ICommand ShowMoreCommand => new Command(ToggleShowMore);
        public ICommand ResetCommand => new Command(Reset);
        public ICommand DarkNavCommand => new Command(ToggleDarkNav);

        public async void Reset()
        {
            try
            {
                Date = DateTime.Today;
                ShowNeighbours = false;
                GeohashLocations.Clear();
                if(CurrentLocation == null)
                {
                    await UpdateCurrentLocation();
                }

                await LoadHashLocation();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
