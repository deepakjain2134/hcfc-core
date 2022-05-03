//using HCF.BDO;
//using HCF.Web.ViewModels;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System;
//using System.Collections.Generic;

//namespace HCF.Web.Base
//{
//    //public interface ICommon
//    {
//        IEnumerable<SelectListItem> Days { get; }
//        IEnumerable<SelectListItem> Months { get; }

//        string CommonFilePath(string path);
//        string ConvertToAMPM(DateTime? dt);
//        string ConvertToAMPM(TimeSpan timespan);
//        string ConvertToAMPM(TimeSpan? timespan);
//        string ConvertToAMPMClient(DateTime? dt);
//        string FilePath(string path);
//        string FilePreviewPath(string path);
//        string GetActiveClass(string url);
//        List<ActivityType> GetActivityType();
//        string GetAlphabeticFromIndex(int column);
//        object GetAssetDue(TFloorAssets item);
//        object GetAssetStatus(TFloorAssets item);
//        string GetAssetStatus(TInspectionActivity activity);
//        string GetAssetsTranStatus(int transStatus, DateTime? dueDate);
//        string GetCommonMessage();
//        string GetEpStandard(EPDetails epDetails);
//        string GetEpStatus(int transStatus);
//        string GetFloorAssetLocation(TFloorAssets tfloorAssets);
//        string GetFloorAssetMake(int? manufactureId);
//        string GetFloorAssetShortLocation(TFloorAssets tfloorAssets);
//        string GetIlsmStatus(int transStatus);
//        string GetInspectionStatus(string transStatus);
//        string GetIssueStatus(int status);
//        int GetQuarter(DateTime? date = null);
//        string GetStatus(int transStatus);
//        string GetTranStatus(int transStatus);
//        string GetTranStatus(string transStatus);
//        string GetUrl(string routeName);
//        string GetVersion();
//        List<CRxWeeks> GetWeeks(int year, int month);
//        bool IsAdminUser();
//        bool IsFireWatchSupervisorUser();
//        bool IsLocalUrl(string url);
//        bool IsMenuExist(string menuname, string orgkey);
//        bool IsSystemAdminUser();
//        T JsonDeserialize<T>(string result);
//        string JsonSerialize<T>(object data);
//        List<UserProfile> RemoveCRxUsers(List<UserProfile> users);
//        bool RemoveCRxUsers(UserProfile user);
//        string ReturnDocImageIcon(string extension);
//        string ReturnImagePath(int docStatus, bool isDocRequired);
//        string ReturnImageTooltip(int docStatus, bool isDocRequired);
//        string setUserName(UserProfile userProfile);
//        string UploadDocTypeImagePath(int? uploadDocTypeId, int? docTypeId);
//    }
//}