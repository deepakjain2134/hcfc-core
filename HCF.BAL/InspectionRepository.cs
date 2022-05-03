//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.Data;

//namespace HCF.BAL
//{
//    public static class InspectionRepository
//    {


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="inspectionId"></param>
//        /// <param name="isComplete"></param>
//        /// <returns></returns>
//        public static bool MarkInsdocStatus(int inspectionId, int isComplete)
//        {
//            return DAL.InspectionRepository.MarkInsdocStatus(inspectionId, isComplete);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static DataTable GetIlsmInspections(Guid? activityId)
//        {
//            return DAL.InspectionRepository.GetIlsmInspections(activityId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<Inspection> GetAllInspections()
//        {
//            return DAL.InspectionRepository.GetCurrentInspections();
//        }

//        public static bool MarkInspectionRa()
//        {
//            return DAL.InspectionRepository.MarkInspectionRa();
//        }


//        public static List<EPsDocument> GetEpsDocument(int epId)
//        {
//            return DAL.InspectionRepository.GetEPsDocument(epId);
//        }


//        #region Inspection Group

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="category"></param>
//        /// <returns></returns>
//        public static int Save(InspectionGroup objInspectionGroup)
//        {
//            return DAL.InspectionRepository.Save(objInspectionGroup);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="category"></param>
//        /// <returns></returns>
//        public static bool UpdateInspectionGroup(InspectionGroup objInspectionGroup)
//        {
//            return DAL.InspectionRepository.UpdateInspectionGroup(objInspectionGroup);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<InspectionGroup> GetInspectionGroup()
//        {
//            return DAL.InspectionRepository.GetInspectionGroup();
//        }

//        #endregion

//        #region Due Date Email

//        public static void SendDueDateEmail()
//        {
//            HCF.BAL.Email.SendDueDateMail();
//        }

//        #endregion

//        #region Inspection calendar 

//        public static List<Inspection> GetInspectionsForCalendar(int userId , DateTime? startDate, DateTime? endDate)
//        {
//            return DAL.InspectionRepository.GetInspectionsForCalendar(userId, startDate, endDate);
//        }


//        public static List<TInspectionActivity> GetInspections(int floorAssetId,int epId)
//        {
//            return DAL.InspectionRepository.GetInspections(floorAssetId,epId);
//        }


//        public static calenderViewDashBoard GetDashboardCalendar(int userId)
//        {
//            return DAL.InspectionRepository.GetDashboardCalendar(userId);
//        }
        

//        #endregion

//        #region Inspection Eps Archived

//        public static List<EPDetails> GetArchivedEPsInspection(int userId)
//        {
//            return DAL.InspectionRepository.GetArchivedEPsInspection(userId);

//        }

//        #endregion
//    }
//}
