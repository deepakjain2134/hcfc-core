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
    [HtmlTargetElement("multiselectbuildingdropdownlist")]
    public class MultiSelectBuildingDropDownList : TagHelper
    {
        private readonly IHtmlHelper _html;
        private readonly IBuildingsService _buildingsService;

        public MultiSelectBuildingDropDownList(IHtmlHelper helper, IBuildingsService buildingsService)
        {
            _html = helper;
            _buildingsService = buildingsService;
        }

        [HtmlAttributeName("name")]
        public string ControlName { get; set; }

        [HtmlAttributeName("firstvalue")]
        public string DefaultValue { get; set; }

        [HtmlAttributeName("multiple")]
        public string Multiple { get; set; }

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
            List<string> SelectedIds = new List<string>();
            var listItems = BindItems();
            List<string> result = SelectedValue.Split(',').ToList();
            SelectedIds = result;
            var customSelectPicker = new CustomSelectPicker
            {
                Items = listItems,
                Name = ControlName,
                ClassName = ClassName,
                SelectedIds= SelectedIds
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
            var values = _buildingsService.GetBuildings().Where(x => x.IsActive).ToList();
            //if (isActiveOnly)
            //    values = values.Where(x => x.IsActive && x.SiteActive).ToList();

            var sites = values.OrderBy(x => x.SiteName).Where(x => x.IsActive).GroupBy(x => x.SiteName);

            if (!string.IsNullOrEmpty(SelectedValue))
                SelectedValue = "," + SelectedValue.Trim() + ",";

            //listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
            foreach (var site in sites)
            {
                var optionGroup = new SelectListGroup() { Name = site.Key };
                listItems.AddRange(site.Select(buildings => new SelectListItem()
                {
                    Value = buildings.BuildingId.ToString(),
                    Text = buildings.BuildingName,
                    Group = optionGroup,
                    Selected = SelectedValue.Contains("," + Convert.ToString(buildings.BuildingId) + ",")
                }));
            }
            return listItems;
        }
    }
}
