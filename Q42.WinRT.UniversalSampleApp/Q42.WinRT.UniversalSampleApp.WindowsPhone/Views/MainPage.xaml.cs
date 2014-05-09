using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Q42.WinRT.UniversalSampleApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ConvertsButton_Click_1(object sender, RoutedEventArgs e)
        {
         this.Frame.Navigate(typeof(ConvertersExamplePage));
        }

        private void DataButton_Click_1(object sender, RoutedEventArgs e)
        {
          Frame.Navigate(typeof(DataExamplePage));

        }

        private void StorageButton_Click_1(object sender, RoutedEventArgs e)
        {
          Frame.Navigate(typeof(StorageExamplePage));

        }

        private void UtilButton_Click_1(object sender, RoutedEventArgs e)
        {
          Frame.Navigate(typeof(UtilExamplePage));

        }

        private void ControlsButton_Click_1(object sender, RoutedEventArgs e)
        {
          Frame.Navigate(typeof(ControlsExamplePage));

        }

        private void ParallaxButton_Click_1(object sender, RoutedEventArgs e)
        {
          Frame.Navigate(typeof(ParallaxPage));

        }
    }
}
