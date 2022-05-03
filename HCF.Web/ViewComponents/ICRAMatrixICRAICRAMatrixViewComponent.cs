using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class ICRAMatrixICRAICRAMatrixViewComponent :ViewComponent
    {
        public ICRAMatrixICRAICRAMatrixViewComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View());
        }
    }
}
