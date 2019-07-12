using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using GeohashCross.Resources;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace GeohashCross.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags(new[] { "CollectionView_Experimental", "Shell_Experimental", "Visual_Experimental" });
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();



            if(!String.IsNullOrWhiteSpace(APIKeys.MapsKey))
            {
                //Xamarin.FormsGoogleMaps.Init("AIzaSyB4t83nCYQnkjVUWpw83gabjKoPXG0QpAs"); //This key has been deactivated but not removed from source. Those who edit git history are doomed to repeat it. https://twitter.com/TheOnlyMego/status/918326739952160769
                Xamarin.FormsGoogleMaps.Init(APIKeys.MapsKey); // initialize for Xamarin.Forms.GoogleMaps
            }
            else
            {
                Debug.WriteLine("Please register for a free google maps api key https://developers.google.com/maps/documentation/ios-sdk/get-api-key");
                Xamarin.FormsGoogleMaps.Init("THIS IS AN INVALID KEY"); // initialize for Xamarin.Forms.GoogleMaps
            }


            Plugin.Jobs.CrossJobs.Init();
            LoadApplication(new App());

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Ask the user for permission to get notifications on iOS 10.0+
                UNUserNotificationCenter.Current.RequestAuthorization(
                        UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                        (approved, error) => { });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // Ask the user for permission to get notifications on iOS 8.0+
                var settings = UIUserNotificationSettings.GetSettingsForTypes(
                        UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                        new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            return base.FinishedLaunching(app, options);
        }

        public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            Plugin.Jobs.CrossJobs.OnBackgroundFetch(completionHandler);
        }


    }
}
