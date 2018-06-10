using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace GeohashCross.Model.Services
{
    public class LocationService
    {
        public LocationService()
        {
        }

        public static async Task<Position> GetLocation()
        {
            
            var position = await CrossGeolocator.Current.GetPositionAsync();

            return position;
        }
    }
}
