using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Q42.WinRT.Converters
{
    /// <summary>
    /// Source: http://w8isms.blogspot.nl/2012/06/metro-parallax-background-in-xaml.html
    /// Used to create background parallex like on the Win8 start screen
    /// </summary>
    public class ParallaxConverter : IValueConverter
    {
        const double _defaultFactor = -0.10;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double factor = _defaultFactor;


            if (parameter != null && parameter is string
                && double.TryParse((string)parameter, NumberStyles.Any, CultureInfo.InvariantCulture, out factor))
                    factor = double.Parse((string)parameter, NumberStyles.Any, CultureInfo.InvariantCulture);

            if (value is double)
            {
                return (double)value * factor;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
