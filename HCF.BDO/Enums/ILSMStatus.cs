using System.ComponentModel;

namespace HCF.BDO.Enums
{
    public enum ILSMStatus
    {
        [Description("Closed")]
        Closed = 1,
        [Description("In-progress")]
        InProgress = 2,
        [Description("Open")]
        Open = 0,
    }
}
