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
        private EchoNestApi api;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            api = new EchoNestApi("R2O4VVBVN5EFMCJRP");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            api.SongSearchCompleted += new EventHandler<EchoNestApiEventArgs>(api_SearchSongCompleted);
            api.SongSearchAsync(new Dictionary<string, string>
                {
                    { "title", "karma police" },
                    { "artist", "radiohead" }
                });

        }

        void api_CatalogListCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                HandleError(e.Error);
            }
            else
            {
                result.Text = ((List<Catalog>)e.GetResultData())
                    .Select<Catalog, string>((cat) => cat.Id)
                    .Aggregate<string>((sofar, current) =>
                        sofar + " " + current);
            }
        }

        void api_SearchSongCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                HandleError(e.Error);
            }
            else
            {
                result.Text = ((List<Song>)e.GetResultData())
                    .Select<Song, string>((so) => so.Title)
                    .Aggregate<string>((sofar, current) =>
                        sofar + " " + current);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            api.CatalogCreateCompleted += new EventHandler<EchoNestApiEventArgs>(api_CatalogCreateCompleted);
            api.CatalogCreateAsync("foo2", "song", null);
        }

        void api_CatalogCreateCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                HandleError(e.Error);
            }
            else
            {
                result.Text = ((Catalog)e.GetResultData()).Id;
            }
        }

        private void HandleError(Exception e)
        {
            if (e is EchoNestApiException)
            {
                EchoNestApiException ex = e as EchoNestApiException;
                result.Text = ex.Code + " " + ex.Message;
            }
            else
            {
                result.Text = e.Message;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            api.CatalogListCompleted += new EventHandler<EchoNestApiEventArgs>(api_CatalogListCompleted);
            api.CatalogListAsync(null);

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            api.CatalogUpdateCompleted += new EventHandler<EchoNestApiEventArgs>(api_CatalogUpdateCompleted);
            api.CatalogUpdateSongAsync("CAACONL1366197A7EC", new List<CatalogAction<Song>> {
                new CatalogAction<Song>{
                    Item = new Song{
                        ItemId = "foo127",
                        SongName = "holiday",
                        ArtistName = "madonna"
                    }
                },
                new CatalogAction<Song>{
                    Item = new Song{
                        ItemId = "foo128",
                        SongName = "candy",
                        ArtistName = "madonna"
                    }
                }
            }, null);
        }

        void api_CatalogUpdateCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                HandleError(e.Error);
            }
            else
            {
                result.Text = e.GetResultData() as string;
            }
        }

    }
}