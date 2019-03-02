﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using GeohashCross.Models;
using Xamarin.Forms;

namespace GeohashCross.Views
{
    public partial class Notifications : ContentPage
    {
        public Notifications()
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
            Debug.WriteLine($"Notifications appearing {DateTime.Now}");
        }
    }
}
