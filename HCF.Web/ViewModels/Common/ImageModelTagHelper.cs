using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewModels.Common
{
    public class ImageModelTagHelper
    {
        public string ClassName
        {
            get; set;
        }
        public string Title { get; set; }

        public Uri Url { get; set; }
    }
}
