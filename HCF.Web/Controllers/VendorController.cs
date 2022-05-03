using HCF.BAL;
using HCF.BDO;
using HCF.Web.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using HCF.Web.Filters;
using Newtonsoft.Json;
using HCF.BAL.Interfaces.Services;
using HCF.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using HCF.Utility;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class VendorController : BaseController
    {

        private readonly IUserSiteService _userSiteService;
        private readonly IVendorService _vendorService;
        private readonly ISiteService _siteService;
        private readonly IFloorTypeService _floorTypeService;
        private readonly IEmailService _emailService;
        private readonly IEpGroupsService _epGroupService;
        private readonly IAssetsService _assetService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;
        private readonly IEpService _epService;
        private readonly ICommonModelFactory _commonModelFactory;

        public VendorController(IEpService epService,
            ICommonModelFactory commonModelFactory,
            IOrganizationService organizationService,
            IUserSiteService userSiteService,
            IVendorService vendorService,
            ISiteService siteService,
            IFloorTypeService floorTypeService,
            IEmailService emailService,
            IEpGroupsService epGroupService,
            IAssetsService assetService,
            IUserService userService,
            ILoggingService loggingService)
        {
            _commonModelFactory = commonModelFactory;
            _epService = epService;
            _organizationService = organizationService;
            _userSiteService = userSiteService;
            _vendorService = vendorService;
            _siteService = siteService;
            _floorTypeService = floorTypeService;
            _emailService = emailService;
            _epGroupService = epGroupService;
            _assetService = assetService;
            _userService = userService;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Vendors()
        {
            UISession.AddCurrentPage("Vendor_Vendors", 0, "Vendors");
            var vendors = _vendorService.GetVendorsUser();
            return View(vendors);
        }

        [HttpGet]
        public ActionResult Addvendors(int? vendorId)
        {
            if (!UserSession.CurrentUser.IsPowerUser)
                return RedirectToAction("Vendors");

            UISession.AddCurrentPage("Vendor_Addvendors", 0, "Add vendor");
            var vendor = new Vendors();
            if (vendorId.HasValue)
            {
                ViewBag.PageTitle = "Update Vendor";
                ViewBag.SubmitValue = "Update";
                vendor = _vendorService.GetVendors().FirstOrDefault(x => x.VendorId == vendorId.Value);
                if (vendor != null)
                {
                    vendor.VendorSites = _userSiteService.GetUserSiteMappings().Where(x => x.IsActive && x.VendorId == vendor.VendorId).ToList();
                }

            }
            else
            {
                ViewBag.PageTitle = "Add Vendor";
                ViewBag.SubmitValue = "Add";
                //vendor.VendorCertificates = new List<Certificates>();
                //var cert = new Certificates();
                //vendor.VendorCertificates.Add(cert);
            }

            vendor.DrawingTypes = _floorTypeService.GetFloorTypes().Select(x => new SelectListItem { Text = x.FloorType, Value = x.FloorTypeId.ToString() }).ToList();

            return View(vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addvendors(Vendors vendor, string submitType, string masterVendorId, string invitationEmail, string siteAssigneeType)
        {
            //var errors = ModelState.Values.SelectMany(v => v.Errors);  //To check errors.
            if (ModelState.IsValid)
            {
                string siteAssignees = string.Empty;
                if (!string.IsNullOrEmpty(siteAssigneeType))
                {
                    var siteMappings = _commonModelFactory.JsonDeserialize<List<UserSiteMapping>>(siteAssigneeType);
                    siteAssignees = string.Join(",", siteMappings.Where(x => x.AssignedType != 0).Select(x => string.Format("{0}%{1}", x.SiteId, x.AssignedType)));
                }




                Guid? invitationId = Guid.NewGuid();
                bool sendInvitationEmail = false;
                if (string.IsNullOrEmpty(masterVendorId))
                    masterVendorId = "0";

                bool isMasterVendor = true;
                if (!string.IsNullOrEmpty(masterVendorId) && Convert.ToInt32(masterVendorId) > 0)
                    isMasterVendor = false;


                if (!string.IsNullOrEmpty(vendor.PhoneNo))
                    vendor.PhoneNo = new string(vendor.PhoneNo.Where(char.IsDigit).ToArray());

                vendor.CreatedBy = UserSession.CurrentUser.UserId;





                if (submitType == "Add")
                {
                    if (!string.IsNullOrEmpty(invitationEmail))
                        sendInvitationEmail = Convert.ToBoolean(invitationEmail);


                    if (!sendInvitationEmail)
                        invitationId = null;


                    vendor.VendorId = _vendorService.AddVendors(vendor, isMasterVendor, Convert.ToInt32(masterVendorId), invitationId, siteAssignees);
                    if (vendor.VendorId > 0)
                    {
                        if (sendInvitationEmail && invitationId.HasValue)
                            _emailService.VendorNotify(invitationId, vendor, UserSession.CurrentOrg);

                        SuccessMessage = Utility.ConstMessage.Vendor_Successfully;
                    }
                    else
                    {
                        ErrorMessage = "Email already Exist !";
                    }
                }
                else if (submitType == "Update")
                {
                    _vendorService.UpdateVendor(vendor, siteAssignees);
                    SuccessMessage = Utility.ConstMessage.Update_vendor_Success;
                }
                return RedirectToAction("Vendors"); //Parveen Kumar

            }
            else
            {
                ViewBag.SubmitValue = submitType;
            }
            return View(vendor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        public ActionResult VendorUsers(int vendorId)
        {
            List<UserProfile> users = _userService.GetUsers().Where(x => x.VendorId == vendorId).ToList();
            return PartialView("_vendorUser", users);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        //public ActionResult VendorCertificates(int vendorId)
        //{
        //    UISession.AddCurrentPage("Vendor_VendorCertificates", 0, "Vendor Certificates");
        //    var certificates = new List<Certificates>();
        //    var firstOrDefault = VendorRepository.GetVendors().FirstOrDefault(x => x.VendorId == vendorId);
        //    if (firstOrDefault != null)
        //    {
        //        certificates = firstOrDefault.VendorCertificates;

        //    }
        //    return View("Certificates", certificates);
        //}

        //[Obsolete]
        //public JsonResult SaveBuildingIds(string vendorId, string buildingsId)
        //{
        //    VendorRepository.saveBuildingIds(vendorId, buildingsId);
        //    return Json("");
        //}

        //public JsonResult GetVendorsBuildings(int vendorId)
        //{
        //    var data = VendorRepository.GetVendorsBuildings(vendorId);
        //    return Json(data);
        //}

        #region VendorConfiguratons

        [CrxAuthorize(Roles = "vendor_vendorconfiguratons")]
        public ActionResult VendorConfigurations()
        {
            UISession.AddCurrentPage("Vendor_VendorConfigurations", 0, "Vendor Configuration");

            var assets = _assetService.GetAssets(UserSession.CurrentUser.UserId).OrderBy(x => x.Name).ToList();
            var vendors = _vendorService.GetVendors().Where(x => x.IsActive).ToList();
            var Campusite = _siteService.GetSites().Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
            ViewBag.AssetsSelectList = new SelectList(assets, "AssetId", "Name", "select");
            ViewBag.VendorsSelectList = new SelectList(vendors, "VendorId", "Name", "select");
            var GetCampus = new SelectList(Campusite, "Code", "Name", "select");
            ViewBag.GetCampus = JsonConvert.SerializeObject(GetCampus);
            ViewBag.Assets = assets;
            ViewBag.Vendors = vendors;
            return View(assets);
        }

        [HttpGet]
        public IActionResult GetData(string mode, int id)
        {
            if (mode == "Vendor")
            {
                ViewBag.mappingAssets = _assetService.GetAssetsMapping(mode, Convert.ToString(id));
                var assets = _assetService.GetAssets(4);
                return PartialView("_AssetVendorData", assets);
            }
            if (mode == "Assets")
            {
                ViewBag.mappingAssets = _assetService.GetAssetsMapping("Asset", Convert.ToString(id));
                var vendors = _vendorService.GetVendors();
                return PartialView("_VendorAssetData", vendors);
            }
            if (mode == "epgroups")
            {
                ViewBag.VendorsEPgroups = _epGroupService.GetVendorEPGroups(id);
                var epgroups = _epGroupService.GetEPGroups(null).Where(x => x.IsActive).ToList();
                return PartialView("_VendorEPgroups", epgroups);
            }

            return new EmptyResult();
        }

        public JsonResult UpdateVendorAssets(int assetId, int vendorId, bool isActive)
        {
            var newAssetsMapping = new AssetsMapping
            {
                AssetId = assetId,
                VendorId = vendorId,
                IsActive = isActive,
                CreatedBy = UserSession.CurrentUser.UserId
            };
            _assetService.AddAssetsMapping(newAssetsMapping);
            return Json("");
        }

        public JsonResult UpdateVendorEPGroup(int groupId, int vendorId, string isActive)
        {
            var result = _epGroupService.SaveVendorEPGroups(vendorId, groupId, Convert.ToBoolean(isActive));
            var users = _userService.GetUsers().Where(x => x.VendorId == vendorId).ToList();
            var epgroups = _epGroupService.GetEPGroups(groupId);
            var detailid = epgroups.Where(x => x.EPGroupId == groupId).Select(x => x.StandardEps).FirstOrDefault().ToList();
            foreach (var user in users)
            {
                foreach (var epdetailid in detailid)
                {
                    _epService.SaveUserEPs("one", user.UserId, Convert.ToBoolean(isActive), Convert.ToInt32(epdetailid.EPDetailId));
                }
            }
            return Json(result);
        }

        #endregion

        #region contact Details

        [CrxAuthorize(Roles = "vendor_UpdateContactDetails")]
        public ActionResult UpdateContactDetails(int? vendorId)
        {
            UISession.AddCurrentPage("Vendor_UpdateContactDetails", 0, "Update Contact Details");
            ViewBag.VendorId = 0;
            if (vendorId.HasValue) {
                ViewBag.VendorId = vendorId.Value;
                ViewBag.Vendors = _vendorService.GetVendors().Where(x => x.VendorId == vendorId).Select(x => new SelectListItem { Text = x.Name, Value = x.VendorId.ToString() }).ToList();
            }

            ViewBag.Vendors = _vendorService.GetVendors().OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.VendorId.ToString() }).ToList();

            return View();
        }

        [HttpPost]
        public JsonResult UpdateContactDetails(int? vendorId, string htmltext, string messageToContractor, int Status)
        {
            var vid = vendorId ?? 0;
            var success = _vendorService.UpdateContactDetails(htmltext, vid, messageToContractor, Status);
            return Json(success);
        }

        public JsonResult GetContactDetails(int? vendorId)
        {
            //int vid = vendorId.HasValue ? vendorId.Value : 0;
            var contactDetail = "";
            var messageToContractor = "";
            var Status = "";
            if (vendorId.HasValue && vendorId.Value > 0)
            {
                var item = _vendorService.GetVendorDetails().FirstOrDefault(x => x.VendorId == vendorId);
                if (item != null)
                {
                    contactDetail = item.ContactDetails;
                    messageToContractor = item.MessageToContractor;
                    Status = Convert.ToString(item.IsActive);

                }
            }
            else
            {
                var item = _organizationService.GetOrganization();
                contactDetail = item.ContactDetails;
                messageToContractor = item.MessageToContractor;
                Status = Convert.ToString(item.IsActive);

            }
            var obj = new List<string>
            {
                contactDetail,
                messageToContractor,
                Status
            };

            return Json(obj);
        }

        #endregion

        #region VendorResources
        public ActionResult VendorResources(int? vendorId)
        {
            UISession.AddCurrentPage("Vendor_VendorResources", 0, "Vendor Resources");
            var vendorResources = _vendorService.GetVendorResource(vendorId);
            return View(vendorResources);
        }


        [HttpGet]
        public ActionResult AddVendorResources(int? vendorId, int? vendorResId)
        {
            UISession.AddCurrentPage("Vendor_VendorResources", 0, (vendorId > 0) ? "Update Vendor Resource" : "Add Vendor Resource");
            var vendorResource = new VendorResource();
            if (vendorResId.HasValue)
            {
                ViewBag.PageTitle = "Update Vendor Resource";
                ViewBag.SubmitValue = "Update";
                vendorResource = _vendorService.GetVendorResource(vendorId).FirstOrDefault(x => x.VendorResId == vendorResId.Value); //_vendorService.GetVendors().FirstOrDefault(x => x.VendorId == vendorId.Value);
                if (vendorResource.File.Any())
                {
                    vendorResource.TFileId = string.Join(",", vendorResource.File.Select(x => x.TFileId));
                }
            }
            else
            {
                ViewBag.PageTitle = "Add Vendor Resource";
                ViewBag.SubmitValue = "Add";
            }

            ViewBag.Vendors = _vendorService.GetVendors().Select(x => new SelectListItem { Text = x.Name, Value = x.VendorId.ToString() }).ToList();


            return View(vendorResource);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVendorResources(VendorResource vmodel, string submitType)
        {
            vmodel.CreatedBy = UserSession.CurrentUser.UserId;
            if (submitType == "Add")
            {
                //vmodel.UploadLink = "//" + vmodel.UploadLink;
                _vendorService.AddVendorResources(vmodel);
                SuccessMessage = ConstMessage.Saved_Successfully;
            }
            else if (submitType == "Update")
            {
                _vendorService.AddVendorResources(vmodel);
                SuccessMessage = ConstMessage.Success_Updated;  
            }
            return RedirectToAction("VendorResources");
        }

        #endregion

        #region UserVendor
        public ActionResult SendOrganizationIdModal(int? vendorId)
        {
            ViewBag.VendorId = vendorId;
            var userprofiles = _userService.GetUsers().ToList();
            return PartialView("_SendOrganizationId", userprofiles);
        }

        [HttpPost]
        public JsonResult SendOrganizationId(string ReceiverEmail, string Subject, string Description, string invitationId, int vendorId)
        {
            var org = new VendorOrganizations
            {
                InvitationId = Guid.Parse(invitationId),
                OrgKey = UserSession.CurrentOrg.Orgkey,
                RequestedBy = UserSession.CurrentUser.UserGuid,
                VendorId = Convert.ToInt32(vendorId)
            };
            var id = _vendorService.SaveVendorInvitation(org);
            if (id > 0)
            {
                var result = _emailService.SendEmail(ReceiverEmail, Subject, Description);
                return Json(result);
            }

            return Json("");
        }
        public ActionResult Vendordashboard(int? vendorId)
        {
            UISession.AddCurrentPage("VendorDashboard", 0, "Vendor Dashboard");
            var vendorResources = _vendorService.GetVendors().Where(x => !string.IsNullOrEmpty(x.MessageToContractor) || !string.IsNullOrEmpty(x.ContactDetails));
            return View(vendorResources);
        }


        #endregion

        #region vendor org mapping


        [HttpPost]
        public JsonResult GetVendorByPrefix(string Prefix)
        {
            var floorAssets = _vendorService.GetVendorByPrefix(Prefix);
            return Json(floorAssets);
        }

        #endregion
    }
}

// 389 july 1 ,2021