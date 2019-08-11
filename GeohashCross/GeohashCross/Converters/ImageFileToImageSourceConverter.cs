using System;
using System.Globalization;
using System.IO;
using GeohashCross.Converters;
using Xamarin.Forms;


namespace GeohashCross.Converters
{
    [ValueConversion(typeof(ImageSource), typeof(FileInfo))]
    public class ImageFileToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ImageSource == false)
            {
                return default(FileInfo);
            }

            var input = (ImageSource)value;

            // TODO: Put your value conversion logic here.

            var path = (string)value;
            return ImageSource.FromFile(path);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}