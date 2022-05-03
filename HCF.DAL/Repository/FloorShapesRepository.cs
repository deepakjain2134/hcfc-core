using HCF.BDO;
using HCF.Utility;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public  class FloorShapesRepository: IFloorShapesRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public FloorShapesRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newFloorShapes"></param>
        /// <returns></returns>
        public  int Save(FloorShapes newFloorShapes)
        {
            int newId;
            string sql = StoredProcedures.Insert_FloorShapes;
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", newFloorShapes.Id);
                command.Parameters.AddWithValue("@CreatedBy", newFloorShapes.CreatedBy);
                command.Parameters.AddWithValue("@Data", newFloorShapes.Data);
                command.Parameters.AddWithValue("@FloorId", newFloorShapes.FloorId);
                command.Parameters.AddWithValue("@Name", newFloorShapes.Name);
                command.Parameters.AddWithValue("@XMax", newFloorShapes.XMax);
                command.Parameters.AddWithValue("@XMin", newFloorShapes.XMin);
                command.Parameters.AddWithValue("@Notes", newFloorShapes.Notes);
                command.Parameters.AddWithValue("@YMax", newFloorShapes.YMax);
                command.Parameters.AddWithValue("@YMin", newFloorShapes.YMin);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  bool DeleteFloorShapes(int id)
        {
            string sql = StoredProcedures.Delete_FloorShapes;
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newShapes"></param>
        /// <returns></returns>
        public  int SaveTFloorAssetsShaps(TFloorAssetsShaps newShapes)
        {
            int newId;
            const string sql = StoredProcedures.Update_TFloorAssetsShape;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorAssetsId", newShapes.FloorAssetsId);
                command.Parameters.AddWithValue("@ShapeId", newShapes.ShapeId);
                command.Parameters.AddWithValue("@CreatedBy", newShapes.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", newShapes.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public  List<FloorShapes> GetFloorShapes(int floorId)
        {
            return GetFloorShapes().Where(x => x.FloorId == floorId).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  IEnumerable<FloorShapes> GetFloorShapes()
        {
            List<FloorShapes> lists;
            const string sql = StoredProcedures.Get_FloorShapes;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(cmd);
                lists = _sqlHelper.ConvertDataTable<FloorShapes>(dt);
            }
            return lists;
        }

    }
}
