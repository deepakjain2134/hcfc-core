using System.ComponentModel;

namespace HCF.BDO.Enums
{
    public enum UploadDocTypes
    {
        Policy = 106,
        Report = 107,
        [Description("Misc. Document")]
        MiscDocuments = 108,
        VendorReport = 110,
        SampleDocument = -1,
        AssetsReport = 109
    };
}
