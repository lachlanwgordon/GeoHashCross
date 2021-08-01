using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//using Android.Gms.Ads;
using Xamarin.Forms;
using Shiny;
using Xamarin.Essentials;

namespace GeohashCross.Droid
{
    [Activity(Label = "GeohashCross", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            //Xamarin.Calabash.Start();

            base.OnCreate(bundle);
            Forms.SetFlags(new[] { "CollectionView_Experimental", "Shell_Experimental", "Visual_Experimental" });
            Xamarin.FormsGoogleMaps.Init(this, bundle); // initialize for Xamarin.Forms.GoogleMaps
            LoadApplication(new App());
        }

        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

