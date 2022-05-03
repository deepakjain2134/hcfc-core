using System;
using HCF.BDO;
using System.Data.SqlClient;
using HCF.Utility;
using System.Data;

namespace HCF.DAL
{
    public  class TEPInspectionRepository: ITEPInspectionRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public TEPInspectionRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        #region Set Asset Inspection Date

        private  int SaveInspectionDate(int? floorAssetId, DateTime inspectionDate, int activityType, int? epId)
        {
            int newId;
            const string sql = StoredProcedures.Insert_AssetInspectionDate;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetsId", floorAssetId);
                command.Parameters.AddWithValue("@inspectionDate", inspectionDate);
                command.Parameters.AddWithValue("@ActivityType", activityType);
                command.Parameters.AddWithValue("@epId", epId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int SaveEpInspectionDate(int epId, DateTime inspectionDate)
        {
            return SaveInspectionDate(null, inspectionDate, 1, epId);
        }

        public  int SaveAssetInspectionDate(int floorAssetId, DateTime inspectionDate)
        {
            return SaveInspectionDate(floorAssetId, inspectionDate, 2, null);
        }

        #endregion

        public  int SaveFixedInspectionDate(AssetsFixInsDate objAssetsFixInsDate)
        {
            int newId;
            const string sql = StoredProcedures.Insert_AssetInspectionDate;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetsId", objAssetsFixInsDate.FloorAssetsId);
                command.Parameters.AddWithValue("@inspectionGroupId", objAssetsFixInsDate.InspectionGroupId);
                command.Parameters.AddWithValue("@inspectionDate", objAssetsFixInsDate.InspectionDate.Value.ToUniversalTime());
                command.Parameters.AddWithValue("@ActivityType", objAssetsFixInsDate.ActivityType);
                command.Parameters.AddWithValue("@epId", objAssetsFixInsDate.EpDetailId);
                command.Parameters.AddWithValue("@assetId", objAssetsFixInsDate.AssetId);
                command.Parameters.AddWithValue("@doctypeId", objAssetsFixInsDate.DocTypeId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  bool AddInspectionDueDate()
        {
            bool status;
            const string sql = StoredProcedures.Insert_TInspectionDueDate;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
    }
}
