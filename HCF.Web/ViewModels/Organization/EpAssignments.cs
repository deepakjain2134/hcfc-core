using HCF.BDO;
using HCF.Web.Models;
using System.Collections.Generic;

namespace HCF.Web.ViewModels.Organization
{
    public class EpAssignmentsViewModel
    {
        public List<UserProfile> UserList { get; set; }

        public SelectPicker MainSelectPicker { get; set; }

        public SelectPicker SecondSelectPicker { get; set; }

        public List<EPDetails> EpDetails { get; set; }

        public int CategoryId { get; set; }

        public int[] UserIds { get; set; }
    }
}