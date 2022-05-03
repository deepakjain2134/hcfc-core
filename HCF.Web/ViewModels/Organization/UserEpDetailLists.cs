using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewModels
{
    public class UserEpDetailLists
    {
        public int[] UserIds { get; set; }

        public List<EPDetails> EpDetails { get; set; }

        public int[] SelectedEps { get; set; }
    }
}
