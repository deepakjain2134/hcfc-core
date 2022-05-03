using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.Web.ViewModels.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.Web.TagHelpers
{
    [HtmlTargetElement("epuserassignementdropdown")]
    public class EpUserAssignementDropDown : TagHelper
    {
        private readonly IHtmlHelper _html;
        private readonly IUserService _userService;


        public EpUserAssignementDropDown(IHtmlHelper helper, IUserService userService)
        {
            _html = helper;
            _userService = userService;
        }

        [HtmlAttributeName("name")]
        public string ControlName { get; set; }

        [HtmlAttributeName("firstvalue")]
        public string DefaultValue { get; set; }

        [HtmlAttributeName("firsttext")]
        public string DefaultText { get; set; }

        [HtmlAttributeName("selectevalue")]
        public string SelectedValue { get; set; }

        [HtmlAttributeName("multiple")]
        public string Multiple { get; set; }

        [HtmlAttributeName("class")]
        public string ClassName { get; set; }

        [HtmlAttributeName("asp-items")]
        List<UserProfile> Users { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Contextualize the ViewContext
            ((IViewContextAware)_html).Contextualize(ViewContext);

            // Make sure we don't have any tags associated with this TagHelper.
            output.TagName = string.Empty;

            // DI'd into the constructor
            IHtmlContent content = null;
            var listItems = BindItems();
            var customSelectPicker = new CustomSelectPicker
            {
                Items = listItems,
                Name = ControlName,
                ClassName = ClassName
            };
            if (!string.IsNullOrEmpty(Multiple) && Multiple.ToLower() == "multiple")
                customSelectPicker.IsMultiple = true;
            content = await _html.PartialAsync("TagHelpers/CommonDropdownList", customSelectPicker);
            output.Content.SetHtmlContent(content);
        }

        private List<SelectListItem> BindItems(bool isShowCount = true, bool showEmail = true)
        {
            var listItems = new List<SelectListItem>();
            var options = "";
            var users = _userService.GetUserList().Where(x => x.IsCRxUser == false && !string.IsNullOrEmpty(x.Email)).OrderBy(x => x.FullName).ToList();
            foreach (var item in users)
            {
                var text = "";
                if (isShowCount)
                    text = item.FullName + " (" + ((item.IsPowerUser) ? " PU " + item.EpsCount : item.EpsCount.ToString()) + " )";
                else
                    text = item.FullName + " " + ((item.IsPowerUser) ? " PU " : ""); 

                //var text = item.FullName + " (" + subText + ")";
                if (showEmail)
                    text = text + " <br/>" + item.Email;

                var option = new TagBuilder("option");
                option.MergeAttribute("value", item.UserId.ToString());
                option.MergeAttribute("data-domain", item.Email);
                option.MergeAttribute("data-content", text);

                if (SelectedValue == item.UserId.ToString())
                    option.MergeAttribute("selected", "selected");
                option.MergeAttribute("text", item.FullName);

                if (item.IsPowerUser)
                    option.MergeAttribute("enabled", "enabled");
                options += option.ToString() + "\n";
            }
            return listItems;
        }
    }
}
