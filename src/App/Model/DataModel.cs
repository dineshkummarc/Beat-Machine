using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using BeatMachine.EchoNest;
using System.ComponentModel;
using System.Collections.ObjectModel;
using BeatMachine.EchoNest.Model;
using XnaSong = Microsoft.Xna.Framework.Media.Song;
using XnaMediaLibrary = Microsoft.Xna.Framework.Media.MediaLibrary;


namespace BeatMachine.Model
{
    public class DataModel : INotifyPropertyChanged
    {
        private List<Song> songsOnDevice;
        public List<Song> SongsOnDevice
        {
            get { return songsOnDevice; }
            set
            {
                songsOnDevice = value; 
                OnPropertyChanged("SongsOnDevice"); 
            }
        }

        private List<Song> analyzedSongs;
        public List<Song> AnalyzedSongs
        {
            get { return analyzedSongs; }
            set {
                analyzedSongs = value;
                OnPropertyChanged("AnalyzedSongs"); 
            }
        }

        private string catalogId;
        public string CatalogId
        {
            get { return catalogId; }
            set
            {
                catalogId = value;
                OnPropertyChanged("CatalogId");
            }
        }

        public DataModel()
        {
            SongsOnDevice = new List<Song>();
            AnalyzedSongs = new List<Song>();
            CatalogId = String.Empty;
        }

        public static void GetSongsOnDevice(object state)
        {
            App thisApp = App.Current as App;
            List<Song> songs = thisApp.Model.SongsOnDevice;
            
            lock(songs){
                if (songs.Count == 0)
                {
                    using (var mediaLib = new XnaMediaLibrary())
                    {
                        songs.AddRange(mediaLib.Songs
                            .Select<XnaSong, Song>((s) => new Song{
                                ItemId = string
                                    .Concat(s.Artist.Name, s.Name)
                                    .Replace(" ", ""),
                                ArtistName = s.Artist.Name,
                                SongName = s.Name
                            }));
                    }

                    thisApp.Model.OnPropertyChanged("SongsOnDevice");
                }
            }

        }

        public static void GetAnalyzedSongs(object state)
        {
            App thisApp = App.Current as App;
            List<Song> songs = thisApp.Model.AnalyzedSongs;

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
            string id;

            lock (thisApp.Model.CatalogId)
            {
                if (String.IsNullOrEmpty(thisApp.Model.CatalogId))
                {
                    if (IsolatedStorageSettings.ApplicationSettings.
                        TryGetValue<string>("CatalogId", out id))
                    {
                        loadedId = true;
                    }
                }
                else
                {
                    loadedId = true;
                    id = thisApp.Model.CatalogId;
                }
            }

            if (!loadedId)
            {
                LoadCatalogIdNeedsToCreateCatalog();
            }
            else
            {
                LoadCatalogIdNeedsToCheckCatalogId(id);
            }   
        }

        private static void LoadCatalogIdNeedsToCreateCatalog()
        {
            App thisApp = App.Current as App;

            thisApp.Api.CatalogCreateCompleted +=
                new EventHandler<EchoNestApiEventArgs>(
                    Api_CatalogCreateCompleted);
            thisApp.Api.CatalogCreateAsync(Guid.NewGuid().ToString(), "song", 
                null, null);
        }


        private static void LoadCatalogIdNeedsToCheckCatalogId(string id)
        {
            App thisApp = App.Current as App;

            thisApp.Api.CatalogReadCompleted +=
                   new EventHandler<EchoNestApiEventArgs>(
                       Api_CatalogReadCompleted);
            thisApp.Api.CatalogReadAsync(id, null, null);
        }

        static void Api_CatalogCreateCompleted(object sender, EchoNestApiEventArgs e)
        {

            if (e.Error == null)
            {
                App thisApp = App.Current as App;

                Catalog cat = (Catalog)e.GetResultData();

                // TODO If isolated storage fails here, then the next time they 
                // run the app, we will create another catalog. We need to handle
                // this somehow - perhaps use the unique device ID (which is bad
                // practice apparently) as the catalog name to make sure there is
                // only one catalog created per device ever.

                lock (thisApp.Model.CatalogId)
                {
                    // Store in isolated storage
                    IsolatedStorageSettings.ApplicationSettings["CatalogId"] =
                        cat.Id;
                    IsolatedStorageSettings.ApplicationSettings.Save();

                    thisApp.Model.CatalogId = cat.Id;
                }
            }
            else
            {
                // Couldn't create successfully, try again later
                LoadCatalogIdNeedsToRunAgain();
            }
        }

        static void Api_CatalogReadCompleted(object sender, 
            EchoNestApiEventArgs e)
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
                App thisApp = App.Current as App;
                Catalog cat = (Catalog)e.GetResultData();
                lock (thisApp.Model.CatalogId)
                {
                    thisApp.Model.CatalogId = cat.Id;
                }
            }
        }

        private static void LoadCatalogIdNeedsToRunAgain()
        {
            ExecutionQueue.Enqueue(new WaitCallback(DataModel.LoadCatalogId),
                ExecutionQueue.Policy.Queued);
        }


        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
