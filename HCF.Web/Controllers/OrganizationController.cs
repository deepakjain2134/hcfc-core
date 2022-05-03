using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HCF.BAL;
using HCF.Web.Base;
using HCF.BDO;
using HCF.Web.Filters;
using HCF.Web.ViewModels.Organization;
using HCF.BAL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using HCF.Utility;
using HCF.Utility.Enum;
using HCF.Web.ViewModels;
using Org.BouncyCastle.Asn1.Cmp;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class OrganizationController : BaseController
    {
        #region Ctor

        private readonly ISiteService _siteService;
        private readonly IUserEpService _userEpService;
        private readonly IActivityService _activityService;
        private readonly IBuildingsService _buildingsService;
        private readonly IFloorService _floorService;
        private readonly IOrganizationService _organizationService;
        private readonly IShiftService _shiftService;
        private readonly IFloorAssetService _floorAssetService;
        private readonly IWingService _wingService;
        private readonly ILocationService _locationService;
        private readonly IEpService _EpService;
        private readonly IFloorTypeService _floorTypeService;
        private readonly IAssetsService _assetsService;
        private readonly IFrequencyService _frequencyService;
        private readonly IUserService _userService;
        private readonly IHttpPostFactory _httpService;
        private readonly IGoalMasterService _goalMasterService;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly ICacheService _cacheService;

        public OrganizationController(
            ICommonModelFactory commonModelFactory,
            IAssetsService assetsService,
            IShiftService shiftService,
            IFloorAssetService floorAssetService,
            IWingService wingService,
            ILocationService locationService,
            IEpService epService,
            IFloorTypeService floorTypeService,
            ISiteService siteService,
            IUserEpService userEpService,
            IActivityService activityService,
            IBuildingsService buildingsService,
            IFloorService floorService,
            IOrganizationService organizationService,
            IFrequencyService frequencyService,
            IUserService userService,
            IHttpPostFactory httpService,
            IGoalMasterService goalMasterService,
            ICacheService cacheService
            )
        {
            _commonModelFactory = commonModelFactory;
            _assetsService = assetsService;
            _floorTypeService = floorTypeService;
            _EpService = epService;
            _siteService = siteService;
            _userEpService = userEpService;
            _activityService = activityService;
            _floorService = floorService;
            _buildingsService = buildingsService;
            _organizationService = organizationService;
            _shiftService = shiftService;
            _wingService = wingService;
            _floorAssetService = floorAssetService;
            _locationService = locationService;
            _frequencyService = frequencyService;
            _userService = userService;
            _httpService = httpService;
            _goalMasterService = goalMasterService;
            _cacheService = cacheService;
        }

        #endregion

        #region Common

        public ActionResult FillFloors(int buildingId)
        {
            var floors = _floorService.GetBuildingFloors(buildingId).Where(x => x.IsActive).ToList();
            return Json(floors);
        }

        public ActionResult FillBuildingFloors(string buildingIds)
        {
            var floors = _floorService.GetBuildingFloors(buildingIds);
            return Json(floors);
        }


        public JsonResult GetFloorByBuilding(int buildingId)
        {
            var floors = _floorService.GetFloors().Where(x => x.BuildingId == buildingId && x.IsActive).ToList();
            return Json(floors);
        }

        public JsonResult FillStops(int floorId)
        {
            var stops = _locationService.GetLocationsMaster(null).Where(x => x.FloorId == floorId && x.IsActive).ToList();
            return Json(stops);
        }


        public JsonResult GetEPAssets(int typeId)
        {
            List<Assets> assets = _assetsService.GetAssetTypes(UserSession.CurrentUser.UserId).
                Where(y => y.Assets.Any(x => x.Count > 0) && y.TypeId == typeId).SelectMany(x => x.Assets).ToList();
            return Json(assets.Where(x => x.StandardEps.Count > 0).ToList());
        }

        public JsonResult GetAssetsBuilding(int assetId)
        {
            var lists = _floorAssetService.GetAssetsBuilding(assetId);
            return Json(lists);
        }

        public JsonResult GetAssetsFloor(int assetId, int buildingId)
        {
            var lists = _floorAssetService.GetAssetsBuilding(assetId).Where(x => x.BuildingId == buildingId).ToList();
            return Json(lists);
        }

        public JsonResult GetStopNotExistInFE(int AssetId, int FloorId)
        {
            var stopsNotIdFE = _locationService.GetStopNotExistInFe(AssetId, FloorId).ToList();
            return Json(stopsNotIdFE);
        }

        public JsonResult GetBuildingBySite(string sitecode)
        {
            List<Buildings> buildings = new();
            if (!string.IsNullOrEmpty(sitecode))
                buildings = _buildingsService.GetBuildings().Where(x => x.SiteCode.ToLower() == sitecode.ToLower() && x.IsActive).ToList();
            else
                buildings = _buildingsService.GetBuildings().Where(x => x.IsActive).ToList();
            return Json(buildings);
        }
        public JsonResult GetBuildingByMultipleSite(string sitecode)
        {
            var sites = _buildingsService.GetAllBuildingsbySite(sitecode);
            return Json(sites);
        }


        #endregion

        #region Site


        public ActionResult Site()
        {
            UISession.AddCurrentPage("Organization_Site", 0, "Campus");
            var sites = _siteService.GetSites();
            return View(sites);
        }


        [HttpGet]
        public ActionResult Addsite(int? siteId)
        {
            UISession.AddCurrentPage("Organization_addbuilding", 0, "Add Site");
            Site newSite = new Site();

            if (siteId == null)
            {
                newSite.CreatedBy = UserSession.CurrentUser.UserId;
                newSite.IsActive = true;
                return View(newSite);
            }

            newSite = _siteService.GetSites().FirstOrDefault(x => x.SiteId == (siteId.HasValue ? siteId.Value : 0));
            return View(newSite);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addsite(Site newSite)
        {
            if (ModelState.IsValid)
            {
                newSite.SiteId = _siteService.Save(newSite);
                if (newSite.SiteId > 0)
                    SuccessMessage = ConstMessage.Saved_Successfully;
                else
                {
                    if (newSite.SiteId == -1)
                    {
                        ErrorMessage = "Code already exist";
                    }
                }
                return RedirectToAction("Site");
            }
            return View(newSite);
        }

        public ActionResult SiteList(int siteId = 0, String site = "")
        {
            var data = _siteService.GetSites();
            ViewBag.SiteName = site;
            return PartialView("_site", data);
        }

        #endregion

        #region Buildings

        [CrxAuthorize(Roles = "organization_buildings")]
        public ActionResult Buildings()
        {
            UISession.AddCurrentPage("Organization_Buildings", 0, "Buildings");
            var buildings = _buildingsService.GetBuildings();
            return View(buildings);
        }

        [HttpGet]
        public ActionResult Addbuilding(int? buildingId)
        {

            Buildings newBuilding = new Buildings();
            if (buildingId == null)
                return View(newBuilding);

            newBuilding = _buildingsService.GetBuildings().FirstOrDefault(x => x.BuildingId == Convert.ToInt32(buildingId));
            UISession.AddCurrentPage("Organization_addbuilding", 0, "Add Building");
            return View(newBuilding);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addbuilding(Buildings newBuilding)
        {
            if (ModelState.IsValid)
            {
                newBuilding.CreatedBy = UserSession.CurrentUser.UserId;
                newBuilding.BuildingId = newBuilding.BuildingId > 0 ? _buildingsService.UpdateBuilding(newBuilding) : _buildingsService.Save(newBuilding);

                if (newBuilding.BuildingId > 0)
                    SuccessMessage = ConstMessage.Saved_Successfully;
                else
                    ErrorMessage = "Building Code Already Exists!";
                return RedirectToAction("Buildings");
            }
            return View(newBuilding);
        }

        [HttpGet]
        public ActionResult EditBuilding(int? buildingId)
        {
            if (buildingId == null)
                return RedirectToAction("Buildings");
            UISession.AddCurrentPage("Organization_editBuilding", 0, "Update Building");
            Buildings newBuilding = _buildingsService.GetBuildings().FirstOrDefault(x => x.BuildingId == Convert.ToInt32(buildingId));
            return View(newBuilding);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBuilding(Buildings newBuilding)
        {
            var newId = 0;
            if (ModelState.IsValid)
            {
                newBuilding.CreatedBy = UserSession.CurrentUser.UserId;
                newId = _buildingsService.UpdateBuilding(newBuilding);
                if (newId > 0) { SuccessMessage = "building code updated successfully"; }
                else
                {
                    ErrorMessage = "Building Code Already Exists!";
                }
            }
            else
            {
                ErrorMessage = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).FirstOrDefault();
            }
            return newId > 0 ? RedirectToAction("Buildings") : RedirectToAction("EditBuilding", new { buildingId = newBuilding.BuildingId });
        }

        #endregion

        #region Wing

        public ActionResult Wings()
        {
            UISession.AddCurrentPage("site_Wings", 0, "Wings");
            var wings = _wingService.GetWings();
            return View(wings);
        }

        [HttpGet]
        public ActionResult addwings(int? wingid)
        {
            var newWing = new Wing();
            if (wingid.HasValue && wingid.Value > 0)
            {
                newWing = _wingService.GetWings().FirstOrDefault(x => x.WingId == wingid);
            }
            return View(newWing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addwings(Wing newWing)
        {
            if (!ModelState.IsValid) return View(newWing);
            newWing.CreatedBy = UserSession.CurrentUser.UserId;
            if (newWing.WingId > 0)
            {
                _wingService.UpdateWing(newWing);
                SuccessMessage = Utility.ConstMessage.Success_Updated;
            }
            else
            {
                _wingService.AddWing(newWing);
                SuccessMessage = Utility.ConstMessage.Success;

            }
            return RedirectToAction("Wings");
        }
        #endregion

        #region Floor

        [CrxAuthorize(Roles = "organization_floor")]
        public ActionResult Floor()
        {
            UISession.AddCurrentPage("Organization_Floor", 0, "Floor Plans");
            var floors = _floorService.GetFloors();
            return View(floors);
        }

        [HttpGet]
        public ActionResult Addfloor(int? floorTypeId)
        {
            UISession.AddCurrentPage("Organization_addfloor", 0, "Add Floor Plan");
            Floor floor = new Floor
            {
                IsActive = true,
                EffectiveDate = DateTime.UtcNow.ToClientTime()
            };

            var floorTypes = _floorTypeService.GetFloorTypes();
            floor.FloorPlanTypes = floorTypes.OrderBy(x => x.FloorTypeId).ToList();

            return View(floor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFloor(Floor newFloor, int floorTypeId, string redirectPage = "")
        {
            HttpResponseMessage httpresponse = new();
            if (ModelState.IsValid)
            {
                var isFloorCodeExist = _floorService.GetFloors().Any(x => x.FloorCode == newFloor.FloorCode && x.BuildingId == newFloor.BuildingId);
                if (!isFloorCodeExist)
                {
                    if (newFloor.EffectiveDate.HasValue)
                    {
                        newFloor.EffectiveDate = newFloor.EffectiveDate.Value.ToUniversalTime();
                        newFloor.EffectiveDateTimeSpan = Conversion.ConvertToTimeSpan(newFloor.EffectiveDate);
                    }
                    newFloor.CreatedBy = UserSession.CurrentUser.UserId;
                    string postData = _commonModelFactory.JsonSerialize<Floor>(newFloor);

                    int StatusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                    string result = _httpService.CallPostMethod(postData, APIUrls.Organization_EditFloor, ref StatusCode);
                    if (StatusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    {
                        httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                        if (httpresponse.Success)
                            SuccessMessage = httpresponse.Message;
                        else
                            ErrorMessage = httpresponse.Message;
                    }
                    else
                        ErrorMessage = ConstMessage.Internal_Server_Error;
                    SuccessMessage = ConstMessage.Saved_Successfully;
                    if (floorTypeId > 0)
                    {
                        if (!string.IsNullOrEmpty(redirectPage))
                        {
                            var returnUrlPart = redirectPage.Split('/').ToList();
                            return RedirectToAction(returnUrlPart[2], returnUrlPart[1], new { floorTypeId = returnUrlPart[3] });
                        }
                        return RedirectToRoute("floors");
                    }
                    else
                    {
                        return RedirectToRoute("floors");
                    }
                }
            }
            else
            {
                ViewBag.FloorType = _floorTypeService.GetFloorTypes();
            }
            ErrorMessage = newFloor.FloorCode + " Floor code is already exist ";
            return RedirectToRoute("addfloor");
        }

        [HttpGet]
        public ActionResult EditFloor(int? fid)
        {
            UISession.AddCurrentPage("Organization_editFloor", 0, "Update Floor Plan");
            var newFloor = new Floor();
            if (fid != null)
            {
                newFloor = _floorService.GetFloors().FirstOrDefault(x => x.FloorId == fid);
                if (newFloor?.EffectiveDate != null)
                    newFloor.EffectiveDate = newFloor.EffectiveDate.Value.ToLocalTime();
            }
            var floorTypes = _floorTypeService.GetFloorTypes();
            if (newFloor != null)
            {
                newFloor.FloorPlanTypes = floorTypes.OrderBy(x => x.FloorTypeId).ToList();
                foreach (var item in newFloor.FloorPlanTypes)
                {
                    var plans = newFloor.FloorPlans.Where((x => x.FloorPlanTypeId == item.FloorTypeId)).ToList();
                    if (plans.Count > 0)
                    {
                        item.FloorPlans = plans;
                    }

                }

            }
            return View("addfloor", newFloor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFloor(Floor updateFloor)
        {
            var isFloorCodeUpdated = updateFloor.OldFloorCode != updateFloor.FloorCode;

            var isFloorCodeExist = _floorService.GetFloors().Any(x => x.FloorCode == updateFloor.FloorCode) && isFloorCodeUpdated;
            if (!isFloorCodeExist)
            {
                if (ModelState.IsValid)
                {
                    if (updateFloor.EffectiveDate.HasValue)
                    {
                        updateFloor.EffectiveDate = updateFloor.EffectiveDate.Value.ToUniversalTime();
                        updateFloor.EffectiveDateTimeSpan = Utility.Conversion.ConvertToTimeSpan(updateFloor.EffectiveDate);
                    }
                    updateFloor.CreatedBy = UserSession.CurrentUser.UserId;
                    var postData = _commonModelFactory.JsonSerialize<Floor>(updateFloor);
                    var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                    _httpService.CallPostMethod(postData, Utility.APIUrls.Organization_EditFloor, ref statusCode);
                    SuccessMessage = Utility.ConstMessage.Success_Updated;
                    return RedirectToRoute("floors");
                }
            }
            else
            {
                ErrorMessage = Utility.ConstMessage.FloorCode_Error;
                ViewBag.FloorType = _floorTypeService.GetFloorTypes();
            }

            return View("addfloor", updateFloor);
        }

        public JsonResult DeleteFloorPlan(string FloorPlanId)
        {
            var status = _floorService.DeleteFloorPlan(Guid.Parse(FloorPlanId));
            return Json(status);
        }


        #endregion

        #region More Configuration

        [CrxAuthorize(Roles = "organization_general")]
        public ActionResult General()
        {
            UISession.AddCurrentPage("Organization_General", 0, "General");
            var org = _organizationService.GetOrganization();
            var emails = _organizationService.Emails().Where(x => x.ClientNo == UserSession.CurrentOrg.ClientNo).ToList();
            org.PopEmails = new List<PopEmails>();
            org.PopEmails = emails;
            return View(org);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult General(IFormFile file, IFormFile logofile, Organization org)
        {
            if (file != null)
            {
                org.FilesContent = _commonModelFactory.ConvertToBase64(file);
                org.FileName = file.FileName;
            }
            if (logofile != null)
            {
                org.LogoBase64 = _commonModelFactory.ConvertToBase64(logofile);
                org.LogoName = logofile.FileName;
            }
            var postData = _commonModelFactory.JsonSerialize<Organization>(org);
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = _httpService.CallPostMethod(postData, APIUrls.Organization_General, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                if (httpresponse.Success) { SuccessMessage = ConstMessage.Update_Organization_Success; } else { ErrorMessage = ConstMessage.Error; }
            }
            return RedirectToAction("General");
        }

        public ActionResult UserDom(string UserDomain)
        {

            var org = _organizationService.GetOrganization();

            return PartialView("UserDom", org);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserDom(Organization orgs)
        {
            string Mode = "All";
            var org = _organizationService.GetOrganization();
            org.UserDomain = orgs.UserDomain;
            var status = _organizationService.UpdateOrganization(org, Mode);
            SuccessMessage = ConstMessage.Update_Organization_Success;
            return RedirectToAction("OrgSettings");
        }


        #endregion

        #region Inbox Configuration

        public ActionResult InboxSetup()
        {
            UISession.AddCurrentPage("CRxSetting_InboxSetup", 0, "Inbox Setup");
            var emails = _organizationService.Emails().Where(x => x.ClientNo == UserSession.CurrentOrg.ClientNo).ToList();
            return View(emails);
        }

        public ActionResult InboxAdd(int? Id)
        {
            if (!Id.HasValue)
                Id = 0;

            PopEmails email = new PopEmails();
            if (Id > 0)
                email = _organizationService.Emails().FirstOrDefault(x => x.Id == Id);

            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InboxAdd(PopEmails emails)
        {
            emails.ClientNo = UserSession.CurrentOrg.ClientNo;
            return View(emails);
        }

        #endregion

        #region Frequency 

        public ActionResult Frequencies()
        {
            var frequencies = _frequencyService.GetFrequency();
            return View(frequencies);
        }

        public ActionResult MngFrequencies(int? id)
        {
            FrequencyMaster frequency = _frequencyService.GetFrequency(id.Value);
            if (frequency == null)
                frequency = new FrequencyMaster();
            return View(frequency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MngFrequencies(FrequencyMaster frequencyMaster)
        {
            if (ModelState.IsValid)
            {
                frequencyMaster.CreatedBy = UserSession.CurrentUser.UserId;
                if (frequencyMaster.FrequencyId > 0)
                    _frequencyService.Update(frequencyMaster);
                else
                    _frequencyService.Save(frequencyMaster);

                return RedirectToAction("Frequencies");
            }
            return View(frequencyMaster);
        }

        #endregion        

        #region Shift

        public ActionResult Shift()
        {
            UISession.AddCurrentPage("Organization_Shift", 0, "Shift");
            var shift = _shiftService.GetShift();
            return View(shift);
        }

        [HttpGet]
        public ActionResult Addshift(int? shiftId)
        {
            UISession.AddCurrentPage("Organization_addShift", 0, "Add Shift");
            var newShift = new Shift();

            if (shiftId.HasValue)
                newShift = _shiftService.GetShift().FirstOrDefault(x => x.ShiftId == shiftId.Value);

            return View(newShift);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addshift(Shift newShift)
        {
            if (!string.IsNullOrEmpty(newShift.StartTimeString) && !string.IsNullOrEmpty(newShift.EndTimeString))
            {
                newShift.CreatedBy = UserSession.CurrentUser.UserId;
                newShift.StartTime = DateTime.ParseExact(newShift.StartTimeString, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                newShift.EndTime = DateTime.ParseExact(newShift.EndTimeString, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                newShift.ShiftId = _shiftService.Save(newShift);
                if (newShift.ShiftId > 0)
                    SuccessMessage = Utility.ConstMessage.Success_Updated;

                return RedirectToAction("Shift");
            }
            return View(newShift);
        }

        #endregion

        #region Ep Assignment

        public ActionResult EpAssignments(int type)
        {
            UISession.AddCurrentPage("Organization_EpAssignments", 0, "EP Assignments");
            var users = new List<UserProfile>();
            var eps = new List<EPDetails>();
            BindEPUserAndAssignee(ref users, ref eps);
            var model = new EpAssignmentsViewModel
            {
                UserList = users,
                EpDetails = eps
            };
            return View(model);
        }

        public ActionResult EpAssignmentList(int? catId, int[] seluserIds)
        {
            var users = new List<UserProfile>();
            var eps = new List<EPDetails>();
            BindEPUserAndAssignee(ref users, ref eps);
            var model = new EpAssignmentsViewModel
            {
                UserList = users,
                EpDetails = eps,
                CategoryId = catId.HasValue ? catId.Value : 0,
                UserIds = seluserIds
            };
            return PartialView("_EpAssignment", model);
        }

        public ActionResult EpFrequencyScheduleDatePopUp(string userIds)
        {
            int[] userIdList = userIds.Split(',').Select(int.Parse).ToArray();
            UserEpDetailLists userEPs = new UserEpDetailLists();
            userEPs.EpDetails = _EpService.OngoingEpDetails().Where(m => m.IsActive = true).ToList();
            userEPs.UserIds = userIdList;
            int[] array = userIds.Split(',').Select(Int32.Parse).ToArray();
            userEPs.SelectedEps = _EpService.GetUsersEps(array).ToArray();
            return PartialView("EpAssignementTreePopup", userEPs);
        }

        private void BindEPUserAndAssignee(ref List<UserProfile> users, ref List<EPDetails> eps)
        {
            eps = _cacheService.Get<List<EPDetails>>(CacheDefaults.EPAssignmentEPs.ToString(), UserSession.CurrentOrg.Orgkey.ToString());
            if (eps == null)
            {
                eps = _EpService.GetEpDetailsAsync();
                _cacheService.Set(CacheDefaults.EPAssignmentEPs.ToString(), eps, CacheTimes.OneDay, UserSession.CurrentOrg.Orgkey.ToString());
            }
            users = _cacheService.Get<List<UserProfile>>(CacheDefaults.EPAssignmentUsers.ToString(), UserSession.CurrentOrg.Orgkey.ToString());
            if (users == null)
            {
                users = _userService.GetUserList().Where(x => !x.IsCRxUser && x.IsActive && !string.IsNullOrEmpty(x.Email)).OrderBy(x => x.FullName).ToList();
                _cacheService.Set(CacheDefaults.EPAssignmentUsers.ToString(), users, CacheTimes.OneDay, UserSession.CurrentOrg.Orgkey.ToString());
            }
            List<EPAssignee> epAssignee = _userEpService.Get();
            if (epAssignee.Count > 0)
            {
                var epusers = epAssignee.GroupBy(x => x.EPDetailId).Select(group => new EPDetails { EPDetailId = group.Key, EPUsers = group.Select(x => x.UserProfile).ToList() }).ToList();
                foreach (var item in eps)
                {
                    var epuser = epusers.FirstOrDefault(x => x.EPDetailId == item.EPDetailId);
                    if (epuser != null)
                        item.EPUsers = epuser.EPUsers;
                    else
                        item.EPUsers = new List<UserProfile>();
                }
                var UsersEPs = epAssignee.GroupBy(x => x.UserId).Select(group => new UserProfile { UserId = group.Key, EpsCount = group.Count() }).ToList();
                foreach (var item in users)
                {
                    var epuser = UsersEPs.FirstOrDefault(x => x.UserId == item.UserId);
                    if (epuser != null)
                        item.EpsCount = epuser.EpsCount;
                    else
                        item.EpsCount = 0;
                }
            }
            eps = eps.OrderBy(x => x.StandardEp).ToList();
            users = users.OrderBy(x => x.FullName).ToList();
        }

        [HttpGet]
        public ActionResult AssignEpResponsibility(string fromUsers, string toUsers)
        {
            var data = new { success = true };
            _goalMasterService.AssignEpResposibility(fromUsers, toUsers, UserSession.CurrentUser.UserId);

            var toUsersList = toUsers.Split(',').Select(Int32.Parse).ToArray();
            var fromUserList = fromUsers.Split(',').Select(Int32.Parse).ToArray();
            _activityService.EpCopyToUser(BDO.Enums.UserActivityType.EPTransferToUser, fromUserList, toUsersList, UserSession.CurrentUser.UserId);
            return Json(data);
        }

        public ActionResult EpUsers(int epdetailId)
        {
            var epUsers = _userEpService.GetEpUsers(epdetailId);
            var ep = new EPDetails
            {
                EPDetailId = epdetailId,
                EPUsers = new List<UserProfile>()
            };
            if (epUsers != null && epUsers.Any())
                ep.EPUsers = epUsers.ToList();

            return PartialView("_epuser", ep);
        }

        public JsonResult GetUserEPs(int userId)
        {
            var eps = _EpService.GetUserEPs(userId);
            return Json(eps);
        }

        public JsonResult GetUsersEPs(string userIds)
        {
            if (!string.IsNullOrEmpty(userIds))
            {
                int[] array = userIds.Split(',').Select(Int32.Parse).ToArray();
                var eps = _EpService.GetUsersEps(array);
                return Json(eps);
            }
            return Json("0");
        }

        [HttpGet]
        public ActionResult GetEpAssignee(int? epId, string userids)
        {
            var epAssignmentsViewModel = new EpAssignmentsViewModel();
            if (!string.IsNullOrEmpty(userids))
            {
                epAssignmentsViewModel.UserIds = userids.Split(',').Select(Int32.Parse).ToArray();
            }
            epAssignmentsViewModel.UserList = _userService.GetUserList().Where(x => !x.IsCRxUser && x.IsActive && !string.IsNullOrEmpty(x.Email)).ToList();
            return PartialView("_addEPAssignee", epAssignmentsViewModel);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult AddEPAssignees(List<EPAssignee> epAssignee, string catId, string mode, string userIds, int isRemoveAll)
        {
            if (epAssignee != null)
            {
                if (mode == "user")
                {
                    var groupByUser = epAssignee.GroupBy(x => x.UserId).Select(x => x.First());
                    foreach (var item in groupByUser)
                    {
                        var userArray = new List<int>();
                        int userId = item.UserId;
                        userArray.Add(userId);

                        var epsToSave = epAssignee.Where(x => x.UserId == userId).ToList();
                        int[] epArray = epsToSave.Select(x => x.EPDetailId).ToArray();

                        var getActivityRecords = _userEpService.GetUpdatedRecordsByUser(userId, epArray);
                        _userEpService.UpdateUserEps(userId, UserSession.CurrentUser.UserId); // mark user

                        SaveEPAssignee(epsToSave, getActivityRecords, userArray.ToArray(), isRemoveAll);
                    }
                }
                else if (mode == "ep")
                {
                    var groupByEp = epAssignee.GroupBy(x => x.EPDetailId).Select(x => x.First());
                    foreach (var item in groupByEp)
                    {
                        var epArray = new List<int>();
                        int ePDetailId = item.EPDetailId;
                        epArray.Add(ePDetailId);

                        var userToSave = epAssignee.Where(x => x.EPDetailId == ePDetailId).ToList();
                        int[] userArray = userToSave.Select(x => x.UserId).ToArray();
                        var getActivityRecords = _userEpService.GetUpdatedRecordsByEp(ePDetailId, userArray);
                        _userEpService.UpdateEpUser(ePDetailId, UserSession.CurrentUser.UserId);
                        if (isRemoveAll == 0)
                            SaveEPUsers(epArray, userToSave, getActivityRecords);
                    }
                }
                return RedirectToAction("EpAssignmentList", new { catId = Convert.ToInt32(catId), seluserIds = userIds.Split(',').Select(int.Parse).ToList() });
            }
            return Json(epAssignee);
        }

        private void SaveEPUsers(List<int> epArray, List<EPAssignee> userToSave, IDictionary<BDO.Enums.UserActivityType, int[]> getActivityRecords)
        {
            userToSave.ForEach(a =>
            {
                a.Status = true;
                a.CreatedBy = UserSession.CurrentUser.UserId;
                a.IsCurrent = true;
                a.CreatedDate = DateTime.UtcNow;
            });

            bool status = _userEpService.SaveEPUser(userToSave);
            if (status)
            {
                if (getActivityRecords.ContainsKey(BDO.Enums.UserActivityType.EPAssigned))
                {
                    var arrayToSave = getActivityRecords[BDO.Enums.UserActivityType.EPAssigned];
                    if (arrayToSave.Any())
                        _activityService.EpUserAssigned(BDO.Enums.UserActivityType.EPAssigned, arrayToSave, epArray.ToArray(), UserSession.CurrentUser.UserId);
                }

                if (getActivityRecords.ContainsKey(BDO.Enums.UserActivityType.EPUnAssigned))
                {
                    var arrayToSave = getActivityRecords[BDO.Enums.UserActivityType.EPUnAssigned];
                    if (arrayToSave.Any())
                        _activityService.EpUserAssigned(BDO.Enums.UserActivityType.EPUnAssigned, arrayToSave, epArray.ToArray(), UserSession.CurrentUser.UserId);
                }
            }
        }

        private void SaveEPAssignee(List<EPAssignee> userToSave, IDictionary<BDO.Enums.UserActivityType, int[]> getActivityRecords, int[] userArray, int isRemoveAll)
        {
            if (isRemoveAll == 0)
            {
                userToSave.ForEach(a =>
                {
                    a.Status = true;
                    a.CreatedBy = UserSession.CurrentUser.UserId;
                    a.IsCurrent = true;
                    a.CreatedDate = DateTime.UtcNow;
                });

                bool status = _userEpService.SaveEPUser(userToSave.Where(x => x.EPDetailId != -1).ToList());
                if (status)
                {
                    if (getActivityRecords.ContainsKey(BDO.Enums.UserActivityType.EPAssigned))
                    {
                        var epArrayToSave = getActivityRecords[BDO.Enums.UserActivityType.EPAssigned];
                        if (epArrayToSave.Any())
                            _activityService.EpUserAssigned(BDO.Enums.UserActivityType.EPAssigned, userArray, epArrayToSave, UserSession.CurrentUser.UserId);
                    }

                    if (getActivityRecords.ContainsKey(BDO.Enums.UserActivityType.EPUnAssigned))
                    {
                        var epArrayToSave = getActivityRecords[BDO.Enums.UserActivityType.EPUnAssigned];
                        if (epArrayToSave.Any())
                            _activityService.EpUserAssigned(BDO.Enums.UserActivityType.EPUnAssigned, userArray, epArrayToSave, UserSession.CurrentUser.UserId);
                    }
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEpAssignee(EPAssignee epAssignee)
        {
            if (epAssignee != null)
            {
                epAssignee.CreatedBy = UserSession.CurrentUser.UserId;
                _goalMasterService.SaveEpAssignee(epAssignee);
            }

            return Json(epAssignee);
        }

        #endregion

        #region Organization NonTrackEPReportDate

        [HttpPost]
        public ActionResult UpdateNonTrackEPReportDate(string annualReportDate)
        {
            var org = new Organization();
            org.NonTrackedEPReportDate = Convert.ToDateTime(annualReportDate);
            var result = _organizationService.UpdateOrganizationSettings(org);
            return Json(result);
        }
        #endregion

        #region Org Settings
        //[CrxAuthorize(Roles = "organization_OrgSettings")]
        public ActionResult OrgSettings()
        {
            UISession.AddCurrentPage("Organization_OrgSettings", 0, "OrgSettings");
            var org = _organizationService.GetOrganization();
            var emails = _organizationService.Emails().Where(x => x.ClientNo == UserSession.CurrentOrg.ClientNo).ToList();
            org.PopEmails = new List<PopEmails>();
            org.PopEmails = emails;
            return View(org);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrgSettings(Organization org, string mode)
        {
            var postData = _commonModelFactory.JsonSerialize<Organization>(org);
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = _httpService.CallPostMethod(postData, APIUrls.Organization_General + "/" + mode, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                if (httpresponse.Success) { SuccessMessage = ConstMessage.Update_Organization_Success; } else { ErrorMessage = ConstMessage.Error; }
            }
            return RedirectToAction("OrgSettings");
        }
        #endregion
    }
}