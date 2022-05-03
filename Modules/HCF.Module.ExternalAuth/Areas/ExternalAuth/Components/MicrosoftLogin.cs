using HCF.Utility.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.ExternalAuth.Areas.ExternalAuth.Components
{
    public class MicrosoftLogin : ViewComponent
    {
        public MicrosoftLogin()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View(this.GetViewPath()));
        }
    }
}
