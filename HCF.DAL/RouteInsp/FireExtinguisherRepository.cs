using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class FireExtinguisherRepository : IFireExtinguisherRepository
    {
        #region ctor
        private readonly ISqlHelper _sqlHelper;
        private readonly IFloorRepository _floorRepository;
        private readonly IFloorAssetRepository _floorAssetRepository;
        private readonly IInspectionActivityRepository _inspectionActivityRepository;

        public FireExtinguisherRepository(ISqlHelper sqlHelper, IFloorRepository floorRepository, IFloorAssetRepository floorAssetRepository,
            IInspectionActivityRepository inspectionActivityRepository)
        {
            _inspectionActivityRepository = inspectionActivityRepository;
            _floorAssetRepository = floorAssetRepository;
            _floorRepository = floorRepository;
            _sqlHelper = sqlHelper;
        }

        #endregion

        #region Fire Extinguisher Assets

        public List<StopMaster> GetExtinguisherAssets(int? floorId, int? inspType, int? routeId, int? assetId)
        {
            var lists = new List<StopMaster>();
            var tfloorAssets = new List<TFloorAssets>();
            var activity = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_ExtinguisherAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorId", floorId > 0 ? floorId : null);
                command.Parameters.AddWithValue("@inspType", inspType > 0 ? inspType : null);
                command.Parameters.AddWithValue("@routeId", routeId > 0 ? routeId : null);
                command.Parameters.AddWithValue("@assetId", assetId > 0 ? assetId : null);
                var ds = _sqlHelper.GetDataSet(command);
                lists = _sqlHelper.ConvertDataTable<StopMaster>(ds.Tables[0]);
                //var manufactures = _sqlHelper.ConvertDataTable<Manufactures>(ds.Tables[1]);
                //var stops = _sqlHelper.ConvertDataTable<StopMaster>(ds.Tables[1]);
                //var assetsubcategory = _sqlHelper.ConvertDataTable<AssetsSubCategory>(ds.Tables[1]);
                var route = _sqlHelper.ConvertDataTable<RouteMaster>(ds.Tables[3]);
                var floors = _floorRepository.GetFloors();
                foreach (DataRow row in ds.Tables[2].Rows)
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
                        InspStatusCode = row["InspStatusCode"].ToString(),
                        IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString()),
                        ActivityInspectionDate = Convert.ToDateTime(row["ActivityInspectionDate"].ToString()),
                        FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                        Frequency = new FrequencyMaster
                        {
                            DisplayName = row["DisplayName"].ToString()
                        },
                        UserProfile = new UserProfile
                        {
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Email = row["Email"].ToString(),
                            UserId = Convert.ToInt32(row["UserId"].ToString())
                        },
                        InspStatus = new InspStatus()
                    };
                    data.InspStatus.StatusName = row["StatusName"].ToString();
                    data.InspResult = new InspResult
                    {
                        ResultName = row["ResultName"].ToString()
                    };
                    activity.Add(data);
                }
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    var floorAsset = new TFloorAssets
                    {
                        SerialNo = row["SerialNo"].ToString(),
                        AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                        FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString()),
                        DeviceNo = row["DeviceNo"].ToString()
                    };
                    if (!string.IsNullOrEmpty(row["OpenWorkOrder"].ToString()))
                    {
                        floorAsset.OpenWorkOrdersCount = Convert.ToInt32(row["OpenWorkOrder"].ToString());
                    }
                    //if (!string.IsNullOrEmpty(row["OpenIlsmCount"].ToString()))
                    //{
                    //    floorAsset.OpenIlsmsCount = Convert.ToInt32(row["OpenIlsmsCount"].ToString());
                    //}
                    if (!string.IsNullOrEmpty(row["ManufactureId"].ToString()))
                    {
                        floorAsset.ManufactureId = Convert.ToInt32(row["ManufactureId"].ToString());
                        floorAsset.Make = new Manufactures
                        {
                            ManufactureId = floorAsset.ManufactureId.Value,
                            ManufactureName = row["ManufactureName"].ToString()
                        };
                    }

                    floorAsset.Stop = new StopMaster();
                    if (!string.IsNullOrEmpty(row["StopId"].ToString()))
                    {
                        floorAsset.StopId = Convert.ToInt32(row["StopId"].ToString());
                        floorAsset.Stop = new StopMaster();
                        floorAsset.Stop.StopName = row["StopName"].ToString();
                        floorAsset.Stop.StopCode = row["StopCode"].ToString();
                        //stops.FirstOrDefault(x => x.StopId == floorAsset.StopId);
                        floorAsset.Stop.Routes = new List<RouteMaster>();
                        floorAsset.Stop.Routes = route.Where(x => x.StopId == floorAsset.StopId).ToList();
                    }
                    floorAsset.AssetSubCategory = new AssetsSubCategory();
                    if (!string.IsNullOrEmpty(row["AscId"].ToString()))
                    {
                        floorAsset.AscId = Convert.ToInt32(row["AscId"].ToString());
                        floorAsset.AssetSubCategory = new AssetsSubCategory();

                        floorAsset.AssetSubCategory.AscName = row["AscName"].ToString();
                        //assetsubcategory.FirstOrDefault(x => x.AscId == floorAsset.AscId);
                    }
                    floorAsset.TInspectionActivity = activity.Where(x => x.FloorAssetId == floorAsset.FloorAssetsId).ToList();

                    floorAsset.Routes = route.Where(x => x.RouteId == routeId).ToList();
                    if (!string.IsNullOrEmpty(row["AssetId"].ToString()))
                    {
                        floorAsset.Assets = new Assets()
                        {
                            Name = row["AssetsName"].ToString(),
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                        };
                    }

                    if (!string.IsNullOrEmpty(row["FETypeId"].ToString()))
                    {
                        floorAsset.FETypeId = Convert.ToInt32(row["FETypeId"].ToString());
                        floorAsset.FireExtinguisherType = new FireExtinguisherTypes
                        {
                            FETypeId = floorAsset.FETypeId,
                            FeType = row["FeType"].ToString()
                        };
                    }

                    tfloorAssets.Add(floorAsset);
                }
                foreach (var item in lists)
                {
                    item.TfloorAssets = tfloorAssets.Where(x => x.StopId == item.StopId).ToList();
                    item.Floor = new Floor();
                    if (item.FloorId.HasValue && item.FloorId > 0)
                        item.Floor = floors.FirstOrDefault(x => x.FloorId == item.FloorId);
                }
            }
            return lists;
        }

        public List<TFloorAssets> GetExtinguisherAssetsWithOutFloor(int assetId, int? inspType)
        {
            List<TFloorAssets> tfloorAssets = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_ExtinguisherAssetsWithOutFloor;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId);
                command.Parameters.AddWithValue("@inspType", inspType);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    tfloorAssets = ConvertToFEAssetsWithTA(ds);
            }
            return tfloorAssets;
        }

        public TFloorAssets GetExtinguisherAssets(string tagNo, string stopcode, string assetId)
        {
            TFloorAssets tFloorAssets = new TFloorAssets();
            const string sql = StoredProcedures.Get_ExtinguisherAsset;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tagNo", tagNo);
                command.Parameters.AddWithValue("@stopcode", stopcode);
                command.Parameters.AddWithValue("@assetId", assetId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    tFloorAssets = _sqlHelper.ConvertDataTable<TFloorAssets>(ds.Tables[0]).FirstOrDefault();
                    if (tFloorAssets != null)
                        tFloorAssets = _floorAssetRepository.GetFloorAsset(tFloorAssets.FloorAssetsId);
                }
            }
            return tFloorAssets;
        }

        #endregion

        #region FloorAssets

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newfloorAssets"></param>
        /// <returns></returns>


        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <returns></returns>
        public bool RemoveFloorAssetsFromCurrentLocation(int floorAssetId)
        {
            return _floorAssetRepository.RemoveFloorAssetsFromCurrentLocation(floorAssetId);
        }

        #endregion

        #region Inspection

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objtinspectionActivity"></param>
        /// <returns></returns>
        public int SaveInspection(TInspectionActivity objtinspectionActivity)
        {
            return _inspectionActivityRepository.Save(objtinspectionActivity);
        }

        #endregion

        #region Asset Insp Frequency

        public List<FrequencyMaster> GetAssetInspFrequency(int assetId)
        {
            List<FrequencyMaster> objfrequencyMaster = new List<FrequencyMaster>();
            const string sql = StoredProcedures.Get_AssetInspFrequency;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId > 0 ? assetId : (object)DBNull.Value);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                {
                    objfrequencyMaster = _sqlHelper.ConvertDataTable<FrequencyMaster>(dt);
                }
            }
            return objfrequencyMaster;
        }

        #endregion

        #region InspResult 

        public List<InspResult> GetInspResult()
        {
            List<InspResult> objinspResult = new List<InspResult>();
            const string sql = StoredProcedures.Get_InspResult;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    objinspResult = _sqlHelper.ConvertDataTable<InspResult>(dt);
            }
            return objinspResult;
        }

        public List<InspStatus> GetInspStatus()
        {
            List<InspStatus> objinspStatus = new List<InspStatus>();
            const string sql = StoredProcedures.Get_InspStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    objinspStatus = _sqlHelper.ConvertDataTable<InspStatus>(dt);
            }
            return objinspStatus;
        }

        #endregion

        #region Reports

        public List<TFloorAssets> Get_ExtinguisherAssetsReports(int assetId, int? buildingId, int? floorId, int? inspType)
        {
            List<TFloorAssets> tfloorAssets = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_ExtinguisherAssetsReports;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId);
                command.Parameters.AddWithValue("@floorId", floorId > 0 ? floorId : null);
                command.Parameters.AddWithValue("@inspType", inspType);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    tfloorAssets = ConvertToFEAssetsWithTA(ds);
            }
            return tfloorAssets;
        }

        private List<TFloorAssets> ConvertToFEAssetsWithTA(DataSet ds)
        {
            List<TFloorAssets> tfloorAssets = new List<TFloorAssets>();
            List<TInspectionActivity> activity = new List<TInspectionActivity>();
            List<Manufactures> manufactures = _sqlHelper.ConvertDataTable<Manufactures>(ds.Tables[0]);
            List<StopMaster> stops = _sqlHelper.ConvertDataTable<StopMaster>(ds.Tables[0]);
            List<AssetsSubCategory> assetsubcategory = _sqlHelper.ConvertDataTable<AssetsSubCategory>(ds.Tables[0]);
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                TInspectionActivity data = new TInspectionActivity
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
                    Frequency = new FrequencyMaster()
                };
                data.Frequency.DisplayName = row["DisplayName"].ToString();
                data.UserProfile = new UserProfile
                {
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    Email = row["Email"].ToString(),
                    UserId = Convert.ToInt32(row["UserId"].ToString())
                };
                data.InspStatus = new InspStatus
                {
                    StatusName = row["StatusName"].ToString()
                };
                data.InspResult = new InspResult
                {
                    ResultName = row["ResultName"].ToString()
                };
                activity.Add(data);
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                TFloorAssets floorAsset = new TFloorAssets
                {
                    SerialNo = row["SerialNo"].ToString(),
                    AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                    FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString()),
                    DeviceNo = row["DeviceNo"].ToString(),
                    //ManufactureId = Convert.ToInt32(row["ManufactureId"].ToString()),
                    Stop = new StopMaster()
                };
                if (!string.IsNullOrEmpty(row["ManufactureId"].ToString()))
                {
                    floorAsset.ManufactureId = Convert.ToInt32(row["ManufactureId"].ToString());
                }
                if (!string.IsNullOrEmpty(row["StopId"].ToString()))
                {
                    floorAsset.StopId = Convert.ToInt32(row["StopId"].ToString());
                    floorAsset.Stop = stops.FirstOrDefault(x => x.StopId == floorAsset.StopId);
                }
                floorAsset.AssetSubCategory = new AssetsSubCategory();
                if (!string.IsNullOrEmpty(row["AscId"].ToString()))
                {
                    floorAsset.AscId = Convert.ToInt32(row["AscId"].ToString());
                    floorAsset.AssetSubCategory = assetsubcategory.FirstOrDefault(x => x.AscId == floorAsset.AscId);
                }
                floorAsset.TInspectionActivity = activity.Where(x => x.FloorAssetId == floorAsset.FloorAssetsId).ToList();
                floorAsset.Make = manufactures.FirstOrDefault(x => x.ManufactureId == floorAsset.ManufactureId);
                if (!string.IsNullOrEmpty(row["AssetId"].ToString()))
                {
                    floorAsset.Assets = new Assets()
                    {
                        Name = row["AssetsName"].ToString(),
                        AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                    };
                }
                if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                {
                    floorAsset.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                    floorAsset.Floor = new Floor
                    {
                        FloorName = row["FloorName"].ToString(),
                        FloorId = floorAsset.FloorId.Value,
                        BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString()) ? Convert.ToInt32(row["BuildingId"].ToString()) : 0,
                        Building = new Buildings
                        {
                            BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString()) ? Convert.ToInt32(row["BuildingId"].ToString()) : 0,
                            BuildingName = row["BuildingName"].ToString(),
                            BuildingCode = row["BuildingCode"].ToString()
                        }
                    };
                }
                else
                {
                    floorAsset.Floor = new Floor
                    {
                        Building = new Buildings()
                    };
                }
                tfloorAssets.Add(floorAsset);
            }
            return tfloorAssets;
        }

        public List<TFloorAssets> Get_ExtinguisherAssetsReport(int year, int? routNo, int? assetId, int? epdetailId)
        {
            var tfloorAssets = new List<TFloorAssets>();
            var activity = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_ExtinguisherRouteReports;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@routNo", routNo > 0 ? routNo : null);
                command.Parameters.AddWithValue("@assetId", assetId > 0 ? assetId : null);
                command.Parameters.AddWithValue("@epdetailId", epdetailId > 0 ? epdetailId : null);
                var ds = _sqlHelper.GetDataSet(command);
                var manufactures = _sqlHelper.ConvertDataTable<Manufactures>(ds.Tables[0]);
                var stops = _sqlHelper.ConvertDataTable<StopMaster>(ds.Tables[0]);
                var assetsubCategory = _sqlHelper.ConvertDataTable<AssetsSubCategory>(ds.Tables[0]);
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    var data = new TInspectionActivity
                    {
                        Comment = row["Comment"].ToString(),
                        FloorAssetId = Convert.ToInt32(row["FloorAssetId"].ToString()),
                        ActivityId = Guid.Parse(row["ActivityId"].ToString()),
                        CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                        CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                        ActivityInspectionDate = Convert.ToDateTime(row["ActivityInspectionDate"].ToString()),
                        Status = Convert.ToInt32(row["Status"].ToString()),
                        SubStatus = row["SubStatus"].ToString(),
                        IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString()),
                        //data.ActivityInspectionDate = Convert.ToDateTime(row["ActivityInspectionDate"].ToString());
                        //data.FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString());
                        // data.Frequency = new FrequencyMaster();
                        // data.Frequency.DisplayName = row["DisplayName"].ToString();
                        UserProfile = new UserProfile
                        {
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Email = row["Email"].ToString(),
                            PhoneNumber = row["PhoneNumber"].ToString(),
                            UserId = Convert.ToInt32(row["UserId"].ToString()),
                            VendorId = !string.IsNullOrEmpty(row["VendorId"].ToString()) ? Convert.ToInt32(row["VendorId"].ToString()) : (int?)null
                        },
                        InspStatus = new InspStatus()
                    };
                    data.InspStatus.StatusName = row["StatusName"].ToString();
                    data.InspResult = new InspResult
                    {
                        ResultName = row["ResultName"].ToString()
                    };
                    activity.Add(data);
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var floorAsset = new TFloorAssets
                    {
                        SerialNo = row["SerialNo"].ToString(),
                        AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                        FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString()),
                        DeviceNo = row["DeviceNo"].ToString(),
                        //ManufactureId = Convert.ToInt32(row["ManufactureId"].ToString()),
                        Stop = new StopMaster()
                    };
                    if (!string.IsNullOrEmpty(row["StopId"].ToString()))
                    {
                        floorAsset.StopId = Convert.ToInt32(row["StopId"].ToString());
                        floorAsset.Stop = stops.FirstOrDefault(x => x.StopId == floorAsset.StopId);
                    }
                    if (!string.IsNullOrEmpty(row["ManufactureId"].ToString()))
                    {
                        floorAsset.ManufactureId = Convert.ToInt32(row["ManufactureId"].ToString());
                        // floorAsset.Make = manufactures.FirstOrDefault(x => x.ManufactureId == floorAsset.ManufactureId);
                    }
                    floorAsset.AssetSubCategory = new AssetsSubCategory();
                    if (!string.IsNullOrEmpty(row["AscId"].ToString()))
                    {
                        floorAsset.AscId = Convert.ToInt32(row["AscId"].ToString());
                        floorAsset.AssetSubCategory = assetsubCategory.FirstOrDefault(x => x.AscId == floorAsset.AscId);
                    }
                    if (!string.IsNullOrEmpty(row["RouteId"].ToString()))
                    {
                        floorAsset.Routes = new List<RouteMaster>();
                        var route = new RouteMaster
                        {
                            RouteId = Convert.ToInt32(row["RouteId"].ToString()),
                            RouteNo = row["RouteNo"].ToString()
                        };
                        floorAsset.Routes.Add(route);
                    }
                    if (!string.IsNullOrEmpty(row["FETypeId"].ToString()))
                    {
                        floorAsset.FETypeId = Convert.ToInt32(row["FETypeId"].ToString());
                        floorAsset.FireExtinguisherType = new FireExtinguisherTypes
                        {
                            FETypeId = floorAsset.FETypeId,
                            FeType = row["FeType"].ToString()
                        };
                    }
                    floorAsset.TInspectionActivity = activity.Where(x => x.FloorAssetId == floorAsset.FloorAssetsId).ToList();
                    if (floorAsset.TInspectionActivity.Count > 0)
                    {
                        var floorAssetId = floorAsset.FloorAssetsId;
                    }
                    floorAsset.Make = manufactures.FirstOrDefault(x => x.ManufactureId == floorAsset.ManufactureId);
                    tfloorAssets.Add(floorAsset);
                }
            }
            return tfloorAssets;
        }

        #endregion     

    }
}