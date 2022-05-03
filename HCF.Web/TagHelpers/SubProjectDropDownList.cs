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
    /// 
    /// </summary>
    [HtmlTargetElement("subprojectdropdownlist")]
    public class SubProjectDropDownList : TagHelper
    {
        private readonly IHtmlHelper _html;
        private readonly ITIcraProjectService _service;

        public SubProjectDropDownList(IHtmlHelper helper, ITIcraProjectService service)
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

        [HtmlAttributeName("projectid")]
        public string ProjectId { get; set; }

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
            var listItems = BindItems();
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

        private List<SelectListItem> BindItems()
        {
            var listItems = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(DefaultText))
            {
                var defaultSelectListItem = new SelectListItem { Text = DefaultText, Value = DefaultValue };
                listItems.Add(defaultSelectListItem);
            }
            bool HideInactiveValue = true;
            if (!string.IsNullOrEmpty(SelectedValue))
                HideInactiveValue = false;

            var values = _service.GetAllActiveTIcraProject(HideInactiveValue)
                .Where(x => x.ParentProjectId == Convert.ToInt32(ProjectId) ||
                x.ParentProjectId == Convert.ToInt32(SelectedValue) &&
                (x.Status == true || x.ProjectId == Convert.ToInt32(SelectedValue))).ToList();

            var projects = values.GroupBy(x => x.ProjectName);            
            foreach (var project in projects)
            {
                var optionGroup = new SelectListGroup() { Name = project.Key };
                listItems.AddRange(project.Select(proj => new SelectListItem() { Value = proj.ProjectId.ToString(), Text = proj.ProjectName + "(" + proj.ProjectNumber + ")", Selected = (proj.ProjectId == Convert.ToInt32(SelectedValue)) }));
            }
            return listItems;
        }
    }
}
