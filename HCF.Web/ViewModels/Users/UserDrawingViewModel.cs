using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.Web.ViewModels
{
    public class UserDrawingViewModel
    {
        public IList<UserProfile> Users { get; set; }

        public IList<FloorTypes> Drawings { get; set; }

        public IList<Vendors> Vendors { get; set; }

        public int? UserId { get; set; }

        public int? VendorId { get; set; }

        public long[] SelectedItems { get; set; }

        //public IList<UserSiteMapping> UserSites { get; set; }

        //public IList<UserSiteMapping> VendorSites { get; set; }
    }
}