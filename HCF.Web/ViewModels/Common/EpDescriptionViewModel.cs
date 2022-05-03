using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.Web.ViewModels.Common
{
    public class EpDescriptionViewModel
    {
        public int EpDetailId { get; set; }
        public EPDetails Epdetails { get; set; }
        public int MaxLength { get; set; }
    }
}
