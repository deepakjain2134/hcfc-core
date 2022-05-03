using System.ComponentModel;

namespace HCF.BDO.Enums
{
    public enum RiskScore
    {
        //[Description("Very Low")]
        //VeryLow = 1,
        [Description("Low")]
        Low = 3,//1
        [Description("Medium")]
        Medium = 5,//2
        [Description("High")]
        High = 7,//3
        [Description("Very High")]
        VeryHigh = 10,//4
    }
}
