using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HCF.BDO;
using HCF.Utility;

namespace HCF.DAL
{
    public class ScheduleRepository : IScheduleRepository
    {
        #region ctor

        private readonly ISqlHelper _sqlHelper;
        public ScheduleRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        #endregion
        #region Schedule

        public int AddSchedule(Schedules newSchedule)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Schedule;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ReferenceName", newSchedule.ReferenceName);
                command.Parameters.AddWithValue("@StartDate", newSchedule.StartDate);
                command.Parameters.AddWithValue("@IsActive", newSchedule.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", newSchedule.CreatedBy);
                command.Parameters.AddWithValue("@Description", newSchedule.Description);
                command.Parameters.AddWithValue("@ScheduleId", newSchedule.ScheduleId);
                command.Parameters.AddWithValue("@IsCustomDate", newSchedule.IsCustomDate);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;
        }

        public bool UpdateSchedule(int scheduleId, bool isActive)
        {
            bool status;
            const string sql = StoredProcedures.Update_Schedule;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@scheduleId", scheduleId);
                command.Parameters.AddWithValue("@isActive", isActive);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public int SaveAssetsSchedule(int scheduleId, string floorAssetIds, int epId, int activityType)
        {
            //int newId;
            const string sql = StoredProcedures.Insert_AssetsSchedule;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ActiviityType", activityType);
                command.Parameters.AddWithValue("@EPDetailId", epId);
                command.Parameters.AddWithValue("@floorAssetsId", floorAssetIds);
                command.Parameters.AddWithValue("@ScheduleId", scheduleId);
                _sqlHelper.ExecuteNonQuery(command);

            }
            return 1;
        }

        public List<Schedules> GetSchedules(int schedulesId)
        {

            List<Schedules> schedules = new List<Schedules>();
            const string sql = StoredProcedures.Get_Schedules;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@schedulesId", schedulesId);
                using (var ds = _sqlHelper.GetDataSet(command))
                {
                    schedules = _sqlHelper.ConvertDataTable<Schedules>(ds.Tables[0]);
                    var scheduleFrequency = ConvertDtToScheduleFrequency(ds.Tables[1]);
                    foreach (var item in schedules)
                    {
                        item.ScheduleFrequency = scheduleFrequency.Where(x => x.ScheduleId == item.ScheduleId).ToList();
                        item.StandardEP = ConvertDtToScheduleEp(ds.Tables[2], item.ScheduleId);
                        item.Assets = ConvertDtToScheduleAssets(ds.Tables[3], item.ScheduleId);
                        foreach (var schedulefreq in item.ScheduleFrequency)
                        {
                            schedulefreq.StartScheduleDate = item.StartDate;
                        }
                    }

                }
            }
            return schedules;
        }

        public List<Schedules> GetSchedules(int? epId, int? floorAssetId)
        {
            List<Schedules> schedules = new List<Schedules>();
            const string sql = StoredProcedures.Get_EPScedules;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epId", epId);
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                var ds = _sqlHelper.GetDataSet(command);
                schedules = _sqlHelper.ConvertDataTable<Schedules>(ds.Tables[0]);
                var data = _sqlHelper.ConvertDataTable<ScheduleFrequency>(ds.Tables[0]);
                foreach (var item in schedules)
                {
                    item.ScheduleFrequency = data.Where(x => x.ScheduleId == item.ScheduleId).ToList();
                }
            }
            return schedules;
        }

        #endregion

        #region Schedule Frequency

        public int AddScheduleFrequency(ScheduleFrequency newScheduleFrequency)
        {
            int newId;
            const string sql = StoredProcedures.Insert_ScheduleFrequency;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ScheduleId", newScheduleFrequency.ScheduleId);
                command.Parameters.AddWithValue("@FrequencyId", newScheduleFrequency.FrequencyId);
                command.Parameters.AddWithValue("@YearNo", newScheduleFrequency.YearNo > 0 ? newScheduleFrequency.YearNo : (object)DBNull.Value);
                command.Parameters.AddWithValue("@MonthNo", newScheduleFrequency.MonthNo > 0 ? newScheduleFrequency.MonthNo : (object)DBNull.Value);
                command.Parameters.AddWithValue("@WeekNo", newScheduleFrequency.WeekNo > 0 ? newScheduleFrequency.WeekNo : (object)DBNull.Value);
                command.Parameters.AddWithValue("@WeekDays", newScheduleFrequency.WeekDays > 0 ? newScheduleFrequency.WeekDays : (object)DBNull.Value);
                command.Parameters.AddWithValue("@StartTime", newScheduleFrequency.StartTime.Minutes > 0 ? newScheduleFrequency.StartTime : (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndTime", newScheduleFrequency.EndTime.Minutes > 0 ? newScheduleFrequency.EndTime : (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsCustomDate", newScheduleFrequency.IsCustomDate);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;
        }


        #endregion

        #region Schedule EP Assets

        #endregion

        #region Common 

        private List<ScheduleFrequency> ConvertDtToScheduleFrequency(DataTable dt)
        {
            var scheduleFrequency = _sqlHelper.ConvertDataTable<ScheduleFrequency>(dt);
            var frequencyMaster = _sqlHelper.ConvertDataTable<FrequencyMaster>(dt);
            foreach (var item in scheduleFrequency)
                item.Frequency = frequencyMaster.FirstOrDefault(x => x.FrequencyId == item.FrequencyId);
            return scheduleFrequency;
        }

        private IEnumerable<StandardEps> ConvertDtToScheduleEp(DataTable dt, int scheduleId)
        {
            var standardEps = new List<StandardEps>();
            string expr = "ScheduleId = '" + scheduleId + "'";
            DataRow[] rows = dt.Select(expr);
            if (rows.Length > 0)
            {
                DataTable newDatatable = rows.CopyToDataTable();
                standardEps = _sqlHelper.ConvertDataTable<StandardEps>(newDatatable);
                return standardEps.GroupBy(test => test.EPDetailId).Select(grp => grp.First()).ToList();
            }
            return standardEps;
        }

        private List<Assets> ConvertDtToScheduleAssets(DataTable dt, int scheduleId)
        {
            List<Assets> assets = new List<Assets>();
            string expr = "ScheduleId = '" + scheduleId + "'";
            DataRow[] rows = dt.Select(expr);
            if (rows.Length > 0)
            {
                DataTable newDatatable = rows.CopyToDataTable();
                assets = _sqlHelper.ConvertDataTable<Assets>(newDatatable);
                return assets.GroupBy(test => test.AssetId).Select(grp => grp.First()).ToList();
            }
            return assets;
        }

        #endregion

        #region Schedules by EPdetailId and floorAssetId

        private Schedules GetSchedulesbyEPAndFloorAssetId(int epdetailId, int? floorAssetsId = 0, Guid? activityId = null)
        {
            List<Schedules> objSchedules = new List<Schedules>();
            const string sql = StoredProcedures.Get_SchedulesbyEP;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetailId", epdetailId);
                command.Parameters.AddWithValue("@floorAssetsId", floorAssetsId);
                command.Parameters.AddWithValue("@activityId", activityId);
                using (var ds = _sqlHelper.GetDataSet(command))
                {
                    objSchedules = _sqlHelper.ConvertDataTable<Schedules>(ds.Tables[0]).GroupBy(x => x.ScheduleId).Select(x => x.First()).ToList();
                    var scheduleFrequency = ConvertDtToScheduleFrequency(ds.Tables[0]);
                    foreach (var item in objSchedules)
                    {
                        item.ScheduleFrequency = scheduleFrequency.Where(x => x.ScheduleId == item.ScheduleId).ToList();
                        foreach (var schedulefreq in item.ScheduleFrequency)
                        {
                            schedulefreq.StartScheduleDate = item.StartDate;
                        }
                    }
                }
            }
            return objSchedules.FirstOrDefault();
        }


        public Schedules GetSchedulesbyEPAndFloorAssetId(int epdetailId, int floorAssetsId)
        {
            return GetSchedulesbyEPAndFloorAssetId(epdetailId, floorAssetsId, null);
        }

        public Schedules GetSchedulesbyEPAndFloorAssetId(Guid activityId)
        {
            return GetSchedulesbyEPAndFloorAssetId(0, 0, activityId);
        }

        public Schedules GetSchedulesbyEPAndFloorAssetId(int epdetailId)
        {
            return GetSchedulesbyEPAndFloorAssetId(epdetailId);
        }

        #endregion
    }
}
