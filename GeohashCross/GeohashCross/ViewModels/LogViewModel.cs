using System;
using GeohashCross.Services;
using MvvmHelpers;

namespace GeohashCross.ViewModels
{
    public class LogViewModel : BaseViewModel
    {
        public LogViewModel()
        {
        }

        public string Log
        {
            get { return Xamarin.Essentials.Preferences.Get(Keys.log, "Log"); }

        }

        internal void UpdateLogDisplay()
        {
            OnPropertyChanged(nameof(Log));
            //var log = new Plugin.Jobs.JobLog();
        }
    }
}
