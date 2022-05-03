using System.ComponentModel;

namespace HCF.BDO.Enums
{
    public enum TipTypes
    {
        [Description("Help Page Text")]
        HelpPageText = 3,
        [Description("CRx Crowdsourced")]
        CRxCrowdSource = 2,
        [Description("Crowdsourced")]
        CrowdSource = 1,
        [Description("FAQs/How-Tos")]
        FAQHowTo = 4,
        [Description("CRX Improvement Suggestion")]
        CRXSuggestion = 5,
    }
}
