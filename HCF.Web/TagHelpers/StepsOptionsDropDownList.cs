using System;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.Web.ViewModels.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace HCF.Web.TagHelpers
{
    [HtmlTargetElement("stepsoptionsdropdownlist")]
    public class StepsOptionsDropDownList : TagHelper
    {
        private readonly IHtmlHelper _html;
        private readonly IQuestionnairesService _service;

        public StepsOptionsDropDownList(IHtmlHelper helper, IQuestionnairesService service)
        {
            _html = helper;
            _service = service;
        }

        [HtmlAttributeName("name")]
        public string ControlName { get; set; }

        [HtmlAttributeName("showOnlyData")]
        public string ShowOnlyData { get; set; }

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

        [HtmlAttributeName("recvalue")]
        public string recvalue { get; set; }

        [HtmlAttributeName("minValue")]
        public string minValue { get; set; }

        [HtmlAttributeName("maxvalue")]
        public string maxvalue { get; set; }



        Dictionary<string, string> attributes = new Dictionary<string, string>();

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            //var currentAttribute = output.Attributes;
            foreach (var item in output.Attributes)
            {
                if (item.Name.StartsWith("asp-for"))                
                    attributes.Add(item.Name, Convert.ToString(item.Value));                
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
                maxvalue= maxvalue,
                RecValue=recvalue,
                minValue=minValue,
                Attributes = attributes

            };
            if (!string.IsNullOrEmpty(Multiple) && Multiple.ToLower() == "multiple")
                customSelectPicker.IsMultiple = true;
            content = await _html.PartialAsync("TagHelpers/CommonStepsDropdownList", customSelectPicker);
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
            var mainList = _service.QuestionnaireHeaderTypes().Where(x => x.IsActive == true).ToList();
            var array = Array.ConvertAll(ShowOnlyData.Split(','), int.Parse);
            var lists = mainList.Where(d => array.Contains(d.QuestionnaireHeaderTypeId));
            foreach (var item in lists)
            {
                var selItem = new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Name//Convert.ToString(item.QuestionnaireHeaderTypeId);
                };
                if (!(string.IsNullOrEmpty(SelectedValue)) &&
                    selItem.Text == Convert.ToString(SelectedValue))
                    selItem.Selected = true;
                else
                    selItem.Selected = false;
                listItems.Add(selItem);

            }
            return listItems;
        }
    }
}
