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
    public partial class CatalogList : PhoneApplicationPage
    {
        public CatalogList()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EchoNestApi api = ((App)App.Current).Api;
            api.CatalogListCompleted += new EventHandler<EchoNestApiEventArgs>(api_CatalogListCompleted);
            api.CatalogListAsync(null, null);

        }

        void api_CatalogListCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                result.Text = App.HandleError(e.Error);
            }
            else
            {
                result.Text = ((List<Catalog>)e.GetResultData())
                    .Select<Catalog, string>((cat) => cat.Name + " with ID " + cat.Id)
                    .Aggregate<string>((sofar, current) =>
                        sofar + Environment.NewLine + current);
            }
        }

    }
}