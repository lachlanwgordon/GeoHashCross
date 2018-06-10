using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeohashCross.Model.Services;
using Xamarin.Forms;

namespace GeohashCross
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Init();
        }


        private async void Init()
        {
            try
            {
                var loc = await Hasher.GetCoordinates();
                latLabel.Text = loc.Latitude.ToString("N4");
                lonLabel.Text = loc.Longitude.ToString("N4");
            }
            catch (Exception ex)
            {
                latLabel.Text = "Error loading dija";
                lonLabel.Text = "Error loading dija";
                Debug.WriteLine($"Error in {this.GetType().Name} \n {ex}\n{ex.StackTrace}");
            }
        }

        private void RefreshClicked(object sender, EventArgs e)
        {
            latLabel.Text = "...";
            lonLabel.Text = "...";
            Init();
        }
    }
}
