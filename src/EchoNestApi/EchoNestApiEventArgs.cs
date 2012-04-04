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
using System.ComponentModel;

namespace BeatMachine.EchoNest
{
    public class EchoNestApiEventArgs : AsyncCompletedEventArgs
    {
        private readonly object _result;

        public EchoNestApiEventArgs(Exception error, bool cancelled, object userState, object result)
            : base(error, cancelled, userState)
        {
            _result = result;
        }

        /// <summary>
        /// Gets the API call result as JSON
        /// </summary>
        /// <returns>
        /// The API call result as JSON
        /// </returns>
        public object GetResultData()
        {
            RaiseExceptionIfNecessary();
            return _result;
        }
    }
}
