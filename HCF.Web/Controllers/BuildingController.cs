using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Shyjus.BrowserDetection;

namespace HCF.Web.Controllers
{  
    public class BuildingController : BaseController  
    {
        #region ctor

        private readonly IAssetsService _assetService;      
        private readonly IBuildingsService _buildingsService;
        private readonly IBrowserDetector _browserDetector;
        public BuildingController(IAssetsService assetService, IBuildingsService buildingsService, IBrowserDetector browserDetector)
        {
            _assetService = assetService;
            _buildingsService = buildingsService;          
            _browserDetector = browserDetector;
        }

        #endregion

        public ActionResult Index()
        {
            Base.UISession.AddCurrentPage("Building_Index", 0, "Building & Floors");
            var buildings = _buildingsService.GetBuildingFloors("", "", "");
            ViewBag.PageMode = "FE";
            ViewBag.OsName = _browserDetector.Browser.OS;
            return View(buildings);
        }

        [AjaxOnly]
        public ActionResult GetAssetsFrequencies(int assetId)
        {
            var assetEps = _assetService.GetAssetEp(assetId);
            return PartialView("_AssetsEps", assetEps);
        }

        public ActionResult RouteInspection(int? assetsid)
        {
            Base.UISession.AddCurrentPage("Building_RouteInspection", 0, "Building & Floors");
            var buildings = new List<Buildings>();
            ViewBag.PageMode = "FE";
            ViewBag.OsName = _browserDetector.Browser.OS;
            return View("Index", buildings);
        }
    }
}