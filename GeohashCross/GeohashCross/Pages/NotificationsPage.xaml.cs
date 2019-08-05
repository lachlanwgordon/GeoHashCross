using System;
using System.Collections.Generic;
using System.Diagnostics;
using GeohashCross.Models;
using Xamarin.Forms;

namespace GeohashCross.Views
{
    public partial class NotificationsPage : ContentPage
    {
        public NotificationsPage()
        {
            InitializeComponent();
        }

        public void Handle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection as NotificationSubscription;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
