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
using HCF.Utility;

namespace HCF.Web.TagHelpers
{
    public class PermitDropDownList : TagHelper
    {
        private readonly IHtmlHelper _html;
        
        public PermitDropDownList(IHtmlHelper helper)
        {
            _html = helper;
            
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
        [HtmlAttributeName("required")]
        public string Required { get; set; }
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
            if (Required == "required")
                customSelectPicker.IsRequired = true;

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
            var lists = from BDO.Enums.PermitType e in Enum.GetValues(typeof(BDO.Enums.PermitType))
                        select new
                        {
                            Value = (int)e,
                            Text = e.GetDescription()
                        };
            foreach (var item in lists.OrderBy(x => x.Text))
            {

                var selItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = Convert.ToString(item.Value)
                };
                if (!string.IsNullOrEmpty(SelectedValue) && SelectedValue == selItem.Value)
                    selItem.Selected = true;
                listItems.Add(selItem);
            }
            return listItems;
        }
    }
}
