using System;
using System.Collections.Generic;
using System.Linq;

using System.Net;
using System.Threading;
using System.IO;
using HCF.BDO.MaintenanceConnection;
using HCF.BDO;
using HCF.Utility;
using System.Text;
using System.Web;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using HCF.Web.Base;

namespace HCF.Web.Controllers
{
    public class MaintenanceConnectionController : BaseController
    {
        private readonly ISiteService _siteService;
        private readonly IFloorService _floorService;
        private readonly IBuildingsService _buildingsService;
        private readonly IUserService _userService;
        private readonly ILoggingService _loggingService;
        private readonly ITmsConnectorService _tmsConnectorService;
        private readonly ICommonModelFactory _commonModelFactory;


        public MaintenanceConnectionController(ICommonModelFactory commonModelFactory, IBuildingsService buildingsService, ISiteService siteService, IFloorService floorService,
            IUserService userService, ILoggingService logging, ITmsConnectorService tmsConnectorService)
        {
            _commonModelFactory = commonModelFactory;
            _loggingService = logging;
            _buildingsService = buildingsService;
            _siteService = siteService;
            _floorService = floorService;
            _userService = userService;
            _tmsConnectorService = tmsConnectorService;
        }


        /// <summary>
        ///  below method will be  used for sync the Maintenance Connection Data to our CRx tables 
        ///  help docs https://api.maintenanceconnection.com/v8/help/docs
        /// </summary>
        private const string _classificationAPI = "https://api.maintenanceconnection.com/v8/classifications";
        private const string _resultAPI = "https://api.maintenanceconnection.com/v8/assets";
        private const string _workOrderAPI = "https://api.maintenanceconnection.com/v8/workorders";
        private const string _userAPI = "https://api.maintenanceconnection.com/v8/Labors";
        private const string _workOrderAssignmentsAPI = "https://api.maintenanceconnection.com/v8/WorkOrderAssignments";
        private const string _workOrderTasks = "https://api.maintenanceconnection.com/v8/WorkOrderTasks";
        private const string _workOrderTasksByWo = "https://api.maintenanceconnection.com/v8/WorkOrders/{0}/Tasks";

        private const string _lookupTableApi = "https://api.maintenanceconnection.com/v8/LookupTables";
        private const string _lookupTableValueApi = "https://api.maintenanceconnection.com/v8/LookupTableValues/{0}/";

        private const string _mcCnKey = "AKRONCH";
        private const string _mcApiKey = "fc5ff399-cb8d-4bdd-b4a2-19361678b072";


        public ActionResult Index()
        {
            return View();
        }

        #region Maintenance Connection (Save records from TMS to CRx TMS tables)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public JsonResult GetTMSClassification(int start = 0, int end = 500)
        {
            var rootObject = new List<TMS_Classifcation>();
            int total = 500;
            int skip = 0;
            while (total > 0)
            {
                var tmsresults = new RootObject<TMS_Classifcation>();
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string results = GetData(_classificationAPI, skip);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    tmsresults = _commonModelFactory.JsonDeserialize<RootObject<TMS_Classifcation>>(results);
                skip = skip + 500;
                total = tmsresults.Total - skip;
                rootObject.AddRange(tmsresults.Results);
            }

            foreach (var item in rootObject)
            {
                //int Id = BAL.Akron.TmsConnector.SaveTMSResults(item.);
                //if (Id == 0)
                //    ErrorLog.LogMsg("Result Record No:" + item.ID);
            }
            return Json("");
        }

        /// <summary>
        /// Save tms data like building/ floor /site/ zones / Assets type and Assets to Temp Table 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public JsonResult GetTMSResults(int days = 5000)
        {
            var lastupdateDate = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");
            var filterexp = $"&$filter=LastModifiedDate%20ge%20\"{lastupdateDate}\"";
            var rootObject = new List<TMS_Results>();
            int total = 500;
            int skip = 0;
            while (total > 0)
            {
                var tmsresults = new RootObject<TMS_Results>();
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string results = GetData(_resultAPI, skip, filterexp);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    tmsresults = _commonModelFactory.JsonDeserialize<RootObject<TMS_Results>>(results);
                skip = skip + 500;
                total = tmsresults.Total - skip;
                rootObject.AddRange(tmsresults.Results);
            }
            foreach (var item in rootObject)
            {
                int Id = _tmsConnectorService.SaveTMSResults(item);
                if (Id == 0)
                    _loggingService.Error("Result Record No:" + item.ID);
            }
            return Json("");
        }

        /// <summary>
        /// save work order to Temp Table
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public JsonResult GetTMSWorkOrder(int start = 0, int end = 500)
        {

            for (int i = 0; i <= 10; i++)
            {
                int skipcount = i * 500;
                RootObject<TMS_WorkOrderRes> tmsresults = new RootObject<TMS_WorkOrderRes>();
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string results = GetData(_workOrderAPI, skipcount);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    tmsresults = _commonModelFactory.JsonDeserialize<RootObject<TMS_WorkOrderRes>>(results);

                foreach (var item in tmsresults.Results)
                {
                    int Id = _tmsConnectorService.SaveTMSWorkOrderToLocal(item);
                    if (Id == 0)
                        _loggingService.Error("Work Order Record #:" + item.ID);
                }
            }
            return Json("");
        }

        /// <summary>
        /// save work order assignment to Temp Table
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        //public JsonResult GetTMSWorkOrderAssignment(int start = 0, int end = 500)
        //{

        //    for (int i = 0; i <= 6; i++)
        //    {
        //        int skipcount = i * 500;
        //        RootObject<TMS_WorkOrderAssignment> tmsresults = new RootObject<TMS_WorkOrderAssignment>();
        //        int statusCode = Convert.ToInt32(Enums.HttpReponseStatusCode.Success);
        //        string results = GetData(_workOrderAssignmentsAPI, skipcount);
        //        if (statusCode == Convert.ToInt32(Enums.HttpReponseStatusCode.Success))
        //            tmsresults = Base.Common.JsonDeserialize<RootObject<TMS_WorkOrderAssignment>>(results);

        //        foreach (var item in tmsresults.Results)
        //        {
        //            int Id = BAL.Akron.TmsConnector.SaveWorkOrderAssignmentToLocal(item);
        //            if (Id == 0)
        //                ErrorLog.LogMsg("Work Order Assignment Record No:" + item.PK);
        //        }
        //    }
        //    return Json("");
        //}

        public JsonResult GetTMSWorkOrderAssignment(int start = 0, int end = 500)
        {

            int TotalLength = 1;
            int i = 0;

            while (i <= TotalLength)
            {

                int skipcount = i * 500;
                RootObject<TMS_WorkOrderAssignment> tmsresults = new RootObject<TMS_WorkOrderAssignment>();
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string results = GetData(_workOrderAssignmentsAPI, skipcount);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    tmsresults = _commonModelFactory.JsonDeserialize<RootObject<TMS_WorkOrderAssignment>>(results);


                if (i == 0 && Convert.ToInt32(tmsresults.Total) > 0)
                    TotalLength = CalculateTotalResult(Convert.ToDouble(tmsresults.Total));

                foreach (var item in tmsresults.Results)
                {
                    int Id = _tmsConnectorService.SaveWorkOrderAssignmentToLocal(item);
                    if (Id == 0)
                        _loggingService.Error("Work Order Assignment Record No:" + item.PK);
                }
                i++;
            }
            return Json("");
        }

        /// <summary>
        /// CalculateTotalResult
        /// </summary>
        /// <param name="TotalResult"></param>
        /// <returns></returns>
        public int CalculateTotalResult(double TotalResult)
        {
            double number = ((double)(TotalResult) / (double)500);
            double result = Math.Ceiling(number);
            int TotalLength = Convert.ToInt32(result);
            if (TotalLength >= 1)
            {
                TotalLength = TotalLength - 1;
            }
            return TotalLength;
        }

        /// <summary>
        /// save tms user to temp table
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public JsonResult GetTmsUsers(int start = 0, int end = 500)
        {
            for (int i = 0; i <= 1; i++)
            {
                int skipcount = i * 500;
                var tmsresults = new RootObject<TMS_UserRes>();
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string results = GetData(_userAPI, skipcount);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    tmsresults = _commonModelFactory.JsonDeserialize<RootObject<TMS_UserRes>>(results);

                foreach (var item in tmsresults.Results)
                {
                    int Id = _tmsConnectorService.SaveTMSUsersToLocal(item);
                    if (Id == 0)
                        _loggingService.Error("User Record No:" + item.ID);
                }
            }
            return Json("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        //public JsonResult GetWorkOrderTasks(int workOrderPk = 0, int start = 0, int end = 500)
        //{
        //    string url = _workOrderTasks;
        //    if (workOrderPk > 0)
        //        url = string.Format(_workOrderTasksByWo, workOrderPk);

        //    for (int i = 0; i <= 1; i++)
        //    {
        //        int skipcount = i * 500;
        //        RootObject<TMS_WorkOrderTasks> tmsresults = new RootObject<TMS_WorkOrderTasks>();
        //        int statusCode = Convert.ToInt32(Enums.HttpReponseStatusCode.Success);
        //        string results = GetData(url, skipcount);
        //        if (statusCode == Convert.ToInt32(Enums.HttpReponseStatusCode.Success))
        //            tmsresults = Base.Common.JsonDeserialize<RootObject<TMS_WorkOrderTasks>>(results);

        //        foreach (var item in tmsresults.Results)
        //        {
        //            int Id = BAL.Akron.TmsConnector.SaveWorkOrderTasksToLocal(item);
        //            if (Id == -1)
        //                ErrorLog.LogMsg("WorkOrder Tasks No:" + item.PK);
        //        }
        //    }
        //    return Json("");
        //}

        public JsonResult GetWorkOrderTasks(int workOrderPk = 0, int start = 0, int end = 500)
        {
            string url = _workOrderTasks;
            if (workOrderPk > 0)
                url = string.Format(_workOrderTasksByWo, workOrderPk);
            int TotalLength = 1;
            int i = 0;
            while (i <= TotalLength)
            {
                //Console.WriteLine(i);                
                int skipcount = i * 500;
                RootObject<TMS_WorkOrderTasks> tmsresults = new RootObject<TMS_WorkOrderTasks>();
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string results = GetData(url, skipcount);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    tmsresults = _commonModelFactory.JsonDeserialize<RootObject<TMS_WorkOrderTasks>>(results);
                if (i == 0 && Convert.ToInt32(tmsresults.Total) > 0)
                    TotalLength = CalculateTotalResult(Convert.ToDouble(tmsresults.Total));
                foreach (var item in tmsresults.Results)
                {
                    int Id = _tmsConnectorService.SaveWorkOrderTasksToLocal(item);
                    if (Id == -1)
                        _loggingService.Error("WorkOrder Tasks No:" + item.PK);
                }
                i++;
            }
            return Json("");
        }

        /// <summary>
        /// Get TMS Lookup Table
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public JsonResult GetTMSlookupTable(int start = 0, int end = 500)
        {
            int TotalLength = 1;
            int i = 0;
            while (i <= TotalLength)
            {
                int skipcount = i * 500;
                RootObject<TMS_LookupTable> tmsresults = new RootObject<TMS_LookupTable>();
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string results = GetData(_lookupTableApi, skipcount);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    tmsresults = _commonModelFactory.JsonDeserialize<RootObject<TMS_LookupTable>>(results);
                if (tmsresults != null)
                {
                    if (i == 0 && Convert.ToInt32(tmsresults.Total) > 0)
                        TotalLength = CalculateTotalResult(Convert.ToDouble(tmsresults.Total));
                    foreach (var item in tmsresults.Results)
                    {
                        int Id = _tmsConnectorService.SavelookupTableToLocal(item);
                        if (Id > 0)
                            GetTMSlookupTableValues(item.LookupTableID);
                        else
                            _loggingService.Error("lookup Table Record No:" + item.LookupTableID);
                    }
                }
                i++;
            }
            return Json("");
        }

        /// <summary>
        /// Get TMS Lookup Table Values
        /// </summary>
        /// <param name="LookupTableID"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public JsonResult GetTMSlookupTableValues(string LookupTableID, int start = 0, int end = 500)
        {
            int TotalLength = 1;
            int i = 0;
            while (i <= TotalLength)
            {
                int skipcount = i * 500;
                if (!string.IsNullOrEmpty(LookupTableID))
                {
                    var lookupTableValueApi = string.Format(_lookupTableValueApi, LookupTableID);
                    RootObject<TMS_LookupTableValues> tmsresults = new RootObject<TMS_LookupTableValues>();
                    int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                    string results = GetData(lookupTableValueApi, skipcount);
                    if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                        tmsresults = _commonModelFactory.JsonDeserialize<RootObject<TMS_LookupTableValues>>(results);
                    if (tmsresults != null)
                    {
                        if (i == 0 && Convert.ToInt32(tmsresults.Total) > 0)
                            TotalLength = CalculateTotalResult(Convert.ToDouble(tmsresults.Total));
                        foreach (var item in tmsresults.Results)
                        {
                            int Id = _tmsConnectorService.SavelookupTableValuesToLocal(item);
                            if (Id == 0)
                                _loggingService.Error("lookup Table Values Record No:" + item.LookupTableID);
                        }
                    }
                    i++;
                }
            }
            return Json("");
        }


        /// <summary>
        /// call Maintenance Connection api using username and password
        /// </summary>
        /// <param name="apiurl"></param>
        /// <param name="skipno"></param>
        /// <returns></returns>
        public string GetData(string apiURL, int skipno)
        {
            return CommonUtility.MaintConnectionGet(apiURL, skipno, _mcCnKey, _mcApiKey);
        }

        public string GetData(string apiURL, int skipno, string filterexp)
        {
            return CommonUtility.MaintConnectionGet(apiURL, skipno, _mcCnKey, _mcApiKey, filterexp);
        }

        #endregion

        #region Temp Table to CRx Table 
        /// <summary>
        /// update  site / buildings /floor and zone from temp table to CRx table  
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateBuildingFloors()
        {
            List<TMS_Results> objTms = new List<TMS_Results>();
            objTms = _tmsConnectorService.GetAllTMSRecords();
            foreach (var site in objTms.Where(x => x.ClassificationRefPK == 27 && x.PK != 4))
            {
                Site objsite = new Site { Code = site.ID, Name = site.Name };
                int siteId = _siteService.Save(objsite);
                if (siteId > 0)
                {
                    foreach (var build in objTms.Where(x => x.ClassificationRefPK == 25 && x.ParentRefPK == site.PK))
                    {
                        Buildings objBuildings = new Buildings
                        {
                            BuildingCode = build.ID,
                            SiteCode = objsite.Code,
                            BuildingTypeId = 2,
                            BuildingName = build.Name
                        };
                        int buildId = _buildingsService.Save(objBuildings);
                        if (buildId > 0)
                        {
                            foreach (var floors in objTms.Where(x => x.ClassificationRefPK == 31 && x.ParentRefPK == build.PK))
                            {
                                Floor floor = new Floor
                                {
                                    BuildingId = buildId,
                                    FloorName = floors.Name,
                                    FloorCode = floors.ID,
                                    BuildingCode = build.ID
                                };
                                int floorId = _floorService.Save(floor);
                            }
                        }
                    }
                }
            }
            return Json("");
        }

        /// <summary>
        /// save assets from temp table to CRx table
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveAssets()
        {
            List<TFloorAssets> objFloorAssets = new List<TFloorAssets>();
            objFloorAssets = _tmsConnectorService.GetAllTMSAsset();
            foreach (var item in objFloorAssets)
            {
                item.CreatedBy = Base.UserSession.CurrentUser.UserId;
                int floorAssetId = _tmsConnectorService.SaveAssets(item);
            }
            return Json("Done");
        }

        public JsonResult SaveUsers()
        {
            List<UserProfile> userProfile = _tmsConnectorService.GetAllTMSUsers();
            foreach (var user in userProfile)
            {
                user.CreatedBy = Base.UserSession.CurrentUser.UserId;
                // user.UserTypeId = 5;
                int userId = _userService.CreateUser(user, "");
            }
            return Json("Done");
        }


        #endregion
    }
}