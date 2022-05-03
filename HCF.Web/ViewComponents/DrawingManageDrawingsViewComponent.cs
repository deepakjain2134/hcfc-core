using HCF.BAL;
using HCF.BDO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class DrawingManageDrawingsViewComponent : ViewComponent
    {
        private readonly IFloorService _floorService;
        public DrawingManageDrawingsViewComponent(IFloorService floorService)
        {
            _floorService = floorService;

        }
        public async Task<IViewComponentResult> InvokeAsync(string mode, Guid? floorPlanId, string permitId, int? ispopup)
        {
            DrawingViewer viewer = new DrawingViewer();
            if (string.IsNullOrEmpty(mode))
            {
                //viewer.ViewerMode = 2;
                // mode = "drawing";
                viewer.ViewerMode = !string.IsNullOrEmpty(permitId) && permitId == "0" ? 2 : 3;
                viewer.PermitNo = permitId;
                viewer.FloorPlanId = Guid.Parse(floorPlanId.ToString());
            }
            var floorPlan = _floorService.GetFloorPlans(floorPlanId.Value);
            if (floorPlan != null)
            {
                viewer.FloorPlan = floorPlan;
                viewer.PermitNo = permitId;
                viewer.FloorPlanId = Guid.Parse(floorPlanId.ToString());
                viewer.ViewerMode = !string.IsNullOrEmpty(permitId) && permitId != "0" ? 3 : 2;
            }

            if (ispopup.HasValue)
                ViewBag.ispopup = ispopup.Value;


            if (!string.IsNullOrEmpty(permitId))
                ViewBag.permitId = permitId;
            else
                ViewBag.permitId = string.Empty;
            return await Task.FromResult(View(viewer));
        }
    }
}
