using HCF.BDO;
using HCF.BDO.MaintenanceConnection;
using HCF.BDO.NBIH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public interface ITmsConnectorRepsitory
    {
        int AddTMSRecords(TMS_Results newData);
        List<TMS_Results> GetAllTMSRecords();
        List<TFloorAssets> GetAllTMSAsset();
        int SaveAssets(TFloorAssets newfloorAssets);

        #region WorkOrder

        int SaveTMSWorkOrderToLocal(TMS_WorkOrderRes data);
        int SaveWorkOrderAssignmentToLocal(TMS_WorkOrderAssignment data);
        int SaveWorkOrderTasksToLocal(TMS_WorkOrderTasks data);

        #endregion

        #region Users

        int SaveTMSUsersToLocal(TMS_UserRes data);
        List<UserProfile> GetAllTMSUsers();

        #endregion

        #region Lookup Table & Values 

        int SavelookupTableToLocal(TMS_LookupTable objlookuptable);
        int SavelookupTableValuesToLocal(TMS_LookupTableValues objlookuptablevalues);


        #endregion


        #region NBIH

        int SaveAssetsTempTable(TMSNBIAssetsResults data);
        int SaveBuildingsTempTable(TMSNBIBuildingsResults data);
        int SavePriorityTempTable(TMSNBIPriorityResults data);
        int SaveAccountTempTable(TMSNBIAccountResults data);
        int SaveSitesTempTable(TMSNBISitesResults data);
        int SaveTypeTempTable(TMSNBITypeResults data);
        int SaveStatusTempTable(TMSNBIStatusResults newData);
        int SaveSubStatusTempTable(TMSNBISubStatusResults newData);
        int SaveLocationTempTable(TMSNBISLocationResults newData);
        int SaveRequesterTempTable(TMSNBISRequesterResults newData);
        int SaveResourcesTempTable(TMSNBIResourceResults data);
        int SaveWorkOrderTempTable(TMSNBIWorkOrderResults newData);

        #endregion
    }
}
