using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace HCF.DAL
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IUsersRepository _usersRepository;
        private readonly IInspectionActivityRepository _inspectionActivityRepository;
        private readonly IFloorAssetRepository _floorAssetRepository;
        private readonly IEpRepository _epRepository;
        //private readonly ITransaction _transaction;
        private readonly IInspectionRepository _inspectionRepository;

        public ReportsRepository(ISqlHelper sqlHelper, IInspectionRepository inspectionRepository,
            IUsersRepository usersRepository, IEpRepository epRepository,
            //ITransaction transaction,
            IFloorAssetRepository floorAssetRepository,
            IInspectionActivityRepository inspectionActivityRepository)
        {
            _inspectionRepository = inspectionRepository;
            _usersRepository = usersRepository;
            _inspectionActivityRepository = inspectionActivityRepository;
            _floorAssetRepository = floorAssetRepository;
            _epRepository = epRepository;
            //_transaction = transaction;
            _sqlHelper = sqlHelper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tInspectionReport"></param>
        /// <returns></returns>
        public int Save(TInspectionReport tInspectionReport)
        {
            int newId;
            const string sql = StoredProcedures.Insert_InspectionReport;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FileName", tInspectionReport.FileName);
                command.Parameters.AddWithValue("@ReportId", tInspectionReport.ReportId);
                command.Parameters.AddWithValue("@FilePath", tInspectionReport.FilePath);
                command.Parameters.AddWithValue("@ReportName", tInspectionReport.ReportName);
                command.Parameters.AddWithValue("@SignPath", tInspectionReport.SignPath);
                command.Parameters.AddWithValue("@Comment", tInspectionReport.Comment);
                command.Parameters.AddWithValue("@IsDeleted", tInspectionReport.IsDeleted);
                command.Parameters.AddWithValue("@CreatedBy", tInspectionReport.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportDetails"></param>
        /// <returns></returns>
        public int Save(TInsReportDetails reportDetails)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TInsReportDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ReportId", reportDetails.ReportId);
                command.Parameters.AddWithValue("@ActivityId", reportDetails.ActivityId);
                command.Parameters.AddWithValue("@inspectionId", reportDetails.InspectionId);
                command.Parameters.AddWithValue("@ePDetailId", reportDetails.EpDetailId);
                command.Parameters.AddWithValue("@floorAssetId", reportDetails.FloorAssetId);
                command.Parameters.AddWithValue("@Comment", reportDetails.Comment);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <param name="assetId"></param>
        /// <param name="floorAssetId"></param>
        /// <param name="inspectiopnId"></param>
        /// <returns></returns>
        private List<TInspectionReport> GetInspectionReport(string assetids, int? epDetailId, int? floorAssetId, int? inspectiopnId = null, DateTime? fromdate = null, DateTime? todate = null, int? signedby = null)
        {

            if (string.IsNullOrEmpty(assetids))
            {
                assetids = null;
            }
            if (string.IsNullOrEmpty(Convert.ToString(fromdate)) && string.IsNullOrEmpty(Convert.ToString(todate)))
            {
                fromdate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToString());
                todate = Convert.ToDateTime(DateTime.Now.ToString());
            }
            var lists = new List<TInspectionReport>();
            const string sql = StoredProcedures.Get_InspectionReports;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EpDetailId", epDetailId);
                command.Parameters.AddWithValue("@AssetId", assetids);
                command.Parameters.AddWithValue("@FloorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@InspectiopnId", inspectiopnId);
                command.Parameters.AddWithValue("@fromdate", fromdate);
                command.Parameters.AddWithValue("@todate", todate);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    lists = _sqlHelper.ConvertDataTable<TInspectionReport>(dt);
                foreach (var item in lists)
                {
                    item.Signby = _usersRepository.GetUserProfile(item.CreatedBy);
                }

                if (signedby.HasValue && signedby.Value > 0)
                {
                    lists = lists.Where(x => x.CreatedBy == signedby.Value).ToList();
                }
                return lists;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TInsReportDetails> GetTInsReportDetails()
        {
            var lists = new List<TInsReportDetails>();
            const string sql = StoredProcedures.Get_TInsReportDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    lists = _sqlHelper.ConvertDataTable<TInsReportDetails>(dt);
                return lists;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TInspectionReport> GetInspectionReport()
        {
            var lists = GetInspectionReport(null, null, null, null);
            return lists;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public List<TInspectionReport> GetInspectionReport(int epDetailId)
        {
            var lists = GetInspectionReport(null, epDetailId, null);
            return lists;

        }
        public List<TInspectionReport> GetInspectionReports(string assetids, int? epDetailId = 0, DateTime? fromdate = null, DateTime? todate = null, int? signedby = null)
        {
            var lists = GetInspectionReport(assetids, epDetailId, null, null, fromdate, todate, signedby);
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="floorAssetId"></param>
        /// <returns></returns>
        public List<TInspectionReport> GetInspectionReport(int assetId, int floorAssetId)
        {
            var lists = GetInspectionReport(null, null, assetId, floorAssetId);
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TInspectionReport> GetAssetsReport(int assetId)
        {
            var lists = GetInspectionReport(null, null, assetId, null);
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public List<TInspectionReport> GetEpReport(int inspectionId, int epDetailId)
        {
            var lists = GetInspectionReport(null, epDetailId, null, inspectionId);
            return lists;
        }



        #region Extension Methods

        //public  List<TInsReportDetails> TInsReportDetails(int reportId)
        //{
        //    return GetTInsReportDetails().Where(x => x.ReportId == reportId).ToList();
        //}

        //public  List<Inspection> Inspections(int reportId)
        //{
        //    List<TInsReportDetails> list = GetTInsReportDetails().Where(x => x.ReportId == reportId).ToList();
        //    List<Inspection> inspections = list.GroupBy(i => i.InspectionId)
        //      .Select(g => new Inspection
        //      {
        //          InspectionId = g.Key,
        //          EPDetailId = g.FirstOrDefault().EpDetailId
        //      }).ToList();

        //    foreach (Inspection ins in inspections)
        //    {
        //        List<TInspectionActivity> activity = new List<TInspectionActivity>();
        //        List<TInsReportDetails> details = list.Where(x => x.InspectionId == ins.InspectionId).ToList();
        //        foreach (TInsReportDetails detail in details)
        //        {
        //            TInspectionActivity act = InspectionActivityRepository.GetInspectionActivity(detail.ActivityId, detail.FloorAssetId.Value);
        //            activity.Add(act);
        //        }
        //        ins.TInspectionActivity = activity;
        //    }
        //    return inspections;
        //}

        #endregion

        public List<InspectionReport> GetInspectionDetailReports(int year, int userId)
        {
            var lists = new List<InspectionReport>();
            const string sql = StoredProcedures.Get_InspectionDetailReports;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[3]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var ep = new InspectionReport
                        {
                            EPNumber = row["EPNumber"].ToString(),
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
                                }
                            },
                            IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString()),
                            Years = GetYears(year).OrderByDescending(x => x.YearId).ToList(),
                        };
                        lists.Add(ep);
                        foreach (var item in ep.Years)
                        {
                            foreach (var month in item.Months)
                            {
                                month.Inspections = new List<Inspection>();
                                month.Inspections = inspections.Where(x => x.CreatedDate.Year == Convert.ToInt16(item.Name) && x.CreatedDate.Month == month.MonthId && x.EPDetailId == ep.EPDetailId).ToList();
                            }
                        }
                        ep.EPUsers = new List<UserProfile>(); //EPUsers(ep.EPDetailId);
                        string expression = "EPDetailId = '" + ep.EPDetailId + "'";
                        string sortOrder = "FirstName ASC";
                        var foundRows = ds.Tables[1].Select(expression, sortOrder);
                        for (var i = 0; i < foundRows.Length; i++)
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

                        ep.EPFrequency = new List<EPFrequency>(); //EPUsers(ep.EPDetailId);
                        string expr = "EpDetailId = '" + ep.EPDetailId + "'";
                        string sort = "DisplayName ASC";
                        DataRow[] drRows = ds.Tables[2].Select(expr, sort);
                        for (var i = 0; i < drRows.Length; i++)
                        {
                            var epfreq = new EPFrequency
                            {
                                Frequency = new FrequencyMaster
                                {
                                    FrequencyId = Convert.ToInt32(drRows[i]["FrequencyId"]),
                                    DisplayName = Convert.ToString(drRows[i]["DisplayName"])
                                }
                            };
                            ep.EPFrequency.Add(epfreq);
                        }

                        if (!string.IsNullOrEmpty(row["IsAssetsRequired"].ToString()))
                        {
                            ep.Assets = new List<Assets>();
                            string assetExpression = "EPDetailId = '" + ep.EPDetailId + "'";
                            string AssetSortOrder = "Name ASC";
                            var drfoundRows = ds.Tables[4].Select(assetExpression, AssetSortOrder);
                            for (int i = 0; i < drfoundRows.Length; i++)
                            {
                                var asset = new Assets
                                {
                                    Name = Convert.ToString(drfoundRows[i]["Name"]),
                                    AssetId = Convert.ToInt32(drfoundRows[i]["AssetId"]),
                                    Vendors = new List<Vendors>()
                                };
                                string assetvendorExpression = "AssetId = '" + asset.AssetId + "'";
                                DataRow[] drvendorfoundRows = ds.Tables[5].Select(assetvendorExpression, AssetSortOrder); // vendor list

                                for (int j = 0; j < drvendorfoundRows.Length; j++)
                                {
                                    var vendor = new Vendors
                                    {
                                        Name = Convert.ToString(drvendorfoundRows[j]["Name"]),
                                        VendorId = Convert.ToInt32(drvendorfoundRows[j]["VendorId"])
                                    };
                                    asset.Vendors.Add(vendor);
                                }
                                ep.Assets.Add(asset);
                            }
                        }

                    }
                }
                return lists;
            }
        }


        public List<InspectionReport> GetInspectionDetailReports(string dataSortOrder, string orderBySort, int year, int userId, int currentUserId, int? categoryId = null)
        {
            List<InspectionReport> lists = new List<InspectionReport>();
            const string sql = StoredProcedures.Get_InspectionDetailReports;
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
                    var inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[3]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var ep = new InspectionReport
                        {




                            EPNumber = row["EPNumber"].ToString(),
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
                                }
                            },
                            IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString()),
                            IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString()),

                            Years = GetYears(year).OrderByDescending(x => x.YearId).ToList(),
                        };
                        ep.StandardEps = new StandardEps();
                        ep.StandardEps.Description = row["Description"].ToString();
                        ep.StandardEps.CategoryId = Convert.ToInt32(row["CategoryId"].ToString());
                        ep.StandardEps.StandardEP = row["StandardEP"].ToString();

                        lists.Add(ep);
                        foreach (var item in ep.Years)
                        {
                            foreach (var month in item.Months)
                            {
                                month.Inspections = new List<Inspection>();
                                month.Inspections = inspections.Where(x => x.CreatedDate.Year == Convert.ToInt16(item.Name) && x.CreatedDate.Month == month.MonthId && x.EPDetailId == ep.EPDetailId)
                                    .ToList();
                            }
                        }
                        ep.EPUsers = new List<UserProfile>(); //EPUsers(ep.EPDetailId);
                        string expression = "EPDetailId = '" + ep.EPDetailId + "'";
                        string sortOrder = "FirstName ASC";
                        DataRow[] foundRows = ds.Tables[1].Select(expression, sortOrder);
                        for (var i = 0; i < foundRows.Length; i++)
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

                        ep.EPFrequency = new List<EPFrequency>(); //EPUsers(ep.EPDetailId);
                        string expr = "EpDetailId = '" + ep.EPDetailId + "'";
                        string sort = "DisplayName ASC";
                        DataRow[] drRows = ds.Tables[2].Select(expr, sort);
                        for (var i = 0; i < drRows.Length; i++)
                        {
                            var epfreq = new EPFrequency
                            {
                                Frequency = new FrequencyMaster
                                {
                                    FrequencyId = Convert.ToInt32(drRows[i]["FrequencyId"]),
                                    DisplayName = Convert.ToString(drRows[i]["DisplayName"]),
                                    Days = Convert.ToInt32(drRows[i]["Days"])
                                }
                            };
                            ep.EPFrequency.Add(epfreq);
                        }

                        if (!string.IsNullOrEmpty(row["IsAssetsRequired"].ToString()))
                        {
                            ep.Assets = new List<Assets>();
                            string assetExpression = "EPDetailId = '" + ep.EPDetailId + "'";
                            string assetSortOrder = "Name ASC";
                            var drfoundRows = ds.Tables[4].Select(assetExpression, assetSortOrder);
                            for (var i = 0; i < drfoundRows.Length; i++)
                            {
                                var asset = new Assets
                                {
                                    Name = Convert.ToString(drfoundRows[i]["Name"]),
                                    AssetId = Convert.ToInt32(drfoundRows[i]["AssetId"]),
                                    Vendors = new List<Vendors>()
                                };
                                string assetvendorExpression = "AssetId = '" + asset.AssetId + "'";
                                DataRow[] drvendorfoundRows = ds.Tables[5].Select(assetvendorExpression, assetSortOrder); // vendor list

                                for (var j = 0; j < drvendorfoundRows.Length; j++)
                                {
                                    var vendor = new Vendors
                                    {
                                        Name = Convert.ToString(drvendorfoundRows[j]["Name"]),
                                        VendorId = Convert.ToInt32(drvendorfoundRows[j]["VendorId"])
                                    };
                                    asset.Vendors.Add(vendor);
                                }
                                ep.Assets.Add(asset);
                            }
                        }

                    }
                }
                return lists;
            }
        }

        private IEnumerable<Year> GetYears(int year)
        {
            var years = new List<Year>();
            //var selectedyear = DateTime.Now.Year;
            //if (!string.IsNullOrEmpty(year))
            //    selectedyear = Convert.ToInt32(year);
            var yr = new Year
            {
                Name = year.ToString(),
                YearId = 1,
                Months = GetMonths()
            };
            years.Add(yr);

            //   var dt = new DateTime();

            // int cyear = dt.Year;
            //for (var i = 0; i < 3; i++)
            //{
            //    var yr = new Year
            //    {
            //        Name = DateTime.Now.AddYears(-i).Year.ToString(),
            //        YearId = i + 1,
            //        Months = GetMonths()
            //    };
            //    years.Add(yr);
            //}
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="ascIds"></param>
        /// <returns></returns>
        public List<TFloorAssets> GetFloorAssetsForInspections(string assetId, string ascIds)
        {
            var lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_FloorAssetsForReports;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@AssetId", assetId);
                command.Parameters.AddWithValue("@ascIds", ascIds);
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    var activityList =
                        _inspectionActivityRepository.ConvertToInspectionActivities(dt.Tables[2]);
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        var list = _floorAssetRepository.BindFloorAsset(row);
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

                        var expression = "AssetId = '" + list.AssetId + "'";

                        var sortOrder = "name DESC";

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
                            //ep.Inspections = FloorAssetRepository.GetInspection(ep.EPDetailId, list.FloorAssetsId, list.Assets.IsStepsOnReport, dt.Tables[2]);
                            ep.TInspectionActivity = activityList.Where(x =>
                                x.FloorAssetId == list.FloorAssetsId && x.EPDetailId == ep.EPDetailId).ToList();
                            list.EPDetails.Add(ep);
                        }
                        lists.Add(list);
                    }
                }
            }
            return lists;
        }


        public List<TRounds> GetRoundReport(int locationGroupId, string from, string todate)
        {
            var lists = new List<TRounds>();
            const string sql = StoredProcedures.Get_RoundReport;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@locationGroupId", locationGroupId);
                command.Parameters.AddWithValue("@from", from);
                command.Parameters.AddWithValue("@todate", todate);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {


                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        var tr1 = new TRounds
                        {
                            CategoryName = row["CategoryName"].ToString(),
                            CompliantCount = Convert.ToDecimal(row["QuestionAnsweredCount"].ToString()),

                        };
                        lists.Add(tr1);
                    }

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var tr = new TRounds
                        {
                            Questions = row["Questions"].ToString(),
                            NumberOfOccurences = Convert.ToInt32(row["NumberOfOccurences"].ToString()),
                            CategoryName = row["CategoryName"].ToString(),
                        };
                        lists.Add(tr);
                    }

                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        var tr = new TRounds
                        {
                            WorkOrder = Convert.ToInt32(row["WorkOrder"].ToString()),
                            Status = Convert.ToInt32(row["Status"].ToString()),
                        };
                        lists.Add(tr);
                    }

                    foreach (DataRow row in ds.Tables[3].Rows)
                    {
                        var tr = new TRounds
                        {
                            Risk = row["Risk"].ToString(),
                            NumberOfOccurencesRisk = Convert.ToInt32(row["NumberOfOccurencesRisk"].ToString()),
                        };
                        lists.Add(tr);
                    }

                    foreach (DataRow row in ds.Tables[4].Rows)
                    {
                        var tr = new TRounds
                        {
                            RoundCount = Convert.ToInt32(row["RoundCount"].ToString()),
                            RoundStatus = row["Status"].ToString(),
                        };
                        lists.Add(tr);
                    }

                    foreach (DataRow row in ds.Tables[5].Rows)
                    {
                        var tr = new TRounds
                        {
                            CompliantCountBuilding = Convert.ToDecimal(row["QuestionAnsweredCountBuilding"].ToString()),
                            BuildingName = row["BuildingName"].ToString(),
                        };
                        lists.Add(tr);
                    }

                    foreach (DataRow row in ds.Tables[6].Rows)
                    {
                        var tr = new TRounds
                        {
                            Occurences = Convert.ToInt32(row["Occurences"].ToString()),
                            RoundInspStatus = row["Status"].ToString(),
                            RoundPercentage = Convert.ToInt32(row["Percentage"].ToString()),
                        };
                        lists.Add(tr);
                    }

                    foreach (DataRow row in ds.Tables[7].Rows)
                    {
                        var tr = new TRounds
                        {
                            QuestionsRisk = row["Questions"].ToString(),
                            RiskCount = Convert.ToInt32(row["NumberOfOccurences"].ToString()),
                            RiskType = row["Risk"].ToString(),
                        };
                        lists.Add(tr);
                    }
                }
                return lists;
            }
        }

        public List<TRounds> GetAssetDeficiencyData(string from, string todate)
        {
            var lists = new List<TRounds>();
            const string sql = StoredProcedures.Get_AssetDeficiencyData;
            using (var command = new SqlCommand(sql))
            {

                command.Parameters.AddWithValue("@from", from);
                command.Parameters.AddWithValue("@todate", todate);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {


                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        var tr1 = new TRounds
                        {
                            Assets = Convert.ToInt32(row["Assets"].ToString()),
                            AssetStatus = row["RiskScore"].ToString(),

                        };
                        lists.Add(tr1);
                    }

                }
                return lists;
            }
        }

        public List<InspectionReport> GetInspectionDetail(int epdetailid, int? frequencyid, int year, int month)
        {
            List<InspectionReport> lists = new List<InspectionReport>();
            const string sql = StoredProcedures.Get_InspectionDetailbyEPDetailId;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;              
                command.Parameters.AddWithValue("@epdetailid", epdetailid);
                command.Parameters.AddWithValue("@year", year);
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
                            Standard = new Standard
                            {
                                CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                TJCStandard = row["TJCStandard"].ToString(),
                                Category = new Category
                                {
                                    CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                }
                            },
                            IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString()),
                            IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString()),
                            Years = GetYears(year).OrderByDescending(x => x.YearId).ToList(),
                        };
                        ep.StandardEps = new StandardEps();
                        ep.StandardEps.Description = row["Description"].ToString();
                        ep.StandardEps.CategoryId = Convert.ToInt32(row["CategoryId"].ToString());
                        ep.StandardEps.StandardEP = row["StandardEP"].ToString();
                        lists.Add(ep);
                        foreach (var item in ep.Years)
                        {
                            foreach (var mnth in item.Months)
                            {
                                mnth.Inspections = new List<Inspection>();
                                mnth.Inspections = inspections.Where(x => x.StartDate.Year == Convert.ToInt16(item.Name) && x.StartDate.Month == mnth.MonthId && x.EPDetailId == ep.EPDetailId)
                                    .ToList();
                            }
                        }
                    }
                }
                return lists;
            }
        }

        #region CRx Auto Report 

        public HttpResponseMessage GetAutoReportdata()
        {
            HttpResponseMessage _objHttpResponseMessage = new HttpResponseMessage();
            const string sql = StoredProcedures.Get_AutoReportdata;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var floorAssets = new List<TFloorAssets>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TFloorAssets objTFloorAssets = new TFloorAssets();
                        if (!string.IsNullOrEmpty(row["AssetId"].ToString()))
                        {
                            objTFloorAssets.AssetId = Convert.ToInt32(row["AssetId"].ToString());
                            objTFloorAssets.Assets = new Assets()
                            {
                                AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                                Name = row["AssetsName"].ToString()
                            };
                        }
                        if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                        {
                            objTFloorAssets.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                            objTFloorAssets.FloorName = row["FloorName"].ToString();
                        }
                        if (!string.IsNullOrEmpty(row["BuildingId"].ToString()))
                        {
                            objTFloorAssets.BuildingID = Convert.ToInt32(row["BuildingId"].ToString());
                            objTFloorAssets.BuildingName = row["BuildingName"].ToString();
                        }
                        objTFloorAssets.SiteCode = row["Code"].ToString();
                        objTFloorAssets.SiteName = row["SiteName"].ToString();
                        if (!string.IsNullOrEmpty(row["EPDetailId"].ToString()))
                        {
                            objTFloorAssets.EpDetailId = Convert.ToInt32(row["EPDetailId"].ToString());
                            objTFloorAssets.EPDetail = _epRepository.GetEpDescription(objTFloorAssets.EpDetailId);
                        }
                        floorAssets.Add(objTFloorAssets);
                    }

                    var policiesNeedingReview = new List<EPDetails>();
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        EPDetails objepdetails = new EPDetails();
                        if (!string.IsNullOrEmpty(row["EPDetailId"].ToString()))
                        {
                            objepdetails = _inspectionRepository.GetCurrentEp(Convert.ToInt32(row["EPDetailId"].ToString()));
                        }
                        policiesNeedingReview.Add(objepdetails);
                    }

                    var otherEPsNeedingReview = new List<EPDetails>();
                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        EPDetails objepdetails = new EPDetails();
                        if (!string.IsNullOrEmpty(row["EPDetailId"].ToString()))
                        {
                            objepdetails = _inspectionRepository.GetCurrentEp(Convert.ToInt32(row["EPDetailId"].ToString()));
                        }
                        otherEPsNeedingReview.Add(objepdetails);
                    }

                    var undocumentedfiredrills = new List<TExercises>();
                    foreach (DataRow row in ds.Tables[3].Rows)
                    {
                        TExercises objTExercises = new TExercises();
                        objTExercises.TExerciseId = Convert.ToInt32(row["TExerciseId"].ToString());
                        if (!string.IsNullOrEmpty(row["QuarterId"].ToString()))
                        {
                            objTExercises.QuarterNo = Convert.ToInt32(row["QuarterNo"].ToString());
                        }
                        objTExercises.NearBy = row["NearBy"].ToString();
                        if (!string.IsNullOrEmpty(row["BuildingId"].ToString()))
                        {
                            objTExercises.BuildingId = Convert.ToInt32(row["BuildingId"].ToString());
                            objTExercises.Building = new Buildings
                            {
                                BuildingId = Convert.ToInt32(row["TExerciseId"].ToString()),
                                BuildingName = row["BuildingName"].ToString()
                            };
                        }
                        if (!string.IsNullOrEmpty(row["ShiftId"].ToString()))
                        {
                            objTExercises.Shift = new Shift
                            {
                                ShiftId = Convert.ToInt32(row["ShiftId"].ToString()),
                                Name = row["Name"].ToString()
                            };
                        }
                        //if (!string.IsNullOrEmpty(row["QuarterId"].ToString()))
                        //{
                        //    objTExercises.QuarterMaster = new QuarterMaster
                        //    {
                        //        QuarterId = Convert.ToInt32(row["QuarterId"].ToString()),
                        //        QuarterName = getQuarterName(row["QuarterNo"].ToString())
                        //    };
                        //}
                        undocumentedfiredrills.Add(objTExercises);
                    }

                    var tilsm = _sqlHelper.ConvertDataTable<TIlsm>(ds.Tables[4]);

                    var allpermits = _sqlHelper.ConvertDataTable<AllPermits>(ds.Tables[5]);
                    _objHttpResponseMessage.Result = new Result
                    {
                        TFloorAssets = floorAssets,
                        PoliciesNeedingReview = policiesNeedingReview,
                        OtherEPsNeedingReview = otherEPsNeedingReview,
                        Exercises = undocumentedfiredrills,
                        TIlsms = tilsm,
                        AllPermits = allpermits
                    };
                }
            }
            return _objHttpResponseMessage;
        }

        private string getQuarterName(string QuarterNo)
        {
            string subject = string.Empty;
            switch (QuarterNo)
            {
                case "1":
                    subject = "(Jan-Mar)";
                    break;
                case "2":
                    subject = "(Apr-Jun)";
                    break;
                case "3":
                    subject = "(Jul-Sep)";
                    break;
                case "4":
                    subject = "(Oct-Dec)";
                    break;
            }
            return subject;
        }

        #endregion

        #region Compliance Assets Tracking Reports

        public List<object> GetComplianceAssetsTrackingReports(string assetIds, string campusid, string buildingId)
        {
            List<object> datalist = new List<object>();
            List<AssetType> AssetTypelst = new List<AssetType>();
            const string sql = StoredProcedures.Get_ComplianceAssetsTrackingReports;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assetIds", assetIds);
                command.Parameters.AddWithValue("@campusid", campusid);
                command.Parameters.AddWithValue("@buildingId", buildingId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        var assestType = new AssetType
                        {
                            TypeId = Convert.ToInt32(row["TypeId"].ToString()),
                            Name = row["AssetTypeName"].ToString(),
                            AssetTypeCode = row["AssetTypeCode"].ToString(),
                        };
                        AssetTypelst.Add(assestType);
                    }
                    var result = _sqlHelper.ConvertDataTable<AssetComplianceMatrix>(ds.Tables[3]);
                    var epdetails = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[4]);
                    foreach (var item in result)
                    {
                        item.StandardEps = epdetails.FirstOrDefault(x => x.EPDetailId == item.EPdetailId);
                    }
                    datalist.Add(_sqlHelper.ConvertDataTable<Site>(ds.Tables[0]));
                    datalist.Add(_sqlHelper.ConvertDataTable<Buildings>(ds.Tables[1]));
                    datalist.Add(AssetTypelst.GroupBy(x => x.TypeId).Select(grp => grp.First()).ToList());
                    datalist.Add(_sqlHelper.ConvertDataTable<Assets>(ds.Tables[2]));
                    datalist.Add(result);

                }
            }
            return datalist;
        }

        #endregion
    }
}
