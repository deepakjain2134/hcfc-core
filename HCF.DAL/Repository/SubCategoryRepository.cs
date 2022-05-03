
//using HCF.Utility;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HCF.DAL
//{
//    public static class SubCategoryRepository
//    {       

//        public static int Save(BDO.SubCategory subCategory)
//        {
//            int newId;
//            const string sql = StoredProcedures.Insert_subCategory;
//            using (var command = new SqlCommand(sql))
//            {
//                command.CommandType = CommandType.StoredProcedure;
//                command.Parameters.AddWithValue("@SubCategoryId", subCategory.SubCategoryId);
//                command.Parameters.AddWithValue("@CategoryId", subCategory.CategoryId);
//                command.Parameters.AddWithValue("@Name", subCategory.Name);
//                command.Parameters.AddWithValue("@Code", subCategory.Code);
//                command.Parameters.AddWithValue("@Description", subCategory.Description);
//                command.Parameters.AddWithValue("@IsActive", subCategory.IsActive);
//                command.Parameters.Add("@newId", SqlDbType.Int);
//                command.Parameters["@newId"].Direction = ParameterDirection.Output;
//                newId = _sql.ExecuteNonQuery(command, "@newId");
//            }
//            return newId;
//        }
//    }
//}
