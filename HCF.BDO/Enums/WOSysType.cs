using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BDO.Enums
{
   public enum WOSysType
    {
        [Description("Maintenance Connection ")]
        MaintenanceConnection =1,
        TMS=2,
        TMA=3,
        Archibus=4,
        [Description("The Worx Hubby Dude Solutions")]
        TheWorxHubbyDudeSolutions =5,
        [Description("Facility ONE")]
        FacilityONE = 6,
        Nuvolo=7,
        [Description("Facility Fit by ARAMARK")]
        FacilityFitbyARAMARK =8,
        [Description("Service Desk")]
        ServiceDesk =9
    }
}
