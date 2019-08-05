using System;
namespace GeohashCross.Models
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Accuracy { get; set; }

        public Location (double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Location()
        {
        }
    }
}
