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
    public partial class CatalogUpdate : PhoneApplicationPage
    {
        public CatalogUpdate()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EchoNestApi api = ((App)App.Current).Api;
            api.CatalogUpdateCompleted += new EventHandler<EchoNestApiEventArgs>(api_CatalogUpdateCompleted);
            Catalog cat = new Catalog
            {
                Id = catalogId.Text,
                SongActions = new List<CatalogAction<Song>> {
                    new CatalogAction<Song>{
                        Item = new Song{
                            ItemId = catalogId.Text + title.Text,
                            SongName = title.Text,
                            ArtistName = artist.Text
                        }
                    }
                }
            };
            api.CatalogUpdateAsync(cat, null);
        }

        void api_CatalogUpdateCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                result.Text = App.HandleError(e.Error);
            }
            else
            {
                result.Text = "ticket " + e.GetResultData() as string;
            }
        }
    }
}