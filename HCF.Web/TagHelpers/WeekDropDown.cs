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
    /// </summary>
    [HtmlTargetElement("weekdropdown")]
    public class WeekDropDown :TagHelper
    {
        private readonly IHtmlHelper _html;
       
        public WeekDropDown(IHtmlHelper helper)
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
            var searchFieldList = new List<SelectListItem> {
                                         new SelectListItem {Text = "First", Value =  "1"},
                                         new SelectListItem {Text = "Second", Value = "2"},
                                         new SelectListItem {Text = "Third", Value = "3"},
                                         new SelectListItem {Text = "Fourth", Value = "4"},
                                         new SelectListItem {Text = "Last", Value = "5"},
                                     };
            foreach (var item in searchFieldList)
            {
                var selItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = Convert.ToString(item.Value)
                };
                listItems.Add(selItem);
            }
            return listItems;
        }
    }
}
