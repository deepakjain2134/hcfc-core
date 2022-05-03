using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Web.TagHelpers
{
    [HtmlTargetElement("ep-risk-score")]
    public class EpRiskSocre : TagHelper
    {
        public string RiskScore { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "EpRiskTagHelper";
            output.TagMode = TagMode.StartTagAndEndTag;

            var sb = new StringBuilder();
            var riskScoreText = (!string.IsNullOrEmpty(RiskScore)) ? "R" : string.Empty;

            if (!string.IsNullOrEmpty(RiskScore))
                sb.AppendFormat("<span>{0}</span>", riskScoreText);

            output.PreContent.SetHtmlContent(sb.ToString());
        }
    }
}
