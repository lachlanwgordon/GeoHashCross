using System;
using System.Collections.Generic;
using GeohashCross.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GeohashCross.Views
{
    public partial class HomePage : ContentPage
    {
        HomePageViewModel VM { get; set; } = new HomePageViewModel();
        public HomePage()
        {
            BindingContext = VM;
            InitializeComponent();
            SetListeners();
        }

        private void SetListeners()
        {
            VM.PinLocations.CollectionChanged += PinLocations_CollectionChanged;
        }

        async void PinLocations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Map.Pins.Clear();
            Position? firstPos = null;
            foreach (var loc in VM.PinLocations)
            {
                var pin = new Xamarin.Forms.GoogleMaps.Pin
                {
                    Label = "Today's hash",
                    Position = new Xamarin.Forms.GoogleMaps.Position(loc.Latitude, loc.Longitude)
                };
                Map.Pins.Add(pin);

                if(!firstPos.HasValue)
                {
                    firstPos = new Position(loc.Latitude, loc.Longitude);
                }
            }

            if(firstPos.HasValue)
            {
                await Map.AnimateCamera(CameraUpdateFactory.NewPosition(firstPos.Value), TimeSpan.FromSeconds(0.5));
            }
        }
    }
}
