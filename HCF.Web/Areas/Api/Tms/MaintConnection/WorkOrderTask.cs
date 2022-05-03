using HCF.BDO;
using HCF.BDO.MaintenanceConnection;
using Hcf.Api.Tms.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
//using System.Web.Script.Serialization;

namespace Hcf.Api.Tms.MaintConnection
{
    public class WorkOrderTask : AssetsFactory
    {
        private const string _workOrderTasks = "https://api.maintenanceconnection.com/v8/WorkOrderTasks";
        private const string _workOrderTasksByWo = "https://api.maintenanceconnection.com/v8/WorkOrders/{0}/Tasks";
        private const string _mcCnKey = "AKRONCH";
        private const string _mcApiKey = "fc5ff399-cb8d-4bdd-b4a2-19361678b072";
        public override List<TFloorAssets> GetAssetsByWorkOrder(int workOrderId)
        {
            List<TFloorAssets> objTFloorAssets = new List<TFloorAssets>();
            var tmsWorkOrderTasks = new List<TMS_WorkOrderTasks>();
            try
            {
                var getWorkorderUsersUrl = string.Format(_workOrderTasksByWo, workOrderId);
                var responseJson = HCF.Utility.CommonUtility.MaintConnectionGet(getWorkorderUsersUrl, 0, _mcCnKey, _mcApiKey);
                //var jsonSerializer = new JavaScriptSerializer();
                var response = JsonConvert.DeserializeObject<RootObject<TMS_WorkOrderTasks>>(responseJson);
                objTFloorAssets = ConvertToLocalWOAssets(response.Results);
            }
            catch (Exception ex)
            {
                //HCF.Utility.ErrorLog.LogError(ex);
            }
            return objTFloorAssets;            
        }

        private List<TFloorAssets> ConvertToLocalWOAssets(List<TMS_WorkOrderTasks> results)
        {
            return results.Select(wotask => new TFloorAssets
            {
                TmsReference = wotask.AssetPK.ToString()              
            }).ToList();
        }
    }
}