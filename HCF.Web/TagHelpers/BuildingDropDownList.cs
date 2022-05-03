using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.BDO;
using HCF.Web.ViewModels.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HCF.Web.TagHelpers
{

    [HtmlTargetElement("buildingdropdownList")]
    public class BuildingDropDownList : TagHelper
    {
       

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

        [HtmlAttributeName("required")]
        public string Required { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlHelper _html;
        private readonly IBuildingsService _buildingsService;
        Dictionary<string, string> attributes = new Dictionary<string, string>();

        public BuildingDropDownList(IHtmlHelper helper, IBuildingsService buildingsService)
        {
            _html = helper;
            _buildingsService = buildingsService;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            foreach (var item in context.AllAttributes)
            {
                if (item.Name.StartsWith("asp-for"))
                    attributes.Add(item.Name.Replace("asp-for-", ""), Convert.ToString(item.Value));
            }

            // Contextualize the ViewContext
            ((IViewContextAware)_html).Contextualize(ViewContext);

            // Make sure we don't have any tags associated with this TagHelper.
            output.TagName = string.Empty;

            // DI'd into the constructor
            IHtmlContent content = null;

            // If we don't have an image, return the noImage.cshtml
            //var users = _service.GetBuildings().ToList();

            var customSelectPicker = new CustomSelectPicker
            {
                Items = BindItems(), //users.Select(user => new SelectListItem() { Text = user.BuildingName, Value = user.BuildingId.ToString() }).ToList(),
                Name = ControlName,
                ClassName = ClassName,
                Attributes = attributes
            };

            content = await _html.PartialAsync("TagHelpers/CommonDropdownList", customSelectPicker);
            output.Content.SetHtmlContent(content);
        }

        private List<SelectListItem> BindItems()
        {
          

            var listItems = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(DefaultText))
            {
                var optionGroup = new SelectListGroup() { Name = string.Empty };
                var defaultSelectListItem = new SelectListItem { Text = DefaultText, Value = DefaultValue, Group = optionGroup };
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
                    Selected = !string.IsNullOrEmpty(SelectedValue) ? SelectedValue.Contains("," + Convert.ToString(buildings.BuildingId) + ",") : false
                })); ;
            }
            return listItems;
        }


    }
}
