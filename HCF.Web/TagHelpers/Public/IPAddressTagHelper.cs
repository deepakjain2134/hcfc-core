using HCF.Utility.AppConfig;
using IdentityServer4.Models;
using Lucene.Net.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace HCF.Web.TagHelpers.Public
{
    [HtmlTargetElement("ip-address")]
    public class IPAddressTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAppSetting _appsettings;

        public IPAddressTagHelper(IHttpContextAccessor contextAccessor, IAppSetting appsettings)
        {
            _contextAccessor = contextAccessor;
            _appsettings = appsettings;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var ipAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress;

            output.TagName = "ipaddress";
            output.TagMode = TagMode.StartTagAndEndTag;

            var sb = new StringBuilder();
            sb.AppendFormat("<p class='login-ipRow' title='{1}'><strong>IP Address: {0}</strong></p>", ipAddress, _appsettings.WebUrlPath);

            output.PreContent.SetHtmlContent(sb.ToString());
        }
    }
}
