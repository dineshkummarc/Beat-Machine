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
                    NotifyPropertyChanging("AnalyzedSongId");
                    analyzedSongId = value;
                    NotifyPropertyChanged("AnalyzedSongId");
                }
            }
        }

        [Column]
        public override string ItemId
        {
            get
            {
                return base.ItemId;
            }
            set
            {
                if (!String.Equals(base.ItemId, value))
                {
                    NotifyPropertyChanging("ItemId");
                    base.ItemId = value;
                    NotifyPropertyChanged("ItemId");
                }
            }
        }

        [Column]
        public override string SongName
        {
            get
            {
                return base.SongName;
            }
            set
            {
                if (!String.Equals(base.SongName, value))
                {
                    NotifyPropertyChanging("SongName");
                    base.SongName = value;
                    NotifyPropertyChanged("SongName");
                }
            }
        }

        [Column]
        public override string ArtistName
        {
            get
            {
                return base.ArtistName;
            }
            set
            {
                if (!String.Equals(base.ArtistName, value))
                {
                    NotifyPropertyChanging("ArtistName");
                    base.ArtistName = value;
                    NotifyPropertyChanged("ArtistName");
                }
            }
        }

        [Table]
        public new class Summary : Song.Summary, INotifyPropertyChanged,
            INotifyPropertyChanging
        {
            private int summaryId;

            [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
            public int SummaryId
            {
                set { summaryId = value; }
                get { return summaryId; }

            }

            [Column]
            public override float? Tempo
            {
                get
                {
                    return base.Tempo;
                }
                set
                {
                    if (value != base.Tempo)
                    {
                        NotifyPropertyChanging("Tempo");
                        base.Tempo = value;
                        NotifyPropertyChanged("Tempo");
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

        [Association(Storage = "audioSummary", ThisKey = "AnalyzedSongId", OtherKey =
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
