using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Q42.WinRT
{
    /// <summary>
    /// Various Utils
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Get application version as formatted string
        /// </summary>
        /// <returns></returns>
        public static string GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        /// <summary>
        /// Uses the WebView control to get the OS Version from the UserAgent string
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetOsVersionAsync()
        {
            string userAgent = await GetUserAgent();

            string result = string.Empty;

            //Parse user agent
            int startIndex = userAgent.ToLower().IndexOf("windows");
            if (startIndex > 0)
            {
                int endIndex = userAgent.IndexOf(";", startIndex);

                if (endIndex > startIndex)
                    result = userAgent.Substring(startIndex, endIndex - startIndex);
            }

            return result;

        }

        /// <summary>
        /// Uses WebView control to get the user agent string
        /// </summary>
        /// <returns></returns>
        private static Task<string> GetUserAgent()
        {
            var tcs = new TaskCompletionSource<string>();

            WebView webView = new WebView();

            string htmlFragment =
              @"<html>
                    <head>
                        <script type='text/javascript'>
                            function GetUserAgent() 
                            {
                                return navigator.userAgent;
                            }
                        </script>
                    </head>
                </html>";

            webView.NavigationCompleted += async (sender, e) =>
            {
                try
                {
                    //Invoke the javascript when the html load is complete
                    string result = await webView.InvokeScriptAsync("GetUserAgent", null);

                    //Set the task result
                    tcs.TrySetResult(result);
                }
                catch(Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            };

            //Load Html
            webView.NavigateToString(htmlFragment);
          
            return tcs.Task;
        }



        ///<summary>
        /// Want to store the hostname to send for push notifications to make
        /// the management UI better. Take the substring up to the first period
        /// of the first DomainName entry.
        /// 
        /// Thanks to Jeff Wilcox and Matthijs Hoekstra
        ///</summary>
        public static string GetMachineName()
        {
            var list = NetworkInformation.GetHostNames().ToArray();
            string name = null;
            if (list.Length > 0)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    var entry = list[i];
                    if (entry.Type == Windows.Networking.HostNameType.DomainName)
                    {
                        string s = entry.CanonicalName;
                        if (!string.IsNullOrEmpty(s))
                        {
                            // Domain-joined. Requires at least a one-
                            // character name.
                            int j = s.IndexOf('.');

                            if (j > 0)
                            {
                                name = s.Substring(0, j);
                                break;
                            }
                            else
                            {
                                // Typical home machine.
                                name = s;
                            }
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                // TODO: Localize?
                name = "Unknown Windows 8";
            }

            return name;

        }
       
    }
}
