using HCF.BAL;
using HCF.BDO;
using HCF.Web.Base;
using HCF.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HCF.Web.Controllers
{
    public class SiteController : BaseController
    {
        private readonly ISiteService _siteService;
        private readonly IBuildingsService _buildingsService;      
        public SiteController(ISiteService siteService, IBuildingsService buildingsService)
        {          
            _buildingsService = buildingsService;
            _siteService = siteService;
        }

        // GET: Site

        #region BBI Tracking 
        [CrxAuthorize(Roles = "setup_bbi")]
        public ActionResult BBI(int? viewmode)
        {
            UISession.AddCurrentPage("Site_BBI", 0, "BBI Tracking");

            ViewBag.IsView = 1;
            if (viewmode.HasValue && viewmode == 0)
            {
                ViewBag.IsView = 0;
            }
            var sites = _siteService.GetBBISitesBuildings();
            return View(sites);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBBI(BuildingDetails buildingDetails)
        {
            buildingDetails.CreatedBy = UserSession.CurrentUser.UserId;
            buildingDetails.CreatedBy = UserSession.CurrentUser.UserId;
            int BBIId = _buildingsService.Insert_BBI(buildingDetails);
            return RedirectToAction("BBI");
        }

        public JsonResult DeleteBBI(int id)
        {
            var result = _buildingsService.DeleteBbi(id);
            return Json(result);
        }
        #endregion

        #region Fire System Type  
        public ActionResult firesystemtype()
        {
            UISession.AddCurrentPage("Site_firesystemtype", 0, "Fire System Type");
            List<FireSystemType> lstfiresystemtype = new List<FireSystemType>();
            lstfiresystemtype = _siteService.GetFireSystemTypeDetails();
            if (lstfiresystemtype.Count == 0)
            {
                FireSystemType firesystemtype = new FireSystemType();
                lstfiresystemtype.Add(firesystemtype);
            }
            ViewBag.sites = _siteService.GetBBISitesBuildings();
            return View(lstfiresystemtype);
        }

        public ActionResult BindFireSystemList(int sequence)
        {
            ViewData["sites"] = _siteService.GetBBISitesBuildings();
            FireSystemType firesystemtype = new FireSystemType();
            ViewData["sequence"] = sequence;
            return PartialView("_FireSystemType", firesystemtype);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFireSystem(List<FireSystemType> FireSystemType)
        {
            foreach (var item in FireSystemType)
            {
                if (!string.IsNullOrEmpty(item.Name))
                {
                    item.CreatedBy = UserSession.CurrentUser.UserId;
                    int BBIId = _siteService.Insert_FireSystemType(item);
                }
            }
            return RedirectToAction("firesystemtype");
        }
        #endregion
    }
}