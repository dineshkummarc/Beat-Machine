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
using System.Windows.Data;

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
                    {"bucket", "audio_summary"},
                    {"results", "100"}
                }, null);
        }

        void api_CatalogReadCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                result.ItemsSource = new List<string> { App.HandleError(e.Error) };
            }
            else
            {
                Catalog cat = (Catalog)e.GetResultData();
                if (cat.Items.Count > 0)
                {
                    result.ItemsSource = cat.Items;
                }
                else 
                {
                    result.ItemsSource = new List<string> { "Empty catalog " };
                }
            }
        }
    }

}