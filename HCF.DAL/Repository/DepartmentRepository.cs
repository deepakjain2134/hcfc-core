using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public DepartmentRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public int Save(Department department)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Department;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                command.Parameters.AddWithValue("@Name", department.Name);
                command.Parameters.AddWithValue("@Code", department.Code);
                command.Parameters.AddWithValue("@Description", department.Description);
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
        public List<BDO.Department> GetDepartment()
        {
            List<Department> department = new List<Department>();
            string sql = StoredProcedures.Get_Department;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(cmd);
                department = _sqlHelper.ConvertDataTable<Department>(dt);
            }
            return department;
        }
        public async Task<List<BDO.Department>> GetDepartmentAsync()
        {
            List<Department> department = new List<Department>();
            string sql = StoredProcedures.Get_Department;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = await _sqlHelper.GetDataTableAsync(cmd);
                department = _sqlHelper.ConvertDataTable<Department>(dt);
            }
            return await Task.FromResult(department);
        }

    }
}
