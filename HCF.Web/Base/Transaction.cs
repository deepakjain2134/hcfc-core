//using HCF.BDO;
//using HCF.Utility;
//using System.Collections.Generic;
//using System.Linq;

//namespace HCF.Web.Base
//{
//    public static class Transaction
//    {
//        //public static int GetFAssetActivitycount(EPDetails ePDetails, int floorAssetId)
//        //{
//        //    var activityCount = 0;
//        //    if (ePDetails != null)
//        //    {
//        //        activityCount += ePDetails.Inspections.Sum(item => item.TInspectionActivity.Where(x => x.FloorAssetId == floorAssetId).ToList().Count);
//        //    }
//        //    return activityCount;
//        //}

//        //public static int GetEPActivitycount(EPDetails ePDetails)
//        //{
//        //    var activityCount = 0;
//        //    if (ePDetails != null)
//        //    {
//        //        foreach (var item in ePDetails.Inspections)
//        //        {

//        //            activityCount++;

//        //        }
//        //    }
//        //    return activityCount;
//        //}

//        public static string GetFloorAssetLastInspectionDate(List<Inspection> inspectionList, int floorAssetId, int epId)
//        {
//            if (inspectionList == null) return "";
//            var inpection = inspectionList.FirstOrDefault(x => x.EPDetailId == epId && x.IsCurrent);
//            if (inpection != null)
//            {
//                var inspectionActivity = inpection.TInspectionActivity?.FirstOrDefault(x => x.FloorAssetId == floorAssetId && x.IsCurrent);
//                if (inspectionActivity != null)
//                    if (inspectionActivity.ActivityInspectionDate != null)
//                        return inspectionActivity.ActivityInspectionDate.Value.ToClientTime().ToFormatDateTime();
//            }
//            return "";
//        }

//        public static string GetFloorAssetNextInspectionDate(List<Inspection> inspectionList, int floorAssetId, int epId)
//        {
//            if (inspectionList != null)
//            {
//                var inpection = inspectionList.FirstOrDefault(x => x.EPDetailId == epId && x.IsCurrent);
//                if (inpection == null) return "";
//                {
//                    if (inpection.TInspectionActivity != null)
//                    {
//                        var inspectionactivity = inpection.TInspectionActivity.FirstOrDefault(x => x.FloorAssetId == floorAssetId && x.IsCurrent);
//                        if (inspectionactivity != null)
//                            if (inspectionactivity.DueDate.HasValue)
//                                return inspectionactivity.DueDate.Value.ToClientTime().ToFormatDate();
//                    }
//                }
//            }
//            return "";
//        }

//    }
//}