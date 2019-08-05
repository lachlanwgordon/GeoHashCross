using System;
using System.Collections.Generic;
using GeohashCross.Services;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace GeohashCross.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Analytics.TrackEvent(AnalyticsManager.PageOpened, new Dictionary<string, string>
            {
                {"Page", GetType().Name}
            });
        }
    }
}
