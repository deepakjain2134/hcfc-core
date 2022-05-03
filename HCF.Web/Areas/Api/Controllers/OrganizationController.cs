using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using AutoMapper;
using Hcf.Api.Application;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Web.Areas.Api.Models;
using HCF.Web.Areas.Api.Models.Request;
using HCF.Web.Areas.Api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HCF.Web.Areas.Api.Controllers
{

    [Route("api/org")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Organization")]
    public class OrganizationApiController : ApiController
    {
        private readonly ILoggingService _loggingService;
        private readonly IFloorService _floorService;
        private readonly IFloorTypeService _floorTypeService;
        private readonly ILocationService _locationService;
        private readonly IApiCommon _apiCommon;
        private readonly IOrganizationService _organizationService;
        private readonly IBuildingsService _buildingsService;
        private readonly ICommonProvider _commonProvider;
        private readonly ISiteService _siteService;
        private readonly IMapper _mapper;

        public OrganizationApiController(ICommonProvider commonProvider, IBuildingsService buildingsService, IOrganizationService organizationService, IApiCommon apiCommon, ILoggingService loggingService, IFloorService floorService,
            ILocationService locationService, IFloorTypeService floorTypeService, ISiteService siteService, IMapper mapper)
        {
            _commonProvider = commonProvider;
            _buildingsService = buildingsService;
            _organizationService = organizationService;
            _apiCommon = apiCommon;
            _loggingService = loggingService;
            _floorService = floorService;
            _locationService = locationService;
            _floorTypeService = floorTypeService;
            _siteService = siteService;
            _mapper = mapper;
        }

        #region MasterData

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetMasterData()
        {
            var _objHttpResponseMessage = _organizationService.GetMastersData();
            _objHttpResponseMessage.Success = true;
            //remove code start
            if (_objHttpResponseMessage.Result == null)
                _objHttpResponseMessage.Result = new Result();
            _objHttpResponseMessage.Result.Organization = _organizationService.GetMasterData();
            // end
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetWoMasterData()
        {
            var _objHttpResponseMessage = _organizationService.GetWOMastersData();
            _objHttpResponseMessage.Success = true;
            //remove code start
            if (_objHttpResponseMessage.Result == null)
                _objHttpResponseMessage.Result = new Result();
            _objHttpResponseMessage.Result.Organization = _organizationService.GetWoMasterData();
            // end     


            return _objHttpResponseMessage;
        }


        #endregion

        #region Organization

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetOrganization()
        {
            var _objHttpResponseMessage = new HttpResponseMessage { Result = new Result() };
            var org = _organizationService.GetOrganization();
            if (org != null)
            {
                _objHttpResponseMessage.Result.Organization = org;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
                _objHttpResponseMessage.Success = false;
            }

            return _objHttpResponseMessage;
        }

        /// <summary>
        /// </summary>
        /// <param name="updateOrg"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("UpdateOrg/{mode?}")]
        public HttpResponseMessage UpdateOrg(Organization updateOrg, string mode)
        {
            var updateMode = BDO.Enums.UpdateMode.All.ToString();
            if (!string.IsNullOrEmpty(mode))
                updateMode = mode;

            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result()
            };
            if (updateOrg.FilesContent != null)
                updateOrg.DashBoadImagePath = _apiCommon.SaveFile(updateOrg.FilesContent, updateOrg.FileName, "dashBoardImage", updateOrg.OrganizationId.ToString()).FilePath;
            if (updateOrg.LogoBase64 != null)
                updateOrg.LogoPath = _apiCommon.SaveFile(updateOrg.LogoBase64, updateOrg.LogoName, "clientlogo", updateOrg.OrganizationId.ToString()).FilePath;
            var org = _organizationService.UpdateOrganization(updateOrg, updateMode);
            // var newOrg = OrganizationRepository.GetOrganization();
            _objHttpResponseMessage.Result.Organization = org;
            _objHttpResponseMessage.Success = true;
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// </summary>
        /// <param name="updateOrg"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage UpdateOrgOutpatient(Organization updateOrg)
        {
            var _objHttpResponseMessage = new HttpResponseMessage { Result = new Result() };
            var org = _organizationService.UpdateOrganization(updateOrg, BDO.Enums.UpdateMode.IsOutPatient.ToString());
            _objHttpResponseMessage.Result.Organization = org;
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetChildOrganizations(string parentOrgKey)
        {
            var _objHttpResponseMessage = new HttpResponseMessage { Result = new Result() };
            var org = _organizationService.GetOrganizations(Guid.Parse(parentOrgKey));
            if (org != null)
            {
                _objHttpResponseMessage.Result.Organizations = org;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;
        }


        #endregion

        #region Site/Campus

        /// <summary>
        /// Returns a list of sites 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("sites")]
        public ActionResult GetSites()
        {
            List<Site> sites = _siteService.GetSites().Where(x => x.IsActive).ToList();
            List<SiteResponseModel> sitesViewModel = _mapper.Map<List<SiteResponseModel>>(sites);
            if (sitesViewModel.Count > 0)
            {
                var data = new Response<List<SiteResponseModel>>(sitesViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        /// <summary>
        /// Creates sites with the details specified in the request body
        /// </summary>
        /// <param name="newSite"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sites")]
        public ActionResult SaveSites([FromBody] SaveSiteRequestModel RequestModel)
        {
            Site newSite = _mapper.Map<BDO.Site>(RequestModel);
            newSite.SiteTypeId = 1;
            newSite.SiteId = _siteService.Save(newSite);
            if (newSite.SiteId > 0)
            {
                var data = new Response<Int32>(newSite.SiteId, ConstMessage.Saved_Successfully);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Error));
        }

        /// <summary>
        /// Updates sites with the details specified in the request body
        /// </summary>
        /// <param name="newSite"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("sites/{siteId}")]
        public ActionResult SaveSites([FromBody] UpdateSiteRequestModel RequestModel, int siteId)
        {
            Site newSite = _mapper.Map<BDO.Site>(RequestModel);
            newSite.SiteId = siteId;
            newSite.SiteId = _siteService.Save(newSite);
            if (newSite.SiteId > 0)
            {
                var data = new Response<Int32>(newSite.SiteId, ConstMessage.Success_Updated);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Error));
        }

        /// <summary>
        /// Returns specified sites
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("sites/{siteId}")]
        public ActionResult GetSites(int siteId)
        {
            Site site = _siteService.GetSites().FirstOrDefault(x => x.SiteId == siteId);
            SiteResponseModel siteViewModel = _mapper.Map<SiteResponseModel>(site);
            if (site != null && site.SiteId > 0)
            {
                var data = new Response<SiteResponseModel>(siteViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        #endregion Site/Campus

        #region Buildings

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetBuildings(string status)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var buildings = _buildingsService.GetBuildings();

            if (!string.IsNullOrEmpty(status))
            {
                var isActive = Convert.ToBoolean(Convert.ToInt32(status));
                buildings = buildings.Where(x => x.IsActive == isActive).ToList();

            }
            if (buildings.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Buildings = buildings };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }

            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newBuilding"></param>
        /// <returns></returns>   
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public ActionResult SaveBuilding(Buildings newBuilding)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (newBuilding.BuildingId > 0)
            {
                _buildingsService.UpdateBuilding(newBuilding);
            }
            else
            {
                newBuilding.BuildingId = _buildingsService.Save(newBuilding);
            }
            if (newBuilding.BuildingId > 0)
            {
                _objHttpResponseMessage.Success = true;
            }
            _objHttpResponseMessage.Result = new Result()
            {
                Building = newBuilding
            };
            return Ok(_objHttpResponseMessage);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetBuildingsFloor()
        {
            var buildings = _buildingsService.GetBuildingFloors();
            var response = new HttpResponseMessage
            {
                Success = true,
                Result = new Result { Buildings = buildings }
            };
            return Ok(response);
        }

        /// <summary>
        /// Returns a list of building 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("buildings")]
        public ActionResult GetBuildings()
        {
            var buildings = _buildingsService.GetBuildings();
            List<BuildingResponseModel> buildingsViewModel = _mapper.Map<List<BuildingResponseModel>>(buildings);
            if (buildingsViewModel.Count > 0)
            {
                var data = new Response<List<BuildingResponseModel>>(buildingsViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }



        /// <summary>
        /// Creates building with the details specified in the request body
        /// </summary>
        /// <param name="RequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("buildings")]
        public ActionResult AddBuilding([FromBody] SaveBuildingRequestModel RequestModel)
        {
            BDO.Buildings newBuilding = _mapper.Map<BDO.Buildings>(RequestModel);
            newBuilding.BuildingTypeId = 1;
            newBuilding.BuildingId = _buildingsService.Save(newBuilding);
            if (newBuilding.BuildingId > 0)
            {
                var data = new Response<Int32>(newBuilding.BuildingId, ConstMessage.Saved_Successfully);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));

        }

        /// <summary>
        /// Updates building with the details specified in the request body
        /// </summary>
        /// <param name="RequestModel"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("buildings/{buildingId}")]
        public ActionResult AddBuilding([FromBody] UpdateBuildingRequestModel RequestModel, int buildingId)
        {
            BDO.Buildings newBuilding = _mapper.Map<BDO.Buildings>(RequestModel);
            newBuilding.BuildingId = buildingId;
            newBuilding.BuildingId = _buildingsService.UpdateBuilding(newBuilding);
            if (newBuilding.BuildingId > 0)
            {
                var data = new Response<Int32>(newBuilding.BuildingId, ConstMessage.Success_Updated);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));

        }

        /// <summary>
        /// Returns specified building
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("buildings/{buildingId}")]
        public ActionResult GetBuildings(int buildingId)
        {
            var buildings = _buildingsService.GetBuildings().FirstOrDefault(x => x.BuildingId == buildingId);
            BuildingResponseModel buildingsViewModel = _mapper.Map<BuildingResponseModel>(buildings);
            if (buildingsViewModel != null && buildingsViewModel.BuildingId > 0)
            {
                var data = new Response<BuildingResponseModel>(buildingsViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        #endregion Buildings

        #region Floors


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        //[Route("floors")]
        public ActionResult GetFloors()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<Floor> floors = _floorService.GetFloors();
            if (floors.Count > 0)
            {
                _objHttpResponseMessage.Success = true;

                _objHttpResponseMessage.Result = new Result
                {
                    Floors = floors
                };
            }
            else
            {
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// Returns a list of floor 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("floors")]

        public ActionResult GetAllFloors(int? buildingId)
        {
            var floors = new List<Floor>();
            if (buildingId.HasValue && buildingId.Value > 0)
                floors = _floorService.GetFloors().Where(x => x.BuildingId == buildingId).ToList();
            else
                floors = _floorService.GetFloors();
            List<FloorResponseModel> floorsViewModel = _mapper.Map<List<FloorResponseModel>>(floors);
            if (floorsViewModel.Count > 0)
            {
                var data = new Response<List<FloorResponseModel>>(floorsViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        /// <summary>
        /// Returns specified floor
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[Route("floors/{floorId}")]
        public ActionResult GetFloors(int floorId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var floor = _floorService.GetFloors().FirstOrDefault(x => x.FloorId == floorId);
            if (floor != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Floor = floor };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }

            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// Returns specified floor
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("floors/{floorId}")]
        public ActionResult GetFloorbyFloorId(int floorId)
        {
            var floor = _floorService.GetFloors().FirstOrDefault(x => x.FloorId == floorId);
            FloorResponseModel floorsViewModel = _mapper.Map<FloorResponseModel>(floor);
            if (floorsViewModel != null && floorsViewModel.BuildingId > 0)
            {
                var data = new Response<FloorResponseModel>(floorsViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorTypeId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetFloorTypes(string floorTypeId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (!string.IsNullOrEmpty(floorTypeId))
            {
                int floortype = Convert.ToInt32(floorTypeId);
                FloorTypes floorType = _floorTypeService.GetFloorTypeFloors(floortype, null);
                if (floorType != null)
                {
                    _objHttpResponseMessage.Success = true;
                    _objHttpResponseMessage.Result = new Result
                    {
                        FloorType = floorType
                    };
                }
                else
                {
                    _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.No_Records_Found;
                }
            }
            else
            {
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.Invalid_Parameter;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage FloorTypes()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<FloorTypes> floorTypes = _floorTypeService.GetFloorTypes();
            if (floorTypes.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    FloorTypes = floorTypes
                };
            }
            else
            {
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.No_Records_Found;
            }

            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public List<Floor> GetBuildingFloor(int buildingId)
        {
            return _floorService.GetBuildingFloors(buildingId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newFloor"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addFloor")]
        public HttpResponseMessage AddFloor(Floor newFloor)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (newFloor.FloorId > 0)
            {
                _floorService.UpdateFloor(newFloor);
                _objHttpResponseMessage.Message = ConstMessage.Update_floor_Success;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                newFloor.FloorId = _floorService.Save(newFloor);
                if (newFloor.FloorId > 0)
                {
                    _objHttpResponseMessage.Message = ConstMessage.Insert_floor_Success;
                    _objHttpResponseMessage.Success = true;
                }
                else { _objHttpResponseMessage.Message = ConstMessage.Error; _objHttpResponseMessage.Success = false; }
            }

            foreach (var floorTypes in newFloor.FloorPlanTypes)
            {
                foreach (var item in floorTypes.FloorPlans)
                {
                    //var bytes = "";
                    item.FloorId = newFloor.FloorId;
                    item.CreatedBy = newFloor.CreatedBy;
                    if (!string.IsNullOrEmpty(item.FileContent))
                    {
                        item.ImagePath = _apiCommon.SaveFile(item.FileContent, item.FileName, "FloorImage", newFloor.BuildingId.ToString()).FilePath;
                        string ext = Path.GetExtension(item.FileName);
                        if (ext == ".svg" || ext == ".pdf" || ext == ".dwg")
                        {
                            item.ThumbImagePath = _apiCommon.SaveFile(item.FileContent, item.FileName, "FloorImage", newFloor.BuildingId.ToString() + "/thumb").FilePath;
                        }
                        else
                        {
                            var bytes = GetThumbNail(_commonProvider.FilePath(item.ImagePath));
                            item.ThumbImagePath = _apiCommon.SaveFile(bytes, item.FileName, "FloorImage", newFloor.BuildingId.ToString() + "/thumb").FilePath;
                        }
                    }
                    if (!string.IsNullOrEmpty(item.ImagePath) || (item.FloorPlanId != Guid.Empty && item.FloorPlanId != null))
                        _floorService.UpdateFloorPlan(item);
                }
            }
            _objHttpResponseMessage.Result = new Result { Floor = newFloor };
            return _objHttpResponseMessage;
        }


        /// <summary>
        /// Creates floor with the details specified in the request body
        /// </summary>
        /// <param name="newFloor"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("floors")]
        public ActionResult SaveFloor([FromBody] SaveFloorRequestModel RequestModel)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            BDO.Floor newFloor = _mapper.Map<BDO.Floor>(RequestModel);
            newFloor.FloorTypeId = 1;
            newFloor.FloorId = _floorService.Save(newFloor);
            if (newFloor.FloorTypeId > 0)
            {
                var data = new Response<Int32>(newFloor.FloorId, ConstMessage.Saved_Successfully);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Error));
        }

        /// <summary>
        /// Updates floor with the details specified in the request body
        /// </summary>
        /// <param name="RequestModel"></param>
        /// <param name="floorId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("floors/{floorId}")]
        public ActionResult SaveFloor([FromBody] UpdateFloorRequestModel RequestModel, int floorId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            BDO.Floor newFloor = _mapper.Map<BDO.Floor>(RequestModel);
            var res = _floorService.UpdateFloor(newFloor);
            if (res)
            {
                var data = new Response<Int32>(newFloor.FloorId, ConstMessage.Success_Updated);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Error));
        }




        //public static SvgImage SVGByteArrayToImage(byte[] bytesArr)
        //{
        //    using (var memstr = new MemoryStream(bytesArr))
        //    {
        //        return SvgImage.FromStream(memstr);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public string GetThumbNail(string url)
        {
            System.Net.ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // return string.Empty;
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(response.GetResponseStream()))
            {
                using (System.Drawing.Image thumbnail = image.GetThumbnailImage(100, 100, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        thumbnail.Save(memoryStream, ImageFormat.Jpeg);
                        Byte[] bytes = new Byte[memoryStream.Length];
                        memoryStream.Position = 0;
                        memoryStream.Read(bytes, 0, (int)bytes.Length);
                        //return "data:image/jpeg;base64," + Convert.ToBase64String(bytes, 0, bytes.Length);

                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newFloor"></param>
        /// <param name="strJson"></param>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private void UpdateMultipleFloorPlanonfloor(Floor newFloor, string strJson)
        {
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //var result = jsonSerializer.Deserialize<List<Floor>>(strJson);
            //foreach (Floor objfloor in result)
            //{
            //    objfloor.ImagePath = newFloor.ImagePath;
            //    objfloor.FileName = newFloor.FileName;
            //    objfloor.CreatedBy = newFloor.CreatedBy;
            //    _floorService.UpdateFloor(objfloor);
            //}
            //if (!string.IsNullOrEmpty(selectedfloorId))
            //{
            //    string[] floorId = selectedfloorId.Split(',');
            //    for (int i = 0; i < floorId.Length; i++)
            //    {
            //        if (!string.IsNullOrEmpty(floorId[i]))
            //        {
            //            Floor objfloor = new Floor();
            //            objfloor.FloorId = Convert.ToInt32(floorId[i]);
            //            objfloor.ImagePath = newFloor.ImagePath;
            //            objfloor.FileName = newFloor.FileName;
            //            objfloor.CreatedBy = newFloor.CreatedBy;
            //            HCF.BAL.FloorRepository.UpdateFloor(objfloor);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetFloorPlans(string floorId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var floor = _floorService.GetFloor(Convert.ToInt32(floorId));
            if (floor != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    Floor = floor
                };
            }
            else
            {
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorPlan"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage UpdateFloorPlan(FloorPlan floorPlan)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            bool status = _floorService.UpdateFloorPlan(floorPlan.FloorId, floorPlan.FloorPlanId);
            if (status)
            {
                _objHttpResponseMessage.Message = ConstMessage.Update_floorPlan_Success;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.Error;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorPlan"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("UpdateFloorPlanThumbImage")]
        public HttpResponseMessage UpdateFloorPlanThumbImage(FloorPlan floorPlan)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            int status = 0;
            if (string.IsNullOrEmpty(floorPlan.ThumbImagePath) && !string.IsNullOrEmpty(floorPlan.ImagePath))
            {
                string fileName = CommonUtility.CreateFileName(floorPlan.FileName);
                var bytes = GetThumbNail(_commonProvider.FilePath(floorPlan.ImagePath));
                floorPlan.ThumbImagePath = _apiCommon.SaveFile(bytes, fileName, "FloorImage", floorPlan.BuildingId.ToString() + "/Thumb").FilePath;
                status = _floorService.UpdateThumbImageFloorPlan(floorPlan);
            }
            if (status > 0)
            {
                _objHttpResponseMessage.Message = ConstMessage.Update_floorPlan_Success;
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { floorPlan = floorPlan };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.Error;
            }
            return _objHttpResponseMessage;
        }

        #endregion

        #region Location

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage Getlocations()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var locations = _locationService.GetLocationsMaster(null);
            if (locations != null)
            {
                _objHttpResponseMessage.Result.Stops = locations;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <param name="routeId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetLocationByFloor(string floorId, string routeId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var locations = _locationService.GetLocationrouteMapping(Convert.ToInt32(floorId), Convert.ToInt32(routeId));
            if (locations != null)
            {
                _objHttpResponseMessage.Result.StopsRouteMapping = locations;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationMaster"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage MngLocation(StopMaster locationMaster)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            int locationId = 0;
            if (locationMaster.StopId > 0)
            {
                locationId = _locationService.UpdateLocation(locationMaster);
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.Success_Updated;
            }
            else
            {
                locationId = _locationService.SaveLocation(locationMaster);
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.Success;
            }
            if (locationId > 0)
            {
                locationMaster.StopId = locationId;
                _objHttpResponseMessage.Result.Stop = locationMaster;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.Error;
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;
        }


        #endregion

        #region Route

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage Getroutes()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var routes = _locationService.GetRouteMaster(null);
            if (routes != null)
            {
                _objHttpResponseMessage.Result.Routes = routes;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeMaster"></param>
        /// <param name="locationsId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage ManageRoute(RouteMaster routeMaster, string locationsId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            int routeId = 0;
            routeId = _locationService.SaveRoute(routeMaster, locationsId);
            if (routeId > 0)
            {
                _objHttpResponseMessage.Message = ConstMessage.Success;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.Error;
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetRouteByFloor(string floorId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var routes = _locationService.GetRouteByFloor(Convert.ToInt32(floorId)).ToList();//.Where(x => x.FloorId == Convert.ToInt32(floorId) && x.IsActive == true).ToList();
            if (routes != null)
            {
                _objHttpResponseMessage.Result.Routes = routes;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;
        }

        #endregion

        #region Menu 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateMenu"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("UpdateMenu")]
        public HttpResponseMessage UpdateMenu(Menus updateMenu)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (updateMenu.FilesContent != null)
                updateMenu.ImagePath = _apiCommon.SaveFile(updateMenu.FilesContent, updateMenu.FileName, "menuImage", "").FilePath;
            var menus = new List<Menus>(); //MenuRepository.Update(updateMenu);
            _objHttpResponseMessage.Success = true;
            return _objHttpResponseMessage;
        }


        #endregion

        #region Mobile App API

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("app/GetBuildingsFloor")]
        public HttpResponseMessageApp AppGetBuildingsFloor()
        {
            var buildings = _buildingsService.GetBuildingFloors();
            var AppBuildings = JsonConvert.DeserializeObject<List<BuildingsApp>>(JsonConvert.SerializeObject(buildings));
            var response = new HttpResponseMessageApp
            {
                Success = true,
                Result = new ResultApp { Buildings = AppBuildings }
            };
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("app/GetHeaderMenu")]
        public HttpResponseMessageApp AppGetHeaderMenu(int UserId)
        {

            var org = new Organization();
            org = _organizationService.GetUserDashBoard(UserId, 0);
            var AppMenus = JsonConvert.DeserializeObject<List<MenuApp>>(JsonConvert.SerializeObject(org.Services));
            var response = new HttpResponseMessageApp
            {
                Success = true,
                Result = new ResultApp { Menus = AppMenus }
            };
            return response;
        }


        #endregion
    }
}