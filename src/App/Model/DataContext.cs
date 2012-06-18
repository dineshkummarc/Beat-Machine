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

namespace BeatMachine.Model
{
    public class BeatMachineDataContext : DataContext
    {
        public static string DBConnectionString = "Data Source=isostore:/BM.sdf";

        public BeatMachineDataContext(string connectionString)
            : base(connectionString)
        { }

        public Table<AnalyzedSong> AnalyzedSongs;

        public Table<AnalyzedSong.Summary> Summary;


    }
}
