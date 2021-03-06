using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.Utility;
using HCF.Web.ViewModels.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HCF.Web.TagHelpers
{
    [HtmlTargetElement("doccategorydropdown")]
    public class DocCategoryDropdown : TagHelper
    {
        [HtmlAttributeName("name")]
        public string ControlName { get; set; }

        [HtmlAttributeName("firstvalue")]
        public string DefaultValue { get; set; }

        [HtmlAttributeName("firsttext")]
        public string DefaultText { get; set; }

        [HtmlAttributeName("selectedvalue")]
        public string SelecteValue { get; set; }

        [HtmlAttributeName("required")]
        public string Required { get; set; }

        [HtmlAttributeName("class")]
        public string ClassName { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        private readonly IHtmlHelper _html;

        public DocCategoryDropdown(IHtmlHelper helper)
        {
            _html = helper;
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


            var listItems = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(DefaultText))
            {
                var defaultSelectListItem = new SelectListItem { Text = DefaultText, Value = DefaultValue };
                listItems.Add(defaultSelectListItem);
            }
            var lists = from BDO.Enums.DocumentCategory e in Enum.GetValues(typeof(BDO.Enums.DocumentCategory))
                        select new
                        {
                            Value = (int)e,
                            Text = e.GetDescription()
                        };
            foreach (var item in lists)
            {
                var selItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = Convert.ToString(item.Value)
                };
                if (!string.IsNullOrEmpty(SelecteValue))
                    selItem.Selected = true;
                listItems.Add(selItem);
            }

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
    }
}
