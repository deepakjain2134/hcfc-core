using HCF.BDO;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;

namespace HCF.Web.ViewComponents
{
    public class GoalAssetsInspectionViewComponent : ViewComponent
    {
        private readonly ITransactionService _transactionService;
        private readonly IInspectionService _inspectionService;
        private readonly IFloorAssetService _floorAssetService;
        public GoalAssetsInspectionViewComponent(IInspectionService inspectionService, ITransactionService transactionService, IFloorAssetService floorAssetService)
        {
            _transactionService = transactionService;
            _floorAssetService = floorAssetService;
            _inspectionService = inspectionService;

        }

        public async Task<IViewComponentResult> InvokeAsync(int? epId, int? inspectionId, int? floorAssetId, int? mode, int ismultipleEP = 0)
        {
            var userId = UserSession.CurrentUser.UserId;
            var epDetail = new EPDetails();
            if (epId > 0)
            {
                var floorAssets = new TFloorAssets();
                epDetail = _transactionService.GetInspectionHistory(Convert.ToInt32(userId), epId == null ? 0 : Convert.ToInt32(epId), inspectionId == null ? 0 : Convert.ToInt32(inspectionId));

                if (floorAssetId > 0)
                    floorAssets = _floorAssetService.GetFloorAssetDescription(floorAssetId.Value);

                ViewBag.FloorAssetId = floorAssetId;
                if (floorAssets.Floor != null)
                {
                    ViewBag.FloorId = floorAssets.FloorId;
                    if (floorAssets.Floor.Building != null)
                        ViewBag.BuildingID = floorAssets.Floor.BuildingId;
                }
                var pageMode = "assetEps";

                if (mode.HasValue)
                    pageMode = (mode.Value == 1) ? "history" : "assetshistory";

                ViewBag.PageMode = pageMode;

                ViewBag.ismultipleEP = TempData["ismulipleEp"] ?? ismultipleEP;
                //TempData.Remove("ismulipleEp");
            }
            if (inspectionId.HasValue)
            {
                ViewBag.IshowDoc = 0;
            }
            else
            {
                ViewBag.IshowDoc = 1;
            }
            if (epDetail != null && epDetail.EPDetailId > 0)
            {
                epDetail.EPsDocument = _transactionService.GetEpsDocument(epDetail.EPDetailId);
                if (epDetail.Inspection != null && epDetail.Inspection.InspectionId > 0)
                {
                    epDetail.InspectionEPDocs = _transactionService.GetInspectionDocs(epDetail.Inspection.InspectionId).ToList();
                }
            }
            return await Task.FromResult(View(epDetail));
        }
    }
}
