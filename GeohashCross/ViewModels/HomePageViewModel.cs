using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GeohashCross.Services;
using MvvmHelpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeohashCross.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public Location CurrentLocation { get; set; }
        public Location TappedLocation { get; set; }
        public Location Offset { get; set; }
        public ObservableRangeCollection<Location> PinLocations = new ObservableRangeCollection<Location>();

        public string Lat
        {
            get
            {
                return CurrentLocation?.Latitude != null ? CurrentLocation.Latitude.ToString("N4") : "Loading...";
            }
        }

        public string Lon
        {
            get
            {
                return CurrentLocation?.Longitude != null ? CurrentLocation.Longitude.ToString("N4") : "Loading...";
            }
        }

        public DateTime Date = DateTime.Today;

        public HomePageViewModel()
        {
            Init();
        }

        private void Init()
        {
            IsBusy = true;
            try
            {
                Device.StartTimer(TimeSpan.FromSeconds(3), UpdateLocation);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {this.GetType().Name} \n {ex}\n{ex.StackTrace}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        bool UpdateLocation()
        {
            InvokeUpdateLocation();
            return true;
        }

        async void InvokeUpdateLocation()
        {
            var loc = await Xamarin.Essentials.Geolocation.GetLocationAsync();
            CurrentLocation = loc;

            OnPropertyChanged(nameof(Lat));
            OnPropertyChanged(nameof(Lon));

            if (Offset == null)
            {
                await LoadLocation();
            }

        }

        private async Task<Location> LoadLocation()
        {
            //var offset = await LoadOffset();

            var loc = TappedLocation != null ? TappedLocation : CurrentLocation;

            var hashLoc = await Hasher.GetLocation(Date, loc);

            PinLocations.Add(hashLoc);

            return hashLoc;
        }

        private async Task<Location> LoadOffset()
        {
            var offset = await Hasher.GetOffset(Date);
            Offset = offset;
            return offset;
        }
    }
}
