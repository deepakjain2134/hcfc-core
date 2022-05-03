using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.Web.ViewModels.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HCF.Web.TagHelpers
{
    /// <summary>
    /// 
    /// </summary>
    [HtmlTargetElement("userdropdownlist")]
    public class UserDropDownList : TagHelper
    {
        [HtmlAttributeName("name")]
        public string ControlName { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlHelper _html;
        private readonly IUserService _userService;       

        [HtmlAttributeName("firstvalue")]
        public string DefaultValue { get; set; }

        [HtmlAttributeName("firsttext")]
        public string DefaultText { get; set; }

        [HtmlAttributeName("class")]
        public string ClassName { get; set; }

        [HtmlAttributeName("selectedvalue")]
        public string SelectedValue { get; set; }

        public UserDropDownList(IHtmlHelper helper, IUserService userService)
        {
            _html = helper;
            _userService = userService;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Contextualize the ViewContext
            ((IViewContextAware)_html).Contextualize(ViewContext);

            // Make sure we don't have any tags associated with this TagHelper.
            output.TagName = string.Empty;

            // DI'd into the constructor
            IHtmlContent content = null;

            // If we don't have an image, return the noImage.cshtml
           

            var customSelectPicker = new CustomSelectPicker
            {
                Items = BindItems(),
                Name = ControlName,
                ClassName=ClassName
            };

            content = await _html.PartialAsync("TagHelpers/CommonDropdownList", customSelectPicker);
            output.Content.SetHtmlContent(content);
        }
       
        private List<SelectListItem> BindItems()
        {
            var users = _userService.GetUserList().Where(x => !x.IsCRxUser && x.IsActive).OrderBy(x => x.FullName).ToList();
            var listItems = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(DefaultText))
            {
                var defaultSelectListItem = new SelectListItem { Text = DefaultText, Value = DefaultValue };
                listItems.Add(defaultSelectListItem);
            }         

            foreach (var item in users)
            {
                var selItem = new SelectListItem
                {
                    Text = $@"{item.FullName} ({item.Email})",
                    Value = Convert.ToString(item.UserId)
                };
                if (!string.IsNullOrEmpty(SelectedValue) && selItem.Value == SelectedValue)
                    selItem.Selected = true;
                listItems.Add(selItem);

            }
            return listItems;
        }
    }
}
