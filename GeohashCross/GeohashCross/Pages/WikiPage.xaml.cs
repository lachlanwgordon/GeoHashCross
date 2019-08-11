using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeohashCross.Services;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeohashCross.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WikiPage : ContentPage
	{
		public WikiPage ()
		{
			InitializeComponent ();
            Web.Navigating += Web_Navigating;
            Web.Navigated += Web_Navigated;
		}

        protected override void OnAppearing()
        {
            //base.OnAppearing();
            //Web.Source = "http://wiki.xkcd.com/geohashing";
            //Analytics.TrackEvent(AnalyticsManager.PageOpened, new Dictionary<string, string>
            //{
            //    {"Page", GetType().Name}
            //});
        }
        bool wiki = true;
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            wiki = !wiki;
            Web.Source = "http://wiki.xkcd.com/geohashing";// : "https://google.com";
            Debug.WriteLine(Web.Source);
        }

        void Web_Navigating(object sender, WebNavigatingEventArgs e)
        {
            Debug.Write(e.Url);
        }

        void Web_Navigated(object sender, WebNavigatedEventArgs e)
        {
            Debug.WriteLine(e.Url);
        }


    }
}