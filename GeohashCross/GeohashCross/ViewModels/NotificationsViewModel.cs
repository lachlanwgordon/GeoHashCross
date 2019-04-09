using System;
using System.Windows.Input;
using GeohashCross.Models;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using Plugin.Jobs;
using Plugin.LocalNotifications;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;

namespace GeohashCross.ViewModels
{
    public class NotificationsViewModel : BaseViewModel
    {
        public NotificationsViewModel()
        {
            if(!DesignMode.IsDesignModeEnabled)
                Init();
        }

        private async void Init()
        {
            await DB.Initialize();
            var subs = DB.Connection.Table<NotificationSubscription>();
            if (await subs.CountAsync() > 0)
            {
                Subscriptions.AddRange(await subs.ToListAsync());

            }


        }


        public ObservableRangeCollection<NotificationSubscription> Subscriptions
        {
            get;
            set;
        } = new ObservableRangeCollection<NotificationSubscription>();

  


        public ICommand AddSubscriptionCommand => new Command(AddSubscription);

        private async void AddSubscription()
        {
            try
            {
                var loc = await Xamarin.Essentials.Geolocation.GetLocationAsync();
                var subscription = new NotificationSubscription
                {
                    Latitude = loc.Latitude,
                    Longitude = loc.Longitude,
                    Radius = 50,
                    AlarmTime = new TimeSpan(8,0,0),//new DateTime(1, 1, 1, 8, 0, 0)
                    IsEditing = true
                };
                Subscriptions.Add(subscription);
                var id = await DB.Connection.InsertAsync(subscription);
                subscription.Id = id;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public ICommand DeleteCommand => new Command(Delete);

        void Delete(object obj)
        {

            var grid = obj as View;
            var sub = grid.BindingContext as NotificationSubscription;

            Subscriptions.Remove(sub);
            DB.Connection.DeleteAsync(sub);

        }

    }
}

