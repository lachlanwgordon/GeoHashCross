using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeohashCross.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public ICommand WebsiteCommand => new Command<string>(OpenMyWebsite);
        public bool CrashesEnabled
        {
            get
            {
                return Preferences.Get(AnalyticsManager.AllowCrashesKey, false);
            }
            set
            {
                AnalyticsManager.SetCrashesEnabled(value);
            }
        }

        public bool AnalyticsEnabled
        {
            get
            {
                return Preferences.Get(AnalyticsManager.AllowAnalyticsKey, false);
            }
            set
            {
                AnalyticsManager.SetAnalyticsEnabled(value);
            }
        }


        async void OpenMyWebsite(string url)
        {
            try
            {
                await Xamarin.Essentials.Browser.OpenAsync(url);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error\n{ex}\n{ex.StackTrace}");
                Crashes.TrackError(ex);
            }
        }

    }
}
