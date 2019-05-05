using System;
namespace GeohashCross.Resources
{
    public static class APIKeys
    {
        public const string PlaceHolder = "YOUR KEY HERE";
        public static bool MapsKeyInitialized;
        public static string MapsKey = "YOUR KEY HERE";//To use google maps you will need an api key from https://developers.google.com/maps/documentation/ios-sdk/get-api-key
        public static string AnalyticsIOSKey = "YOUR KEY HERE";//This key only needed if you want to connect to your own App in AppCenter 
        public static string AnalyticsAndroidKey = "YOUR KEY HERE";//This key only needed if you want to connect to your own App in AppCenter 
    }
}
