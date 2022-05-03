using HCF.BDO;
using HCF.BDO.Enums;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class FloorAssetRepository : IFloorAssetRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IEpRepository _epRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly IInsDetailRepository _insDetailRepository;
        public FloorAssetRepository(ISqlHelper sqlHelper, IEpRepository epRepository, IFloorRepository floorRepository, IInsDetailRepository insDetailRepository)
        {
            _epRepository = epRepository;
            _floorRepository = floorRepository;
            _insDetailRepository = insDetailRepository;
               _sqlHelper = sqlHelper;
        }

        #region Floor Assets


     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newFloorAsset"></param>
        /// <returns></returns>
        public  int AddFloorAssets(TFloorAssets newFloorAsset)
        {
            int newId;
            const string sql = StoredProcedures.Insert_FloorAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorId", newFloorAsset.FloorId);
                command.Parameters.AddWithValue("@AssetId", newFloorAsset.AssetId);
                command.Parameters.AddWithValue("@Name", newFloorAsset.Name);
                command.Parameters.AddWithValue("@CreatedBy", newFloorAsset.CreatedBy);
                command.Parameters.AddWithValue("@SerialNo", newFloorAsset.SerialNo);
                command.Parameters.AddWithValue("@Xcoordinate", newFloorAsset.Xcoordinate);
                command.Parameters.AddWithValue("@Ycoordinate", newFloorAsset.Ycoordinate);
                command.Parameters.AddWithValue("@AreaCode", newFloorAsset.AreaCode);
                command.Parameters.AddWithValue("@ZoomLevel", newFloorAsset.ZoomLevel);
                command.Parameters.AddWithValue("@Notes", newFloorAsset.Notes);
                command.Parameters.AddWithValue("@DeviceNo", newFloorAsset.DeviceNo);//assets #
                command.Parameters.AddWithValue("@NearBy", newFloorAsset.NearBy);
                command.Parameters.AddWithValue("@DateCreated", newFloorAsset.DateCreated);
                command.Parameters.AddWithValue("@DateUpdated", newFloorAsset.DateUpdated);
                command.Parameters.AddWithValue("@LocationCode", newFloorAsset.LocationCode);//assets #
                command.Parameters.AddWithValue("@BuildingCode", newFloorAsset.BuildingCode);
                command.Parameters.AddWithValue("@description", newFloorAsset.Description);
                command.Parameters.AddWithValue("@StatusCode", !string.IsNullOrEmpty(newFloorAsset.StatusCode) ? newFloorAsset.StatusCode : StatusCode.ACTIV.ToString());
                command.Parameters.AddWithValue("@TmsReference", newFloorAsset.TmsReference);
                command.Parameters.AddWithValue("@ManufactureId", newFloorAsset.ManufactureId);
                command.Parameters.AddWithValue("@AscId", newFloorAsset.AscId);
                command.Parameters.AddWithValue("@StopId", newFloorAsset.StopId);
                command.Parameters.AddWithValue("@FETypeId", newFloorAsset.FETypeId);
                command.Parameters.AddWithValue("@ModelNo", newFloorAsset.Model);
                command.Parameters.AddWithValue("@Rating", newFloorAsset.Rating);
                command.Parameters.AddWithValue("@WallRating", newFloorAsset.WallRating);
                command.Parameters.AddWithValue("@DoorRating", newFloorAsset.DoorRating);
                command.Parameters.AddWithValue("@FrameRating", newFloorAsset.FrameRating);
                command.Parameters.AddWithValue("@AssetTypeId", newFloorAsset.AssetTypeId);
                command.Parameters.AddWithValue("@AreaId", newFloorAsset.AreaId);
                command.Parameters.AddWithValue("@IsTrackingAsset", newFloorAsset.IsTrackingAsset);
                command.Parameters.AddWithValue("@AssetChangeDeviceType", newFloorAsset.AssetChangeDeviceType);
                command.Parameters.AddWithValue("@Path", newFloorAsset.Path);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAsset"></param>
        /// <returns></returns>
        public  bool UpdateFloorAssets(TFloorAssets floorAsset)
        {
            bool status;
            const string sql = StoredProcedures.Update_FloorAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorAssetsId", floorAsset.FloorAssetsId);
                command.Parameters.AddWithValue("@Name", floorAsset.Name);
                command.Parameters.AddWithValue("@ALTERdBy", floorAsset.CreatedBy);
                command.Parameters.AddWithValue("@Notes", floorAsset.Notes);
                command.Parameters.AddWithValue("@Model", floorAsset.Model);
                command.Parameters.AddWithValue("@NearBy", floorAsset.NearBy);
                command.Parameters.AddWithValue("@TmsReference", floorAsset.TmsReference);
                command.Parameters.AddWithValue("@SerialNo", floorAsset.SerialNo);
                command.Parameters.AddWithValue("@DeviceNo", floorAsset.DeviceNo);
                command.Parameters.AddWithValue("@FloorId", floorAsset.FloorId);
                command.Parameters.AddWithValue("@Description", floorAsset.Description);
                command.Parameters.AddWithValue("@StatusCode", floorAsset.StatusCode);
                command.Parameters.AddWithValue("@ManufactureId", floorAsset.ManufactureId);
                command.Parameters.AddWithValue("@Rating", floorAsset.Rating);
                command.Parameters.AddWithValue("@AssetId", floorAsset.AssetId);
                command.Parameters.AddWithValue("@WallRating", floorAsset.WallRating);
                command.Parameters.AddWithValue("@DoorRating", floorAsset.DoorRating);
                command.Parameters.AddWithValue("@AssetTypeId", floorAsset.AssetTypeId);
                command.Parameters.AddWithValue("@OnFloorPlan", floorAsset.OnFloorPlan);
                //command.Parameters.AddWithValue("@Model", floorAsset.Model);
                command.Parameters.AddWithValue("@StopId", floorAsset.StopId);
                command.Parameters.AddWithValue("@FrameRating", floorAsset.FrameRating);
                //command.Parameters.AddWithValue("@StopId", floorAsset.StopId);
                command.Parameters.AddWithValue("@AreaId", floorAsset.AreaId);
                command.Parameters.AddWithValue("@AssetChangeDeviceType", floorAsset.AssetChangeDeviceType);
                command.Parameters.AddWithValue("@Path", floorAsset.Path);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public  bool UpdateTfloorassetstatus(int floorId, int assetId, string statusCode)
        {
            bool status;
            const string sql = StoredProcedures.UpdateTfloorassetstatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AssetId", assetId);
                command.Parameters.AddWithValue("@StatusCode", statusCode);
                command.Parameters.AddWithValue("@FloorId", floorId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public  bool CheckExistingAssets(string Deviceno)
        {
            bool status;
            const string sql = StoredProcedures.Check_ExistingAssetNo;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DeviceNo", Deviceno);
                status = Convert.ToBoolean(_sqlHelper.GetScalarValue(command));
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <returns></returns>
        public  bool RemoveFloorAssetsFromCurrentLocation(int floorAssetId)
        {
            bool status;
            const string sql = StoredProcedures.Remove_FloorAssetsFromLocation;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newFloorAsset"></param>
        /// <returns></returns>
        public  bool AddFloorAssetsToStop(TFloorAssets newFloorAsset)
        {
            bool status;
            const string sql = StoredProcedures.Insert_FloorAssetsToLocation;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetId", newFloorAsset.FloorAssetsId);
                command.Parameters.AddWithValue("@StopId", newFloorAsset.StopId);
                command.Parameters.AddWithValue("@assetsid", newFloorAsset.AssetId != 0 ? newFloorAsset.AssetId : null);
                command.Parameters.AddWithValue("@CreatedBy", newFloorAsset.CreatedBy);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <returns></returns>
        public  TFloorAssets GetFloorAsset(int floorAssetId)
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_TFloorAssetsById;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = GetFloorAssetsFromDataSet(ds);
            }
            return lists.FirstOrDefault();
        }

        public  TFloorAssets GetFloorAssetByTmsAssetId(int tmsAssetId)
        {
            var lists = new TFloorAssets();
            const string sql = StoredProcedures.Get_FloorAssetByTmsAssetId;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tmsAssetId", tmsAssetId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<TFloorAssets>(ds.Tables[0]).ToList().FirstOrDefault();
                    var seps = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[1]).ToList();
                    if (lists != null)
                    {
                        lists.Assets = new Assets();
                        lists.Assets.EPdetails = seps;

                        foreach (DataRow item in ds.Tables[2].Rows)
                        {
                            WorkOrder workOrder = new WorkOrder();
                            workOrder.IssueId = Convert.ToInt32(item["IssueId"].ToString());
                            workOrder.WorkOrderId = Convert.ToInt32(item["WorkOrderId"].ToString());
                            workOrder.WorkOrderNumber = item["WorkOrderNumber"].ToString();

                            if (!string.IsNullOrEmpty(item["ActivityId"].ToString()))
                            {
                                TInspectionActivity inspectionActivity = new TInspectionActivity();
                                inspectionActivity.EPDetailId = Convert.ToInt32(item["EpDetailId"].ToString());
                                inspectionActivity.ActivityId = Guid.Parse(item["ActivityId"].ToString());
                                inspectionActivity.FloorAssetId = Convert.ToInt32(item["FloorAssetId"].ToString());
                                workOrder.TInspectionActivity = inspectionActivity;
                            }
                            lists.WorkOrders.Add(workOrder);

                        }
                    }

                }
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<TFloorAssets> GetFloorAsset()
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_TFloorAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = BindFloorAssets(ds.Tables[0]);
            }
            return lists.Where(x => x.IsDeleted == false).ToList();
        }

        public  int UpdateInspectionDate(int floorAssetId, DateTime lastPmDate, string referenceNo)
        {
            const string sql = StoredProcedures.Update_First_InspectionDate;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorAssetsId", floorAssetId);
                command.Parameters.AddWithValue("@FirstInspectionDate", lastPmDate);
                command.Parameters.AddWithValue("@ReferenceNo", referenceNo);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return floorAssetId;
        }

        public  TFloorAssets GetFeFloorAssetInspActivity(int floorAssetId, int userId)
        {
            var tfloorAssets = new TFloorAssets();
            const string sql = StoredProcedures.Get_FEFloorAssetInspActivity;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    //List<TFloorAssets> lists = new List<TFloorAssets>();
                    tfloorAssets = BindFloorAssets(ds.Tables[0]).FirstOrDefault();
                    tfloorAssets.EPDetails = _epRepository.GetEpByAssets(tfloorAssets.AssetId.Value);
                    var activity = new List<TInspectionActivity>();
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var data = new TInspectionActivity
                        {
                            Comment = row["Comment"].ToString(),
                            FloorAssetId = Convert.ToInt32(row["FloorAssetId"].ToString()),
                            ActivityId = Guid.Parse(row["ActivityId"].ToString()),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                            CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                            Status = Convert.ToInt32(row["Status"].ToString()),
                            SubStatus = row["SubStatus"].ToString(),
                            IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString()),
                            ActivityInspectionDate = Convert.ToDateTime(row["ActivityInspectionDate"].ToString()),
                            FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                            Frequency = new FrequencyMaster { DisplayName = row["DisplayName"].ToString() }
                            
                        };
                        data.UserProfile = new UserProfile();
                        data.UserProfile.FirstName = row["FirstName"].ToString();
                        data.UserProfile.LastName = row["LastName"].ToString();
                        data.UserProfile.Email = row["Email"].ToString();
                        data.UserProfile.UserId = Convert.ToInt32(row["UserId"].ToString());
                        data.InspStatus = new InspStatus { StatusName = row["StatusName"].ToString() };
                        data.InspResult = new InspResult { ResultName = row["ResultName"].ToString() };
                        activity.Add(data);
                    }

                    if (tfloorAssets != null) tfloorAssets.TInspectionActivity = activity;
                }
            }
            return tfloorAssets;
        }

        //public  int AddAssetsToStop(int floorAssetsId, int stopId)
        //{
        //    int newId;
        //    const string sql = StoredProcedures.Insert_FloorAssetsToLocation;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@floorAssetsId", floorAssetsId);
        //        command.Parameters.AddWithValue("@stopId", stopId);               
        //        command.Parameters.Add("@newId", SqlDbType.Int);
        //        command.Parameters["@newId"].Direction = ParameterDirection.Output;
        //        newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
        //    }
        //    return newId;
        //}

        public  List<Buildings> GetAssetsBuilding(int assetId)
        {
            List<Buildings> lists;
            const string sql = StoredProcedures.Get_AssetsBuilding;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId);
                using (var dt = _sqlHelper.GetDataSet(command))
                {
                    lists = _sqlHelper.ConvertDataTable<Buildings>(dt.Tables[0]).GroupBy(x => x.BuildingId).Select(x => x.First()).ToList();
                    var floors = _sqlHelper.ConvertDataTable<Floor>(dt.Tables[0]);
                    foreach (var item in lists)
                        item.Floor = floors.Where(x => x.BuildingId == item.BuildingId).ToList();
                }

            }
            return lists;
        }

        /// <summary>
        /// Get Floor Asset Short Description datrowImp
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <returns></returns>
        public  TFloorAssets GetFloorAssetDescription(int floorAssetId)
        {
            var floorAsset = new TFloorAssets();
            const string sql = StoredProcedures.Get_TFloorAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                        floorAsset = BindFloorAsset(row);

                    var assetsUser = _sqlHelper.ConvertDataTable<UserProfile>(dt.Tables[2]);
                    if (assetsUser.Count > 0 && floorAsset != null)
                        floorAsset.Assets.Users = assetsUser;

                    try
                    {
                        foreach (DataRow item in dt.Tables[3].Rows)
                        {
                            floorAsset.OpenIlsmsCount = Convert.ToInt32(item["OpenIlsmsCount"].ToString());
                            floorAsset.OpenWorkOrdersCount = Convert.ToInt32(item["OpenWorkOrdersCount"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        //Utility.ErrorLog.LogError(ex, "GetFloorAssetDescription", "DAL");
                    }
                }

            }
            return floorAsset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<TFloorAssets> GetTFloorAssets()
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_TFloorAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = GetFloorAssetsFromDataSet(ds);
            }
            return lists;
        }

        public  List<TFloorAssets> GetTFloorAssetslist()
        {
            var lists = new List<TFloorAssets>();
            string sql = StoredProcedures.Get_TFloorAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var inspectionactivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var floorAssets = new TFloorAssets
                        {
                            Name = row["Name"].ToString(),
                            SerialNo = row["SerialNo"].ToString(),
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            DeviceNo = row["DeviceNo"].ToString(),
                            FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString()),
                            Assets = new Assets
                            {
                                Name = row["AssetsName"].ToString(),
                                AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                                IconPath = row["IconPath"].ToString(),
                                IsStepsOnReport = Convert.ToBoolean(row["IsStepsOnReport"].ToString()),
                                AssetTypeId = Convert.ToInt32(row["AssetTypeId"].ToString()),
                                //IsRouteInsp = Convert.ToBoolean(row["IsRouteInsp"].ToString()),
                                AssetType = new AssetType
                                {
                                    Name = row["AssetTypeName"].ToString(),
                                    TypeId = Convert.ToInt32(row["AssetTypeId"].ToString()),
                                },
                            }
                        };
                        if ((!string.IsNullOrEmpty(row["FloorId"].ToString())) && (!string.IsNullOrEmpty(row["BuildingID"].ToString())))
                        {
                            floorAssets.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                            floorAssets.BuildingID = Convert.ToInt32(row["BuildingID"].ToString());
                            floorAssets.Floor = new Floor
                            {
                                FloorName = row["FloorName"].ToString(),
                                FloorId = floorAssets.FloorId.Value,
                                FloorCode = row["FloorCode"].ToString(),
                                BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString()) ? Convert.ToInt32(row["BuildingId"].ToString()) : 0,
                                Building = new Buildings
                                {
                                    BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString()) ? Convert.ToInt32(row["BuildingId"].ToString()) : 0,
                                    BuildingName = row["BuildingName"].ToString(),
                                    BuildingCode = row["BuildingCode"].ToString()
                                }
                            };

                            if (row.Table.Columns.Contains("FloorCode"))
                                floorAssets.Floor.FloorCode = row["FloorCode"].ToString();
                        }
                        else
                        {
                            floorAssets.Floor = new Floor();
                            floorAssets.Floor.Building = new Buildings();
                        }
                        floorAssets.OnFloorPlan = !string.IsNullOrEmpty(row["OnFloorPlan"].ToString()) && Convert.ToBoolean(row["OnFloorPlan"].ToString());
                        if (row.Table.Columns.Contains("StatusCode"))
                            floorAssets.StatusCode = row["StatusCode"].ToString();
                        floorAssets.TInspectionActivity = new List<TInspectionActivity>();
                        floorAssets.TInspectionActivity = inspectionactivity.Where(x =>
                            x.FloorAssetId == Convert.ToInt32(row["FloorAssetsId"].ToString())).ToList();
                        lists.Add(floorAssets);
                    }
                }
            }
            return lists;
        }

        public  List<TFloorAssets> GetTFloorAssets(int assetId, int routId)
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_AssetsBarCode;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId > 0 ? assetId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@routId", routId > 0 ? routId : (object)DBNull.Value);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<TFloorAssets>(ds.Tables[0]);
            }
            return lists;
        }


        public  List<TFloorAssets> GetTFloorAssetByAssets(string ids)
        {
            var lists = new List<TFloorAssets>();
            var sql = StoredProcedures.GetT_FloorAssetByAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ids", ids);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = GetFloorAssetsFromDataSet(ds);
            }
            return lists;
        }


        public  List<TFloorAssets> GetFloorAssetByAsset(Request objRequest, string ids)
        {
            var lists = new List<TFloorAssets>();
            var sql = StoredProcedures.GetT_FloorAssetByAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ids", ids);
                command.Parameters.AddWithValue("@pageSize", null);
                command.Parameters.AddWithValue("@pageIndex", null);
                command.CommandTimeout = 1000;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = GetFloorAssetsFromDataSet(ds);
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<TFloorAssets> GetFloorAssets()
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_FloorAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = GetFloorAssetsFromDataSet(ds);
            }
            return lists;
        }

        public  List<TFloorAssets> GetAssetsByPrefix(string prefix)
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_AssetsByPrefix;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@prefix", prefix);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        TFloorAssets floorAssets = new TFloorAssets();
                        floorAssets.FloorAssetsId = Convert.ToInt32(item["FloorAssetsId"].ToString());
                        floorAssets.DeviceNo = item["DeviceNo"].ToString();
                        floorAssets.SerialNo = item["SerialNo"].ToString();
                        floorAssets.Assets = new Assets();
                        floorAssets.Assets.Name = item["AssetName"].ToString();
                        if (!string.IsNullOrEmpty(item["FloorId"].ToString()))
                        {
                            floorAssets.FloorId = Convert.ToInt32(item["FloorId"].ToString());
                            floorAssets.Floor = new Floor
                            {
                                BuildingId = Convert.ToInt32(item["BuildingId"].ToString()),
                                FloorName = item["FloorName"].ToString(),
                                Building = new Buildings
                                {
                                    BuildingName = item["BuildingName"].ToString()
                                }
                            };
                        }
                        lists.Add(floorAssets);
                    }
            }
            return lists;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<TFloorAssets> View_getFloorAssets(int? assetId, int? buildingId, int? floorId, int? groupId)
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_View_FloorAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId.HasValue && assetId.Value > 0 ? assetId : null);
                command.Parameters.AddWithValue("@buildingId", buildingId);
                command.Parameters.AddWithValue("@floorId", floorId);
                command.Parameters.AddWithValue("@groupId", groupId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var list = BindBasicFloorAsset(row);
                        //var list = BindFloorAsset(row);
                        lists.Add(list);
                    }
                }
            }
            return lists;
        }

        private  TFloorAssets BindBasicFloorAsset(DataRow row)
        {
            var floorAssets = new TFloorAssets
            {
                Name = row["Name"].ToString(),
                SerialNo = row["SerialNo"].ToString(),
                AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                DeviceNo = row["DeviceNo"].ToString(),
                FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString()),
                NearBy = row["NearBy"].ToString(),
                StatusCode = row["StatusCode"].ToString(),
                Assets = new Assets
                {
                    Name = row["AssetsName"].ToString(),
                    AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                    IconPath = row["IconPath"].ToString(),
                    IsStepsOnReport = Convert.ToBoolean(row["IsStepsOnReport"].ToString()),
                    AssetTypeId = Convert.ToInt32(row["AssetTypeId"].ToString()),
                    AssetType = new AssetType
                    {
                        Name = row["AssetTypeName"].ToString(),
                        TypeId = Convert.ToInt32(row["AssetTypeId"].ToString()),
                    },
                }
            };
            if ((!string.IsNullOrEmpty(row["FloorId"].ToString())) &&
                (!string.IsNullOrEmpty(row["BuildingID"].ToString())))
            {
                floorAssets.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                floorAssets.BuildingID = Convert.ToInt32(row["BuildingID"].ToString());
                floorAssets.Floor = new Floor
                {
                    FloorName = row["FloorName"].ToString(),
                    FloorId = floorAssets.FloorId.Value,
                    FloorCode = row["FloorCode"].ToString(),
                    BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString())
                        ? Convert.ToInt32(row["BuildingId"].ToString())
                        : 0,
                    Building = new Buildings
                    {
                        BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString())
                            ? Convert.ToInt32(row["BuildingId"].ToString())
                            : 0,
                        BuildingName = row["BuildingName"].ToString(),
                        BuildingCode = row["BuildingCode"].ToString()
                    }
                };

                if (row.Table.Columns.Contains("FloorCode"))
                    floorAssets.Floor.FloorCode = row["FloorCode"].ToString();
            }
            else
            {
                floorAssets.Floor = new Floor { Building = new Buildings() };
            }
            if (!string.IsNullOrEmpty(row["TmsReference"].ToString()))
                floorAssets.TmsReference = row["TmsReference"].ToString();
                return floorAssets;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private  List<TFloorAssets> GetFloorAssetsFromDataSet(DataSet ds)
        {
            var lists = new List<TFloorAssets>();
            var inspectionActivities = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var list = BindFloorAsset(row);
                list.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                list.AssetTypeId = Convert.ToInt32(row["AssetTypeId"].ToString());
                list.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                //list.StopId = Convert.ToInt32(row["StopId"].ToString());
                //list.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                list.Description = row["Description"].ToString();
                list.InspectionCount = Convert.ToInt32(row["InspectionCount"].ToString());
                list.IsDeleted = Convert.ToBoolean(row["IsDeleted"].ToString());
                list.EPCount = Convert.ToInt32(row["EPCount"].ToString());
                list.Xcoordinate = !string.IsNullOrEmpty(row["Xcoordinate"].ToString()) ? row["Xcoordinate"].ToString() : "0";

                list.Ycoordinate = !string.IsNullOrEmpty(row["Ycoordinate"].ToString()) ? row["Ycoordinate"].ToString() : "0";

                list.ZoomLevel = !string.IsNullOrEmpty(row["ZoomLevel"].ToString()) ? row["ZoomLevel"].ToString() : "0";

                list.OnFloorPlan = !string.IsNullOrEmpty(row["OnFloorPlan"].ToString()) && Convert.ToBoolean(row["OnFloorPlan"].ToString());

                if (ds.Tables[0].Columns.Contains("StopId"))
                {
                    list.StopId = !string.IsNullOrEmpty(row["StopId"].ToString()) ? Convert.ToInt32(row["StopId"].ToString()) : 0;
                }

                list.TInspectionActivity = new List<TInspectionActivity>();
                list.TInspectionActivity = inspectionActivities.Where(x =>
                    x.FloorAssetId == Convert.ToInt32(row["FloorAssetsId"].ToString())).ToList();

                lists.Add(list);
            }
            return lists.OrderByDescending(m => m.CreatedDate).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public  List<TFloorAssets> GetFloorAssets(int floorId)
        {
            var lists = new List<TFloorAssets>();
            var floor = _floorRepository.GetFloor(floorId);
            if (floor != null)
                lists = GetFloorAsset(floor.FloorPlanId);
            return lists;
        }


        public  List<TFloorAssets> GetFloorAssets(Guid floorPlanId)
        {
            return GetFloorAsset(floorPlanId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorPlanId"></param>
        /// <returns></returns>
        public  List<TFloorAssets> GetFloorAsset(Guid floorPlanId)
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_AssetsByFloorPlan;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorPlanId", floorPlanId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = GetFloorAssetsFromDataSet(ds);

            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<TFloorAssets> GetAssetsWithoutFloor()
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_TFloorAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var expression = "FloorId is null";

                    // Use the Select method to find all rows matching the filter.
                    var foundRows = ds.Tables[0].Select(expression);

                    //DataTable assetsWithourFloor = ds.Tables[0];
                    if (foundRows.Any())
                    {
                        var nds = new DataSet();
                        var assetsWithoutFloor = foundRows.CopyToDataTable();
                        assetsWithoutFloor.TableName = "assetsWithourFloor";
                        nds.Tables.Add(assetsWithoutFloor);
                        var activity = ds.Tables[1].Copy();
                        activity.TableName = "dtactivity";
                        nds.Tables.Add(activity);
                        lists = GetFloorAssetsFromDataSet(nds);
                    }
                }
            }
            return lists.Where(x => x.IsDeleted == false).ToList();
            // List<TFloorAssets> lists = GetTFloorAssets().Where(x => x.FloorId == null && x.IsDeleted == false).ToList();
            //  return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>       

        public  List<TFloorAssets> GetFloorAssetsbyBarcode(string barcode)
        {
            List<TFloorAssets> lists;
            const string sql = StoredProcedures.Get_FloorAssetsbyBarcode;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@barcode", barcode);
                var ds = _sqlHelper.GetDataSet(command);
                lists = GetFloorAssetsFromDataSet(ds);
                foreach (var item in lists)
                {
                    if (item.AssetId.HasValue)
                    {
                        item.EPDetails = new List<EPDetails>();
                        item.EPDetails = _epRepository.GetEpByAssets(item.AssetId.Value).OrderByDescending(x => x.CreatedDate).ToList();
                    }
                }
            }
            return lists;
        }
        public List<TFloorAssets> GetFloorAssetsbyBarcodeSearch(string barcode, int? IsAssetDevice=0)
        {
            List<TFloorAssets> lists;
            const string sql = StoredProcedures.Get_FloorAssetsbySearch;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@barcode", barcode);
                command.Parameters.AddWithValue("@IsAssetDevice", IsAssetDevice);
                var ds = _sqlHelper.GetDataSet(command);
                lists = GetFloorAssetsFromDataSet(ds);
              
            }
            return lists;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>




        public  List<TFloorAssets> InventoryAssetsReport(string ids, string buildingId, string floorId, string fromdate, string todate, string status = null)
        {
            var floorAssets = GetTFloorAssetByAssets(ids);

            if (!string.IsNullOrEmpty(buildingId) && Convert.ToInt32(buildingId) > 0)
            {
                floorAssets = floorAssets.Where(x => x.Floor.BuildingId == Convert.ToInt32(buildingId)).ToList();
            }
            if (!string.IsNullOrEmpty(floorId) && Convert.ToInt32(floorId) > 0)
            {
                floorAssets = floorAssets.Where(x => x.FloorId == Convert.ToInt32(floorId)).ToList();
            }
            if (!string.IsNullOrEmpty(status))
            {
                floorAssets = floorAssets.Where(x => x.StatusCode == status).ToList();
            }            
            return floorAssets;
        }
       


      
        /// <summary>
        /// datrowImp
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="ascIds"></param>
        /// <returns></returns>
        public  List<TFloorAssets> GetFloorAssetsForInspections(string assetId, string ascIds)
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_FloorAssetsForInspections;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@AssetId", assetId);
                command.Parameters.AddWithValue("@ascIds", ascIds);
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        var list = BindFloorAsset(row);
                        if (!string.IsNullOrEmpty(row["AscId"].ToString()))
                        {
                            list.AssetSubCategory = new AssetsSubCategory
                            {
                                AscId = Convert.ToInt32(row["AscId"].ToString()),
                                AscName = row["ASCName"].ToString()
                            };
                            list.AscId = Convert.ToInt32(row["AscId"].ToString());
                        }

                        list.IsInspReady = Convert.ToInt32(row["IsInspReady"].ToString());
                        list.EPDetails = new List<EPDetails>();
                        string expression;
                        string sortOrder;

                        expression = "AssetId = '" + list.AssetId + "'";// "AssetId =" + list.AssetId;

                        sortOrder = "name DESC";

                        var foundRows = dt.Tables[1].Select(expression, sortOrder);
                        foreach (var epRow in foundRows)
                        {
                            var ep = new EPDetails
                            {
                                EPDetailId = Convert.ToInt32(epRow["EPDetailId"].ToString()),
                                EPNumber = epRow["EPNumber"].ToString(),
                                Description = epRow["Description"].ToString(),
                                IsAssetSteps = Convert.ToInt32(epRow["IsAssetSteps"].ToString()),
                                Standard = new Standard
                                {
                                    StDescID = Convert.ToInt32(epRow["StDescID"].ToString()),
                                    TJCStandard = epRow["TJCStandard"].ToString(),
                                    TJCDescription = epRow["TJCDescription"].ToString()
                                }
                            };
                            ep.Inspections = GetInspection(ep.EPDetailId, list.FloorAssetsId, list.Assets.IsStepsOnReport, dt.Tables[2]);
                            list.EPDetails.Add(ep);

                        }
                        lists.Add(list);
                    }
                }
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <param name="floorAssetsId"></param>
        /// <param name="isStepsOnReport"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public  List<Inspection> GetInspection(int epDetailId, int floorAssetsId, bool isStepsOnReport, DataTable dt)
        {
            var users = _sqlHelper.ConvertDataTable<UserProfile>(dt);
            var inspection = _sqlHelper.ConvertDataTable<Inspection>(dt);
            var inspectionActivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(dt);
            var inspections = inspection.Where(x => x.EPDetailId == epDetailId).ToList();
            foreach (var inspectionCurrent in inspections)
            {
                var activities = inspectionActivity.Where(x => x.FloorAssetId == floorAssetsId && x.InspectionId == inspectionCurrent.InspectionId).ToList();
                inspectionCurrent.TInspectionActivity = activities.GroupBy(test => test.ActivityId).Select(grp => grp.First()).ToList();
                foreach (var activity in inspectionCurrent.TInspectionActivity)
                {
                    if (isStepsOnReport)
                        activity.TInspectionDetail = _insDetailRepository.GetTInspectionDetails(activity.ActivityId);
                    activity.UserProfile = users.FirstOrDefault(x => x.UserId == activity.CreatedBy);
                }
            }

            return inspections;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetsId"></param>
        /// <param name="ePdetailId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public  TFloorAssets GetAssetEpInsHistory(int floorAssetsId, int ePdetailId, int userId)
        {
            var floorAssets = GetFloorAsset(floorAssetsId);            
            return floorAssets;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetsId"></param>
        /// <param name="epDetailId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public  TFloorAssets GetFloorAssetInspectionActivity(int floorAssetsId, int epDetailId, int userId)
        {
            var floorAssets = GetFloorAsset(floorAssetsId);           
            return floorAssets;
        }

        /// <summary>
        /// datrowImp
        /// </summary>
        /// <param name="assetTypeId"></param>
        /// <param name="userId"></param>
        /// <param name="floorAssetId"></param>
        /// <returns></returns>
        public  List<TFloorAssets> GetFloorAssetsByAssetType(int? assetTypeId, int userId, int? floorAssetId)
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_FloorAssetsByAssetTye;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@AssetTypeId", assetTypeId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        //var list = BindFloorAsset(row);
                        var list = BindBasicFloorAsset(row);
                        list.EPDetails = new List<EPDetails>();
                        if (!string.IsNullOrEmpty(row["EPDetailId"].ToString()))
                        {
                            var ep = new EPDetails
                            {
                                EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                                EPNumber = row["EPNumber"].ToString(),
                                Description = row["Description"].ToString(),
                                Frequency = new FrequencyMaster { DisplayName = row["FrequencyName"].ToString() },
                                Standard = new Standard
                                {
                                    StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                    TJCStandard = row["TJCStandard"].ToString(),
                                }
                            };
                            list.EPDetails.Add(ep);
                        }
                        lists.Add(list);
                    }
                }
            }
            return lists;
        }


        public  List<TFloorAssets> GetFloorAssetsEps(int? assetTypeId, int userId, int? floorAssetId)
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_FloorAssetsByAssetTye;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@AssetTypeId", assetTypeId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        var list = BindBasicFloorAsset(row);
                        list.InspectionCount = Convert.ToInt32(row["InspectionCount"].ToString());
                        list.EPDetails = new List<EPDetails>();
                        if (!string.IsNullOrEmpty(row["EPDetailId"].ToString()))
                        {
                            var ep = new EPDetails
                            {
                                EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                                EPNumber = row["EPNumber"].ToString(),
                                Description = row["Description"].ToString(),
                                Frequency = new FrequencyMaster { DisplayName = row["FrequencyName"].ToString() },
                                Standard = new Standard
                                {
                                    StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                    TJCStandard = row["TJCStandard"].ToString(),
                                }
                            };
                            // row["FrequencyName"].ToString();
                            list.EPDetails.Add(ep);
                        }
                        foreach (var ep in list.EPDetails)
                        {
                            if (!string.IsNullOrEmpty(row["LastInspectionDate"].ToString()))
                            {
                                ep.Inspections = new List<Inspection>();
                                var ins = new Inspection
                                {
                                    EPDetailId = ep.EPDetailId,
                                    IsCurrent = true,
                                    TInspectionActivity = new List<TInspectionActivity>()
                                };

                                var insActivity = new TInspectionActivity
                                {
                                    ActivityInspectionDate =
                                        Convert.ToDateTime(row["LastInspectionDate"].ToString())
                                };
                                if (!string.IsNullOrEmpty(row["FixedDueDate"].ToString()))
                                {
                                    insActivity.DueDate = Convert.ToDateTime(row["FixedDueDate"].ToString());
                                }
                                insActivity.FloorAssetId = list.FloorAssetsId;
                                insActivity.IsCurrent = true;
                                ins.TInspectionActivity.Add(insActivity);
                                ep.Inspections.Add(ins);
                            }
                        }
                        lists.Add(list);
                    }
                }
            }
            return lists;
        }

        #endregion

        #region Extension Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public  int InspectionCount(int floorAssetId, int epDetailId)
        {
            int count;
            const string sql = StoredProcedures.Get_InspctionCount;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@epDetailId", epDetailId);
                count = (int)_sqlHelper.GetScalarValue(command);
            }
            return count;
        }

        #endregion

        #region Common Method

        public  List<TFloorAssets> BindFloorAssets(DataTable dt)
        {
            var lsttfloorAssets = new List<TFloorAssets>();
            var inspectionGroup = new List<InspectionGroup>(); //InspectionRepository.GetInspectionGroup();
            foreach (DataRow item in dt.Rows)
            {
                var objtfloorAssets = BindFloorAsset(item, inspectionGroup);
                lsttfloorAssets.Add(objtfloorAssets);
            }
            return lsttfloorAssets;
        }

        public  TFloorAssets BindFloorAsset(DataRow row, List<InspectionGroup> inspectiongroup = null)
        {
            var floorAssets = new TFloorAssets
            {
                Name = row["Name"].ToString(),
                SerialNo = row["SerialNo"].ToString(),
                AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                DeviceNo = row["DeviceNo"].ToString(),
                FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString()),
                Assets = new Assets
                {
                    Name = row["AssetsName"].ToString(),
                    AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                    IconPath = row["IconPath"].ToString(),
                    IsStepsOnReport = Convert.ToBoolean(row["IsStepsOnReport"].ToString()),
                    AssetTypeId = Convert.ToInt32(row["AssetTypeId"].ToString()),
                    AssetType = new AssetType
                    {
                        Name = row["AssetTypeName"].ToString(),
                        TypeId = Convert.ToInt32(row["AssetTypeId"].ToString()),
                    },
                }
            };

            if (row.Table.Columns.Contains("IsRouteInsp"))
                floorAssets.Assets.IsRouteInsp = Convert.ToBoolean(row["IsRouteInsp"].ToString());

            if (row.Table.Columns.Contains("IsTrackingAsset") && !string.IsNullOrEmpty(row["IsTrackingAsset"].ToString()))
                floorAssets.IsTrackingAsset = Convert.ToBoolean(row["IsTrackingAsset"].ToString());

            if (row.Table.Columns.Contains("AreaId") && !string.IsNullOrEmpty(row["AreaId"].ToString()))
            {
                floorAssets.AreaId = Convert.ToInt32(row["AreaId"].ToString());
            }


            if (row.Table.Columns.Contains("Model"))
                floorAssets.Model = row["Model"].ToString();

            if (row.Table.Columns.Contains("Description"))
                floorAssets.Description = row["Description"].ToString();

            if ((!string.IsNullOrEmpty(row["FloorId"].ToString())) &&
                (!string.IsNullOrEmpty(row["BuildingID"].ToString())))
            {
                floorAssets.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                floorAssets.BuildingID = Convert.ToInt32(row["BuildingID"].ToString());
                floorAssets.Floor = new Floor
                {
                    FloorName = row["FloorName"].ToString(),
                    FloorId = floorAssets.FloorId.Value,
                    FloorCode = row["FloorCode"].ToString(),
                    BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString())
                        ? Convert.ToInt32(row["BuildingId"].ToString())
                        : 0,
                    Building = new Buildings
                    {
                        BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString())
                            ? Convert.ToInt32(row["BuildingId"].ToString())
                            : 0,
                        BuildingName = row["BuildingName"].ToString(),
                        BuildingCode = row["BuildingCode"].ToString()
                    }
                };

                if (row.Table.Columns.Contains("FloorCode"))
                    floorAssets.Floor.FloorCode = row["FloorCode"].ToString();
            }
            else
            {
                floorAssets.Floor = new Floor { Building = new Buildings() };
            }

            if (row.Table.Columns.Contains("SiteCode"))
                floorAssets.Floor.Building.SiteCode = row["SiteCode"].ToString();

            if (row.Table.Columns.Contains("Notes"))
                floorAssets.Notes = row["Notes"].ToString();

            if (row.Table.Columns.Contains("Rating"))
                floorAssets.Rating = row["Rating"].ToString();

            if (row.Table.Columns.Contains("IsDeleted"))
                floorAssets.IsDeleted = Convert.ToBoolean(row["IsDeleted"].ToString());

            if (row.Table.Columns.Contains("TmsReference"))
                floorAssets.TmsReference = row["TmsReference"].ToString();

            if (row.Table.Columns.Contains("NearBy"))
                floorAssets.NearBy = row["NearBy"].ToString();

            if (row.Table.Columns.Contains("OnFloorPlan"))
            {
                if (!string.IsNullOrEmpty(row["OnFloorPlan"].ToString()))
                {
                    floorAssets.OnFloorPlan = Convert.ToBoolean(row["OnFloorPlan"].ToString());
                }
            }

            if (row.Table.Columns.Contains("StatusCode"))
                floorAssets.StatusCode = row["StatusCode"].ToString();

            if (row.Table.Columns.Contains("Source"))
            {
                if (!string.IsNullOrEmpty(row["Source"].ToString()))
                {
                    floorAssets.Source = Convert.ToInt32(row["Source"].ToString());
                }
            }

            if (row.Table.Columns.Contains("FirstInspectionDate"))
            {
                if (!string.IsNullOrEmpty(row["FirstInspectionDate"].ToString()))
                {
                    floorAssets.FirstInspectionDate = DateTime.Parse(row["FirstInspectionDate"].ToString());
                }
            }

            if (row.Table.Columns.Contains("WallRating"))
            {
                floorAssets.WallRating = row["WallRating"].ToString();
                floorAssets.DoorRating = row["DoorRating"].ToString();
                floorAssets.FrameRating = row["FrameRating"].ToString();
            }



            floorAssets.InspectionGroupId = 1;
            if (row.Table.Columns.Contains("EPCount"))
                floorAssets.EPCount = Convert.ToInt32(row["EPCount"].ToString());

            floorAssets.AssetSubCategory = new AssetsSubCategory();
            if (!string.IsNullOrEmpty(row["AscId"].ToString()))
            {
                floorAssets.AscId = Convert.ToInt32(row["AscId"].ToString());
                if (row.Table.Columns.Contains("DeviceType"))
                {
                    floorAssets.AssetSubCategory.AscName = row["DeviceType"].ToString();
                }
            }
            floorAssets.Make = new Manufactures();
            if (row.Table.Columns.Contains("ManufactureId"))
            {
                if (!string.IsNullOrEmpty(row["ManufactureId"].ToString()))
                {
                    floorAssets.ManufactureId = Convert.ToInt32(row["ManufactureId"].ToString());
                    floorAssets.Make.ManufactureId = floorAssets.ManufactureId.Value;
                    if (row.Table.Columns.Contains("ManufactureName"))
                    {
                        floorAssets.Make.ManufactureName = row["ManufactureName"].ToString();
                    }
                }
            }

            if (floorAssets.Assets != null && floorAssets.Assets.IsRouteInsp)
            {
                floorAssets.Stop = new StopMaster();
                if (row.Table.Columns.Contains("StopId"))
                {
                    if (!string.IsNullOrEmpty(row["StopId"].ToString()))
                    {
                        floorAssets.StopId = Convert.ToInt32(row["StopId"].ToString());
                        floorAssets.Stop = new StopMaster();
                        floorAssets.Stop = GetLocationsMaster(null).FirstOrDefault(x => x.StopId == floorAssets.StopId);
                    }
                }
                if (row.Table.Columns.Contains("RouteId"))
                {
                    if (!string.IsNullOrEmpty(row["RouteId"].ToString()))
                    {
                        var routeId = Convert.ToInt32(row["RouteId"].ToString());
                        floorAssets.Routes = GetRouteMaster(null).Where(x => x.RouteId == routeId).ToList();
                    }
                }
            }

            if (row.Table.Columns.Contains("OpenILSMCount"))
                floorAssets.OpenIlsmsCount = Convert.ToInt32(row["OpenILSMCount"].ToString());

            if (row.Table.Columns.Contains("OpenWorkOrder"))
                floorAssets.OpenWorkOrdersCount = Convert.ToInt32(row["OpenWorkOrder"].ToString());


            if (row.Table.Columns.Contains("AssetChangeDeviceType"))
                floorAssets.AssetChangeDeviceType = row["AssetChangeDeviceType"]!=null && !string.IsNullOrEmpty(row["AssetChangeDeviceType"].ToString())
                        ? Convert.ToInt32(row["AssetChangeDeviceType"].ToString())
                        : 0;

            if (row.Table.Columns.Contains("Path"))
                floorAssets.Path = row["Path"]!=null && !string.IsNullOrEmpty(row["Path"].ToString())
                        ? Convert.ToInt32(row["Path"].ToString())
                        : 0;

            return floorAssets;
        }

        #endregion

        public  List<Assets> InspectByFloor(string assetId, string ascIds, int buildingId, int floorId, int routeId)
        {
            var assets = new List<Assets>();
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_FloorAssetsForInspections;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@AssetId", assetId);
                command.Parameters.AddWithValue("@ascIds", !string.IsNullOrEmpty(ascIds) ? ascIds : (object)DBNull.Value);
                command.Parameters.AddWithValue("@buildingId", buildingId > 0 ? buildingId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@floorId", floorId > 0 ? floorId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@routeId", routeId > 0 ? routeId : (object)DBNull.Value);
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    assets = _sqlHelper.ConvertDataTable<Assets>(dt.Tables[3]);
                    string expression;
                    string sortOrder;
                    foreach (var item in assets)
                    {
                        expression = "AssetId = '" + item.AssetId + "'";
                        sortOrder = "name DESC";
                        var foundRows = dt.Tables[1].Select(expression, sortOrder);
                        foreach (var epRow in foundRows)
                        {
                            var ep = new EPDetails
                            {
                                EPDetailId = Convert.ToInt32(epRow["EPDetailId"].ToString()),
                                EPNumber = epRow["EPNumber"].ToString(),
                                Description = epRow["Description"].ToString(),
                                IsAssetSteps = Convert.ToInt32(epRow["IsAssetSteps"].ToString()),
                                IsDocRequired = Convert.ToBoolean(epRow["IsDocRequired"].ToString()),
                                Standard = new Standard
                                {
                                    StDescID = Convert.ToInt32(epRow["StDescID"].ToString()),
                                    TJCStandard = epRow["TJCStandard"].ToString(),
                                    TJCDescription = epRow["TJCDescription"].ToString()
                                },

                            };
                            if (!string.IsNullOrEmpty(epRow["IsCurrent"].ToString()))
                                ep.IsCurrent = Convert.ToBoolean(epRow["IsCurrent"].ToString());
                                ep.Frequency = new FrequencyMaster();
                            if (!string.IsNullOrEmpty(epRow["FrequencyId"].ToString()))
                            {
                                ep.Frequency.FrequencyId = Convert.ToInt32(epRow["FrequencyId"].ToString());
                                ep.Frequency.Days = Convert.ToInt32(epRow["Days"].ToString());
                                ep.Frequency.DisplayName = epRow["DisplayName"].ToString();
                            }

                            item.EPdetails.Add(ep);
                        }
                    }
                    //  var users = _sqlHelper.ConvertDataTable<UserProfile>(dt.Tables[2]);
                    //  var inspection = _sqlHelper.ConvertDataTable<Inspection>(dt.Tables[2]);
                    var inspectionActivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(dt.Tables[2]).Where(x => x.IsCurrent).ToList();

                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        //var list = BindFloorAsset(row);

                        var floorAssets = new TFloorAssets
                        {
                            EpDetailId = Convert.ToInt32(row["EpDetailId"].ToString()),
                            //SubStatus = row["SubStatus"]==null?"": row["SubStatus"].ToString(),
                            Name = row["Name"].ToString(),
                            SerialNo = row["SerialNo"].ToString(),
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            DeviceNo = row["DeviceNo"].ToString(),
                            FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString()),
                            Source = string.IsNullOrEmpty(row["Source"].ToString()) ? 0 : Convert.ToInt32(row["Source"].ToString()),// Convert.ToInt32(row["Source"].ToString()),
                            OpenWorkOrdersCount = Convert.ToInt32(row["OpenWorkOrdersCount"].ToString()),
                            OpenIlsmsCount = Convert.ToInt32(row["OpenIlsmsCount"].ToString()),

                        };
                        if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                        {
                            floorAssets.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                            floorAssets.Floor = new Floor
                            {
                                FloorName = row["FloorName"].ToString(),
                                FloorId = floorAssets.FloorId.Value,
                                BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString()) ? Convert.ToInt32(row["BuildingId"].ToString()) : 0,
                                Building = new Buildings
                                {
                                    BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString()) ? Convert.ToInt32(row["BuildingId"].ToString()) : 0,
                                    BuildingName = row["BuildingName"].ToString(),
                                    BuildingCode = row["BuildingCode"].ToString()
                                }
                            };
                        }
                        floorAssets.IsInspReady = Convert.ToInt32(row["IsInspReady"].ToString());
                        floorAssets.TInspectionActivity = new List<TInspectionActivity>();
                        floorAssets.TInspectionActivity = inspectionActivity.Where(x => x.FloorAssetId == floorAssets.FloorAssetsId).ToList();
                        lists.Add(floorAssets);
                    }
                }
            }
            foreach (var item in assets)
            {
                item.TFloorAssets = buildingId == -2 ? lists.Where(x => x.AssetId == item.AssetId && x.FloorId == null).ToList() : lists.Where(x => x.AssetId == item.AssetId).ToList();
            }
            return assets;
        }

        #region 
        public  DataSet AddImportedFloorAssets(TFloorAssets newFloorAsset)
        {
            DataSet ds = new DataSet();
            try
            {
                const string sql = StoredProcedures.Insert_ImportedFloorAssets;
                using (var command = new SqlCommand(sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FloorId", newFloorAsset.FloorId);
                    command.Parameters.AddWithValue("@AssetId", newFloorAsset.AssetId);
                    command.Parameters.AddWithValue("@Name", newFloorAsset.Name);
                    command.Parameters.AddWithValue("@CreatedBy", newFloorAsset.CreatedBy);
                    command.Parameters.AddWithValue("@SerialNo", newFloorAsset.SerialNo);
                    command.Parameters.AddWithValue("@Xcoordinate", String.IsNullOrEmpty(newFloorAsset.Xcoordinate) ? "0" : newFloorAsset.Xcoordinate);
                    command.Parameters.AddWithValue("@Ycoordinate", String.IsNullOrEmpty(newFloorAsset.Ycoordinate) ? "0" : newFloorAsset.Ycoordinate);
                    command.Parameters.AddWithValue("@AreaCode", newFloorAsset.AreaCode);
                    command.Parameters.AddWithValue("@ZoomLevel", newFloorAsset.ZoomLevel);
                    command.Parameters.AddWithValue("@Notes", newFloorAsset.Notes);
                    command.Parameters.AddWithValue("@DeviceNo", newFloorAsset.DeviceNo);//assets #
                    command.Parameters.AddWithValue("@NearBy", newFloorAsset.NearBy);
                    command.Parameters.AddWithValue("@DateCreated", newFloorAsset.DateCreated);
                    command.Parameters.AddWithValue("@DateUpdated", newFloorAsset.DateUpdated);
                    command.Parameters.AddWithValue("@LocationCode", newFloorAsset.LocationCode);//assets #
                    command.Parameters.AddWithValue("@BuildingCode", newFloorAsset.BuildingCode);
                    command.Parameters.AddWithValue("@description", newFloorAsset.Description);
                    command.Parameters.AddWithValue("@StatusCode", !string.IsNullOrEmpty(newFloorAsset.StatusCode) ? newFloorAsset.StatusCode : StatusCode.ACTIV.ToString());
                    command.Parameters.AddWithValue("@TmsReference", newFloorAsset.TmsReference);
                    command.Parameters.AddWithValue("@Manufacture", newFloorAsset.Make.ManufactureName);
                    command.Parameters.AddWithValue("@AscId", newFloorAsset.AscId);
                    command.Parameters.AddWithValue("@StopId", newFloorAsset.StopId);
                    command.Parameters.AddWithValue("@FETypeId", newFloorAsset.FETypeId);
                    command.Parameters.AddWithValue("@ModelNo", newFloorAsset.Model);
                    command.Parameters.AddWithValue("@Rating", newFloorAsset.Rating);
                    command.Parameters.AddWithValue("@WallRating", newFloorAsset.WallRating);
                    command.Parameters.AddWithValue("@DoorRating", newFloorAsset.DoorRating);
                    command.Parameters.AddWithValue("@FrameRating", newFloorAsset.FrameRating);
                    command.Parameters.AddWithValue("@BuildingName", newFloorAsset.BuildingName);
                    command.Parameters.AddWithValue("@BuildingId", newFloorAsset.BuildingID);
                    command.Parameters.AddWithValue("@BuildingTypeId", String.IsNullOrEmpty(newFloorAsset.BuildingTypeId) ? "1" : newFloorAsset.BuildingTypeId);
                    command.Parameters.AddWithValue("@SiteCode", newFloorAsset.SiteCode);
                    command.Parameters.AddWithValue("@FloorName", newFloorAsset.FloorName);
                    command.Parameters.AddWithValue("@IconPath", "~/Images/Assets/default.png");
                    command.Parameters.AddWithValue("@AssetTypeId", newFloorAsset.AssetTypeId);
                    command.Parameters.AddWithValue("@LocationName", newFloorAsset.LocationName);
                    ds = _sqlHelper.ExecuteNonQueryReturnDS(command);
                    int ind = ds.Tables.Count;
                    ind = ind - 3;
                    if (ds.Tables.Count > 1) { ds.Tables[ind].TableName = "NotMatchData"; }
                    if (ds.Tables.Count > 2) { ds.Tables[ind + 1].TableName = "MatchData"; }
                    if (ds.Tables.Count > 3) { ds.Tables[ind + 2].TableName = "NewData"; }

                    string importMsg = $@"Matched Records : { GetImportedAssetData(ds.Tables["MatchData"])}\n
                                        NotMatch Records : { GetImportedAssetData(ds.Tables["NotMatchData"])}\n
                                        New Records : { GetImportedAssetData(ds.Tables["NewData"])}";
                    //ErrorLog.LogMsg(importMsg);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return ds;
        }

        /// <summary>
        /// gets all the record of asset import csv file which are inserted / updated / new
        /// </summary>
        private  string GetImportedAssetData(DataTable dataTable)
        {
            string records = "";
            foreach (var item in dataTable.Rows)
            {
                if (item != null)
                {
                    if (!String.IsNullOrEmpty(item.ToString()))
                    {
                        records += item.ToString() + ",";
                    }
                }
                if (String.IsNullOrEmpty(records))
                {
                    records = "There is no data for this operation";
                }
            }
            return records;
        }

        #endregion


        public  TFloorAssets GetAssetsOpenItems(int floorAssetsId)
        {
            var tFloorAssets = new TFloorAssets();
            const string sql = StoredProcedures.Get_AssetsWorkOrderStatus;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@floorAssetsId", floorAssetsId);
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    foreach (DataRow item in dt.Tables[0].Rows)
                    {
                        tFloorAssets.FloorAssetsId = Convert.ToInt32(item["FloorAssetsId"].ToString());
                        tFloorAssets.OpenFailSetpsCount = Convert.ToInt32(item["OpenFailSetpsCount"].ToString());
                        tFloorAssets.OpenIlsmsCount = Convert.ToInt32(item["OpenIlsmsCount"].ToString());
                        tFloorAssets.OpenIlsmsCount = Convert.ToInt32(item["OpenIlsmsCount"].ToString());
                    }
                }
            }
            return tFloorAssets;
        }


        public  void SaveLocation(FloorShapes newItem)
        {
            //PointF[] ptarray = new PointF[1];
            //if (newItem.FloorId != null)
            //{
            //    var tFloorAssets = DAL.FloorAssetRepository.GetFloorAssets(newItem.FloorId.Value);
            //    var serializer = new JavaScriptSerializer();
            //    serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            //    dynamic data = serializer.Deserialize(newItem.Data, typeof(object));
            //    var lineList = data.lineList;
            //    if (lineList.Count > 0)
            //    {
            //        ptarray = new PointF[(int)lineList.Count];
            //        for (int i = 0; i < lineList.Count; i++)
            //        {
            //            ptarray[i] = new PointF(float.Parse((lineList[i].x).ToString(), CultureInfo.InvariantCulture.NumberFormat),
            //                float.Parse((lineList[i].y).ToString(), CultureInfo.InvariantCulture.NumberFormat));
            //        }
            //    }
            //    foreach (var item in tFloorAssets)
            //    {
            //        PointF point = new PointF(float.Parse(item.Xcoordinate, CultureInfo.InvariantCulture.NumberFormat),
            //            float.Parse(item.Ycoordinate, CultureInfo.InvariantCulture.NumberFormat));
            //        var status = IsPointInPolygon(ptarray, point);
            //        if (status)
            //        {
            //            TFloorAssetsShaps location = new TFloorAssetsShaps { FloorAssetsId = item.FloorAssetsId, ShapeId = newItem.Id };
            //            SaveTFloorAssetsShaps(location);
            //        }
            //    }
            //}
        }


        #region AssetType Update RBI

        public  int UpdateAssetType(int ascid, int FloorAssetsId)
        {
            int newId;
            const string sql = StoredProcedures.Update_AssetType;

            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ascid", ascid);
                command.Parameters.AddWithValue("@FloorAssetsId", FloorAssetsId);

                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }
        #endregion


        #region remove these methods after implement DI

        // remove this mehtod and use LocationRepo GetLocationsMaster method
        private  List<StopMaster> GetLocationsMaster(int? locationMasterId, string stopCode = null)
        {
            var stops = new List<StopMaster>();
            const string sql = StoredProcedures.Get_StopMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StopId", locationMasterId);
                command.Parameters.AddWithValue("@stopCode", stopCode);
                var ds = _sqlHelper.GetDataSet(command);
                var routes = _sqlHelper.ConvertDataTable<RouteMaster>(ds.Tables[1]);
                if (ds.Tables[0] != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var objstopMaster = new StopMaster
                        {
                            StopId = Convert.ToInt32(row["StopId"].ToString()),
                            StopName = row["StopName"].ToString(),
                            StopCode = row["StopCode"].ToString(),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                        };
                        if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                        {
                            objstopMaster.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                            objstopMaster.BuildingId = Convert.ToInt32(row["BuildingID"].ToString());
                            objstopMaster.Floor = new Floor
                            {
                                FloorName = row["FloorName"].ToString(),
                                FloorId = objstopMaster.FloorId.Value,
                                BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString()) ? Convert.ToInt32(row["BuildingId"].ToString()) : 0,
                                Building = new Buildings
                                {
                                    BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString()) ? Convert.ToInt32(row["BuildingId"].ToString()) : 0,
                                    BuildingName = row["BuildingName"].ToString(),
                                    BuildingCode = row["BuildingCode"].ToString()
                                }
                            };
                        }
                        objstopMaster.Routes = routes.Where(x => x.StopId == Convert.ToInt32(row["StopId"].ToString())).ToList();
                        stops.Add(objstopMaster);
                    }
                }
            }
            return stops;
        }

        // remove this method and use LocationRepo GetRouteMaster method
        private  List<RouteMaster> GetRouteMaster(int? routeId)
        {
            List<RouteMaster> routes = new List<RouteMaster>();
            string sql = StoredProcedures.Get_RouteMaster;
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@routeId", routeId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        RouteMaster objrouteMaster = new RouteMaster
                        {
                            RouteId = Convert.ToInt32(row["RouteId"].ToString()),
                            RouteNo = row["RouteNo"].ToString(),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            IsDefault = Convert.ToBoolean(row["IsDefault"].ToString()),
                        };
                        
                        objrouteMaster.StopsRouteMapping = GetLocationrouteMapping(null, objrouteMaster.RouteId);
                        routes.Add(objrouteMaster);
                    }
                }
            }
            return routes;
        }

        private  List<StopsRouteMapping> GetLocationrouteMapping(int? floorId, int? routeId)
        {
            List<StopsRouteMapping> objstopsRouteMapping = new List<StopsRouteMapping>();
            string sql = StoredProcedures.Get_StopRouteMapping;
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorId", floorId.HasValue && floorId > 0 ? floorId : null);
                command.Parameters.AddWithValue("@routeId", routeId.HasValue && routeId > 0 ? routeId : null);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        StopsRouteMapping objlocationrouteMap = new StopsRouteMapping
                        {
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString())
                        };
                        if (!string.IsNullOrEmpty(row["StopId"].ToString()))
                        {
                            objlocationrouteMap.StopId = Convert.ToInt32(row["StopId"].ToString());
                            objlocationrouteMap.Stops = new StopMaster
                            {
                                StopId = Convert.ToInt32(row["StopId"].ToString()),
                                StopName = row["StopName"].ToString(),
                                StopCode = row["StopCode"].ToString(),
                                IsInRoute = Convert.ToBoolean(row["IsInRoute"].ToString())
                            };

                            if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                                objlocationrouteMap.Stops.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["RouteId"].ToString()))
                        {
                            objlocationrouteMap.RouteId = Convert.ToInt32(row["RouteId"].ToString());
                            objlocationrouteMap.Route = new RouteMaster
                            {
                                RouteId = Convert.ToInt32(row["RouteId"].ToString()),
                                RouteNo = row["RouteNo"].ToString(),
                            };
                        }
                        objstopsRouteMapping.Add(objlocationrouteMap);
                    }
                }
            }
            return objstopsRouteMapping;
        }

        public TFloorAssets GetAssetInsHistory(int floorAssetsId, int epDetailId, int selectedEp)
        {
            throw new NotImplementedException();
        }

        public List<TFloorAssets> GetAssetsReports(string assetids)
        {
            throw new NotImplementedException();
        }

        public List<TFloorAssets> GetFilterAssetsReports(string assetids, string buildingId, string floorId, string fromdate, string todate)
        {
            throw new NotImplementedException();
        }

        public List<TFloorAssets> InventoryInspectionAssetsReport(Request objRequest, string ids, string buildingId, string floorId, string fromdate, string todate, string status = null)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
