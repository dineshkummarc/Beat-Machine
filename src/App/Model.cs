using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using BeatMachine.EchoNest;
using Microsoft.Xna.Framework.Media;
using BMCatalog = BeatMachine.EchoNest.Model.Catalog;
using BMSong = BeatMachine.EchoNest.Model.Song;

namespace App
{
    public class Model
    {
        public List<BMSong> SongsOnDevice
        {
            get;
            set;
        }

        public List<BMSong> AnalyzedSongs
        {
            get; set;
        }

        public string CatalogId
        {
            get;
            set;
        }

        public Model()
        {
            SongsOnDevice = new List<BMSong>();
            AnalyzedSongs = new List<BMSong>();
            CatalogId = String.Empty;
        }

        public static void GetSongsOnDevice(object state)
        {
            App thisApp = App.Current as App;
            List<BMSong> songs = thisApp.Model.SongsOnDevice;
            
            lock(songs){
                if (songs.Count == 0)
                {
                    using (var mediaLib = new MediaLibrary())
                    {
                        songs.AddRange(mediaLib.Songs
                            .Select<Song, BMSong>((s) => new BMSong{
                                ItemId = string
                                    .Concat(s.Artist.Name, s.Name)
                                    .Replace(" ", ""),
                                ArtistName = s.Artist.Name,
                                SongName = s.Name
                            }));
                    }
                }
            }
        }

        public static void GetAnalyzedSongs(object state)
        {
            App thisApp = App.Current as App;
            List<BMSong> songs = thisApp.Model.AnalyzedSongs;

            lock (songs)
            {
                if (songs.Count == 0)
                {


                }
            }
        }


        /// <summary>
        /// Ensures Model.CatalogId is populated. Will try the following:
        /// 1. Try loading it from the "CatalogId" setting in storage
        /// 2. If (1) is successful, it will try reading 1 song from the 
        /// catalog to make sure it is there on the web service
        /// 3. If (1) or (2) fails, it will create a new catalog on the 
        /// web service
        /// </summary>
        public static void LoadCatalogId(object state)
        {
            App thisApp = App.Current as App;
            bool loadedId = false;

            lock (thisApp.Model.CatalogId)
            {
                if (String.IsNullOrEmpty(thisApp.Model.CatalogId))
                {
                    string id;
                    if (IsolatedStorageSettings.ApplicationSettings.
                        TryGetValue<string>("CatalogId", out id))
                    {
                        thisApp.Model.CatalogId = id;
                        loadedId = true;
                    }
                }
                else
                {
                    loadedId = true;
                }
            }

            if (!loadedId)
            {
                LoadCatalogIdNeedsToCreateCatalog();
            }
            else
            {
                LoadCatalogIdNeedsToCheckCatalogId();
            }   
        }

        private static void LoadCatalogIdNeedsToCreateCatalog()
        {
            App thisApp = App.Current as App;

            thisApp.Api.CatalogCreateCompleted +=
                new EventHandler<BeatMachine.EchoNest.EchoNestApiEventArgs>(Api_CatalogCreateCompleted);
            thisApp.Api.CatalogCreateAsync(Guid.NewGuid().ToString(), "song", null);
        }


        private static void LoadCatalogIdNeedsToCheckCatalogId()
        {
            App thisApp = App.Current as App;

            thisApp.Api.CatalogReadCompleted +=
                   new EventHandler<BeatMachine.EchoNest.EchoNestApiEventArgs>(Api_CatalogReadCompleted);
            thisApp.Api.CatalogReadAsync(thisApp.Model.CatalogId, null);
        }

        static void Api_CatalogCreateCompleted(object sender, BeatMachine.EchoNest.EchoNestApiEventArgs e)
        {

            if (e.Error == null)
            {
                App thisApp = App.Current as App;

                BMCatalog cat = (BMCatalog)e.GetResultData();

                // TODO If isolated storage fails here, then the next time they 
                // run the app, we will create another catalog. We need to handle
                // this somehow - perhaps use the unique device ID (which is bad
                // practice apparently) as the catalog name to make sure there is
                // only one catalog created per device ever.

                lock (thisApp.Model.CatalogId)
                {
                    thisApp.Model.CatalogId = cat.Id;

                    // Store in isolated storage
                    IsolatedStorageSettings.ApplicationSettings["CatalogId"] =
                        thisApp.Model.CatalogId;
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }
            }
            else
            {
                // Couldn't create successfully, try again later
                LoadCatalogIdNeedsToRunAgain();
            }
        }

        static void Api_CatalogReadCompleted(object sender, BeatMachine.EchoNest.EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                if (e.Error is WebExceptionWrapper)
                {
                    // Transient network error
                    LoadCatalogIdNeedsToRunAgain();
                }
                else
                {
                    // They either didn't have a CatalogId in local storage, or
                    // they did have one, but it wasn't on the cloud service. In
                    // both cases, we create a new one
                    LoadCatalogIdNeedsToCreateCatalog();
                }
            }
            else
            {
                // This catalog exists, everything is great 
            }
        }

        private static void LoadCatalogIdNeedsToRunAgain()
        {
            ExecutionQueue.Enqueue(new WaitCallback(Model.LoadCatalogId),
                ExecutionQueue.Policy.Queued);
        }

    }
}
