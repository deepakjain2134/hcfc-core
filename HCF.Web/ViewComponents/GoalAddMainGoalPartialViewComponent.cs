using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class GoalAddMainGoalPartialViewComponent :ViewComponent
    {
        private readonly IEpService _epService;
        private readonly IAssetsService _assetService;
        private readonly IMainGoalService _mainGoalService;
        private readonly IFloorAssetService _floorAssetService;
        public GoalAddMainGoalPartialViewComponent(IMainGoalService mainGoalService,
            IEpService epService,
            IAssetsService assetService,
            IFloorAssetService floorAssetService
            )
        {
            _floorAssetService = floorAssetService;
            _assetService = assetService;
            _epService = epService;
            _mainGoalService = mainGoalService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? epdetailid, int? assetId, int? floorAssetId = null)
        {
            List<MainGoal> model = GetMainGoalView(epdetailid, assetId, floorAssetId);
            return await Task.FromResult(View(model));
        }

        private List<MainGoal> GetMainGoalView(int? epdetailid, int? assetId, int? floorAssetId)
        {
            var model = new List<MainGoal>();
            if (epdetailid.HasValue && epdetailid.Value > 0)
            {
                ViewBag.EpdetailId = epdetailid;
                var eps = _epService.GetEpDescription(epdetailid);
                ViewBag.CurrentEP = eps;
                model = _mainGoalService.GetAllMainGoal(epdetailid);
            }
            else if (assetId.HasValue && floorAssetId.HasValue)
            {
                ViewBag.FloorAssetId = floorAssetId ?? floorAssetId;
                var assets = _assetService.Get(assetId.Value);
                if (assets != null)
                    assets.EPdetails = _assetService.GetAssetEp(assetId.Value);
                assets.TFloorAsset = _floorAssetService.GetFloorAsset(floorAssetId.Value);
                ViewBag.CurrentAssets = assets;
                //model = _mainGoalService.GetAssetMainGoals(assetId.Value);
                model = _mainGoalService.GetMainGoalsbyFloorAssetId(HCF.Web.Base.UserSession.CurrentOrg.ClientNo, floorAssetId.Value, assetId.Value);
            }
            else if (assetId.HasValue)
            {
                ViewBag.AssetId = assetId;
                var assets = _assetService.Get(assetId.Value);
                if (assets != null)
                    assets.EPdetails = _assetService.GetAssetEp(assetId.Value);
                ViewBag.CurrentAssets = assets;
                model = _mainGoalService.GetAssetMainGoals(assetId.Value);
            }
            return model;
        }
    }
}
