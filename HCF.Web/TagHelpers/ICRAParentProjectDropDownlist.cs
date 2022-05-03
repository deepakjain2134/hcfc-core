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
    public class ICRAParentProjectDropDownlist : TagHelper
    {
        private readonly IHtmlHelper _html;
        private readonly ITIcraProjectService _constructionService;

        public ICRAParentProjectDropDownlist(IHtmlHelper helper, ITIcraProjectService constructionService)
        {
            _html = helper;
            _constructionService = constructionService;
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

        [HtmlAttributeName("hideinactive")]
        public bool HideInActive { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("required")]
        public string Required { get; set; }
        Dictionary<string, string> attributes = new Dictionary<string, string>();
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
            var listItems = BindItems(HideInActive);
            var customSelectPicker = new CustomSelectPicker
            {
                Items = listItems,
                Name = ControlName,
                ClassName = ClassName,
                Attributes = attributes
            };
            if (Required == "required")
                customSelectPicker.IsRequired = true;

            content = await _html.PartialAsync("TagHelpers/CommonDropdownList", customSelectPicker);
            output.Content.SetHtmlContent(content);
        }

        private List<SelectListItem> BindItems(bool HideInActive = true)
        {
            var listItems = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(DefaultText))
            {
                var defaultSelectListItem = new SelectListItem { Text = DefaultText, Value = DefaultValue };
                listItems.Add(defaultSelectListItem);
            }
            bool HideInactiveValue = true;
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                HideInactiveValue = false;
            }
            var data = _constructionService.GetAllActiveTIcraProject(HideInActive);
            if (!HideInactiveValue)
            {
                data = data.Where(x => x.Status == true || x.ProjectId == Convert.ToInt32(SelectedValue)).ToList();
            }
            else
            {
                data = data.Where(x => x.Status == true).ToList();
            }
            var icraProjectList = data.Where(x => x.ParentProjectId == null).ToList();

            foreach (var item in icraProjectList)
            {
                var selItem = new SelectListItem
                {
                    Text = $@"{item.ProjectName} ({item.ProjectNumber})",
                    Value = Convert.ToString(item.ProjectId)
                };
                if (!string.IsNullOrEmpty(SelectedValue) && SelectedValue == selItem.Value)
                    selItem.Selected = true;
                listItems.Add(selItem);
            }
            return listItems;
        }
    }
}
