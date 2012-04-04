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

namespace BeatMachine.EchoNest
{
    public class EchoNestApiException : Exception
    {
        public enum EchoNestApiExceptionType
        {
            UnknownError = -1,
            MissingOrInvalidApiKey = 1,
            ApiKeyNotAllowedToCallMethod = 2,
            RateLimitExceeded = 3,
            MissingParameter = 4,
            InvalidParameter = 5
        }

        private EchoNestApiExceptionType code;

        public EchoNestApiExceptionType Code
        {
            get { return code; }
        }

        public EchoNestApiException(EchoNestApiExceptionType code, string message)
            : base(message)
        {
            this.code = code;
        }

    }
}
