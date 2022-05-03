using HCF.BAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class CommonProjectTypeDDLViewComponent : ViewComponent
    {
        private readonly IConstructionService _constructionService;
        public CommonProjectTypeDDLViewComponent(IConstructionService constructionService)
        {
            _constructionService = constructionService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var projecttype = _constructionService.GetProjectType().ToList();
            return await Task.FromResult(View(projecttype));
        }
    }
}
