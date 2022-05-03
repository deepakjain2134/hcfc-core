using HCF.BDO;
using HCF.BDO.MaintenanceConnection;
using HCF.BDO.NBIH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public interface ITmsConnectorService
    {

        int SaveTMSResults(TMS_Results data);
        List<TMS_Results> GetAllTMSRecords();
        int SaveAssets(TFloorAssets data);
        List<TFloorAssets> GetAllTMSAsset();

        #region WorkOrder

        int SaveTMSWorkOrderToLocal(TMS_WorkOrderRes data);
        int SaveWorkOrderAssignmentToLocal(TMS_WorkOrderAssignment data);
        int SaveWorkOrderTasksToLocal(TMS_WorkOrderTasks data);


        #endregion

        #region Users/Labors

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
        int SaveStatusTempTable(TMSNBIStatusResults data);
        int SaveAccountTempTable(TMSNBIAccountResults data);
        int SaveSitesTempTable(TMSNBISitesResults data);
        int SaveTypeTempTable(TMSNBITypeResults data);
        int SaveSubStatusTempTable(TMSNBISubStatusResults data);
        int SaveLocationTempTable(TMSNBISLocationResults data);
        int SaveRequesterTempTable(TMSNBISRequesterResults data);
        int SaveResourcesTempTable(TMSNBIResourceResults data);
        int SaveWorkOrderTempTable(TMSNBIWorkOrderResults data);

        #endregion
    }
}
