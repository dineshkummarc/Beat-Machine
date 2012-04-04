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
using Microsoft.Xna.Framework.Content;
using System.Windows.Interop;

namespace BeatMachine2
{
    public class SongWrapper
    {
        public SongWrapper(Uri song)
        {
            Uri = song;
        }

        public SongWrapper(Uri song, double bmp)
        {
            Uri = song;
            BeatsPerMinute = bmp;
             
        }

        public Uri Uri { get; set; }
        public double BeatsPerMinute { get; set; }
    }
}
