using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BDO.Enums
{
    public enum TrainingSessionStatus
    {
        [Description("Not Started")]
        NotStarted = 1,
        [Description("Completed")]
        Completed = 2,
        [Description("Not completed")]
        Notcompleted = 3,
        [Description("Customer Deferred")]
        CustomerDeferred = 4,
    }
}
