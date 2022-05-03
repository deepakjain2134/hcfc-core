using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.Web.ViewModels.Dashboard
{
    public class VendorDashboardViewModel
    {
        public VendorRecordsCount VendorRecordsCount { get; set; }
        public List<Vendors> vendorRes { get; set; }
    }
}