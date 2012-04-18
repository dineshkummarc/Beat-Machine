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
    public partial class CatalogDelete : PhoneApplicationPage
    {
        public CatalogDelete()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EchoNestApi api = ((App)App.Current).Api;
            api.CatalogDeleteCompleted += new EventHandler<EchoNestApiEventArgs>(api_CatalogDeleteCompleted);
            api.CatalogDeleteAsync(catalogId.Text, null);
        }

        void api_CatalogDeleteCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                result.Text = App.HandleError(e.Error);
            }
            else
            {
                Catalog cat = (Catalog)e.GetResultData();
                result.Text = "Deleted " + cat.Name + " with ID " + cat.Id;
            }
        }
    }
}