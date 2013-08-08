using System;

#if NETFX_CORE
using Windows.UI.Xaml.Data;
#elif WINDOWS_PHONE
using System.Windows.Data;
#endif

namespace Q42.WinRT.Converters
{
    /// <summary>
    /// Converts Bytes to Strings
    /// Copied from Silverlight Multi File Uploader project:
    /// http://slfileupload.codeplex.com
    /// </summary>
    public class ByteToStringConverter : IValueConverter
    {
        /// <summary>
        ///  Converts Bytes to Strings
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
            string size = "0 KB";

            if (value != null)
            {

                double byteCount = 0;

                byteCount = System.Convert.ToDouble(value);
                    

                if (byteCount >= 1073741824)
                    size = String.Format("{0:##.##}", byteCount / 1073741824) + " GB";
                else if (byteCount >= 1048576)
                    size = String.Format("{0:##.##}", byteCount / 1048576) + " MB";
                else if (byteCount >= 1024)
                    size = String.Format("{0:##.##}", byteCount / 1024) + " KB";
                else if (byteCount > 0 && byteCount < 1024)
                    size = "1 KB";    //Bytes are unimportant ;)            

            }

            return size;

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
