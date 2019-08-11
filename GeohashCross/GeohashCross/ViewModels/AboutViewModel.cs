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

        public ICommand ShowOnboardingCommand => new Command(ShowOnboarding);

        private void ShowOnboarding(object obj)
        {
            OnBoardingViewModel.IsVisible = true;
        }

        public OnBoardingViewModel OnBoardingViewModel { get; set; } = new OnBoardingViewModel();

        public ICommand WebsiteCommand => new Command<string>(OpenMyWebsite);

        public ICommand LogCommand => new Command(OpenLog);

        private async void OpenLog(object obj)
        {
            try
            {
                var defaultLog = $"New log in About page {DateTime.Now}\n";
                var log = Preferences.Get(Keys.log, "");
                if (string.IsNullOrEmpty(log))
                {
                    log = defaultLog;
                    Xamarin.Essentials.Preferences.Set(Keys.log, log);
                }
                await Shell.Current.DisplayAlert("Log", log, "Okay");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

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
