
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HCF.BDO;
using HCF.Utility;

namespace HCF.DAL
{
    public class GoalMaster : IGoalMaster
    {
        private readonly ISqlHelper _sqlHelper;
        public GoalMaster(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public int UpdateEpFrequency(EPFrequency newdata)
        {
            int newId;
            const string sql = StoredProcedures.Update_EPFrequency;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", newdata.EpDetailId);
                command.Parameters.AddWithValue("@CreatedBy", newdata.CreatedBy);
                command.Parameters.AddWithValue("@FrequencyId", newdata.FrequencyId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int AddEpFrequency(EPFrequency newdata)
        {
            int newId;
            const string sql = StoredProcedures.Insert_EPFrequency;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", newdata.EpDetailId);
                command.Parameters.AddWithValue("@CreatedBy", newdata.CreatedBy);
                command.Parameters.AddWithValue("@FrequencyId", newdata.FrequencyId);
                command.Parameters.AddWithValue("@TjcFrequencyId", newdata.TjcFrequencyId);
                command.Parameters.AddWithValue("@RecFrequencyId", newdata.RecFrequencyId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int GetAssignedEpCount(string users)
        {
            int count;
            const string sql = StoredProcedures.GET_AssignedEPCount;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Users", users);
                command.Parameters.Add("@count", SqlDbType.Int);
                command.Parameters["@count"].Direction = ParameterDirection.Output;
                count = _sqlHelper.ExecuteNonQuery(command, "@count");
            }
            return count;
        }




        public List<AffectedEPs> GetAffectedEPs(int epDetailId)
        {
            List<AffectedEPs> affectedEPs;
            const string sql = StoredProcedures.Get_AffectedEPs; //"dbo.Get_AffectedEPs";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epDetailId", epDetailId);
                var dt = _sqlHelper.GetDataTable(command);
                affectedEPs = _sqlHelper.ConvertDataTable<AffectedEPs>(dt);
            }
            return affectedEPs;
        }

        public List<AffectedEPs> GetAffectedEPs()
        {
            List<AffectedEPs> affectedEPs;
            const string sql = StoredProcedures.Get_AffectedEPs; //"dbo.Get_AffectedEPs";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                affectedEPs = _sqlHelper.ConvertDataTable<AffectedEPs>(dt);
            }
            return affectedEPs;
        }

        public List<EPSteps> GetEpTransInfo(int epdetailId, int inspectionId, int? inspectiongroupId)
        {
            List<EPSteps> ePsteps = new List<EPSteps>();
            const string sql = StoredProcedures.Get_EPTransInfo;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetailId", epdetailId);
                command.Parameters.AddWithValue("@inspectionId", inspectionId);
                command.Parameters.AddWithValue("@inspectiongroupId", inspectiongroupId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    ePsteps = _sqlHelper.ConvertDataTable<EPSteps>(ds.Tables[0]);
                    if (ds.Tables[1] != null)
                    {
                        List<Inspection> isnpections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[1]);
                        // List<EPDetails> epDetails = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[2]);
                        foreach (var epstap in ePsteps)
                        {
                            var ins = isnpections.FirstOrDefault(x => x.InspectionId == epstap.InspectionId);
                            epstap.Inspection = ins ?? new Inspection();
                            //if (epDetails != null && epDetails.Count > 0)
                            //    epstap.EPDetails = epDetails.FirstOrDefault();
                        }


                    }
                }

            }
            return ePsteps;
        }


        #region EP Users





        public IEnumerable<EPAssignee> GetEPUsers()
        {
            var epAssignees = new List<EPAssignee>();
            const string sql = StoredProcedures.Get_EPAssignee;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    epAssignees = _sqlHelper.ConvertDataTable<EPAssignee>(ds.Tables[0]);
                    var userProfile = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[1]);
                    foreach (var epAssignee in epAssignees)
                    {
                        epAssignee.UserProfile = userProfile.FirstOrDefault(x => x.UserId == epAssignee.UserId);
                    }
                }
            }
            return epAssignees;
        }

        public List<int> GetUsersEps(int[] userIds)
        {
            var epAssignees = new List<EPAssignee>();
            const string sql = StoredProcedures.Get_EPAssignee;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    epAssignees = _sqlHelper.ConvertDataTable<EPAssignee>(ds.Tables[0]);
                }
            }
            return epAssignees.Where(x => userIds.Contains(x.UserId)).Select(x => x.EPDetailId).ToList();
        }

        public void AssignEpResposibility(string fromUsers, string toUsers, int createdBy)
        {
            const string sql = StoredProcedures.Assign_EP_Resposibility;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@fromUsers", fromUsers);
                command.Parameters.AddWithValue("@toUsers", toUsers);
                command.Parameters.AddWithValue("@createdBy", createdBy);
                _sqlHelper.ExecuteNonQuery(command);
            }
        }

        public int SaveUserEPs(int userId, int epdetailId, string epdetailIds, int createdBy, string type, bool status)
        {
            int newId;
            const string sql = StoredProcedures.Insert_EPUsers;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId ", userId);
                command.Parameters.AddWithValue("@EpDetailId ", epdetailId);
                command.Parameters.AddWithValue("@EpDetailIds ", epdetailIds);
                command.Parameters.AddWithValue("@CreatedBy ", createdBy);
                command.Parameters.AddWithValue("@status ", status);
                command.Parameters.AddWithValue("@Type ", type);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int SaveEpAssignee(EPAssignee ePAssignee)
        {
            int newId = 1;
            const string sql = StoredProcedures.Save_EPUsers;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserIds", (ePAssignee.UserIds.Count > 0) ? string.Join(",", ePAssignee.UserIds) : "");
                command.Parameters.AddWithValue("@userId", ePAssignee.UserId);
                command.Parameters.AddWithValue("@epsId", ePAssignee.EpDetailIDs);
                command.Parameters.AddWithValue("@epId", ePAssignee.EPDetailId);
                command.Parameters.AddWithValue("@Status", ePAssignee.Status);
                command.Parameters.AddWithValue("@CreatedBy", ePAssignee.CreatedBy);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return newId;
        }

        public bool AddEpAssignees(EPAssignee newEpAssigne, string catId)
        {
            bool status;
            const string sql = StoredProcedures.Insert_EPAssignees;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", newEpAssigne.UserId);
                command.Parameters.AddWithValue("@EpDetailIds", newEpAssigne.EpDetailIDs);
                command.Parameters.AddWithValue("@EpDetailId", newEpAssigne.EPDetailId);
                command.Parameters.AddWithValue("@UserIds", (newEpAssigne.UserIds != null && newEpAssigne.UserIds.Count > 0) ? string.Join(",", newEpAssigne.UserIds) : "");
                command.Parameters.AddWithValue("@CreatedBy", newEpAssigne.CreatedBy);
                command.Parameters.AddWithValue("@Type", catId);
                command.Parameters.AddWithValue("@Status", newEpAssigne.Status);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public bool Add(EPAssignee newEpAssigne)
        {
            bool status;
            const string sql = StoredProcedures.Crud_EPAssignees;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AssigneeId", newEpAssigne.AssigneeId);
                command.Parameters.AddWithValue("@UserId", newEpAssigne.UserId);
                command.Parameters.AddWithValue("@EpDetailId", newEpAssigne.EPDetailId);
                command.Parameters.AddWithValue("@IsCurrent", newEpAssigne.IsCurrent);
                command.Parameters.AddWithValue("@Type", newEpAssigne.Type);
                command.Parameters.AddWithValue("@Status", newEpAssigne.Status);
                command.Parameters.AddWithValue("@CreatedBy", newEpAssigne.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }


        public bool Update(EPAssignee newEpAssigne)
        {
            bool status;
            const string sql = StoredProcedures.Crud_EPAssignees;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AssigneeId", newEpAssigne.AssigneeId);
                command.Parameters.AddWithValue("@UserId", newEpAssigne.UserId);
                command.Parameters.AddWithValue("@EpDetailId", newEpAssigne.EPDetailId);
                command.Parameters.AddWithValue("@IsCurrent", newEpAssigne.IsCurrent);
                command.Parameters.AddWithValue("@Type", newEpAssigne.Type);
                command.Parameters.AddWithValue("@Status", newEpAssigne.Status);
                command.Parameters.AddWithValue("@CreatedBy", newEpAssigne.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        #endregion

        public List<ActivityType> GetActivityType()
        {
            List<ActivityType> activityType = new List<ActivityType>();
            ActivityType activity1 = new ActivityType
            {
                Name = "EP",
                ActivityTypeId = 1
            };
            ActivityType activity2 = new ActivityType
            {
                Name = "Assets",
                ActivityTypeId = 2
            };
            ActivityType activity3 = new ActivityType
            {
                Name = "Doc",
                ActivityTypeId = 3
            };
            activityType.Add(activity1);
            activityType.Add(activity2);
            activityType.Add(activity3);
            return activityType;
        }

        public RiskManagement GetRiskCount(int userId)
        {
            var list = new RiskManagement();
            const string sql = StoredProcedures.GET_RiskCount;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    list = _sqlHelper.ConvertDataTable<RiskManagement>(ds.Tables[0]).FirstOrDefault();
                    list.StandardEPs = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[1]);
                }
            }
            return list;
        }


        #region CMS
        public int SaveCMSdata(CmsEpMapping cmsEpMapping)
        {
            int newId;
            const string sql = StoredProcedures.Insert_CmsEpMapping;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", cmsEpMapping.EPDetailId);
                command.Parameters.AddWithValue("@CopDetailsId", cmsEpMapping.CopDetailsId);
                command.Parameters.AddWithValue("@CreatedBy", cmsEpMapping.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }



        public bool Delete_CmsEpMapping(int? standardEps, int? cmstext)
        {
            bool status;
            const string sql = Utility.StoredProcedures.Delete_CmsEpMapping;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", standardEps);
                command.Parameters.AddWithValue("@CopDetailsId", cmstext);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public EPDetails EpRelationStatus(int Epdetailid)
        {
            var list = new EPDetails();
            const string sql = StoredProcedures.GET_EpRelationStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Epdetailid", Epdetailid);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {

                    list.EPDetailId = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                    list.PendingRelationEpCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                }
            }
            return list;
        }

        public EPDetails EpSearchbyEpNumber(string epSearchText, int currentUserId, int userId)
        {
            var list = new EPDetails();
            const string sql = StoredProcedures.GET_EpSearchbyEpNumber;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@currentUserId", currentUserId);
                command.Parameters.AddWithValue("@searchBy", epSearchText);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        list.EPDetailId = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                    }
                    else
                    {
                        list.EPDetailId = Convert.ToInt32(0);
                    }

                }
            }
            return list;
        }


        #endregion
    }
}
