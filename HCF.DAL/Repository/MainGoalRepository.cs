using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public  class MainGoalRepository : IMainGoalRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public MainGoalRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maingoal"></param>
        /// <returns></returns>
        public  int Save(MainGoal maingoal)
        {
            int newId;
            const string sql = StoredProcedures.Insert_MainGoal;        
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", maingoal.EPDetailId > 0 ? maingoal.EPDetailId : (object)null);
                command.Parameters.AddWithValue("@Goal", maingoal.Goal);
                command.Parameters.AddWithValue("@AssetId", maingoal.AssetId > 0 ? maingoal.AssetId : (object)null);
                command.Parameters.AddWithValue("@DoctypeId", maingoal.DoctypeId > 0 ? maingoal.DoctypeId : (object)null);
                command.Parameters.AddWithValue("@CreatedBy", maingoal.CreatedBy);
                command.Parameters.AddWithValue("@ActivityType", maingoal.ActivityType);
                command.Parameters.AddWithValue("@frequencyId", maingoal.FrequencyId > 0 ? maingoal.FrequencyId : (object)null);
                command.Parameters.AddWithValue("@FloorAssetId", maingoal.FloorAssetId > 0 ? maingoal.FloorAssetId : (object)null);
                command.Parameters.AddWithValue("@ClientNo", maingoal.ClientNo > 0 ? maingoal.ClientNo : (object)null);
                command.Parameters.AddWithValue("@IsActive", maingoal.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maingoal"></param>
        /// <returns></returns>
        public  int Update(MainGoal maingoal)
        {
            int newId;
            const string sql = StoredProcedures.Update_MainGoal;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MainGoalId", maingoal.MainGoalId);
                command.Parameters.AddWithValue("@EPDetailId", maingoal.EPDetailId);
                command.Parameters.AddWithValue("@Goal", maingoal.Goal);
                command.Parameters.AddWithValue("@AssetId", maingoal.AssetId);
                command.Parameters.AddWithValue("@DoctypeId", maingoal.DoctypeId);
                command.Parameters.AddWithValue("@IsActive", maingoal.IsActive);
                command.Parameters.AddWithValue("@ActivityType", maingoal.ActivityType);
                command.Parameters.AddWithValue("@Type", maingoal.Type);               
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <param name="activityType"></param>
        /// <param name="isActiveSteps"></param>
        /// <returns></returns>
        private  IEnumerable<MainGoal> GetMainGoals(int? epDetailId = null, int? activityType = null, bool isActiveSteps = true)
        {
            var lists = new List<MainGoal>();
            const string sql = StoredProcedures.Get_MainGoal;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", epDetailId);
                command.Parameters.AddWithValue("@activityType", activityType);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<MainGoal>(ds.Tables[0]);
                    var steps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[1]);
                    foreach (var item in lists)
                    {
                        item.Steps = steps.Where(m => m.MainGoalId == item.MainGoalId).ToList();
                        if (isActiveSteps)
                            item.Steps = item.Steps.Where(x => x.IsActive).ToList();
                    }
                }
            }
            return lists;
        }

        public  MainGoal GetMainGoalById(int maingoalId)
        {
            var lists = new List<MainGoal>();
            const string sql = StoredProcedures.Get_GetMainGoalById;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@maingoalId", maingoalId);               
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<MainGoal>(ds.Tables[0]);
                    var steps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[1]);
                    foreach (var item in lists)
                    {
                        item.Steps = steps.Where(m => m.MainGoalId == item.MainGoalId).ToList();                     
                           
                    }
                }
            }
            return lists.FirstOrDefault();
        }       

        public  List<MainGoal> GetMainGoal(int? epDetailId = null, int? activityType = null)
        {
            return GetMainGoals(epDetailId, activityType).Where(x => x.IsActive).ToList();
        }


        public  List<MainGoal> GetAllMainGoal(int? epDetailId = null, int? activityType = null)
        {
            return GetMainGoals(epDetailId, activityType, false).ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public  List<MainGoal> GetAssetMainGoals(int assetId,int? epdetailId)
        {
            var lists = new List<MainGoal>();
            const string sql = StoredProcedures.Get_AssetsMainGoals;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId);
                command.Parameters.AddWithValue("@epdetailId", epdetailId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<MainGoal>(ds.Tables[0]);
                    var steps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[1]);
                    var stepsmapping = _sqlHelper.ConvertDataTable<IlsmStepsMapping>(ds.Tables[2]);
                    foreach (var item in lists)
                    {
                        item.Steps = new List<Steps>();
                        item.Steps = steps.Where(m => m.MainGoalId == item.MainGoalId).ToList();
                        foreach (var mapping in item.Steps.Where(x => x.IsIlsmLink))
                        {
                            mapping.IlsmStepsMapping = stepsmapping.Where(x => x.StepsId == mapping.StepsId && x.IsActive).ToList();
                        }
                    }
                }
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityType"></param>
        /// <param name="epDetailId"></param>
        /// <param name="floorAssetId"></param>
        /// <param name="frequencyId"></param>
        /// <returns></returns>
        public  List<MainGoal> GetMainGoalByActivity(int activityType, int epDetailId, int? floorAssetId, int frequencyId , int? ClientNo)
        {
            var mainGoals = new List<MainGoal>();
            const string sql = StoredProcedures.Get_MainGoalByActivity;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Mode", activityType);
                command.Parameters.AddWithValue("@FloorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@EPDetailId", epDetailId);
                command.Parameters.AddWithValue("@FrequencyId", frequencyId > 0 ? frequencyId : (object)null);
                command.Parameters.AddWithValue("@ClientNo", ClientNo > 0 ? ClientNo : (object)null);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    mainGoals = _sqlHelper.ConvertDataTable<MainGoal>(ds.Tables[0]);
                    var steps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[1]);
                    var inspectionSteps = _sqlHelper.ConvertDataTable<InspectionSteps>(ds.Tables[2]);

                    if (inspectionSteps.Count > 0)
                    {
                        foreach (var item in steps)
                        {
                            item.inspectionsteps = inspectionSteps.Where(x => x.StepsId == item.StepsId).ToList();
                        }
                    }

                    foreach (var item in mainGoals)
                        item.Steps = steps.Where(x => x.MainGoalId == item.MainGoalId).ToList();

                }
                return mainGoals;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<MainGoal> GetGoalTransactionalRecords(int activityType, int epDetailId, int? floorAssetId, int? inspectionId, int status, int? frequencyId)
        {
            var lists = new List<MainGoal>();
            const string sql = StoredProcedures.Get_GoalTransactionalRecords;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Mode", activityType);
                command.Parameters.AddWithValue("@epdetailId", epDetailId);
                command.Parameters.AddWithValue("@FloorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@InspectionId", inspectionId);
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue("@FrequencyId", frequencyId > 0 ? frequencyId : (object)null);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                {
                    lists = _sqlHelper.ConvertDataTable<MainGoal>(dt);
                    lists = lists.GroupBy(test => test.InspectionDetailId).Select(grp => grp.First()).ToList();
                }
            }
            return lists;
        }

        public  List<MainGoal> GetGoalTransactionalRecords(Guid activityId)
        {
            var lists = new List<MainGoal>();
            const string sql = StoredProcedures.Get_TransactionalRecordsByActivityId;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);             
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                {
                    lists = _sqlHelper.ConvertDataTable<MainGoal>(dt);
                    lists = lists.GroupBy(test => test.InspectionDetailId).Select(grp => grp.First()).ToList();
                }
            }
            return lists;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<MainGoal> GetIlsmMainGoal()
        {
            return GetMainGoal(null, 0).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public  List<MainGoal> GetActivityMainGoals(Guid activityId)
        {
            var lists = new List<MainGoal>();
            const string sql = StoredProcedures.Get_ActivityMainGoals;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ActivityId", activityId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<MainGoal>(ds.Tables[0]);
                    var steps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[1]);
                    foreach (var item in lists)
                        item.Steps = steps.Where(m => m.MainGoalId == item.MainGoalId).ToList();
                }
            }
            return lists;
        }

        public  List<MainGoal> GetMainGoalsbyFloorAssetId(int clientno , int? floorAssetId, int? assetId)
        {
            var lists = new List<MainGoal>();
            const string sql = StoredProcedures.Get_MainGoalsbyFloorAssetId;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@assetId", assetId);
                command.Parameters.AddWithValue("@clientno", clientno);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<MainGoal>(ds.Tables[0]);
                    var steps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[1]);
                    foreach (var item in lists)
                        item.Steps = steps.Where(m => m.MainGoalId == item.MainGoalId).ToList();
                }
            }
            return lists;
        }
    }
}
