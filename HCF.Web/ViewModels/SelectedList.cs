using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.Web.Models
{
    public class SelectedList
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class SelectPicker
    {

        public bool IsMultiple { get; set; }
        public int SelectedValue { get; set; }

        public bool ShowEmail { get; set; }
        public bool ShowEpCount { get; set; }
    }
    public class MultiSelectPicker
    {

        public bool IsMultiple { get; set; }
        public string SelectedValue { get; set; }



        public bool ShowEmail { get; set; }
    }
}