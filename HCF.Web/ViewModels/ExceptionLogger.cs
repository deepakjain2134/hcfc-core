using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.Web.Models
{
    public class ExceptionLogger
    {
        
        public int Id { get; set; }
        public string ExceptionMessage { get; set; }
        public string ControllerName { get; set; }
        public string ExceptionStackTrace { get; set; }
        public DateTime LogTime { get; set; }

    }
}