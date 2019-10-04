using System;
using System.Collections.Generic;
using GeohashCross.Services;
using GeohashCross.ViewModels;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace GeohashCross.Views
{
    public partial class AboutPage : ContentPage
    {
        OnBoardingView onboarding;
        public void Help_Tapped(object sender, EventArgs e)
        {
            onboarding = new OnBoardingView
            {
                BindingContext = new OnBoardingViewModel(),
            };
            onboarding.OnDisappearing += OnBoadingClosed;
            TheGrid.Children.Add(onboarding);


        }

        private void OnBoadingClosed(object sender, EventArgs e)
        {
            TheGrid.Children.Remove(onboarding);
        }

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
