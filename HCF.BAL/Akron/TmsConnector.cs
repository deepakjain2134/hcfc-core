//using System;
//using System.Collections.Generic;
//using HCF.BDO.MaintenanceConnection;
//using HCF.DAL;
//using HCF.BDO;

//namespace HCF.BAL.Akron
//{
//    public class TmsConnector
//    {
//        public static int SaveTMSResults(TMS_Results data)
//        {
//            return DAL.Akron.TmsConnector.AddTMSRecords(data);

//        }

//        public static List<TMS_Results> GetAllTMSRecords()
//        {
//            return DAL.Akron.TmsConnector.GetAllTMSRecords();
//        }

//        public static int SaveAssets(TFloorAssets data)
//        {
//            return DAL.Akron.TmsConnector.SaveAssets(data);
//        }

//        public static List<TFloorAssets> GetAllTMSAsset()
//        {
//            return DAL.Akron.TmsConnector.GetAllTMSAsset();
//        }

//        #region WorkOrder


//        public static int SaveTMSWorkOrderToLocal(TMS_WorkOrderRes data)
//        {
//            return DAL.Akron.TmsConnector.SaveTMSWorkOrderToLocal(data);

//        }

//        public static int SaveWorkOrderAssignmentToLocal(TMS_WorkOrderAssignment data)
//        {
//            return DAL.Akron.TmsConnector.SaveWorkOrderAssignmentToLocal(data);
//        }

//        public static int SaveWorkOrderTasksToLocal(TMS_WorkOrderTasks data)
//        {
//            return DAL.Akron.TmsConnector.SaveWorkOrderTasksToLocal(data);

//        }

//        #endregion


//        #region Users/Labors

//        public static int SaveTMSUsersToLocal(TMS_UserRes data)
//        {
//            return DAL.Akron.TmsConnector.SaveTMSUsersToLocal(data);

//        }

//        public static List<UserProfile> GetAllTMSUsers()
//        {
//            return DAL.Akron.TmsConnector.GetAllTMSUsers();
//        }


//        #endregion

//        #region Lookup Table & Values 

//        public static int SavelookupTableToLocal(TMS_LookupTable objlookuptable)
//        {
//            return DAL.Akron.TmsConnector.SavelookupTableToLocal(objlookuptable);
//        }

//        public static int SavelookupTableValuesToLocal(TMS_LookupTableValues objlookuptablevalues)
//        {
//            return DAL.Akron.TmsConnector.SavelookupTableValuesToLocal(objlookuptablevalues);
//        }

//        #endregion

//    }
//}
