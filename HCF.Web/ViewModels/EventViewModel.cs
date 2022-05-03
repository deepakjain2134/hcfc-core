using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HCF.BDO;

namespace HCF.Web.Models
{
    public class EventViewModel
    {
        public int id { get; set; }

        public string title { get; set; }

        public string start { get; set; }

        public string end { get; set; }

        public bool allDay { get; set; }

        public string color { get; set; }

        public string textColor { get; set; }

        public string users { get; set; }

        public RoundGroup LocationGroup { get; set; }
        public string eventDate { get; set; }

        public string text { get; set; }


    }
}