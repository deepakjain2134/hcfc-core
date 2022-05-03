using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Web.Base;
using HCF.Web.Controllers;
using HCF.Web.ViewModels.Location;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCF.Web.Controllers
{
    public class LocationController : BaseController
    {       
        private readonly ILocationService _locationService;
        private readonly IBuildingsService _buildingsService;

        public LocationController(         
            ILocationService locationService,
            IBuildingsService buildingsService
            )
        {          
            _locationService = locationService;
            _buildingsService = buildingsService;

        }

        #region Location Master

        //[CrxAuthorize(Roles = "Location_locations")]
        public ActionResult Locations()
        {
            UISession.AddCurrentPage("Location_locations", 0, "Locations");
            var locations = _locationService.GetLocationsMaster(null);
            return View(locations);
        }

        //public JsonResult GetLocationsByFloor(int floorId)
        //{
        //    var locations = new List<LocationMaster>();
        //    return Json(locations, );
        //}

        public ActionResult MngLocation(int? locationId)
        {
            StopMaster location = new StopMaster();
            var buildings = _buildingsService.GetBuildings().Where(x => x.IsActive).ToList();
            ViewBag.Buildings = buildings; //new SelectList(buildings, "BuildingId", "BuildingName", "");
            if (locationId > 0)
            {
                location = _locationService.GetLocationsMaster(locationId).FirstOrDefault();
            }
            return View(location);
        }


        public ActionResult FillStops(int floorId)
        {
            var stops = _locationService.GetLocationsMaster(null).Where(x => x.FloorId == floorId).ToList();
            return Json(stops);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MngLocation(StopMaster stopMaster)
        {
            if (ModelState.IsValid)
            {
                // int locationId = 0;
                stopMaster.CreatedBy = UserSession.CurrentUser.UserId;
                if (stopMaster.StopId > 0)
                    _locationService.UpdateLocation(stopMaster);
                else
                    _locationService.SaveLocation(stopMaster);
                return RedirectToAction("locations");
            }
            //}
            var buildings = _buildingsService.GetBuildings().Where(x => x.IsActive).ToList();
            ViewBag.Buildings = buildings;
            return View(stopMaster);
        }

        #endregion

        #region Route

        // [CrxAuthorize(Roles = "Location_routes")]
        public ActionResult Routes()
        {
            UISession.AddCurrentPage("Location_routes", 0, "Routes");
            var routes = _locationService.GetRouteMaster(null);
            return View(routes);
        }

        public ActionResult mngRoute(int? routeId)
        {
            var route = new RouteMaster();
            var buildings = _buildingsService.GetBuildings().Where(x => x.IsActive).ToList();
            ViewBag.Buildings = buildings; //new SelectList(buildings, "BuildingId", "BuildingName", "");          
            if (routeId > 0)
                route = _locationService.GetRouteMaster(routeId).FirstOrDefault();

            return View(route);
        }

        [HttpPost]
        public ActionResult mngRoute(RouteMaster routeMaster, string locationsId)
        {
            if (ModelState.IsValid)
            {
                if (routeMaster.RouteId == 0 && routeMaster.RouteNo == null)
                {
                    ErrorMessage = Utility.ConstMessage.Route_Name_Blank;
                    return RedirectToAction("routes");
                }

                routeMaster.CreatedBy = UserSession.CurrentUser.UserId;
                var routeId = _locationService.SaveRoute(routeMaster, locationsId);
                if (routeId > 0)
                {
                    return RedirectToAction("routes");
                }
                else
                {
                    ErrorMessage = HCF.Utility.ConstMessage.Route_Name_Already_Exist;
                    return RedirectToAction("routes");
                }
            }
            ErrorMessage = HCF.Utility.ConstMessage.Internal_Server_Error;
            List<Buildings> buildings = _buildingsService.GetBuildings().Where(x => x.IsActive).ToList();
            ViewBag.Buildings = buildings;
            return View(routeMaster);
        }

        public JsonResult GetLocationByFloor(int? floorId, int? routeId)
        {
            var locations = _locationService.GetLocationrouteMapping(floorId, routeId);
            return Json(locations);
        }


        public JsonResult GetLocationRoute(int? routeId)
        {
            var locations = _locationService.GetLocationRouteMapping(routeId);
            return Json(locations);
        }


        public JsonResult GetRouteByFloor(int floorId)
        {
            var results = _locationService.GetRouteByFloor(floorId).ToList();
            //var results = LocationRepository.GetRouteMaster(null).Where(x => x.FloorId == floorId && x.IsActive == true).ToList();
            return Json(results);
        }

        #endregion



        #region Location Groups

        public ActionResult LocationGroupBuildings()
        {
            UISession.AddCurrentPage("rounds_LocationGroupBuildings", 0, "Group Buildings");
            LocationGroupViewModel model = BindModelValue();
            return View(model);
        }

        public ActionResult LocationGroupBuilding()
        {
            LocationGroupViewModel model = BindModelValue();
            return PartialView("_locationGroupBuilding", model);
        }

        private LocationGroupViewModel BindModelValue()
        {
            var model = new LocationGroupViewModel
            {
                locationGroup = _locationService.GetLocationGroup(),
                buildings = _locationService.GetGroupBuildings().ToList()
            };
            return model;
        }

        public ActionResult GetGroupBuildings()
        {
            var model = new LocationGroupViewModel
            {
                buildings = _locationService.GetGroupBuildings().ToList()
            };
            return PartialView("_getGroupBuildings", model);
        }

        public ActionResult AddLocationGroup(int? locationGroupId)
        {   
            var objLocationGroups = new LocationGroups();
            if (locationGroupId == null)
            {
                objLocationGroups.CreatedBy = UserSession.CurrentUser.UserId;
                objLocationGroups.IsActive = true;
                return PartialView("AddLocationGroup", objLocationGroups);
            }
            else if (locationGroupId.HasValue && locationGroupId.Value > 0)
                objLocationGroups = _locationService.GetLocationGroup().FirstOrDefault(x => x.LocationGroupId == locationGroupId.Value);
            return PartialView("AddLocationGroup", objLocationGroups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLocationGroup(LocationGroups objLocationGroups)
        {
            if (ModelState.IsValid)
            {
                objLocationGroups.CreatedBy = UserSession.CurrentUser.UserId;
                objLocationGroups.LocationGroupId = _locationService.SaveLocationGroup(objLocationGroups);
                if (objLocationGroups.LocationGroupId > 0)
                    SuccessMessage = Utility.ConstMessage.Saved_Successfully;
            }
            return View();
        }

        public ActionResult LocationGroup()
        {
            var locationGroup = _locationService.GetLocationGroup();
            return PartialView("_locationGroup", locationGroup);
        }

        public JsonResult SaveGroupLocationsDetails(string locationGroupId, string buildingId, bool IsActive)
        {
            LocationGroupDetails objlocationGroupDetails = new LocationGroupDetails();
            objlocationGroupDetails.LocationGroupId = Convert.ToInt32(locationGroupId);
            objlocationGroupDetails.BuildingId = Convert.ToInt32(buildingId);
            objlocationGroupDetails.IsActive = IsActive;
            objlocationGroupDetails.CreatedBy = Base.UserSession.CurrentUser.UserId;
            var locationGroupDetailId = _locationService.SaveGroupLocationsDetails(objlocationGroupDetails);
            return Json(locationGroupDetailId);
        }

        public JsonResult GetGroupLocationsDetails(string locationGroupId)
        {
            var groupLocation = _locationService.GetGroupLocationsDetails(Convert.ToInt32(locationGroupId));
            return Json(new { Result = groupLocation });
        }

        public JsonResult GetBuildingAndLocationBySite(string sitecode)
        {
            List<Buildings> buildings = new();
            //List<Buildings> individualBuilding = new List<Buildings>();
            //var locationGroup = _locationService.GetLocationGroup();
            //foreach (var item in locationGroup)
            //{
            //    Buildings building = new Buildings();
            //    building.BuildingName = item.Name;
            //    building.LocationGroupId = item.LocationGroupId;
            //    building.BuildingId = 0;
            //    building.BuildingTypeId = 0;
            //    buildings.Add(building);
            //}
            if (!string.IsNullOrEmpty(sitecode))
                buildings = _locationService.GetGroupBuildings().Where(x => x.SiteCode.ToLower() == sitecode.ToLower() && x.IsActive).ToList();
            else
                buildings = _locationService.GetGroupBuildings().Where(x => x.IsActive).ToList();
            //buildings.AddRange(individualBuilding);
            return Json(buildings);
        }

        #endregion
    }
}