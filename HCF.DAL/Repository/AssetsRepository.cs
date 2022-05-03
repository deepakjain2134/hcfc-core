using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HCF.BDO;
using HCF.BDO.Enums;
using HCF.Utility;

namespace HCF.DAL
{
    public class AssetsRepository : IAssetsRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ICommonRepository _commonRepository;
        public AssetsRepository(ISqlHelper sqlHelper, ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
            _sqlHelper = sqlHelper;
        }

        #region Assets


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newAssets"></param>
        /// <returns></returns>
        public int UpdateAsset(Assets newAssets)
        {
            int newId;
            string sql = StoredProcedures.Update_Assets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AssetId", newAssets.AssetId);
                command.Parameters.AddWithValue("@AssetTypeId", newAssets.AssetTypeId);
                command.Parameters.AddWithValue("@Name", newAssets.Name);
                command.Parameters.AddWithValue("@IconPath", newAssets.IconPath);
                command.Parameters.AddWithValue("@CreatedBy", newAssets.CreatedBy);
                command.Parameters.AddWithValue("@AssetCode", newAssets.AssetCode);
                command.Parameters.AddWithValue("@IsActive", newAssets.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public Assets GetAssetEPs(int assetId)
        {
            var asset = new Assets();           
            const string sql = StoredProcedures.Get_AssetsEPs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId);
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    asset = _sqlHelper.ConvertDataTable<Assets>(dt.Tables[1]).FirstOrDefault();
                    if (asset != null)
                        asset.StandardEps = _sqlHelper.ConvertDataTable<StandardEps>(dt.Tables[0]);
                }
            }
            return asset;
        }

        public List<Assets> GetAssetEPs(string assetIds)
        {
            var lists = new List<Assets>();
            const string sql = StoredProcedures.Get_Assets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var assetSubCategory = _sqlHelper.ConvertDataTable<AssetsSubCategory>(ds.Tables[1]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var list = new Assets
                        {
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                            IsStepsOnReport = Convert.ToBoolean(row["IsStepsOnReport"].ToString()),
                            Name = row["Name"].ToString(),
                            Count = Convert.ToInt32(row["Count"].ToString()),
                            CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                            AssetTypeId = Convert.ToInt32(row["AssetTypeId"].ToString()),
                            IconPath = row["IconPath"].ToString(),
                            AssetType = new AssetType
                            {
                                Name = row["AssetTypeName"].ToString(),
                                AssetTypeCode = row["AssetTypeCode"].ToString()
                            },
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            AssetCode = row["AssetCode"].ToString(),
                            StepsCount = Convert.ToInt32(row["StepsCount"].ToString())

                        };
                        list.AssetId = Convert.ToInt32(row["AssetId"].ToString());
                        list.EPdetails = new List<EPDetails>();
                        var eps = new List<StandardEps>();
                        var epdetails = new List<EPDetails>();
                        string expression = "AssetId = '" + list.AssetId + "'";
                        string sortOrder = "StandardEp ASC";
                        DataRow[] foundRows = ds.Tables[2].Select(expression, sortOrder);
                        for (int i = 0; i < foundRows.Length; i++)
                        {
                            var ep = new StandardEps
                            {
                                EPNumber = Convert.ToString(foundRows[i]["EPNumber"]),
                                Description = Convert.ToString(foundRows[i]["Description"]),
                                EPDetailId = Convert.ToInt32(foundRows[i]["EPDetailId"]),
                                StDescID = Convert.ToInt32(foundRows[i]["StDescID"]),
                                TJCStandard = Convert.ToString(foundRows[i]["TJCStandard"]),
                                TJCDescription = Convert.ToString(foundRows[i]["TJCDescription"])
                            };
                            var epdetail = new EPDetails
                            {
                                EPNumber = Convert.ToString(foundRows[i]["EPNumber"]),
                                Description = Convert.ToString(foundRows[i]["Description"]),
                                EPDetailId = Convert.ToInt32(foundRows[i]["EPDetailId"]),
                                StDescID = Convert.ToInt32(foundRows[i]["StDescID"]),
                                Standard = new Standard
                                {
                                    TJCStandard = Convert.ToString(foundRows[i]["TJCStandard"]),
                                    TJCDescription = Convert.ToString(foundRows[i]["TJCDescription"])
                                }
                            };
                            ep.StDescID = Convert.ToInt32(foundRows[i]["StDescID"]);
                            ep.StandardEP = Convert.ToString(foundRows[i]["StandardEP"]);
                            eps.Add(ep);
                            epdetails.Add(epdetail);
                        }
                        list.EPdetails = epdetails;
                        list.StandardEps = eps;
                        lists.Add(list);
                    }
                }
            }
            if (!string.IsNullOrEmpty(assetIds))
            {
                int[] assetArray = assetIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                return lists.Where(x => assetArray.Contains(x.AssetId)).ToList();
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Assets> GetAssetMaster(int userId)
        {
            var lists = new List<Assets>();
            const string sql = StoredProcedures.Get_Assets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var assetSubCategory = _sqlHelper.ConvertDataTable<AssetsSubCategory>(ds.Tables[1]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var list = new Assets
                        {
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                            IsStepsOnReport = Convert.ToBoolean(row["IsStepsOnReport"].ToString()),
                            Name = row["Name"].ToString(),
                            Count = Convert.ToInt32(row["Count"].ToString()),
                            CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                            AssetTypeId = Convert.ToInt32(row["AssetTypeId"].ToString()),
                            IconPath = row["IconPath"].ToString(),
                            AssetType = new AssetType
                            {
                                Name = row["AssetTypeName"].ToString(),
                                AssetTypeCode = row["AssetTypeCode"].ToString()
                            },
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            AssetCode = row["AssetCode"].ToString(),
                            StepsCount = Convert.ToInt32(row["StepsCount"].ToString())

                        };
                        list.AssetSubCategory = assetSubCategory.Where(x => x.AssetId == list.AssetId).ToList();
                        list.AssetId = Convert.ToInt32(row["AssetId"].ToString());
                        list.EPdetails = new List<EPDetails>();
                        //list.EPdetails = EpRepository.GetEpByAssets(list.AssetId).OrderByDescending(x => x.CreatedDate).ToList();
                        var eps = new List<StandardEps>();
                        var epdetails = new List<EPDetails>();
                        string expression = "AssetId = '" + list.AssetId + "'";
                        string sortOrder = "StandardEp ASC";
                        DataRow[] foundRows = ds.Tables[2].Select(expression, sortOrder);
                        for (int i = 0; i < foundRows.Length; i++)
                        {
                            var ep = new StandardEps
                            {
                                EPNumber = Convert.ToString(foundRows[i]["EPNumber"]),
                                Description = Convert.ToString(foundRows[i]["Description"]),
                                EPDetailId = Convert.ToInt32(foundRows[i]["EPDetailId"]),
                                StDescID = Convert.ToInt32(foundRows[i]["StDescID"]),
                                TJCStandard = Convert.ToString(foundRows[i]["TJCStandard"]),
                                TJCDescription = Convert.ToString(foundRows[i]["TJCDescription"])
                            };
                            var epdetail = new EPDetails
                            {
                                EPNumber = Convert.ToString(foundRows[i]["EPNumber"]),
                                Description = Convert.ToString(foundRows[i]["Description"]),
                                EPDetailId = Convert.ToInt32(foundRows[i]["EPDetailId"]),
                                StDescID = Convert.ToInt32(foundRows[i]["StDescID"]),
                                Standard = new Standard
                                {
                                    TJCStandard = Convert.ToString(foundRows[i]["TJCStandard"]),
                                    TJCDescription = Convert.ToString(foundRows[i]["TJCDescription"])
                                }
                            };
                            epdetail.EPFrequency = new List<EPFrequency>();
                            string freqexpr = "EpDetailId = '" + epdetail.EPDetailId + "'";
                            DataRow[] freqRows = ds.Tables[3].Select(freqexpr);
                            foreach (DataRow t in freqRows)
                            {
                                var epfreq = new EPFrequency
                                {
                                    Frequency = new FrequencyMaster
                                    {
                                        FrequencyId = Convert.ToInt32(t["FrequencyId"].ToString()),
                                        DisplayName = t["DisplayName"].ToString(),
                                        Days = Convert.ToInt32(t["Days"].ToString())
                                    },
                                    FrequencyId = Convert.ToInt32(t["FrequencyId"].ToString()),
                                };
                                epdetail.EPFrequency.Add(epfreq);
                            }
                            ep.StDescID = Convert.ToInt32(foundRows[i]["StDescID"]);
                            ep.StandardEP = Convert.ToString(foundRows[i]["StandardEP"]);
                            eps.Add(ep);
                            epdetails.Add(epdetail);
                        }
                        list.EPdetails = epdetails;
                        list.StandardEps = eps;
                        lists.Add(list);
                    }
                }
            }
            return lists;
        }

        public List<Assets> Get()
        {
            return _commonRepository.GetTableData<Assets>(StoredProcedures.Get_Assets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Assets> GetAssets(int userId)
        {
            return GetAssetMaster(userId).Where(x => x.IsActive).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Assets> GetAssets()
        {
            var lists = new List<Assets>();
            const string sql = StoredProcedures.Get_MasterAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                foreach (DataRow row in dt.Rows)
                {
                    var list = new Assets
                    {
                        AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                        CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                        Name = row["Name"].ToString(),
                        IconPath = row["IconPath"].ToString(),
                        IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                        // UserProfile = new UserProfile { UserName = row["UserName"].ToString() },
                        CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                        AssetTypeId = Convert.ToInt32(row["AssetTypeId"].ToString()),
                        IsStepsOnReport = Convert.ToBoolean(row["IsStepsOnReport"].ToString())
                    };
                    list.AssetType = new AssetType
                    {
                        Name = row["AssetTypeName"].ToString(),
                        TypeId = list.AssetTypeId,
                        AssetTypeCode = row["AssetTypeCode"].ToString()

                    };
                    list.AssetCode = row["AssetCode"].ToString();
                    list.Count = Convert.ToInt32(row["Count"].ToString());
                    lists.Add(list);
                }
            }
            return lists;
        }

        public List<Assets> ConvertToAssets(DataTable dt)
        {
            var assets = _sqlHelper.ConvertDataTable<Assets>(dt).GroupBy(x => x.AssetId).Select(x => x.FirstOrDefault()).ToList();
            return assets;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public Assets GetAsset(int assetId)
        {
            return GetAssets().SingleOrDefault(x => x.AssetId == assetId);
        }

        public List<Assets> GetAllAsset()
        {
            return GetAssets().ToList();
        }

        public List<Assets> GetEpAssets(int epDetailId)
        {
            //var lists = new List<Assets>();
            //const string sql = StoredProcedures.Get_EPAssets; //"dbo.Get_EPAssets";
            //using (var command = new SqlCommand(sql))
            //{
            //    command.CommandType = CommandType.StoredProcedure;
            //    command.Parameters.AddWithValue("@epDetailId", epDetailId);
            //    var dt = _sqlHelper.GetDataTable(command);
            //    if (dt != null)
            //        lists = _sqlHelper.ConvertDataTable<Assets>(dt);
            //}
            //return lists;

            var assets = new List<Assets>();
            const string sql = StoredProcedures.Get_EPAssetTypes;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", epDetailId);
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Assets> GetAssetsMapping(string mode, string id)
        {
            var lists = new List<Assets>();
            const string sql = StoredProcedures.Get_AssetsMapping;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                var assetsVendorMapping = _sqlHelper.ConvertDataTable<AssetsMapping>(dt);
                if (mode.ToUpper() == (BDO.Enums.Mode.ASSET).ToString())
                {
                    lists = GetAssets().Where(m => m.AssetId == Convert.ToInt32(id)).ToList();
                }
                else if (mode.ToUpper() == (Mode.VENDOR).ToString())
                {
                    foreach (var item in assetsVendorMapping.Where(m => m.VendorId == Convert.ToInt32(id) && m.IsActive))
                    {
                        if (item.AssetId != null)
                        {
                            var list = GetAsset(item.AssetId.Value);
                            if (list != null)
                            {
                                lists.Add(list);
                            }
                        }
                    }
                }
                foreach (var assets in lists.Where(x => x.AssetId > 0))
                {
                    var vendors = assetsVendorMapping.Where(x => x.VendorId != null)
                                 .Where(x => x.AssetId == assets.AssetId)
                                 .Select(x =>
                                 {
                                     if (x.VendorId != null) return new Vendors() { VendorId = x.VendorId.Value, IsActive = x.IsActive };
                                     return null;
                                 })
                                 .ToList();
                    assets.Vendors = vendors;
                }
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Assets> GetAssetsUserMapping(string mode, string id)
        {
            List<Assets> lists = new List<Assets>();
            string sql = StoredProcedures.Get_AssetsMapping;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                var assetsVendorMapping = _sqlHelper.ConvertDataTable<AssetsMapping>(dt);
                if (mode == (Mode.ASSET).ToString())
                {
                    lists = GetAssets().Where(m => m.AssetId == Convert.ToInt32(id)).ToList();
                }
                else if (mode == (Mode.USER).ToString())
                {
                    foreach (AssetsMapping item in assetsVendorMapping.Where(m => m.UserId != null))
                    {
                        if (item.AssetId != null)
                        {
                            var list = GetAsset(item.AssetId.Value);
                            lists.Add(list);
                        }
                    }
                }
                foreach (var assets in lists)
                {
                    var users = assetsVendorMapping
                                 .Where(x => x.AssetId == assets.AssetId && x.UserId != null)
                                 .Select(x => new UserProfile() { UserId = x.UserId.Value })
                                 .ToList();
                    assets.Users = users;
                }
            }
            return lists;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newAssetsUserMapping"></param>
        /// <returns></returns>
        public int AddAssetsMapping(AssetsMapping newAssetsUserMapping)
        {
            int newId;
            const string sql = StoredProcedures.Insert_AssetsMapping;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", newAssetsUserMapping.UserId);
                command.Parameters.AddWithValue("@VendorId", newAssetsUserMapping.VendorId);
                command.Parameters.AddWithValue("@AssetId", newAssetsUserMapping.AssetId);
                command.Parameters.AddWithValue("@CreatedBy", newAssetsUserMapping.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", newAssetsUserMapping.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        //public  int CalculateDefeciencyScore(int assetId, int score)
        //{
        //    int defeciencyCount;
        //    const string sql = StoredProcedures.Get_DefeciencyCount;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@AssetId", assetId);
        //        defeciencyCount = Convert.ToInt32(_sqlHelper.GetScalarValue(command));
        //    }
        //    return (defeciencyCount + score);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newData"></param>
        /// <returns></returns>
        public int SaveFloorAssetsInspection(FloorAssetsInspection newData)
        {
            int newId;
            const string sql = StoredProcedures.Insert_FloorAssetsInspection;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorAssetId", newData.FloorAssetId);
                command.Parameters.AddWithValue("@EPDetailId", newData.EPDetailId);
                command.Parameters.AddWithValue("@InspectionDate", newData.InspectionDate);
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
        //public List<Assets> GetAssetsByFloor(int floorId)
        //{
        //    var asstsList = new List<Assets>();
        //    const string sql = StoredProcedures.Get_AssetsByFloor;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@floorId", floorId);
        //        var dt = _sqlHelper.GetDataTable(command);
        //        if (dt != null)
        //            asstsList = _sqlHelper.ConvertDataTable<Assets>(dt);
        //    }
        //    return asstsList;
        //}

        public int AttachAssetToEp(int assetId, int epId, int userId)
        {
            int newId;
            const string sql = StoredProcedures.Insert_EpAsset;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId);
                command.Parameters.AddWithValue("@epId", epId);
                command.Parameters.AddWithValue("@CreatedBy", userId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public List<Assets> GetAssetFrequency()
        {
            var asstsList = new List<Assets>();
            const string sql = StoredProcedures.Get_AssetFrequency;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    asstsList = _sqlHelper.ConvertDataTable<Assets>(ds.Tables[0]);
                    foreach (var item in asstsList)
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
                }
            }
            return asstsList;
        }





        #endregion

        #region Floor Assets CSV Method
        /// <summary>
        /// Get TFloorAssetsData Match from CSV file.
        /// </summary>
        /// <returns></returns>
        //public DataTable GetAssestsDataCSVMatch()
        //{
        //    const string sql = StoredProcedures.Get_AssestsCSVMatch;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        var ds = _sqlHelper.GetDataSet(command);
        //        return ds.Tables[0];
        //    }
        //}
        /// <summary>
        /// Save New FloorAssets Record which is filter from CSV file.
        /// </summary>
        /// <param name="xmlRecord"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public DataSet SaveCSVFilterNewAssets(string xmlRecord, int userId)
        //{
        //    DataSet ds = null;
        //    string sql = StoredProcedures.Insert_NewAssestRecordFromCSV;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@NewRecordXml", xmlRecord);
        //        command.Parameters.AddWithValue("@LoginId", userId);
        //        ds = _sqlHelper.ExecuteNonQueryReturnDS(command);
        //    }
        //    return ds;
        //}
        /// <summary>
        /// Update Exist TfloorAssets Record from CSV file.
        /// </summary>
        /// <param name="xmlRecord"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public bool UpdateExistAssetsFromCSV(string xmlRecord, int userId)
        //{
        //    bool result = false;
        //    string sql = StoredProcedures.Update_ExistedAssestRecordFromCSV;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@RecordXml", xmlRecord);
        //        command.Parameters.AddWithValue("@LoginId", userId);
        //        result = _sqlHelper.ExecuteNonQuery(command);
        //    }
        //    return result;
        //}
        #endregion

        #region Extension Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        //public  List<EPDetails> EPdetails(int assetId)
        //{
        //    return EpRepository.GetEpByAssets(assetId);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        //public  List<TFloorAssets> TFloorAssets(int assetId)
        //{
        //    return FloorAssetRepository.GetTFloorAssets().Where(x => x.AssetId == assetId).ToList();
        //}

        #endregion



        public DataTable GetAssetsDetailsByTmsAssetId(string tmsAssetsIds)
        {
            var dt = new DataTable();
            const string sql = StoredProcedures.Get_AssetsDetailsbyTMSAssetId;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tmsAssetsIds", tmsAssetsIds);
                dt = _sqlHelper.GetDataTable(command);
            }
            return dt;
        }

        public string GetAssetsCurrentStatus(int Epdetailid, string FAssestId)
        {
            string status = ""; //GetAssetsDetailsByTmsAssetId
             var ds = new DataSet();
            const string sql = StoredProcedures.Get_AllAssetsCurrentStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Epdetailid", Epdetailid);
                command.Parameters.AddWithValue("@FAssestId", FAssestId);
                ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    string result1 = ds.Tables[0].Rows[0][0].ToString();
                    status = result1;
                }
            }
            return status;
        }

        #region Get Tracking Asset Creation
        public List<object> GetTrackingAssetCreation(string assetIds, string campusid, string buildingId)
        {
            List<object> datalist = new List<object>();
            const string sql = StoredProcedures.Get_Tracking_Asset_Creation;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetIds", assetIds);
                command.Parameters.AddWithValue("@campusid", campusid);
                command.Parameters.AddWithValue("@buildingId", buildingId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    datalist.Add(_sqlHelper.ConvertDataTable<Site>(ds.Tables[0]));
                    datalist.Add(_sqlHelper.ConvertDataTable<Buildings>(ds.Tables[1]));
                    datalist.Add(_sqlHelper.ConvertDataTable<Floor>(ds.Tables[2]));
                    datalist.Add(_sqlHelper.ConvertDataTable<Assets>(ds.Tables[3]));
                    datalist.Add(_sqlHelper.ConvertDataTable<TFloorAssets>(ds.Tables[4]));

                }
            }
            return datalist;
        }
        public List<TFloorAssets> GetAllTypesfloorassets()
        {
            List<TFloorAssets> datalst = new List<TFloorAssets>();
            TFloorAssets data = new TFloorAssets();
            const string sql = StoredProcedures.Get_Alltypes_Floorassets;
            using (var command = new SqlCommand(sql))
            {
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    datalst = _sqlHelper.ConvertDataTable<TFloorAssets>(ds.Tables[0]);
                };
            }
            return datalst;
        }   


        #endregion
    }
}