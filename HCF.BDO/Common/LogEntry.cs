using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.BDO
{
    public partial class LogEntry
    {
        public DateTime Date { get; set; }

        public string DbName { get; set; }
        public string Module { get; set; }
        public string Method { get; set; }
        public string DeclaringType { get; set; }
        public string LineNumber { get; set; }
        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        public string UserName { get; set; }

        public string ErrorCode { get; set; }

        public string OsName { get; set; }
        public string BrowserName { get; set; }

        public string DeviceType { get; set; }
    }
}