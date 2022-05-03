using System;
using System.Threading.Tasks;
using HCF.Utility.AppConfig;
using HCF.Web.ViewModels.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HCF.Web.TagHelpers
{
    [HtmlTargetElement("common-image")]
    public class CommonImage : TagHelper
    {
        [HtmlAttributeName("src")]
        public string ImageUrl { get; set; }


        [HtmlAttributeName("title")]
        public string Title { get; set; }
               

        [HtmlAttributeName("class")]
        public string Class { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlHelper _html;
        private readonly IAppSetting _appSetting;

        public CommonImage(IHtmlHelper helper, IAppSetting appSetting)
        {
            _html = helper;
            _appSetting = appSetting;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Contextualize the ViewContext
            ((IViewContextAware)_html).Contextualize(ViewContext);

            // Make sure we don't have any tags associated with this TagHelper.
            output.TagName = String.Empty;

            // DI'd into the constructor
            IHtmlContent content = null;
            var model = new ImageModelTagHelper();

            // If we don't have an image, return the noImage.cshtml
            if (string.IsNullOrEmpty(ImageUrl))
            {
                content = await _html.PartialAsync("taghelpers/noimage", model);
            }
            else
            {               
                var newSource = $"{_appSetting.CommonFileBasePath}{ImageUrl.ToString().Replace("~/", string.Empty)}";
                var uri = new Uri(newSource.ToLower());
                model = new ImageModelTagHelper
                {
                    ClassName = Class,
                    Title = Title,
                    Url = uri
                };
                content = await _html.PartialAsync("taghelpers/commonimage", model);
            }

            output.Content.SetHtmlContent(content);
        }
    }
}
