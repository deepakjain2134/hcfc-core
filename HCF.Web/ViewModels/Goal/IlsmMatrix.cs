using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.Web.ViewModels.Goal
{
    public class IlsmMatrixViewModel
    {
        public IEnumerable<MainGoal> MainGoals { get; set; }
        public List<EPDetails> IlsmEPs { get; set; }
        public List<AffectedEPs> AffectedEPs { get; set; }
    }
}