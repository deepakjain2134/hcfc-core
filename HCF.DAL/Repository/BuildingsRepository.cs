using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public  class BuildingsRepository: IBuildingsRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public BuildingsRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newBuildings"></param>
        /// <returns></returns>
        public int Save(Buildings newBuildings)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Building;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SiteCode", newBuildings.SiteCode);
                command.Parameters.AddWithValue("@BuildingTypeId", newBuildings.BuildingTypeId);
                command.Parameters.AddWithValue("@CreatedBy", newBuildings.CreatedBy);
                command.Parameters.AddWithValue("@BuildingName", newBuildings.BuildingName);
                command.Parameters.AddWithValue("@BuildingCode", newBuildings.BuildingCode);
                command.Parameters.AddWithValue("@CityId", newBuildings.CityId);
                command.Parameters.AddWithValue("@Address", newBuildings.Address);
                command.Parameters.AddWithValue("@IsActive", newBuildings.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters.AddWithValue("@UserId", newBuildings.BuildingAssignUserId);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newBuilding"></param>
        /// <returns></returns>
        public int UpdateBuliding(Buildings newBuilding)
        {
            const string sql = StoredProcedures.Update_Building;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BuildingId", newBuilding.BuildingId);
                command.Parameters.AddWithValue("@BuildingTypeId", newBuilding.BuildingTypeId);
                command.Parameters.AddWithValue("@BuildingName", newBuilding.BuildingName);
                command.Parameters.AddWithValue("@BuildingCode", newBuilding.BuildingCode);
                command.Parameters.AddWithValue("@CityId", newBuilding.CityId);
                command.Parameters.AddWithValue("@Address", newBuilding.Address);
                command.Parameters.AddWithValue("@SiteCode", newBuilding.SiteCode);
                command.Parameters.AddWithValue("@IsActive", newBuilding.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", newBuilding.CreatedBy);
                command.Parameters.AddWithValue("@UserId", newBuilding.BuildingAssignUserId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                var newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
                return newId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Buildings> GetBuildings()
        {
            var buildings = new List<Buildings>();
            string sql = StoredProcedures.Get_Building;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var building = new Buildings
                        {
                            BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
                            BuildingName = row["BuildingName"].ToString(),
                            BuildingCode = row["BuildingCode"].ToString(),
                            CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            BuildingType = new BuildingType(),
                            BuildingTypeId = Convert.ToInt32(row["BuildingTypeId"].ToString()),
                            SiteCode = row["SiteCode"].ToString(),
                            SiteName = row["SiteName"] != null ? row["SiteName"].ToString() : "",
                            BuildingAssignUserId = Convert.ToInt32(Convert.ToString(row["UserId"])),
                            BuildingAssignUser = Convert.ToString(row["Supervisor"]),
                            SiteId = Convert.ToInt32(row["SiteId"].ToString()),
                            SiteActive = Convert.ToBoolean(row["SiteActive"].ToString()),
                            Address = row["Address"].ToString()
                        };
                        building.BuildingType.Name = row["BuildingTypeName"].ToString();
                        building.BuildingType.BuildingTypeId = building.BuildingTypeId;

                        if (!string.IsNullOrEmpty(row["CityId"].ToString()))
                        {
                            building.CityId = int.TryParse(Convert.ToString(row["CityId"]), out int res2) ? Convert.ToInt32(row["CityId"]) : 0;
                            building.StateId = int.TryParse(Convert.ToString(row["StateId"]), out int res1) ? Convert.ToInt32(row["StateId"]) : 0;
                            building.CityName = Convert.ToString(row["CityName"]);
                            building.StateCode = Convert.ToString(row["StateCode"]);
                        }
                        buildings.Add(building);
                    }
                }
            }
            return buildings;
        }

        public List<Site> GetAllBuildingsbySite(string sitecode)
        {
            List<Site> sites = new();
            string sql = StoredProcedures.Get_AllBuilding;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SiteCode", sitecode);
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null)
                {
                    sites = _sqlHelper.ConvertDataTable<Site>(dt);
                    var buildings = _sqlHelper.ConvertDataTable<Buildings>(dt);
                    sites = sites.GroupBy(x => x.Code).Select(grp => grp.First()).ToList();
                    foreach (var site in sites)
                    {
                        site.Buildings = buildings.Where(x => x.SiteCode == site.Code).ToList();
                    }
                }
            }
            return sites;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<BuildingType> GetBuildingByType()
        {
            List<BuildingType> lists = new List<BuildingType>();           
            string sql = StoredProcedures.Get_Building;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                DataSet dt = _sqlHelper.GetDataSet(cmd);
                if (dt != null)
                {
                    var buildings = new List<Buildings>();
                    lists = _sqlHelper.ConvertDataTable<BuildingType>(dt.Tables[1]);
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        var building = new Buildings
                        {
                            BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
                            BuildingName = row["BuildingName"].ToString(),
                            BuildingCode = row["BuildingCode"].ToString(),
                            CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            BuildingTypeId = Convert.ToInt32(row["BuildingTypeId"].ToString())
                        };
                        buildings.Add(building);
                    }
                    foreach (BuildingType type in lists)
                    {
                        type.Buildings = buildings.Where(x => x.BuildingTypeId == type.BuildingTypeId && x.IsActive).ToList();
                    }
                }
            }
            return lists;
        }
        public  List<BuildingDetails> GetBuildingDetails()
        {
            List<BuildingDetails> BuildingDetails = new();
            string sql = StoredProcedures.Get_BuildingDetails;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null)
                    BuildingDetails = _sqlHelper.ConvertDataTable<BuildingDetails>(dt);
            }
            return BuildingDetails;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Buildings> GetBuildingFloors(string siteCode = null, string stateId = null, string cityId = null)
        {
            var buildings = new List<Buildings>();
            string sql = StoredProcedures.Get_Building;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SiteCode", siteCode);
                cmd.Parameters.AddWithValue("@StateID", stateId);
                cmd.Parameters.AddWithValue("@CityID", cityId);
                var ds = _sqlHelper.GetDataSet(cmd);
                if (ds != null)
                {
                    var floors = ConvertToFloorList(ds.Tables[2]);
                    // var floors = _sqlHelper.ConvertDataTable<Floor>(ds.Tables[2]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var building = new Buildings
                        {
                            BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
                            BuildingName = row["BuildingName"].ToString(),
                            BuildingCode = row["BuildingCode"].ToString(),
                            CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            BuildingType = new BuildingType(),
                            BuildingTypeId = Convert.ToInt32(row["BuildingTypeId"].ToString()),
                            SiteCode = row["SiteCode"].ToString(),
                            SiteName = row["SiteName"].ToString(),
                            SiteActive = Convert.ToBoolean(row["SiteActive"].ToString()),
                            ///*Added on 03/04/2020* START*/
                            StateId = Convert.ToInt32(string.IsNullOrEmpty(Convert.ToString(row["StateId"].ToString())) ? "0" : row["StateId"].ToString()),
                            StateName = string.IsNullOrEmpty(row["StateName"].ToString()) ? "" : row["StateName"].ToString(),
                            CityId = Convert.ToInt32(string.IsNullOrEmpty(Convert.ToString(row["CityId"].ToString())) ? "0" : row["CityId"].ToString()),
                            CityName = string.IsNullOrEmpty(row["CityName"].ToString()) ? "" : row["CityName"].ToString(),

                            /*Added on 03/04/2020* END*/

                        };


                        building.BuildingType.Name = row["BuildingTypeName"].ToString();
                        building.BuildingType.BuildingTypeId = building.BuildingTypeId;
                        building.Floor = floors.Where(x => x.BuildingId == building.BuildingId).ToList();
                        foreach (var newFloor in building.Floor)
                        {
                            newFloor.Assets = GetBuildingAssets(ds.Tables[3], newFloor.FloorId);
                        }
                        buildings.Add(building);
                    }
                }              
            }
            return buildings.Where(x => x.IsActive).ToList();
        }

        public  List<Buildings> GetBuildingFloors()
        {
            var buildings = new List<Buildings>();
            string sql = StoredProcedures.Get_Building;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(cmd);
                //var floors = _sqlHelper.ConvertDataTable<Floor>(ds.Tables[2]);

                var floors = ConvertToFloorList(ds.Tables[2]);



                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var building = new Buildings
                    {
                        BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
                        BuildingName = row["BuildingName"].ToString(),
                        BuildingCode = row["BuildingCode"].ToString(),
                        CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                        IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                        BuildingType = new BuildingType(),
                        BuildingTypeId = Convert.ToInt32(row["BuildingTypeId"].ToString()),
                        SiteCode = row["SiteCode"].ToString(),
                        SiteName = row["SiteName"].ToString(),
                        SiteActive = Convert.ToBoolean(row["SiteActive"].ToString()),
                        ///*Added on 03/04/2020* START*/
                        StateId = Convert.ToInt32(string.IsNullOrEmpty(Convert.ToString(row["StateId"].ToString())) ? "0" : row["StateId"].ToString()),
                        StateName = string.IsNullOrEmpty(row["StateName"].ToString()) ? "" : row["StateName"].ToString(),
                        CityId = Convert.ToInt32(string.IsNullOrEmpty(Convert.ToString(row["CityId"].ToString())) ? "0" : row["CityId"].ToString()),
                        CityName = string.IsNullOrEmpty(row["CityName"].ToString()) ? "" : row["CityName"].ToString(),

                        /*Added on 03/04/2020* END*/

                    };


                    building.BuildingType.Name = row["BuildingTypeName"].ToString();
                    building.BuildingType.BuildingTypeId = building.BuildingTypeId;
                    building.Floor = floors.Where(x => x.BuildingId == building.BuildingId).ToList();


                    buildings.Add(building);
                }
            }
            return buildings.Where(x => x.IsActive).ToList();
        }

        private List<Floor> ConvertToFloorList(DataTable dataTable)
        {
            var floors = new List<Floor>();
            foreach (DataRow row in dataTable.Rows)
            {
                var floor = new Floor();
                floor.Alias = row["alias"].ToString();
                //floor.AliasSequence = row["AliasSequence"].ToString();
                floor.BuildingId = Convert.ToInt32(row["BuildingId"].ToString());
                floor.FloorCode= row["FloorCode"].ToString();
                floor.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                floor.FloorTypeId = Convert.ToInt32(row["FloorTypeId"].ToString());
                floor.FloorName = row["FloorName"].ToString();
                floor.IsActive =Convert.ToBoolean(row["IsActive"].ToString());
                floor.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
                floor.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
                floor.ImagePath = row["ImagePath"].ToString();
                floor.FileName = row["FileName"].ToString();               

                if (!string.IsNullOrEmpty(row["FloorPlanId"].ToString()))
                {
                    floor.FloorPlanId = Guid.Parse(row["FloorPlanId"].ToString());
                    floor.ImagePath = row["ImagePath"].ToString();
                }

                floors.Add(floor);
            }
            return floors;
        }

        public List<Buildings> ConvertToBuildings(DataTable dt)
        {
            var buildings = _sqlHelper.ConvertDataTable<Buildings>(dt).GroupBy(x => x.BuildingId).Select(x => x.First()).ToList();
            var floors = _sqlHelper.ConvertDataTable<Floor>(dt);
            foreach (var item in buildings)
                item.Floor = floors.Where(x => x.BuildingId == item.BuildingId).ToList();
            return buildings;
        }

        private  List<Assets> GetBuildingAssets(DataTable dt, int floorId)
        {
            var assets = new List<Assets>();
            string expression;
            expression = "FloorId = " + floorId;
            DataRow[] foundRows = dt.Select(expression);
            if (foundRows.Any())
            {
                DataTable assetsOnBuilding = foundRows.CopyToDataTable();
                assets = _sqlHelper.ConvertDataTable<Assets>(assetsOnBuilding);
            }
            return assets;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="ascIds"></param>
        /// <returns></returns>
        public  List<Buildings> GetFilterBuildings(string assetId, string ascIds)
        {
            List<Buildings> lists = new();
            string sql = StoredProcedures.Get_FilterBuildings;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@AssetId", assetId);
                command.Parameters.AddWithValue("@ascIds", ascIds);
                command.CommandType = CommandType.StoredProcedure;
                DataSet ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[0]);
                }
            }
            return lists;
        }
        
        #region BBI

        public  int Insert_BBI(BuildingDetails BuildingDetails)
        {
            int newId;
            const string sql = StoredProcedures.Insert_BBI;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Beds", BuildingDetails.Beds);
                command.Parameters.AddWithValue("@BuildingId", BuildingDetails.BuildingId);
                command.Parameters.AddWithValue("@CreatedBy", BuildingDetails.CreatedBy);
                command.Parameters.AddWithValue("@GovEnvSusp", BuildingDetails.GovEnvSusp);
                command.Parameters.AddWithValue("@IsDeleted", BuildingDetails.IsDeleted);
                command.Parameters.AddWithValue("@OpenSPFI", BuildingDetails.OpenSPFI);
                command.Parameters.AddWithValue("@PercentageRenovated", BuildingDetails.PercentageRenovated);
                command.Parameters.AddWithValue("@PercentageSqrFt", BuildingDetails.PercentageSqrFt);
                command.Parameters.AddWithValue("@Sprinkled", BuildingDetails.Sprinkled);
                command.Parameters.AddWithValue("@SquareFtRange", BuildingDetails.SquareFtRange);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public  bool DeleteBBI(int Id)
        {
            bool status;
            const string sql = StoredProcedures.Delete_BBI;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", Id);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        #endregion

        public List<Buildings> GetCampus(int userid,  int epdetailid)
        {
            var list = new List<Buildings>();
            const string sql = StoredProcedures.Get_Campus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userid", userid);
                command.Parameters.AddWithValue("@epId", epdetailid);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var campus = new Buildings
                        {
                            SiteId = Convert.ToInt32(item["SiteId"].ToString()),
                            SiteName = item["Name"].ToString(),
                            SiteCode = item["Code"].ToString(),
                            SiteActive = Convert.ToBoolean(item["IsActive"].ToString()),
                            IsInspectionDone=Convert.ToBoolean(item["IsInspectionDone"].ToString())
                        };
                        list.Add(campus);
                    }
                }
            }
            return list;
        }
    }
}
