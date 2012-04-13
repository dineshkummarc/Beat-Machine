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
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace BeatMachine.EchoNest.Model
{
    public class Ticket
    {
        public enum TicketStatus
        {
            unknown,
            pending,
            complete, 
            error
        }

        public class Item
        {
            [JsonProperty(PropertyName = "item_id")]
            public string Id
            {
                get;
                set; 
            }

            [JsonProperty(PropertyName = "info")]
            public string Info
            {
                get;
                set;
            }
        }

        [JsonProperty(PropertyName="ticket_status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TicketStatus Status
        {
            get;
            set;
        }

        [JsonProperty(PropertyName="total_items")]
        public int TotalItems
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "items_updated")]
        public int ItemsUpdated
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "percent_complete")]
        public float PercentComplete
        {
            get;
            set;
        }


        [JsonProperty(PropertyName = "details")]
        public string Details
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "update_info")]
        public List<Item> UpdateInfo
        {
            get;
            set;
        }
    }
}
