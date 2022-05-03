using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewModels.Location
{
    public class LocationGroupViewModel
    {
        public IList<LocationGroups> locationGroup { get; set; }
        public IList<Buildings> buildings { get; set; }
    }
}
