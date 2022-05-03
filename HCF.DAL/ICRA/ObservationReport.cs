using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class ObservationReport : IObservationReport
    {
        private readonly ISqlHelper _sqlHelper;

        public ObservationReport(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }


        #region ICRAObsReportCheckPoints 

        public int AddObservationReportCheckPoint(ICRAObsReportCheckPoints modelData)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_ICRAReportCheckPoint;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ICRAReportPointId", modelData.ICRAReportPointId);
                command.Parameters.AddWithValue("@CheckPoints", modelData.CheckPoints);
                command.Parameters.AddWithValue("@Description", modelData.Description);
                command.Parameters.AddWithValue("@IsActive", modelData.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public List<ICRAObsReportCheckPoints> GetReportCheckPoint()
        {
            var lists = new List<ICRAObsReportCheckPoints>();
            const string sql = StoredProcedures.Get_ICRAObsReportCheckPoints;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<ICRAObsReportCheckPoints>(ds.Tables[0]);
            }
            return lists;
        }


        #endregion

        #region TICRA Report

        public int AddTICRAReport(TICRAReports modelData)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_TICRAReports;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TicraId", modelData.TICRAId);
                command.Parameters.AddWithValue("@ReportDate", modelData.ReportDate);
                command.Parameters.AddWithValue("@ReportName", modelData.ReportName);
                command.Parameters.AddWithValue("@ObserverSign", modelData.ObserverSign);
                command.Parameters.AddWithValue("@ObserverSignDate", modelData.ObserverSignDate);
                command.Parameters.AddWithValue("@ObserverSignTime", modelData.ObserverSignTime);
                command.Parameters.AddWithValue("@ContractorSign", modelData.ContractorSign);
                command.Parameters.AddWithValue("@ContractorSignDate", modelData.ContractorSignDate);
                command.Parameters.AddWithValue("@ContractorSignTime", modelData.ContractorSignTime);
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                command.Parameters.AddWithValue("@TICRAReportId", modelData.TICRAReportId);
                command.Parameters.AddWithValue("@Comment", modelData.Comment);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int AddTICRAReportCheckPoint(TICRAReportsCheckPoints modelData)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_TICRAReportCheckPoint;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TICRAReportId", modelData.TICRAReportId);
                command.Parameters.AddWithValue("@ICRAReportPointId", modelData.ICRAReportPointId);
                command.Parameters.AddWithValue("@Status", modelData.Status);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public TIcraLog GetTICRAReport(int icraId)
        {
            TIcraLog icraLog = new TIcraLog();
            const string sql = Utility.StoredProcedures.Get_TICRAReport;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@icraId", icraId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    List<TICRAReportsCheckPoints> data = new List<TICRAReportsCheckPoints>();
                    var reports = _sqlHelper.ConvertDataTable<TICRAReports>(ds.Tables[2]);
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        TICRAReportsCheckPoints tCheckPoint = new TICRAReportsCheckPoints();
                        tCheckPoint.ICRAReportPointId = Convert.ToInt32(item["ICRAReportPointId"].ToString());
                        if (!string.IsNullOrEmpty(item["Status"].ToString()))
                        {
                            tCheckPoint.Status = Convert.ToInt32(item["Status"].ToString());
                        }
                        tCheckPoint.CheckPoints = new ICRAObsReportCheckPoints();
                        tCheckPoint.CheckPoints.CheckPoints = item["CheckPoints"].ToString();
                        tCheckPoint.CheckPoints.ICRAReportPointId = Convert.ToInt32(item["ICRAReportPointId"].ToString());
                        data.Add(tCheckPoint);
                    }
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        icraLog.TicraId = Convert.ToInt32(item["TICRAId"].ToString());
                        icraLog.ProjectCoordinator = item["ProjectCoordinator"].ToString();
                        icraLog.Location = item["Location"].ToString();
                        icraLog.PermitNo = item["PermitNo"].ToString();
                        icraLog.Contractor = item["Contractor"].ToString();

                        if (!string.IsNullOrEmpty(item["StartDate"].ToString()))
                            icraLog.StartDate = Convert.ToDateTime(item["StartDate"].ToString());


                        icraLog.ObservtionReport = new TICRAReports();
                        icraLog.ObservtionReport.TICRAId = icraLog.TicraId;
                        icraLog.ObservtionReport.CheckPoints = data;
                    }
                    icraLog.ObservtionReports = reports;
                }
            }
            return icraLog;
        }

        public bool DeleteFile(int fileId)
        {
            bool status = false;
            const string sql = Utility.StoredProcedures.Delete_ICRAFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@fileId", fileId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }



        public TIcraLog GetIcraReport(int icraId, int reportId)
        {
            TIcraLog icraLog = new TIcraLog();
            const string sql = Utility.StoredProcedures.Get_TICRAReports;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@icraId", icraId);
                command.Parameters.AddWithValue("@icraReportId", (reportId > 0) ? reportId : (object)DBNull.Value);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {

                    icraLog = _sqlHelper.ConvertDataTable<TIcraLog>(ds.Tables[0]).FirstOrDefault();
                    var reports = _sqlHelper.ConvertDataTable<TICRAReports>(ds.Tables[1]);
                    var tICRAReportsCheckPoints = _sqlHelper.ConvertDataTable<TICRAReportsCheckPoints>(ds.Tables[2]);
                    var iCRAObsReportCheckPoints = _sqlHelper.ConvertDataTable<ICRAObsReportCheckPoints>(ds.Tables[3]);

                    foreach (var item in tICRAReportsCheckPoints)
                    {
                        item.CheckPoints = iCRAObsReportCheckPoints.Where(x => x.ICRAReportPointId == item.ICRAReportPointId).FirstOrDefault();
                    }

                    foreach (var item in reports)
                    {
                        item.CheckPoints = tICRAReportsCheckPoints.Where(x => x.TICRAReportId == item.TICRAReportId).ToList();
                    }
                    icraLog.ObservtionReports = reports;
                }
            }
            return icraLog;
        }

        #endregion

        public TICRAReports GetICRAProjectObservationReport(string projectIds, string reportId = null)
        {
            TICRAReports ticrareport = new TICRAReports();
            const string sql = StoredProcedures.Get_TICRAProjectReport; // "Get_TICRAProjectReport";
            using (var command = new SqlCommand(sql))
            {
                //TO DO REPORT
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@projectIds", projectIds);
                command.Parameters.AddWithValue("@reportId", Convert.ToInt32(reportId) > 0 ? reportId : (object)DBNull.Value);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var reports = _sqlHelper.ConvertDataTable<TICRAReports>(ds.Tables[0]);
                    var projects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[1]);
                    var checkpoints = _sqlHelper.ConvertDataTable<TICRAReportsCheckPoints>(ds.Tables[2]);
                    List<UserProfile> users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[3]);
                    List<DigitalSignature> digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[4]);
                    foreach (var report in reports)
                    {
                        report.Projects = projects.Where(x => x.TIcraReportId == report.TICRAReportId).ToList();
                        report.ProjectNames = string.Join(" , ", report.Projects.Select(x => x.ProjectName).ToList());
                        report.ProjectNos = string.Join(" , ", report.Projects.Select(x => x.ProjectNumber).ToList());
                        report.ProjectIds = string.Join(",", report.Projects.Select(x => x.ProjectId).OrderBy(x => x).ToList());
                        report.ProjectLocations = string.Join(" , ", report.Projects.Select(x => x.ProjectLocation).ToList());
                        report.ProjectContractors = string.Join(" , ", report.Projects.Select(x => x.ProjectContractor).ToList());
                        report.CheckPoints = checkpoints.Where(x => x.TICRAReportId == report.TICRAReportId).ToList();

                        if (report.ContractorSignId.HasValue)
                            report.DSContractorSignId = digitalSignature.Where(x => x.DigSignatureId == report.ContractorSignId).FirstOrDefault();

                        if (report.ObserverSignId.HasValue)
                            report.DSObserverSignId = digitalSignature.Where(x => x.DigSignatureId == report.ObserverSignId).FirstOrDefault();

                        if (report.ContractorId.HasValue)
                            report.ContractorUser = users.Where(x => x.UserId == report.ContractorId).FirstOrDefault();

                        if (report.ObserverId.HasValue)
                            report.ObserverUser = users.Where(x => x.UserId == report.ObserverId).FirstOrDefault();
                        report.RelatedReports = reports;

                    }
                    //foreach (var report in reports)
                    //{
                    //    report.RelatedReports = reports;
                    //}


                    if (reportId == "0" || !reports.Any())
                    {
                        ticrareport = new TICRAReports();

                        ticrareport.ProjectIds = projectIds;
                        ticrareport.CheckPoints = checkpoints.Where(x => x.TICRAReportId == 0).ToList();
                        ticrareport.ProjectReport = reports.FirstOrDefault();
                        List<TIcraProject> projs = new List<TIcraProject>();
                        foreach (var pId in projectIds.Split(',').ToList())
                        {
                            var proj = projects.GroupBy(x => x.ProjectId).Select(grp => grp.First()).Where(x => x.ProjectId == Convert.ToInt32(pId)).ToList();
                            projs.AddRange(proj);
                        }
                        ticrareport.Projects = projs;
                        ticrareport.ProjectNames = string.Join(" , ", ticrareport.Projects.Select(x => x.ProjectName).ToList());
                        ticrareport.ProjectNos = string.Join(" , ", ticrareport.Projects.Select(x => x.ProjectNumber).ToList());
                        ticrareport.ProjectLocations = string.Join(" , ", ticrareport.Projects.Select(x => x.ProjectLocation).ToList());
                        ticrareport.ProjectContractors = string.Join(" , ", ticrareport.Projects.Select(x => x.ProjectContractor).ToList());
                        ticrareport.ContractorSignDate = ticrareport.ContractorSignDate != null ? ticrareport.ContractorSignDate : DateTime.Now;
                        ticrareport.ObserverSignDate = ticrareport.ObserverSignDate != null ? ticrareport.ObserverSignDate : DateTime.Now;
                        ticrareport.RelatedReports = reports;
                        //ticrareport = reports.FirstOrDefault();
                    }
                    else if (reportId != "0" && reportId != null)
                    {
                        ticrareport = reports.Where(x => x.TICRAReportId == Convert.ToInt32(reportId)).FirstOrDefault();
                    }
                    else
                    {
                        ticrareport = reports.FirstOrDefault();
                    }
                    //foreach (var report in reports)
                    //{
                    //    ticrareport.RelatedReports = reports;
                    //}
                }
            }
            return ticrareport;
        }
        public string GetPermitNumber(int TICRAId, string permittype)
        {
            string PermitNumber = "";
            const string sql = StoredProcedures.Get_PermitNumber; // "Get_TICRAProjectReport";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TICRAId", TICRAId);
                command.Parameters.AddWithValue("@permittype", permittype);

                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    PermitNumber = Convert.ToString(ds.Tables[0].Rows[0]["PermitNumber"]);
                }
            }
            return PermitNumber;
        }
        public int SaveProjectReport(TICRAReports modelData)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TICRAProjectReports; // "Insert_TICRAProjectReports"; //Utility.StoredProcedures.Insert_TICRAReports;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@projectIds", modelData.ProjectIds);
                command.Parameters.AddWithValue("@ReportDate", modelData.ReportDate);
                command.Parameters.AddWithValue("@ReportName", modelData.ReportName);
                command.Parameters.AddWithValue("@ObserverSign", modelData.ObserverSign);
                command.Parameters.AddWithValue("@ObserverSignDate", modelData.ObserverSignDate);
                command.Parameters.AddWithValue("@ObserverSignTime", modelData.ObserverSignTime);
                command.Parameters.AddWithValue("@ContractorSign", modelData.ContractorSign);
                command.Parameters.AddWithValue("@ContractorSignDate", modelData.ContractorSignDate);
                command.Parameters.AddWithValue("@ContractorSignTime", modelData.ContractorSignTime);
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                command.Parameters.AddWithValue("@TICRAReportId", modelData.TICRAReportId);
                command.Parameters.AddWithValue("@Comment", modelData.Comment);
                command.Parameters.AddWithValue("@TICRAId", modelData.TICRAId);

                command.Parameters.AddWithValue("@ContractorId", modelData.ContractorId);
                command.Parameters.AddWithValue("@ContractorSignId", modelData.ContractorSignId);
                command.Parameters.AddWithValue("@ObserverId", modelData.ObserverId);
                command.Parameters.AddWithValue("@ObserverSignId", modelData.ObserverSignId);
                command.Parameters.AddWithValue("@TIlsmId", modelData.TIlsmId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public bool SaveObservationReport(TICRAReports newTICRAReports)
        {
            const string sql = StoredProcedures.Save_ObservationReport;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TICRAReportId", newTICRAReports.TICRAReportId);
                command.Parameters.AddWithValue("@FileName", newTICRAReports.FileName);
                command.Parameters.AddWithValue("@ReportPath", newTICRAReports.ReportPath);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }
    }
}
