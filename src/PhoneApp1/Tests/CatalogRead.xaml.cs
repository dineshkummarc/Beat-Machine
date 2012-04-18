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
using System.Text;

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
            api.CatalogReadAsync(catalogId.Text, new Dictionary<string, string>
                {
                    {"bucket", "audio_summary"}
                });
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
                if(cat.Items.Count > 0)
                {
                result.Text = cat.Items
                   .Select<Song, string>((so) => DisplaySong(so))
                   .Aggregate<string>((sofar, current) =>
                       sofar + Environment.NewLine + current);
                } else {
                    result.Text = "Empty catalog";
                }
            }
        }

        string DisplaySong(Song s)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} by {1}", s.SongName ?? "?", s.ArtistName ?? "?");
            sb.AppendLine();
            if (s.AudioSummary != null)
            {
                sb.AppendFormat("BPM: {0} ", s.AudioSummary.Tempo);
                sb.AppendFormat("Dan: {0} ", s.AudioSummary.Danceability);
                sb.AppendFormat("Dur: {0} ", s.AudioSummary.Duration);
                sb.AppendFormat("Ene: {0} ", s.AudioSummary.Energy);
                sb.AppendFormat("Key: {0} ", s.AudioSummary.Key);
                sb.AppendFormat("Lou: {0} ", s.AudioSummary.Loudness);
                sb.AppendFormat("Mod: {0} ", s.AudioSummary.Mode);
                sb.AppendFormat("Sig {0} ", s.AudioSummary.TimeSignature);
                sb.AppendLine();
            }
            return sb.ToString();

        }
    }
}