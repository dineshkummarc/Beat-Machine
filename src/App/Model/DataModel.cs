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

// TODO Add a logger for debuggin

// TODO Figure out of all the locks are needed

// TODO Unit tests


namespace BeatMachine.Model
{
    public class DataModel : INotifyPropertyChanged
    {
        private static int uploadTake = 300;
        private static int uploadSkip = 0;

        // More than 100 will fail due to EchoNest hardcoded limit
        private static int downloadTake = 100;
        private static int downloadSkip = 0;
        

        private List<AnalyzedSong> songsOnDevice;
        public List<AnalyzedSong> SongsOnDevice
        {
            get { return songsOnDevice; }
            set
            {
                songsOnDevice = value; 
                OnPropertyChanged("SongsOnDevice"); 
            }
        }

        private bool songsOnDeviceLoaded;
        public bool SongsOnDeviceLoaded
        {
            get { return songsOnDeviceLoaded; }
            set
            {
                songsOnDeviceLoaded = value;
                OnPropertyChanged("SongsOnDeviceLoaded");
            }
        }

        private List<AnalyzedSong> songsToAnalyze;
        public List<AnalyzedSong> SongsToAnalyze
        {
            get { return songsToAnalyze; }
            set
            {
                songsToAnalyze = value;
                OnPropertyChanged("SongsToAnalyze");
            }
        }

        private bool songsToAnalyzeLoaded;
        public bool SongsToAnalyzeLoaded
        {
            get { return songsToAnalyzeLoaded; }
            set
            {
                songsToAnalyzeLoaded = value;
                OnPropertyChanged("SongsToAnalyzeLoaded");
            }
        }

        private bool songsToAnalyzeBatchUploadReady;
        public bool SongsToAnalyzeBatchUploadReady
        {
            get { return songsToAnalyzeBatchUploadReady; }
            set
            {
                songsToAnalyzeBatchUploadReady = value;
                OnPropertyChanged("SongsToAnalyzeBatchUploadReady");
            }
        }

        private bool songsToAnalyzeBatchDownloadReady;
        public bool SongsToAnalyzeBatchDownloadReady
        {
            get { return songsToAnalyzeBatchDownloadReady; }
            set
            {
                songsToAnalyzeBatchDownloadReady = value;
                OnPropertyChanged("SongsToAnalyzeBatchDownloadReady");
            }
        }

        private int songsToAnalyzeBatchSize;
        public int SongsToAnalyzeBatchSize
        {
            get { return songsToAnalyzeBatchSize; }
            set
            {
                songsToAnalyzeBatchSize = value;
                OnPropertyChanged("SongsToAnalyzeBatchSize");
            }
        }

        private List<AnalyzedSong> analyzedSongs;
        public List<AnalyzedSong> AnalyzedSongs
        {
            get { return analyzedSongs; }
            set {
                analyzedSongs = value;
                OnPropertyChanged("AnalyzedSongs"); 
            }
        }

        private bool analyzedSongsLoaded;
        public bool AnalyzedSongsLoaded
        {
            get { return analyzedSongsLoaded; }
            set
            {
                analyzedSongsLoaded = value;
                OnPropertyChanged("AnalyzedSongsLoaded");
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
            SongsOnDevice = new List<AnalyzedSong>();
            SongsOnDeviceLoaded = false;

            AnalyzedSongs = new List<AnalyzedSong>();
            AnalyzedSongsLoaded = false;

            SongsToAnalyze = new List<AnalyzedSong>();
            SongsToAnalyzeLoaded = false;

            CatalogId = String.Empty;
        }

        public static void GetSongsOnDevice(object state)
        {
            App thisApp = App.Current as App;
            List<AnalyzedSong> songs = thisApp.Model.SongsOnDevice;
            Dictionary<string, AnalyzedSong> uniqueSongs =
                new Dictionary<string, AnalyzedSong>();

            lock (songs)
            {
                using (var mediaLib = new XnaMediaLibrary())
                {
                    foreach(XnaSong s in mediaLib.Songs)
                    {
                        string id = string.Concat(s.Artist.Name, s.Name)
                            .Replace(" ", "");
                        uniqueSongs[id] = new AnalyzedSong
                        {
                            ItemId = id,
                            ArtistName = s.Artist.Name,
                            SongName = s.Name
                        };
                        
                    }
                    
                    songs.AddRange(uniqueSongs.Values
                        .OrderBy(s => s.ArtistName)
                        .ThenBy(s => s.SongName));
                }

                thisApp.Model.SongsOnDeviceLoaded = true;

            }
        }

        public static void GetAnalyzedSongs(object state)
        {
            App thisApp = App.Current as App;
            List<AnalyzedSong> songs = thisApp.Model.AnalyzedSongs;
            BeatMachineDataContext context = new BeatMachineDataContext(
                BeatMachineDataContext.DBConnectionString);


            lock (songs)
            {
                var loadedSongs = from AnalyzedSong song
                                      in context.AnalyzedSongs
                                  select song;

                songs.AddRange(loadedSongs.ToArray<AnalyzedSong>());

                thisApp.Model.AnalyzedSongsLoaded = true;
            }
        }

        public static void DiffSongs(object state)
        {
            App thisApp = App.Current as App;
            List<AnalyzedSong> analyzedSongs = thisApp.Model.AnalyzedSongs;
            List<AnalyzedSong> songsOnDevice = thisApp.Model.SongsOnDevice;
            List<AnalyzedSong> songsToAnalyze = thisApp.Model.SongsToAnalyze;
         
            lock (analyzedSongs)
            {       
                lock (songsOnDevice)
                {
                    lock (songsToAnalyze)
                    {
                        List<string> analyzedSongIds = analyzedSongs
                            .Select<AnalyzedSong, string>((x) => x.SongId).ToList<string>();

                        if (analyzedSongs.Count != songsOnDevice.Count)
                        {
                            foreach (AnalyzedSong song in songsOnDevice)
                            {
                                if (!analyzedSongIds.Contains(song.SongId))
                                {
                                    songsToAnalyze.Add(song);
                                }
                            }

                            thisApp.Model.SongsToAnalyzeLoaded = true;
                        }
                    }
                }
            }

        }

        private static EventHandler<EchoNestApiEventArgs> updateHandler =
            new EventHandler<EchoNestApiEventArgs>(Api_CatalogUpdateCompleted);

        public static void AnalyzeSongs(object state)
        {
            App thisApp = App.Current as App;
            if (thisApp.Model.SongsToAnalyzeBatchDownloadReady)
            {
                thisApp.Model.SongsToAnalyzeBatchDownloadReady = false;
            }
            List<AnalyzedSong> songsToAnalyze = thisApp.Model.SongsToAnalyze;

            lock (songsToAnalyze)
            {
               
                // TODO Find a better way to remove all handlers from an event
                thisApp.Api.CatalogUpdateCompleted -= updateHandler;
                thisApp.Api.CatalogUpdateCompleted -= updateHandler1;
                thisApp.Api.CatalogUpdateCompleted += updateHandler;

                List<CatalogAction<Song>> list = songsToAnalyze
                    .Skip(uploadSkip * uploadTake)
                    .Take(uploadTake)
                    .Select<AnalyzedSong, CatalogAction<Song>>(
                    (s) =>
                    {
                        return new CatalogAction<Song>
                        {
                            Item = (Song)s
                        };
                    })
                    .ToList();

                thisApp.Model.SongsToAnalyzeBatchSize = list.Count;

                if (thisApp.Model.SongsToAnalyzeBatchSize != 0)
                {
                    thisApp.Api.CatalogUpdateAsync(new Catalog
                    {
                        Id = thisApp.Model.CatalogId,
                        SongActions = list,
                    }, null, null);
                }
            } 
        }

        static void Api_CatalogUpdateCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error == null)
            {
                App thisApp = App.Current as App;

                uploadSkip++;
                thisApp.Model.SongsToAnalyzeBatchUploadReady = true;

            }
            else
            {
                // AnalyzeSongs needs to run again
                ExecutionQueue.Enqueue(new WaitCallback(DataModel.AnalyzeSongs),
                    ExecutionQueue.Policy.Queued);
            }
        }
    

        public static void DownloadAnalyzedSongs(object state)
        {
            App thisApp = App.Current as App;
            if (thisApp.Model.songsToAnalyzeBatchUploadReady)
            {
                // First time around in this batch download
                downloadSkip = 0;
                thisApp.Model.songsToAnalyzeBatchUploadReady = false;
            };

            EventHandler<EchoNestApiEventArgs> handler =
                new EventHandler<EchoNestApiEventArgs>(Api_CatalogReadCompleted);
            thisApp.Api.CatalogReadCompleted -= handler;
            thisApp.Api.CatalogReadCompleted += handler;

            thisApp.Api.CatalogReadAsync(thisApp.Model.CatalogId,
                new Dictionary<string, string>
            {
                {"bucket", "audio_summary"},
                {"results", downloadTake.ToString()},
                {"start", downloadSkip.ToString()} 

            }, null);
        }

        static void Api_CatalogReadCompleted(object sender, EchoNestApiEventArgs e)
        {
            if (e.Error == null)
            {
                App thisApp = App.Current as App;
                BeatMachineDataContext context = new BeatMachineDataContext(
                    BeatMachineDataContext.DBConnectionString);
                
                Catalog cat = (Catalog)e.GetResultData();

                // TODO This check doesn't work well, it won't terminate 
                // especially in the case where the catalog has more items that
                // the client doesn't know about

                if (!(cat.Items.Count == 0 &&
                    context.AnalyzedSongs.Count() ==
                    thisApp.Model.SongsToAnalyzeBatchSize))
                {
                    context.AnalyzedSongs.InsertAllOnSubmit(
                        cat.Items.Select<Song, AnalyzedSong>(
                        s => new AnalyzedSong
                        {
                            ItemId = s.Request.ItemId,
                            ArtistName = s.ArtistName ?? s.Request.ArtistName,
                            SongName = s.SongName ?? s.Request.SongName,
                            AudioSummary = s.AudioSummary != null ?
                                new AnalyzedSong.Summary {
                                    Tempo = s.AudioSummary.Tempo
                                } : null
                        }
                       ));
                    context.SubmitChanges();

                    downloadSkip = context.AnalyzedSongs.Count();

                    DownloadAnalyzedSongsNeedsToRunAgain();
                }
                else
                {
                    thisApp.Model.SongsToAnalyzeBatchDownloadReady = true;
                }

            } else {
                DownloadAnalyzedSongsNeedsToRunAgain();
            }
        }


        private static void DownloadAnalyzedSongsNeedsToRunAgain()
        {
            ExecutionQueue.Enqueue(new WaitCallback(DataModel.DownloadAnalyzedSongs),
                ExecutionQueue.Policy.Queued);
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

            EventHandler<EchoNestApiEventArgs> handler =  
                new EventHandler<EchoNestApiEventArgs>(Api_CatalogCreateCompleted);
            thisApp.Api.CatalogCreateCompleted -= handler;
            thisApp.Api.CatalogCreateCompleted += handler;

            thisApp.Api.CatalogCreateAsync(Guid.NewGuid().ToString(), "song", 
                null, null);
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

        private static EventHandler<EchoNestApiEventArgs> updateHandler1 =
              new EventHandler<EchoNestApiEventArgs>(Api_CatalogUpdateCompleted1);

        private static void LoadCatalogIdNeedsToCheckCatalogId(string id)
        {
            App thisApp = App.Current as App;

            thisApp.Api.CatalogUpdateCompleted -= updateHandler1;
            thisApp.Api.CatalogUpdateCompleted -= updateHandler;
            thisApp.Api.CatalogUpdateCompleted += updateHandler1;

            // Issue dummy update to make sure it's there
            thisApp.Api.CatalogUpdateAsync(new Catalog
                {
                    Id = id,
                    SongActions = new List<CatalogAction<Song>>
                    {
                        new CatalogAction<Song>
                        {
                            Action = CatalogAction<Song>.ActionType.delete,
                            Item = new Song {
                                ItemId = "dummy"
                            }
                        }
                    }
                }, null, id);
        }

        static void Api_CatalogUpdateCompleted1(object sender,
            EchoNestApiEventArgs e)
        {
            if (e.Error != null)
            {
                if (e.Error is WebExceptionWrapper)
                {
                    // Transient network error
                    LoadCatalogIdNeedsToRunAgain();
                }
                else if (e.Error is EchoNestApiException)
                {
                    EchoNestApiException er = e.Error as EchoNestApiException;
                    if (er.Code == EchoNestApiException.EchoNestApiExceptionType.InvalidParameter &&
                        String.Equals(er.Message, "This catalog does not exist"))
                    {
                        // They either didn't have a CatalogId in local storage, or
                        // they did have one, but it wasn't on the cloud service. In
                        // both cases, we create a new one
                        LoadCatalogIdNeedsToCreateCatalog();
                    }
                }
             
            }
            else
            {
                // This catalog exists, everything is great 
                App thisApp = App.Current as App;
                lock (thisApp.Model.CatalogId)
                {
                    thisApp.Model.CatalogId = (string)e.UserState;
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
