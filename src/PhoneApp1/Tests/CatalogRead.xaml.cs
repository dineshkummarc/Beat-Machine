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
    public partial class CatalogRead : PhoneApplicationPage
    {
        public CatalogRead()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EchoNestApi api = ((App)App.Current).Api;
            api.CatalogReadCompleted += new EventHandler<EchoNestApiEventArgs>(api_CatalogReadCompleted);
            api.CatalogReadAsync(catalogId.Text, null);
        }

        void api_CatalogReadCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                result.Text = App.HandleError(e.Error);
            }
            else
            {

                Catalog cat = (Catalog)e.GetResultData();
                result.Text = cat.Items
                   .Select<Song, string>((so) => 
                       String.Format("{0} by {1} at {2} BPM", 
                       so.Title, 
                       so.ArtistName, 
                       so.AudioSummary.Tempo))
                   .Aggregate<string>((sofar, current) =>
                       sofar + Environment.NewLine + current);
            }
        }
    }
}