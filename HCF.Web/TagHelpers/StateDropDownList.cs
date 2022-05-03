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
    [HtmlTargetElement("statedropdownlist")]
    public class StateDropDownList : TagHelper
    {
        private readonly IHtmlHelper _html;
        private readonly ISiteService _service;

        public StateDropDownList(IHtmlHelper helper, ISiteService service)
        {
            _html = helper;
            _service = service;
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
        Dictionary<string, string> attributes = new Dictionary<string, string>();
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Contextualize the ViewContext
            ((IViewContextAware)_html).Contextualize(ViewContext);

            // Make sure we don't have any tags associated with this TagHelper.
            output.TagName = string.Empty;
            foreach (var item in context.AllAttributes)
            {
                if (item.Name.StartsWith("asp-for"))
                    attributes.Add(item.Name.Replace("asp-for-", ""), Convert.ToString(item.Value));
            }
            // DI'd into the constructor
            IHtmlContent content = null;
            var listItems = BindItems();
            var customSelectPicker = new CustomSelectPicker
            {
                Items = listItems,
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
                var defaultSelectListItem = new SelectListItem { Text = DefaultText, Value = DefaultValue };
                listItems.Add(defaultSelectListItem);
            }
            var state = _service.GetStates().Where(x => x.IsActive).ToList();
            foreach (var item in state)
            {
                var selItem = new SelectListItem { Text = item.StateName + " ( " + item.StateCode + " )", Value = Convert.ToString(item.StateId) };
                if (!string.IsNullOrEmpty(SelectedValue) &&
                   SelectedValue == selItem.Value)
                    selItem.Selected = true;
                listItems.Add(selItem);
            }
            return listItems;
        }
    }
}
