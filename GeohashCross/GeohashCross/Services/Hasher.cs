using GeohashCross.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GeohashCross.Services
{
    public class Hasher
    {
        public static async Task<HashData> GetHashData(DateTime date, Location currentLocation)
        {
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            var hash = new HashData
            {
                RequestDate = date,
                CurrentLocation = currentLocation
            };
            hash.Date30W = DowJonesDates.Check30W(date, currentLocation);
            var djDate = DowJonesDates.GetMostRecentDJDate(hash.Date30W);
            if(!djDate.Success)
            {
                hash.Message = djDate.Message;
                hash.Success = false;
                return hash;
            }
            hash.DJDate = djDate.Data;

            var djia = await Webclient.GetDjia(djDate.Data);
            if(!djia.Success)
            {
                hash.Message = djia.Message;
                hash.Success = false;
                return hash;
            }
            hash.DJIA = djia.Data;
            hash.Offset = CalculateOffset(date, djia.Data);
            hash.NearestHashLocation = CalculateHashLocation(currentLocation, hash.Offset, date);

            hash.Success = true;
            hash.Message = "Hashes calculated successfully";
            return hash;
        }

        

        public static Location CalculateHashLocation(Location currentLocation, Location offset, DateTime date)
        {
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            var lat = currentLocation.Latitude > 0 ? Math.Floor(currentLocation.Latitude) + offset.Latitude : Math.Ceiling(currentLocation.Latitude) - offset.Latitude;
            var lon = currentLocation.Longitude > 0 ? Math.Floor(currentLocation.Longitude) + offset.Longitude : Math.Ceiling(currentLocation.Longitude) - offset.Longitude;

            var loc = new Location(lat, lon, date);
            return loc;
        }


        //Get Offset needs current location to know whether to apply 30W rule
        static Location CalculateOffset(DateTime date, string djia)
        {
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            var prehashString = $"{date.ToString("yyyy-MM-dd")}-{djia}";
            Debug.WriteLine($"Prehash string: {prehashString}");
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
            string latStr = ((part1 / 2.0) / (long.MaxValue + (ulong)1)).ToString(CultureInfo.InvariantCulture); // Some tricks are required to divide by ulong.MaxValue + 1
            string lonStr = ((part2 / 2.0) / (long.MaxValue + (ulong)1)).ToString(CultureInfo.InvariantCulture);
            Debug.WriteLine($"Offset Lat: {latStr}, Lon: {lonStr}");
            double lat, lon;
            double.TryParse(latStr, out lat);
            double.TryParse(lonStr, out lon);

            return new Location(lat, lon);
        }
    }
}
