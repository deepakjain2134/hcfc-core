using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewModels.Common
{
    public partial record LogoModel 
    {
        public string OrganizationName  { get; set; }

        public string LogoPath { get; set; }
    }
}
