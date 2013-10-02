using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Q42.WinRT.Phone.Sample.Resources;

namespace Q42.WinRT.Phone.Sample
{
  public partial class MainPage : PhoneApplicationPage
  {
    // Constructor
    public MainPage()
    {
      InitializeComponent();

    }

    private void DataButton_Click_1(object sender, RoutedEventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/Views/DataExamplePage.xaml", UriKind.Relative));

    }

    private void StorageButton_Click_1(object sender, RoutedEventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/Views/StorageExamplePage.xaml", UriKind.Relative));
    }

    private void ConvertsButton_Click_1(object sender, RoutedEventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/Views/ConvertersExamplePage.xaml", UriKind.Relative));

    }

    private void ImageCacheButton_Click_1(object sender, RoutedEventArgs e)
    {
        this.NavigationService.Navigate(new Uri("/Views/ImageCacheExamplePage.xaml", UriKind.Relative));

    }

  }
}