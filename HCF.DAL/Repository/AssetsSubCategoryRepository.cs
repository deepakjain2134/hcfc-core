using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class AssetsSubCategoryRepository : IAssetsSubCategoryRepository
    {
        #region ctor

        private readonly ISqlHelper _sqlHelper;

        public AssetsSubCategoryRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        #endregion

        #region methods

        public List<AssetsSubCategory> GetAssetsSubCategory(int assetId)
        {
            return GetAssetsSubCategorys(assetId).Where(x => x.IsActive).ToList();
        }

        public List<FireExtinguisherTypes> GetAssetSubCategorySize(int? ascId)
        {
            var lists = new List<FireExtinguisherTypes>();
            const string sql = StoredProcedures.Get_AssetSubCategorySize;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ascId", ascId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<FireExtinguisherTypes>(ds.Tables[0]);
            }
            return lists;
        }

        public Assets ScheduleAssets(int assetId, int epDetailId)
        {
            Assets asset = new Assets();
            const string sql = StoredProcedures.Get_FloorAssetsByAsset;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId);
                command.Parameters.AddWithValue("@epDetailId", epDetailId);
                using (var ds = _sqlHelper.GetDataSet(command))
                {
                    List<TFloorAssets> floorAssets = new List<TFloorAssets>();
                    asset = _sqlHelper.ConvertDataTable<Assets>(ds.Tables[0]).FirstOrDefault();
                    var standardEPs = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[1]);
                    var userProfile = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[3]);
                    var schedules = _sqlHelper.ConvertDataTable<Schedules>(ds.Tables[4]);

                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        TFloorAssets floorAsset = new TFloorAssets();
                        floorAsset.DeviceNo = row["DeviceNo"].ToString();
                        floorAsset.SerialNo = row["SerialNo"].ToString();
                        floorAsset.NearBy = row["NearBy"].ToString();
                        floorAsset.FloorAssetsId = int.Parse(row["FloorAssetsId"].ToString());
                        floorAsset.AssetId = asset.AssetId;
                        floorAsset.Assets = asset;
                        if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                        {
                            floorAsset.FloorId = int.Parse(row["FloorId"].ToString());
                            floorAsset.Floor = new Floor
                            {
                                FloorName = row["FloorName"].ToString(),
                                FloorId = int.Parse(row["FloorId"].ToString()),
                                BuildingId = int.Parse(row["BuildingId"].ToString()),
                                Building = new Buildings
                                {
                                    BuildingName = row["BuildingName"].ToString(),
                                    BuildingId = int.Parse(row["BuildingId"].ToString())
                                }
                            };
                        }
                        floorAsset.Schedules = new List<Schedules>();
                        if (!string.IsNullOrEmpty(row["ScheduleIds"].ToString()))
                        {
                            var names = row["ScheduleIds"].ToString().Split(',');
                            var schedulesIds = Array.ConvertAll(names, int.Parse);
                            var matcheSchedule = from schedule in schedules
                                                 where schedulesIds.Contains(schedule.ScheduleId)
                                                 select schedule;

                            floorAsset.Schedules = matcheSchedule.ToList();
                        }
                        floorAssets.Add(floorAsset);
                    }

                    asset.TFloorAssets = floorAssets;
                    asset.StandardEps = standardEPs;
                    asset.Users = userProfile;
                }
            }
            return asset;
        }

        public List<AssetsSubCategory> GetAssetsSubCategory()
        {
            return GetAssetsSubCategorys(null);
        }

        #endregion

        #region Private method

        private List<AssetsSubCategory> GetAssetsSubCategorys(int? assetId)
        {
            var lists = new List<AssetsSubCategory>();
            const string sql = StoredProcedures.Get_AssetsSubCategory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<AssetsSubCategory>(ds.Tables[0]);
            }
            return lists;
        }

        #endregion
    }
}
