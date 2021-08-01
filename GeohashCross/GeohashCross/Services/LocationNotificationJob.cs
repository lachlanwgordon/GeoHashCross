using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;
using Shiny.Jobs;
using Shiny.Notifications;
using Xamarin.Essentials;

namespace GeohashCross.Models
{
    public class LocationNotificationJob : IJob
    {
        IDistanceCalculator DistanceCalculator = new DistanceCalculator();

        public LocationNotificationJob(INotificationManager notificationManager)
        {
            this.notificationManager = notificationManager;
        }

        private INotificationManager notificationManager;

        public async Task<bool> Run(JobInfo jobInfo, CancellationToken cancelToken)
        {

            try
            {
                var subscriptionsSync = DB.ConnectionSync.Table<NotificationSubscription>().ToList();

                foreach (var sub in subscriptionsSync)
                {
                    var subLocation = new Location(sub.Latitude, sub.Longitude);
                    var hash = await Hasher.GetHashData(DateTime.Today, subLocation);

                    var nearBy = new List<Location>(hash.Data.Neighbours) { hash.Data };
                    var closest = nearBy.OrderBy(x => DistanceCalculator.CalculateDistance(subLocation, x)).FirstOrDefault();

                    var distance = DistanceCalculator.CalculateDistance(subLocation, closest);

                    var message = $"Today's hash is {distance}km from {sub.Name} at ({sub.Latitude.ToString("0.###")},{sub.Longitude.ToString("0.###")})";
                    if (distance < sub.Radius)
                    {
                        await notificationManager.Send(new Notification { Title = $"Hash is close to {sub.Name}", Message = message + $". Alarm set for {sub.AlarmTime}" });
                    }
                }
                

            }
            catch (Exception ex)
            {
                //CrossLocalNotifications.Current.Show("Error", ex.ToString(), 5);//Never seen this
            }

            return true;
        }
       
    }
}
