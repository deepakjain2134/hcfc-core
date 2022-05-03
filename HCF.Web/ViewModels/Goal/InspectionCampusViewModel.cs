using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewModels.Goal
{
    public class InspectionCampusViewModel
    {
        public IList<string> stringarray { get; set; }
        public IList<Buildings> Campus { get; set; }
        public bool ShowAll { get; set; }
    }
}
