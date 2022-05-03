using System;
using System.Globalization;

namespace HCF.Utility
{
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message) : base(message) { }

        public AppException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }

        public int StatusCode { get; set; }

        public string AppErrorMessage { get; set; }

        public string AppStackTrace { get; set; }

        public string ErrorPath { get; set; }
    }
}
