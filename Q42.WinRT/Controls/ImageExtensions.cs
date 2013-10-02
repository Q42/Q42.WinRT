using System;
using System.Diagnostics;
using Q42.WinRT.Data;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;
using System.IO.IsolatedStorage;
#endif

namespace Q42.WinRT.Controls
{
    /// <summary>
    /// Attached properties for Images
    /// </summary>
    public static class ImageExtensions
    {

        /// <summary>
        /// Using a DependencyProperty as the backing store for WebUri.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CacheUriProperty =
            DependencyProperty.RegisterAttached(
                "CacheUri",
                typeof(Uri),
                typeof(ImageExtensions),
                new PropertyMetadata(null, OnCacheUriChanged));

        /// <summary>
        /// Gets the CacheUri property. This dependency property 
        /// WebUri that has to be cached
        /// </summary>
        public static Uri GetCacheUri(DependencyObject d)
        {
            return (Uri)d.GetValue(CacheUriProperty);
        }

        /// <summary>
        /// Sets the CacheUri property. This dependency property 
        /// WebUri that has to be cached
        /// </summary>
        public static void SetCacheUri(DependencyObject d, Uri value)
        {
            d.SetValue(CacheUriProperty, value);
        }

        private static async void OnCacheUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            //Uri oldCacheUri = (Uri)e.OldValue;
            Uri newCacheUri = (Uri)d.GetValue(CacheUriProperty);
            var image = (Image)d;

            if (newCacheUri != null)
            {
                try
                {
                    //Get image from cache (download and set in cache if needed)
                    var cacheUri = await WebDataCache.GetLocalUriAsync(newCacheUri);

                    #if NETFX_CORE
                        //Set cache uri as source for the image
                        image.Source = new BitmapImage(cacheUri);

                    #elif WINDOWS_PHONE
                        BitmapImage bimg = new BitmapImage();

                        using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            using (IsolatedStorageFileStream stream = iso.OpenFile(cacheUri.PathAndQuery, FileMode.Open, FileAccess.Read))
                            {
                                bimg.SetSource(stream);
                            }
                        }
                        //Set cache uri as source for the image
                        image.Source = bimg;
                    #endif
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);

                    //Revert to using passed URI
                    image.Source = new BitmapImage(newCacheUri);
                }
            }
            else
                image.Source = null;

        }

    }

}
