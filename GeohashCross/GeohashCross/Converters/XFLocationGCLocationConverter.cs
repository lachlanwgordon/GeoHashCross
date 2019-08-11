using System;
using Xamarin.Forms.GoogleMaps;

namespace GeohashCross.Converters
{
    public static class XFLocationGCLocationConverter
    {
        public static Xamarin.Essentials.Location ToEssentialsLocation(this GeohashCross.Models.Location location)
        {
            return new Xamarin.Essentials.Location(location.Latitude, location.Longitude);
        }

        public static GeohashCross.Models.Location ToGCLocation(this Xamarin.Essentials.Location location)
        {
            return new GeohashCross.Models.Location(location.Latitude, location.Longitude);
        }

        public static Position ToPosition(this GeohashCross.Models.Location location)
        {
            return new Position(location.Latitude, location.Longitude);
        }

    }
}
