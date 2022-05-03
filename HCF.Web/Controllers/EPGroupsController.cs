using HCF.BDO;
using HCF.Web.Base;
using System.Linq;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class EPGroupsController : BaseController
    {
        private readonly IVendorService _vendorService;
        private readonly IEpGroupsService _epGroupService;
        

        public EPGroupsController(IVendorService vendorService, IEpGroupsService epGroupService) 
        {           
            _vendorService = vendorService;
            _epGroupService = epGroupService;

        }
        // GET: EPGroups
        public ActionResult GetEPGroups(int? epgroupId, int mode = 0, int checkboxmode = 0)  // 0 for slider and 1 for popup
        {
            var epgroups = _epGroupService.GetEPGroups(epgroupId);
            ViewBag.Checkbox = checkboxmode;
            return PartialView(mode == 0 ? "_epgroups" : "_epgroupseps", epgroups);
        }

        public ActionResult VendorReports()
        {
            UISession.AddCurrentPage("EPGroups_VendorReports", 0, "Vendor Reports");
            var vendorSelectListItems = _vendorService.GetVendors().Where(x => x.IsActive).Select(x => new SelectListItem { Text = x.Name, Value = x.VendorId.ToString() }).ToList();
            return View(vendorSelectListItems);
        }

        public ActionResult GetVendorEPGroups(int vendorId)
        {
            var epgroups = _epGroupService.GetVendorEPGroups(vendorId);
            return PartialView("_vendorEpgroups", epgroups);
        }

        public ActionResult GetEPGroupsNameList()
        {
            UISession.AddCurrentPage("EPGroups_EPGroups", 0, "EP Groups");
            var epGroupName = _epGroupService.GetEPGroupNameList();
            return View(epGroupName);
        }

        [HttpGet]
        public ActionResult AddEPGroupsName(int? epGroupId)
        {
            var ePGroups = new EPGroups();
            if (epGroupId.HasValue && epGroupId.Value > 0)
                ePGroups = _epGroupService.GetEPGroupName(epGroupId.Value);
            return View(ePGroups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEPGroupsName(EPGroups ePGroups)
        {
            if (!ModelState.IsValid)
                return View();

            var isGroupExists = _epGroupService.GetEPGroupNameList().Any(x => x.EPGroupName == ePGroups.EPGroupName);
            if (!isGroupExists)
            {
                _epGroupService.AddEPGroupsName(ePGroups);
                SuccessMessage = Utility.ConstMessage.Saved_Successfully;
            }
            else
                ErrorMessage = Utility.ConstMessage.EpGroup_Already_Exist;
            return RedirectToAction("GetEPGroupsNameList");
        }

        public ActionResult AsignEPs(int? epgroupId)
        {
            var epGroups = _epGroupService.AssignEpsList(epgroupId);
            ViewBag.epdetailcheck = epGroups;
            ViewBag.epgroupid = epgroupId;
            return PartialView("_AsignEPs", epGroups);
        }

        public JsonResult AssignEPsGroup(int epdetailId, int epGruopId, bool IsActive)
        {
            int result = _epGroupService.AssignEPsGroup(epdetailId, epGruopId, IsActive);
            return Json(result);
        }

        #region epgroups
        public ActionResult EPGroups()
        {
            UISession.AddCurrentPage("Menu_EPGroups", 0, "EP Groups");
            var epGroups = _epGroupService.GetVendorEPGroups(0);
            return View(epGroups);
        }


        [HttpGet]
        public ActionResult AddEPGroup(int? epGroupId)
        {
            var ePGroups = new EPGroups();
            if (epGroupId.HasValue && epGroupId.Value > 0)
                ePGroups = _epGroupService.GetEPGroupName(epGroupId.Value);
            return View(ePGroups);
        }

        [HttpPost]
        public ActionResult AddEPGroup(EPGroups ePGroups)
        {
            if (!ModelState.IsValid)
                return View();

            var isGroupExists = _epGroupService.GetEPGroupNameList().Any(x => x.EPGroupName == ePGroups.EPGroupName);
            if (!isGroupExists)
            {
                _epGroupService.AddEPGroupsName(ePGroups);
                SuccessMessage = Utility.ConstMessage.Saved_Successfully;
            }
            else               
                ErrorMessage = Utility.ConstMessage.EpGroup_Already_Exist;
            return RedirectToAction("EPGroups");
        }


        public ActionResult EPsAssign(int epgroupId, string epgroupname, string searchText)
        {
            var epGroups = _epGroupService.AssignEpsList(epgroupId);
            ViewBag.epdetail = epGroups;
            ViewBag.EPGroupID = epgroupId;
            ViewBag.EPGroupName = epgroupname;
            if (searchText != null)
            {
                var standardEps = epGroups.FirstOrDefault()?.StandardEps.Where(x => x.StandardEP.Contains(searchText)).ToList();
                foreach (var item in epGroups)
                {
                    item.StandardEps.Clear();
                    item.StandardEps = standardEps;
                    break;
                }
                return PartialView("_EPsAsign", epGroups);
            }
            return PartialView("_EPsAsign", epGroups);
        }

        public ActionResult AssignEPToGroup(int epDetailId, int epGroupId, bool isActive)
        {
            var ep_count = 0;
            int result = _epGroupService.AssignEPsGroup(epDetailId, epGroupId, isActive);
            var epGroups = _epGroupService.GetEPGroups(epGroupId);
            var count = epGroups.Where(x => x.EPGroupId == epGroupId).Select(x => x.StandardEps).ToList();
            foreach (var epCount in count)
            {
                ep_count = epCount.Count;
            }
            var response = new
            {
                ep_count
            };
            return Json(response);
        }

        #endregion

    }
}