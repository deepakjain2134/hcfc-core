using System.ComponentModel;

namespace HCF.BDO.Enums
{
    public enum FiredrillDocStatus
    {
        [Description("Documentation Pending")]
        DocumentationPending = -1,

        [Description("Documentation In-progress")]
        DocumentationInprogress = 2,

        [Description("Documentation Completed")]
        DocumentationCompleted = 1,

        [Description("Documentation Past Due")]
        DocumentationPastdue = 0,


    }
}
