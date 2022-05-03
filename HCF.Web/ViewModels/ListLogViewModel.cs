using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.Web.ViewModels
{
    public class ListLogViewModel
    {
        public IList<LogEntry> LogFiles { get; set; }

    }
}