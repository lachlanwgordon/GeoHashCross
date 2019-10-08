//using System;
//using System.Globalization;
//using GeohashCross.Models;
//using Xamarin.Forms;
//using Xamarin.Forms.GoogleMaps;

//namespace GeohashCross.Converters
//{
//    public class MapTappedEventArgsToLocationConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            var eventArgs = value as MapClickedEventArgs;
//            var pos = eventArgs.Point;
//            var loc = new Location(pos.Latitude, pos.Longitude);
//            return loc;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
