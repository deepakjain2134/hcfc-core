using HCF.BAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class CommonbuildingCustomddlViewComponent :ViewComponent
    {
        private readonly IBuildingsService _buildingsService;
        public CommonbuildingCustomddlViewComponent(IBuildingsService buildingsService)
        {
            _buildingsService = buildingsService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var buildings = _buildingsService.GetBuildings().Where(x => x.IsActive).ToList();
            return await Task.FromResult(View(buildings));
        }
    }
}
