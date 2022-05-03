using System.ComponentModel;

namespace HCF.BDO.Enums
{

    public enum passwordStatus
    {
        create,
        reset,
        recover
    };

    public enum InspectionStatus
    {
        Compliant = 1,
        NonCompliant = 0,
        Pending = -1,
        NotApplicable = -3
    }
    public enum EpStateCode
    {       
        [Description("Low")]
        Expired = 3,//1
        [Description("Medium")]
        UpComing = 5,//2
        [Description("High")]
        InspectionDisabled = 7,//3
        [Description("TJCInActive")]
        TJCInActive = 9,//3
        [Description("NotApplicable")]
        NotApplicable = 8,//3
        [Description("ACTIV")]
        ACTIV = 10//3

    }
}
