using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public  class ConstructionRepository : IConstructionRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IUsersRepository _usersRepository;
        private readonly ICommonRepository _commonRepository;
        public ConstructionRepository(ISqlHelper sqlHelper, IUsersRepository usersRepository, ICommonRepository commonRepository)
        {
            _usersRepository = usersRepository;
            _sqlHelper = sqlHelper;
            _commonRepository = commonRepository;
        }


        #region Construction Type AND Construction Activity
        public  IEnumerable<ConstructionType> GetConstructionType()
        {
            List<ConstructionType> constructionTypes = new List<ConstructionType>();
            List<ConstructionActivity> ConstructionActivity = new List<ConstructionActivity>();
            const string sql = StoredProcedures.Get_ConstructionType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = _sqlHelper.GetCommonDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            constructionTypes = _sqlHelper.ConvertDataTable<ConstructionType>(ds.Tables[0]);
                        }
                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            ConstructionActivity = _sqlHelper.ConvertDataTable<ConstructionActivity>(ds.Tables[1]);
                        }
                        foreach (ConstructionType item in constructionTypes)
                            item.ConstructionActivity = ConstructionActivity.Where(x => x.ConstructionTypeId == item.ConstructionTypeId).ToList();
                    }
                }

            }
            return constructionTypes;
        }

        public  ConstructionType GetConstructionType(int? constructionTypeId)
        {
            ConstructionType constructionTypes = new ConstructionType();
            const string sql = StoredProcedures.Get_ConstructionType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@constructiontypeid", constructionTypeId);
                using (DataSet ds = _sqlHelper.GetCommonDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            constructionTypes = _sqlHelper.ConvertDataTable<ConstructionType>(ds.Tables[0]).FirstOrDefault();
                        }
                        if (constructionTypes != null)
                            constructionTypes.ConstructionActivity = _sqlHelper.ConvertDataTable<ConstructionActivity>(ds.Tables[1]);
                    }
                }
            }
            return constructionTypes;
        }



        public  ConstructionActivity GetConstructionActivity(int? constActivityId)
        {
            ConstructionActivity objConstructionActivity = new ConstructionActivity();
            const string sql = StoredProcedures.Get_ConstructionType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = _sqlHelper.GetCommonDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            objConstructionActivity = _sqlHelper.ConvertDataTable<ConstructionActivity>(ds.Tables[1]).FirstOrDefault(x => x.ConstActivityId == constActivityId);
                        }
                    }
                }
            }
            return objConstructionActivity;
        }

        public  int AddConstructionType(ConstructionType modelData)
        {
            int newId;
            const string sql = StoredProcedures.Insert_ConstructionType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ConstructionTypeId", modelData.ConstructionTypeId);
                command.Parameters.AddWithValue("@TypeName", modelData.TypeName);
                command.Parameters.AddWithValue("@Description", modelData.Description);
                command.Parameters.AddWithValue("@IsActive", modelData.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int AddConstuctionActivity(ConstructionActivity modelData)
        {
            int newId;
            const string sql = StoredProcedures.Insert_ConstructionActivity;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ConstActivityId", modelData.ConstActivityId);
                command.Parameters.AddWithValue("@ConstructionTypeId", modelData.ConstructionTypeId);
                command.Parameters.AddWithValue("@Activity", modelData.Activity);
                command.Parameters.AddWithValue("@IsActive", modelData.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        #endregion

        #region Construction Risk 
        public  List<ConstructionRisk> GetConstructionRisk()
        {
            List<ConstructionRisk> constructionRisk = new List<ConstructionRisk>();
            const string sql = StoredProcedures.Get_ConstructionRisk;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        constructionRisk = _sqlHelper.ConvertDataTable<ConstructionRisk>(ds.Tables[0]);
                    }
                }
                if (constructionRisk != null)
                {
                    foreach (var item in constructionRisk)
                    {
                        DataRow[] result = ds.Tables[1].Select("ConstructionRiskId=" + item.ConstructionRiskId);//.CopyToDataTable();
                        if (result.Any())
                        {
                            var riskAreas = _sqlHelper.ConvertDataTable<ICRARiskArea>(result.CopyToDataTable());
                            item.RiskArea = riskAreas;
                        }
                    }
                }
            }
            return constructionRisk;
        }

        public  int SaveAreaSurrounding(TICRAAreasNearBy tICRAAreasNearBy)
        {
            int newId;
            const string sql = StoredProcedures.Insert_AreaSurrounding;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Comment", tICRAAreasNearBy.Comment);
                command.Parameters.AddWithValue("@AreasSurroundingId", tICRAAreasNearBy.AreasSurroundingId);
                command.Parameters.AddWithValue("@RiskAreaIds", tICRAAreasNearBy.RiskAreaIds);
                command.Parameters.AddWithValue("@ConstructionRiskId", tICRAAreasNearBy.ConstructionRiskId);
                command.Parameters.AddWithValue("@TicraId", tICRAAreasNearBy.TicraId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  ConstructionRisk GetConstructionRisk(int? constructionriskid)
        {
            ConstructionRisk constructionrisk = new ConstructionRisk();
            const string sql = StoredProcedures.Get_ConstructionRisk;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ConstructionRiskId", constructionriskid);
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            constructionrisk = _sqlHelper.ConvertDataTable<ConstructionRisk>(ds.Tables[0]).FirstOrDefault();
                        }
                        if (constructionrisk != null)
                        {
                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                List<ICRARiskArea> riskAreas = _sqlHelper.ConvertDataTable<ICRARiskArea>(ds.Tables[1]);
                                constructionrisk.RiskArea = riskAreas;
                            }
                        }
                    }
                }
            }
            return constructionrisk;
        }

        public  int AddConstructionRisk(ConstructionRisk modelData)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_ConstructionRisk;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ConstructionRiskId", modelData.ConstructionRiskId);
                command.Parameters.AddWithValue("@RiskName", modelData.RiskName);
                command.Parameters.AddWithValue("@GroupName", modelData.GroupName);
                command.Parameters.AddWithValue("@IsActive", modelData.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int AddRiskAreaToArea(int constructionRiskId, string riskAreaIds, int createdBy)
        {

            const string sql = StoredProcedures.Insert_AddRiskAreaToArea;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@constructionRiskId", constructionRiskId);
                command.Parameters.AddWithValue("@riskAreaIds", riskAreaIds);
                command.Parameters.AddWithValue("@CreatedBy", createdBy);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return constructionRiskId;
        }

        public  bool DeleteRiskAreaToArea(int riskAreaIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_ConstRiskDeptMap;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@riskAreaId", riskAreaIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }



        public  List<ConstRiskDeptMap> GetConstRiskDeptMap()
        {
            List<ConstRiskDeptMap> constRiskDeptMaps = new List<ConstRiskDeptMap>();
            const string sql = StoredProcedures.Get_ConstRiskDeptMap;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            constRiskDeptMaps = _sqlHelper.ConvertDataTable<ConstRiskDeptMap>(ds.Tables[0]);
                        }
                    }
                }
            }
            return constRiskDeptMaps;
        }


        #endregion

        #region Construction Class

        public  List<ConstructionClass> GetConstructionClass()
        {
            List<ConstructionClass> constructionClass = new List<ConstructionClass>();
            List<ConstructionClassActivity> constructionClassActivity = new List<ConstructionClassActivity>();
            const string sql = Utility.StoredProcedures.Get_ConstructionClass;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        constructionClass = _sqlHelper.ConvertDataTable<ConstructionClass>(ds.Tables[0]);
                    }
                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        constructionClassActivity = _sqlHelper.ConvertDataTable<ConstructionClassActivity>(ds.Tables[1]);
                    }
                    foreach (ConstructionClass item in constructionClass)
                    {
                        item.ConstructionClassActivity = constructionClassActivity.Where(x => x.ConstClassId == item.ConstructionClassId).ToList();
                    }
                }
            }
            return constructionClass;
        }

        public  ConstructionClass GetConstructionClass(int? constructionclassid)
        {
            ConstructionClass constructionClass = new ConstructionClass();
            const string sql = StoredProcedures.Get_ConstructionClass;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ConstructionClassId", constructionclassid);
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    constructionClass = _sqlHelper.ConvertDataTable<ConstructionClass>(ds.Tables[0]).FirstOrDefault();
                    if (constructionClass != null)
                        constructionClass.ConstructionClassActivity = _sqlHelper.ConvertDataTable<ConstructionClassActivity>(ds.Tables[1]);
                }
            }
            return constructionClass;
        }

        public  int AddConstructionClass(ConstructionClass modelData)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_ConstructionClass;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ConstructionClassId", modelData.ConstructionClassId);
                command.Parameters.AddWithValue("@ClassName", modelData.ClassName);
                command.Parameters.AddWithValue("@IsActive", modelData.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                command.Parameters.AddWithValue("@Version", modelData.Version);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public  int AddConstClassActivity(ConstructionClassActivity modelData)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_ConstructionClassActivity;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ConstClassActivityId", modelData.ConstClassActivityId);
                command.Parameters.AddWithValue("@ConstClassCatId", modelData.ConstClassCatId);
                command.Parameters.AddWithValue("@ConstClassId", modelData.ConstClassId);
                command.Parameters.AddWithValue("@Activity", modelData.Activity);
                command.Parameters.AddWithValue("@IsActive", modelData.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public  List<ConstructionClassCategrory> GetConstructionClassCategory()
        {
            List<ConstructionClassCategrory> objConstructionClassCategrory = new List<ConstructionClassCategrory>();
            ConstructionClassCategrory obj1 = new ConstructionClassCategrory
            {
                ConstClassCatId = 1,
                Categrory = "During Construction Project"
            };
            ConstructionClassCategrory obj2 = new ConstructionClassCategrory
            {
                ConstClassCatId = 2,
                Categrory = "Upon Completion of Project"
            };
            objConstructionClassCategrory.Add(obj1);
            objConstructionClassCategrory.Add(obj2);
            return objConstructionClassCategrory;
        }

        #endregion

        #region ICRASteps

        public  List<ICRASteps> GetICRASteps()
        {
            List<ICRASteps> objICRASteps = new List<ICRASteps>();
            const string sql = Utility.StoredProcedures.Get_ICRASteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    objICRASteps = _sqlHelper.ConvertDataTable<ICRASteps>(ds.Tables[0]);
                }
            }
            return objICRASteps;
        }

        public  ICRASteps GetICRASteps(int icrastepId)
        {
            ICRASteps objICRASteps = new ICRASteps();
            const string sql = Utility.StoredProcedures.Get_ICRASteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@icrastepId", icrastepId);
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    objICRASteps = _sqlHelper.ConvertDataTable<ICRASteps>(ds.Tables[0]).FirstOrDefault();
                }
            }
            return objICRASteps;
        }

        public  int AddICRASteps(ICRASteps modelData)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_ICRASteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ICRAStepId", modelData.ICRAStepId);
                command.Parameters.AddWithValue("@Steps", modelData.Steps);
                command.Parameters.AddWithValue("@Alias", modelData.Alias);
                command.Parameters.AddWithValue("@IsActive", modelData.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        #endregion

        #region ICRA Inspection
        public  List<TIcraLog> GetAllInspectionIcraSteps(int icraId, DateTime? fromdate = null, DateTime? todate = null, int? statusId = null, int? constructionrikId = null, int? version = null)
        {
            List<TIcraLog> objTIcraLog = new List<TIcraLog>();
            List<TICRAFiles> objTICRAFiles = new List<TICRAFiles>();
            const string sql = StoredProcedures.Get_InspectionIcraSteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@icraId", icraId);
                command.Parameters.AddWithValue("@fromdate", fromdate!=null && fromdate.ToString() == "1/1/0001 12:00:00 AM"?null: fromdate);
                command.Parameters.AddWithValue("@todate", todate != null && todate.ToString() == "1/1/0001 12:00:00 AM" ? null : todate);
                command.Parameters.AddWithValue("@statusId", statusId);
                command.Parameters.AddWithValue("@constructionrikId", constructionrikId);
                command.Parameters.AddWithValue("@version", version);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if(ds.Tables[0]!=null && ds.Tables[0].Rows.Count>0)
                    {
                        objTIcraLog = _sqlHelper.ConvertDataTable<TIcraLog>(ds.Tables[0]);
                    }
                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        objTICRAFiles = ConvertdtTICRAFiles(ds.Tables[1]);//.Where(x => x.IsReport = false).ToList(); //_sqlHelper.ConvertDataTable<TICRAFiles>(ds.Tables[1]);
                    }
                    
                    foreach (var item in objTIcraLog)
                    {
                        if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                        {
                            item.Project = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[2]).FirstOrDefault(x => x.ProjectId == item.ProjectId);
                        }
                        
                        item.TICRAFiles = objTICRAFiles.Where(x => x.TicraId == item.TicraId).ToList();
                        if (item.ConstructionTypeId > 0)
                            item.ConstructionType = GetConstructionType(item.ConstructionTypeId);
                        if (item.ConstructionClassId > 0)
                            item.ConstructionClass = GetConstructionClass(item.ConstructionClassId);
                        if (item.ConstructionRiskId > 0)
                            item.ConstructionRisk = GetConstructionRisk(item.ConstructionRiskId);
                    }
                }
            }
            return objTIcraLog;
        }
        public  AllPermitReport GetAllPermitCount(string from, string todate,string  permittype)
        {
            AllPermitReport obj = new AllPermitReport();
            List<PermitCountDetail> objICRAReport = new List<PermitCountDetail>();
            const string sql = StoredProcedures.GetPermitsCount;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@fromdate", from);
                command.Parameters.AddWithValue("@todate", todate);
                command.Parameters.AddWithValue("@PermitType", permittype);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {

                    obj.PermitCountDetail = _sqlHelper.ConvertDataTable<PermitCountDetail>(ds.Tables[0]);
                    var allpermitdata = _sqlHelper.ConvertDataTable<AllPermitReport>(ds.Tables[1]);
                    foreach (var data in allpermitdata)
                    {
                        obj.AllPermitCount = data.AllPermitCount;
                        obj.ProjectCount = data.ProjectCount;
                    }
                }
            }
            return obj;
        }
        public  List<TICRAFiles> ConvertdtTICRAFiles(DataTable dataTable)
        {
            List<TICRAFiles> Files = new List<TICRAFiles>();
            foreach (DataRow item in dataTable.Rows)
            {
                TICRAFiles file = new TICRAFiles
                {
                    TicraId = Convert.ToInt32(item["TicraId"].ToString()),
                    Comment = Convert.ToString(item["Comment"].ToString()),
                    IsReport = Convert.ToBoolean(item["IsReport"].ToString()),
                    IsDeleted = Convert.ToBoolean(item["IsDeleted"].ToString()),
                    FileName = Convert.ToString(item["FileName"].ToString()),
                    FilePath = Convert.ToString(item["FilePath"].ToString()),
                    TICRAFileId = Convert.ToInt32(item["TICRAFileId"].ToString())
                };
                if (!string.IsNullOrEmpty(item["UploadedBy"].ToString()))
                    file.UploadedBy = Convert.ToInt32(item["UploadedBy"].ToString());
                Files.Add(file);
            }
            return Files;
        }

        public  TIcraLog GetInspectionIcraSteps(int? icraId)
        {
            TIcraLog objTIcraLog = new TIcraLog();
            List<TIcraSteps> objTIcraSteps = new List<TIcraSteps>();
            const string sql = StoredProcedures.Get_InspectionIcraSteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@icraId", icraId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    var users = _usersRepository.GetUsersList().ToList();
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        objTIcraLog = _sqlHelper.ConvertDataTable<TIcraLog>(ds.Tables[0]).FirstOrDefault();
                    }
                    List<TICRAFiles> ticrafiles = new List<TICRAFiles>();
                    if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {
                     ticrafiles = _sqlHelper.ConvertDataTable<TICRAFiles>(ds.Tables[2]);
                    }
                    List<DigitalSignature> digitalSignature = new List<DigitalSignature>();
                    if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                    {
                        digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[4]);
                    }
                    List<TPermitWorkFlowDetails> TPermitWorkFlowDetails = new List<TPermitWorkFlowDetails>();
                    if (ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
                    {
                         TPermitWorkFlowDetails = _sqlHelper.ConvertDataTable<TPermitWorkFlowDetails>(ds.Tables[6]);
                    }
                    List<PermitSetting> permitsettings = new List<PermitSetting>();
                    if (ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
                    {
                        permitsettings = _sqlHelper.ConvertDataTable<PermitSetting>(ds.Tables[7]);
                    }
                    
                    Random generator = new Random();
                    if (objTIcraLog != null && objTIcraLog.TicraId==0)
                    {
                        objTIcraLog = new TIcraLog();
                        objTIcraLog.Status = -1;
                        List<TIcraLog> lstticralog = GetAllInspectionIcraSteps(-1);
                        if (lstticralog.Count > 0)
                        {
                            int tempprojectno = Convert.ToInt32(lstticralog.OrderByDescending(x => x.TicraId).Select(x => x.PermitNo).FirstOrDefault());
                            objTIcraLog.PermitNo = (tempprojectno + 1).ToString();
                        }
                        else
                        {
                            objTIcraLog.PermitNo = generator.Next(0, 999999).ToString("D6");
                        }
                    }

                    if (objTIcraLog.PermitRequestBy > 0)
                        objTIcraLog.PermitRequestByUser = users.Where(x => x.UserId == objTIcraLog.PermitRequestBy).FirstOrDefault();

                    if (objTIcraLog.PermitAuthorizedBy.HasValue)
                        objTIcraLog.PermitAuthorizedByUser = users.Where(x => x.UserId == objTIcraLog.PermitAuthorizedBy).FirstOrDefault();

                    if (objTIcraLog.PermitAuthorizedBySignId.HasValue)
                        objTIcraLog.DSPermitAuthorizedBy = digitalSignature.Where(x => x.DigSignatureId == objTIcraLog.PermitAuthorizedBySignId).FirstOrDefault();

                    if (objTIcraLog.PermitRequestBySignId.HasValue)
                        objTIcraLog.DSPermitRequestBy = digitalSignature.Where(x => x.DigSignatureId == objTIcraLog.PermitRequestBySignId).FirstOrDefault();

                    if (objTIcraLog.PermitReviewerBySignId.HasValue)
                        objTIcraLog.DSPermitReviewerBy = digitalSignature.Where(x => x.DigSignatureId == objTIcraLog.PermitReviewerBySignId).FirstOrDefault();

                    if (objTIcraLog.PermitReviewerBy.HasValue)
                        objTIcraLog.PermitReviewerByUser = users.Where(x => x.UserId == objTIcraLog.PermitReviewerBy).FirstOrDefault();

                    if (!string.IsNullOrEmpty(objTIcraLog.TDrawingFields))
                        objTIcraLog.DrawingAttachments = _commonRepository.GetUploadedDrawings(objTIcraLog.TDrawingFields);

                    objTIcraLog.ConstructionType = GetConstructionType(objTIcraLog.ConstructionTypeId);
                    objTIcraLog.ConstructionClass = GetConstructionClass(objTIcraLog.ConstructionClassId);
                    objTIcraLog.ConstructionRisk = GetConstructionRisk(objTIcraLog.ConstructionRiskId);
                    objTIcraLog.TIcraSteps = new List<TIcraSteps>();
                    objTIcraLog.TICRAFiles = new List<TICRAFiles>();
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        TIcraSteps list = new TIcraSteps
                        {
                            TIrcaStepId = !string.IsNullOrEmpty(item["TIrcaStepId"].ToString()) ? Convert.ToInt32(item["TIrcaStepId"].ToString()) : 0,
                            TicraId = !string.IsNullOrEmpty(item["TicraId"].ToString()) ? Convert.ToInt32(item["TicraId"].ToString()) : 0,
                            ICRAStepId = !string.IsNullOrEmpty(item["ICRAStepId"].ToString()) ? Convert.ToInt32(item["ICRAStepId"].ToString()) : 0,
                            Comment = item["Comment"].ToString()
                        };
                        if (list.ICRAStepId == 4)
                        {
                            if (!string.IsNullOrEmpty(list.Comment))
                            {
                                string[] comments = list.Comment.Split(';');
                                foreach (var comment in comments)
                                {
                                    if (comment.Contains("UnitBelow"))
                                    {
                                        objTIcraLog.UnitBelow = comment.Split(':')[1];
                                    }
                                    else if (comment.Contains("UnitAbove"))
                                    {
                                        objTIcraLog.UnitAbove = comment.Split(':')[1];
                                    }
                                    else if (comment.Contains("Lateral"))
                                    {
                                        objTIcraLog.Lateral = comment.Split(':')[1];
                                    }
                                    else if (comment.Contains("Secondl"))
                                    {
                                        objTIcraLog.Secondl = comment.Split(':')[1];
                                    }
                                    else if (comment.Contains("Behind"))
                                    {
                                        objTIcraLog.Behind = comment.Split(':')[1];
                                    }
                                    else if (comment.Contains("Front"))
                                    {
                                        objTIcraLog.Front = comment.Split(':')[1];
                                    }
                                }
                            }
                        }
                        list.Step = new ICRASteps
                        {
                            ICRAStepId = Convert.ToInt32(item["ICRAStepId"].ToString()),
                            Steps = item["Steps"].ToString(),
                            Alias = item["Alias"].ToString(),
                            IsActive = Convert.ToBoolean(item["ICRAStepsIsActive"].ToString()),
                            ParentICRAStepId = !string.IsNullOrEmpty(item["ParentICRAStepId"].ToString()) ? Convert.ToInt32(item["ParentICRAStepId"].ToString()) : 0,
                        };
                        objTIcraSteps.Add(list);
                    }
                    objTIcraLog.TIcraSteps = objTIcraSteps;
                    objTIcraLog.TICRAFiles = ticrafiles;
                    foreach (var item in ticrafiles)
                    {
                        if (!string.IsNullOrEmpty(item.UploadedBy.ToString()))
                        {
                            item.UserProfile = new UserProfile();
                            item.UserProfile = _usersRepository.GetUsersList(item.UploadedBy);
                        }
                    }
                    objTIcraLog.AreasSurroundings = new List<TICRAAreasNearBy>();
                    foreach (DataRow item in ds.Tables[3].Rows)
                    {
                        TICRAAreasNearBy newTICRAAreasNearBy = new TICRAAreasNearBy
                        {
                            TicraId = !string.IsNullOrEmpty(item["TicraId"].ToString()) ? Convert.ToInt32(item["TicraId"].ToString()) : 0,
                            RiskAreaIds = item["RiskAreaIds"].ToString(),
                            ConstructionRiskId = Convert.ToInt32(item["ConstructionRiskId"].ToString())
                        };
                        if (newTICRAAreasNearBy.ConstructionRiskId > 0)
                        {
                            newTICRAAreasNearBy.ConstructionRisk = new ConstructionRisk();
                            newTICRAAreasNearBy.ConstructionRisk = GetConstructionRisk(newTICRAAreasNearBy.ConstructionRiskId);
                        }
                        newTICRAAreasNearBy.Comment = item["Comment"].ToString();
                        newTICRAAreasNearBy.RiskAreaIdNames = item["RiskAreaIdNames"].ToString();
                        newTICRAAreasNearBy.AreasSurroundingId = Convert.ToInt32(item["AreasSurroundingId"].ToString());
                        newTICRAAreasNearBy.AreasSurrounding = (BDO.Enums.AreasSurrounding)newTICRAAreasNearBy.AreasSurroundingId;
                        objTIcraLog.AreasSurroundings.Add(newTICRAAreasNearBy);
                    }
                   
                    if (TPermitWorkFlowDetails != null && objTIcraLog != null)
                    {
                        objTIcraLog.TPermitWorkFlowDetails = TPermitWorkFlowDetails;
                        if (objTIcraLog.TPermitWorkFlowDetails != null && objTIcraLog.TPermitWorkFlowDetails.Count > 0)
                        {

                            foreach (var permitworkflow in objTIcraLog.TPermitWorkFlowDetails)
                            {
                                if (permitsettings != null && permitsettings.Count > 0)
                                {
                                    permitworkflow.PermitSetting = permitsettings.Where(x => x.Id == permitworkflow.FormSettingId).FirstOrDefault();
                                    if (permitworkflow != null && permitworkflow.PermitSetting != null)
                                    {
                                        permitworkflow.PermitSetting.Id = permitworkflow.FormSettingId;
                                    }

                                }
                                permitworkflow.DSPermitSignature = digitalSignature.Where(x => x.DigSignatureId == permitworkflow.LabelSignId).FirstOrDefault();
                                permitworkflow.UserDetail = users.Where(x => x.UserId == permitworkflow.LabelValue).FirstOrDefault();
                            }
                        }
                    }
                }

               
            }
            return objTIcraLog;
        }

        public  int AddInspectionIcra(TIcraLog modelData)
        {
            int newId;
            const string sql = StoredProcedures.Insert_InspectionIcra;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TicraId", modelData.TicraId);
                command.Parameters.AddWithValue("@ProjectId", modelData.ProjectId);
                //command.Parameters.AddWithValue("@ProjectNo", modelData.ProjectNo);
                command.Parameters.AddWithValue("@PermitNo", modelData.PermitNo);
                command.Parameters.AddWithValue("@ProjectName", modelData.ProjectName);
                command.Parameters.AddWithValue("@FloorId", modelData.FloorId);
                command.Parameters.AddWithValue("@Scope", modelData.Scope);
                command.Parameters.AddWithValue("@StartDate", modelData.StartDate);
                command.Parameters.AddWithValue("@CompletionDate", modelData.CompletionDate);
                command.Parameters.AddWithValue("@IsICRA", modelData.IsICRA);
                command.Parameters.AddWithValue("@ConstructionTypeId", modelData.ConstructionTypeId);
                command.Parameters.AddWithValue("@ConstructionRiskId", modelData.ConstructionRiskId);
                command.Parameters.AddWithValue("@ConstructionClassId", modelData.ConstructionClassId);
                command.Parameters.AddWithValue("@Comment", modelData.Comment);
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                command.Parameters.AddWithValue("@Location", modelData.Location);
                command.Parameters.AddWithValue("@ProjectCoordinator", modelData.ProjectCoordinator);
                command.Parameters.AddWithValue("@EstimatedDuration", modelData.EstimatedDuration);
                command.Parameters.AddWithValue("@Contractor", modelData.Contractor);
                command.Parameters.AddWithValue("@Supervisor", modelData.Supervisor);
                command.Parameters.AddWithValue("@Telephone", modelData.Telephone);
                command.Parameters.AddWithValue("@PermitRequestBy", modelData.PermitRequestBy);
                command.Parameters.AddWithValue("@RequestDate", modelData.RequestDate);
                command.Parameters.AddWithValue("@PermitAuthorizedBy", modelData.PermitAuthorizedBy);
                command.Parameters.AddWithValue("@AuthorizedDate", modelData.AuthorizedDate);
                command.Parameters.AddWithValue("@PermitAuthorizedBySignId", modelData.PermitAuthorizedBySignId);
                command.Parameters.AddWithValue("@PermitRequestBySignId", modelData.PermitRequestBySignId);
                command.Parameters.AddWithValue("@RiskAreaId", modelData.RiskAreaId);
                command.Parameters.AddWithValue("@Date1", modelData.Date1);
                command.Parameters.AddWithValue("@Initial1", modelData.Initial1);
                command.Parameters.AddWithValue("@Date2", modelData.Date2);
                command.Parameters.AddWithValue("@Initial2", modelData.Initial2);
                command.Parameters.AddWithValue("@ClassDate", modelData.ClassDate);
                command.Parameters.AddWithValue("@ClassInitial", modelData.ClassInitial);
                command.Parameters.AddWithValue("@Status", modelData.Status);
                command.Parameters.AddWithValue("@ClosedDate", modelData.ClosedDate);
                command.Parameters.AddWithValue("@ActivityLists", modelData.ActivityLists);
                command.Parameters.AddWithValue("@InfectionPreventionist", modelData.InfectionPreventionist);
                command.Parameters.AddWithValue("@Class3Date", modelData.Class3Date);
                command.Parameters.AddWithValue("@Class4Date", modelData.Class4Date);
                command.Parameters.AddWithValue("@Class4Initial", modelData.Class4Initial);
                command.Parameters.AddWithValue("@Class3Initial", modelData.Class3Initial);
                command.Parameters.AddWithValue("@PermitReviewerBy", modelData.PermitReviewerBy);
                command.Parameters.AddWithValue("@PermitReviewerBySignId", modelData.PermitReviewerBySignId);
                command.Parameters.AddWithValue("@ReasonRejection", modelData.ReasonRejection);
                command.Parameters.AddWithValue("@TDrawingFields", modelData.TDrawingFields);
                command.Parameters.AddWithValue("@Version", modelData.Version);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public  bool DeleteICRADrawings(int TicraId, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_ICRADrawings;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TicraId", TicraId);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        public  int AddTICRASteps(TIcraSteps modelData)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_TIcraSteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TIrcaStepId", modelData.TIrcaStepId);
                command.Parameters.AddWithValue("@TicraId", modelData.TicraId);
                command.Parameters.AddWithValue("@ICRAStepId", modelData.ICRAStepId);
                command.Parameters.AddWithValue("@Comment", modelData.Comment);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int SaveTICRAFiles(TICRAFiles modelData)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TIcraFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TICRAFileId", modelData.TICRAFileId);
                command.Parameters.AddWithValue("@TicraId", modelData.TicraId);
                command.Parameters.AddWithValue("@FileName", modelData.FileName);
                command.Parameters.AddWithValue("@FilePath", modelData.FilePath);
                command.Parameters.AddWithValue("@Comment", modelData.Comment);
                command.Parameters.AddWithValue("@UploadedBy", modelData.UploadedBy);
                command.Parameters.AddWithValue("@IsReport", modelData.IsReport);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  List<TICRAFiles> GetTICRAFiles(int icraId)
        {
            List<TICRAFiles> objicrahistory = new List<TICRAFiles>();
            const string sql = StoredProcedures.Get_IcraHistory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@icraId", icraId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    objicrahistory = ConvertdtTICRAFiles(ds.Tables[1]); 
                                                                        
                }
            }
            return objicrahistory;
        }

        public  List<Audit> GetIcraHistory(int icraId)
        {
            List<Audit> objicrahistory = new List<Audit>();
            const string sql = Utility.StoredProcedures.Get_IcraHistory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@icraId", icraId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    objicrahistory = _sqlHelper.ConvertDataTable<Audit>(ds.Tables[0]).ToList();
                }
            }
            return objicrahistory;
        }

        public  List<TIcraLog> GetIcras(int? icraId)
        {
            List<TIcraLog> logs = new List<TIcraLog>();
            const string sql = StoredProcedures.Get_Icras;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@icraId", icraId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    logs = _sqlHelper.ConvertDataTable<TIcraLog>(ds.Tables[0]);
                    var reports = _sqlHelper.ConvertDataTable<TICRAReports>(ds.Tables[1]);
                    foreach (var item in logs)
                    {
                        item.ObservtionReports = reports.Where(x => x.TICRAId == item.TicraId).ToList();
                    }
                }
            }
            return logs;
        }

        public  List<ProjectType> GetProjectType()
        {

            List<ProjectType> ProjectType = new List<ProjectType>();
            const string sql = Utility.StoredProcedures.Get_ProjectType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    ProjectType = _sqlHelper.ConvertDataTable<ProjectType>(ds.Tables[0]);
                }
            }
            return ProjectType;
        }
        public  List<TIcraLog> GetIcrasWithIlsm(string searchParam, bool IsICRA)
        {
            List<TIcraLog> logs = new List<TIcraLog>();
            const string sql = StoredProcedures.Get_ICRALogWithILSM;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@searchParam", searchParam);
                command.Parameters.AddWithValue("@IsICRA", IsICRA);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    logs = _sqlHelper.ConvertDataTable<TIcraLog>(ds.Tables[0]);
                    List<TIcraProject> projects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[0]);
                    foreach (var item in logs)
                    {
                        string expression;
                        expression = "TicraId = " + item.TicraId;
                        DataRow[] foundRows = ds.Tables[1].Select(expression);
                        if (foundRows.Any())
                        {
                            DataTable tilsmIds = foundRows.CopyToDataTable();
                            item.ILSM = _sqlHelper.ConvertDataTable<TIlsm>(tilsmIds);
                        }
                        item.Project = projects.FirstOrDefault(x => x.ProjectId == item.ProjectId);
                    }
                }
            }
            return logs;
        }

        #endregion

        #region ICRA Risk Area

        public  int AddICRARiskArea(ICRARiskArea modelData)
        {
            int newId;
            const string sql = StoredProcedures.Insert_ICRARiskArea;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                command.Parameters.AddWithValue("@Name", modelData.Name);
                command.Parameters.AddWithValue("@IsActive", modelData.IsActive);
                command.Parameters.AddWithValue("@ApprovalStatus", modelData.ApprovalStatus);
                command.Parameters.AddWithValue("@IsSendEmail", modelData.IsSendEmail);
                command.Parameters.AddWithValue("@FireRiskType", modelData.FireRiskType);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  bool UpdateICRARiskArea(ICRARiskArea modelData)
        {
            bool status = true;
            const string sql = StoredProcedures.Update_ICRARiskArea;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ALTERdBy", modelData.CreatedBy);
                command.Parameters.AddWithValue("@RiskAreaId", modelData.RiskAreaId);
                command.Parameters.AddWithValue("@Name", modelData.Name);
                command.Parameters.AddWithValue("@IsActive", modelData.IsActive);
                command.Parameters.AddWithValue("@ApprovalStatus", modelData.ApprovalStatus);
                command.Parameters.AddWithValue("@IsSendEmail", modelData.IsSendEmail);
                command.Parameters.AddWithValue("@FireRiskType", modelData.FireRiskType);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public  List<ICRARiskArea> GetICRARiskArea()
        {
            List<ICRARiskArea> iCRARiskArea = new List<ICRARiskArea>();
            const string sql = StoredProcedures.Get_ICRARiskArea;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    iCRARiskArea = _sqlHelper.ConvertDataTable<ICRARiskArea>(ds.Tables[0]);
                    //List<ConstructionRisk> constructionRisk = _sqlHelper.ConvertDataTable<ConstructionRisk>(ds.Tables[1]);
                    List<ICRARiskArea> icraRiskArea = _sqlHelper.ConvertDataTable<ICRARiskArea>(ds.Tables[1]);

                    foreach (var item in iCRARiskArea)
                    {
                        var list = icraRiskArea.Where(x => x.ConstructionRiskAreaId == item.RiskAreaId).ToList();
                        if (list.Count > 0)
                        {
                            foreach (var _list in list)
                            {
                                item.RiskType = _list.Name;
                                item.RiskTypeID = _list.RiskTypeID;
                                //if(_list.FireRiskType>0)
                                //{
                                //    item.FireRiskType = _list.FireRiskType;
                                //}
                            }

                        }
                    }
                }

            }
            return iCRARiskArea;
        }

        public  ICRARiskArea GetICRARiskArea(int riskAreaId)
        {
            return GetICRARiskArea().Where(x => x.RiskAreaId == riskAreaId).FirstOrDefault();
        }

        #endregion

        #region ICRAMatixPrecautions

        public  int AddICRAMatixPrecautions(int constructionRiskId, int constructionTypeId, string classIdss, int createdby,int version)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_ICRAMatixPrecautions;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ConstructionRiskId", constructionRiskId);
                command.Parameters.AddWithValue("@ConstructionTypeId", constructionTypeId);
                command.Parameters.AddWithValue("@ClassIdss", classIdss);
                command.Parameters.AddWithValue("@version", version);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  List<ICRAMatixPrecautions> GetICRAMatixPrecautions()
        {
            List<ICRAMatixPrecautions> objICRAMatixPrecautions = new List<ICRAMatixPrecautions>();
            const string sql = StoredProcedures.Get_ICRAMatixPrecautions;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ICRAMatixPrecautions icraMatixPrecautions = new ICRAMatixPrecautions()
                        {
                            ConstructionClassId = Convert.ToInt32(row["ConstructionClassId"].ToString()),
                            ConstructionTypeId = Convert.ToInt32(row["ConstructionTypeId"].ToString()),
                            ConstructionRiskId = Convert.ToInt32(row["ConstructionRiskId"].ToString()),
                            Version = Convert.ToInt32(row["Version"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            ConstructionType = new ConstructionType()
                            {
                                ConstructionTypeId = Convert.ToInt32(row["ConstructionTypeId"].ToString()),
                                TypeName = row["TypeName"].ToString(),
                            },
                            ConstructionClass = new ConstructionClass()
                            {
                                ConstructionClassId = Convert.ToInt32(row["ConstructionClassId"].ToString()),
                                ClassName = row["ClassName"].ToString(),
                            },
                            ConstructionRisk = new ConstructionRisk()
                            {
                                ConstructionRiskId = Convert.ToInt32(row["ConstructionRiskId"].ToString()),
                                RiskName = row["RiskName"].ToString(),
                            },
                        };
                        objICRAMatixPrecautions.Add(icraMatixPrecautions);
                    }
                }
                return objICRAMatixPrecautions;
            }
        }
        #endregion

        //GetIcras NEW_PRAVEEN
        public  List<TICRAReports> GetICRAProjectReport(string projectIds = null)
        {
            List<TICRAReports> ticrareports = new List<TICRAReports>();
            const string sql = StoredProcedures.Get_Icras_Reports;

            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@projectIds", projectIds);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    ticrareports = _sqlHelper.ConvertDataTable<TICRAReports>(ds.Tables[0]);
                    var ticraprojects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[1]);
                    List<UserProfile> users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]);
                    foreach (var report in ticrareports)
                    {
                        report.Projects = ticraprojects.Where(x => x.TIcraReportId == report.TICRAReportId).ToList();
                        report.ProjectNames = string.Join(" , ", report.Projects.Select(x => x.ProjectName).ToList());
                        report.ProjectNos = string.Join(" , ", report.Projects.Select(x => x.ProjectNumber).ToList());
                        report.ProjectIds = string.Join(",", report.Projects.Select(x => x.ProjectId).ToList());
                        report.ProjectLocations = string.Join(",", report.Projects.Select(x => x.ProjectLocation).ToList());
                        report.ProjectContractors = string.Join(",", report.Projects.Select(x => x.ProjectContractor).ToList());

                        if (report.ContractorId.HasValue)
                            report.ContractorUser = users.Where(x => x.UserId == report.ContractorId).FirstOrDefault();

                        if (report.ObserverId.HasValue)
                            report.ObserverUser = users.Where(x => x.UserId == report.ObserverId).FirstOrDefault();
                    }
                }
            }
            return ticrareports;
        }

    }
}
