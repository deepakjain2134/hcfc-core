using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HCF.BDO;
namespace HCF.Web.ViewModels
{
    public class InspectionActivityHistoryViewModel
    {
       public IList<TInspectionActivity> activity { get; set; }
        public EPDetails  EPDetails { get; set; }
    }
}