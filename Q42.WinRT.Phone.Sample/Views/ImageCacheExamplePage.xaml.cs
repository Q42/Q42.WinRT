using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Q42.WinRT.Data;

namespace Q42.WinRT.Phone.Sample.Views
{
    public partial class ImageCacheExamplePage : PhoneApplicationPage
    {
        public ImageCacheExamplePage()
        {
            InitializeComponent();
        }

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			WebDataCache.Delete(new Uri("https://www.google.com/images/logo.png"));
		}
	}
}