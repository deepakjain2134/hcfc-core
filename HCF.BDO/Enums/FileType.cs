using System.ComponentModel;

namespace HCF.BDO.Enums
{

    public enum FileType
    {
        [Description("Policy")]
        Policy = 106,
        [Description("Report")]
        Report = 107,
        [Description("Misc. Documents")]
        MiscDocuments = 108,
        [Description("Assets Report")]
        AssetsReport = 109,
        [Description("FireDrill Report")]
        FireDrillReport = 110,
    }
}
