using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using BeatMachine.EchoNest;
using BeatMachine.EchoNest.Model;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Tests/SongSearch.xaml", UriKind.Relative));
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Tests/CatalogList.xaml", UriKind.Relative));
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Tests/CatalogCreate.xaml", UriKind.Relative));
        }



        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Tests/CatalogUpdate.xaml", UriKind.Relative));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Tests/CatalogStatus.xaml", UriKind.Relative));
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Tests/CatalogRead.xaml", UriKind.Relative));
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Tests/LocalMusicCatalog.xaml", UriKind.Relative));
        }

    }
}