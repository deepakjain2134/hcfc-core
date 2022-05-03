using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public  class AssetTypeRepository : IAssetTypeRepository
    {

        private readonly ISqlHelper _sqlHelper;
        private readonly IAssetsRepository _assetsRepository;
        private readonly IEpRepository _epRepository;

        public AssetTypeRepository(ISqlHelper sqlHelper, IAssetsRepository assetsRepository, IEpRepository epRepository)
        {
            _sqlHelper = sqlHelper;
            _assetsRepository = assetsRepository;
            _epRepository = epRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newAssetsType"></param>
        /// <param name="type"></param>
        /// <returns></returns
        public  int Save(AssetType newAssetsType)
        {
            int newId;
            const string sql = StoredProcedures.AssetType_CRUD;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", newAssetsType.Name);
                command.Parameters.AddWithValue("@CreatedBy", newAssetsType.CreatedBy);
                command.Parameters.AddWithValue("@TypeId", newAssetsType.TypeId);
                command.Parameters.AddWithValue("@AssetTypeCode", newAssetsType.AssetTypeCode);
                command.Parameters.AddWithValue("@IsActive", newAssetsType.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                //newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<AssetType> AssetTypeMaster(int userId)
        {
            var types = new List<AssetType>();
            const string sql = StoredProcedures.Get_AssetsType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    types = _sqlHelper.ConvertDataTable<AssetType>(dt);
            }
            return types;
        }

        public  List<AssetType> GetAssetMaster()
        {
            var types = new List<AssetType>();
            List<Assets> assets;
            const string sql = StoredProcedures.Get_AssetsTypeMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    types = _sqlHelper.ConvertDataTable<AssetType>(dt.Tables[0]);
                    assets = _sqlHelper.ConvertDataTable<Assets>(dt.Tables[1]);
                    foreach (var item in types)
                        item.Assets = assets.Where(x => x.AssetTypeId == item.TypeId).ToList();
                }
            }
            return types;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<AssetType> GetAssestsType(int userId)
        {
            var types = AssetTypeMaster(userId).Where(x => x.IsActive).ToList();
            foreach (var type in types)
            {
                type.Assets = _assetsRepository.GetAssets(userId).Where(m => m.AssetTypeId == type.TypeId).ToList();
            }
            return types;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="assetType"></param>
        /// <returns></returns>
        public  List<AssetType> GetAssetsByType(int userId, int assetType)
        {

            var types = new List<AssetType>();
            const string sql = StoredProcedures.Get_AssetsByType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (assetType > 0)
                    command.Parameters.Add(new SqlParameter("@typeid", assetType));

                command.Parameters.Add(new SqlParameter("@userId", userId));
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    types = _sqlHelper.ConvertDataTable<AssetType>(ds.Tables[0]);
                    var assets = _sqlHelper.ConvertDataTable<Assets>(ds.Tables[1]);
                    foreach (var type in types)
                    {
                        type.Assets = new List<Assets>();
                        type.Assets = assets.Where(x => x.AssetTypeId == type.TypeId).ToList();
                        foreach (var item in type.Assets)
                        {
                            item.EPdetails = new List<EPDetails>();
                            var epdetails = _epRepository.GetEpByAssets(item.AssetId);// assetGoalMapping.GroupBy(x => x.EPDetailId).Select(g => g.First()).ToList().Where(x => x.AssetId == item.AssetId).ToList();
                            foreach (var items in epdetails)
                            {
                                var epdetail = _epRepository.GetEpShortDescription(Convert.ToInt32(items.EPDetailId));
                                item.EPdetails.Add(epdetail);
                            }
                        }
                    }
                }
            }
            return types.Where(x => x.Assets.Count > 0).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public  List<AssetType> GetInternalAssetsByType(int userId)
        {
            var types = new List<AssetType>();
            const string sql = StoredProcedures.Get_InternalAssetsByType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@userId", userId));
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var assets = _sqlHelper.ConvertDataTable<Assets>(ds.Tables[1]);
                    types = _sqlHelper.ConvertDataTable<AssetType>(ds.Tables[0]);
                    var assetsubCategory = _sqlHelper.ConvertDataTable<AssetsSubCategory>(ds.Tables[2]);
                    var fireExtinguisherTypes = _sqlHelper.ConvertDataTable<FireExtinguisherTypes>(ds.Tables[3]);
                    foreach (var type in types)
                    {
                        type.Assets = assets.Where(x => x.AssetTypeId == type.TypeId).ToList();
                        foreach (var asset in type.Assets)
                        {
                            //asset.AssetFrequency = FireExtinguisherRepository.GetAssetInspFrequency(asset.AssetId);
                            asset.EPdetails = _epRepository.GetEpByAssets(asset.AssetId);
                            asset.AssetSubCategory = assetsubCategory.Where(x => x.AssetId == asset.AssetId).ToList();
                            foreach (var item in asset.AssetSubCategory)
                            {
                                item.FireExtinguisherTypes = fireExtinguisherTypes; /*.Where(x => x.AscId == item.AscId).ToList();*/
                            }
                        }
                    }
                }
          
            }
            return types;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //private  List<AssetType> GetAssetsByCategory(int userId, int type)
        //{
        //    var assetTypes = GetAssestsType(userId);

        //    List<Floor> floors = FloorRepository.GetAssetLocations();
        //    List<TFloorAssets> floorAssetsMain = new List<TFloorAssets>();
        //    floorAssetsMain = FloorAssetRepository.GetFloorAsset();
        //    List<TInspectionActivity> activity = InspectionActivityRepository.GetAssetInspectionActivity().ToList();
        //    List<Inspection> inspection = InspectionRepository.GetInspections().Where(x => x.IsCurrent).ToList();


        //    foreach (var assetType in assetTypes)
        //    {
        //        foreach (var assets in assetType.Assets)
        //        {
        //            List<EPDetails> epDetails = EpRepository.GetEpByAssets(assets.AssetId);

        //            assets.TFloorAssets = floorAssetsMain.Where(x => x.AssetId == assets.AssetId).ToList();
        //            foreach (TFloorAssets floorAsset in assets.TFloorAssets.Where(x => x.EPCount > 0))
        //            {
        //                if (floorAsset.FloorId.HasValue)
        //                    floorAsset.Floor = floors.SingleOrDefault(x => x.FloorId == floorAsset.FloorAssetsId);

        //                //                        floorAsset.EPDetails = new List<EPDetails>();

        //                foreach (var eps in epDetails)
        //                {
        //                    if (floorAsset.InspectionCount > 0)
        //                    {
        //                        eps.Inspections = inspection.Where(x => x.EPDetailId == eps.EPDetailId).ToList();
        //                        foreach (Inspection ins in eps.Inspections)
        //                        {
        //                            ins.TInspectionActivity = activity.Where(x => x.InspectionId == ins.InspectionId && x.FloorAssetId == floorAsset.FloorAssetsId).ToList(); //InspectionActivityRepository.GetInspectionActivity(ins.InspectionId, floorAsset.FloorAssetsId, Convert.ToInt32(HCF.Utility.Enums.Mode.ASSET)).ToList();
        //                        }
        //                    }
        //                    floorAsset.EPDetails.Add(eps);
        //                }
        //            }

        //        }
        //    }
        //    return assetTypes.Where(x => x.Assets.Count > 0).ToList();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epdetailId"></param>
        /// <returns></returns>
        internal  List<AssetType> GetEpAssetTypes(int epdetailId)
        {
            var assetType = new List<AssetType>();
            const string sql = StoredProcedures.Get_EPAssetTypes;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", epdetailId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt.Rows.Count > 0)
                    assetType = _sqlHelper.ConvertDataTable<AssetType>(dt);
                return assetType;
            }
        }


        internal  List<Assets> GetEpAssets(int epdetailId)
        {
            var assets = new List<Assets>();
            const string sql = StoredProcedures.Get_EPAssetTypes;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", epdetailId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds.Tables[0].Rows.Count > 0)
                    assets = _sqlHelper.ConvertDataTable<Assets>(ds.Tables[0]);

                foreach (var item in assets)
                {
                    item.Users = new List<UserProfile>();
                    var assetExpression = "AssetId = '" + item.AssetId + "'";
                    var AssetSortOrder = "FirstName ASC";
                    var drfoundRows = ds.Tables[1].Select(assetExpression, AssetSortOrder);
                    for (var i = 0; i < drfoundRows.Length; i++)
                    {
                        var user = new UserProfile
                        {
                            FirstName = Convert.ToString(drfoundRows[i]["FirstName"]),
                            LastName = Convert.ToString(drfoundRows[i]["LastName"]),
                            Email = Convert.ToString(drfoundRows[i]["Email"]),
                            UserId = Convert.ToInt32(drfoundRows[i]["UserId"])
                        };
                        item.Users.ToList().Add(user);
                    }
                }
                return assets;
            }
        }

        #region Offline Assets
        public  HttpResponseMessage GetOfflineAssets()
        {
            var objHttpResponseMessage = new HttpResponseMessage();
            const string sql = StoredProcedures.Get_OfflineAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var assetType = _sqlHelper.ConvertDataTable<AssetType>(ds.Tables[0]);
                    var assets = _sqlHelper.ConvertDataTable<Assets>(ds.Tables[1]);
                    var subCategory = _sqlHelper.ConvertDataTable<AssetsSubCategory>(ds.Tables[2]);
                    var assetSize = _sqlHelper.ConvertDataTable<FireExtinguisherTypes>(ds.Tables[3]);
                    var tfloorAssets = _sqlHelper.ConvertDataTable<TFloorAssets>(ds.Tables[4]);
                    var routeMaster = _sqlHelper.ConvertDataTable<RouteMaster>(ds.Tables[5]);
                    var stopsRouteMapping = _sqlHelper.ConvertDataTable<StopsRouteMapping>(ds.Tables[6]);
                    var stopMaster = _sqlHelper.ConvertDataTable<StopMaster>(ds.Tables[6]);
                    var assetInspFrequency = _sqlHelper.ConvertDataTable<AssetInspFrequency>(ds.Tables[7]);
                    var frequencies= _sqlHelper.ConvertDataTable<FrequencyMaster>(ds.Tables[7]);
                    var floors = _sqlHelper.ConvertDataTable<Floor>(ds.Tables[8]);
                    var buildings = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[8]).GroupBy(test => test.BuildingId).Select(grp => grp.First()).ToList();
                    var activities = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[9]);
                    var objManufactures = _sqlHelper.ConvertDataTable<Manufactures>(ds.Tables[10]);
                    var objAssetsSubCategory = _sqlHelper.ConvertDataTable<AssetsSubCategory>(ds.Tables[2]);
                    var objuserProfile = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[11]);
                    foreach (var item in assetType)
                    {
                        item.Assets = assets.Where(x => x.AssetTypeId == item.TypeId).ToList();
                        foreach (var asset in item.Assets)
                        {
                            asset.EPdetails = _epRepository.GetEpByAssets(asset.AssetId);
                            asset.AssetSubCategory = subCategory.Where(x => x.AssetId == asset.AssetId).ToList();
                            foreach (var size in asset.AssetSubCategory)
                            {
                                size.FireExtinguisherTypes = assetSize.Where(x => x.AscId == size.AscId).ToList();
                            }
                            asset.TFloorAssets = tfloorAssets.Where(x => x.AssetId == asset.AssetId).ToList();
                            foreach (var floorAssets in asset.TFloorAssets)
                            {
                                if (floorAssets.FloorId == null)
                                    floorAssets.FloorId = 0;
                                                                                             
                                if (floorAssets.ManufactureId.HasValue)
                                    floorAssets.Make = objManufactures.FirstOrDefault(x => x.ManufactureId == floorAssets.ManufactureId);

                                if (floorAssets.AscId.HasValue)
                                    floorAssets.AssetSubCategory = objAssetsSubCategory.FirstOrDefault(x => x.AscId == floorAssets.AscId);

                                floorAssets.TInspectionActivity = activities.Where(x => x.FloorAssetId == floorAssets.FloorAssetsId).ToList();
                                foreach (var activity in floorAssets.TInspectionActivity)
                                {
                                    activity.UserProfile = objuserProfile.FirstOrDefault(x => x.UserId == activity.CreatedBy);                                  
                                }
                            }
                        }
                    }
                    foreach (var route in routeMaster)
                    {
                        route.StopsRouteMapping = stopsRouteMapping.Where(x => x.RouteId == route.RouteId).ToList();
                        foreach (var stoproute in route.StopsRouteMapping)
                        {
                            stoproute.Stops = new StopMaster();
                            stoproute.Stops = stopMaster.FirstOrDefault(x => x.StopId == stoproute.StopId);
                            if (stoproute.Stops != null)
                            {
                                stoproute.Stops.Floor = new Floor();
                                stoproute.Stops.Floor =
                                    floors.FirstOrDefault(x => x.FloorId == stoproute.Stops.FloorId);
                                if (stoproute.Stops.Floor != null)
                                {
                                    stoproute.Stops.Floor.Building = new Buildings();
                                    stoproute.Stops.Floor.Building = buildings.FirstOrDefault(x =>
                                        x.BuildingId == stoproute.Stops.Floor.BuildingId);
                                }
                            }
                        }
                    }
                    foreach (var item in assetInspFrequency)
                    {
                        item.InspType = frequencies.FirstOrDefault(x => x.FrequencyId == item.FrequencyId);
                    }

                    objHttpResponseMessage.Success = true;
                    objHttpResponseMessage.Result = new Result { AssetType = assetType, Routes = routeMaster, AssetInspFrequency = assetInspFrequency };
                }
                return objHttpResponseMessage;
            }
        }

        #endregion
    }
}
