using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q42.WinRT.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Q42.WinRT.Controls
{
    public static class ImageExtensions
    {

        // Using a DependencyProperty as the backing store for WebUri.  This enables animation, styling, binding, etc...
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

                    //Set cache uri as source for the image
                    image.Source = new BitmapImage(cacheUri);
                }
                catch (Exception)
                {
                }
            }
            else
                image.Source = null;

        }

    }

}
