using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class InsDetailRepository : IInsDetailRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public InsDetailRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        private List<TInspectionDetail> GetTInspectionDetail(Guid? activityId)
        {
            var tInspectionDetail = new List<TInspectionDetail>();
            const string sql = StoredProcedures.Get_TInspectionDetail;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    tInspectionDetail = _sqlHelper.ConvertDataTable<TInspectionDetail>(dt);
            }
            return tInspectionDetail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        //public  List<TInspectionDetail> GetTInspectionDetails(Guid activityId)
        //{
        //    List<TInspectionDetail> tInspectionDetail = GetTInspectionDetail(activityId).ToList();
        //    List<MainGoal> mainGoals = MainGoalRepository.GetMainGoal();
        //    List<Steps> steps = StepsRepository.GetSteps();
        //    List<InspectionSteps> tsteps = InsStepsRepository.GetInspectionSteps(activityId);
        //    foreach (var inspectionDetail in tInspectionDetail)
        //    {
        //        inspectionDetail.InspectionSteps = tsteps.Where(x => x.InspectionDetailId == inspectionDetail.InspectionDetailId).ToList();
        //        inspectionDetail.MainGoal = new MainGoal();
        //        inspectionDetail.MainGoal = mainGoals.FirstOrDefault(x => x.MainGoalId == inspectionDetail.MainGoalId);  //mainGoal.FirstOrDefault(x => x.MainGoalId == inspectionDetail.MainGoalId);
        //        foreach (var inspectionStep in inspectionDetail.InspectionSteps)
        //        {
        //            inspectionStep.Steps = new Steps();
        //            inspectionStep.Steps = steps.FirstOrDefault(x => x.StepsId == inspectionStep.StepsId);
        //            inspectionStep.WorkOrders = WorkOrderRepository.GetWorkOrder(activityId, inspectionStep.StepsId);
        //        }
        //    }
        //    return tInspectionDetail;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        public List<TInspectionDetail> GetTInspectionDetails(Guid activityId)
        {
            var tInspectionDetails = new List<TInspectionDetail>();
            const string sql = StoredProcedures.Get_InspectionDetail;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                //command.Parameters.AddWithValue("@inspectionId", inspectionId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    tInspectionDetails = _sqlHelper.ConvertDataTable<TInspectionDetail>(ds.Tables[0]);
                    var mainGoals = _sqlHelper.ConvertDataTable<MainGoal>(ds.Tables[0]);
                    var tsteps = _sqlHelper.ConvertDataTable<InspectionSteps>(ds.Tables[1]);
                    var steps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[1]);
                    var tinsStepsTask = _sqlHelper.ConvertDataTable<TInsStepsTask>(ds.Tables[2]);
                    foreach (InspectionSteps tstep in tsteps)
                    {
                        tstep.Steps = steps.FirstOrDefault(x => x.StepsId == tstep.StepsId);
                        tstep.TInsStepsTask = tinsStepsTask.Where(x => x.TInsStepsId == tstep.TInsStepsId).ToList();
                        tstep.WorkOrders = GetWorkOrder(activityId, tstep.StepsId);
                    }
                    foreach (TInspectionDetail tInspectionDetail in tInspectionDetails)
                    {
                        tInspectionDetail.MainGoal = mainGoals.FirstOrDefault(x => x.MainGoalId == tInspectionDetail.MainGoalId);
                        tInspectionDetail.InspectionSteps = tsteps.Where(x => x.InspectionDetailId == tInspectionDetail.InspectionDetailId).ToList();
                    }
                }

            }
            return tInspectionDetails;
        }


        public List<WorkOrder> GetWorkOrder(Guid activityId, int stepsId)
        {
            var workOrders = new List<WorkOrder>();
            const string sql = StoredProcedures.Get_GetStepWorkOrder;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                command.Parameters.AddWithValue("@StepsId", stepsId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    workOrders = _sqlHelper.ConvertDataTable<WorkOrder>(dt);
            }
            return workOrders;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        //public  List<TInspectionDetail> TInspectionDetail(Guid? activityId)
        //{
        //    var tInspectionDetail = GetTInspectionDetail(activityId);
        //    return tInspectionDetail;
        //}
    }
}
