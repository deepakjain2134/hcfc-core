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
    [HtmlTargetElement("roundcommoncategorydropdown")]
    public class RoundCommonCategoryDropDown : TagHelper
    {
        private readonly IHtmlHelper _html;
        private readonly IRoundsService _constructionService;

        public RoundCommonCategoryDropDown(IHtmlHelper helper, IRoundsService service)
        {
            _html = helper;
            _constructionService = service;
        }

        [HtmlAttributeName("name")]
        public string ControlName { get; set; }

        [HtmlAttributeName("firstvalue")]
        public string DefaultValue { get; set; }

        [HtmlAttributeName("firsttext")]
        public string DefaultText { get; set; }

        [HtmlAttributeName("selectevalue")]
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
            var roundsCateg =_constructionService.GetCommonRoundCategory().Where(x => x.IsActive).ToList();
          
            foreach (var item in roundsCateg)
            {
                var selItem = new SelectListItem
                {
                    Text = item.CategoryName,
                    Value = Convert.ToString(item.RoundCatId)
                };
                if (!string.IsNullOrEmpty(SelectedValue) && SelectedValue == selItem.Value)
                    selItem.Selected = true;
                listItems.Add(selItem);

            }
            return listItems;
        }
    }
}
