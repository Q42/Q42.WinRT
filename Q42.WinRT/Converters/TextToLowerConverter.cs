using System;
using Windows.UI.Xaml.Data;

namespace Q42.WinRT.Converters
{
    /// <summary>
    /// Converts text to lowercase
    /// </summary>
    public class TextToLowerConverter : IValueConverter
    {
        /// <summary>
        /// Returns value to lowercase
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((string)value).ToLower();
        }

        /// <summary>
        /// Returns input value
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
