using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using GeohashCross.Models;
using Xamarin.Forms;

namespace GeohashCross.Converters
{
    public class LocationSummary : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var loc = value as Location;

            return loc == null ? "Loading..." : $"{loc.Latitude.ToString("N3")}, {loc.Longitude.ToString("N3")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            var latlon = str.Split(',');
            var lat = Double.Parse(latlon[0]);
            var lon = Double.Parse(latlon[1]);
            var loc = new Location(lat, lon);
            return loc;
        }
    }
}
