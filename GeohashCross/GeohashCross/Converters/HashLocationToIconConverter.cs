using System;
using System.Globalization;
using GeohashCross.Converters;
using GeohashCross.Models;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GeohashCross.Converters
{
    [ValueConversion(typeof(HashLocation), typeof(BitmapDescriptor))]
    public class HashLocationToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HashLocation == false)
            {
                return default(BitmapDescriptor);
            }

            var input = (HashLocation)value;

            // TODO: Put your value conversion logic here.

            Color pinColor;
            if(input.IsGlobal)//Global Hash
            {
                pinColor = Color.Blue;
            }
            else if(input.Date < DateTime.Today)//Past Hash
            {
                pinColor = Color.Yellow;
            }
            else if(input.Date > DateTime.Today.AddDays(1))//After tomorrow
            {
                pinColor = Color.Fuchsia;
            }
            else if (input.Date > DateTime.Today)//Tomorrow
            {
                pinColor = Color.Green;
            }
            else//Normal hash today
            {
                pinColor = Color.Red;
            }


            return BitmapDescriptorFactory.DefaultMarker(pinColor);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}