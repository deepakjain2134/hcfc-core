using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Web.Base;
using HCF.Web.Filters;
using HCF.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;     
using System.Collections.Generic;
using System.Linq;


namespace HCF.Web.Controllers
{
    [Authorize]
    public class DrawingController : BaseController
    {
           
        private readonly IFloorService _floorService;
        private readonly IFloorTypeService _floorTypeService;

        public DrawingController(IFloorService floorService, IFloorTypeService floorTypeService)
        {
            _floorService = floorService;
            _floorTypeService = floorTypeService;
        }

        public ActionResult Index(string mode, Guid? floorPlanId, string permitId, int? ispopup)
        { 
            DrawingViewer viewer = new DrawingViewer
            {
                ViewerMode = 2
            };
            if (floorPlanId.HasValue)
            {
                viewer.FloorPlanId = floorPlanId.Value;

                var floorPlan = _floorService.GetFloorPlans(floorPlanId.Value);
                if (floorPlan != null)
                {
                    viewer.FloorPlan = floorPlan;
                    viewer.PermitNo = permitId;
                    viewer.FloorPlanId = Guid.Parse(floorPlanId.ToString());
                    viewer.ViewerMode = !string.IsNullOrEmpty(permitId) && permitId != "0" ? 3 : 2;
                }
            }

            if (ispopup.HasValue)
                ViewBag.ispopup = ispopup.Value;

            if (!string.IsNullOrEmpty(permitId))
                ViewBag.permitId = permitId;
            else
                ViewBag.permitId = string.Empty;

            return View(viewer);
        }


        public ActionResult ManageDrawings(string mode, Guid? floorPlanId, string permitId, int? ispopup)
        {
            DrawingViewer viewer = new DrawingViewer();
            if (string.IsNullOrEmpty(mode))
            {
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


            return PartialView("_ManageDrawing", viewer);
        }

        #region Repository FloorPlan       

        public ActionResult FloorPlan(int floorId)
        {
            var floorObject = _floorService.GetFloorWithPlans(floorId);
            var floorTypes = GetDrawingTypeByUser();
            var drawingFloorViewModel = new DrawingFloorViewModel
            {
                FloorTypes = floorTypes,
                FloorPlans = floorObject.FloorPlans.Where(y => floorTypes.Select(x => x.FloorTypeId).Contains(y.FloorPlanTypeId)).ToList(),
                Floor = floorObject

            };
            UISession.AddCurrentPage("Repository_FloorPlan", 0, "Floor Plan History", true);
            return View(drawingFloorViewModel);

        }

        [CrxAuthorize(Roles = "dash_drawings")]
        public ActionResult FloorPlans()
        {
            UISession.AddCurrentPage("Repository_FloorPlans", 0, "Drawings");
            var floorTpes = GetDrawingTypeByUser();
            return View("FloorPlanIndex", floorTpes);
        }

        private List<FloorTypes> GetDrawingTypeByUser()
        {
            var userId = UserSession.CurrentUser.UserId;
            if (UserSession.CurrentUser.IsPowerUser)
                userId = 0;
            var floorTpes = _floorTypeService.GetFloorTypes(userId).ToList();
            return floorTpes;
        }

        public ActionResult DrwaingsFloorPlans(int floorTypeId)
        {
            var floorTpes = new List<FloorTypes>();
            FloorTypes floorType = _floorTypeService.GetFloorTypeFloors(floorTypeId, null);
            ViewBag.floorTypID = floorTypeId;
            if (floorType != null)
                floorTpes.Add(floorType);

            return PartialView("_DrawingsFloorsPlan", floorTpes);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateFloorPlan(int floorId, Guid floorPlanId)
        {
            bool status = _floorService.UpdateFloorPlan(floorId, floorPlanId);
            return Json(status);
        }

        #endregion

    }
}