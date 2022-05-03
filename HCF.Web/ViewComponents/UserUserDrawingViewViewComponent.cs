using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class UserUserDrawingViewViewComponent : ViewComponent
    {
        private static IFloorTypeService _floorTypeService;
        public UserUserDrawingViewViewComponent(IFloorTypeService floorTypeService)
        {
            _floorTypeService = floorTypeService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? userId, int? vendorId, long[] selectedValues)
        {
            var data = _floorTypeService.GetFloorTypes().ToList();
            var model = new UserDrawingViewModel
            {
                Drawings = data,
                UserId = userId,
                VendorId = vendorId,
                SelectedItems = selectedValues
            };
            return await Task.FromResult(View(model));
        }
    }
}
