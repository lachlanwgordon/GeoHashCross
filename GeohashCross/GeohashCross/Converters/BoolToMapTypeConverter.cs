using System;
using System.Globalization;
using GeohashCross.Converters;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GeohashCross.Converters
{
    [ValueConversion(typeof(bool), typeof(MapType))]
    public class BoolToMapTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool == false)
            {
                return default(MapType);
            }

            var input = (bool)value;

            // TODO: Put your value conversion logic here.

            return input ? MapType.Satellite : MapType.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}