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
using BeatMachine.Model;

namespace BeatMachine
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadSongs();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadSongs(); 
        }

        private void LoadSongs()
        {
            BeatMachineDataContext context = new BeatMachineDataContext(
              BeatMachineDataContext.DBConnectionString);

            var songs = context.AnalyzedSongs.ToList();

            songsHeader.Header = String.Format("songs ({0})", songs.Count);

            if (songs.Count > 0)
            {
                result.ItemsSource = songs;
            }
            else
            {
                result.ItemsSource = new List<string> { "No analyzed songs available" };
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BeatMachineDataContext context = new BeatMachineDataContext(
                BeatMachineDataContext.DBConnectionString);

            context.AnalyzedSongs.DeleteAllOnSubmit(
                context.AnalyzedSongs.ToList());
            context.SubmitChanges();
        }
    }
}