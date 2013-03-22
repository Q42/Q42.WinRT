using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Q42.WinRT.Converters
{
    /// <summary>
    /// Converts anything to inverse visibility
    /// I know the VisibilityConvert supports the input parameter !. But it's easier to have two converters
    /// </summary>
    public class InverseVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts anything to inverse visibility
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {

#if DEBUG
            //Always return true for the designer, for easy blend support
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return Visibility.Visible;
#endif
        
            bool visible = true;
            if (value is Visibility)
            {
                visible = (Visibility.Visible == (Visibility)value);
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
            if ((string)parameter == "!")
            {
                visible = !visible;
            }

            return visible ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// NotImplementedException
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
