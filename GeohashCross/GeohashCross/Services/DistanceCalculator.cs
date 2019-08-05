using System;
using GeohashCross.Converters;
using GeohashCross.Models;
using Xamarin.Essentials;
using Location = GeohashCross.Models.Location;

namespace GeohashCross.Services
{
    public class DistanceCalculator : IDistanceCalculator
    {
        public DistanceCalculator()
        {
        }

        public double CalculateDistance(Location start, Location end)
        {
            var distance = Xamarin.Essentials.Location.CalculateDistance(start.ToEssentialsLocation(), end.ToEssentialsLocation(), DistanceUnits.Kilometers);
            return distance;
        }
    }
}
