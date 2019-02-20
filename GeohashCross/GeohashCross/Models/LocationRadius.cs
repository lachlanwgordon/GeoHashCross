using System;
using Xamarin.Essentials;

namespace GeohashCross.Models
{
    public class LocationRadius : Location
    {
        public LocationRadius()
        {
        }
        public double Radius { get; set; }
    }
}
