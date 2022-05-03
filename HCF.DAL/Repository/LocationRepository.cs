using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IAssetsRepository _assetsRepository;
        public LocationRepository(ISqlHelper sqlHelper, IAssetsRepository assetsRepository)
        {
            _assetsRepository = assetsRepository;
            _sqlHelper = sqlHelper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TFloorAssetsLocations> GetLocations()
        {
            return GetLocations(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TFloorAssetsLocations> GetLocations(bool status)
        {
            return GetLocations().Where(x => x.IsCurrentLocation == status).ToList();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <returns></returns>
        private List<TFloorAssetsLocations> GetLocations(int? floorAssetId)
        {
            var locations = new List<TFloorAssetsLocations>();
            const string sql = StoredProcedures.Get_Locations;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    locations = _sqlHelper.ConvertDataTable<TFloorAssetsLocations>(dt);
            }
            return locations;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newLocation"></param>
        /// <returns></returns>
        public bool AddFloorAssetsLocation(TFloorAssetsLocations newLocation)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TFloorAssetLocation;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorAssetsId", newLocation.FloorAssetsId);
                command.Parameters.AddWithValue("@Xcoordinate", newLocation.Xcoordinate);
                command.Parameters.AddWithValue("@Ycoordinate", newLocation.Ycoordinate);
                command.Parameters.AddWithValue("@AreaCode", newLocation.AreaCode);
                command.Parameters.AddWithValue("@ZoomLevel", newLocation.ZoomLevel);
                command.Parameters.AddWithValue("@CreatedBy", newLocation.CreatedBy);
                command.Parameters.AddWithValue("@FloorId", newLocation.FloorId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }

            return (newId > 0) ? true : false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newLocation"></param>
        /// <returns></returns>
        public bool UpdateFloorAssetsLocation(TFloorAssetsLocations newLocation)
        {
            bool status;
            const string sql = StoredProcedures.Update_TFloorAssetLocation;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorAssetsId", newLocation.FloorAssetsId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;

        }


        #region Location Master

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newStopMaster"></param>
        /// <returns></returns>
        public int SaveLocation(StopMaster newStopMaster)
        {
            int newId = 0;
            const string sql = StoredProcedures.Insert_Stops;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StopName", newStopMaster.StopName);
                command.Parameters.AddWithValue("@StopCode", newStopMaster.StopCode);
                command.Parameters.AddWithValue("@FloorId", newStopMaster.FloorId);
                command.Parameters.AddWithValue("@CreatedBy", newStopMaster.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", newStopMaster.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationMasterId"></param>
        /// <param name="stopCode"></param>
        /// <returns></returns>
        public List<StopMaster> GetLocationsMaster(int? locationMasterId, string stopCode = null)
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

        public List<StopMaster> GetStopNotExistInFe(int assetId, int floorId)
        {
            var stops = new List<StopMaster>();
            const string sql = StoredProcedures.Get_StopNotExistInFE;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AssetId", assetId);
                command.Parameters.AddWithValue("@FloorId", floorId);
                var ds = _sqlHelper.GetDataSet(command);
                stops = _sqlHelper.ConvertDataTable<StopMaster>(ds.Tables[0]);
            }
            return stops;
        }


        public StopMaster GetStopByCode(string stopCode)
        {
            return GetLocationsMaster(null, stopCode).FirstOrDefault();
        }


        public List<TFloorAssets> GetStopAssets(int stopId)
        {
            var floorAssets = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_StopAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@stopId", stopId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var dt = ds.Tables[0];
                    var assets = _assetsRepository.ConvertToAssets(dt);
                    floorAssets = _sqlHelper.ConvertDataTable<TFloorAssets>(dt).GroupBy(x => x.FloorAssetsId).Select(x => x.FirstOrDefault()).ToList();

                    foreach (var item in assets)
                    {
                        var freqlist = new List<FrequencyMaster>();
                        string expression;
                        string sortOrder = "DisplayName DESC";
                        expression = "AssetId = '" + item.AssetId + "'";
                        DataRow[] foundRows = ds.Tables[1].Select(expression, sortOrder);
                        foreach (DataRow assetrow in foundRows)
                        {
                            FrequencyMaster objfreq = new FrequencyMaster()
                            {
                                FrequencyId = Convert.ToInt32(assetrow["FrequencyId"].ToString()),
                                DisplayName = assetrow["DisplayName"].ToString(),
                                Days = Convert.ToInt32(assetrow["Days"].ToString()),
                                IsActive = Convert.ToBoolean(assetrow["IsActive"].ToString())
                            };
                            freqlist.Add(objfreq);
                        }
                        item.AssetFrequency = freqlist;
                    }

                    foreach (var item in floorAssets)
                    {
                        item.Assets = assets.FirstOrDefault(x => x.AssetId == item.AssetId);
                    }
                }
            }
            return floorAssets;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newStopMaster"></param>
        /// <returns></returns>
        public int UpdateLocation(StopMaster newStopMaster)
        {
            int newId;
            const string sql = StoredProcedures.Update_Stops;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StopId", newStopMaster.StopId);
                command.Parameters.AddWithValue("@StopName", newStopMaster.StopName);
                command.Parameters.AddWithValue("@StopCode", newStopMaster.StopCode);
                command.Parameters.AddWithValue("@FloorId", newStopMaster.FloorId);
                command.Parameters.AddWithValue("@ALTERdBy", newStopMaster.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", newStopMaster.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;

        }

        #endregion

        #region Route Master

        public int SaveRoute(RouteMaster newRouteMaster)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Route;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RouteId", newRouteMaster.RouteId);
                command.Parameters.AddWithValue("@RouteNo", newRouteMaster.RouteNo);
                command.Parameters.AddWithValue("@FloorId", newRouteMaster.FloorId);
                command.Parameters.AddWithValue("@CreatedBy", newRouteMaster.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", newRouteMaster.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public List<RouteMaster> GetRouteMaster(int? routeId)
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
                        //if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                        //{
                        //    objrouteMaster.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                        //    objrouteMaster.BuildingID = Convert.ToInt32(row["BuildingID"].ToString());
                        //    objrouteMaster.Floor = new Floor
                        //    {
                        //        FloorName = row["FloorName"].ToString(),
                        //        FloorId = objrouteMaster.FloorId.Value,
                        //        BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString()) ? Convert.ToInt32(row["BuildingId"].ToString()) : 0,
                        //        Building = new Buildings
                        //        {
                        //            BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString()) ? Convert.ToInt32(row["BuildingId"].ToString()) : 0,
                        //            BuildingName = row["BuildingName"].ToString(),
                        //            BuildingCode = row["BuildingCode"].ToString()
                        //        }
                        //    };
                        //}
                        objrouteMaster.StopsRouteMapping = GetLocationrouteMapping(null, objrouteMaster.RouteId);
                        routes.Add(objrouteMaster);
                    }
                }
            }
            return routes;
        }

        public List<RouteMaster> GetRouteByAsset(int assetId)
        {
            var routes = new List<RouteMaster>();
            string sql = StoredProcedures.Get_RouteByAsset;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var objrouteMaster = new RouteMaster
                        {
                            RouteId = Convert.ToInt32(row["RouteId"].ToString()),
                            RouteNo = row["RouteNo"].ToString(),
                            AssetCounts = Convert.ToInt32(row["AssetCounts"].ToString()),
                            StopCount = Convert.ToInt32(row["StopCount"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            IsDefault = Convert.ToBoolean(row["IsDefault"].ToString()),
                        };
                        GetAssetsRouteCounts(ds, objrouteMaster);
                        //objrouteMaster.StopsRouteMapping = GetLocationrouteMapping(null, objrouteMaster.RouteId);
                        routes.Add(objrouteMaster);
                    }
                }
            }
            return routes;
        }

        private static void GetAssetsRouteCounts(DataSet ds, RouteMaster objrouteMaster)
        {
            var tblFiltered = new DataTable();
            var rows = ds.Tables[1].AsEnumerable()
                                         .Where(r => r.Field<int>("RouteId") == objrouteMaster.RouteId);
            if (rows.Any())
                tblFiltered = rows.CopyToDataTable();

            if (tblFiltered.Rows.Count > 0)
            {
                foreach (DataRow item in tblFiltered.Rows)
                {
                    var assets = new Assets
                    {
                        AssetId = Convert.ToInt32(item["AssetId"].ToString()),
                        Name = item["Name"].ToString(),
                        Count = Convert.ToInt32(item["AssetCount"].ToString())
                    };
                    objrouteMaster.Assets.Add(assets);
                }
            }
        }

        public List<RouteMaster> GetRouteByFloor(int? floorId)
        {
            List<RouteMaster> routes = new List<RouteMaster>();
            string sql = StoredProcedures.Get_RouteByFloor;
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorId", floorId > 0 ? floorId : null);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    routes = _sqlHelper.ConvertDataTable<RouteMaster>(ds.Tables[0]);
                }
            }
            return routes;
        }

        #endregion

        #region Location Route Mapping

        public int SaveLocationRouteMapping(StopsRouteMapping newStopsRouteMapping, string locationsId)
        {
            int newId;
            const string sql = StoredProcedures.Insert_StopRouteMapping;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LocationsId", string.IsNullOrEmpty(locationsId) ? locationsId : locationsId.Remove(locationsId.Length - 1));
                command.Parameters.AddWithValue("@RouteId", newStopsRouteMapping.RouteId);
                //command.Parameters.AddWithValue("@IsActive", newStopsRouteMapping.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public List<StopsRouteMapping> GetLocationrouteMapping(int? floorId, int? routeId)
        {
            List<StopsRouteMapping> objstopsRouteMapping = new List<StopsRouteMapping>();
            string sql = StoredProcedures.Get_StopRouteMapping;
            using (var command = new SqlCommand(sql))
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

        #endregion

        #region FileDetails
        public List<InspectionEPDocs> GetFileDetailsById(int fileId)
        {
            List<InspectionEPDocs> document = new List<InspectionEPDocs>();
            string sql = StoredProcedures.Get_FileDetailsById;
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@fileId", fileId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var insDoc = new InspectionEPDocs
                        {
                            TInsDocs = Convert.ToInt32(row["TInsDocs"].ToString()),
                            ActivityId = Guid.Parse(row["ActivityId"].ToString()),
                            // DocReferenceId = row["DocReferenceId"].ToString()
                        };
                        if (!string.IsNullOrEmpty(row["DocTypeId"].ToString()))
                            insDoc.DocTypeId = Convert.ToInt32(row["DocTypeId"].ToString());
                        if (!string.IsNullOrEmpty(row["BinderId"].ToString()))
                            insDoc.BinderId = Convert.ToInt32(row["BinderId"].ToString());

                        if (!string.IsNullOrEmpty(row["DtEffectiveDate"].ToString()))
                            insDoc.DtEffectiveDate = Convert.ToDateTime(row["DtEffectiveDate"].ToString());
                        insDoc.Text_DtEffectiveDate = Convert.ToDateTime(row["DtEffectiveDate"].ToString()).ToString("MMM dd yyyy");

                        insDoc.CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString());
                        //insDoc.Version = row["Version"].ToString();
                        insDoc.DocumentName = row["DocumentName"].ToString();
                        if (!string.IsNullOrEmpty(row["ActivityType"].ToString()))
                            insDoc.ActivityType = Convert.ToInt32(row["ActivityType"].ToString());
                        insDoc.Path = row["Path"].ToString();

                        //if (!string.IsNullOrEmpty(row["AttachmentId"].ToString()))
                        //    insDoc.AttachmentId = Convert.ToInt32(row["AttachmentId"].ToString());

                        //if (!string.IsNullOrEmpty(row["DocumentId"].ToString()))
                        //    insDoc.DocumentId = Convert.ToInt32(row["DocumentId"].ToString());

                        if (!string.IsNullOrEmpty(row["FileId"].ToString()))
                            insDoc.FileId = Convert.ToInt32(row["FileId"].ToString());

                        if (!string.IsNullOrEmpty(row["IsDeleted"].ToString()))
                            insDoc.IsDeleted = Convert.ToBoolean(row["IsDeleted"].ToString());

                        if (insDoc.DocTypeId.HasValue)
                        {
                            insDoc.DocumentType = new DocumentType();
                            insDoc.DocTypeId = insDoc.DocTypeId.Value;
                            insDoc.DocumentType.Name = row["Name"].ToString();
                            insDoc.DocumentType.Path = row["SampleDocPath"].ToString();
                        }
                        insDoc.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                        insDoc.Text_CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()).ToString("MMM dd yyyy");
                        if (!string.IsNullOrEmpty(row["ExpiredDate"].ToString()))
                        {
                            insDoc.ExpiredDate = Convert.ToDateTime(row["ExpiredDate"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["UploadDocTypeId"].ToString()))
                            insDoc.UploadDocTypeId = Convert.ToInt32(row["UploadDocTypeId"].ToString());

                        insDoc.UserProfile = new UserProfile
                        {
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            UserId = Convert.ToInt32(row["CreatedBy"].ToString()),
                        };
                        document.Add(insDoc);
                    }
                }
            }

            return document;
        }
        #endregion


        #region Location Groups
        public int SaveLocationGroup(LocationGroups newLocationGroups)
        {
            int newId = 0;
            const string sql = StoredProcedures.Insert_LocationGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LocationGroupId", newLocationGroups.LocationGroupId);
                command.Parameters.AddWithValue("@Name", newLocationGroups.Name);
                command.Parameters.AddWithValue("@Type", newLocationGroups.Type);
                command.Parameters.AddWithValue("@Description", newLocationGroups.Description);
                command.Parameters.AddWithValue("@CreatedBy", newLocationGroups.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", newLocationGroups.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public List<LocationGroups> GetLocationGroup()
        {
            var locationGroups = new List<LocationGroups>();
            var locationGroupDetails = new List<LocationGroupDetails>();
            const string sql = StoredProcedures.Get_LocationGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    locationGroups = _sqlHelper.ConvertDataTable<LocationGroups>(ds.Tables[0]);                    
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var locationGroupDetail = new LocationGroupDetails
                        {
                            LocationGroupDetailId = Convert.ToInt32(row["LocationGroupDetailId"].ToString()),
                            LocationGroupId = Convert.ToInt32(row["LocationGroupId"].ToString()),
                            BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            Buildings = new Buildings()
                            {
                                BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
                                BuildingName = row["BuildingName"].ToString(),
                                BuildingCode = row["BuildingCode"].ToString()
                            },
                        };
                        locationGroupDetails.Add(locationGroupDetail);
                    }
                    foreach (var item in locationGroups)
                    {
                        item.LocationGroupDetails = locationGroupDetails.Where(x => x.LocationGroupId == item.LocationGroupId).ToList();
                    }
                }
            }
            return locationGroups;
        }


        public int SaveGroupLocationsDetails(LocationGroupDetails objlocationGroupDetails)
        {
            int newId;
            const string sql = StoredProcedures.Insert_GroupLocationsDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LocationGroupId", objlocationGroupDetails.LocationGroupId);
                command.Parameters.AddWithValue("@BuildingId", objlocationGroupDetails.BuildingId);
                command.Parameters.AddWithValue("@CreatedBy", objlocationGroupDetails.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", objlocationGroupDetails.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public List<Buildings> GetGroupLocationsDetails(int? locationGroupId)
        {
            var buildings = new List<Buildings>();
            const string sql = StoredProcedures.Get_GroupLocationsDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@locationGroupId", locationGroupId.HasValue && locationGroupId.Value > 0 ? locationGroupId.Value : null);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    buildings = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[0]).GroupBy(test => test.BuildingId).Select(grp => grp.First()).ToList();
                }
            }
            return buildings;
        }

        public List<Buildings> GetGroupBuildings()
        {
            var buildings = new List<Buildings>();
            string sql = StoredProcedures.Get_GroupBuildings;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(cmd);
                if (ds != null)
                {
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
                            IsHavingFiredrill = Convert.ToBoolean(row["IsHavingFiredrill"].ToString())
                        };
                        buildings.Add(building);
                    }
                }
            }
            return buildings.Where(x => x.IsActive).ToList();
        }

        #endregion
    }
}
