
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HCF.BDO;
using HCF.Utility;

namespace HCF.DAL
{
    public  class FireWatchRepository : IFireWatchRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IBuildingsRepository _buildingsRepository;
        private readonly IUsersRepository _usersRepository;

        public FireWatchRepository(ISqlHelper sqlHelper, IBuildingsRepository buildingsRepository, IUsersRepository usersRepository)
        {
            _buildingsRepository = buildingsRepository;
            _usersRepository = usersRepository;
            _sqlHelper = sqlHelper;
        }


        #region Fire Watch

        public  List<TimeSlots> GetTimeSlots(DateTime selectedDate, int timeSlotPeriod)
        {
            List<TimeSlots> slots = new List<TimeSlots>();

            DateTime oCurrentDate = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, selectedDate.Hour, selectedDate.Minute, selectedDate.Second);
            DateTime oTimeSlot = new DateTime(oCurrentDate.Year, oCurrentDate.Month, oCurrentDate.Day, oCurrentDate.Hour, oCurrentDate.Minute, oCurrentDate.Second);
            bool isNext = false;
            for (var iCounter = 1; iCounter <= 24 / timeSlotPeriod; iCounter++)
            {
                TimeSlots slot = new TimeSlots();
                slot.Start = oTimeSlot;

                slot.Sequence = iCounter;
                slot.DateText = selectedDate.ToString("MMM dd, yyyy");
                slot.End = oTimeSlot.AddHours(timeSlotPeriod).AddMinutes(-1);
                slot.StartTimeSpan = Utility.Conversion.ConvertToTimeSpan(slot.Start.ToUniversalTime());
                slot.EndTimeSpan = Utility.Conversion.ConvertToTimeSpan(slot.End.ToUniversalTime());

                slot.StartTimeText = slot.Start.ToString("t");
                slot.EndTimeText = slot.End.ToString("t");

                slot.UTCEndTime = slot.End.ToUniversalTime();
                slot.UTCStartTime = slot.Start.ToUniversalTime();
                slot.isNext = isNext;
                if (DateTime.UtcNow.Ticks > slot.UTCStartTime.Ticks && DateTime.UtcNow.Ticks < slot.UTCEndTime.Ticks)
                {
                    slot.isCurrent = true;
                }
                slots.Add(slot);
                if (DateTime.UtcNow.Ticks > slot.UTCStartTime.Ticks && DateTime.UtcNow.Ticks < slot.UTCEndTime.Ticks)
                {
                    isNext = true;
                }
                oTimeSlot = oTimeSlot.AddHours(timeSlotPeriod);
            }

            return slots;
        }


        public  int Save(FireWatchLog objFireWatchLog)
        {
            int newId;
            const string sql = StoredProcedures.Insert_FireWatchLog;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@fwlogId", objFireWatchLog.FWLogId);
                command.Parameters.AddWithValue("@Comment", objFireWatchLog.Comment);
                command.Parameters.AddWithValue("@CreatedBy", objFireWatchLog.CreatedBy);
                command.Parameters.AddWithValue("@FinishTime", objFireWatchLog.FinishTime);
                command.Parameters.AddWithValue("@Area", objFireWatchLog.Area);
                command.Parameters.AddWithValue("@InspectorName", objFireWatchLog.InspectorName);
                command.Parameters.AddWithValue("@LogDate", objFireWatchLog.LogDate);
                command.Parameters.AddWithValue("@LogTime", objFireWatchLog.LogTime);
                command.Parameters.AddWithValue("@StartTime", objFireWatchLog.StartTime);
                command.Parameters.AddWithValue("@RoundInspDate", objFireWatchLog.RoundInspDate);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public  List<FireWatchLog> GetFireWatchLog(DateTime? roundInspdate)
        {
            List<FireWatchLog> lists = new List<FireWatchLog>();
            const string sql = StoredProcedures.Get_FireWatchLog;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@roundInspdate", roundInspdate);
                DataSet ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<FireWatchLog>(ds.Tables[0]);
                    List<UserProfile> users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                    foreach (var item in lists)
                    {
                        if (item.FinishTime.HasValue)
                        {
                            DateTime time = DateTime.Today.Add(item.FinishTime.Value);
                            item.CompleteTime = time.ToString("hh:mm tt");
                        }
                        item.RoundInspDateTimeSpan = Conversion.ConvertToTimeSpan(item.RoundInspDate);
                        //item.LogDate = item.LogDate.Value.ToUniversalTime();
                        item.UserProfile = users.FirstOrDefault(x => x.UserId == item.CreatedBy);
                    }
                }
            }
            return lists;
        }


        #endregion

        #region FirewatchNotification
        public  int SaveFirewatchNotification(TFirewatchNotificationType t)
        {
            int newId = 0;
            const string sql = StoredProcedures.Insert_FirewatchNotification;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@FirewatchID", t.FirewatchID);
                command.Parameters.AddWithValue("@ScheduledLogID", t.ScheduledLogID);
                command.Parameters.AddWithValue("@FirewatchNotificationTypeId", t.FirewatchNotificationTypeId);
                command.Parameters.AddWithValue("@InitNotificationDate", t.InitNotificationDate);
                command.Parameters.AddWithValue("@EndNotificationDate", t.EndNotificationDate);
                command.Parameters.AddWithValue("@Name", t.Name);
                command.Parameters.AddWithValue("@CreatedBy", t.CreatedBy);
                command.Parameters.AddWithValue("@InitNotificationTime", t.InitNotificationTime);
                command.Parameters.AddWithValue("@EndNotificationTime", t.EndNotificationTime);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
                
            }
            return newId;
        }
        #endregion

        #region ScheduledLogs


        /// <summary>
        /// </summary>
        /// <param name="objScheduledLogs"></param>
        /// <returns></returns>
        public  int Save(ScheduledLogs objScheduledLogs)
        {
            int newId;
            const string sql = StoredProcedures.Insert_ScheduledLogs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BuildingId", objScheduledLogs.BuildingId);
                command.Parameters.AddWithValue("@FloorId", objScheduledLogs.FloorId);
                command.Parameters.AddWithValue("@IsClosed", objScheduledLogs.IsClosed);
                command.Parameters.AddWithValue("@Area", objScheduledLogs.Area);
                command.Parameters.AddWithValue("@Comment", objScheduledLogs.Comment);
                command.Parameters.AddWithValue("@CreatedBy", objScheduledLogs.CreatedBy);
                command.Parameters.AddWithValue("@StartDate", objScheduledLogs.StartDate);
                command.Parameters.AddWithValue("@Enddate", objScheduledLogs.Enddate);
                command.Parameters.AddWithValue("@PrintInitial", objScheduledLogs.PrintInitial);
                command.Parameters.AddWithValue("@PrintFinal", objScheduledLogs.PrintFinal);
                command.Parameters.AddWithValue("@Signature1Id", objScheduledLogs.Signature1Id);
                command.Parameters.AddWithValue("@Signature2Id", objScheduledLogs.Signature2Id);
                command.Parameters.AddWithValue("@PermitNo", objScheduledLogs.PermitNo);
                command.Parameters.AddWithValue("@PermitType", objScheduledLogs.PermitType);
                command.Parameters.AddWithValue("@InitiatedBy", objScheduledLogs.InitiatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objScheduledLogs"></param>
        /// <returns></returns>
        public  bool Update(ScheduledLogs objScheduledLogs)
        {
            const string sql = StoredProcedures.Update_ScheduledLogs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", objScheduledLogs.Id);
                command.Parameters.AddWithValue("@BuildingId", objScheduledLogs.BuildingId);
                command.Parameters.AddWithValue("@FloorId", objScheduledLogs.FloorId);
                command.Parameters.AddWithValue("@IsClosed", objScheduledLogs.IsClosed);
                command.Parameters.AddWithValue("@Area", objScheduledLogs.Area);
                command.Parameters.AddWithValue("@Comment", objScheduledLogs.Comment);
                command.Parameters.AddWithValue("@CreatedBy", objScheduledLogs.CreatedBy);
                command.Parameters.AddWithValue("@StartDate", objScheduledLogs.StartDate);
                command.Parameters.AddWithValue("@Enddate", objScheduledLogs.Enddate);
                command.Parameters.AddWithValue("@PrintInitial", objScheduledLogs.PrintInitial);
                command.Parameters.AddWithValue("@PrintFinal", objScheduledLogs.PrintFinal);
                command.Parameters.AddWithValue("@Signature1Id", objScheduledLogs.Signature1Id);
                command.Parameters.AddWithValue("@Signature2Id", objScheduledLogs.Signature2Id);
                command.Parameters.AddWithValue("@PermitNo", objScheduledLogs.PermitNo);
                command.Parameters.AddWithValue("@PermitType", objScheduledLogs.PermitType);
                command.Parameters.AddWithValue("@InitiatedBy", objScheduledLogs.InitiatedBy);
                return _sqlHelper.ExecuteNonQuery(command);

            }
        }


       
        public  List<ScheduledLogs> GetScheduledLogs(int? id)
        {
            var lists = new List<ScheduledLogs>();
            const string sql = StoredProcedures.Get_ScheduledLogs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<ScheduledLogs>(ds.Tables[0]);
                    var buildings = _buildingsRepository.GetBuildings();

                    // var buildings = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[0]);
                    //var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                    var users = _usersRepository.GetUsersList();
                    foreach (var item in lists)
                    {
                        item.StartDateTimeSpan = item.StartDate != null ? Conversion.ConvertToTimeSpan(item.StartDate) : 0;
                        item.EnddateTimeSpan = item.Enddate != null ? Conversion.ConvertToTimeSpan(item.Enddate) : 0;
                        item.Buildings = buildings.FirstOrDefault(x => x.BuildingId == item.BuildingId);
                        item.UserProfile = users.FirstOrDefault(x => x.UserId == item.CreatedBy);
                        if (item.ClosedBy.HasValue)                       
                            item.ClosedByUser = users.FirstOrDefault(x => x.UserId == item.ClosedBy.Value); //Users.GetUserProfile(item.ClosedBy.Value);                        
                    }
                }
            }
             return lists;

        }

        public  List<ScheduledLogs> GetScheduledLogsDetail(int? id)
        {
            var lists = new List<ScheduledLogs>();
            const string sql = StoredProcedures.Get_ScheduledLogs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<ScheduledLogs>(ds.Tables[0]);
                    var buildings = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[0]);
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                    var tFirewatchNotificationType = _sqlHelper.ConvertDataTable<TFirewatchNotificationType>(ds.Tables[1]);
                    List<DigitalSignature> digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[2]);
                    if (id != null)
                    {
                        
                        foreach (var item in lists)
                        {
                            item.StartDateTimeSpan = item.StartDate != null ? Conversion.ConvertToTimeSpan(item.StartDate) : 0;
                            item.EnddateTimeSpan = item.Enddate != null ? Conversion.ConvertToTimeSpan(item.Enddate) : 0;
                            item.Buildings = buildings.FirstOrDefault(x => x.BuildingId == item.BuildingId);
                            item.UserProfile = users.FirstOrDefault(x => x.UserId == item.CreatedBy);
                            item.TFirewatchNotificationType = tFirewatchNotificationType;

                            if (item.ClosedBy.HasValue)
                            {
                                item.ClosedByUser = _usersRepository.GetUsersList(item.ClosedBy.Value);
                            }

                            if (item.Signature1Id.HasValue)
                                item.DSSign1Signature = digitalSignature.Where(x => x.DigSignatureId == item.Signature1Id).FirstOrDefault();


                            if (item.Signature2Id.HasValue)
                                item.DSSign2Signature = digitalSignature.Where(x => x.DigSignatureId == item.Signature2Id).FirstOrDefault();
                        }
                    }
                }
            }
            return lists;



        }
        public  ScheduledLogs GetFirewatchNotificationType()
        {
            var Logs = new ScheduledLogs();
            const string table = StoredProcedures.Get_Notificationtype;
            using (var command = new SqlCommand(table))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataTable data = _sqlHelper.GetDataTable(command);
                if (data != null)
                {
                    Logs.TFirewatchNotificationType = _sqlHelper.ConvertDataTable<TFirewatchNotificationType>(data);
                  
                  
                }
            }
            return Logs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="closedBy"></param>
        /// <returns></returns>
        public  bool CloseScheduleLog(int id, int closedBy)
        {
            bool status;
            const string sql = StoredProcedures.Close_Schedulelog;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@closedBy", closedBy);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        #endregion
    }
}
