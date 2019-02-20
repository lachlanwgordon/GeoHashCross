using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;
using Plugin.Jobs;
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
            var log = Xamarin.Essentials.Preferences.Get(Keys.log, $"new job log {DateTime.Now}\n");



            try
            {
                var loc = await Xamarin.Essentials.Geolocation.GetLocationAsync();
                var locationRadius = new LocationRadius { Latitude = loc.Latitude, Longitude = loc.Longitude, Radius = 40 };
                var hash = await Hasher.GetHashData(DateTime.Today, locationRadius);
                var distance = hash.NearestHashLocation.CalculateDistance(locationRadius, DistanceUnits.Kilometers);

                if (distance < locationRadius.Radius)
                {
                    Debug.WriteLine($"Hash is close: {distance}km away");
                    log += $"Hash is close: {distance}km away {DateTime.Now}\n";
                }
                else
                {
                    Debug.WriteLine($"Hash is far: {distance}km away");
                    log += $"Hash is far:{distance}km away {DateTime.Now}\n";

                }
            }
            catch (Exception ex)
            {
                log += $"log failed {DateTime.Now}\n";
                Crashes.TrackError(ex);
            }
            finally
            {
                Xamarin.Essentials.Preferences.Set(Keys.log, log);
            }


        }
    }
}
