using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GeohashCross.Model.Services;
using MvvmHelpers;
using Xamarin.Forms.GoogleMaps;

namespace GeohashCross.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            Init();
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
                _Date = value;
                if(CurrentPosition.HasValue|| ClickedPosition.HasValue)
                {
                    LoadPins(clearPins: true);
                }
            }
        } 


        string _CurrentLocation;
        public string CurrentLocation
        {
            get
            {
                return _CurrentLocation;
            }

            set
            {
                _CurrentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
            }
        }
        string _HashLocation;

        public string HashLocation
        {
            get
            {
                return _HashLocation;
            }

            set
            {
                _HashLocation = value;
                OnPropertyChanged(nameof(HashLocation));
            }
        }
        Position? _CurrentPosition;

        public Position? CurrentPosition
        {
            get
            {
                return _CurrentPosition;
            }

            set
            {
                _CurrentPosition = value;
            }
        }

        Position? _ClickedPosition;

        public Position? ClickedPosition
        {
            get
            {
                return _ClickedPosition;
            }

            set
            {
                _ClickedPosition = value;
                LoadPins();
            }
        }

        public double HashLatitude { get; set; } 
        public double HashLongitude { get; set; }
        bool _LoadNeighbours;

        public bool LoadNeighbours
        {
            get
            {
                return _LoadNeighbours;
            }

            set
            {
                _LoadNeighbours = value;
                if (CurrentPosition.HasValue || ClickedPosition.HasValue)
                {
                    LoadPins(clearPins: true);
                }
            }
        }

        public bool LoadFuture { get; set; }

        public async void Init()
        {
            try
            {
                await GetLocation();
                await LoadPins();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {this.GetType().Name} \n {ex}\n{ex.StackTrace}");
            }
        }

        public async Task<bool> GetLocation()
        {
            bool success = true;
            var pos = await LocationService.GetLocation();
            if (pos != null)
            {
                CurrentPosition = new Position(pos.Latitude, pos.Longitude);
                CurrentLocation = $"{pos.Latitude.ToString("N4")}, {pos.Longitude.ToString("N4")}";
            }
            else
            {
                CurrentLocation = "Could not load your location, tap somewhere on the map.";
                success = false;
            }
            return success;
        }

        internal async Task<bool> LoadPins( bool clearPins = false)
        {
            var postion = ClickedPosition != null ? ClickedPosition.Value : CurrentPosition.Value;

            if(clearPins)
            {
                Pins.Clear();
            }

            var locs = await Hasher.GetCoordinates(lat: postion.Latitude, lon: postion.Longitude, date: Date, loadNeighbours: LoadNeighbours);
            if(locs?.Count > 0)
            {
                HashLatitude = locs[0].Latitude;
                HashLongitude = locs[0].Longitude;
                HashLocation = $"{locs[0].Latitude.ToString("N4")}, {locs[0].Longitude.ToString("N4")}";
                foreach (var loc in locs)
                {
                    if (!Pins.Any(x => x.Position.Latitude == loc.Latitude && x.Position.Longitude == loc.Longitude))
                    {
                        var pos = new Position(loc.Latitude, loc.Longitude);

                        Pins.Add(new Pin
                        {
                            Position = pos,
                            Label = "Today's Hash",
                            Address = "Open in Maps"
                             

                        });
                    }
                }
                return true;
            }
            else
            {
                HashLocation = "Couldn't load hash, please connect to internet";
                return false;
            }
        }

        public ObservableRangeCollection<Pin> Pins = new ObservableRangeCollection<Pin>();
    }
}
