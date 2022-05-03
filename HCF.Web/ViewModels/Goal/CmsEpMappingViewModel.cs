using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.Web.ViewModels.Goal
{
    public class CmsEpMappingViewModel
    {
        public List<CopDetails> CopDetails { get; set; }

        public List<EPDetails> EPDetails { get; set; }

        public List<HCF.BDO.CmsEpMapping> CmsEpMapping { get; set; }
    }
}