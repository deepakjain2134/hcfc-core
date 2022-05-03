using HCF.BAL;
using HCF.BDO;
using HCF.Module.Core.Extensions;
using HCF.Utility;
using HCF.Utility.AppConfig;
using HCF.Web.ViewModels;
using HCF.Web.ViewModels.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace HCF.Web.Base.Factory
{
    public class CommonModelFactory : ICommonModelFactory
    {
        private readonly IAppSetting _appSettings;
        private readonly IManufactureService _manufactureService;
        private readonly IOrganizationService _organizationService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IWorkContext _workContext;

        public CommonModelFactory(IWebHostEnvironment webHostEnvironment, IAppSetting appSettings, IManufactureService manufactureService, IOrganizationService organizationService
            , IWorkContext workContext)
        {
            _appSettings = appSettings;
            _manufactureService = manufactureService;
            _organizationService = organizationService;
            _webHostEnvironment = webHostEnvironment;
            _workContext = workContext;
        }

        public string GenerateCode()
        {
            string newdevicecode = "";
            var random = RandomNumberGenerator.Create();
            var bytes = new byte[sizeof(int)]; // 4 bytes
            random.GetNonZeroBytes(bytes);
            var val = Convert.ToString(BitConverter.ToInt32(bytes)).Replace("-", "");
            Console.WriteLine(val);
            if (val.Length > 5)
                newdevicecode = val.Substring(0, Math.Min(val.Length, 5));
            else
                newdevicecode = GenerateCode();
            return newdevicecode;
        }

        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        public string SizeSuffix(Int64 value, int decimalPlaces = 2)
        {
           
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return "0"; }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }
        public virtual LogoModel PrepareLogoModel()
        {
            var model = new LogoModel
            {
                OrganizationName = UserSession.CurrentOrg.Name,
                LogoPath = "~/dist/img/small-Logo.png"
            };
            return model;
        }
        public string ServerMapPath(string path)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;
            return string.Format("{0}{1}", webRootPath, path);
            //return Path.Combine(webRootPath, path);
        }
        public List<ActivityType> GetActivityType()
        {
            List<ActivityType> activityType = new List<ActivityType>();
            var activity1 = new ActivityType
            {
                Name = "EP",
                ActivityTypeId = 1
            };
            var activity2 = new ActivityType
            {
                Name = "Assets",
                ActivityTypeId = 2
            };
            var activity3 = new ActivityType
            {
                Name = "Doc",
                ActivityTypeId = 3
            };
            activityType.Add(activity1);
            activityType.Add(activity2);
            activityType.Add(activity3);
            return activityType;
        }
        public IEnumerable<SelectListItem> Days
        {
            get
            {
                return DateTimeFormatInfo
                       .InvariantInfo
                       .DayNames
                       .Select((dayName, index) => new SelectListItem
                       {
                           Value = (index + 1).ToString(),
                           Text = dayName
                       });
            }
        }
        public IEnumerable<SelectListItem> Months
        {
            get
            {
                return DateTimeFormatInfo
                       .InvariantInfo
                       .MonthNames
                       .Select((monthName, index) => new SelectListItem
                       {
                           Value = (index + 1).ToString(),
                           Text = monthName
                       });
            }
        }
        public List<CRxWeeks> GetWeeks(int year, int month)
        {
            var lists = new List<CRxWeeks>();
            var count = 1;
            var date = new DateTime(year, month, 1);
            var dates = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n => new DateTime(date.Year, date.Month, n));

            var weekends = from d in dates
                           where d.DayOfWeek == DayOfWeek.Monday
                           select d;
            foreach (var item in weekends)
            {
                var obj = new CRxWeeks { weekStart = item, weekFinish = item.AddDays(6), weekNum = count };
                count++;
                lists.Add(obj);
            }
            return lists;
        }        
        public string ReturnImagePath(int docStatus, bool isDocRequired)
        {
            string imagePath;
            switch (Convert.ToInt32(docStatus))
            {
                case 1: // document Uploaded 
                    imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/doc_required_upload.png";
                    break;
                case 2:  // partial upload document 
                    imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/incomplete_doc_icon.png";
                    break;
                case 0: // doc uploaded but due date expired
                    imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/doc_due.png";
                    break;
                case -1: // doc required but not yet uploaded
                    imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/doc_required.png";
                    break;
                case -3:// document not required
                    imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/doc_not_required.png";
                    if (isDocRequired)
                        imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/doc_required.png";
                    break;
                case 3:// document not required but uploaded
                    imagePath = _appSettings.WebUrlPath + "dist/Images/Icons/doc_not_required_upload.png";
                    break;
                default:
                    imagePath = "";
                    break;
            }
            return imagePath;
        }
        public string UploadDocTypeImagePath(int? uploadDocTypeId, int? docTypeId)
        {
            string imagePath;
            if (docTypeId.HasValue && docTypeId.Value == (int)BDO.Enums.UploadDocTypes.MiscDocuments)
                imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.MiscDocuments.Replace("~", "");
            else
            {
                switch (uploadDocTypeId)
                {
                    case (int)BDO.Enums.UploadDocTypes.Policy: // document Uploaded 
                        imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.PolicyIcon.Replace("~", "");
                        break;
                    case (int)BDO.Enums.UploadDocTypes.Report: // partial upload document 
                        imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.Report.Replace("~", "");
                        break;
                    case (int)BDO.Enums.UploadDocTypes.MiscDocuments: // doc required but not yet uploaded
                        imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.MiscDocuments.Replace("~", "");
                        break;
                    case (int)BDO.Enums.UploadDocTypes.SampleDocument: // document not required
                        imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.SampleDocument.Replace("~", "");
                        break;
                    case (int)BDO.Enums.UploadDocTypes.AssetsReport: // 
                        imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.Report.Replace("~", "");
                        break;
                    default:
                        imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.DocumentIcon.Replace("~", "");
                        break;
                }
            }
            return imagePath;

        }
        public string ReturnImageTooltip(int docStatus, bool isDocRequired)
        {
            string tooltip;
            switch (Convert.ToInt32(docStatus))
            {
                case 1: // document Uploaded 
                    tooltip = "Required document is attached";
                    break;
                case 2:  // partial upload document 
                    tooltip = "Awaiting for more required documents";
                    break;
                case 0: // doc required but not yet uploaded
                    tooltip = "Document required";
                    break;
                case -3:// document not required
                    tooltip = "Document is optional";
                    if (isDocRequired)
                        tooltip = _appSettings.WebUrlPath + "dist/Images/Icons/doc_required.png";
                    break;
                case 3:// document not required but uploaded
                    tooltip = "Document is attached";
                    break;
                default:
                    tooltip = "";
                    break;
            }
            return tooltip;
        }
        public string FilePath(string path)
        {
            var bucketName = Convert.ToString(UserSession.CurrentOrg.ClientNo);
            var absPath = $"{_appSettings.ImageBasePath}{bucketName}/";
            if (!string.IsNullOrEmpty(path))            
                absPath = absPath + path.Replace("~/", "");            

            return absPath.ToLower();
        }
        public string FilePreviewPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {               
                var absPath = _appSettings.ImageBasePath + path.Replace("~/", "");
                return absPath.ToLower();
            }
            else
                return "";
        }
        public string CommonFilePath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var absPath = _appSettings.CommonFileBasePath + path.Replace("~/", "");
                return absPath.ToLower();
            }
            else
                return "";
        }
        public string GetTranStatus(int transStatus)
        {
            string status = string.Empty;
            switch (transStatus)
            {
                case -1:
                    status = "status_not_started";
                    break;
                case 1:
                    status = "status_pass";
                    break;
                case 2:
                    status = "status_inprogress";
                    break;
                case 3:
                    status = "status_pastdue";
                    break;
                case 0:
                    status = "status_pastdue";
                    break;
                case 4:
                    status = "status_grace_period";
                    break;
            }
            return status;
        }
        public string GetTranStatus(string transStatus)
        {
            string status = string.Empty;
            switch (transStatus)
            {
                case "P":
                    status = "status_not_started";
                    break;
                case "I":
                    status = "status_inprogress";
                    break;
                case "C":
                    status = "status_pass";
                    break;
                case "F":
                    status = "status_fail";
                    break;
                case "D":
                    status = "status_pastdue";
                    break;
                case "G":
                    status =
                        UserSession.CurrentUser.IsPowerUser ||
                        UserSession.CurrentUser.UserGroups.Any(x => x.GroupId == 1) ? "status_grace_in_days_sysadmin" : "status_grace_in_days";
                    break;
            }
            return status;
        }
        public string GetInspectionStatus(string transStatus)
        {
            string status = string.Empty;
            switch (transStatus)
            {
                case "NS":
                    status = "status_pastdue";//"status_not_started";
                    break;
                case "NA":
                    status = "status_pass";
                    break;
                case "IN":
                    status = "status_inprogress";
                    break;
                case "PD":
                    status = "status_pastdue";
                    break;
                case "DE":
                    status = "status_pastdue";
                    break;
                case "GP":
                    status = "status_grace_period";
                    break;
                case "RA":
                    status = "status_pastdue";
                    break;
                case "GY":
                    status = "alleps";
                    break;

            }
            return status;
        }
        public string GetAssetsTranStatus(int transStatus, DateTime? dueDate)
        {
            string status = string.Empty;
            if (dueDate.HasValue)
            {
                if (DateTime.Now > dueDate.Value)
                {
                    status = "status_grace_in_days_sysadmin";
                    return status;
                }
            }
            switch (transStatus)
            {
                case -1:
                    status = "assets_status_not_started";
                    break;
                case 1:
                    status = "status_pass";
                    break;
                case 2:
                    status = "status_inprogress";
                    break;
                case 3:
                    status = "status_pastdue";
                    break;
                case 0:
                    status = "status_pastdue";
                    break;
            }
            return status;
        }      
        public string GetEpStatus(int transStatus)
        {
            string status = string.Empty;
            switch (transStatus)
            {
                case 1:
                    status = "Pass";
                    break;
                case 2:
                    status = "In Progress";
                    break;
                case 0:
                    status = "Failed";
                    break;
            }
            return status;
        }      
        public string GetIlsmStatus(int transStatus)
        {
            string status = string.Empty;
            switch (transStatus)
            {
                case 1:
                    status = "Closed";
                    break;
                case 2:
                    status = "In Progress";
                    break;
                case 0:
                    status = "Open";
                    break;
            }
            return status;
        }
        public string GetStatus(int transStatus)
        {
            string status = string.Empty;
            switch (transStatus)
            {
                case 1:
                    status = "Compliant";
                    break;
                case 2:
                    status = "In Progress";
                    break;
                case 0:
                    status = "Non - Compliant";
                    break;
            }
            return status;
        }
        private string GetFloorBuilding(Floor floor)
        {
            return floor != null ? floor.FloorLocation : "NA";
        }
        public string GetFloorAssetLocation(TFloorAssets tfloorAssets)
        {
            return GetFloorBuilding(tfloorAssets.Floor);
        }
        public string GetFloorAssetShortLocation(TFloorAssets tfloorAssets)
        {
            if (tfloorAssets != null && tfloorAssets.Floor != null)
            {
                return tfloorAssets.Floor.ShortFloorLocation;
            }
            else
                return "NA";
        }
        public string GetFloorAssetMake(int? manufactureId)
        {
            var manufactureName = "NA";
            if (manufactureId.HasValue)
            {
                var makeList = _manufactureService.GetAll().Where(x => x.IsActive && x.ManufactureId == manufactureId);
                foreach (var make in makeList)
                    manufactureName = make.ManufactureName;
            }
            return manufactureName;
        }
        public string GetEpStandard(EPDetails epDetails)
        {
            var ePstandard = "";
            if (epDetails.Standard != null)
            {
                ePstandard = $"{epDetails.Standard.TJCStandard},{epDetails.EPNumber}";
            }
            return ePstandard;
        }       
        public string ConvertToBase64(IFormFile image)
        {
            string thePictureDataAsString = string.Empty;
            if (image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    thePictureDataAsString = Convert.ToBase64String(fileBytes);                    
                }               
            }
            return thePictureDataAsString;            
        }
        //public string GetActiveClass(string url)
        //{
        //    var menuName = string.Empty;
        //    string actionName;
        //    if (url.Contains("ChildPage"))
        //    {
        //        var pageId = Convert.ToInt32(url.Split('/')[3]);
        //        menuName = _organizationService.GetUserDashBoard(UserSession.CurrentUser.UserId, 0).Services.FirstOrDefault(x => x.Id == pageId)?.Name.Replace(" ", "").Replace("&", "");
        //    }
        //    else
        //    {
        //        actionName = url.Split('/')[1];
        //        switch (actionName.ToLower())
        //        {
        //            case "activitydashboard":
        //                menuName = "Dashboard";
        //                break;
        //            case "risk_management":
        //                menuName = "RiskManagement";
        //                break;
        //            case "deficiencies":
        //                menuName = "RiskManagement";
        //                break;
        //            case "workorder":
        //                menuName = "WorkOrders";
        //                break;
        //            case "wocreate":
        //                menuName = "WorkOrders";
        //                break;
        //            case "inspection":
        //                menuName = "Inspection";
        //                break;
        //            case "assetview":
        //                menuName = "Inspection";
        //                break;
        //            case "generate_report":
        //                menuName = "Inspection";
        //                break;
        //            case "assets_generate_report":
        //                menuName = "Inspection";
        //                break;
        //            case "firedrill":
        //                menuName = "RoundsDrills";
        //                break;
        //            case "survey":
        //                menuName = "RoundsDrills";
        //                break;
        //            case "rounds":
        //                menuName = "RoundsDrills";
        //                break;
        //            case "reports_compliance":
        //                menuName = "Reports";
        //                break;
        //            case "repors_assets":
        //                menuName = "Reports";
        //                break;
        //            case "reports_risk_management_reports":
        //                menuName = "Reports";
        //                break;
        //            case "reports_documents":
        //                menuName = "Reports";
        //                break;
        //            case "reports_epassignments":
        //                menuName = "Reports";
        //                break;
        //            case "reports":
        //                menuName = "Reports";
        //                break;
        //            case "recentactivity":
        //                menuName = "RecentActivity";
        //                break;
        //            case "docrepo":
        //                menuName = "Binders";
        //                break;
        //            case "getepdocbybinder":
        //                menuName = "Binders";
        //                break;
        //            case "hvacfloorplans":
        //                menuName = "Drawings";
        //                break;
        //            case "pfloorplans":
        //                menuName = "Drawings";
        //                break;
        //            case "mgfloorplans":
        //                menuName = "Drawings";
        //                break;
        //            case "floorplanhistory":
        //                menuName = "Drawings";
        //                break;
        //            case "general":
        //                menuName = "SetUp";
        //                break;
        //            case "vendorconfiguratons":
        //                menuName = "SetUp";
        //                break;
        //            case "permissions":
        //                menuName = "SetUp";
        //                break;
        //            case "epassignments":
        //                menuName = "SetUp";
        //                break;
        //            case "binders":
        //                menuName = "SetUp";
        //                break;
        //            case "floors":
        //                menuName = "SetUp";
        //                break;
        //            case "ilsm":
        //                menuName = "SetUp";
        //                break;
        //            case "ilsmScore":
        //                menuName = "SetUp";
        //                break;
        //            case "tmsrefresh":
        //                menuName = "SetUp";
        //                break;
        //            case "floorplans":
        //                menuName = "Drawings";
        //                break;
        //            case "dashboard": break;
        //            case "myprofile":
        //                menuName = "myProfile";
        //                break;
        //            case "changepassword":
        //                menuName = "myProfile";
        //                break;
        //            case "usermenu":
        //                menuName = "Usermenu";
        //                break;
        //            case "userrole":
        //                menuName = "userrole";
        //                break;
        //            case "epgroups":
        //                menuName = "epgroups";
        //                break;
        //            default:
        //                menuName = "Dashboard";
        //                break;
        //        }
        //    }
        //    return menuName;
        //}                   
        public object GetAssetStatus(TFloorAssets item)
        {
            var activities = item.TInspectionActivity;
            return activities.Count > 0 ? activities.FirstOrDefault()?.SubStatus : "-1";
        }
        public string GetAssetStatus(TInspectionActivity activity)
        {
            string status = string.Empty;
            if (activity != null)
            {
                switch (activity.Status)
                {
                    case 1:
                        status = (activity.Status == 1 && activity.IlsmDate.HasValue) ? $"{"Pass"} ({"ILSM"})" : "Pass";
                        break;
                    case 2:
                        status = "In progress";
                        break;
                    case 0:
                        status = (activity.SubStatus == BDO.Enums.InspectionSubStatus.NA.ToString()) ? $"{"Failed"} ({"Resolved"})"
                            : "Failed";
                        break;
                }
            }
            return status;
        }
        public object GetAssetDue(TFloorAssets item)
        {
            var activities = item.TInspectionActivity;
            if (activities.Count > 0)
            {
                return activities.FirstOrDefault().DueDate.HasValue
                    ? (object)(activities.FirstOrDefault().DueDate.Value - DateTime.UtcNow).TotalDays
                    : "-1";
            }
            else
                return "-1";
        }
        public string GetVersion()
        {
            return _appSettings.BuildVersion;
        }       
        public string ConvertToAMPM(TimeSpan? timespan)
        {
            if (timespan.HasValue)
            {
                var time = DateTime.Today.Add(timespan.Value);
                return time.ToString("hh:mm tt");
            }
            else { return ""; }
        }
        public string ConvertToAMPM(DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToString("hh:mm tt") : "";
        }
        public string ConvertToAMPMClient(DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToClientTime().ToString("hh:mm tt") : string.Empty;
        }   
        public T JsonDeserialize<T>(string result)
        {
            var data = JsonConvert.DeserializeObject<T>(result);
            return data;
        }
        public string JsonSerialize<T>(object data)
        {
            var jsonString = string.Empty;
            try
            {
                var microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                    MaxDepth = Int32.MaxValue

                };

                jsonString = JsonConvert.SerializeObject(data, microsoftDateFormatSettings);
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex, "JsonSerialize", "Common");
            }
            return jsonString;
        }         
        public List<UserProfile> RemoveCRxUsers(List<UserProfile> users)
        {
            return users.Where(x => !x.IsCRxUser).ToList();
        }
      
        //public bool RemoveCRxUsers(UserProfile user)
        //{
        //    if (user.IsPowerUser)
        //    {
        //        var orgNames = _appSettings.ShowCRxUsers.Split(',');
        //        return (orgNames.Contains(UserSession.UserOrg) || UserSession.CurrentUser.IsPowerUser);

        //    }
        //    return true;
        //}

        public string GetCommonMessage()
        {
            return _appSettings.CommonHospitalMessage;
        }
        public bool IsAdminUser()
        {
            return UserSession.CurrentUser.UserGroups.Any(x => x.GroupId == 2);
        }
        public bool IsSystemAdminUser()
        {
            return UserSession.CurrentUser.UserGroups.Any(x => x.GroupId == 1);
        }
        public bool IsFireWatchSupervisorUser()
        {
            return UserSession.CurrentUser.UserGroups.Any(x => x.GroupId == 7);
        }
        public string ReturnDocImageIcon(string extension)
        {
            string imagePath;
            switch (extension.ToLower())
            {
                case ".pdf": // document Uploaded 
                    imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.PDFIcon.Replace("~", "");
                    break;
                case ".doc": // document Uploaded                  
                case ".docx": // document Uploaded                 
                case ".xls": // document Uploaded 
                    imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.DocumentBlueIcon.Replace("~", "");
                    break;
                case ".jpg": // document Uploaded   
                case ".jpeg": // document Uploaded     
                case ".png": // document Uploaded 
                    imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.Imageicon.Replace("~", "");
                    break;
                default:
                    imagePath = _appSettings.WebUrlPath + Models.ImagePathModel.DocumentBlueIcon.Replace("~", "");
                    break;
            }
            return imagePath;
        }       
        public bool IsMenuExist(string menuname, string orgkey)
        {
            bool IsExist = _organizationService.CheckMenuExist(menuname, orgkey);
            return IsExist;
        }  
        public string GetAlphabeticFromIndex(int column)
        {
            column--;
            string col = Convert.ToString((char)('A' + (column % 26)));
            while (column >= 26)
            {
                column = (column / 26) - 1;
                col = Convert.ToString((char)('A' + (column % 26))) + col;
            }
            return col;
        }
        public string setUserName(UserProfile userProfile)
        {            
            if (userProfile != null && userProfile.UserId > 0)
                return userProfile.FullName;
            else
                return "CRx";
        }
        public bool IsChecked(Assets asset, List<Assets> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            return list.Any(x => x.AssetId == asset.AssetId);
        }
        public bool IsVendorsChecked(Vendors item, List<Assets> list)
        {
            return list[0].Vendors.Any(x => x.VendorId == item.VendorId);
        }
        public bool IsCheckedEPGroups(EPGroups epGroup, List<EPGroups> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            return list.Any(x => x.EPGroupId == epGroup.EPGroupId);
        }
        public string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }
        public string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
        public bool IsSelected(StandardEps standardep, List<StandardEps> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            return list.Any(x => x.EPDetailId == standardep.EPDetailId);
        }
    }
}