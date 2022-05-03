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

namespace HCF.Web.TagHelpers
{
    [HtmlTargetElement("sitedropdownlist")]
    public class SiteDropDownList : TagHelper
    {
        private readonly IHtmlHelper _html;
        private readonly ISiteService _siteService;

        public SiteDropDownList(IHtmlHelper helper, ISiteService siteService)
        {
            _html = helper;
            _siteService = siteService;
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
            
            if (HCF.Web.Base.UserSession.CurrentUser.IsPowerUser || HCF.Web.Base.UserSession.CurrentUser.IsSystemUser)
            {
                var sites = _siteService.GetSites().Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
                foreach (var item in sites)
                {
                    var selItem = new SelectListItem { Text = item.Name, Value = Convert.ToString(item.Code.Trim()) };
                    if (!string.IsNullOrEmpty(SelectedValue) &&
                        selItem.Value.ToLower() == Convert.ToString(SelectedValue).ToLower())
                        selItem.Selected = true;
                    listItems.Add(selItem);
                }
            }
            else 
            {
                var UserId = HCF.Web.Base.UserSession.CurrentUser.UserId;
                var userwisesites = _siteService.GetSitesByUserId(UserId).Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
                foreach (var item in userwisesites)
                {
                    var selItem = new SelectListItem { Text = item.Name, Value = Convert.ToString(item.Code.Trim()) };
                    if (!string.IsNullOrEmpty(SelectedValue) &&
                        selItem.Value.ToLower() == Convert.ToString(SelectedValue).ToLower())
                        selItem.Selected = true;
                    listItems.Add(selItem);
                }


            }
            return listItems;
        }
    }
}
