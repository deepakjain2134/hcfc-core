using HCF.BDO;

using HCF.Utility;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class FloorTypeRepository : IFloorTypeRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public FloorTypeRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        #region

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <returns></returns>
        private  IEnumerable<FloorTypes> FloorTypes()
        {
            return GetFloorTypes(0);
        }

        //public static List<Floor> Floors(int floorTypeId, int? CheckFileEmpty)
        //{
        //    return FloorRepository.GetFloorsByFloorType(floorTypeId, CheckFileEmpty);
        //}
        private  IEnumerable<FloorTypes> GetFloorTypes(int userId)
        {
            var floorTypes = new List<FloorTypes>();
            var sql = StoredProcedures.Get_FloorTypes;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null)
                    floorTypes = _sqlHelper.ConvertDataTable<FloorTypes>(dt);
            }
            return floorTypes;
        }

        public  IEnumerable<FloorTypes> GetUserDrawings(int userId)
        {
            return GetFloorTypes(userId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public  List<FloorTypes> FloorTypes(bool status)
        {
            return FloorTypes().Where(x => x.IsActive == status).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorTypeId"></param>
        /// <returns></returns>
        public  FloorTypes FloorTypes(int floorTypeId)
        {
            return FloorTypes().FirstOrDefault(x => x.FloorTypeId == floorTypeId);
        }

        #endregion

      
    }
}
