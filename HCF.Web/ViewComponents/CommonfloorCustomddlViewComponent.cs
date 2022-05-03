using HCF.BAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class CommonfloorCustomddlViewComponent :ViewComponent
    {
        private readonly IFloorService _floorService;
        public CommonfloorCustomddlViewComponent(IFloorService floorService)
        {
            _floorService = floorService;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var floors = _floorService.GetFloors().Where(x => x.IsActive).ToList();
            return await Task.FromResult(View(floors));
        }
    }
}
