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

namespace PhoneApp1.Tests
{
    public partial class SongSearch : PhoneApplicationPage
    {
        public SongSearch()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EchoNestApi api = ((App)App.Current).Api;
            api.SongSearchCompleted += new EventHandler<EchoNestApiEventArgs>(api_SearchSongCompleted);
            api.SongSearchAsync(new Dictionary<string, string>
                {
                    { "title", title.Text},
                    { "artist", artist.Text }
                });

        }

        void api_SearchSongCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                result.Text = App.HandleError(e.Error);
            }
            else
            {
                result.Text = ((List<Song>)e.GetResultData())
                    .Select<Song, string>((so) => so.Title + " by " + so.ArtistName)
                    .Aggregate<string>((sofar, current) =>
                        sofar + Environment.NewLine + current);
            }
        }

    }
}