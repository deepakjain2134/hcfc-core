using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class InsStepsRepository : IInsStepsRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IStepsRepository _stepsRepository;
        public InsStepsRepository(ISqlHelper sqlHelper, IStepsRepository stepsRepository)
        {
            _sqlHelper = sqlHelper;
            _stepsRepository = stepsRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionSteps"></param>
        /// <returns></returns>
        public int AddInspectionSteps(InspectionSteps inspectionSteps)
        {
            int newId;
            const string sql = StoredProcedures.Insert_InspectionSteps; //"Insert_InspectionSteps";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectionDetailId", inspectionSteps.InspectionDetailId);
                command.Parameters.AddWithValue("@StepId", inspectionSteps.StepsId);
                command.Parameters.AddWithValue("@Status", inspectionSteps.Status);
                command.Parameters.AddWithValue("@Comments", inspectionSteps.Comments);
                command.Parameters.AddWithValue("@FileName", inspectionSteps.FileName);
                command.Parameters.AddWithValue("@FilePath", inspectionSteps.FilePath);
                command.Parameters.AddWithValue("@DRTime", inspectionSteps.DRTime);
                command.Parameters.AddWithValue("@DeficiencyDate", inspectionSteps.DeficiencyDate);
                command.Parameters.AddWithValue("@DeficiencyCode", inspectionSteps.DeficiencyCode);
                command.Parameters.AddWithValue("@RaDate", inspectionSteps.RaDate);
                command.Parameters.AddWithValue("@InputValue", inspectionSteps.InputValue);
                command.Parameters.AddWithValue("@AddedBy", inspectionSteps.AddedBy);
                command.Parameters.AddWithValue("@AddedDate", inspectionSteps.AddedDate);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionSteps"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public bool Update(InspectionSteps inspectionSteps, int createdBy)
        {
            bool status;
            const string sql = StoredProcedures.Update_InspectionSteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DeficiencyStatus", inspectionSteps.DeficiencyStatus);
                command.Parameters.AddWithValue("@InspectionDetailId", inspectionSteps.InspectionDetailId);
                command.Parameters.AddWithValue("@createdBy", createdBy);
                command.Parameters.AddWithValue("@StepId", inspectionSteps.StepsId);
                command.Parameters.AddWithValue("@TInsStepsId", inspectionSteps.TInsStepsId);
                command.Parameters.AddWithValue("@Comments", inspectionSteps.Comments);
                command.Parameters.AddWithValue("@DeficiencyCode", inspectionSteps.DeficiencyCode);
                command.Parameters.AddWithValue("@RaDate", inspectionSteps.RaDate);
                command.Parameters.AddWithValue("@DateCompleted", inspectionSteps.DateCompleted);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<InspectionSteps> GetInspectionSteps(int? inspectionDetailId, Guid? activityId, int? inspectionId)
        {
            List<InspectionSteps> lists = new List<InspectionSteps>();
            const string sql = StoredProcedures.Get_TinspectionSteps;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inspectionDetailId", inspectionDetailId);
                cmd.Parameters.AddWithValue("@activityId", activityId);
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null)
                    lists = _sqlHelper.ConvertDataTable<InspectionSteps>(dt);
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<InspectionSteps> GetInspectionSteps()
        {
            return GetInspectionSteps(null, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionDetailId"></param>
        /// <returns></returns>
        public List<InspectionSteps> GetInspectionSteps(Guid activityId)
        {
            return GetInspectionSteps(null, activityId, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public List<InspectionSteps> GetInspectionSteps(int inspectionId, int epDetailId)
        {
            return GetInspectionSteps(null, null, inspectionId);
        }

        public List<InspectionSteps> GetTinspectionStepsByActivityId(Guid activityId)
        {
            var steps = new List<InspectionSteps>();
            const string sql = StoredProcedures.Get_TinspectionStepsByActivityId;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                DataTable dt = _sqlHelper.GetDataTable(command);
                steps = _sqlHelper.ConvertDataTable<InspectionSteps>(dt);
                foreach (InspectionSteps step in steps)
                    step.Steps = _stepsRepository.GetStep(step.StepsId);
            }
            return steps;
        }

        public TInspectionActivity GetTransactionSteps(Guid activityId)
        {
            var activity = new TInspectionActivity();
            var details = new List<TInspectionDetail>();
            const string sql = StoredProcedures.Get_InspectionSteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                DataSet dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    List<InspectionSteps> steps = _sqlHelper.ConvertDataTable<InspectionSteps>(dt.Tables[0]);
                    List<Steps> msteps = _sqlHelper.ConvertDataTable<Steps>(dt.Tables[0]);
                    List<WorkOrder> wo = new List<WorkOrder>();

                    foreach (DataRow row in dt.Tables[1].Rows)
                    {
                        var worder = new WorkOrder
                        {
                            WorkOrderId = Convert.ToInt32(row["WorkOrderId"].ToString()),
                            IssueId = Convert.ToInt32(row["IssueId"].ToString()),
                            WorkOrderNumber = row["WorkOrderNumber"].ToString(),
                            DeficiencyCode = row["DeficiencyCode"].ToString()
                        };
                        if (!string.IsNullOrEmpty(row["CreatedDate"].ToString()))
                            worder.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                        if (!string.IsNullOrEmpty(row["DateCompleted"].ToString()))
                            worder.DateCompleted = Convert.ToDateTime(row["DateCompleted"].ToString());
                        wo.Add(worder);
                    }

                    details = _sqlHelper.ConvertDataTable<TInspectionDetail>(dt.Tables[0]).GroupBy(car => car.InspectionDetailId).Select(g => g.First()).ToList();
                    activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(dt.Tables[2]).FirstOrDefault();
                    foreach (var step in steps)
                    {
                        step.Steps = msteps.FirstOrDefault(x => x.StepsId == step.StepsId);
                        step.WorkOrders = wo.Where(x => x.IssueId == step.IssueId).ToList();
                    }
                    foreach (TInspectionDetail detail in details)
                    {
                        detail.InspectionSteps = steps.Where(x => x.InspectionDetailId == detail.InspectionDetailId).ToList();
                    }
                }
            }
            activity.TInspectionDetail = details;
            return activity;
        }

        public List<InspectionSteps> GetTinspectionStepsByFrequency(int? frequencyId, int? floorAssetId, int? stepsId, int? activityType)
        {
            List<InspectionSteps> steps = new List<InspectionSteps>();
            const string sql = StoredProcedures.Get_TinspectionStepsByFrequency;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@frequencyId", frequencyId);
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@stepsId", stepsId);
                command.Parameters.AddWithValue("@activityType", activityType);
                var dt = _sqlHelper.GetDataTable(command);
                steps = _sqlHelper.ConvertDataTable<InspectionSteps>(dt);
                foreach (InspectionSteps step in steps)
                    step.Steps = _stepsRepository.GetStep(step.StepsId);
            }
            return steps;
        }

        public bool Update(InspectionSteps inspectionSteps, int createdBy, Guid activityId)
        {
            bool status;
            const string sql = StoredProcedures.Update_InspectionIlsmFailSteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@stepsId", inspectionSteps.StepsId);
                command.Parameters.AddWithValue("@activityId", activityId);
                command.Parameters.AddWithValue("@createdBy", createdBy);
                command.Parameters.AddWithValue("@Status", inspectionSteps.Status);
                command.Parameters.AddWithValue("@Comments", inspectionSteps.Comments);
                command.Parameters.AddWithValue("@fileName", inspectionSteps.FileName);
                command.Parameters.AddWithValue("@filepath", inspectionSteps.FilePath);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
    }
}
