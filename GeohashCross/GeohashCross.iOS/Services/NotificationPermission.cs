using System;
using System.Threading.Tasks;
using Foundation;
using GeohashCross.iOS.Services;
using GeohashCross.Services;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationPermission))]
namespace GeohashCross.iOS.Services
{
    public class NotificationPermission : INotificationPermission
    {
        public async Task<bool> GetPermission()
        {
            // Ask the user for permission to get notifications on iOS 10.0+
            var allowed = await UNUserNotificationCenter.Current.RequestAuthorizationAsync(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound);

            return allowed.Item1;
        }
    }
}
