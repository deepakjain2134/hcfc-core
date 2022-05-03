using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace HCF.Web.ViewModels.Common
{
    public class CustomSelectPicker
    {
        public CustomSelectPicker()
        {
            Attributes = new Dictionary<string, string>();
        }

        public CustomSelectPicker(string id, string firstText, string firstValue,string selectedValue)
        {
            Name = id;
            DefaultText = firstText;
            DefaultValue=firstValue;
            SelectedValue = selectedValue;

        }

        public List<SelectListItem> Items { get; set; }

        public List<string> SelectedIds { get; set; }
        

        public bool IsDisabled { get; set; }

        public string Name { get; set; }

        public string ClassName { get; set;}
       
        public string ID { get; set; }

        public bool IsMultiple { get; set; }

        public bool IsRequired { get; set; }

        public string RecValue { get; set; }
        public string minValue { get; set; }
        public string maxvalue { get; set; }

        public string DefaultValue { get; set; }

        public string DefaultText { get; set; }


        public string SelectedValue { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
}
