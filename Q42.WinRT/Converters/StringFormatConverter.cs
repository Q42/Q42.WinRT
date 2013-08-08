using System;
using Windows.UI.Xaml.Data;

namespace Q42.WinRT.Converters
{
    /// <summary>
    /// Does a simple String format
    /// </summary>
    public class StringFormatConverter : IValueConverter
    {
        /// <summary>
        /// String.Format Converter
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //Check input params
            if (parameter == null || value == null)
                return value;

            return string.Format((string)parameter, value);
        }

        /// <summary>
        /// Returns the value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

}
