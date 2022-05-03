using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public  class WingRepository :IWingRepository
    {
        private readonly ISqlHelper _sqlHelper;

        public WingRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newWings"></param>
        /// <returns></returns>
        public  int Save(Wing wing)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Wing;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@WingName", wing.WingName);
                command.Parameters.AddWithValue("@WingId", wing.WingId);
                command.Parameters.AddWithValue("@floorId", wing.FloorId);
                command.Parameters.AddWithValue("@CreatedBy", wing.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", wing.IsActive);                
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
        private  List<Wing> GetWings(int? floorId, int? wingId)
        {
            var wings = new List<Wing>();
            const string sql = StoredProcedures.Get_Wing;
            using (var cmd = new SqlCommand(sql))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@floorId", floorId);
                cmd.Parameters.AddWithValue("@wingId", wingId);
                DataTable dt = _sqlHelper.GetDataTable(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    var wing = new Wing
                    {
                        WingId =  Convert.ToInt32(row["WingId"].ToString()),
                        WingName = row["WingName"].ToString(),
                        BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
                        FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                        IsActive = Convert.ToBoolean(row["IsActive"].ToString()),                      
                        Floor = new Floor
                        {
                            FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                            FloorName = row["FloorName"].ToString(),
                            FloorCode = row["FloorCode"].ToString(),
                            BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            Building = new Buildings
                            {
                                BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
                                BuildingName = row["BuildingName"].ToString()                               
                            }
                        }
                    };
                    wings.Add(wing);
                }
                //if (dt!=null)
                //    wings=DBOperations.ConvertDataTable<Wing>(dt);

            }
            return wings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public List<Wing> GetWings(int floorId)
        {
            return GetWings(floorId, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Wing> GetWings()
        {
            return GetWings(null, null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public  Wing GetWing(int wingId)
        {
            return GetWings(null, wingId).FirstOrDefault(); 
        }
    }
}
