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
    public  class SkillsRepository : ISkillsRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public SkillsRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public  int Save(Skills skills)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Skills;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SkillId", skills.Id);
                command.Parameters.AddWithValue("@Name", skills.Name);
                command.Parameters.AddWithValue("@Code", skills.Code);
                command.Parameters.AddWithValue("@Description", skills.Description);
                command.Parameters.AddWithValue("@IsActive", skills.IsActive);
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
        public  List<Skills> GetSkills()
        {
            List<HCF.BDO.Skills> lists = new List<BDO.Skills>();
            const string sql = StoredProcedures.Get_Skills;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<HCF.BDO.Skills>(dt);
            }
            return lists;
        }

        public  Skills GetSkills(string skillCode)
        {
            return GetSkills().Where(x => x.Code == skillCode).FirstOrDefault();
        }
    }
}
