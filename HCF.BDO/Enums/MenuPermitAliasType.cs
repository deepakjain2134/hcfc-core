using System.ComponentModel;

namespace HCF.BDO.Enums
{
    public enum MenuPermitAliasType
    {

        [Description("Ceiling Permit Request")]
        CeilingPermit = 1,

        [Description("Occurrence Report")]
        OccurrencePermit = 2,

        [Description("Method Of Procedure")]
        Mop = 3,

        [Description("Fire System Bypass Permit")]
        FireSystemBypassPermit = 4,

        [Description("Life Safety Device")]
        LsdAdditionForm = 5,

        [Description("Life Safety Device")]
        LsdRemovalForm = 6,

        [Description("Hot Work Permit")]
        HotWorkPermit = 7,

        [Description("CRA")]
        CRA = 8,

        [Description("PCRA")]
        PCRA = 10,

        [Description("ICRA")]
        ICRA = 11,

        [Description("ICRA V2")]
        ICRAv2 = 12,        

        [Description("Asset Device Change")]
        ADCAdditionForm = 13,

        [Description("Asset Device Change")]
        ADCRemovalForm = 14,
    }
}
