using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        IDistanceCalculator DistanceCalculator = new DistanceCalculator();
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
                    var subLocation = new Location(sub.Latitude, sub.Longitude);
                    var hash = await Hasher.GetHashData(DateTime.Today, subLocation);

                    var nearBy = new List<Location>(hash.Data.Neighbours) { hash.Data  };
                    var closest = nearBy.OrderBy(x => DistanceCalculator.CalculateDistance(subLocation, x)).FirstOrDefault();

                    var distance = DistanceCalculator.CalculateDistance(subLocation, closest);

                    var message = $"Today's hash is {distance}km from {sub.Name} at ({sub.Latitude.ToString("0.###")},{sub.Longitude.ToString("0.###")})";
                    if (distance < sub.Radius)
                    {
                        CrossLocalNotifications.Current.Show($"Hash is close to {sub.Name}", message + $". Alarm set for {sub.AlarmTime}", 100 + sub.Id);
                        CrossLocalNotifications.Current.Show($"ALARM Hash is close to {sub.Name}", message , 200 + sub.Id, DateTime.Today.Add(sub.AlarmTime));

                    }
                    else
                    {
                        CrossLocalNotifications.Current.Show("No hash today", message + $". Alarm set for {sub.AlarmTime}", 100 + sub.Id);
                        CrossLocalNotifications.Current.Show($"ALARM No hash today", message, 200 + sub.Id, DateTime.Today.Add(sub.AlarmTime));
                    }
                }
                if(!subscriptionsSync.Any())
                {
                    CrossLocalNotifications.Current.Show("Hello", "No subscriptions", 3);

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
