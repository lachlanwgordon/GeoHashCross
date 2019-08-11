using System;
using System.Globalization;
using GeohashCross.Converters;
using Xamarin.Forms;


namespace GeohashCross.Converters
{
    [ValueConversion(typeof(double?), typeof(string))]
    public class NullableDoubleToDistanceStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double? == false)
            {
                return default(object);
            }

            var distance = (double?)value;

            if (!distance.HasValue)
                return "Calculating...";
            if (distance < 1)
                return (distance.Value * 1000).ToString("N2") + "m";
            else
                return distance.Value.ToString("N2") + "Km";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}