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
using Newtonsoft.Json;

namespace BeatMachine.EchoNest.Model
{
    public class Song
    {
        /// <summary>
        /// Use in search or catalog methods
        /// </summary>
        [JsonProperty(PropertyName = "artist_id")]
        public string ArtistId
        {
            get;
            set;
        }

        /// <summary>
        /// Use in search method
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Use in catalog method
        /// </summary>
        [JsonProperty(PropertyName = "item_id")]
        public string ItemId
        {
            get;
            set;
        }

        /// <summary>
        /// Use in search or catalog methods
        /// </summary>
        [JsonProperty(PropertyName = "artist_name")]
        public string ArtistName
        {
            get;
            set;
        }

        /// <summary>
        /// Use in search method
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Use in catalog method
        /// </summary>
        [JsonProperty(PropertyName = "song_name")]
        public string SongName
        {
            get;
            set;
        }
    }
}