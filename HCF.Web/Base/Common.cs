//using HCF.BDO;
//using HCF.Utility;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;

//using HCF.BAL;
//using Newtonsoft.Json;
//using HCF.Web.ViewModels;
//using HCF.BAL.Ioc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using HCF.Utility.AppConfig;
//using System.Security.Policy;

//namespace HCF.Web.Base
//{
//    public class Common : ICommon
//    {
//        private readonly IAppSetting _appSettings;
//        public Common(IAppSetting appSetting)
//        {
//            _appSettings = appSetting;
//        }

//        public List<ActivityType> GetActivityType()
//        {
//            List<ActivityType> activityType = new List<ActivityType>();
//            var activity1 = new ActivityType
//            {
//                Name = "EP",
//                ActivityTypeId = 1
//            };
//            var activity2 = new ActivityType
//            {
//                Name = "Assets",
//                ActivityTypeId = 2
//            };
//            var activity3 = new ActivityType
//            {
//                Name = "Doc",
//                ActivityTypeId = 3
//            };
//            activityType.Add(activity1);
//            activityType.Add(activity2);
//            activityType.Add(activity3);
//            return activityType;
//        }

//        public List<CRxWeeks> GetWeeks(int year, int month)
//        {
//            var lists = new List<CRxWeeks>();
//            var count = 1;
//            var date = new DateTime(year, month, 1);
//            var dates = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n => new DateTime(date.Year, date.Month, n));

//            var weekends = from d in dates
//                           where d.DayOfWeek == DayOfWeek.Monday
//                           select d;
//            foreach (var item in weekends)
//            {
//                var obj = new CRxWeeks { weekStart = item, weekFinish = item.AddDays(6), weekNum = count };
//                count++;
//                lists.Add(obj);
//            }
//            return lists;
//        }

//        public string GetUrl(string routeName)
//        {
//            var url = string.Empty;
//            //if (RouteTable.Routes[routeName] is Route route)
//            //    url = route.Url;
//            return url;
//        }

//        public IEnumerable<SelectListItem> Months
//        {
//            get
//            {
//                return DateTimeFormatInfo
//                       .InvariantInfo
//                       .MonthNames
//                       .Select((monthName, index) => new SelectListItem
//                       {
//                           Value = (index + 1).ToString(),
//                           Text = monthName
//                       });
//            }
//        }

//        public IEnumerable<SelectListItem> Days
//        {
//            get
//            {
//                return DateTimeFormatInfo
//                       .InvariantInfo
//                       .DayNames
//                       .Select((dayName, index) => new SelectListItem
//                       {
//                           Value = (index + 1).ToString(),
//                           Text = dayName
//                       });
//            }
//        }

//        public string ReturnImagePath(int docStatus, bool isDocRequired)
//        {
//            string imagePath;
//            switch (Convert.ToInt32(docStatus))
//            {
//                case 1: // document Uploaded 
//                    imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/doc_required_upload.png";
//                    break;
//                case 2:  // partial upload document 
//                    imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/incomplete_doc_icon.png";
//                    break;
//                case 0: // doc uploaded but due date expired
//                    imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/doc_due.png";
//                    break;
//                case -1: // doc required but not yet uploaded
//                    imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/doc_required.png";
//                    break;
//                case -3:// document not required
//                    imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/doc_not_required.png";
//                    if (isDocRequired)
//                        imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/doc_required.png";
//                    break;
//                case 3:// document not required but uploaded
//                    imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/doc_not_required_upload.png";
//                    break;
//                default:
//                    imagePath = "";
//                    break;
//            }
//            return imagePath;
//        }

//        public string UploadDocTypeImagePath(int? uploadDocTypeId, int? docTypeId)
//        {
//            string imagePath;
//            if (docTypeId.HasValue && docTypeId.Value == (int)BDO.Enums.UploadDocTypes.MiscDocuments)
//                imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.MiscDocuments.Replace("~", "");
//            else
//            {
//                switch (uploadDocTypeId)
//                {
//                    case (int)BDO.Enums.UploadDocTypes.Policy: // document Uploaded 
//                        imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.PolicyIcon.Replace("~", "");
//                        break;
//                    case (int)BDO.Enums.UploadDocTypes.Report: // partial upload document 
//                        imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.Report.Replace("~", "");
//                        break;
//                    case (int)BDO.Enums.UploadDocTypes.MiscDocuments: // doc required but not yet uploaded
//                        imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.MiscDocuments.Replace("~", "");
//                        break;
//                    case (int)BDO.Enums.UploadDocTypes.SampleDocument: // document not required
//                        imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.SampleDocument.Replace("~", "");
//                        break;
//                    case (int)BDO.Enums.UploadDocTypes.AssetsReport: // 
//                        imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.Report.Replace("~", "");
//                        break;
//                    default:
//                        imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.DocumentIcon.Replace("~", "");
//                        break;
//                }
//            }
//            return imagePath;

//        }

//        public string ReturnImageTooltip(int docStatus, bool isDocRequired)
//        {
//            string tooltip;
//            switch (Convert.ToInt32(docStatus))
//            {
//                case 1: // document Uploaded 
//                    tooltip = "Required document is attached";
//                    break;
//                case 2:  // partial upload document 
//                    tooltip = "Awaiting for more required documents";
//                    break;
//                case 0: // doc required but not yet uploaded
//                    tooltip = "Document required";
//                    break;
//                case -3:// document not required
//                    tooltip = "Document is optional";
//                    if (isDocRequired)
//                        tooltip = _appSettings.WebUrlPath + "dist/Images/Icons/doc_required.png";
//                    break;
//                case 3:// document not required but uploaded
//                    tooltip = "Document is attached";
//                    break;
//                default:
//                    tooltip = "";
//                    break;
//            }
//            return tooltip;
//        }

//        public string FilePath(string path)
//        {
//            if (!string.IsNullOrEmpty(path))
//            {
//                var bucketName = Convert.ToString(UserSession.CurrentOrg.ClientNo);
//                var absPath = _appSettings.ImageBasePath + bucketName + "/" + path.Replace("~/", "");
//                return absPath.ToLower();
//            }
//            else
//                return "";
//        }
//        public string FilePreviewPath(string path)
//        {
//            if (!string.IsNullOrEmpty(path))
//            {
//                //var bucketName = Convert.ToString(UserSession.CurrentOrg.ClientNo);
//                var absPath = _appSettings.ImageBasePath + path.Replace("~/", "");
//                return absPath.ToLower();
//            }
//            else
//                return "";
//        }

//        public string CommonFilePath(string path)
//        {
//            if (!string.IsNullOrEmpty(path))
//            {
//                var absPath = _appSettings.CommonFileBasePath + path.Replace("~/", "");
//                return absPath.ToLower();
//            }
//            else
//                return "";
//        }

//        public string GetTranStatus(int transStatus)
//        {
//            string status = string.Empty;
//            switch (transStatus)
//            {
//                case -1:
//                    status = "status_not_started";
//                    break;
//                case 1:
//                    status = "status_pass";
//                    break;
//                case 2:
//                    status = "status_inprogress";
//                    break;
//                case 3:
//                    status = "status_pastdue";
//                    break;
//                case 0:
//                    status = "status_pastdue";
//                    break;
//                case 4:
//                    status = "status_grace_period";
//                    break;
//            }
//            return status;
//        }


//        public string GetInspectionStatus(string transStatus)
//        {
//            string status = string.Empty;
//            switch (transStatus)
//            {
//                case "NS":
//                    status = "status_pastdue";//"status_not_started";
//                    break;
//                case "NA":
//                    status = "status_pass";
//                    break;
//                case "IN":
//                    status = "status_inprogress";
//                    break;
//                case "PD":
//                    status = "status_pastdue";
//                    break;
//                case "DE":
//                    status = "status_pastdue";
//                    break;
//                case "GP":
//                    status = "status_grace_period";
//                    break;
//                case "RA":
//                    status = "status_pastdue";
//                    break;
//                case "GY":
//                    status = "alleps";
//                    break;

//            }
//            return status;
//        }

//        public string GetAssetsTranStatus(int transStatus, DateTime? dueDate)
//        {
//            string status = string.Empty;
//            if (dueDate.HasValue)
//            {
//                if (DateTime.Now > dueDate.Value)
//                {
//                    status = "status_grace_in_days_sysadmin";
//                    return status;
//                }
//            }
//            switch (transStatus)
//            {
//                case -1:
//                    status = "assets_status_not_started";
//                    break;
//                case 1:
//                    status = "status_pass";
//                    break;
//                case 2:
//                    status = "status_inprogress";
//                    break;
//                case 3:
//                    status = "status_pastdue";
//                    break;
//                case 0:
//                    status = "status_pastdue";
//                    break;
//            }
//            return status;
//        }

//        public string GetTranStatus(string transStatus)
//        {
//            string status = string.Empty;
//            switch (transStatus)
//            {
//                case "P":
//                    status = "status_not_started";
//                    break;
//                case "I":
//                    status = "status_inprogress";
//                    break;
//                case "C":
//                    status = "status_pass";
//                    break;
//                case "F":
//                    status = "status_fail";
//                    break;
//                case "D":
//                    status = "status_pastdue";
//                    break;
//                case "G":
//                    status = UserSession.CurrentUser.IsPowerUser || UserSession.CurrentUser.UserGroups.Any(x => x.GroupId == 1) ? "status_grace_in_days_sysadmin" : "status_grace_in_days";
//                    break;
//            }
//            return status;
//        }

//        //public  string GetEPStatus(int transStatus)
//        //{
//        //    string status = string.Empty;
//        //    switch (transStatus)
//        //    {
//        //        case -1:
//        //            status = "status_not_started";
//        //            break;
//        //        case 2:
//        //            status = "status_inprogress";
//        //            break;
//        //        case 1:
//        //            status = "status_pass";
//        //            break;
//        //        case 0:
//        //            status = "status_fail";
//        //            break;

//        //    }
//        //    return status;
//        //}

//        public string GetEpStatus(int transStatus)
//        {
//            string status = string.Empty;
//            switch (transStatus)
//            {
//                case 1:
//                    status = "Pass";
//                    break;
//                case 2:
//                    status = "In Progress";
//                    break;
//                case 0:
//                    status = "Failed";
//                    break;
//            }
//            return status;
//        }

//        public string GetAssetStatus(TInspectionActivity activity)
//        {
//            string status = string.Empty;
//            if (activity != null)
//            {
//                switch (activity.Status)
//                {
//                    case 1:
//                        status = (activity.Status == 1 && activity.IlsmDate.HasValue) ? $"{"Pass"} ({"ILSM"})" : "Pass";
//                        break;
//                    case 2:
//                        status = "In progress";
//                        break;
//                    case 0:
//                        status = (activity.SubStatus == BDO.Enums.InspectionSubStatus.NA.ToString()) ? $"{"Failed"} ({"Resolved"})"
//                            : "Failed";
//                        break;
//                }
//            }
//            return status;
//        }

//        public string GetIlsmStatus(int transStatus)
//        {
//            string status = string.Empty;
//            switch (transStatus)
//            {
//                case 1:
//                    status = "Closed";
//                    break;
//                case 2:
//                    status = "In Progress";
//                    break;
//                case 0:
//                    status = "Open";
//                    break;
//            }
//            return status;
//        }

//        public string GetStatus(int transStatus)
//        {
//            string status = string.Empty;
//            switch (transStatus)
//            {
//                case 1:
//                    status = "Compliant";
//                    break;
//                case 2:
//                    status = "In Progress";
//                    break;
//                case 0:
//                    status = "Non - Compliant";
//                    break;
//            }
//            return status;
//        }

//        private string GetFloorBuilding(Floor floor)
//        {
//            return floor != null ? floor.FloorLocation : "NA";
//        }

//        public string GetFloorAssetLocation(TFloorAssets tfloorAssets)
//        {
//            return GetFloorBuilding(tfloorAssets.Floor);
//        }

//        public string GetFloorAssetShortLocation(TFloorAssets tfloorAssets)
//        {
//            if (tfloorAssets != null && tfloorAssets.Floor != null)
//            {
//                return tfloorAssets.Floor.ShortFloorLocation;
//            }
//            else
//                return "NA";
//        }

//        public string GetFloorAssetMake(int? manufactureId)
//        {
//            var manufactureName = "NA";
//            if (manufactureId.HasValue)
//            {
//                var makeList = UnityContextFactory<IManufactureService>.CreateContext().GetAll().Where(x => x.IsActive && x.ManufactureId == manufactureId);
//                foreach (var make in makeList)
//                    manufactureName = make.ManufactureName;
//            }
//            return manufactureName;
//        }

//        //public  string GetResponsible(EPDetails ePDetails)
//        //{
//        //    return ePDetails.EPUsers.Count > 0 ? ePDetails.EPUsers[0].FullName : string.Empty;
//        //}

//        //public  string GetFloorAssetLastInspectionDate(EPDetails ePDetails, int floorAssetId)
//        //{
//        //    string date = string.Empty;
//        //    if (ePDetails.Inspections.Count > 0)
//        //    {
//        //        var firstOrDefault = ePDetails.Inspections.FirstOrDefault(x => x.IsCurrent);
//        //        var activity = firstOrDefault?.TInspectionActivity.FirstOrDefault(x => x.FloorAssetId == floorAssetId && x.InspectionId == firstOrDefault.InspectionId && x.IsCurrent);
//        //        if (activity?.CreatedDate != null)
//        //            date = activity.ActivityInspectionDate != null
//        //                ? activity.ActivityInspectionDate.Value.ToClientTime().ToFormatDateTime()
//        //                : activity.CreatedDate.Value.ToFormatDateTime();
//        //    }
//        //    return date;
//        //}

//        //public  string GetFloorAssetNextDueDate(EPDetails ePDetails, int floorAssetId)
//        //{
//        //    string date = string.Empty;
//        //    if (ePDetails.Inspections.Count > 0)
//        //    {
//        //        var firstOrDefault = ePDetails.Inspections.FirstOrDefault(x => x.IsCurrent);
//        //        if (firstOrDefault != null)
//        //        {
//        //            var activity = firstOrDefault.TInspectionActivity.FirstOrDefault(x => x.FloorAssetId == floorAssetId && x.InspectionId == firstOrDefault.InspectionId && x.IsCurrent);
//        //            if (activity != null)
//        //            {
//        //                if (activity.DueDate != null) date = activity.DueDate.Value.ToLocalTime().ToFormatDate();
//        //            }
//        //        }
//        //    }
//        //    return date;
//        //}

//        //public  string GetFloorAssetLastInspectionDate(List<FloorAssetsInspection> inspectionList, int floorAssetId, int epId)
//        //{
//        //    if (inspectionList != null)
//        //    {
//        //        var inpection = inspectionList.FirstOrDefault(x => x.FloorAssetId == floorAssetId && x.EPDetailId == epId && x.IsCurrent);
//        //        if (inpection != null)
//        //        {
//        //            return inpection.InspectionDate.ToShortDateString();

//        //        }
//        //    }
//        //    return string.Empty;
//        //}

//        public string GetEpStandard(EPDetails epDetails)
//        {
//            var ePstandard = "";
//            if (epDetails.Standard != null)
//            {
//                ePstandard = $"{epDetails.Standard.TJCStandard},{epDetails.EPNumber}";
//            }
//            return ePstandard;
//        }

//        //public  string GetDate(DateTime? datetime)
//        //{
//        //    var date = "N/A";
//        //    if (datetime.HasValue)
//        //    {
//        //        date = datetime.Value.ToFormatDate();
//        //    }
//        //    return date;
//        //}

//        //public  string GetTime(DateTime? datetime)
//        //{
//        //    var time = "N/A";
//        //    if (datetime.HasValue)
//        //    {
//        //        time = datetime.Value.ToShortTimeString();
//        //    }
//        //    return time;
//        //}

//        public string GetIssueStatus(int status)
//        {
//            string currentStatus;
//            switch (status)
//            {
//                case 1:
//                    currentStatus = "Open";
//                    break;
//                case 0:
//                    currentStatus = "Close";
//                    break;
//                case 2:
//                    currentStatus = "In Progress";
//                    break;
//                default:
//                    currentStatus = "-";
//                    break;
//            }
//            return currentStatus;
//        }

//        //public  string ConvertToBase64(HttpPostedFileBase image)
//        //{
//        //    byte[] thePictureAsBytes;
//        //    using (var theReader = new BinaryReader(image.InputStream))
//        //    {
//        //        thePictureAsBytes = theReader.ReadBytes(image.ContentLength);
//        //    }
//        //    string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
//        //    return thePictureDataAsString;
//        //}

//        public string GetActiveClass(string url)
//        {
//            var menuName = string.Empty;
//            string actionName;
//            if (url.Contains("ChildPage"))
//            {
//                var pageId = Convert.ToInt32(url.Split('/')[3]);

//                menuName = UnityContextFactory<IOrganizationService>.CreateContext().GetUserDashBoard(UserSession.CurrentUser.UserId, 0).Services.FirstOrDefault(x => x.Id == pageId)?.Name.Replace(" ", "").Replace("&", "");
//            }
//            else
//            {
//                actionName = url.Split('/')[1];
//                switch (actionName.ToLower())
//                {
//                    case "activitydashboard":
//                        menuName = "Dashboard";
//                        break;
//                    case "risk_management":
//                        menuName = "RiskManagement";
//                        break;
//                    case "deficiencies":
//                        menuName = "RiskManagement";
//                        break;
//                    case "workorder":
//                        menuName = "WorkOrders";
//                        break;
//                    case "wocreate":
//                        menuName = "WorkOrders";
//                        break;
//                    case "inspection":
//                        menuName = "Inspection";
//                        break;
//                    case "assetview":
//                        menuName = "Inspection";
//                        break;
//                    case "generate_report":
//                        menuName = "Inspection";
//                        break;
//                    case "assets_generate_report":
//                        menuName = "Inspection";
//                        break;
//                    case "firedrill":
//                        menuName = "RoundsDrills";
//                        break;
//                    case "survey":
//                        menuName = "RoundsDrills";
//                        break;
//                    case "rounds":
//                        menuName = "RoundsDrills";
//                        break;
//                    case "reports_compliance":
//                        menuName = "Reports";
//                        break;
//                    case "repors_assets":
//                        menuName = "Reports";
//                        break;
//                    case "reports_risk_management_reports":
//                        menuName = "Reports";
//                        break;
//                    case "reports_documents":
//                        menuName = "Reports";
//                        break;
//                    case "reports_epassignments":
//                        menuName = "Reports";
//                        break;
//                    case "reports":
//                        menuName = "Reports";
//                        break;
//                    case "recentactivity":
//                        menuName = "RecentActivity";
//                        break;
//                    case "docrepo":
//                        menuName = "Binders";
//                        break;
//                    case "getepdocbybinder":
//                        menuName = "Binders";
//                        break;
//                    case "hvacfloorplans":
//                        menuName = "Drawings";
//                        break;
//                    case "pfloorplans":
//                        menuName = "Drawings";
//                        break;
//                    case "mgfloorplans":
//                        menuName = "Drawings";
//                        break;
//                    case "floorplanhistory":
//                        menuName = "Drawings";
//                        break;
//                    case "general":
//                        menuName = "SetUp";
//                        break;
//                    case "vendorconfiguratons":
//                        menuName = "SetUp";
//                        break;
//                    case "permissions":
//                        menuName = "SetUp";
//                        break;
//                    case "epassignments":
//                        menuName = "SetUp";
//                        break;
//                    case "binders":
//                        menuName = "SetUp";
//                        break;
//                    case "floors":
//                        menuName = "SetUp";
//                        break;
//                    case "ilsm":
//                        menuName = "SetUp";
//                        break;
//                    case "ilsmScore":
//                        menuName = "SetUp";
//                        break;
//                    case "tmsrefresh":
//                        menuName = "SetUp";
//                        break;
//                    case "floorplans":
//                        menuName = "Drawings";
//                        break;
//                    case "dashboard": break;
//                    case "myprofile":
//                        menuName = "myProfile";
//                        break;
//                    case "changepassword":
//                        menuName = "myProfile";
//                        break;
//                    case "usermenu":
//                        menuName = "Usermenu";
//                        break;
//                    case "userrole":
//                        menuName = "userrole";
//                        break;
//                    case "epgroups":
//                        menuName = "epgroups";
//                        break;
//                    default:
//                        menuName = "Dashboard";
//                        break;


//                }
//            }
//            return menuName;
//        }

//        public int GetQuarter(DateTime? date = null)
//        {
//            if (!date.HasValue)
//            {
//                date = DateTime.Now;
//            }
//            int[] quarter = { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4 };
//            return quarter[(date.Value.Month) - 1];
//        }

//        public object GetAssetStatus(TFloorAssets item)
//        {
//            var activities = item.TInspectionActivity;
//            return activities.Count > 0 ? activities.FirstOrDefault()?.SubStatus : "-1";
//        }

//        public object GetAssetDue(TFloorAssets item)
//        {
//            var activities = item.TInspectionActivity;
//            if (activities.Count > 0)
//            {
//                return activities.FirstOrDefault().DueDate.HasValue
//                    ? (object)(activities.FirstOrDefault().DueDate.Value - DateTime.UtcNow).TotalDays
//                    : "-1";
//            }
//            else
//            {
//                return "-1";
//            }
//        }

//        public string GetVersion()
//        {
//            return _appSettings.BuildVersion;
//        }

//        public string ConvertToAMPM(TimeSpan timespan)
//        {
//            if (timespan.TotalMilliseconds > 0 || timespan != null)
//            {
//                var time = DateTime.Today.Add(timespan);
//                return time.ToString("hh:mm tt");
//            }
//            else { return ""; }
//        }

//        public string ConvertToAMPM(TimeSpan? timespan)
//        {
//            if (timespan.HasValue)
//            {
//                var time = DateTime.Today.Add(timespan.Value);
//                return time.ToString("hh:mm tt");
//            }
//            else { return ""; }
//        }

//        //public  string ConvertTo24HoursFormat(TimeSpan? timespan)
//        //{
//        //    if (timespan.HasValue)
//        //    {
//        //        var time = DateTime.Today.Add(timespan.Value);
//        //        return time.ToString("HH:mm");
//        //    }
//        //    else { return ""; }
//        //}

//        public string ConvertToAMPM(DateTime? dt)
//        {
//            return dt.HasValue ? dt.Value.ToString("hh:mm tt") : "";
//        }

//        public string ConvertToAMPMClient(DateTime? dt)
//        {
//            return dt.HasValue ? dt.Value.ToClientTime().ToString("hh:mm tt") : string.Empty;
//        }

//        #region json Serializer  / Deserialize

//        public T JsonDeserialize<T>(string result)
//        {
//            var data = JsonConvert.DeserializeObject<T>(result);
//            return data;
//        }

//        public string JsonSerialize<T>(object data)
//        {
//            var jsonString = string.Empty;
//            try
//            {
//                var microsoftDateFormatSettings = new JsonSerializerSettings
//                {
//                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
//                    MaxDepth = Int32.MaxValue

//                };

//                jsonString = JsonConvert.SerializeObject(data, microsoftDateFormatSettings);
//            }
//            catch (Exception ex)
//            {
//                //ErrorLog.LogError(ex, "JsonSerialize", "Common");
//            }
//            return jsonString;
//        }

//        #endregion

//        public bool IsLocalUrl(string url)
//        {
//          if (string.IsNullOrEmpty(url))
//                return false;
//            else
//            {
//                return ((url[0] == '/' && (url.Length == 1 ||
//                        (url[1] != '/' && url[1] != '\\'))) ||   // "/" or "/foo" but not "//" or "/\"
//                        (url.Length > 1 &&
//                         url[0] == '~' && url[1] == '/'));   // "~/" or "~/foo"
//            }
//        }

//        public List<UserProfile> RemoveCRxUsers(List<UserProfile> users)
//        {
//            return users.Where(x => !x.IsCRxUser).ToList();           
//        }

//        public bool RemoveCRxUsers(UserProfile user)
//        {
//            //if (user.IsPowerUser)
//            //{
//            //    var orgNames = _appSettings.ShowCRxUsers.Split(',');
//            //    return (orgNames.Contains(UserSession.UserOrg) || UserSession.CurrentUser.IsPowerUser);

//            //}
//            return true;
//        }

//        public string GetCommonMessage()
//        {
//            return _appSettings.CommonHospitalMessage;
//        }

//        public bool IsAdminUser()
//        {
//            return UserSession.CurrentUser.UserGroups.Any(x => x.GroupId == 2);
//        }

//        public bool IsSystemAdminUser()
//        {
//            return UserSession.CurrentUser.UserGroups.Any(x => x.GroupId == 1);
//        }

//        public bool IsFireWatchSupervisorUser()
//        {
//            return UserSession.CurrentUser.UserGroups.Any(x => x.GroupId == 7);
//        }

//        public string ReturnDocImageIcon(string extension)
//        {
//            string imagePath;
//            switch (extension.ToLower())
//            {
//                case ".pdf": // document Uploaded 
//                    imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.PDFIcon.Replace("~", "");
//                    break;
//                case ".doc": // document Uploaded                  
//                case ".docx": // document Uploaded                 
//                case ".xls": // document Uploaded 
//                    imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.DocumentBlueIcon.Replace("~", "");
//                    break;
//                case ".jpg": // document Uploaded   
//                case ".jpeg": // document Uploaded     
//                case ".png": // document Uploaded 
//                    imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.Imageicon.Replace("~", "");
//                    break;
//                default:
//                    imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.DocumentBlueIcon.Replace("~", "");
//                    break;
//            }
//            return imagePath;
//        }

//        #region ICRA PCRA menu check
//        public bool IsMenuExist(string menuname, string orgkey)
//        {


//            bool IsExist = UnityContextFactory<IOrganizationService>.CreateContext().CheckMenuExist(menuname, orgkey);
//            return IsExist;
//        }
//        #endregion

//        public string GetAlphabeticFromIndex(int column)
//        {
//            column--;
//            string col = Convert.ToString((char)('A' + (column % 26)));
//            while (column >= 26)
//            {
//                column = (column / 26) - 1;
//                col = Convert.ToString((char)('A' + (column % 26))) + col;
//            }
//            return col;
//        }


//        public string setUserName(HCF.BDO.UserProfile userProfile)
//        {
//            string userName = string.Empty;
//            if (userProfile != null && userProfile.UserId > 0)
//                return userProfile.FullName;
//            else
//                return "CRx";
//        }
//    }
//}