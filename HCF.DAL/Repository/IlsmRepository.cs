using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace HCF.DAL
{
    public class IlsmRepository : IIlsmRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IFloorAssetRepository _floorAssetRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IInsDetailRepository _insDetailRepository;


        public IlsmRepository(ISqlHelper sqlHelper, IFloorAssetRepository floorAssetRepository, IUsersRepository usersRepository,
            IInsDetailRepository insDetailRepository)
        {
            _floorAssetRepository = floorAssetRepository;
            _usersRepository = usersRepository;
            _insDetailRepository = insDetailRepository;
            _sqlHelper = sqlHelper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="affectedEps"></param>
        /// <returns></returns>
        public bool UpdateIlsmMatrix(AffectedEPs affectedEps)
        {
            const string sql = StoredProcedures.Update_IlsmMatrix;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StepsId", affectedEps.StepsId);
                command.Parameters.AddWithValue("@AffectedEPDetailId", affectedEps.AffectedEPDetailId);
                command.Parameters.AddWithValue("@IsActive", affectedEps.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", affectedEps.CreatedBy);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        public bool UpdateIlsmScore(Steps steps)
        {
            const string sql = StoredProcedures.Update_IlsmStep;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@isRa", steps.IsRA);
                command.Parameters.AddWithValue("@raScore", steps.RAScore);
                command.Parameters.AddWithValue("@stepsId", steps.StepsId);
                command.Parameters.AddWithValue("@CreatedBy", steps.CreatedBy);
                command.Parameters.AddWithValue("@IsIlsmLink", steps.IsIlsmLink);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        public bool AddStepWithIlLsmStep(int stepId, int ilsmStepsId)
        {
            const string sql = StoredProcedures.Insert_StepWithIlLsmStep;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@stepId", stepId);
                command.Parameters.AddWithValue("@ilsmStepsId", ilsmStepsId);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }


        public List<TIlsm> Get()
        {
            var lists = new List<TIlsm>();
            const string sql = StoredProcedures.Get_TIlsmDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var tilsm = new TIlsm();
                        ConvertToTIlsm(row, tilsm);
                        lists.Add(tilsm);
                    }
                }
            }
            return lists.OrderBy(x => x.CreatedDate).ToList();
        }

        public List<TIlsm> GetIlsm()
        {
            var lists = new List<TIlsm>();
            const string sql = StoredProcedures.Get_TIlsmDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[5]);
                    var tIlsmfloorAssets = _sqlHelper.ConvertDataTable<TIlsmfloorAssets>(ds.Tables[6]);
                    var activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);
                    var floorAssets = _floorAssetRepository.BindFloorAssets(ds.Tables[1]);
                    foreach (var act in activity)
                    {
                        act.TFloorAssets = floorAssets.FirstOrDefault(x => x.FloorAssetsId == act.FloorAssetId);
                        act.UserProfile = users.FirstOrDefault(x => x.UserId == act.CreatedBy);
                    }

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var tilsm = new TIlsm();
                        ConvertToTIlsm(row, tilsm);

                        tilsm.TIlsmfloorAssets = tIlsmfloorAssets.Where(x => x.TilsmId == tilsm.TIlsmId).GroupBy(test => test.ActivityId).Select(grp => grp.First()).ToList();

                        foreach (var item in tilsm.TIlsmfloorAssets)
                            item.TInspectionActivity = activity.FirstOrDefault(x => x.ActivityId == item.ActivityId);

                        if (tilsm.SourceInspection != null)
                            tilsm.SourceInspection.TInspectionActivity = activity;

                        lists.Add(tilsm);
                    }
                }
            }
            return lists.OrderBy(x => x.CreatedDate).ToList();
        }


        public List<TIlsm> GetIlsmAsync()
        {
            var lists = new List<TIlsm>();
            const string sql = StoredProcedures.Get_TIlsmDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null)
                    {
                        var users = _usersRepository.ConvertToUser(ds.Tables[5]);
                        var tIlsmfloorAssets = _sqlHelper.ConvertDataTable<TIlsmfloorAssets>(ds.Tables[6]);
                        var activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);
                        var floorAssets = _floorAssetRepository.BindFloorAssets(ds.Tables[1]);
                        var buildings = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[13]);
                        foreach (var act in activity)
                        {
                            act.TFloorAssets = floorAssets.FirstOrDefault(x => x.FloorAssetsId == act.FloorAssetId);
                            act.UserProfile = users.FirstOrDefault(x => x.UserId == act.CreatedBy);
                        }

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            var tilsm = new TIlsm();
                            ConvertToTIlsm(row, tilsm);

                            tilsm.TIlsmfloorAssets = tIlsmfloorAssets.Where(x => x.TilsmId == tilsm.TIlsmId).GroupBy(test => test.ActivityId).Select(grp => grp.First()).ToList();

                            tilsm.Buildings = new List<Buildings>();
                            if (!string.IsNullOrEmpty(tilsm.BuildingIds))
                            {
                                string[] buildingIds = tilsm.BuildingIds.Split(',');
                                tilsm.Buildings = buildings.Where(x => buildingIds.Contains(x.BuildingId.ToString())).ToList();
                            }

                            foreach (var item in tilsm.TIlsmfloorAssets)
                                item.TInspectionActivity = activity.FirstOrDefault(x => x.ActivityId == item.ActivityId);

                            if (tilsm.SourceInspection != null)
                                tilsm.SourceInspection.TInspectionActivity = activity;

                            lists.Add(tilsm);
                        }
                    }
                }
            }
            return lists.OrderBy(x => x.CreatedDate).ToList();
            // return lists.OrderBy(x => x.CreatedDate).ToList();
        }

        public bool IlsMlinkToWo(int tilsmId, int issueId)
        {
            const string sql = StoredProcedures.Update_ILSMlinkToWO;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tilsmId", tilsmId);
                command.Parameters.AddWithValue("@issueId", issueId);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        public bool UpdateIlsmStatus(TIlsm tilsm)
        {
            const string sql = StoredProcedures.Update_ILSMStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tilsmId", tilsm.TIlsmId);
                command.Parameters.AddWithValue("@comment", tilsm.CompletionComment);
                command.Parameters.AddWithValue("@Status", tilsm.Status);
                command.Parameters.AddWithValue("@closedBy", tilsm.ClosedBy);
                command.Parameters.AddWithValue("@reopenBy", tilsm.ReopenBy);
                command.Parameters.AddWithValue("@IsDeleted", tilsm.IsDeleted);
                command.Parameters.AddWithValue("@UserId", tilsm.DeletedBy);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        public bool SaveAdditionalTilsmEp(int epdetailId, int tilsmId, bool isActive)
        {
            const string sql = StoredProcedures.Save_AdditionalTilsmEP;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetailId", epdetailId);
                command.Parameters.AddWithValue("@tilsmId", tilsmId);
                command.Parameters.AddWithValue("@isActive", isActive);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        private void ConvertToTIlsm(DataRow row, TIlsm tilsm)
        {
            try
            {
                tilsm.IncidentId = row["IncidentId"].ToString();
                tilsm.llsmDate = Convert.ToDateTime(row["llsmDate"].ToString());
                if (!string.IsNullOrEmpty(row["TRoundId"].ToString()))
                    tilsm.TRoundId = Convert.ToInt32(row["TRoundId"].ToString());
                tilsm.Notes = row["Notes"].ToString();
                tilsm.TIlsmId = Convert.ToInt32(row["TIlsmId"].ToString());
                tilsm.Status = (HCF.BDO.Enums.ILSMStatus)Convert.ToInt32(row["Status"].ToString());
                if (!string.IsNullOrEmpty(row["InspectionId"].ToString()))
                {
                    tilsm.InspectionId = Convert.ToInt32(row["InspectionId"].ToString());
                }
                tilsm.IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString());
                tilsm.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                tilsm.CompletionComment = row["CompletionComment"].ToString();
                if (!string.IsNullOrEmpty(row["TicraId"].ToString()))
                    tilsm.TicraId = Convert.ToInt32(row["TicraId"].ToString());

                tilsm.Name = row["Name"].ToString();

                tilsm.WoCount = Convert.ToInt32(row["WoCount"].ToString());
                tilsm.ICRAsCount = Convert.ToInt32(row["ICRAsCount"].ToString());
                tilsm.DocumentCount = Convert.ToInt32(row["DocumentCount"].ToString());
                tilsm.ObservationReportCount = Convert.ToInt32(row["ObservationReportCount"].ToString());

                tilsm.BuildingIds = row["BuildingIds"].ToString();
                tilsm.FloorIds = row["FloorIds"].ToString();


                if (row.Table.Columns.Contains("FilePath"))
                    tilsm.FilePath = row["FilePath"].ToString();

                if (row.Table.Columns.Contains("FileName"))
                    tilsm.FileName = row["FileName"].ToString();

                if (row.Table.Columns.Contains("BuildingIds"))
                    tilsm.BuildingIds = row["BuildingIds"].ToString();

                if (!string.IsNullOrEmpty(row["CompletedDate"].ToString()))
                    tilsm.CompletedDate = Convert.ToDateTime(row["CompletedDate"].ToString());

                if (!string.IsNullOrEmpty(row["EPDetailId"].ToString()))
                {
                    tilsm.SourceInspection = new Inspection
                    {
                        EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                        CreatedDate = Convert.ToDateTime(row["InsCreatedDate"].ToString()),
                        EPDetails = new EPDetails
                        {
                            EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                            EPNumber = row["EPNumber"].ToString(),
                            Description = row["Description"].ToString(),
                            ScoreId = Convert.ToInt32(row["ScoreId"].ToString()),
                            Standard = new Standard()
                            {
                                StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                TJCStandard = row["TJCStandard"].ToString()
                            },
                            // Assets = AssetsRepository.GetEpAssets(Convert.ToInt32(row["EPDetailId"].ToString()))
                        }
                    };
                }
                //if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                //{
                //    tilsm.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                //    tilsm.Floor = new Floor
                //    {
                //        FloorName = row["FloorName"].ToString(),
                //        FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                //        BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString()) ? Convert.ToInt32(row["BuildingId"].ToString()) : 0,
                //        Building = new Buildings
                //        {
                //            BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString()) ? Convert.ToInt32(row["BuildingId"].ToString()) : 0,
                //            BuildingName = row["BuildingName"].ToString(),
                //            BuildingCode = row["BuildingCode"].ToString()
                //        }
                //    };
                //}

                if (row.Table.Columns.Contains("IsDeleted"))
                {
                    if (!string.IsNullOrEmpty(row["IsDeleted"].ToString()))
                        tilsm.IsDeleted = Convert.ToBoolean(row["IsDeleted"].ToString());
                }

                if (!string.IsNullOrEmpty(row["llsmTime"].ToString()))
                {
                    tilsm.llsmTime = TimeSpan.Parse(row["llsmTime"].ToString());
                    if (tilsm.llsmTime.HasValue)
                    {
                        var time = DateTime.Today.Add(tilsm.llsmTime.Value);
                        tilsm.llsmStartTime = time.ToString("hh:mm tt");
                    }
                }

                if (!string.IsNullOrEmpty(row["UserId"].ToString()))
                {
                    tilsm.Inspector = new UserProfile();
                    tilsm.Inspector.FirstName = row["FirstName"].ToString();
                    tilsm.Inspector.LastName = row["LastName"].ToString();
                    tilsm.Inspector.UserId = Convert.ToInt32(row["UserId"].ToString());
                }   
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex, "DAL- ILSMRepository" + row["IncidentId"], "ConvertToTIlsm");
            }
        }

        private List<TComment> ConvertToTcomments(DataTable dt)
        {
            var comments = _sqlHelper.ConvertDataTable<TComment>(dt);
            var userProfiles = _sqlHelper.ConvertDataTable<UserProfile>(dt);
            foreach (var item in comments)
            {
                item.AddedDateTimeSpan = Conversion.ConvertToTimeSpan(item.AddedDate);
                item.AddedByUserProfile = userProfiles.FirstOrDefault(x => x.UserId == item.AddedBy);
            }
            return comments;
        }

        public TIlsm GetIlsmInspection(int tilsmId)
        {
            var tIlsm = new TIlsm();
            List<TInspectionActivity> incidentActivity;
            const string sql = StoredProcedures.Get_TIlsmDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tilsmId", tilsmId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // trigger steps
                        var maingoals = _sqlHelper.ConvertDataTable<MainGoal>(ds.Tables[3]).GroupBy(x => x.MainGoalId).Select(x => x.First()).ToList();
                        var steps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[3]);
                        var standardeps = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[4]);
                        var tComment = ConvertToTcomments(ds.Tables[7]); //_sqlHelper.ConvertDataTable<TComment>(ds.Tables[7]);
                        var tilsmEPs = _sqlHelper.ConvertDataTable<TIlsmEP>(ds.Tables[4]);
                        var workOrder = _sqlHelper.ConvertDataTable<WorkOrder>(ds.Tables[8]);
                        var icraLogList = _sqlHelper.ConvertDataTable<TIcraLog>(ds.Tables[9]);

                        var deficienciesSteps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[10]);

                        var buildings = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[11]);
                        var floors = _sqlHelper.ConvertDataTable<Floor>(ds.Tables[12]);
                        var ticraprojects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[14]);
                        var ticrareports = _sqlHelper.ConvertDataTable<TICRAReports>(ds.Tables[15]);

                        foreach (var item in buildings)
                        {
                            item.Floor = floors.Where(x => x.BuildingId == item.BuildingId).ToList();
                        }

                        tIlsm.Buildings = buildings;


                        foreach (var report in ticrareports)
                        {
                            report.Projects = ticraprojects.Where(x => x.TIcraReportId == report.TICRAReportId).ToList();
                            report.ProjectNames = string.Join(" , ", report.Projects.Select(x => x.ProjectName).ToList());
                            report.ProjectNos = string.Join(" , ", report.Projects.Select(x => x.ProjectNumber).ToList());
                            report.ProjectIds = string.Join(",", report.Projects.Select(x => x.ProjectId).ToList());
                            report.ProjectLocations = string.Join(",", report.Projects.Select(x => x.ProjectLocation).ToList());
                            report.ProjectContractors = string.Join(",", report.Projects.Select(x => x.ProjectContractor).ToList());
                        }
                        tIlsm.TICRAReports = ticrareports;


                        if (deficienciesSteps.Count > 0)
                        {
                            tIlsm.Deficiencies = new List<MainGoal>();
                            MainGoal mainGoal = new MainGoal();
                            mainGoal.Steps = deficienciesSteps;
                            tIlsm.Deficiencies.Add(mainGoal);
                        }

                        tIlsm.ICRALists = icraLogList;
                        foreach (var item in maingoals.GroupBy(x => x.MainGoalId).Select(x => x.First()))
                        {
                            item.Steps = steps.Where(x => x.MainGoalId == item.MainGoalId).ToList();
                            foreach (var currentStep in item.Steps)
                                currentStep.StepsComments = tComment.Where(x => x.StepId == currentStep.StepsId).ToList();
                        }

                        tIlsm.WorkOrders = workOrder;
                        tIlsm.TriggerEps = standardeps;
                        tIlsm.TIlsmEP = tilsmEPs;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow row = ds.Tables[0].Rows[0];
                            ConvertToTIlsm(row, tIlsm);
                        }
                        var activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);

                        //List<TFloorAssets> floorAssets = _sqlHelper.ConvertDataTable<TFloorAssets>(ds.Tables[1]);
                        var dtfloorAssets = ds.Tables[1];
                        var floorAssets = _floorAssetRepository.BindFloorAssets(dtfloorAssets);


                        var users = _usersRepository.ConvertToUser(ds.Tables[5]); //_sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[5]);
                        foreach (var act in activity)
                        {
                            act.TInspectionDetail = new List<TInspectionDetail>();
                            act.TInspectionDetail = _insDetailRepository.GetTInspectionDetails(act.ActivityId);
                            act.TFloorAssets = floorAssets.FirstOrDefault(x => x.FloorAssetsId == act.FloorAssetId);
                            act.UserProfile = users.FirstOrDefault(x => x.UserId == act.CreatedBy);
                        }
                        tIlsm.SourceInspection.TInspectionActivity = activity;

                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            incidentActivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[2]);
                            var inspectionDetailslist = new List<TInspectionDetail>();
                            foreach (var item in incidentActivity)
                            {
                                item.TInspectionDetail = new List<TInspectionDetail>();
                                inspectionDetailslist = _insDetailRepository.GetTInspectionDetails(item.ActivityId);//InsDetailRepository.GetTInspectionDetails(item.ActivityId);
                            }

                            if (maingoals.Count > 0 && incidentActivity.Any())
                            {
                                //List<int> activityMainGaols = incidentActivity.FirstOrDefault().TInspectionDetail.Select(x => x.MainGoal).ToList().Select(x => x.MainGoalId).ToList();
                                //List<int> newMainGoals = maingoals.Select(x => x.MainGoalId).Except(activityMainGaols).ToList();

                                foreach (var item in maingoals)
                                {
                                    TInspectionDetail details;
                                    details = inspectionDetailslist.FirstOrDefault(x => x.MainGoalId == item.MainGoalId);
                                    if (details == null)
                                    {
                                        details = new TInspectionDetail
                                        {
                                            MainGoal = maingoals.FirstOrDefault(
                                                x => x.MainGoalId == item.MainGoalId)
                                        };
                                    }
                                    var newsteps = new List<InspectionSteps>();
                                    foreach (var step in item.Steps)
                                    {
                                        InspectionSteps maingoalStep;
                                        maingoalStep = details.InspectionSteps.FirstOrDefault(x => x.InspectionDetailId == details.InspectionDetailId && x.StepsId == step.StepsId);
                                        if (maingoalStep == null)
                                        {
                                            maingoalStep = new InspectionSteps
                                            {
                                                Status = -1,
                                                StepsId = step.StepsId,
                                                Steps = step
                                            };

                                        }
                                        newsteps.Add(maingoalStep);
                                    }
                                    details.InspectionSteps = newsteps;
                                    incidentActivity.FirstOrDefault()?.TInspectionDetail.Add(details);
                                }
                            }
                            tIlsm.TinspectionActivity = incidentActivity;
                        }
                        else
                        {
                            //TInspectionActivity temactivity = new TInspectionActivity();
                            tIlsm.MainGoal = maingoals;
                        }

                        foreach (var item in tIlsm.TriggerEps)
                            item.MainGoals = maingoals.Where(x => x.EPDetailId == item.EPDetailId).ToList();
                        foreach (var item in tIlsm.TIlsmEP)
                            item.StandardEp = standardeps.FirstOrDefault(x => x.EPDetailId == item.EPDetailId);
                    }
                }
            }
            return tIlsm;
        }

        public TIlsm GetIlsmDetails(int tilsmId)
        {
            TIlsm tIlsm = new TIlsm();
            List<TInspectionActivity> incidentActivity = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_TIlsmDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tilsmId", tilsmId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    List<WorkOrder> workOrder = _sqlHelper.ConvertDataTable<WorkOrder>(ds.Tables[8]);
                    List<TIcraLog> icraLogList = _sqlHelper.ConvertDataTable<TIcraLog>(ds.Tables[9]);
                    var buildings = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[11]);
                    var floors = _sqlHelper.ConvertDataTable<Floor>(ds.Tables[12]);
                    var ticraprojects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[14]);
                    var ticrareports = _sqlHelper.ConvertDataTable<TICRAReports>(ds.Tables[15]);
                    foreach (var item in buildings)
                    {
                        item.Floor = floors.Where(x => x.BuildingId == item.BuildingId).ToList();
                    }
                    tIlsm.Buildings = buildings;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        ConvertToTIlsm(row, tIlsm);
                        List<TInspectionActivity> activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);
                        DataTable dtfloorAssets = ds.Tables[1];
                        List<TFloorAssets> floorAssets = _floorAssetRepository.BindFloorAssets(dtfloorAssets);
                        // List<UserProfile> users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[5]);
                        foreach (TInspectionActivity act in activity)
                        {
                            act.TInspectionDetail = new List<TInspectionDetail>();
                            act.TFloorAssets = floorAssets.FirstOrDefault(x => x.FloorAssetsId == act.FloorAssetId);
                        }
                        tIlsm.SourceInspection.TInspectionActivity = activity;
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            incidentActivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[2]);
                        }
                        tIlsm.TinspectionActivity = incidentActivity;
                        tIlsm.WorkOrders = workOrder;
                        tIlsm.ICRALists = icraLogList;
                        foreach (var report in ticrareports)
                        {
                            report.Projects = ticraprojects.Where(x => x.TIcraReportId == report.TICRAReportId).ToList();
                            report.ProjectNames = string.Join(" , ", report.Projects.Select(x => x.ProjectName).ToList());
                            report.ProjectNos = string.Join(" , ", report.Projects.Select(x => x.ProjectNumber).ToList());
                            report.ProjectIds = string.Join(",", report.Projects.Select(x => x.ProjectId).ToList());
                            report.ProjectLocations = string.Join(",", report.Projects.Select(x => x.ProjectLocation).ToList());
                            report.ProjectContractors = string.Join(",", report.Projects.Select(x => x.ProjectContractor).ToList());
                        }
                        tIlsm.TICRAReports = ticrareports;
                    }
                }
            }
            return tIlsm;
        }

        public string GetIlsmwoDescription(int tilsmId)
        {
            string description = "";
            const string sql = StoredProcedures.Get_ILMSWorkOrderDescriptions;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tilsmId", tilsmId);
                DataTable dt = _sqlHelper.GetDataTable(command);
                if (dt != null && dt.Rows.Count > 0)
                {
                    description = dt.Rows[0][0].ToString();
                }
            }
            return description;
        }

        public bool SaveTilsmReport(TIlsm newTilsm)
        {
            const string sql = StoredProcedures.Save_TilsmReport;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TIlsmId", newTilsm.TIlsmId);
                command.Parameters.AddWithValue("@FileName", newTilsm.FileName);
                command.Parameters.AddWithValue("@FilePath", newTilsm.FilePath);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        public string CompleteIlsmIncident(int tilsId)
        {
            string message = string.Empty;
            const string sql = StoredProcedures.Update_CompleteIlsmIncident;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tilsId", tilsId);
                DataTable dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    message = dt.Rows[0][0].ToString();
            }
            return message;
        }

        public TIlsm GetCurrentTilsm(Guid activityId)
        {
            TIlsm lists = new TIlsm();
            const string sql = StoredProcedures.Get_ILSMByAssetFLoor;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lists = _sqlHelper.ConvertDataTable<TIlsm>(ds.Tables[0]).FirstOrDefault();
                        var data = _sqlHelper.ConvertDataTable<TIlsmfloorAssets>(ds.Tables[1]);
                        if (lists != null)
                        {
                            lists.TIlsmfloorAssets = data;
                        }
                    }

                }
            }
            return lists;
        }

        public List<TIlsm> GetAllIlsm(DateTime? fromdate = null, DateTime? todate = null, int? tilsmId = null)
        {
            //if (string.IsNullOrEmpty(Convert.ToString(fromdate)) && string.IsNullOrEmpty(Convert.ToString(todate)))
            //{
            //    fromdate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToString());
            //    todate = Convert.ToDateTime(DateTime.Now.ToString());
            //}
            List<TIlsm> lists = new List<TIlsm>();
            const string sql = StoredProcedures.Get_TIlsmDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@fromdate", fromdate);
                command.Parameters.AddWithValue("@todate", todate);
                command.Parameters.AddWithValue("@tilsmId", tilsmId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<TIlsm>(ds.Tables[0]);
            }
            return lists;
        }

        public bool IcraLinkToIslm(int icraId, int lsmId, int createdBy)
        {
            const string sql = StoredProcedures.Insert_MappingIcraILsm;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TILsmId", lsmId);
                command.Parameters.AddWithValue("@TIcraId", icraId);
                command.Parameters.AddWithValue("@CreatedBy", createdBy);
                int i = Convert.ToInt32(_sqlHelper.ExecuteNonQuery(command));
                return i > 0 ? true : false;
            }
        }


        #region Tilsm 

        public int InsertTilsm(TIlsm tilsm)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Tilsm;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectionId", tilsm.InspectionId);
                command.Parameters.AddWithValue("@Status", tilsm.Status);
                command.Parameters.AddWithValue("@Name", tilsm.Name);
                command.Parameters.AddWithValue("@IncidentId", tilsm.IncidentId);
                command.Parameters.AddWithValue("@floorId", tilsm.FloorId);
                command.Parameters.AddWithValue("@AssetId", tilsm.AssetId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }
        public int UpdateIlsm(TIlsm objtilsm)
        {
            int newId;
            const string sql = StoredProcedures.Update_Tilsm;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TIlsmId", objtilsm.TIlsmId);
                command.Parameters.AddWithValue("@Notes", objtilsm.Notes);
                command.Parameters.AddWithValue("@Name", objtilsm.Name);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }
        public DataTable InsertConstructionTilsm(TIlsm ilsm, string epDetailIds)
        {
            DataTable dtnew;
            const string sql = StoredProcedures.Insert_ConstructionTilsm;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Status", ilsm.Status);
                command.Parameters.AddWithValue("@llsmTime", ilsm.llsmTime);
                command.Parameters.AddWithValue("@llsmDate", ilsm.llsmDate);
                command.Parameters.AddWithValue("@IncidentId", ilsm.IncidentId);
                command.Parameters.AddWithValue("@Notes", ilsm.Notes);
                command.Parameters.AddWithValue("@floorId", ilsm.FloorId);
                command.Parameters.AddWithValue("@TicraId", ilsm.TicraId);
                command.Parameters.AddWithValue("@ConstIlsmStepId", ilsm.ConstIlsmStepId);
                command.Parameters.AddWithValue("@epdetailIds", epDetailIds);
                command.Parameters.AddWithValue("@Name", ilsm.Name);
                command.Parameters.AddWithValue("@IssueId", ilsm.IssueId);
                command.Parameters.AddWithValue("@CreatedBy", ilsm.CreatedBy);
                command.Parameters.AddWithValue("@BuildingIds", ilsm.BuildingIds);
                command.Parameters.AddWithValue("@FloorIds", ilsm.FloorIds);
                command.Parameters.AddWithValue("@TRoundId", ilsm.TRoundId);
                var ds = _sqlHelper.ExecuteNonQueryReturnDS(command);
                dtnew = ds.Tables[0];
            }
            return dtnew;
        }

        #endregion


        #region ILSM link to Observation Report

        public bool ILSMlinkToObservationReport(string tilsmId, string ticraReportIds)
        {
            const string sql = StoredProcedures.Update_ILSMlinkToObservationReport;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tilsmId", tilsmId);
                command.Parameters.AddWithValue("@ticraReportIds", ticraReportIds);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        #endregion

    }
}
