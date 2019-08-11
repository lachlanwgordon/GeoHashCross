using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeohashCross.Resources;
using GeohashCross.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeohashCross.Services
{
    public static class AnalyticsManager
    {
        public const string AllowAnalyticsKey = "ALLOW_ANALYTICS";
        public const string AllowCrashesKey = "ALLOW_CRASHES";
        public const string PageOpened = "Page Opened";

        static bool IsInitialized;

        public static async Task<bool> Initialize()
        {
            if(IsInitialized)
                return true;
            var allowAnalytics = Preferences.Get(AllowAnalyticsKey, false);
            var allowCrashes = Preferences.Get(AllowCrashesKey, false);

            if (allowAnalytics || allowCrashes)
            {
                if (String.IsNullOrEmpty(APIKeys.AnalyticsIOSKey))
                {
                    await Shell.Current.DisplayAlert("APIKey missing", "Please set analytics key in APIKeys.cs", "Okay");
                    return false;
                }
                IsInitialized = true;
                AppCenter.Start($"ios={APIKeys.AnalyticsIOSKey};android={APIKeys.AnalyticsAndroidKey}", typeof(Analytics), typeof(Crashes));

                AppCenter.LogLevel = LogLevel.Verbose;

                await Analytics.SetEnabledAsync(allowAnalytics);
                await Crashes.SetEnabledAsync(allowCrashes);



            }
            return true;

        }

        public static async Task<bool> SetAnalyticsEnabled(bool allowAnalytics)
        {
            Preferences.Set(AllowAnalyticsKey, allowAnalytics);

            var success = await Initialize();
            await Analytics.SetEnabledAsync(allowAnalytics);

            Analytics.TrackEvent("SetAnalyticsEnabled", new Dictionary<string, string>
            {
                { "allowAnalytics", $"{allowAnalytics}"}
            });
            return success;
        }

        public static async Task<bool> SetCrashesEnabled(bool allowCrashes)
        {
            Preferences.Set(AllowCrashesKey, allowCrashes);
            var success = await Initialize();
            await Crashes.SetEnabledAsync(allowCrashes);

            Analytics.TrackEvent("SetCrashesEnabled", new Dictionary<string, string>
            {
                { "allowCrashes", $"{allowCrashes}"}
            });
            return success;
        }
    }
}
