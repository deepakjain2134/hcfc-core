using System;
using System.Collections.Generic;
using HCF.BDO.MaintenanceConnection;
using HCF.DAL;
using HCF.BDO;
using HCF.BDO.NBIH;

namespace HCF.BAL
{
    public class TmsConnectorService : ITmsConnectorService
    {
        private readonly ITmsConnectorRepsitory _tmsConnectorRepsitory;
        public TmsConnectorService(ITmsConnectorRepsitory tmsConnectorRepsitory)
        {
            _tmsConnectorRepsitory = tmsConnectorRepsitory;            
        }
        public int SaveTMSResults(TMS_Results data)
        {
            return _tmsConnectorRepsitory.AddTMSRecords(data);

        }

        public List<TMS_Results> GetAllTMSRecords()
        {
            return _tmsConnectorRepsitory.GetAllTMSRecords();
        }

        public int SaveAssets(TFloorAssets data)
        {
            return _tmsConnectorRepsitory.SaveAssets(data);
        }

        public List<TFloorAssets> GetAllTMSAsset()
        {
            return _tmsConnectorRepsitory.GetAllTMSAsset();
        }

        #region WorkOrder


        public int SaveTMSWorkOrderToLocal(TMS_WorkOrderRes data)
        {
            return _tmsConnectorRepsitory.SaveTMSWorkOrderToLocal(data);

        }

        public int SaveWorkOrderAssignmentToLocal(TMS_WorkOrderAssignment data)
        {
            return _tmsConnectorRepsitory.SaveWorkOrderAssignmentToLocal(data);
        }

        public int SaveWorkOrderTasksToLocal(TMS_WorkOrderTasks data)
        {
            return _tmsConnectorRepsitory.SaveWorkOrderTasksToLocal(data);

        }

        #endregion


        #region Users/Labors

        public int SaveTMSUsersToLocal(TMS_UserRes data)
        {
            return _tmsConnectorRepsitory.SaveTMSUsersToLocal(data);

        }

        public List<UserProfile> GetAllTMSUsers()
        {
            return _tmsConnectorRepsitory.GetAllTMSUsers();
        }


        #endregion

        #region Lookup Table & Values 

        public int SavelookupTableToLocal(TMS_LookupTable objlookuptable)
        {
            return _tmsConnectorRepsitory.SavelookupTableToLocal(objlookuptable);
        }

        public int SavelookupTableValuesToLocal(TMS_LookupTableValues objlookuptablevalues)
        {
            return _tmsConnectorRepsitory.SavelookupTableValuesToLocal(objlookuptablevalues);
        }

        #endregion


        #region NBIH

        public int SaveAssetsTempTable(TMSNBIAssetsResults data)
        {
            return _tmsConnectorRepsitory.SaveAssetsTempTable(data);
        }
        public int SaveBuildingsTempTable(TMSNBIBuildingsResults data)
        {
            return _tmsConnectorRepsitory.SaveBuildingsTempTable(data);
        }
        public int SavePriorityTempTable(TMSNBIPriorityResults data)
        {
            return _tmsConnectorRepsitory.SavePriorityTempTable(data);
        }
        public int SaveAccountTempTable(TMSNBIAccountResults data)
        {
            return _tmsConnectorRepsitory.SaveAccountTempTable(data);
        }
        public int SaveSitesTempTable(TMSNBISitesResults data)
        {
            return _tmsConnectorRepsitory.SaveSitesTempTable(data);
        }
        public int SaveTypeTempTable(TMSNBITypeResults data)
        {
            return _tmsConnectorRepsitory.SaveTypeTempTable(data);
        }
        public int SaveStatusTempTable(TMSNBIStatusResults data)
        {
            return _tmsConnectorRepsitory.SaveStatusTempTable(data);
        }
        public int SaveSubStatusTempTable(TMSNBISubStatusResults data)
        {
            return _tmsConnectorRepsitory.SaveSubStatusTempTable(data);
        }
        public int SaveLocationTempTable(TMSNBISLocationResults data)
        {
            return _tmsConnectorRepsitory.SaveLocationTempTable(data);
        }
        public int SaveRequesterTempTable(TMSNBISRequesterResults data)
        {
            return _tmsConnectorRepsitory.SaveRequesterTempTable(data);
        }
        public int SaveResourcesTempTable(TMSNBIResourceResults data)
        {
            return _tmsConnectorRepsitory.SaveResourcesTempTable(data);
        }

        public int SaveWorkOrderTempTable(TMSNBIWorkOrderResults data)
        {
            return _tmsConnectorRepsitory.SaveWorkOrderTempTable(data);
        }

        #endregion         

    }
}
