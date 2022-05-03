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
using HCF.Web.Models;

namespace HCF.Web.TagHelpers
{
    [HtmlTargetElement("commonseachdropdown")]
    public class CommonSeachDropDown: TagHelper
    {
        private readonly IHtmlHelper _html;
        
        public CommonSeachDropDown(IHtmlHelper helper)
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
            if (!string.IsNullOrEmpty(DefaultText))
            {
                var defaultSelectListItem = new SelectListItem { Text = DefaultText, Value = DefaultValue };
                listItems.Add(defaultSelectListItem);
            }
            var searchFieldList = new
             List<SelectedList> {
                                         new SelectedList {Text = "(All Fields)", Value =  string.Empty},
                                         new SelectedList {Text = "Asset #", Value = "AssetNo"},
                                         new SelectedList {Text = "Barcode #", Value = "BarCodeNo"},
                                         new SelectedList {Text = "Binder Name", Value = "BinderName"},
                                         new SelectedList {Text = "ILSM Incident #", Value = "IncidentNo"}
                                     };
            foreach (var item in searchFieldList)
            {
                var selItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = Convert.ToString(item.Value)
                };
                if (!string.IsNullOrEmpty(SelectedValue) && selItem.Value == SelectedValue)
                    selItem.Selected = true;
                listItems.Add(selItem);

            }
            return listItems;
        }
    }
}
