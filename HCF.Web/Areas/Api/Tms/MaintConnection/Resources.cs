using Hcf.Api.Tms.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web.Script.Serialization;
using HCF.BDO;
using HCF.BDO.MaintenanceConnection;
using Newtonsoft.Json;

namespace Hcf.Api.Tms.MaintConnection
{
    public class Resources : ResourceFactory
    {
        private static readonly string _getUsers = "https://api.maintenanceconnection.com/v8/Labors";
        private static readonly string _getWorkorderUsers = "https://api.maintenanceconnection.com/v8/WorkOrders/{0}/Assignments";
        private static string _postWorkorderUsers= "https://api.maintenanceconnection.com/v8/WorkOrders/{0}/Assignments";
        private const string _mcCnKey = "AKRONCH";
        private const string _mcApiKey = "fc5ff399-cb8d-4bdd-b4a2-19361678b072";
        public override List<UserProfile> GetResource()
        {
            var userList = new List<UserProfile>();
            var tmsUserRes = new List<TMS_UserRes>();
            try
            {
                var responseJson = HCF.Utility.CommonUtility.MaintConnectionGet(_getUsers, 0, _mcCnKey, _mcApiKey);
                //var jsonSerializer = new JavaScriptSerializer();
                var response = JsonConvert.DeserializeObject<RootObject<TMS_UserRes>>(responseJson);
                userList = ConvertToLocalUsers(response.Results);
            }
            catch (Exception ex)
            {
                //HCF.Utility.ErrorLog.LogError(ex);
            }
            return userList;
        }

        public override List<UserProfile> GetResourceByWorkOrder(int workOrderId)
        {
            var userList = new List<UserProfile>();
            var tmsWorkOrderAssignment = new List<TMS_WorkOrderAssignment>();
            try
            {
                var getWorkorderUsersUrl = string.Format(_getWorkorderUsers, workOrderId);
                var responseJson = HCF.Utility.CommonUtility.MaintConnectionGet(getWorkorderUsersUrl, 0, _mcCnKey, _mcApiKey);
                //var jsonSerializer = new JavaScriptSerializer();
                var response = JsonConvert.DeserializeObject<RootObject<TMS_WorkOrderAssignment>>(responseJson);
                userList = ConvertToLocalWOAssignments(response.Results);
            }
            catch (Exception ex)
            {
                //HCF.Utility.ErrorLog.LogError(ex);
            }
            return userList;
        }

        public override UserProfile SaveWoAssignment(UserProfile woAssignments, int WorkorderPk)
        {
            TMS_WorkOrderAssignment tmsWorkOrderAssignment = new TMS_WorkOrderAssignment();
            UserProfile objuserprofile = new UserProfile();
            tmsWorkOrderAssignment = ConvertToTmsWOAssignments(woAssignments);
            var postDataString = string.Empty;
            try
            {
                _postWorkorderUsers = string.Format(_postWorkorderUsers, WorkorderPk);
                postDataString = JsonConvert.SerializeObject(tmsWorkOrderAssignment);
                var responseJson = HCF.Utility.CommonUtility.MaintConnectionPost(_postWorkorderUsers, _mcCnKey, _mcApiKey, postDataString);
                //var jsonSerializer = new JavaScriptSerializer();
                var response = JsonConvert.DeserializeObject<RootObject<TMS_WorkOrderAssignment>>(responseJson);
                objuserprofile = ConvertToLocalWOAssignments(response.Results).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return objuserprofile;
        }


        #region Private Methods

        private static List<UserProfile> ConvertToLocalUsers(IEnumerable<TMS_UserRes> tmsUsers)
        {
            return tmsUsers.Select(user => new UserProfile
            {
                ResourceId = user.PK,
                ResourceNumber = user.ID,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }).ToList();
        }

        private static TMS_WorkOrderAssignment ConvertToTmsWOAssignments(UserProfile WoAssigments)
        {
            var tmsWorkOrderAssignment = new TMS_WorkOrderAssignment
            {
                WOlaborPK = WoAssigments.ResourceId,
                AssignedDate = DateTime.UtcNow,
                IsAssigned = true,
            };
            return tmsWorkOrderAssignment;
        }

        private static List<UserProfile> ConvertToLocalWOAssignments(IEnumerable<TMS_WorkOrderAssignment> tmsWorkOrderAssignment)
        {
            return tmsWorkOrderAssignment.Select(user => new UserProfile
            {
                ResourceId = user.LaborPK,
                ResourceNumber = user.LaborID
            }).ToList();
        }

        #endregion
    }
}