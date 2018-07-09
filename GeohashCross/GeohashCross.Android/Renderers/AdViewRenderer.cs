using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Gms.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GeohashCross.Controls;

[assembly: ExportRenderer(typeof(GeohashCross.Controls.AdControlView), typeof(GeohashCross.Droid.Renderers.AdViewRenderer))]
namespace GeohashCross.Droid.Renderers
{
    public class AdViewRenderer : ViewRenderer<Controls.AdControlView, AdView>
    {
        string adUnitId = "ca-app-pub-7124080599643764/8359064883";
        AdSize adSize = AdSize.SmartBanner;
        AdView adView;


        AdView CreateAdView()
        {
            if (adView != null)
                return adView;
            adView = new AdView(Forms.Context);
            adView.AdSize = adSize;
            adView.AdUnitId = adUnitId;
            var adParams = new LinearLayout.LayoutParams( LayoutParams.WrapContent, LayoutParams.WrapContent);
            adView.LayoutParameters = adParams;

            adView.LoadAd(new AdRequest.Builder().Build());

            return adView;

             
        }


        protected override void OnElementChanged(ElementChangedEventArgs<AdControlView> e)
        {
            base.OnElementChanged(e);

            if(Control == null)
            {
                CreateAdView();
                SetNativeControl(adView);
            }
        }
    }
}