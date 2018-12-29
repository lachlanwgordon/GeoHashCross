using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GeohashCross.Models
{
    public static class LocationExtension
    {
        public static List<Location> GetNeighbours(this Location location)
        {
            var hashes = new List<Location>
                {
                    new Location(location.Latitude -1, location.Longitude -1, location.Timestamp),
                    new Location(location.Latitude -1, location.Longitude, location.Timestamp),
                    new Location(location.Latitude -1, location.Longitude + 1, location.Timestamp),

                    new Location(location.Latitude, location.Longitude -1, location.Timestamp),
                    new Location(location.Latitude, location.Longitude +1, location.Timestamp),

                    new Location(location.Latitude +1, location.Longitude -1, location.Timestamp),
                    new Location(location.Latitude +1, location.Longitude, location.Timestamp),
                    new Location(location.Latitude +1, location.Longitude +1, location.Timestamp),
                };
            return hashes;
        }

        public static double FixDegreesRange (this double degrees)
        {
            if (degrees < 0)
                return 360 - Math.Abs(degrees);
            if (degrees > 360)
                return degrees - 360;
            return degrees;
        }

        public static double ToDegrees (this double radians)
        {
            var degrees = (180 / Math.PI) * radians;
            return degrees;
        }

        public static double ToRadians(this double degrees)
        {
            var radians = (Math.PI / 180) * degrees;
            return radians;
        }

    }
}
