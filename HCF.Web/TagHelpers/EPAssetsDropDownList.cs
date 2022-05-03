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
using HCF.BAL.Interfaces.Services;
using HCF.Web.Base;

namespace HCF.Web.TagHelpers
{
    [HtmlTargetElement("epassetsdropdownlist")]
    public class EPAssetsDropDownList : TagHelper
    {
        private readonly IHtmlHelper _html;
        private readonly IAssetsService _constructionService;

        public EPAssetsDropDownList(IHtmlHelper helper, IAssetsService constructionService)
        {
            _html = helper;
            _constructionService = constructionService;
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

        [HtmlAttributeName("multiple")]
        public string Multiple { get; set; }

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

        private List<SelectListItem> BindItems()
        {
            var listItems = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(DefaultText))
            {
                var defaultSelectListItem = new SelectListItem { Text = DefaultText, Value = DefaultValue };
                listItems.Add(defaultSelectListItem);
            }
            var lists = _constructionService.GetAssetsByType(UserSession.CurrentUser.UserId, 0).ToList();
           
            foreach (var item in lists.Where(x => x.IsActive))
            {
                foreach (var asset in item.Assets.Where(y => y.Count > 0 && y.StandardEps.Any() && y.IsActive).OrderBy(x => x.Name))
                {
                    var selItem = new SelectListItem
                    {
                        Text = asset.Name,
                        Value = Convert.ToString(asset.AssetId)
                    };
                    if (!string.IsNullOrEmpty(SelectedValue) && selItem.Value == SelectedValue)
                        selItem.Selected = true;
                    listItems.Add(selItem);
                }

            }
            return listItems;
        }
    }
}
