using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;

namespace GeohashCross.Services
{
    public static class AnalyticsManager
    {
        public const string AllowAnalyticsKey = "ALLOW_ANALYTICS";
        public const string AllowCrashesKey = "ALLOW_CRASHES";
        public const string PageOpened = "Page Opened";

        public static async Task Initialize()
        {
            var allowAnalytics = Preferences.Get(AllowAnalyticsKey, false);
            var allowCrashes = Preferences.Get(AllowCrashesKey, false);

            AppCenter.Start("ios=c5b0f61c-8af9-4135-9644-37708b280ff0;" +
                "uwp={Your UWP App secret here};" +
                "android={Your Android App secret here}",
                typeof(Analytics), typeof(Crashes));

            AppCenter.LogLevel = LogLevel.Verbose;

            await Analytics.SetEnabledAsync(allowAnalytics);
            await Crashes.SetEnabledAsync(allowCrashes);
        }

        public static async Task SetAnalyticsEnabled(bool allowAnalytics)
        {
            Preferences.Set(AllowAnalyticsKey, allowAnalytics);
            await Analytics.SetEnabledAsync(allowAnalytics);

            Analytics.TrackEvent("SetAnalyticsEnabled", new Dictionary<string, string>
            {
                { "allowAnalytics", $"{allowAnalytics}"}
            });
        }

        public static async Task SetCrashesEnabled(bool allowCrashes)
        {
            Preferences.Set(AllowCrashesKey, allowCrashes);
            await Crashes.SetEnabledAsync(allowCrashes);

            Analytics.TrackEvent("SetCrashesEnabled", new Dictionary<string, string>
            {
                { "allowCrashes", $"{allowCrashes}"}
            });
        }
    }
}
