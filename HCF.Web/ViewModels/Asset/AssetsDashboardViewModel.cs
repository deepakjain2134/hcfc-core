using HCF.BDO;
using System.Collections.Generic;


namespace HCF.Web.ViewModels
{
    public class AssetsDashboardViewModel
    {
        public IList<Assets> assets { get; set; }
        public IList<Buildings> buildings { get; set; }
    }
}