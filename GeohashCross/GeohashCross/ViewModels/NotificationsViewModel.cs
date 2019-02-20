using System;
using System.Windows.Input;
using GeohashCross.Models;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using Plugin.Jobs;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeohashCross.ViewModels
{
    public class NotificationsViewModel : BaseViewModel
    {
        public string Log
        {
            get
            {
                var defaultMessage = $"New log from notifications page {DateTime.Now}\n";
                var log = Xamarin.Essentials.Preferences.Get(Keys.log, "");
                if(string.IsNullOrEmpty(log))
                {
                    log = defaultMessage;
                    Xamarin.Essentials.Preferences.Set(Keys.log, log);
                }
                return log;
            }
        }

        public ICommand SubscribeCommand => new Command(Subscribe);

        private async void Subscribe()
        {
            var log = Xamarin.Essentials.Preferences.Get(Keys.log, "");

            var loc = await Xamarin.Essentials.Geolocation.GetLocationAsync();
            var location = new LocationRadius { Latitude = loc.Latitude, Longitude = loc.Longitude, Radius = 20 };
            var job = new JobInfo
            {
                Name = "Distance",
                Type = typeof(LocationNotificationJob),
                BatteryNotLow = false,
                DeviceCharging = false,
                RequiredNetwork = NetworkType.Any,


            };
            job.SetValue("Location", location);
            log += "subscribing to job\n";
            Xamarin.Essentials.Preferences.Set(Keys.log, log);
            OnPropertyChanged(nameof(Log));
            await CrossJobs.Current.Schedule(job);

        }

        public ICommand RunNowCommand => new Command(RunNow);

        private async void RunNow()
        {
            var log = Xamarin.Essentials.Preferences.Get(Keys.log, "");

            try
            {
                log += $"Manual run {DateTime.Now}\n";
                Xamarin.Essentials.Preferences.Set(Keys.log, log);
                await CrossJobs.Current.Run("Distance");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }   
            finally
            {
                //Xamarin.Essentials.Preferences.Set(Keys.log, log);
            }
        }
    }
}

