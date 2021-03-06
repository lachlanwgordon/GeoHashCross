﻿using GeohashCross.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Location = GeohashCross.Models.Location;

namespace GeohashCross.Services
{
    public class Hasher
    {
        public static async Task<Response<HashLocation>> GetHashData(DateTime date, Location currentLocation)
        {
            var date30W = DowJonesDates.Get30WCompliantDate(date, currentLocation);
            var djDate = DowJonesDates.GetApplicableDJDate(date30W);
            var djia = await Webclient.GetDjia(djDate);

            if (!djia.Success)
            {
                return new Response<HashLocation>(null, false, djia.Message);
            }
            var offset = CalculateOffset(date, djia.Data);
            var loc = CalculateHashLocation(currentLocation, offset);

            var hash = new HashLocation(loc.Latitude, loc.Longitude, date, false, false);

            return new Response<HashLocation>(hash, true, "Hashes calculated successfully");
        }

        private static Location CalculateGlobalHash(Location offset)
        {
            var latDecimalPart = offset.Latitude;
            var lonDecimalPart = offset.Longitude;

            var globalLat = latDecimalPart * 180 - 90;
            var globalLon = lonDecimalPart * 360 - 180;

            var loc = new Location(globalLat, globalLon);
            return loc;
        }

        //Get Offset needs current location to know whether to apply 30W rule
        public static Location CalculateHashLocation(Location currentLocation, Location offset)
        {
            var lat = currentLocation.Latitude > 0 ? Math.Floor(currentLocation.Latitude) + offset.Latitude : Math.Ceiling(currentLocation.Latitude) - offset.Latitude;
            var lon = currentLocation.Longitude > 0 ? Math.Floor(currentLocation.Longitude) + offset.Longitude : Math.Ceiling(currentLocation.Longitude) - offset.Longitude;

            var loc = new Location(lat, lon);
            return loc;
        }


        public static Location CalculateOffset(DateTime date, string djia)
        {
            var prehashString = $"{date.ToString("yyyy-MM-dd")}-{djia}";
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
            double lat, lon;
            double.TryParse(latStr, out lat);
            double.TryParse(lonStr, out lon);

            return new Location(lat, lon);
        }
    }
}
