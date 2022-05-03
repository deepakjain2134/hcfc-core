using System.ComponentModel;

namespace HCF.BDO.Enums
{
    public enum ModuleServiceMode
    {
        [Description("Active")]
        Active = 1,
        [Description("In-Trial")]
        InTrial = 2,
        [Description("Resume Trial")]
        TrialPause = 3,
        [Description("Trial Expired")]
        TrialExpired = 4,
        [Description("In-Trial")]
        TrialResume = 5,
        [Description("Start Trial")]
        StartTrial = -3
    }
}
