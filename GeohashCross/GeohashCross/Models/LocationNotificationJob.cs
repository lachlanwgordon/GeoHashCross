using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;
using Plugin.Jobs;
using Plugin.LocalNotifications;
using Xamarin.Essentials;

namespace GeohashCross.Models
{
    public class LocationNotificationJob : IJob
    {
        public LocationNotificationJob()
        {
        }

        public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
        {

            CrossLocalNotifications.Current.Show("Hello", "Starting Job", 2);
            try
            {
                //var loc = await Xamarin.Essentials.Geolocation.GetLocationAsync();//This will silently fail
                //CrossLocalNotifications.Current.Show("Location", $"{loc.Latitude},{loc.Longitude}", 6);//This never appears

                //var subscriptions = await DB.Connection.Table<NotificationSubscription>().ToListAsync();
                var subscriptionsSync = DB.ConnectionSync.Table<NotificationSubscription>().ToList();

                foreach (var sub in subscriptionsSync)
                {
                    CrossLocalNotifications.Current.Show("Location Notification", $"There is a hash within {sub.RadiusInKilometers}km of {sub.Latitude},{sub.Longitude}", 100 + sub.Id);
                }




            }
            catch (Exception ex)
            {
                CrossLocalNotifications.Current.Show("Error", ex.ToString(), 5);//Never seen this
            }


            //try
            //{
            //    var log = Xamarin.Essentials.Preferences.Get(Keys.log, $"new job log {DateTime.Now}\n");

            //    var loc = await Xamarin.Essentials.Geolocation.GetLocationAsync();
            //    var manual = jobInfo.GetValue<bool>("Manual");
            //    log += $"Running manually: {manual}  {DateTime.Now}\n";

            //    var locationRadius = new LocationRadius { Latitude = loc.Latitude, Longitude = loc.Longitude, Radius = 40 };
            //    var hash = await Hasher.GetHashData(DateTime.Today, locationRadius);
            //    var distance = hash.NearestHashLocation.CalculateDistance(locationRadius, DistanceUnits.Kilometers);

            //    if (distance < locationRadius.Radius)
            //    {
            //        Debug.WriteLine($"Hash is close: {distance}km away");
            //        log += $"Hash is close: {distance}km away {DateTime.Now}\n";
            //        CrossLocalNotifications.Current.Show("Geohash is close", $"Hash is close: {distance}km away");

            //    }
            //    else
            //    {
            //        Debug.WriteLine($"Hash is far: {distance}km away");
            //        log += $"Hash is far:{distance}km away {DateTime.Now}\n";
            //        CrossLocalNotifications.Current.Show("Geohash is far", $"Hash is far: {distance}km away, you probably don't care");

            //    }
            //}
            //catch (Exception ex)
            //{
            //    CrossLocalNotifications.Current.Show("Crash", $"Error in background task, check app center{ex}");
            //    Crashes.TrackError(ex);

            //}
            //finally
            //{
            //    CrossLocalNotifications.Current.Show("Finally", $"Finally block");
            //}

            //CrossLocalNotifications.Current.Show("Geohash is far", $"Hash is somewhere: km away, you probably don't care");

        }
    }
}
