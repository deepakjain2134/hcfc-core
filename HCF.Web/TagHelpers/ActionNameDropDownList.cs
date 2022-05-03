using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HCF.Web.ViewModels.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HCF.Web.TagHelpers
{
    /// <summary>
    /// 
    /// Need to work on this
    /// </summary>
    [HtmlTargetElement("actionnamedropdownlist")]
    public class ActionNameDropDownList : TagHelper
    {
        private readonly IHtmlHelper _html;
        public ActionNameDropDownList(IHtmlHelper helper)
        {
            _html = helper;
        }

        [HtmlAttributeName("name")]
        public string ControlName { get; set; }

        [HtmlAttributeName("firstvalue")]
        public string DefaultValue { get; set; }

        [HtmlAttributeName("firsttext")]
        public string DefaultText { get; set; }

        [HtmlAttributeName("selectevalue")]
        public string SelecteValue { get; set; }

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

            // If we don't have an image, return the noImage.cshtml


            var listItems = new List<SelectListItem>();
            var asm = Assembly.GetExecutingAssembly();

            var actionMethods = asm.GetTypes()
              .Where(type => typeof(Controller).IsAssignableFrom(type))
              .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
              .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
              .Select(x => new
              {
                  Controller = x.DeclaringType.Name,
                  ActionMethod = x.Name,
                  // Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
              })
     .ToList();

            listItems.Add(new SelectListItem { Value = string.Empty, Text = DefaultText });
            //var controllerName = htmlAttributes.ToString().Replace("{", "").Replace("}", "").Split(',')[0].Split('=')[1].Trim().ToLower() + "controller";
            //actionMethods = actionMethods.Where(x => x.Controller.ToLower() == controllerName).Distinct().ToList();
            //foreach (var item in actionMethods)
            //{
            //    var selItem = new SelectListItem { Text = item.ActionMethod, Value = item.ActionMethod };
            //    if (selItem != null && selectedActionMethod == item.ActionMethod)
            //        selItem.Selected = true;
            //    listItems.Add(selItem);
            //}

            var customSelectPicker = new CustomSelectPicker
            {
                Items = listItems,
                Name = ControlName,
                ClassName = ClassName
            };

            content = await _html.PartialAsync("TagHelpers/CommonDropdownList", customSelectPicker);
            output.Content.SetHtmlContent(content);
        }
    }
}
