using System;
using System.Globalization;
using GeohashCross.Converters;
using GeohashCross.Models;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GeohashCross.Converters
{
    [ValueConversion(typeof(HashLocation), typeof(Position))]
    public class HashLocationToPositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HashLocation == false)
            {
                return default(Position);
            }

            var input = (HashLocation)value;

            // TODO: Put your value conversion logic here.


            var pos = new Position(input.Latitude, input.Longitude);

            return pos;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}