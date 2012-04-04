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

namespace BeatMachine.EchoNest.Model
{
    public class CatalogAction<T>
    {
        public enum ActionType
        {
            update,
            delete,
            play,
            skip
        }

        public CatalogAction()
        {
            Action = ActionType.update;
        }

        [JsonProperty(PropertyName="action")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ActionType Action
        {
            get;
            set;
        }

        [JsonProperty(PropertyName="item")]
        public T Item
        {
            get;
            set;
        }
    }
}
