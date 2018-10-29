﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GeohashCross.Models;
using GeohashCross.Services;
using MvvmHelpers;
using Plugin.Permissions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeohashCross.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        
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

                OnPropertyChanged(nameof(IsAdVisible));
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
        //public bool ShowAdvanced { get; set; } = true;
        public bool IsAdVisible
        {
            get
            {
                return !ImHere;
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
                if (CurrentLocation == null || HashData == null || HashData.NearestHashLocation == null)
                    return null;
                var distance = Xamarin.Essentials.Location.CalculateDistance(CurrentLocation, HashData.NearestHashLocation, DistanceUnits.Kilometers);
                return distance;
            }
        }

        public String Distance
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

        public bool ImHere
        {
            get
            {
                var distance = _Distance;

                if (!distance.HasValue)
                {
                    return false;
                }

                if(distance.Value < 0.005)//5 metres
                {
                    return true;
                }
                if(CurrentLocation.Accuracy.HasValue && CurrentLocation.Accuracy.Value <= 50)//Accuracy greater than 10 metres
                {
                    if(distance * 1000 < CurrentLocation.Accuracy.Value)
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
            //Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            Locations.Clear();
            LocationsToDisplay.Clear();
            TappedLocation = null;
            _Date = DateTime.Today;
            OnPropertyChanged(nameof(Date));
            await LoadHashLocation();
        }

        bool TimerInitiatedUpdateLocation()
        {
            //Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            InvokeUpdateCurrentLocation();
            return true;
        }

        public async void InvokeUpdateCurrentLocation()
        {
            //Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            await UpdateCurrentLocation();
        }

        public async Task<Response<Location>> UpdateCurrentLocation()
        {
            //Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            var loc = await Xamarin.Essentials.Geolocation.GetLocationAsync();
            CurrentLocation = loc;

            if(!Initialised)
            {
                SetUpTimer();
            }
            return new Response<Location>(loc, true, "Location Loaded successfully") ;
        }


        private bool Initialised = false;
        private void SetUpTimer()
        {
            //Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            try
            {
                Device.StartTimer(TimeSpan.FromSeconds(3), TimerInitiatedUpdateLocation);
                Initialised = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {this.GetType().Name} \n {ex}\n{ex.StackTrace}");
            }
        }

        public async void InvokeLoadHashLocation()
        {
            //Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            try
            {
                var loc = await LoadHashLocation();
                if(loc.Success == false)
                {
                    await Application.Current.MainPage.DisplayAlert("Couldn't load hash", loc.Message, "Okay");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error invoking load hash: \n{ex}\n{ex.StackTrace}");
            }
        }
        public async Task<HashData> LoadHashLocation()
        {
            //Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            var loc = TappedLocation ?? CurrentLocation;

            var hashData = await Hasher.GetHashData(Date, loc);

            

            if (hashData.Success)
            {
                
                var existingPins = Locations.Where(x => x.Latitude == hashData.NearestHashLocation.Latitude
                                                                && x.Longitude == hashData.NearestHashLocation.Longitude).ToList();
                Locations.RemoveRange(existingPins);
                Locations.Add(hashData.NearestHashLocation);
                LocationsToDisplay.Add(hashData.NearestHashLocation);
                if(ShowNeighbours)
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
            if(ShowNeighbours)
            {
                foreach (var loc in Locations)
                {
                    LocationsToDisplay.AddRange(loc.GetNeighbours());
                }
            }
            else
            {
                //var remove = new List<Location>();
                //foreach (var loc in LocationsToDisplay)
                //{
                //    var keep = Locations.Any(x => x.Latitude == loc.Latitude && x.Longitude == loc.Longitude);
                //    if(!keep)
                //    {
                //        remove.Add(loc);
                //    }
                //}

                var removeMe = LocationsToDisplay.Where(x => Locations.Where(y => y.Latitude == x.Latitude && y.Longitude == x.Longitude).Count() == 0).ToList();
                LocationsToDisplay.RemoveRange(removeMe, System.Collections.Specialized.NotifyCollectionChangedAction.Remove);
            }

        }

        

    }
}
