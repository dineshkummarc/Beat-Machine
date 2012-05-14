using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using BeatMachine.EchoNest.Model;

namespace BeatMachine.Model
{
    [Table]
    public class AnalyzedSong : Song, INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string analyzedSongId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public string AnalyzedSongId
        {
            get
            {
                return analyzedSongId;
            }
            set
            {
                if (!String.Equals(analyzedSongId, value))
                {
                    NotifyPropertyChanging("IsComplete");
                    analyzedSongId = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }

        private string songId;

        [Column]
        public override string SongId
        {
            get
            {
                return songId;
            }
            set
            {
                if (!String.Equals(songId, value))
                {
                    NotifyPropertyChanging("IsComplete");
                    songId = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }


        private string songName;

        [Column]
        public override string SongName
        {
            get
            {
                return songName;
            }
            set
            {
                if (!String.Equals(songName, value))
                {
                    NotifyPropertyChanging("IsComplete");
                    songName = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }

        private string artistName;

        [Column]
        public override string ArtistName
        {
            get
            {
                return artistName;
            }
            set
            {
                if (!String.Equals(artistName, value))
                {
                    NotifyPropertyChanging("IsComplete");
                    artistName = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }

        [Table]
        public new class Summary : Song.Summary, INotifyPropertyChanged,
            INotifyPropertyChanging
        {
            private int summaryId;

            [Column(IsPrimaryKey = true)]
            public int SummaryId
            {
                set { summaryId = value; }
                get { return summaryId; }

            }

            private float? tempo;

            [Column]
            public override float? Tempo
            {
                get
                {
                    return tempo;
                }
                set
                {
                    if (value != tempo)
                    {
                        NotifyPropertyChanging("IsComplete");
                        tempo = value;
                        NotifyPropertyChanged("IsComplete");
                    }
                }
            }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            // Used to notify the page that a data context property changed
            private void NotifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            #endregion

            #region INotifyPropertyChanging Members

            public event PropertyChangingEventHandler PropertyChanging;

            // Used to notify the data context that a data context property is about to change
            private void NotifyPropertyChanging(string propertyName)
            {
                if (PropertyChanging != null)
                {
                    PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
                }
            }

            #endregion

        }

        private EntityRef<AnalyzedSong.Summary> audioSummary;

        [Association(Storage = "audioSummary", ThisKey = "SongId", OtherKey =
            "SummaryId")]
        public new AnalyzedSong.Summary AudioSummary
        {
            set { audioSummary.Entity = value; }
            get { return audioSummary.Entity; }

        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

    }
}
