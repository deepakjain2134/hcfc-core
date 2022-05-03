using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public  class FloorRepository: IFloorRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IFloorPlanRepository _floorPlanRepository;
        public FloorRepository(ISqlHelper sqlHelper, IFloorPlanRepository floorPlanRepository)
        {
            _floorPlanRepository = floorPlanRepository;
                _sqlHelper = sqlHelper;
        }
        /// <summary>
        /// s
        /// </summary>
        /// <param name="newFloor"></param>
        /// <returns></returns>
        public  int Save(Floor newFloor)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Floor;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorName", newFloor.FloorName);
                command.Parameters.AddWithValue("@BuildingID", newFloor.BuildingId);
                command.Parameters.AddWithValue("@BuildingCode", newFloor.BuildingCode);
                command.Parameters.AddWithValue("@ImagePath", newFloor.ImagePath);
                command.Parameters.AddWithValue("@FloorCode", newFloor.FloorCode);
                command.Parameters.AddWithValue("@FileName", newFloor.FileName);
                command.Parameters.AddWithValue("@CreatedBy", newFloor.CreatedBy);
                command.Parameters.AddWithValue("@FloorTypeId", newFloor.FloorTypeId);
                command.Parameters.AddWithValue("@EffectiveDate", HCF.Utility.Conversion.ConvertToDateTime(newFloor.EffectiveDateTimeSpan));
                command.Parameters.AddWithValue("@Alias", newFloor.Alias);
                command.Parameters.AddWithValue("@Sequence", newFloor.AliasSequence);
                command.Parameters.AddWithValue("@IsActive", newFloor.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int UpdateFloorPlan(FloorPlan item)
        {
            int newId;
            const string sql = StoredProcedures.Insert_FloorPlan;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorPlanId", item.FloorPlanId);
                command.Parameters.AddWithValue("@FloorId", item.FloorId);
                command.Parameters.AddWithValue("@FileName", item.FileName);
                command.Parameters.AddWithValue("@ImagePath", item.ImagePath);
                command.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                command.Parameters.AddWithValue("@FloorPlanTypeId", item.FloorPlanTypeId);
                command.Parameters.AddWithValue("@IsActive", item.IsActive);
                //command.Parameters.AddWithValue("@IsDefault", item.IsDefault);
                command.Parameters.AddWithValue("@ThumbImagePath", item.ThumbImagePath);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public  int UpdateThumbImageFloorPlan(FloorPlan item)
        {
            int newId;
            const string sql = StoredProcedures.Update_FloorPlanThumbImage;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorPlanId", item.FloorPlanId);
                command.Parameters.AddWithValue("@FloorId", item.FloorId);
                command.Parameters.AddWithValue("@ThumbImagePath", item.ThumbImagePath);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateFloor"></param>
        /// <returns></returns>
        public  bool UpdateFloor(Floor updateFloor)
        {
            bool status;
            const string sql = StoredProcedures.Update_UpdateFloor;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorId", updateFloor.FloorId);
                command.Parameters.AddWithValue("@FloorName", updateFloor.FloorName);
                command.Parameters.AddWithValue("@FileName", updateFloor.FileName);
                command.Parameters.AddWithValue("@floorCode", updateFloor.FloorCode);
                command.Parameters.AddWithValue("@ImagePath", updateFloor.ImagePath);
                command.Parameters.AddWithValue("@IsActive", updateFloor.IsActive);
                command.Parameters.AddWithValue("@Alias", updateFloor.Alias);
                command.Parameters.AddWithValue("@Sequence", updateFloor.AliasSequence);
                command.Parameters.AddWithValue("@ALTERdBy", updateFloor.CreatedBy);
                command.Parameters.AddWithValue("@FloorPlanId", updateFloor.FloorPlanId);
                command.Parameters.AddWithValue("@EffectiveDate", HCF.Utility.Conversion.ConvertToDateTime(updateFloor.EffectiveDateTimeSpan));
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public  Floor ConvertToFloor(DataTable dataTable)
        {
            var floor = _sqlHelper.ConvertDataTable<Floor>(dataTable).FirstOrDefault();
            var building = _sqlHelper.ConvertDataTable<Buildings>(dataTable).FirstOrDefault();
            if (floor != null)
                floor.Building = building;
            return floor;
        }


        internal  List<Floor> ConvertToFloors(DataTable dataTable)
        {
            var floors = _sqlHelper.ConvertDataTable<Floor>(dataTable).GroupBy(x => x.FloorId).Select(x => x.FirstOrDefault()).ToList();
            var building = _sqlHelper.ConvertDataTable<Buildings>(dataTable).GroupBy(x => x.BuildingId).Select(x => x.FirstOrDefault()).ToList();
            foreach (var item in floors)
            {
                item.Building = building.FirstOrDefault(x => x.BuildingId == item.BuildingId);
            }
            return floors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <param name="floorPlanId"></param>
        /// <returns></returns>
        public  bool UpdateFloorPlan(int floorId, Guid? floorPlanId)
        {
            bool status;
            const string sql = StoredProcedures.Update_FloorPlan;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorPlanId", floorPlanId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private  List<Floor> GetFloors(int? floorTypeId, int? buildingId, int? floorId, int? CheckFileEmpty)
        {
            var floors = new List<Floor>();
            const string sql = StoredProcedures.Get_Floors;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@floorTypeId", floorTypeId);
                cmd.Parameters.AddWithValue("@buildingId", buildingId);
                cmd.Parameters.AddWithValue("@floorId", floorId);
                cmd.Parameters.AddWithValue("@CheckFileEmpty", @CheckFileEmpty);
                var ds = _sqlHelper.GetDataSet(cmd);
                if (ds != null)
                {
                    var routeMaster = _sqlHelper.ConvertDataTable<RouteMaster>(ds.Tables[1]);
                    var floorPlans = _sqlHelper.ConvertDataTable<FloorPlan>(ds.Tables[3]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var floor = new Floor
                        {
                            BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                            FloorTypeId = Convert.ToInt32(row["FloorTypeId"].ToString()),
                            CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                            FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                            FloorName = row["FloorName"].ToString(),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            Alias = row["Alias"].ToString(),
                            AliasSequence = row["Sequence"].ToString(),
                            FloorCode = row["FloorCode"].ToString(),
                            FloorType =
                                new FloorTypes
                                {
                                    FloorType = row["FloorType"].ToString(),
                                    FloorTypeId = Convert.ToInt32(row["FloorTypeId"].ToString())
                                },
                            Building = new Buildings
                            {
                                BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                                BuildingName = row["BuildingName"].ToString(),
                                BuildingCode = row["BuildingCode"].ToString(),
                                SiteCode = row["SiteCode"].ToString(),
                                IsActive = Convert.ToBoolean(row["BuildingStatus"].ToString())
                            }
                        };

                        //var floorPlan = floorPlans.FirstOrDefault(x => x.FloorId == floor.FloorId && x.FloorPlanTypeId == 1 && x.IsActive && x.IsCurrent);
                        var floorPlan = floorPlans.FirstOrDefault(x => x.FloorId == floor.FloorId && x.FloorPlanTypeId == (floorTypeId.HasValue ? floorTypeId.Value : 1) && x.IsActive && x.IsCurrent);
                        if (floorPlan != null)
                        {
                            floor.FileName = floorPlan.FileName;
                            floor.ImagePath = floorPlan.ImagePath;
                            floor.FloorPlanId = floorPlan.FloorPlanId.Value;
                            floor.EffectiveDate = Convert.ToDateTime(floorPlan.EffectiveDate);
                            floor.EffectiveDateTimeSpan = Conversion.ConvertToTimeSpan(Convert.ToDateTime(floorPlan.EffectiveDate));
                        }
                        floor.FloorPlans = floorPlans.Where(x => x.FloorId == floor.FloorId && x.IsActive).ToList();
                        floor.Routes = routeMaster.Where(x => x.FloorId == floor.FloorId).ToList();

                        floors.Add(floor);
                    }
                }
            }
            return floors.GroupBy(x => x.FloorId).Select(x => x.FirstOrDefault()).ToList();
        }

        public List<Floor> GetFloorsList()
        {
            var floors = new List<Floor>();
            const string sql = StoredProcedures.Get_Floors;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;              
                var ds = _sqlHelper.GetDataSet(cmd);
                if (ds != null)
                {                   
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var floor = new Floor
                        {
                            BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                            FloorTypeId = Convert.ToInt32(row["FloorTypeId"].ToString()),
                            CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                            FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                            FloorName = row["FloorName"].ToString(),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            Alias = row["Alias"].ToString(),
                            AliasSequence = row["Sequence"].ToString(),
                            FloorCode = row["FloorCode"].ToString(),
                            FloorType =
                                new FloorTypes
                                {
                                    FloorType = row["FloorType"].ToString(),
                                    FloorTypeId = Convert.ToInt32(row["FloorTypeId"].ToString())
                                },
                            Building = new Buildings
                            {
                                BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                                BuildingName = row["BuildingName"].ToString(),
                                BuildingCode = row["BuildingCode"].ToString(),
                                SiteCode = row["SiteCode"].ToString(),
                                IsActive = Convert.ToBoolean(row["BuildingStatus"].ToString())
                            }
                        };

                        //var floorPlan = floorPlans.FirstOrDefault(x => x.FloorId == floor.FloorId && x.FloorPlanTypeId == 1 && x.IsActive && x.IsCurrent);
                        //var floorPlan = floorPlans.FirstOrDefault(x => x.FloorId == floor.FloorId && x.FloorPlanTypeId == (floorTypeId.HasValue ? floorTypeId.Value : 1) && x.IsActive && x.IsCurrent);
                        //if (floorPlan != null)
                        //{
                        //    floor.FileName = floorPlan.FileName;
                        //    floor.ImagePath = floorPlan.ImagePath;
                        //    floor.FloorPlanId = floorPlan.FloorPlanId.Value;
                        //    floor.EffectiveDate = Convert.ToDateTime(floorPlan.EffectiveDate);
                        //    floor.EffectiveDateTimeSpan = Conversion.ConvertToTimeSpan(Convert.ToDateTime(floorPlan.EffectiveDate));
                        //}
                        //floor.FloorPlans = floorPlans.Where(x => x.FloorId == floor.FloorId && x.IsActive).ToList();
                        //floor.Routes = routeMaster.Where(x => x.FloorId == floor.FloorId).ToList();

                        floors.Add(floor);
                    }
                }
            }
            return floors.GroupBy(x => x.FloorId).Select(x => x.FirstOrDefault()).ToList();
        }


        private  IEnumerable<DrawingpathwayFiles> GetUploadedDrawings(string floorPlanId)
        {
            var DrawingpathwayFiles = new List<DrawingpathwayFiles>();
            const string sql = StoredProcedures.GetUploadedDrawings;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@floorPlanId", floorPlanId);

                var dt = _sqlHelper.GetDataSet(cmd);
                if (dt != null)
                    DrawingpathwayFiles = _sqlHelper.ConvertDataTable<DrawingpathwayFiles>(dt.Tables[0]);
            }
            return DrawingpathwayFiles;

        }
        public  DrawingpathwayFiles GetUploadedDrawing(string fileId)
        {
            return GetUploadedDrawings(Convert.ToString(fileId)).FirstOrDefault();
        }
        public  Floor GetFloorPlans(int floorId)
        {
            var floors = new List<Floor>();
            const string sql = StoredProcedures.Get_Floors;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@floorTypeId", (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@buildingId", (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@floorId", floorId);
                var ds = _sqlHelper.GetDataSet(cmd);
                if (ds != null)
                {
                    var floorPlans = _sqlHelper.ConvertDataTable<FloorPlan>(ds.Tables[3]);
                    var drawingTypes = _sqlHelper.ConvertDataTable<FloorTypes>(ds.Tables[4]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var floor = new Floor
                        {
                            BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                            FloorTypeId = Convert.ToInt32(row["FloorTypeId"].ToString()),
                            CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                            FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                            FloorName = row["FloorName"].ToString(),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            Alias = row["Alias"].ToString(),
                            AliasSequence = Convert.ToString(row["Sequence"].ToString()),
                            FloorCode = row["FloorCode"].ToString(),

                            FloorType = new FloorTypes
                            {
                                FloorType = row["FloorType"].ToString(),
                                FloorTypeId = Convert.ToInt32(row["FloorTypeId"].ToString())
                            },
                            Building = new Buildings
                            {
                                BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                                BuildingName = row["BuildingName"].ToString(),
                                BuildingCode = row["BuildingCode"].ToString(),
                                SiteCode = row["SiteCode"].ToString(),
                            }
                        };
                        var floorPlan = floorPlans.FirstOrDefault(x => x.FloorId == floor.FloorId && x.FloorPlanTypeId == 1 && x.IsActive && x.IsCurrent);
                        if (floorPlan != null)
                        {
                            floor.FileName = floorPlan.FileName;
                            floor.ImagePath = floorPlan.ImagePath;
                            floor.FloorPlanId = floorPlan.FloorPlanId.Value;
                            floor.EffectiveDate = Convert.ToDateTime(floorPlan.EffectiveDate);
                            floor.EffectiveDateTimeSpan = Conversion.ConvertToTimeSpan(Convert.ToDateTime(floorPlan.EffectiveDate));
                        }
                        floor.FloorPlans = floorPlans.Where(x => x.FloorId == floor.FloorId).ToList();
                        foreach (var item in floor.FloorPlans)
                            item.DrawingType = drawingTypes.FirstOrDefault(x => x.FloorTypeId == item.FloorPlanTypeId);

                        floors.Add(floor);
                    }
                }
            }
            return floors.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Floor> GetFloors()
        {
            return GetFloors(null, null, null, null);
        }

        public  FloorPlan GetFloorPlans(Guid? floorPlanId)
        {
            FloorPlan objFloorPlan = new FloorPlan();
            const string sql = StoredProcedures.Get_FloorPlans;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@floorPlanId", floorPlanId);
                var ds = _sqlHelper.GetDataSet(cmd);
                if (ds != null)
                {
                    //objFloorPlan = _sqlHelper.ConvertDataTable<FloorPlan>(ds.Tables[0]).FirstOrDefault();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(row["FloorPlanId"].ToString()))
                            objFloorPlan.FloorPlanId = Guid.Parse(row["FloorPlanId"].ToString());

                        objFloorPlan.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                        objFloorPlan.FloorPlanTypeId = Convert.ToInt32(row["FloorPlanTypeId"].ToString());
                        objFloorPlan.ImagePath = row["ImagePath"].ToString();
                        objFloorPlan.FileName = row["FileName"].ToString();
                        objFloorPlan.IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString());
                        objFloorPlan.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
                        objFloorPlan.CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString());
                        objFloorPlan.Floor = new Floor()
                        {
                            BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                            CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                            FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                            FloorName = row["FloorName"].ToString(),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            Alias = row["Alias"].ToString(),
                            FloorCode = row["FloorCode"].ToString(),
                            Building = new Buildings
                            {
                                BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                                BuildingName = row["BuildingName"].ToString(),
                                BuildingCode = row["BuildingCode"].ToString()
                            }
                        };
                    }
                }
            }
            return objFloorPlan;
        }


        //public  List<FloorPlan> GetFloorPlans()
        //{
        //    return CommonRepository.GetTableData<FloorPlan>(StoredProcedures.Get_FloorPlans);
        //}


        //public  List<Floor> GetFilterFloors(string floorId, int? buildingId)
        //{
        //    return GetFloors(null, buildingId, null);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Floor> GetFloorsByFloorType(int floorTypeId, int? CheckFileEmpty)
        {
            return GetFloors(floorTypeId, null, null, CheckFileEmpty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Floor> GetAssetLocations()
        {
            var floors = new List<Floor>();
            const string sql = StoredProcedures.Get_Floors;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(cmd);
                var floorPlans = _sqlHelper.ConvertDataTable<FloorPlan>(ds.Tables[3]);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var floor = new Floor
                    {
                        BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                        CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                        CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                        FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                        FloorName = row["FloorName"].ToString(),
                        IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                        Alias = row["Alias"].ToString(),
                        FloorCode = row["FloorCode"].ToString(),
                        Building = new Buildings
                        {
                            BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                            BuildingName = row["BuildingName"].ToString(),
                            BuildingCode = row["BuildingCode"].ToString()
                        }
                    };
                    var floorPlan = floorPlans.FirstOrDefault(x => x.FloorId == floor.FloorId && x.FloorPlanTypeId == 1 && x.IsActive && x.IsCurrent);
                    if (floorPlan != null)
                    {
                        floor.FileName = floorPlan.FileName;
                        floor.ImagePath = floorPlan.ImagePath;
                        floor.FloorPlanId = floorPlan.FloorPlanId.Value;
                        floor.EffectiveDate = Convert.ToDateTime(floorPlan.EffectiveDate);
                        floor.EffectiveDateTimeSpan = Conversion.ConvertToTimeSpan(Convert.ToDateTime(floorPlan.EffectiveDate));
                    }
                    floors.Add(floor);
                }
            }
            return floors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public  Floor GetFloor(int floorId)
        {
            var floors = new List<Floor>();
            const string sql = StoredProcedures.Get_Floors;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@floorId", floorId);
                var dt = _sqlHelper.GetDataTable(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    var floor = new Floor
                    {
                        BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                        CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                        CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                        FileName = row["FileName"].ToString(),
                        FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                        FloorName = row["FloorName"].ToString(),
                        ImagePath = row["ImagePath"].ToString(),
                        User = new UserProfile { UserName = row["UserName"].ToString() },
                        Building = new Buildings
                        {
                            BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                            BuildingName = row["BuildingName"].ToString(),
                            BuildingCode = row["BuildingCode"].ToString()
                        },
                    };
                    if (!string.IsNullOrEmpty(row["FloorPlanId"].ToString()))
                        floor.FloorPlanId = Guid.Parse(row["FloorPlanId"].ToString());

                    floor.FloorPlans = _floorPlanRepository.GetFloorPlans(floor.FloorId);
                    floors.Add(floor);
                }
            }
            return floors.FirstOrDefault(x => x.FloorId == floorId);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public  Floor GetFloorDescription(int floorId)
        {
            var floors = new List<Floor>();
            const string sql = StoredProcedures.Get_Floors;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@floorId", floorId);
                var dt = _sqlHelper.GetDataTable(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    var floor = new Floor
                    {
                        BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                        FloorPlanId = (!string.IsNullOrEmpty(row["FloorPlanId"].ToString())) ? Guid.Parse(row["FloorPlanId"].ToString()) : Guid.Empty,
                        FloorName = row["FloorName"].ToString(),
                        Alias = row["Alias"].ToString(),
                        FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                        FloorCode = row["FloorCode"].ToString(),
                        IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                        ImagePath = row["ImagePath"].ToString(),
                        Building = new Buildings
                        {
                            BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                            BuildingName = row["BuildingName"].ToString(),
                            BuildingCode = row["BuildingCode"].ToString(),
                            BuildingTypeId = Convert.ToInt32(row["BuildingTypeId"].ToString()),
                        }
                    };

                    floors.Add(floor);
                }
            }
            return floors.FirstOrDefault(x => x.FloorId == floorId);

        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public  Floor GetFloorDetails(int floorId)
        {
           return GetFloor(floorId);           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public  IEnumerable<Floor> GetFloorByAssets(IEnumerable<int> values)
        {
            var floors = new List<Floor>();
            const string sql = StoredProcedures.Get_FloorByAssets;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@assetdId", string.Join(",", values.Select(n => n.ToString()).ToArray()));
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null)
                {
                    floors = _sqlHelper.ConvertDataTable<Floor>(dt);
                }
            }

            return floors;
        }

        public  List<BDO.FloorAssetStatus> GetFloorByAssetsWithStatus(string assetId, string ascIds, int insptype)
        {
            var floorAssetStatus = new List<BDO.FloorAssetStatus>();
            const string sql = StoredProcedures.Get_FloorByAssetsWithStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetdId", assetId);
                command.Parameters.AddWithValue("@ascIds", ascIds);
                command.Parameters.AddWithValue("@insptype", insptype);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    floorAssetStatus = _sqlHelper.ConvertDataTable<BDO.FloorAssetStatus>(ds.Tables[0]);
            }
            return floorAssetStatus;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //private  IEnumerable<TFloorAssets> GetFloorAssetsShapes(int shapeId)
        //{
        //    var lists = new List<TFloorAssets>();
        //    var locations = LocationRepository.GetLocations().Where(x => x.ShapeId == shapeId && x.IsCurrentLocation).ToList();
        //    foreach (var location in locations)
        //    {
        //        var floosAsset = new TFloorAssets { FloorAssetsId = location.FloorAssetsId };
        //        lists.Add(floosAsset);
        //    }
        //    return lists;
        //}      

        public  List<Floor> Floors(int floorTypeId, int? CheckFileEmpty)
        {
            return GetFloorsByFloorType(floorTypeId, CheckFileEmpty);
        }

        #region
        public  Tuple<string, string, string, string, string, string, string> GetAssetsIds(string assetName, string buildingName, string floorName, string stopName, string assetsCategory, string assetsSubCategory)
        {
            const string sql = StoredProcedures.Get_BuildingFloorId;
            string assetId = "", buildingId = "", floorId = "", stopCode = "", buildingCode = "", assetsCategoryId = "", assetsSubCategoryId = "";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AssetName", assetName);
                command.Parameters.AddWithValue("@BuildingName", buildingName);
                command.Parameters.AddWithValue("@FloorName", floorName);
                command.Parameters.AddWithValue("@StopName", stopName);
                command.Parameters.AddWithValue("@AssetsCategory", assetsCategory);
                command.Parameters.AddWithValue("@AssetsSubCategory", assetsSubCategory);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                        assetId = Convert.ToString(ds.Tables[0].Rows[0].ItemArray[0]);
                    if (String.IsNullOrEmpty(assetId) || !int.TryParse(assetId, out int resAssetId))
                        assetId = "0";

                    if (ds.Tables.Count > 1)
                        buildingId = Convert.ToString(ds.Tables[1].Rows[0].ItemArray[0]);
                    if (String.IsNullOrEmpty(buildingId) || !int.TryParse(buildingId, out int resBuildingId))
                        buildingId = "0";

                    if (ds.Tables.Count > 2)
                        floorId = Convert.ToString(ds.Tables[2].Rows[0].ItemArray[0]);
                    if (String.IsNullOrEmpty(floorId) || !int.TryParse(floorId, out int resFloorId))
                        floorId = "0";

                    if (ds.Tables.Count > 4)
                        stopCode = Convert.ToString(ds.Tables[4].Rows[0].ItemArray[0]);
                    if (String.IsNullOrEmpty(stopCode))
                    {
                        stopCode = stopName.Length >= 2 ? stopName.Substring(0, 2).ToUpper() : stopName.ToUpper();
                    }

                    if (ds.Tables.Count > 5)
                        buildingCode = Convert.ToString(ds.Tables[5].Rows[0].ItemArray[0]);
                    if (String.IsNullOrEmpty(buildingCode))
                    {
                        buildingCode = buildingName.Length >= 2 ? buildingName.Substring(0, 2).ToUpper() : buildingName.ToUpper();
                    }

                    if (ds.Tables.Count > 6)
                        assetsCategoryId = Convert.ToString(ds.Tables[6].Rows[0].ItemArray[0]);
                    if (String.IsNullOrEmpty(assetsCategoryId))
                        assetsCategoryId = "0";

                    if (ds.Tables.Count > 7)
                        assetsSubCategoryId = Convert.ToString(ds.Tables[7].Rows[0].ItemArray[0]);
                    if (String.IsNullOrEmpty(assetsSubCategoryId))
                        assetsSubCategoryId = "0";
                }
            }
            return new Tuple<string, string, string, string, string, string, string>(assetId, buildingId, floorId, stopCode, buildingCode, assetsCategoryId, assetsSubCategoryId);
        }

        public List<FloorPlan> FloorPlans(int floorId)
        {
            throw new NotImplementedException();
        }

        public List<FloorShapes> FloorShapes(int floorId)
        {
            throw new NotImplementedException();
        }

        public List<FloorPlan> GetFloorPlans()
        {
            throw new NotImplementedException();
        }

        public List<Wing> Wing(int floorId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
