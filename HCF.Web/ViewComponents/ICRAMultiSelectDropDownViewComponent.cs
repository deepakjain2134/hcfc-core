using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class ICRAMultiSelectDropDownViewComponent:ViewComponent
    {
        private readonly IConstructionService _constructionService;
        public ICRAMultiSelectDropDownViewComponent(IConstructionService constructionService)
        {
            _constructionService = constructionService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string controlId)
        {
            var dataModel = _constructionService.GetConstructionRisk();
            ViewBag.controlId = controlId;
            return await Task.FromResult(View(dataModel));
        }
    }
}
