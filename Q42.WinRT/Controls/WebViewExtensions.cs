using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Q42.WinRT.Controls
{
    public static class WebViewExtensions
    {
        // Using a DependencyProperty to use a binding to populate the webview with a HTML String instead of calling NavigateToString
        public static readonly DependencyProperty SourceHtmlProperty =
            DependencyProperty.RegisterAttached(
                "SourceHtml",
                typeof(string),
                typeof(WebViewExtensions),
                new PropertyMetadata(null, OnSourceHtmlChanged));

        /// <summary>
        /// Gets the SourceHtmlProperty property. This dependency property 
        /// </summary>
        public static string GetSourceHtml(DependencyObject d)
        {
            return (string)d.GetValue(SourceHtmlProperty);
        }

        /// <summary>
        /// Sets the SourceHtmlProperty property. This dependency property 
        /// </summary>
        public static void SetSourceHtml(DependencyObject d, string value)
        {
            d.SetValue(SourceHtmlProperty, value);
        }

        private static void OnSourceHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string newHtmlString = (string)d.GetValue(SourceHtmlProperty);
            var webView = (WebView)d;

            webView.NavigateToString(newHtmlString);

        }

    }

}
