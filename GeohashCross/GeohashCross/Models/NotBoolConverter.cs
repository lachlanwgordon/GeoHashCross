using System;
using System.Globalization;
using Xamarin.Forms;

namespace GeohashCross.Models
{
    public class NotBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isBool = (bool)value;
            return !isBool;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isBool = (bool)value;
            return !isBool;
        }
    }
}
