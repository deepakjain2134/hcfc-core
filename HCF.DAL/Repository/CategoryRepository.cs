using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HCF.DAL
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public CategoryRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public int Save(Category category)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Category;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryId", category.CategoryId);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@Code", category.Code);
                command.Parameters.AddWithValue("@ModuleCode", category.ModuleCode);
                command.Parameters.AddWithValue("@Description", category.Description);
                command.Parameters.AddWithValue("@IsActive", category.IsActive);
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
        public List<Category> GetCategory()
        {
            var lists = new List<Category>();
            const string sql = StoredProcedures.Get_Category;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                foreach (DataRow row in dt.Rows)
                {
                    var list = new Category
                    {
                        CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                        Name = row["Name"].ToString(),
                        Description = row["Description"].ToString()
                    };
                    lists.Add(list);
                }
            }
            return lists;
        }
    }
}
