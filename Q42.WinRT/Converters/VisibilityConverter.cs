using System;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#elif WINDOWS_PHONE
using System.Windows.Data;
using System.Windows;
#endif

namespace Q42.WinRT.Converters
{
    /// <summary>
    /// Converts anything to inverse visibility
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts anything to inverse visibility
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
#if NETFX_CORE
        public object Convert(object value, Type targetType, object parameter, string language)
#elif WINDOWS_PHONE
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
#endif
      {
#if DEBUG
            //Always return true for the designer, for easy blend support
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return Visibility.Visible;
#endif
            bool visible = true;
            if (value is Visibility)
            {
                return value;
            } 
            else if (value is bool)
            {
                visible = (bool)value;
            }
            else if (value is int || value is short || value is long)
            {
                visible = 0 != (int)value;
            }
            else if (value is float || value is double)
            {
                visible = 0.0 != (double)value;
            }
            else if (value is string && string.IsNullOrEmpty((string)value))
            {
                visible = false;
            }
            else if (value == null)
            {
                visible = false;
            }
            if ((string)parameter == "!") //Inverse visibility
            {
                visible = !visible;
            }

            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// NotImplementedException
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
#if NETFX_CORE
        public object ConvertBack(object value, Type targetType, object parameter, string language)
#elif WINDOWS_PHONE
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
#endif
        {
          throw new NotImplementedException();
        }
    }
}
