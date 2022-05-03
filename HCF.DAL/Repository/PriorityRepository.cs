using HCF.Utility;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HCF.DAL
{
    public  class PriorityRepository : IPriorityRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public PriorityRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }


        #region Priority

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPriority"></param>
        /// <returns></returns>
        public  int Save(BDO.Priority newPriority)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Priority;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PriorityId", newPriority.PriorityId);
                command.Parameters.AddWithValue("@Name", newPriority.Name);
                command.Parameters.AddWithValue("@Code", newPriority.Code);
                command.Parameters.AddWithValue("@ModuleCode", newPriority.ModuleCode);
                command.Parameters.AddWithValue("@Description", newPriority.Description);  
                command.Parameters.AddWithValue("@IsActive", newPriority.IsActive);
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
        public  List<BDO.Priority> GetPriority()
        {
            var lists = new List<BDO.Priority>();
            const string sql = StoredProcedures.Get_Priority;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;               
                var dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<HCF.BDO.Priority>(dt);
            }
            return lists;
        }
        

        #endregion
       
    }
}
