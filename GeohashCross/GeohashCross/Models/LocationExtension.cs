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
                    new Location(location.Latitude -1, location.Longitude -1, location.TimestampUtc),
                    new Location(location.Latitude -1, location.Longitude, location.TimestampUtc),
                    new Location(location.Latitude -1, location.Longitude + 1, location.TimestampUtc),

                    new Location(location.Latitude, location.Longitude -1, location.TimestampUtc),
                    new Location(location.Latitude, location.Longitude +1, location.TimestampUtc),

                    new Location(location.Latitude +1, location.Longitude -1, location.TimestampUtc),
                    new Location(location.Latitude +1, location.Longitude, location.TimestampUtc),
                    new Location(location.Latitude +1, location.Longitude +1, location.TimestampUtc),
                };
            return hashes;
        }
        
    }
}
