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
    [HtmlTargetElement("binderlistdropdown")]
    public class BinderListDropDown : TagHelper
    {
        private readonly IHtmlHelper _html;
        private readonly IDocumentsService _documentsService;
        public BinderListDropDown(IHtmlHelper helper,IDocumentsService documentsService)
        {
            _html = helper;
            _documentsService = documentsService;
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
            var binders = _documentsService.GetBindersList().Where(x => x.IsActive).OrderBy(x => x.BinderName).ToList();
            listItems.Add(new SelectListItem { Text = DefaultText, Value = DefaultValue });
            foreach (var item in binders)
            {
                var selItem = new SelectListItem
                {
                    Text = item.BinderName,
                    Value = Convert.ToString(item.BinderId)
                };
                if (!string.IsNullOrEmpty(SelectedValue) && selItem.Value== SelectedValue)
                    selItem.Selected = true;
                listItems.Add(selItem);
            }

            return listItems;
        }
    }
}
