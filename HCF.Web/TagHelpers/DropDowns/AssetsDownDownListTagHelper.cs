using HCF.BAL.Interfaces.Services;
using HCF.Utility;
using HCF.Utility.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.TagHelpers.DropDowns
{


    [HtmlTargetElement("assets-select", TagStructure = TagStructure.WithoutEndTag)]
    public class AssetsDownDownListTagHelper : TagHelper
    {
        #region Constants

        private const string FOR_ATTRIBUTE_NAME = "asp-for";
        private const string NAME_ATTRIBUTE_NAME = "asp-for-name";
        private const string ITEMS_ATTRIBUTE_NAME = "asp-items";
        private const string MULTIPLE_ATTRIBUTE_NAME = "asp-multiple";
        private const string REQUIRED_ATTRIBUTE_NAME = "asp-required";
        private const string SELECTEDVALUE_ATTRIBUTE_NAME = "asp-selected";
        private const string DEFAUTL_TEXT_ATTRIBUTE_NAME = "asp-default-text";
        private const string DEFAUTL_VALUE_ATTRIBUTE_NAME = "asp-default-value";

        #endregion

        #region Properties

        /// <summary>
        /// An expression to be evaluated against the current model
        /// </summary>
        [HtmlAttributeName(FOR_ATTRIBUTE_NAME)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Name for a dropdown list
        /// </summary>
        [HtmlAttributeName(NAME_ATTRIBUTE_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Items for a dropdown list
        /// </summary>
        [HtmlAttributeName(ITEMS_ATTRIBUTE_NAME)]
        public IEnumerable<SelectListItem> Items { set; get; } = new List<SelectListItem>();

        /// <summary>
        /// Indicates whether the field is required
        /// </summary>
        [HtmlAttributeName(REQUIRED_ATTRIBUTE_NAME)]
        public string IsRequired { set; get; }

        /// <summary>
        /// Indicates whether the input is multiple
        /// </summary>
        [HtmlAttributeName(MULTIPLE_ATTRIBUTE_NAME)]
        public string IsMultiple { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(SELECTEDVALUE_ATTRIBUTE_NAME)]
        public string SelectedValue { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(DEFAUTL_TEXT_ATTRIBUTE_NAME)]
        public string DefaultText { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(DEFAUTL_VALUE_ATTRIBUTE_NAME)]
        public string DefaultValue { get; set; }

        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        #endregion

        #region Ctor

        public AssetsDownDownListTagHelper(IHtmlHelper helper, IAssetsService service)
        {
            _htmlHelper = helper;
            _service = service;
        }

        #endregion

        #region Fields

        private readonly IHtmlHelper _htmlHelper;
        private readonly IAssetsService _service;

        #endregion

        #region Methods

        /// <summary>
        /// Asynchronously executes the tag helper with the given context and output
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            //clear the output
            output.SuppressOutput();

            //required asterisk
            if (bool.TryParse(IsRequired, out var required) && required)
            {
                output.PreElement.SetHtmlContent("<div class='input-group input-group-required'>");
                output.PostElement.SetHtmlContent("<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>");
            }

            //contextualize IHtmlHelper
            var viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            //get htmlAttributes object
            var htmlAttributes = new Dictionary<string, object>();
            var attributes = context.AllAttributes;
            foreach (var attribute in attributes)
            {
                if (!attribute.Name.Equals(FOR_ATTRIBUTE_NAME) &&
                    !attribute.Name.Equals(NAME_ATTRIBUTE_NAME) &&
                    !attribute.Name.Equals(ITEMS_ATTRIBUTE_NAME) &&
                    !attribute.Name.Equals(MULTIPLE_ATTRIBUTE_NAME))                    
                {
                    htmlAttributes.Add(attribute.Name, attribute.Value);
                }
            }

            Items = BindItems();
            //generate editor
            var tagName = For != null ? For.Name : Name;
            bool.TryParse(IsMultiple, out var multiple);
            if (!string.IsNullOrEmpty(tagName))
            {
                IHtmlContent selectList;
                if (multiple)
                {
                    var templateName = For.ModelExplorer.ModelType == typeof(List<string>) ? "MultiSelectString" : "MultiSelect";
                    selectList = _htmlHelper.Editor(tagName, templateName, new { htmlAttributes, SelectList = Items });
                }
                else
                {
                    if (htmlAttributes.ContainsKey("class"))
                        htmlAttributes["class"] += " form-control";
                    else
                        htmlAttributes.Add("class", "form-control");

                    selectList = _htmlHelper.DropDownList(tagName, Items, htmlAttributes);
                }
                output.Content.SetHtmlContent(await selectList.RenderHtmlContentAsync());
            }
        }     

        private List<SelectListItem> BindItems()
        {
            var listItems = new List<SelectListItem>();
            var lists = _service.GetAllAsset().Where(x => x.IsActive).ToList();

            if (SelectedValue != "-2")
            {
                int[] values = lists.Select(x => x.AssetId).ToArray();
                string.Join(",", values.ToArray());
                if (SelectedValue != null && SelectedValue == "-1")
                    listItems.Add(new SelectListItem { Value = "-1", Text = DefaultText, Selected = true });
                else
                    listItems.Add(new SelectListItem { Value = "-1", Text = DefaultText });
            }
            else
                listItems.Add(new SelectListItem { Value = "", Text = DefaultText });


            var groups = lists.GroupBy(x => x.AssetTypeId).ToList();
            foreach (var group in groups.OrderBy(x => x.FirstOrDefault().AssetType?.Name))
            {
                var groupSelected = new SelectListGroup() { Name = group.FirstOrDefault().AssetType?.Name };
                foreach (var assetLists in lists.Where(x => x.AssetTypeId == group.Key))
                {
                    SelectListItem selItem = BindeDropDownItem(assetLists, groupSelected);
                    listItems.Add(selItem);
                }
            }

            return listItems;
        }

        private SelectListItem BindeDropDownItem(BDO.Assets item, SelectListGroup group)
        {
            var selItem = new SelectListItem
            {
                Text = item.Name.ToTitleCase(),
                Value = Convert.ToString(item.AssetId),
                Group = group

            };
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                foreach (var select in SelectedValue.Split(','))
                {
                    if (!string.IsNullOrEmpty(select) && item.AssetId == Convert.ToInt32(select))
                        selItem.Selected = true;
                }
            }

            return selItem;
        }

        #endregion
    }
}
