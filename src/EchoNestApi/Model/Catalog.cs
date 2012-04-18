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
using System.Collections.Generic;

namespace BeatMachine.EchoNest.Model
{
    public class Catalog
    {
        /// <summary>
        /// Used in catalog list, update, and delte methods
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Useed in catalog delete method
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "type")]
        public string Type
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "total")]
        public int Total
        {
            get;
            set;
        }


        /// <summary>
        /// Used only in catalog read method
        /// </summary>
        [JsonProperty(PropertyName = "start")]
        public int Start
        {
            get;
            set;
        }

        /// <summary>
        /// Used only in catalog read method
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public List<Song> Items
        {
            get;
            set;
        }

        /// <summary>
        /// Used only in catalog update method
        /// </summary>
        public List<CatalogAction<Song>> SongActions
        {
            get;
            set;
        }

        // TODO Add support for artist catalogs

        /// <summary>
        /// Used only in catalog update method
        /// </summary>
        // public List<CatalogAction<Artist>> ArtistActions
        // {
        //     get;
        //     set;
        // }

    }
}
