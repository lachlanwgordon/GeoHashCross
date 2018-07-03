using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GeohashCross.Services
{
    public class Hasher
    {
        public static async Task<Location> GetLocation(DateTime date, Location currentLocation)
        {
            var offset = await GetOffset(date);

            var lat = currentLocation.Latitude > 0 ? Math.Floor(currentLocation.Latitude) + offset.Latitude : Math.Ceiling(currentLocation.Latitude) - offset.Latitude;
            var lon = currentLocation.Longitude > 0 ? Math.Floor(currentLocation.Longitude) + offset.Longitude : Math.Ceiling(currentLocation.Longitude) - offset.Longitude;

            var loc = new Location(lat, lon);
            return loc;
        }

        public static async Task<Location> GetOffset(DateTime date)
        {
            var dija = await GetDjia(date);
            var prehashString = $"{date.ToString("yyyy-MM-DD")}-dija";
            var offset = CalculateOffset(prehashString);
            return offset;
        }

        public static async Task<string> GetDjia(DateTime date)
        {
            var djia = await Webclient.GetDjia(date);
            return djia;
        }

        private static Location CalculateOffset(string prehashString)
        {
            var md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(prehashString);
            byte[] result = md5.ComputeHash(inputBytes);

            ulong part1 = ((ulong)result[0x0] << 0x38)
                + ((ulong)result[0x1] << 0x30)
                + ((ulong)result[0x2] << 0x28)
                + ((ulong)result[0x3] << 0x20)
                + ((ulong)result[0x4] << 0x18)
                + ((ulong)result[0x5] << 0x10)
                + ((ulong)result[0x6] << 0x08)
                + ((ulong)result[0x7] << 0x00);
            ulong part2 = ((ulong)result[0x8] << 0x38)
                + ((ulong)result[0x9] << 0x30)
                + ((ulong)result[0xA] << 0x28)
                + ((ulong)result[0xB] << 0x20)
                + ((ulong)result[0xC] << 0x18)
                + ((ulong)result[0xD] << 0x10)
                + ((ulong)result[0xE] << 0x08)
                + ((ulong)result[0xF] << 0x00);
            string latStr = ((part1 / 2.0) / (long.MaxValue + (ulong)1)).ToString(CultureInfo.InvariantCulture).Substring("0.".Length); // Some tricks are required to divide by ulong.MaxValue + 1
            string lonStr = ((part2 / 2.0) / (long.MaxValue + (ulong)1)).ToString(CultureInfo.InvariantCulture).Substring("0.".Length);

            double lat, lon;
            double.TryParse(latStr, out lat);
            double.TryParse(lonStr, out lon);

            return new Location(lat, lon);
        } 
    }
}
