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
using System.Globalization;
using HCF.Web.Base;

namespace HCF.Web.TagHelpers
{
    /// <summary>
    /// 
    /// </summary>
    [HtmlTargetElement("userslist")]
    public class UsersList : TagHelper
    {
        private readonly IHtmlHelper _html;
        private readonly IUserService _userService;

        public UsersList(IHtmlHelper helper, IUserService userService)
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

        [HtmlAttributeName("selectedvalue")]
        public string SelectedValue { get; set; }

        [HtmlAttributeName("class")]
        public string ClassName { get; set; }

        [HtmlAttributeName("id")]
        public string ID { get; set; }

        [HtmlAttributeName("required")]
        public string Required { get; set; }

        [HtmlAttributeName("isworkflow")]
        public string isworkflow { get; set; }

        [HtmlAttributeName("isVendorUseronly")]
        public string IsVendorUseronly { get; set; }

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
                ClassName = ClassName,
                ID = ID
            };

            if (Required == "required")
                customSelectPicker.IsRequired = true;


            if(!string.IsNullOrEmpty(isworkflow) && isworkflow.ToLower() == "true")
            {
                customSelectPicker.IsRequired = false;
            }
            content = await _html.PartialAsync("TagHelpers/CommonDropdownList", customSelectPicker);
            output.Content.SetHtmlContent(content);
        }

        private List<SelectListItem> BindItems()
        {
            var listItems = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(DefaultText))
            {
                var defaultSelectListItem = new SelectListItem { Text = DefaultText, Value = DefaultValue };
                listItems.Add(defaultSelectListItem);
            }
            var users = _userService.Get().Where(x => x.IsActive && !x.IsCRxUser && !string.IsNullOrEmpty(x.Email)).OrderBy(x => x.FullName).ToList();
            if (!string.IsNullOrEmpty(IsVendorUseronly) && UserSession.CurrentUser.IsVendorUser)
            {
                users = users.Where(x => x.VendorId == UserSession.CurrentUser.VendorId).ToList();
            }


            foreach (var item in users)
            {
                var selItem = new SelectListItem
                {
                    Text = $@"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.FullName.ToLower())} ({(string.IsNullOrEmpty(item.Email) ? "NA Email" : item.Email)})",
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
