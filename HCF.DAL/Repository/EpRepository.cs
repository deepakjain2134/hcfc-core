using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace HCF.DAL
{
    public class EpRepository : IEpRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IAssetsRepository _assetsRepository;
        private readonly IFrequencyRepository _frequencyRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IDocumentTypeRepository _documentTypeRepository;

        public EpRepository(IDocumentTypeRepository documentTypeRepository, IUsersRepository usersRepository, ISqlHelper sqlHelper, IAssetsRepository assetsRepository, IFrequencyRepository frequencyRepository)
        {
            _documentTypeRepository = documentTypeRepository;
            _usersRepository = usersRepository;
            _frequencyRepository = frequencyRepository;
            _sqlHelper = sqlHelper;
            _assetsRepository = assetsRepository;
        }

        #region  EP Repository

        private HttpResponseMessage _objHttpResponseMessage = new HttpResponseMessage();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EPDetails> GetNonAssetEPsSchedule()
        {
            var epdetails = new List<EPDetails>();
            const string sql = StoredProcedures.Get_NonAssetEPsSchedule;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                var schedules = _sqlHelper.ConvertDataTable<Schedules>(ds.Tables[1]);
                var frequencyMaster = _sqlHelper.ConvertDataTable<FrequencyMaster>(ds.Tables[2]);
                var epFrequencyList = _sqlHelper.ConvertDataTable<EPFrequency>(ds.Tables[3]);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var list = new EPDetails
                    {
                        EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                        StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                        Description = row["Description"].ToString(),
                        EPNumber = row["EPNumber"].ToString(),
                        IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString()),
                        IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString()),
                        IsIlsmEP = Convert.ToBoolean(row["IsIlsmEP"].ToString()),
                        Priority = Convert.ToInt32(row["Priority"].ToString()),
                        ScoreId = Convert.ToInt32(row["ScoreId"].ToString()),
                        IsActive = Convert.ToBoolean(row["EPActive"].ToString()),
                        IsApplicable = Convert.ToBoolean(row["IsApplicable"]),
                        IsFrequencyChange = Convert.ToBoolean(row["IsFrequencyChange"].ToString()),
                        Standard = new Standard
                        {
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                            TJCStandard = row["TJCStandard"].ToString()
                        },
                        Scores = new Score
                        {
                            Id = Convert.ToInt32(row["ScoreId"].ToString()),
                            Name = row["ScoreName"].ToString()
                        }
                    };
                    if (!string.IsNullOrEmpty(row["ScheduleIds"].ToString()))
                    {
                        var names = row["ScheduleIds"].ToString().Split(',');
                        var schedulesIds = Array.ConvertAll(names, int.Parse);
                        var matcheSchedule = from schedule in schedules
                                             where schedulesIds.Contains(schedule.ScheduleId)
                                             select schedule;

                        list.Schedules = matcheSchedule.ToList();
                    }
                    var epFrequency = new List<EPFrequency>();
                    foreach (var frequency in epFrequencyList.Where(x => x.EpDetailId == list.EPDetailId && x.IsActive))
                    {
                        frequency.TjcFrequency = frequencyMaster.SingleOrDefault(x => x.FrequencyId == frequency.TjcFrequencyId);
                        frequency.Frequency = frequencyMaster.SingleOrDefault(x => x.FrequencyId == frequency.FrequencyId);
                        epFrequency.Add(frequency);
                    }
                    if (epFrequency.Count > 0)
                        list.EPFrequency = epFrequency;
                    epdetails.Add(list);
                }
                return epdetails;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="objRequest"></param>
        /// <param name="userId"></param>
        /// <param name="categoryId"></param>
        /// <param name="status"></param>
        /// <param name="noofdays"></param>
        /// <param name="duemonth"></param>
        /// <param name="dueyear"></param>
        /// <returns></returns>
        public HttpResponseMessage GetDashBoardEp(Request objRequest, int userId, int? categoryId = null, int? status = null, int? noofdays = null, int? duemonth = null, int? dueyear = null)
        {
            DataSet ds;
            var epDetails = new List<EPDetails>();
            var sql = StoredProcedures.Get_EPDetails_Paging_Proc;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@FilterByUserUserId", objRequest.FilterByUserUserId);
                command.Parameters.AddWithValue("@mode", objRequest.Mode);
                command.Parameters.AddWithValue("@pageIndex", objRequest.PageIndex);
                command.Parameters.AddWithValue("@pageSize", objRequest.PageSize);
                command.Parameters.AddWithValue("@sortOrders", objRequest.SortOrder);
                command.Parameters.AddWithValue("@orderbySort", objRequest.OrderbySort);
                command.Parameters.AddWithValue("@categoryId", categoryId > 0 ? categoryId : null);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@noofdays", noofdays != -1 ? noofdays : null);
                command.Parameters.AddWithValue("@duemonth", duemonth != 0 ? duemonth : null);
                command.Parameters.AddWithValue("@dueyear", dueyear != 0 ? dueyear : null);
                command.Parameters.AddWithValue("@searchBy", objRequest.SearchcBy);
                ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var userRecordsCount = GetEpSummary(ds.Tables[0]);

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var ep = new EPDetails
                        {
                            EPNumber = row["EPNumber"].ToString(),
                            Description = row["Description"].ToString(),
                            EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            Priority = Convert.ToInt32(row["Priority"].ToString()),
                            Standard = new Standard
                            {
                                CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                TJCStandard = row["TJCStandard"].ToString(),
                                Category = new Category
                                {
                                    CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                    Code = row["Code"].ToString()
                                }
                            },
                            Scores = new Score
                            {
                                Name = row["ScoreName"].ToString()
                            },
                            IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString()),
                            DocStatus = Convert.ToInt32(row["DocStatus"].ToString()),
                            Inspection = new Inspection
                            {
                                InspectionId = Convert.ToInt32(row["InspectionId"].ToString()),
                                InspectionGroupId = Convert.ToInt32(row["InspectionGroupId"].ToString()),
                                CreatedDate = string.IsNullOrEmpty(row["StartDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["StartDate"].ToString()),
                                StartDate = string.IsNullOrEmpty(row["StartDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["StartDate"].ToString()),
                                DueDate = string.IsNullOrEmpty(row["DueDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["DueDate"].ToString()),
                                GraceDate = string.IsNullOrEmpty(row["GraceDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["GraceDate"].ToString()),
                                Status = Convert.ToInt32(row["Status"].ToString()),
                                SubStatus = row["SubStatus"].ToString(),

                                // FixDueDate=epDetails.in
                            },
                        };

                        if (!string.IsNullOrEmpty(row["EPStatus"].ToString()))
                            ep.EPStatus = Convert.ToInt16(row["EPStatus"].ToString());

                        if (!string.IsNullOrEmpty(row["InitialInspDate"].ToString()))
                            ep.InitialInspDate = Convert.ToDateTime(row["InitialInspDate"].ToString());

                        if (ep.Inspection.InspectionId > 0)
                            ep.EpTransStatus = EpTransStatus(ep.EPDetailId, ep.Inspection);
                        else
                            ep.EpTransStatus = "P";

                        ep.EPFrequency = new List<EPFrequency>();

                        // set ep frequency
                        if (!string.IsNullOrEmpty(row["FrequencyId"].ToString()))
                        {
                            var epfreq = new EPFrequency
                            {
                                Frequency = new FrequencyMaster
                                {
                                    FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                                    DisplayName = row["FrequencyDisplayName"].ToString(),
                                },
                                FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),

                            };
                            ep.EPFrequency.Add(epfreq);
                        }
                        if (!string.IsNullOrEmpty(row["IsAssetsRequired"].ToString()))
                        {
                            ep.Assets = new List<Assets>();
                            var assetExpression = "EPDetailId = '" + ep.EPDetailId + "'";
                            var AssetSortOrder = "Name ASC";
                            var drfoundRows = ds.Tables[2].Select(assetExpression, AssetSortOrder);
                            for (int i = 0; i < drfoundRows.Length; i++)
                            {
                                var asset = new Assets
                                {
                                    Name = Convert.ToString(drfoundRows[i]["Name"]),
                                    AssetId = Convert.ToInt32(drfoundRows[i]["AssetId"]),
                                    IsRouteInsp = Conversion.TryCastBoolean(drfoundRows[i]["IsRouteInsp"])
                                };
                                ep.Assets.Add(asset);
                            }
                        }

                        ep.EPUsers = new List<UserProfile>();
                        string expression = "EPDetailId = '" + ep.EPDetailId + "'";
                        string sortOrder = "FirstName ASC";
                        var foundRows = ds.Tables[3].Select(expression, sortOrder);
                        for (int i = 0; i < foundRows.Length; i++)
                        {
                            var user = new UserProfile
                            {
                                Email = Convert.ToString(foundRows[i]["Email"]),
                                FirstName = Convert.ToString(foundRows[i]["FirstName"]),
                                LastName = Convert.ToString(foundRows[i]["LastName"]),
                                UserId = Convert.ToInt32(foundRows[i]["UserId"])
                            };
                            ep.EPUsers.Add(user);
                        }
                        // set epBinders
                        ep.Binders = new List<Binders>();
                        if (!string.IsNullOrEmpty(row["BinderId"].ToString()))
                        {
                            var binder = new Binders
                            {
                                BinderId = Convert.ToInt32(row["BinderId"]),
                                BinderName = row["BinderName"].ToString()
                            };
                            ep.Binders.Add(binder);
                        }
                        ep.IsCustomFrequency = Convert.ToBoolean(row["IsCustomFrequency"]);
                        if (!string.IsNullOrEmpty(row["EffectiveDate"].ToString()))
                            ep.EffectiveDate = Convert.ToDateTime(row["EffectiveDate"].ToString());
                        if (!string.IsNullOrEmpty(row["EffectiveDate"].ToString()))
                        {
                            DateTime expiryDate = DateTime.Parse(row["EffectiveDate"].ToString()).AddDays(30);
                            if (DateTime.Now < expiryDate)
                            {
                                ep.IsNewEp = true;
                            }
                        }

                        if (!string.IsNullOrEmpty(row["LastUpdatedDate"].ToString()))
                        {
                            ep.LastUpdatedDate = Convert.ToDateTime(row["LastUpdatedDate"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["EPDetailId"].ToString()))
                        {
                            ep.CopDetails = new List<CopDetails>();
                            var copExpression = "EPDetailId = '" + ep.EPDetailId + "'";

                            var drfoundRows = ds.Tables[4].Select(copExpression);
                            for (int i = 0; i < drfoundRows.Length; i++)
                            {
                                var copdetail = new CopDetails
                                {
                                    CopText = Convert.ToString(drfoundRows[i]["CopText"]),
                                    RequirementName = Convert.ToString(drfoundRows[i]["RequirementName"]),
                                    CopStdesc = new CopStdesc()
                                    {
                                        CopTitle = Convert.ToString(drfoundRows[i]["CopTitle"])
                                    }
                                };
                                ep.CopDetails.Add(copdetail);
                            }
                        }
                        epDetails.Add(ep);
                    }
                    _objHttpResponseMessage = new HttpResponseMessage
                    {
                        Result = new Result
                        {
                            EPDetails = epDetails,
                            UserRecordsCount = userRecordsCount
                        },
                        PageCount = Convert.ToInt32(ds.Tables[0].Rows.Count)
                    };
                }
            }
            return _objHttpResponseMessage;
        }

        public List<EPDetails> GetEpDetailsAsync()
        {
            var lists = new List<EPDetails>();
            const string sql = StoredProcedures.Get_Eps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var list = new EPDetails
                        {
                            EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            Description = row["Description"].ToString(),
                            EPNumber = row["EPNumber"].ToString(),
                            IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString()),
                            IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString()),
                            IsIlsmEP = Convert.ToBoolean(row["IsIlsmEP"].ToString()),
                            Priority = Convert.ToInt32(row["Priority"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            IsApplicable = Convert.ToBoolean(row["IsApplicable"]),
                            IsFrequencyChange = Convert.ToBoolean(row["IsFrequencyChange"].ToString()),
                            IsExpired = Convert.ToBoolean(row["IsExpired"]),
                            EPSearchText = row["EPSearchText"].ToString()

                        };
                        if (!string.IsNullOrEmpty(row["ScoreId"].ToString()))
                            list.ScoreId = Convert.ToInt32(row["ScoreId"].ToString());
                        if (!string.IsNullOrEmpty(row["IsCurrent"].ToString()))
                            list.IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString());

                        if (!string.IsNullOrEmpty(row["InspectionDate"].ToString()))
                        {
                            list.InspectionDate = Convert.ToDateTime(row["InspectionDate"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["ExpiryDate"].ToString()))
                            list.ExpiryDate = Convert.ToDateTime(row["ExpiryDate"].ToString());

                        if (!string.IsNullOrEmpty(row["EffectiveDate"].ToString()))
                            list.EffectiveDate = Convert.ToDateTime(row["EffectiveDate"].ToString());

                        list.Standard = new Standard
                        {
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                            TJCStandard = row["TJCStandard"].ToString(),
                            DisplayDescription = row["TJCDescription"].ToString()
                        };

                        if (!string.IsNullOrEmpty(row["ScoreId"].ToString()))
                        {
                            list.Scores = new Score
                            {
                                Id = Convert.ToInt32(row["ScoreId"].ToString()),
                                Name = row["ScoreName"].ToString()
                            };
                        }
                        else
                        {
                            list.Scores = new Score();
                        }
                        if (list.IsDocRequired)
                            list.DocumentType = DocumentTypes(list.EPDetailId);
                        lists.Add(list);
                    }
                }
            }
            return lists;
        }

        public DataTable GetMasterEPsData()
        {
            DataTable dt = new();
            var sql = StoredProcedures.Get_MasterEPsData;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                dt = _sqlHelper.GetDataTable(command);
            }
            return dt;
        }

        private UserRecordsCount GetEpSummary(DataTable dataTable)
        {
            var userRecordsCount = new UserRecordsCount();

            if (dataTable.Rows.Count > 0)
            {
                userRecordsCount.TotalEcEps = dataTable.AsEnumerable().Count(myRow => myRow.Field<int>("CategoryId") == 1);
                userRecordsCount.TotalLsEps = dataTable.AsEnumerable().Count(myRow => myRow.Field<int>("CategoryId") == 2);
                userRecordsCount.TotalEmEps = dataTable.AsEnumerable().Count(myRow => myRow.Field<int>("CategoryId") == 3);
                userRecordsCount.TotalFailedEps = dataTable.AsEnumerable().Count(myRow => myRow.Field<int>("Status") == 0);
                userRecordsCount.TotalDueEps = dataTable.AsEnumerable().Count(myRow => myRow.Field<int>("DueWithInDays") < 0);
                userRecordsCount.TotalInProgressEps = dataTable.AsEnumerable().Count(myRow => myRow.Field<int>("Status") == 2);
                userRecordsCount.TotalCompletedEps = dataTable.AsEnumerable().Count(myRow => myRow.Field<int>("Status") == 1 || myRow.Field<int>("Status") == 3);


            }
            return userRecordsCount;
        }

        public int GetTotalEpAssigned(string userIds, int catId)
        {
            int newId;
            const string sql = StoredProcedures.Get_Assigned_Ep_Count;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userIds", userIds);
                command.Parameters.AddWithValue("@catId", catId);
                command.Parameters.Add("@outCount", SqlDbType.Int);
                command.Parameters["@outCount"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@outCount");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetNonAssetsEps()
        {
            DataTable dt;
            var sql = StoredProcedures.Get_NonAssetsEps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                dt = _sqlHelper.GetDataTable(command);
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EPDetails> GetNonAssetsEp()
        {
            DataTable dt;
            var lists = new List<EPDetails>();
            var sql = StoredProcedures.Get_NonAssetsEps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                dt = _sqlHelper.GetDataTable(command);
                lists.AddRange(from DataRow row in dt.Rows
                               select new EPDetails
                               {
                                   EPNumber = row["EPNumber"].ToString(),
                                   Description = row["Description"].ToString(),
                                   EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                                   StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                   Standard = new Standard { CategoryId = Convert.ToInt32(row["CategoryId"].ToString()), StDescID = Convert.ToInt32(row["StDescID"].ToString()), TJCStandard = row["TJCStandard"].ToString() }
                               });
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newEpDetails"></param>
        /// <returns></returns>
        public int CreateEp(EPDetails newEpDetails)
        {
            int newId;
            const string sql = StoredProcedures.Insert_EPDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StDescID", newEpDetails.StDescID);
                command.Parameters.AddWithValue("@EPNumber", newEpDetails.EPNumber);
                command.Parameters.AddWithValue("@Description", newEpDetails.Description);
                command.Parameters.AddWithValue("@Version", newEpDetails.Version != null ? newEpDetails.Version : "");
                command.Parameters.AddWithValue("@ScoreId", newEpDetails.ScoreId);
                command.Parameters.AddWithValue("@EPDetailId", newEpDetails.EPDetailId);
                command.Parameters.AddWithValue("@CreatedBy", newEpDetails.CreatedBy);
                command.Parameters.AddWithValue("@IsDocRequired", newEpDetails.IsDocRequired);
                command.Parameters.AddWithValue("@IsFrequencyChange", newEpDetails.IsFrequencyChange);
                command.Parameters.AddWithValue("@IsApplicable", newEpDetails.IsApplicable);
                command.Parameters.AddWithValue("@EffectiveDate", newEpDetails.EffectiveDate);
                command.Parameters.AddWithValue("@ExpiryDate", newEpDetails.ExpiryDate);
                command.Parameters.AddWithValue("@Priority", newEpDetails.Priority);
                command.Parameters.AddWithValue("@TjcFrequencyId", (newEpDetails.EPFrequency.Count > 0) ? newEpDetails.EPFrequency.FirstOrDefault().TjcFrequencyId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@RecFrequencyId", (newEpDetails.EPFrequency.Count > 0) ? newEpDetails.EPFrequency.FirstOrDefault().RecFrequencyId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsAssetsRequired", newEpDetails.IsAssetsRequired);
                command.Parameters.AddWithValue("@IsActive", newEpDetails.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int SaveEpDescription(EPDescriptions objEpDescriptions)
        {
            int newId;
            const string sql = StoredProcedures.Insert_EPDescriptions;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDescriptionId", objEpDescriptions.EPDescriptionId);
                command.Parameters.AddWithValue("@EPDetailId", objEpDescriptions.EPDetailId);
                command.Parameters.AddWithValue("@HospitalTypeId", objEpDescriptions.HospitalTypeId);
                command.Parameters.AddWithValue("@Description", objEpDescriptions.Description);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetail"></param>
        /// <returns></returns>
        public bool UpdateEPApplicablestatus(EPDetails epDetail)
        {
            const string sql = StoredProcedures.Update_EPApplicablestatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", epDetail.EPDetailId);
                command.Parameters.AddWithValue("@IsApplicable", epDetail.IsApplicable);
                command.Parameters.AddWithValue("@ReasonforNonTracked", epDetail.ReasonforNonTracked);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public EPDetails GetEpShortDescription(int epDetailId)
        {
            return GetEps(epDetailId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stdescId"></param>
        /// <returns></returns>
        public List<EPDetails> GetEpDetails(int stdescId)
        {
            var lists = GetStandardEp(stdescId);
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stDescId"></param>
        /// <param name="epBinderId"></param>
        /// <returns></returns>
        public List<EPDetails> GetEpDetailsforBinders(int stDescId, int epBinderId = 0)
        {
            List<EPDetails> lists;
            const string sql = StoredProcedures.Get_Eps_for_Binders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@stDescId", stDescId);
                command.Parameters.AddWithValue("@epBinderId", epBinderId);
                var ds = _sqlHelper.GetDataSet(command);
                lists = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[0]);
            }
            return lists;
        }

        public List<EPDetails> GetEPNumber(int stDescId)
        {
            List<EPDetails> lists = new();
            const string sql = StoredProcedures.Get_Eps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StDescID", stDescId);
                var ds = _sqlHelper.GetDataSet(command);
                lists = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[0]);
            }
            return lists;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public EPDetails GetEpDescription(int? epDetailId)
        {
            var list = new EPDetails();
            const string sql = StoredProcedures.Get_Eps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetailId", epDetailId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        list.IsExpired = Convert.ToBoolean(row["IsExpired"].ToString());
                        list.EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString());
                        list.StDescID = Convert.ToInt32(row["StDescID"].ToString());
                        list.Description = row["Description"].ToString();
                        if (!string.IsNullOrEmpty(row["ScoreId"].ToString()))
                            list.ScoreId = Convert.ToInt32(row["ScoreId"].ToString());
                        list.EPNumber = row["EPNumber"].ToString();
                        list.CmsEPNumber = row["CmsEPNumber"].ToString();
                        list.CmsDescription = row["CmsDescription"].ToString();
                        list.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
                        list.IsApplicable = Convert.ToBoolean(row["IsApplicable"]);
                        if (!string.IsNullOrEmpty(row["ScoreId"].ToString()))
                        {
                            list.Scores = new Score
                            {
                                Id = Convert.ToInt32(row["ScoreId"].ToString())
                            };
                        }
                        else
                        {
                            list.Scores = new Score();
                        }
                        list.StDescID = Convert.ToInt32(row["StDescID"].ToString());
                        list.Scores.Name = row["ScoreName"].ToString();
                        list.IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString());
                        list.IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString());
                        list.DocStatus = DocStaus(list.EPDetailId, list.IsDocRequired);
                        if (!string.IsNullOrEmpty(row["InitialInspDate"].ToString()))
                            list.InitialInspDate = Convert.ToDateTime(row["InitialInspDate"].ToString());

                        list.Standard = new Standard
                        {
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                            DisplayDescription = row["TJCDescription"].ToString(),
                            TJCStandard = row["TJCStandard"].ToString(),
                            CmsStandard = row["CmsStandard"].ToString(),
                            CmsStdDescription = row["CmsStdDescription"].ToString()
                        };
                        if (list.IsAssetsRequired)
                            list.Assets = _assetsRepository.GetEpAssets(list.EPDetailId);
                        list.EPUsers = GetEpUsers(list.EPDetailId);
                        list.EPFrequency = new List<EPFrequency>();
                        var freqExpr = "EpDetailId = '" + list.EPDetailId + "'";
                        var freqRows = ds.Tables[1].Select(freqExpr);
                        foreach (var t in freqRows)
                        {
                            var epfreq = new EPFrequency
                            {
                                EpDetailId = list.EPDetailId,
                                Frequency = new FrequencyMaster
                                {
                                    FrequencyId = Convert.ToInt32(t["FrequencyId"].ToString()),
                                    DisplayName = t["DisplayName"].ToString(),
                                    Days = Convert.ToInt32(t["Days"].ToString())
                                },
                                FrequencyId = Convert.ToInt32(t["FrequencyId"].ToString()),

                            };
                            list.EPFrequency.Add(epfreq);
                        }
                        if (list.IsDocRequired)
                            list.DocumentType = DocumentTypes(list.EPDetailId);

                        if (!string.IsNullOrEmpty(row["EPDetailId"].ToString()))
                        {
                            list.CopDetails = new List<CopDetails>();
                            var copExpression = "EPDetailId = '" + list.EPDetailId + "'";

                            var drfoundRows = ds.Tables[6].Select(copExpression);
                            for (int i = 0; i < drfoundRows.Length; i++)
                            {
                                var copdetail = new CopDetails
                                {
                                    CopText = Convert.ToString(drfoundRows[i]["CopText"]),
                                    RequirementName = Convert.ToString(drfoundRows[i]["RequirementName"]),
                                    CopStdesc = new CopStdesc()
                                    {
                                        CopTitle = Convert.ToString(drfoundRows[i]["CopTitle"]),
                                        TagCode = Convert.ToString(drfoundRows[i]["TagCode"])
                                    }
                                };
                                list.CopDetails.Add(copdetail);
                            }
                        }
                    }
                }
            }
            return list;
        }

        public EPDetails GetEpShortDetails(int epDetailId)
        {
            var list = new EPDetails();
            const string sql = StoredProcedures.Get_Eps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetailId", epDetailId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        list.IsExpired = Convert.ToBoolean(row["IsExpired"].ToString());
                        list.EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString());
                        list.StDescID = Convert.ToInt32(row["StDescID"].ToString());
                        list.Description = row["Description"].ToString();
                        if (!string.IsNullOrEmpty(row["ScoreId"].ToString()))
                            list.ScoreId = Convert.ToInt32(row["ScoreId"].ToString());
                        list.EPNumber = row["EPNumber"].ToString();
                        list.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
                        list.IsApplicable = Convert.ToBoolean(row["IsApplicable"]);
                        if (!string.IsNullOrEmpty(row["ScoreId"].ToString()))
                        {
                            list.Scores = new Score
                            {
                                Id = Convert.ToInt32(row["ScoreId"].ToString())
                            };
                        }
                        else
                        {
                            list.Scores = new Score();
                        }
                        list.StDescID = Convert.ToInt32(row["StDescID"].ToString());
                        list.Scores.Name = row["ScoreName"].ToString();
                        list.IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString());
                        list.IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString());
                        list.DocStatus = DocStaus(list.EPDetailId, list.IsDocRequired);
                        if (!string.IsNullOrEmpty(row["InitialInspDate"].ToString()))
                            list.InitialInspDate = Convert.ToDateTime(row["InitialInspDate"].ToString());

                        list.Standard = new Standard
                        {
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                            DisplayDescription = row["TJCDescription"].ToString(),
                            TJCStandard = row["TJCStandard"].ToString(),
                            CmsStandard = row["CmsStandard"].ToString(),
                            CmsStdDescription = row["CmsStdDescription"].ToString(),
                            Category = new Category
                            {
                                CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                Name = row["CategoryName"].ToString()
                            }
                        };
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        private EPDetails GetEpDetail(int epDetailId)
        {
            return GetEpsAll().FirstOrDefault(x => x.EPDetailId == epDetailId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EPDetails> GetEpAssignment()
        {
            var lists = GetEps().Where(x => !x.IsExpired).ToList();
            var epUsers = GetEPUsers();
            if (epUsers.Any())
            {
                foreach (var ep in lists)
                {
                    ep.EPUsers = epUsers.Where(x => x.EPDetailId == ep.EPDetailId).Select(x => x.UserProfile).ToList();
                }
            }
            return lists;
        }

        public IEnumerable<EPAssignee> GetEPUsers()
        {
            var epAssignees = new List<EPAssignee>();
            const string sql = StoredProcedures.Get_EPAssignee;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    epAssignees = _sqlHelper.ConvertDataTable<EPAssignee>(ds.Tables[0]);
                    var userProfile = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[1]);
                    foreach (var epAssignee in epAssignees)
                    {
                        epAssignee.UserProfile = userProfile.FirstOrDefault(x => x.UserId == epAssignee.UserId);
                    }
                }
            }
            return epAssignees;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        private EPDetails GetEps(int epDetailId)
        {
            return GetEps(null, epDetailId).FirstOrDefault();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EPDetails> GetEps()
        {
            return GetEps(null, null);
        }

        public List<EPDetails> GetEpsAll()
        {
            return GetEpsAll(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stdescId"></param>
        /// <returns></returns>
        private List<EPDetails> GetStandardEp(int stdescId)
        {
            return GetEps(stdescId, null);
        }

        /// <summary>
        /// Get All EPs Short Info
        /// </summary>
        /// <returns></returns>
        private List<EPDetails> GetEps(int? stdesc, int? ePDetailId)
        {
            return GetAllEps(stdesc, ePDetailId).Where(x => x.IsActive).ToList();
        }

        private List<EPDetails> GetEpsAll(int? stdesc, int? ePDetailId)
        {
            return GetAllEps(stdesc, ePDetailId).ToList();
        }


        public List<EPDetails> GetAllEps(int? stdesc, int? ePDetailId)
        {
            var lists = new List<EPDetails>();
            const string sql = StoredProcedures.Get_Eps;
            var frequencyMaster = _frequencyRepository.GetFrequency();
            var epFrequencyList = GetEpFrequency();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StDescID", stdesc);
                command.Parameters.AddWithValue("@EPDetailId", ePDetailId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var inspectionDates = _sqlHelper.ConvertDataTable<TInspectionDates>(ds.Tables[2]);
                    var hospitalTypes = _sqlHelper.ConvertDataTable<HospitalType>(ds.Tables[4]);
                    var epdescriptions = _sqlHelper.ConvertDataTable<EPDescriptions>(ds.Tables[5]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var list = new EPDetails
                        {
                            EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            Description = row["Description"].ToString(),
                            EPNumber = row["EPNumber"].ToString(),
                            IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString()),
                            IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString()),
                            IsIlsmEP = Convert.ToBoolean(row["IsIlsmEP"].ToString()),
                            Priority = Convert.ToInt32(row["Priority"].ToString()),
                            //ScoreId = Convert.ToInt32(row["ScoreId"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            IsApplicable = Convert.ToBoolean(row["IsApplicable"]),
                            IsFrequencyChange = Convert.ToBoolean(row["IsFrequencyChange"].ToString()),
                            InitialInpDateEditable = Convert.ToBoolean(row["InitialInpDateEditable"]),
                            IsExpired = Convert.ToBoolean(row["IsExpired"]),
                            EPSearchText = row["EPSearchText"].ToString()
                            //EpdetailGuid= Guid.Parse(row["EpdetailGuid"].ToString())
                            //IsExpired = Convert.ToBoolean(row["IsExpired"])
                        };
                        if (!string.IsNullOrEmpty(row["ScoreId"].ToString()))
                            list.ScoreId = Convert.ToInt32(row["ScoreId"].ToString());
                        if (!string.IsNullOrEmpty(row["IsCurrent"].ToString()))
                            list.IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString());
                        if (!string.IsNullOrEmpty(row["EPStatus"].ToString()))
                            list.EPStatus = Convert.ToInt16(row["EPStatus"].ToString());

                        if (!string.IsNullOrEmpty(row["InspectionDate"].ToString()))
                        {
                            list.InspectionDate = Convert.ToDateTime(row["InspectionDate"].ToString());
                        }


                        if (!string.IsNullOrEmpty(row["ExpiryDate"].ToString()))
                            list.ExpiryDate = Convert.ToDateTime(row["ExpiryDate"].ToString());

                        if (!string.IsNullOrEmpty(row["EffectiveDate"].ToString()))
                            list.EffectiveDate = Convert.ToDateTime(row["EffectiveDate"].ToString());

                        list.Standard = new Standard
                        {
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                            TJCStandard = row["TJCStandard"].ToString(),
                            DisplayDescription = row["TJCDescription"].ToString()
                        };

                        // handled for scoreid and score name is null or empty
                        if (!string.IsNullOrEmpty(row["ScoreId"].ToString()))
                        {
                            list.Scores = new Score
                            {
                                Id = Convert.ToInt32(row["ScoreId"].ToString()),
                                Name = row["ScoreName"].ToString()
                            };
                        }
                        else
                        {
                            list.Scores = new Score();
                        }


                        var epFrequency = new List<EPFrequency>();
                        foreach (var frequency in epFrequencyList.Where(x => x.EpDetailId == list.EPDetailId && x.IsActive))
                        {
                            frequency.TjcFrequency = frequencyMaster.SingleOrDefault(x => x.FrequencyId == frequency.TjcFrequencyId);
                            frequency.Frequency = frequencyMaster.SingleOrDefault(x => x.FrequencyId == frequency.FrequencyId);
                            if (frequency.RecFrequencyId.HasValue)
                                frequency.RecFrequency = frequencyMaster.SingleOrDefault(x => x.FrequencyId == frequency.RecFrequencyId);
                            epFrequency.Add(frequency);
                        }
                        if (!string.IsNullOrEmpty(row["IsAssetsRequired"].ToString()))
                        {
                            list.Assets = new List<Assets>();
                            var assetExpression = "EPDetailId = '" + list.EPDetailId + "'";
                            var AssetSortOrder = "Name ASC";
                            var drfoundRows = ds.Tables[3].Select(assetExpression, AssetSortOrder);
                            for (var i = 0; i < drfoundRows.Length; i++)
                            {
                                var asset = new Assets
                                {
                                    Name = Convert.ToString(drfoundRows[i]["Name"]),
                                    AssetId = Convert.ToInt32(drfoundRows[i]["AssetId"]),
                                    AssetTypeId = Convert.ToInt32(drfoundRows[i]["TypeId"])
                                };
                                list.Assets.Add(asset);
                            }
                        }

                        if (!string.IsNullOrEmpty(row["EPDetailId"].ToString()))
                        {
                            list.CopDetails = new List<CopDetails>();
                            var copExpression = "EPDetailId = '" + list.EPDetailId + "'";

                            var drfoundRows = ds.Tables[6].Select(copExpression);
                            for (int i = 0; i < drfoundRows.Length; i++)
                            {
                                var copdetail = new CopDetails
                                {
                                    CopText = Convert.ToString(drfoundRows[i]["CopText"]),
                                    RequirementName = Convert.ToString(drfoundRows[i]["RequirementName"]),
                                    CopStdesc = new CopStdesc()
                                    {
                                        CopTitle = Convert.ToString(drfoundRows[i]["CopTitle"]),
                                        TagCode = Convert.ToString(drfoundRows[i]["TagCode"])
                                    }
                                };
                                list.CopDetails.Add(copdetail);
                            }
                        }

                        if (epFrequency.Count > 0)
                            list.EPFrequency = epFrequency;

                        if (list.IsDocRequired)
                            list.DocumentType = DocumentTypes(list.EPDetailId);

                        list.InspectionDates = inspectionDates.Where(x => x.EpDetailId == list.EPDetailId).ToList();
                        list.EPDescriptions = GetHospitalTypeEPdescriptions(hospitalTypes, epdescriptions, list);
                        lists.Add(list);
                    }
                }
            }
            return lists;
        }

        public List<EPDescriptions> GetHospitalTypeEpDescriptions()
        {
            List<HospitalType> hospitalType;
            List<EPDescriptions> lstepdesc = new();
            const string sql = StoredProcedures.Get_HospitalType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                hospitalType = _sqlHelper.ConvertDataTable<HospitalType>(ds.Tables[0]);
                lstepdesc = GetHospitalTypeEPdescriptions(hospitalType, null, null);
            }
            return lstepdesc;
        }

        private List<EPDescriptions> GetHospitalTypeEPdescriptions(IEnumerable<HospitalType> hospitalTypes, IReadOnlyCollection<EPDescriptions> epDescriptions, EPDetails list)
        {
            var lstepdesc = new List<EPDescriptions>();
            foreach (var item in hospitalTypes)
            {
                var obj = new EPDescriptions { HospitalTypeId = item.HospitalTypeId, Type = item.Type };
                if (epDescriptions != null && list != null)
                {
                    obj.EPDetailId = list.EPDetailId;
                    var epdesc = epDescriptions.FirstOrDefault(x => x.EPDetailId == list.EPDetailId && x.HospitalTypeId == item.HospitalTypeId);
                    if (epdesc != null)
                    {
                        obj.EPDescriptionId = epdesc.EPDescriptionId;
                        obj.Description = epdesc.Description;
                    }
                }
                lstepdesc.Add(obj);
            }

            return lstepdesc;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EPDetails> GetEpAssignmentUser()
        {
            var ePlists = new List<EPDetails>();
            var lists = GetEpAssignment();
            var userlists = _usersRepository.GetUsersList().Where(x => x.IsActive).ToList();
            foreach (var item in userlists)
            {
                var tempEpdetails = lists.Where(x => x.EPUsers.Any(y => y.UserId == item.UserId)).ToList();
                foreach (var tempEp in tempEpdetails)
                {
                    tempEp.EPUsers.RemoveAll(x => x.UserId != item.UserId);
                }
                ePlists.AddRange(tempEpdetails);
            }
            return ePlists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EPDetails> OngoingEpDetails()
        {
            var epDetails = GetEps(null, null);
            return epDetails;
        }


        public List<EPDetails> GetUserDeficientEPs(int userId)
        {
            var lists = new List<EPDetails>();
            const string sql = StoredProcedures.Get_UserDeficientEPs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[1]);
                    var inspectionActivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[2]);
                    foreach (var item in inspections)
                    {
                        item.TInspectionActivity = inspectionActivity.Where(x => x.InspectionId == item.InspectionId).ToList();
                    }

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var eP = new EPDetails
                        {
                            EPNumber = item["EPNumber"].ToString(),
                            EPStatus = Convert.ToInt32(item["EPStatus"].ToString()),
                            Description = item["Description"].ToString(),
                            EPDetailId = Convert.ToInt32(item["EPDetailId"].ToString()),
                            ScoreId = Convert.ToInt32(item["ScoreId"].ToString()),
                            StDescID = Convert.ToInt32(item["StDescID"].ToString()),
                            IsDocRequired = Convert.ToBoolean(item["IsDocRequired"].ToString()),
                            IsAssetsRequired = Convert.ToBoolean(item["IsAssetsRequired"].ToString()),
                            Standard = new Standard { TJCStandard = item["TJCStandard"].ToString() },
                            Scores = new Score { Name = item["ScoreName"].ToString() }
                        };
                        if (eP.IsAssetsRequired)
                            eP.Assets = _assetsRepository.GetEpAssets(eP.EPDetailId);

                        eP.Inspection = inspections.FirstOrDefault(x => x.EPDetailId == eP.EPDetailId);

                        lists.Add(eP);
                    }
                }
            }

            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public StandardEps GetStandardEPs(int epDetailId)
        {
            return GetStandardEPs(new int[1] { epDetailId }).FirstOrDefault();
        }


        public List<StandardEps> GetStandardEPs(int[] epDetailId)
        {
            var objStandardEps = new List<StandardEps>();
            const string sql = StoredProcedures.Get_ViewEpDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPdetailId", string.Join(",", epDetailId));
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    objStandardEps = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[0]);
            }
            return objStandardEps;
        }

        public List<StandardEps> GetStandardEPs()
        {
            var objStandardEps = new List<StandardEps>();
            const string sql = StoredProcedures.Get_StandardEps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    objStandardEps = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[0]);
            }
            return objStandardEps;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EPDetails> GetIlsmEPs()
        {
            return GetEps().Where(x => x.IsIlsmEP).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public List<EPDetails> GetEpByAssets(int assetId)
        {
            var lists = new List<EPDetails>();
            const string sql = StoredProcedures.Get_AssetsEPs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId);
                command.Parameters.AddWithValue("@floorAssetId", null);
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        EPDetails ep = new EPDetails();
                        ep.Description = row["Description"].ToString();
                        ep.EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString());
                        ep.EPNumber = row["EPNumber"].ToString();
                        ep.Standard = new Standard
                        {
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                            DisplayDescription = row["TJCDescription"].ToString(),
                            TJCStandard = row["TJCStandard"].ToString(),
                            //CmsStandard = row["CmsStandard"].ToString(),
                            // CmsStdDescription = row["CmsStdDescription"].ToString()
                        };
                        var epfreq = new EPFrequency
                        {
                            EpDetailId = ep.EPDetailId,
                            Frequency = new FrequencyMaster
                            {
                                FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                                DisplayName = row["DisplayName"].ToString(),
                                Days = Convert.ToInt32(row["Days"].ToString())
                            },
                            FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),

                        };
                        ep.EPFrequency.Add(epfreq);
                        ep.IsAssetSteps = Convert.ToInt32(row["IsAssetSteps"].ToString());
                        var epasset = new Assets
                        {
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            Name = row["Name"].ToString(),

                        };
                        ep.Assets.Add(epasset);
                        lists.Add(ep);
                    }


                    foreach (DataRow row in dt.Tables[2].Rows)
                    {

                        var tempEp = new EPDetails();
                        var epfreq = new EPFrequency
                        {

                            Frequency = new FrequencyMaster
                            {
                                FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                                DisplayName = row["DisplayName"].ToString(),
                                Days = Convert.ToInt32(row["Days"].ToString())
                            },
                            FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),

                        };
                        tempEp.EPFrequency.Add(epfreq);
                        var epasset = new Assets
                        {
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            Name = row["Name"].ToString(),

                        };
                        tempEp.Assets.Add(epasset);
                        lists.Add(tempEp);

                        ;
                    };
                    //    var frequencies = _sqlHelper.ConvertDataTable<FrequencyMaster>(dt.Tables[2]);
                    //if (frequencies.Any())
                    //{
                    //    foreach (var withOutEpFrequency in frequencies)
                    //    {
                    //        var tempEp = new EPDetails { EPFrequency = new List<EPFrequency>() };
                    //        var newEpFrequency = new EPFrequency { Frequency = withOutEpFrequency };                           
                    //        tempEp.EPFrequency.Add(newEpFrequency);                           
                    //        lists.Add(tempEp);
                    //    }
                    //}
                }
            }
            return lists;
        }

        public EpAssets UpdateEpAssets(EpAssets epAssets)
        {
            const string sql = StoredProcedures.Update_EpAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", epAssets.AssetId);
                command.Parameters.AddWithValue("@EPDetailId", epAssets.EPDetailId == null ? 0 : epAssets.EPDetailId.Value);
                command.Parameters.AddWithValue("@IsActive", epAssets.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", epAssets.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                epAssets.MappingId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return epAssets;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDocRequired"></param>
        /// <returns></returns>
        public List<EPDetails> GetEpDetails(bool isDocRequired)
        {
            var epDetails = GetEps().Where(x => x.IsDocRequired).ToList();
            return epDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public EPDetails GetDocsbyEp(int userId, int epDetailId)
        {
            var epDetails = GetEpDescription(epDetailId);
            epDetails.Assets = Assets(epDetailId);
            return epDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epdetailId"></param>
        /// <returns></returns>
        public EPDetails GetEpInspectionHistory(int epdetailId)
        {
            var ep = GetEpDetail(epdetailId);
            return ep;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        public EPDetails GetEpInspectionHistory(int epDetailId, int inspectionId)
        {
            var ep = GetEpDetail(epDetailId);
            return ep;
        }

        public List<EPFrequency> GetEpFrequency()
        {
            var list = new List<EPFrequency>();
            const string sql = StoredProcedures.Get_EPsFrequency;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    list = _sqlHelper.ConvertDataTable<EPFrequency>(dt);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public List<EPFrequency> GetEpFrequency(int epDetailId)
        {
            return GetEpFrequency().Where(x => x.EpDetailId == epDetailId && x.IsActive).ToList();
        }

        #endregion

        #region Extension Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <param name="currentInspection"></param>
        /// <returns></returns>
        public string EpTransStatus(int epDetailId, Inspection currentInspection = null)
        {
            var epTransSt = "P";
            if (currentInspection == null)
                currentInspection = GetCurrentInspection(epDetailId);
            if (currentInspection != null)
            {
                var nextDueDate = currentInspection.DueDate;
                var dueDate = Convert.ToDateTime(currentInspection.DueDate).Date.AddDays(1);
                var graceDate = new DateTime();
                if (currentInspection.GraceDate.HasValue)
                    graceDate = currentInspection.GraceDate.Value.Date.AddDays(1);


                if (currentInspection.EPDetails != null && currentInspection.EPDetails.InitialInspDate.HasValue)
                {
                    nextDueDate = ScheduleDateTime.GetScheduleFixedDate(currentInspection.EPDetails.FrequencyId, currentInspection.EPDetails.InitialInspDate.Value, 0);
                    dueDate = nextDueDate.Value;
                    graceDate = nextDueDate.Value;
                }

                if (currentInspection.Status == 0)
                    epTransSt = "F";
                else if (nextDueDate == null)
                    epTransSt = "I";
                else
                {
                    var currentDate = DateTime.UtcNow.Date;
                    if (currentDate.Date >= dueDate.Date && currentDate.Date <= graceDate.Date)
                        epTransSt = "G";
                    else if (currentDate > graceDate)
                    {
                        epTransSt = "D";
                        MarkInspectionDueDatePassed(currentInspection.InspectionId);
                    }
                    else
                    {
                        if (currentInspection.Status != null)
                        {
                            switch (currentInspection.Status.Value)
                            {
                                case 2:
                                    epTransSt = "I";
                                    break;
                                case 1:
                                    epTransSt = "C";
                                    break;
                            }
                        }
                    }
                }
            }

            return epTransSt;
        }

        internal bool MarkInspectionDueDatePassed(int inspectionId)
        {
            bool status;
            const string sql = StoredProcedures.Update_InspectionDueDatePassed;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inspectionId", inspectionId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        private Inspection GetCurrentInspection(int epDetaild)
        {
            var inspections = new List<Inspection>();
            const string sql = StoredProcedures.Get_Inspections;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epDetailId", epDetaild);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]);
                    var epDetails = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[0]);
                    var standard = _sqlHelper.ConvertDataTable<Standard>(ds.Tables[0]);
                    var Users = _usersRepository.ConvertToUser(ds.Tables[0]);

                    foreach (var epDetail in epDetails)
                        epDetail.Standard = standard.FirstOrDefault(x => x.StDescID == epDetail.StDescID);
                    foreach (var item in inspections)
                        item.EPDetails = epDetails.FirstOrDefault(x => x.EPDetailId == item.EPDetailId);
                    foreach (var item in inspections)
                        item.UserProfile = Users.FirstOrDefault(x => x.UserId == item.LastUpdatedBy);

                }
            }
            return inspections.FirstOrDefault(x => x.IsCurrent);
        }


        public List<DashboardDetails> GetDashBoardData(int userId)
        {
            List<DashboardDetails> objDashboardDetails;
            const string sql = StoredProcedures.Get_UserDashboardData;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var dt = _sqlHelper.GetDataTable(command);
                objDashboardDetails = _sqlHelper.ConvertDataTable<DashboardDetails>(dt);
            }
            return GetEpByUser(objDashboardDetails, userId);
        }


        private List<T> GetEpByUser<T>(List<T> collection, int userId)
        {
            string property = "EPDetailId";
            var userprofile = _usersRepository.GetUsersList(userId);
            var epAssignee = GetEpAssignee().Where(x => x.UserId == userId && x.Status).ToList();

            if (userprofile.IsSystemUser)
                return collection;


            var filteredCollection = new List<T>();
            foreach (var item in collection)
            {
                var propertyInfo =
                    item.GetType()
                        .GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                    throw new NotSupportedException("property given does not exists");

                var propertyValue = propertyInfo.GetValue(item, null);
                if (propertyValue != null && epAssignee.Any(x => x.EPDetailId == Convert.ToInt32(propertyValue)))
                {
                    filteredCollection.Add(item);
                }
            }

            return filteredCollection;
        }

        private IEnumerable<EPAssignee> GetEpAssignee()
        {
            var epAssignee = new List<EPAssignee>();
            const string sql = StoredProcedures.Get_EPAssignee;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    epAssignee = _sqlHelper.ConvertDataTable<EPAssignee>(ds.Tables[0]);

            }
            return epAssignee;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public List<Assets> Assets(int epDetailId)
        {
            return _assetsRepository.GetEpAssets(epDetailId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <param name="isDocRequired"></param>
        /// <returns></returns>
        private int DocStaus(int epDetailId, bool isDocRequired)
        {
            int docStaus;
            if (isDocRequired)
                docStaus = GetEpDocStatus(epDetailId);
            else
                docStaus = -3;

            return docStaus;
        }

        private int GetEpDocStatus(int epDetailId)
        {
            int status = 0;
            const string sql = StoredProcedures.Get_EpDocStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", epDetailId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    status = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            return status;
        }

        public int DocStaus(int epDetailId)
        {
            return DocStaus(epDetailId, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public int CheckDocRequired(int epDetailId)
        {
            int docStaus = 0;
            var eps = GetEps(epDetailId);
            if (!eps.IsDocRequired)
            {
                docStaus = -3;
            }
            return docStaus;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ePDetailId"></param>
        /// <returns></returns>
        private List<DocumentType> DocumentTypes(int ePDetailId)
        {
            return _documentTypeRepository.GetDocumentTypes(ePDetailId);
        }

        #endregion

        #region EPs Users

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dueWithInDays"></param>
        /// <returns></returns>
        public List<EPDetails> GetDueWithInDaysEPs(int dueWithInDays)
        {
            var list = new List<EPDetails>();
            List<TFloorAssets> tfloorAssets;
            const string sql = StoredProcedures.Get_DueWithInDaysEPs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@duewithindays", dueWithInDays);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    tfloorAssets = _sqlHelper.ConvertDataTable<TFloorAssets>(ds.Tables[2]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var ep = new EPDetails
                        {
                            EPNumber = row["EPNumber"].ToString(),
                            Description = row["Description"].ToString(),
                            EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            Standard = new Standard
                            {
                                CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                TJCStandard = row["TJCStandard"].ToString(),
                                Category = new Category
                                {
                                    CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                    Code = row["Code"].ToString()
                                }
                            },
                            DueWithInDays = Convert.ToInt32(row["DueWithInDays"].ToString()),
                            Scores = new Score
                            {
                                Name = row["Name"].ToString()
                            },
                            IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString()),
                            IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString()),
                        };
                        if (ep.IsAssetsRequired)
                        {
                            ep.Assets = new List<Assets>();
                            string freqexpr = "EPDetailId = '" + ep.EPDetailId + "'";
                            var freqRows = ds.Tables[1].Select(freqexpr);
                            foreach (var dr in freqRows)
                            {
                                var asset = new Assets
                                {
                                    AssetId = Convert.ToInt32(dr["AssetId"].ToString()),
                                    Name = dr["Name"].ToString(),
                                    IsActive = Convert.ToBoolean(dr["IsActive"].ToString())
                                };
                                ep.Assets.Add(asset);
                            }
                            foreach (var assets in ep.Assets)
                            {
                                assets.TFloorAssets = tfloorAssets.Where(x => x.AssetId == assets.AssetId).ToList();
                            }
                        }
                        list.Add(ep);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epdetailId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<EPAssignee> GetUserEPs(int? epdetailId, int? userId, int? docTypeId)
        {
            List<EPAssignee> epAssignee;
            const string sql = StoredProcedures.Get_UserEPs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetailId", epdetailId > 0 ? epdetailId : null);
                command.Parameters.AddWithValue("@userId", userId > 0 ? userId : null);
                command.Parameters.AddWithValue("@docTypeId", docTypeId > 0 ? docTypeId : null);
                var ds = _sqlHelper.GetDataSet(command);
                epAssignee = _sqlHelper.ConvertDataTable<EPAssignee>(ds.Tables[0]);
                var userProfiles = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                foreach (var item in epAssignee)
                {
                    item.UserProfile = new UserProfile();
                    item.UserProfile = userProfiles.FirstOrDefault(x => x.UserId == item.UserId);
                }
            }
            return epAssignee;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        private List<UserProfile> GetEpUsers(int epDetailId)
        {
            return GetUserEPs(epDetailId, null, null).Where(x => x.IsCurrent && x.Status).Select(y => y.UserProfile).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public List<EPAssignee> GetEpAssignee(int userId, int epDetailId)
        {
            var epAssignee = new List<EPAssignee>();
            const string sql = StoredProcedures.Get_EPsUser;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId > 0 ? userId : (object)null);
                command.Parameters.AddWithValue("@epdetailId", epDetailId > 0 ? epDetailId : (object)null);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var data = new EPAssignee
                        {
                            EPDetailId = Convert.ToInt32(item["EPDetailId"].ToString()),
                            Status = Convert.ToBoolean(item["Status"].ToString())
                        };
                        epAssignee.Add(data);
                    }

                    var ePs = new List<EPDetails>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var ep = new EPDetails
                        {
                            EPNumber = item["EPNumber"].ToString(),
                            IsDocRequired = Convert.ToBoolean(item["IsDocRequired"].ToString()),
                            IsAssetsRequired = Convert.ToBoolean(item["IsAssetsRequired"].ToString()),
                            Description = item["Description"].ToString(),
                            EPDetailId = Convert.ToInt32(item["EPDetailId"].ToString()),
                            DocStatus = Convert.ToInt32(item["DocStatus"].ToString()),
                            Standard = new Standard
                            {
                                TJCStandard = item["TJCStandard"].ToString()
                            },
                            EPUsers = new List<UserProfile>()
                        };
                        var freqexpr = "EPDetailId = '" + ep.EPDetailId + "'";
                        var freqRows = ds.Tables[1].Select(freqexpr);
                        foreach (var dr in freqRows)
                        {
                            var user = new UserProfile
                            {
                                UserId = Convert.ToInt32(dr["UserId"].ToString()),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                //IsInternalUser = Convert.ToBoolean(dr["IsInternalUser"])
                            };
                            ep.EPUsers.Add(user);
                        }

                        if (ep.IsAssetsRequired)
                            ep.Assets = Assets(ep.EPDetailId);

                        if (ep.IsDocRequired)
                            ep.DocumentType = DocumentTypes(ep.EPDetailId);
                        ePs.Add(ep);
                    }

                    foreach (var item in epAssignee)
                    {
                        item.EPDetails = ePs.FirstOrDefault(x => x.EPDetailId == item.EPDetailId);
                    }
                }
            }
            return epAssignee;
        }

        #endregion


        #region EP Assignment

        public List<EPDetails> GetEpAssignees()
        {
            var lists = new List<EPDetails>();
            const string sql = StoredProcedures.GetEpAssignees;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", 4);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]);
                    var epUsers = _sqlHelper.ConvertDataTable<EPAssignee>(ds.Tables[1]);



                    var epandUsers = from e in epUsers
                                     group e by e.EPDetailId into g
                                     select new
                                     {
                                         EpDetailId = g.Key,
                                         UserIds = g.Select(x => x.UserId).ToList()
                                     };


                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var currentnEpUsers = new List<UserProfile>();
                        var ep = new EPDetails
                        {
                            EPNumber = item["EPNumber"].ToString(),
                            IsDocRequired = Convert.ToBoolean(item["IsDocRequired"].ToString()),
                            IsAssetsRequired = Convert.ToBoolean(item["IsAssetsRequired"].ToString()),
                            Description = item["Description"].ToString(),
                            EPDetailId = Convert.ToInt32(item["EPDetailId"].ToString()),
                            DocStatus = Convert.ToInt32(item["DocStatus"].ToString()),
                            Standard = new Standard
                            {
                                TJCStandard = item["TJCStandard"].ToString(),
                                CategoryId = Convert.ToInt32(item["CategoryId"].ToString())

                            },
                            EPUsers = new List<UserProfile>()
                        };
                        var epUserLists = epandUsers.FirstOrDefault(x => x.EpDetailId == ep.EPDetailId);
                        if (epUserLists != null)
                        {
                            var userLists = epUserLists.UserIds;
                            currentnEpUsers = users.Where(x => userLists.Contains(x.UserId)).ToList();
                        }
                        ep.EPUsers = currentnEpUsers;
                        lists.Add(ep);
                    }
                }
            }
            return lists;
        }

        public List<EPDetails> GetEpAssignees_Paging(Request request)
        {

            var lists = new List<EPDetails>();
            const string sql = StoredProcedures.Get_EpAssignees_Paging_Proc;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", 4);
                command.Parameters.AddWithValue("@pageIndex", request.PageIndex);
                command.Parameters.AddWithValue("@pageSize", request.PageSize);
                command.Parameters.AddWithValue("@sortOrders", request.SortOrder);
                command.Parameters.AddWithValue("@orderbySort", request.OrderbySort);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]);
                    var epUsers = _sqlHelper.ConvertDataTable<EPAssignee>(ds.Tables[1]);

                    var epandUsers = from e in epUsers
                                     group e by e.EPDetailId into g
                                     select new
                                     {
                                         EpDetailId = g.Key,
                                         UserIds = g.Select(x => x.UserId).ToList()
                                     };


                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var currentnEpUsers = new List<UserProfile>();
                        var ep = new EPDetails
                        {
                            EPNumber = item["EPNumber"].ToString(),
                            IsDocRequired = Convert.ToBoolean(item["IsDocRequired"].ToString()),
                            IsAssetsRequired = Convert.ToBoolean(item["IsAssetsRequired"].ToString()),
                            Description = item["Description"].ToString(),
                            EPDetailId = Convert.ToInt32(item["EPDetailId"].ToString()),
                            DocStatus = Convert.ToInt32(item["DocStatus"].ToString()),
                            Standard = new Standard
                            {
                                TJCStandard = item["TJCStandard"].ToString(),
                                CategoryId = Convert.ToInt32(item["CategoryId"].ToString())

                            },
                            EPUsers = new List<UserProfile>()
                        };
                        var epUserLists = epandUsers.FirstOrDefault(x => x.EpDetailId == ep.EPDetailId);
                        if (epUserLists != null)
                        {
                            var userLists = epUserLists.UserIds;
                            currentnEpUsers = users.Where(x => userLists.Contains(x.UserId)).ToList();
                        }
                        ep.EPUsers = currentnEpUsers;
                        lists.Add(ep);
                    }
                }
            }
            return lists;
        }

        public int SaveUserEPs(string mode, int userId, bool status, int epId)
        {
            int newId;
            const string sql = StoredProcedures.Insert_UserEps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@userId", userId > 0 ? userId : (object)null);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@epId", epId > 0 ? epId : (object)null);
                command.Parameters.Add("@outCount", SqlDbType.Int);
                command.Parameters["@outCount"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@outCount");
            }
            return newId;
        }

        #endregion


        #region CMS Dashboard

        public List<CopDetails> GetCMSDashboard(int userid)
        {
            var lstcopDetails = new List<CopDetails>();
            List<CopStdesc> lstcopstdesc = new();
            var epDetails = new List<EPDetails>();
            const string sql = StoredProcedures.Get_CMSDashboard;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userid);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lstcopDetails = _sqlHelper.ConvertDataTable<CopDetails>(ds.Tables[0]);
                    lstcopstdesc = _sqlHelper.ConvertDataTable<CopStdesc>(ds.Tables[0]);
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var ep = new EPDetails
                        {
                            EPNumber = row["EPNumber"].ToString(),
                            Description = row["Description"].ToString(),
                            EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            Priority = Convert.ToInt32(row["Priority"].ToString()),
                            CopDetailsId = Convert.ToInt32(row["CopDetailsId"].ToString()),
                            Standard = new Standard
                            {
                                CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                TJCStandard = row["TJCStandard"].ToString(),
                                Category = new Category
                                {
                                    CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                    Code = row["Code"].ToString()
                                }
                            },
                            Scores = new Score
                            {
                                Name = row["ScoreName"].ToString()
                            },
                            IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString()),
                            DocStatus = Convert.ToInt32(row["DocStatus"].ToString()),
                            Inspection = new Inspection
                            {
                                InspectionId = Convert.ToInt32(row["InspectionId"].ToString()),
                                InspectionGroupId = Convert.ToInt32(row["InspectionGroupId"].ToString()),
                                CreatedDate = string.IsNullOrEmpty(row["StartDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["StartDate"].ToString()),
                                StartDate = string.IsNullOrEmpty(row["StartDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["StartDate"].ToString()),
                                DueDate = string.IsNullOrEmpty(row["DueDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["DueDate"].ToString()),
                                GraceDate = string.IsNullOrEmpty(row["GraceDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["GraceDate"].ToString()),
                                Status = Convert.ToInt32(row["Status"].ToString()),
                                SubStatus = row["SubStatus"].ToString(),
                            },
                        };
                        if (!string.IsNullOrEmpty(row["EPStatus"].ToString()))
                            ep.EPStatus = Convert.ToInt16(row["EPStatus"].ToString());

                        if (ep.Inspection.InspectionId > 0)
                            ep.EpTransStatus = EpTransStatus(ep.EPDetailId, ep.Inspection);
                        else
                            ep.EpTransStatus = "P";

                        ep.EPFrequency = new List<EPFrequency>();
                        if (!string.IsNullOrEmpty(row["FrequencyId"].ToString()))
                        {
                            var epfreq = new EPFrequency
                            {
                                Frequency = new FrequencyMaster
                                {
                                    FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                                    DisplayName = row["FrequencyDisplayName"].ToString(),
                                },
                                FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),

                            };
                            ep.EPFrequency.Add(epfreq);
                        }
                        ep.EPUsers = new List<UserProfile>();
                        string expression = "EPDetailId = '" + ep.EPDetailId + "'";
                        string sortOrder = "FirstName ASC";
                        var foundRows = ds.Tables[2].Select(expression, sortOrder);
                        for (int i = 0; i < foundRows.Length; i++)
                        {
                            var user = new UserProfile
                            {
                                Email = Convert.ToString(foundRows[i]["Email"]),
                                FirstName = Convert.ToString(foundRows[i]["FirstName"]),
                                LastName = Convert.ToString(foundRows[i]["LastName"]),
                                UserId = Convert.ToInt32(foundRows[i]["UserId"])
                            };
                            ep.EPUsers.Add(user);
                        }
                        ep.IsCustomFrequency = Convert.ToBoolean(row["IsCustomFrequency"]);
                        epDetails.Add(ep);
                    }
                    foreach (var item in lstcopDetails)
                    {
                        item.CopStdesc = lstcopstdesc.FirstOrDefault(x => x.CopStdescId == item.CopStdescId);
                        item.EPdetails = epDetails.Where(x => x.CopDetailsId == item.CopDetailsId).ToList();
                    }
                }
            }
            return lstcopDetails;
        }


        public List<CopDetails> GetCopDetails()
        {
            var list = new List<CopDetails>();
            const string sql = StoredProcedures.Get_CopDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    list = _sqlHelper.ConvertDataTable<CopDetails>(ds.Tables[0]);
                }
            }
            return list;
        }

        public List<CmsEpMapping> GetCmsEpMapping()
        {
            var list = new List<CmsEpMapping>();
            const string sql = StoredProcedures.Get_CopDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    list = _sqlHelper.ConvertDataTable<CmsEpMapping>(ds.Tables[1]);
                }
            }
            return list;
        }

        #endregion


        #region binder search

        public List<BinderSearch> GetBinderSearch(string searchString)
        {
            List<BinderSearch> binders = new List<BinderSearch>();
            const string sql = StoredProcedures.Get_BinderSearch;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@search", searchString);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    binders = _sqlHelper.ConvertDataTable<BinderSearch>(ds.Tables[0]);
                }
            }
            return binders;
        }
        #endregion

        #region Upgrade Ep
        public int UpgradeEp(string stdescId, string epdetailId, int? CreatedBy)
        {
            int newId;
            const string sql = StoredProcedures.Upgrade_EPDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", epdetailId);
                command.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        #endregion      

        #region TinspectionCycle
        public List<InspectionReport> GetTinspectionCycle(string dataSortOrder, string orderBySort, int year, int userId, int currentUserId, int? categoryId = null)
        {
            List<InspectionReport> lists = new List<InspectionReport>();
            const string sql = StoredProcedures.Get_TinspectionCycle;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@sortOrders", dataSortOrder);
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@currentUserId", currentUserId);

                command.Parameters.AddWithValue("@orderbySort", orderBySort);
                command.Parameters.AddWithValue("@categoryId", categoryId > 0 ? categoryId : null);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[1]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var ep = new InspectionReport
                        {
                            EPNumber = row["EPNumber"].ToString(),
                            EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString()),
                            IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString()),
                            Years = GetYears(year).OrderByDescending(x => x.YearId).ToList(),

                        };

                        lists.Add(ep);
                        foreach (var item in ep.Years)
                        {
                            foreach (var month in item.Months)
                            {
                                month.Inspections = new List<Inspection>();
                                month.Inspections = inspections.Where(x => x.StartDate.Year == Convert.ToInt16(item.Name) && x.StartDate.Month == month.MonthId && x.EPDetailId == ep.EPDetailId && x.IsPreviousCycle != -3).ToList();
                            }
                        }

                        ep.EPFrequency = new List<EPFrequency>();
                        string expr = "EpDetailId = '" + ep.EPDetailId + "'";
                        DataRow[] drRows = ds.Tables[0].Select(expr);
                        for (var i = 0; i < drRows.Length; i++)
                        {
                            var epfreq = new EPFrequency
                            {
                                Frequency = new FrequencyMaster
                                {
                                    FrequencyId = Convert.ToInt32(drRows[i]["FrequencyId"]),
                                }
                            };
                            ep.EPFrequency.Add(epfreq);
                        }
                    }
                }
                return lists;
            }
        }

        private IEnumerable<Year> GetYears(int year)
        {
            var years = new List<Year>();
            var yr = new Year
            {
                Name = year.ToString(),
                YearId = 1,
                Months = GetMonths()
            };
            years.Add(yr);
            return years;
        }

        private List<Months> GetMonths()
        {
            var months = new List<Months>();
            string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;
            for (int i = 0; i < names.Length - 1; i++)
            {
                var mn = new Months { MonthId = i + 1, Name = names[i] };
                months.Add(mn);
            }
            return months;
        }

        #endregion

        public int UpdateEpInitScheduleDate(int[] epIds, DateTime scheduleDate)
        {
            int status = 0;
            const string sql = StoredProcedures.Update_EPScheduleDate;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@scheduleDate", scheduleDate);
                command.Parameters.AddWithValue("@EPDetailId", string.Join(",", epIds));
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                status = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return status;
        }

        public List<EPDetails> GetAssetsEPOnRoute(int? routeId, string assetids)
        {
            var lists = new List<EPDetails>();
            const string sql = StoredProcedures.Get_AssetsEPOnRoute;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@routeId", routeId);
                command.Parameters.AddWithValue("@assetids", assetids);
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        EPDetails ep = new EPDetails();
                        ep.EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString());
                        ep.EPNumber = row["StandardEP"].ToString();
                        var epfreq = new EPFrequency
                        {
                            EpDetailId = ep.EPDetailId,
                            Frequency = new FrequencyMaster
                            {
                                FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                                DisplayName = row["DisplayName"].ToString(),
                            },
                            FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),

                        };

                        ep.EPFrequency.Add(epfreq);
                        var epasset = new Assets
                        {
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            Name = row["AssetsName"].ToString(),
                        };
                        ep.Assets.Add(epasset);
                        lists.Add(ep);
                    }
                }
            }
            return lists;
        }

        public List<EPDetails> GetEpByBulkAssets(string assetId)
        {
            var lists = new List<EPDetails>();
            const string sql = StoredProcedures.Get_AssetsEPs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetId", assetId);
                command.Parameters.AddWithValue("@floorAssetId", null);
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        EPDetails ep = new EPDetails();
                        ep.Description = row["Description"].ToString();
                        ep.EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString());
                        ep.EPNumber = row["EPNumber"].ToString();
                        ep.Standard = new Standard
                        {
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                            DisplayDescription = row["TJCDescription"].ToString(),
                            TJCStandard = row["TJCStandard"].ToString(),
                        };
                        var epfreq = new EPFrequency
                        {
                            EpDetailId = ep.EPDetailId,
                            Frequency = new FrequencyMaster
                            {
                                FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                                DisplayName = row["DisplayName"].ToString(),
                                Days = Convert.ToInt32(row["Days"].ToString())
                            },
                            FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),

                        };
                        ep.EPFrequency.Add(epfreq);
                        ep.IsAssetSteps = Convert.ToInt32(row["IsAssetSteps"].ToString());
                        var epasset = new Assets
                        {

                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            Name = row["Name"].ToString(),
                        };
                        ep.Assets.Add(epasset);
                        lists.Add(ep);
                    }
                    foreach (DataRow row in dt.Tables[2].Rows)
                    {

                        var tempEp = new EPDetails();
                        var epfreq = new EPFrequency
                        {

                            Frequency = new FrequencyMaster
                            {
                                FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                                DisplayName = row["DisplayName"].ToString(),
                                Days = Convert.ToInt32(row["Days"].ToString())
                            },
                            FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),

                        };
                        tempEp.EPFrequency.Add(epfreq);
                        var epasset = new Assets
                        {
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            Name = row["Name"].ToString(),

                        };
                        tempEp.Assets.Add(epasset);
                        lists.Add(tempEp);

                        ;
                    };
                    //var frequencies = _sqlHelper.ConvertDataTable<FrequencyMaster>(dt.Tables[2]);                    
                    //if (frequencies.Any())
                    //{
                    //    foreach (var withOutEpFrequency in frequencies)
                    //    {
                    //        var tempEp = new EPDetails { EPFrequency = new List<EPFrequency>() };                           
                    //        var newEpFrequency = new EPFrequency { Frequency = withOutEpFrequency };                           
                    //        tempEp.EPFrequency.Add(newEpFrequency);
                    //        lists.Add(tempEp);
                    //    }
                    //}
                }
            }
            return lists;
        }

    }
}