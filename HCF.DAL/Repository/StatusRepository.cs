using HCF.Utility;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HCF.DAL
{
    public class StatusRepository : IStatusRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public StatusRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public  int Save(BDO.WoStatus status)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TMSStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@WoStatusId", status.WoStatusId);
                command.Parameters.AddWithValue("@Name", status.Name);
                command.Parameters.AddWithValue("@Code", status.Code);
                command.Parameters.AddWithValue("@ModuleCode", status.ModuleCode);
                command.Parameters.AddWithValue("@Description", status.Description);
                command.Parameters.AddWithValue("@IsActive", status.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<BDO.WoStatus> GetStatus()
        {
            List<HCF.BDO.WoStatus> lists = new List<BDO.WoStatus>();
            const string sql = StoredProcedures.Get_TMSStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<HCF.BDO.WoStatus>(dt);
            }
            return lists;
        }
    }
    #region Shift
    public  class ShiftRepository : IShiftRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public ShiftRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Shift"></param>
        /// <returns></returns>
        public  int Save(BDO.Shift shift)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Shift;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ShiftId", shift.ShiftId);
                command.Parameters.AddWithValue("@Name", shift.Name);
                command.Parameters.AddWithValue("@IsActive", shift.IsActive);
                command.Parameters.AddWithValue("@StartTime", shift.StartTime);
                command.Parameters.AddWithValue("@EndTime", shift.EndTime);
                command.Parameters.AddWithValue("@CreatedBy", shift.CreatedBy);
                //command.Parameters.AddWithValue("@Createddate", shift.CreatedDate.Year.Equals(1) ? DateTime.Now: shift.CreatedDate);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public  List<BDO.Shift> GetShift()
        {
            List<HCF.BDO.Shift> lists = new List<BDO.Shift>();
            const string sql = StoredProcedures.Get_Shift;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<HCF.BDO.Shift>(dt);
            }
            return lists;
        }
    }

    #endregion
}
