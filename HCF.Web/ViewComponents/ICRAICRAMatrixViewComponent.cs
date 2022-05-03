using HCF.BAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class ICRAICRAMatrixViewComponent :ViewComponent
    {
        private readonly IConstructionService _constructionService;
        public ICRAICRAMatrixViewComponent(IConstructionService constructionService)
        {
            _constructionService = constructionService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string Version)
        {
            ViewBag.Constructiontype = _constructionService.GetConstructionType();
            ViewBag.ConstructionRisk = _constructionService.GetConstructionRisk();
            int Versionnum = 1;
            if (!string.IsNullOrEmpty(Version))
            {
                Versionnum = Convert.ToInt32(Version);
            }
            var lists = _constructionService.GetICRAMatixPrecautions().Where(x => x.Version == Convert.ToInt32(Versionnum)).ToList() ;
            return await Task.FromResult(View(lists));
        }
    }
}
