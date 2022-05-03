using HCF.BAL;
using HCF.BDO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class CommonFLoorDDLViewComponent:ViewComponent
    {
        private readonly IFloorService _floorService;
        public CommonFLoorDDLViewComponent(IFloorService floorService)
        {
            _floorService = floorService;

        }
        public async Task<IViewComponentResult> InvokeAsync(int buildingId, int selectedValue, string firstValue, string firstText)
        {
            var values = new List<SelectListItem>();
            List<Floor> floors;
            if (!string.IsNullOrEmpty(firstText))
            {
                var firstItem = new SelectListItem { Value = firstValue, Text = firstText };
                values.Add(firstItem);
            }
            if (buildingId > 0)
            {
                floors = _floorService.GetBuildingFloors(buildingId);
                foreach (var item in floors)
                {
                    var selItem = new SelectListItem
                    {
                        Text = item.FloorName,
                        Value = Convert.ToString(item.FloorId)
                    };
                    if (selectedValue > 0 && item.FloorId == selectedValue)
                        selItem.Selected = true;
                    values.Add(selItem);
                }
            }
            return await Task.FromResult(View(values));
        }
    }
}
