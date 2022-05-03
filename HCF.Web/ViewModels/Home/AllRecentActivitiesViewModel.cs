using HCF.BAL.Models.General;
using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.Web.ViewModels.Home
{
    public class AllRecentActivitiesViewModel
    {
        public PaginatedList<ActivityBase> Activities { get; set; }

        public int? PageIndex { get; set; }
        public int? TotalCount { get; set; }
    }
}