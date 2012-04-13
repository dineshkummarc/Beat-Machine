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
    public partial class CatalogStatus : PhoneApplicationPage
    {
        public CatalogStatus()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EchoNestApi api = ((App)App.Current).Api;
            api.CatalogStatusCompleted += new EventHandler<EchoNestApiEventArgs>(api_CatalogStatusCompleted);
            api.CatalogStatusAsync(ticketId.Text, null);
        }

        void api_CatalogStatusCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                result.Text = App.HandleError(e.Error);
            }
            else
            {
                Ticket t = e.GetResultData() as Ticket;
                result.Text = String.Format("ticket {0} at {1}% updated {2} items", 
                    t.Status, t.PercentComplete, t.ItemsUpdated);
            }
        }
    }
}