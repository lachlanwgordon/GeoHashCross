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
            Init();
        }

        private async void Init()
        {
            await DB.Initialize();
            var subs = DB.Connection.Table<NotificationSubscription>();
            if(await subs.CountAsync() > 0)
            {
                Subscriptions.AddRange(await subs.ToListAsync());

            }


        }

        //public string Log
        //{
        //    get
        //    {
        //        var defaultMessage = $"New log from notifications page {DateTime.Now}\n";
        //        var log = Xamarin.Essentials.Preferences.Get(Keys.log, "");
        //        if(string.IsNullOrEmpty(log))
        //        {
        //            log = defaultMessage;
        //            Xamarin.Essentials.Preferences.Set(Keys.log, log);
        //        }
        //        return log;
        //    }
        //}

        //public ICommand SubscribeCommand => new Command(Subscribe);

        public ObservableRangeCollection<NotificationSubscription> Subscriptions
        {
            get;
            set;
        } = new ObservableRangeCollection<NotificationSubscription>();

        //private async void Subscribe()
        //{
        //    var log = Xamarin.Essentials.Preferences.Get(Keys.log, "");

        //    var loc = await Xamarin.Essentials.Geolocation.GetLocationAsync();
        //    var location = new LocationRadius { Latitude = loc.Latitude, Longitude = loc.Longitude, Radius = 20 };
        //    var job = new JobInfo
        //    {
        //        Name = "Distance",
        //        Type = typeof(LocationNotificationJob),
        //        BatteryNotLow = false,
        //        DeviceCharging = false,
        //        RequiredNetwork = NetworkType.Any,


        //    };
        //    log += "subscribing to job\n";
        //    Xamarin.Essentials.Preferences.Set(Keys.log, log);
        //    OnPropertyChanged(nameof(Log));
        //    await CrossJobs.Current.Schedule(job);



        //}



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
                    RadiusInKilometers = 50,
                    AlarmTime = new DateTime(1, 1, 1, 8, 0, 0)
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

    }
}

