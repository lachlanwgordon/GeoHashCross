using System;
using System.Collections.Generic;
using System.Diagnostics;
using GeohashCross.Models;
using GeohashCross.ViewModels;
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

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var context = BindingContext as NotificationsViewModel;
            context.AddSubscriptionCommand.Execute(null);
        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            var context = BindingContext as NotificationsViewModel;
            context.AddSubscriptionCommand.Execute(null);
        }
    }
}
