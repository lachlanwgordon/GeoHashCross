using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeohashCross.Model.Services;
using GeohashCross.ViewModels;
using Plugin.ExternalMaps;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GeohashCross
{
    public partial class MainPage : ContentPage
    {
        public MainPageViewModel VM = new MainPageViewModel();

        public MainPage()
        {
            BindingContext = VM;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            try
            {
                VM.Pins.CollectionChanged += Pins_CollectionChanged;
                map.UiSettings.ZoomControlsEnabled = true;
                map.UiSettings.ZoomGesturesEnabled = true;
                map.UiSettings.MyLocationButtonEnabled = true;
                map.InfoWindowClicked += Map_InfoWindowClicked;
                map.MyLocationButtonClicked += Map_MyLocationButtonClicked;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {this.GetType().Name} \n {ex}\n{ex.StackTrace}");
            }
        }

        private async void RefreshClicked(object sender, EventArgs e)
        {
            try
            {
                await VM.LoadPins(clearPins: true);  
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {this.GetType().Name} \n {ex}\n{ex.StackTrace}");
            }
        }

        async void Handle_MapClicked(object sender, Xamarin.Forms.GoogleMaps.MapClickedEventArgs e)
        {
            try
            {
                VM.ClickedPosition = e.Point;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {this.GetType().Name} \n {ex}\n{ex.StackTrace}");
            }
        }

        async void Pins_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {

                if(e.Action == NotifyCollectionChangedAction.Add)
                {
                    var newPins = e.NewItems;
                    foreach (var item in newPins)
                    {
                        var pin = item as Pin;
                        Debug.WriteLine($"Adding pin: {pin.Position.Latitude}, {pin.Position.Longitude}");
                        map.Pins.Add(pin);
                        await map.AnimateCamera(CameraUpdateFactory.NewPosition(pin.Position), TimeSpan.FromSeconds(0.5));
                    } 
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    var oldPins = e.OldItems;
                    foreach (var item in oldPins)
                    {
                        var pin = item as Pin;
                        Debug.WriteLine($"Removing pin: {pin.Position.Latitude}, {pin.Position.Longitude}");
                        map.Pins.Remove(pin);
                    } 
                }
                else if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    map.Pins.Clear();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {this.GetType().Name} \n {ex}\n{ex.StackTrace}");
            }
        }

        async void Map_InfoWindowClicked(object sender, InfoWindowClickedEventArgs e)
        {
            try
            {
                Debug.WriteLine($"clicked on popup{e.Pin.Position.Latitude}");

                await CrossExternalMaps.Current.NavigateTo("Geohash", e.Pin.Position.Latitude, e.Pin.Position.Longitude);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {this.GetType().Name} \n {ex}\n{ex.StackTrace}");
            }

        }

        async void Map_MyLocationButtonClicked(object sender, MyLocationButtonClickedEventArgs e)
        {
            try
            {
                await VM.GetLocation();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in {this.GetType().Name} \n {ex}\n{ex.StackTrace}");
            }
        }

    }
}
