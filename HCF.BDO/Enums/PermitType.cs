using System.ComponentModel;

namespace HCF.BDO.Enums
{
    public enum PermitType
    {

        [Description("Ceiling Permit Request")]
        CeilingPermit = 1,

        [Description("Facilities Maintenance Occurrence")]
        OccurrencePermit = 2,

        [Description("Method of Procedure")]
        Mop = 3,

        [Description("Fire System Bypass Permit")]
        FireSystemBypassPermit = 4,

        [Description("Life Safety Device - Addition Form")]
        LsdAdditionForm = 5,

        [Description("Life Safety Device - Removal Form")]
        LsdRemovalForm = 6,

        [Description("Hot Work Permit")]
        HotWorkPermit = 7,

        [Description("Construction Risk Assessment [CRA]")]
        CRA = 8,

        [Description("Pre-Construction Risk Assessment [PCRA]")]
        PCRA = 10,

        [Description("Infection Control Risk Assessment [ICRA]")]
        ICRA = 11,

        [Description("Infection Control Risk Assessment [ICRA v2]")]
        ICRAv2 = 12,

        [Description("Asset Device Change- Addition Form")]
        ADCAdditionForm = 13,

        [Description("Asset Device Change- Removal Form")]
        ADCRemovalForm = 14,
    }
}
