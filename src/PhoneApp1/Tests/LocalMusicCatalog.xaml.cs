
namespace PhoneApp1.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using BeatMachine.EchoNest;
    using BeatMachine.EchoNest.Model;
    using Microsoft.Phone.Controls;
    using Microsoft.Xna.Framework.Media;
    using BMSong = BeatMachine.EchoNest.Model.Song;

    public partial class LocalMusicCatalog : PhoneApplicationPage
    {
        public LocalMusicCatalog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var catalogID = "CAJSYHH1369B16F57D";
            var api = ((App)App.Current).Api;
            api.CatalogUpdateCompleted += new EventHandler<EchoNestApiEventArgs>(api_CatalogUpdateCompleted);

            var songsActions = new List<CatalogAction<BMSong>>();
            using (var mediaLib = new MediaLibrary())
            {
                foreach (var song in mediaLib.Songs)
                {
                    var catalogAction = new CatalogAction<BMSong>();
                    catalogAction.Action = CatalogAction<BMSong>.ActionType.update;
                    catalogAction.Item = new BMSong
                    {
                        ItemId = Guid.NewGuid().ToString("D"),
                        ArtistName = song.Artist.Name,
                        SongName = song.Name,
                    };

                    songsActions.Add(catalogAction);
                }
            }

            var catalog = new Catalog();
            catalog.Id = catalogID;
            catalog.SongActions = songsActions;

            api.CatalogUpdateAsync(catalog, null);
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