using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HCF.BDO;

namespace HCF.Web.ViewModels
{
    public class MainGoalViewModel
    {
        public MainGoal goal { get; set; }
        public IList<EPDetails> epdetails { get; set; }
    }
}