using System.ComponentModel;

namespace HCF.BDO.Enums
{
    public enum ApprovalStatus
    {
        [Description("Requested")]
        Requested = 2,
        [Description("Approved")]
        Approved = 1,
        [Description("Rejected")]
        Rejected = 0,
        [Description("Pending")]
        Pending = 3,
        [Description("Pending Submission")]
        PendingSubmission = -1,
        [Description("Created")]
        Created = 4,
        [Description("Closed")]
        Closed = 5,
        [Description("Reviewed")]
        Reviewed = 6
    }
}
