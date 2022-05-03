using System.Linq;
using System.Threading.Tasks;
using HCF.Web.ViewModels.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using HCF.BAL.Interfaces.Services;
using HCF.Utility;

namespace HCF.Web.TagHelpers
{
    [HtmlTargetElement("assetsdownlist")]
    public class AssetsDownList : TagHelper
    {
        private readonly IHtmlHelper _html;
        private readonly IAssetsService _service;

        public AssetsDownList(IHtmlHelper helper, IAssetsService constructionService)
        {
            _html = helper;
            _service = constructionService;
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
            var lists = _service.GetAllAsset().Where(x => x.IsActive).ToList();

            var optionGroup = new SelectListGroup() { Name = string.Empty };
            if (SelectedValue != "-2")
            {
                int[] values = lists.Select(x => x.AssetId).ToArray();
                string.Join(",", values.ToArray());
                if (SelectedValue != null && SelectedValue == "-1")               
                    listItems.Add(new SelectListItem { Value = "-1", Text = DefaultText, Selected = true });               
                else              
                    listItems.Add(new SelectListItem { Value = "-1", Text = DefaultText });               
            }
            else
                listItems.Add(new SelectListItem { Value = "", Text = DefaultText});


            var groups = lists.GroupBy(x => x.AssetTypeId).ToList();
            foreach (var group in groups.OrderBy(x=>x.FirstOrDefault().AssetType?.Name))
            {
                var groupSelected = new SelectListGroup() { Name = group.FirstOrDefault().AssetType?.Name };
                foreach (var assetLists in lists.Where(x => x.AssetTypeId == group.Key))
                {
                    SelectListItem selItem = BindeDropDownItem(assetLists, groupSelected);
                    listItems.Add(selItem);
                }
            }

            return listItems;
        }

        private SelectListItem BindeDropDownItem(BDO.Assets item, SelectListGroup group)
        {
            var selItem = new SelectListItem
            {
                Text = item.Name.ToTitleCase(),
                Value = Convert.ToString(item.AssetId),
                Group = group

            };
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                foreach (var select in SelectedValue.Split(','))
                {
                    if (!string.IsNullOrEmpty(select) && item.AssetId == Convert.ToInt32(select))
                        selItem.Selected = true;
                }
            }

            return selItem;
        }
    }
}
