using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Xamarin.Forms;

namespace GeohashCross.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public ICommand WebsiteCommand => new Command<string>(OpenMyWebsite);

        async void OpenMyWebsite(string url)
        {
            try
            {
                await Xamarin.Essentials.Browser.OpenAsync(url);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error in opening site\n{ex}\n{ex.StackTrace}");
            }
        }

    }
}
