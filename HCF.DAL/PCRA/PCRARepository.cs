using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class PCRARepository : IPCRARepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IConstructionRepository _constructionRepository;
        private readonly ICommonRepository _commonRepository;
        public PCRARepository(ISqlHelper sqlHelper, IConstructionRepository constructionRepository, ICommonRepository commonRepository)
        {
            _constructionRepository = constructionRepository;
            _sqlHelper = sqlHelper;
            _commonRepository = commonRepository;
        }

        #region Question PCRA

        public  List<QuestionPCRA> GetQuestionPCRA()
        {
            List<QuestionPCRA> questionPCRAs = new List<QuestionPCRA>();
            const string sql = Utility.StoredProcedures.Get_QuestionPCRA;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        questionPCRAs = _sqlHelper.ConvertDataTable<QuestionPCRA>(ds.Tables[0]);
                    }
                }
            }
            return questionPCRAs;
        }

        public  int SaveQuestionPCRA(QuestionPCRA newQuestionPCRA)
        {
            int newId = 0;
            const string sql = Utility.StoredProcedures.Insert_QuestionPCRA;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@QuesPCRAId", newQuestionPCRA.QuesPCRAId);
                command.Parameters.AddWithValue("@Questions", newQuestionPCRA.Questions);
                command.Parameters.AddWithValue("@CanComment", newQuestionPCRA.CanComment);
                command.Parameters.AddWithValue("@IsOption", newQuestionPCRA.IsOption);
                command.Parameters.AddWithValue("@Notes", newQuestionPCRA.Notes);
                command.Parameters.AddWithValue("@IsActive", newQuestionPCRA.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", newQuestionPCRA.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        #endregion

        #region TPCRA Question

        public  TPCRAQuestion GetQuestionTPCRA(int? projectId, int? tPCRAQuesId,int? icraId,bool? IsPCRA)
        {
            TPCRAQuestion questionTPCRAs = new TPCRAQuestion();
            const string sql = Utility.StoredProcedures.Get_TPCRAQuestion;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProjectId", projectId.HasValue ? projectId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@TPCRAQuesId", tPCRAQuesId.HasValue ? tPCRAQuesId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@icraId", icraId.HasValue ? icraId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsPCRA", IsPCRA.HasValue ? IsPCRA.Value : (object)DBNull.Value);
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        List<TPCRAQuestionDetails> lists = new List<TPCRAQuestionDetails>();
                         List<QuestionPCRA> QuestionPCRA = new List<QuestionPCRA>();
                        List<QuestionOptionPCRA> QuestionOptionPCRA = new List<QuestionOptionPCRA>();
                        List<DigitalSignature> digitalSignature = new List<DigitalSignature>();
                        List<TPermitWorkFlowDetails> TPermitWorkFlowDetails = new List<TPermitWorkFlowDetails>();
                        var tIcraProject = new List<TIcraProject>();
                        var users = new List<UserProfile>();
                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            lists = _sqlHelper.ConvertDataTable<TPCRAQuestionDetails>(ds.Tables[1]);
                            QuestionPCRA = _sqlHelper.ConvertDataTable<QuestionPCRA>(ds.Tables[1]);
                            QuestionOptionPCRA = _sqlHelper.ConvertDataTable<QuestionOptionPCRA>(ds.Tables[1]);
                        }
                        if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                        {
                             digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[3]);
                        }
                        if (ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
                        {
                            TPermitWorkFlowDetails = _sqlHelper.ConvertDataTable<TPermitWorkFlowDetails>(ds.Tables[6]);
                        }
                        if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                        {
                             tIcraProject = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[2]);
                        }

                        //List<TDepartmentNearConstruction> lstdepatconstruction = _sqlHelper.ConvertDataTable<TDepartmentNearConstruction>(ds.Tables[5]);
                        lists = lists.GroupBy(test => test.QuesPCRAId).Select(grp => grp.First()).Distinct().ToList();
                        if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                        {
                             users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[4]);
                        }

                        List<PermitSetting> permitsettings = new List<PermitSetting>();
                        if(IsPCRA.HasValue && IsPCRA.Value && (!icraId.HasValue))
                        {
                            if (ds.Tables[8] != null && ds.Tables[8].Rows.Count > 0)
                            {
                                permitsettings = _sqlHelper.ConvertDataTable<PermitSetting>(ds.Tables[8]);
                            }
                        }
                        else
                        {
                            if (ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
                            {
                                permitsettings = _sqlHelper.ConvertDataTable<PermitSetting>(ds.Tables[7]);
                            }
                        }
                        
                        foreach (var item in lists)
                        {
                            item.QuesStatus = item.QuesStatus != null ? item.QuesStatus : -1;
                            item.QuestionPCRA = QuestionPCRA.FirstOrDefault(x => x.QuesPCRAId == item.QuesPCRAId);
                            item.QuestionPCRA.QuestionOptionPCRAs = new List<QuestionOptionPCRA>();
                            item.QuestionPCRA.QuestionOptionPCRAs = QuestionOptionPCRA.Where(x => x.QuesPCRAId == item.QuesPCRAId && x.PCRAOptionId != null).ToList(); //.GroupBy(test => test.Text).Select(grp => grp.Last()).ToList();

                        }


                        if ((tPCRAQuesId.HasValue && tPCRAQuesId.Value > 0) || (icraId.HasValue && icraId.Value > 0))
                        {
                            //questionTPCRAs = _sqlHelper.ConvertDataTable<TPCRAQuestion>(ds.Tables[0]).FirstOrDefault(x => x.TPCRAQuesId == tPCRAQuesId.Value);
                            questionTPCRAs = _sqlHelper.ConvertDataTable<TPCRAQuestion>(ds.Tables[0]).FirstOrDefault();
                            questionTPCRAs.Project = tIcraProject.FirstOrDefault(x => x.ProjectId == questionTPCRAs.ProjectId);



                            if (questionTPCRAs.Sign1SignatureId.HasValue)
                                questionTPCRAs.DSSign1Signature = digitalSignature.Where(x => x.DigSignatureId == questionTPCRAs.Sign1SignatureId).FirstOrDefault();


                            if (questionTPCRAs.Sign2SignatureId.HasValue)
                                questionTPCRAs.DSSign2Signature = digitalSignature.Where(x => x.DigSignatureId == questionTPCRAs.Sign2SignatureId).FirstOrDefault();

                            if (questionTPCRAs.Sign1Name.HasValue)
                                questionTPCRAs.Sign1User = users.Where(x => x.UserId == questionTPCRAs.Sign1Name).FirstOrDefault();

                            if (questionTPCRAs.Sign2Name.HasValue)
                                questionTPCRAs.Sign2User = users.Where(x => x.UserId == questionTPCRAs.Sign2Name).FirstOrDefault();


                            if (questionTPCRAs.ContractorSignatureId.HasValue)
                                questionTPCRAs.DSContractorSignature = digitalSignature.Where(x => x.DigSignatureId == questionTPCRAs.ContractorSignatureId).FirstOrDefault();

                            if (questionTPCRAs.ContractorId.HasValue)
                                questionTPCRAs.ContractorUser = users.Where(x => x.UserId == questionTPCRAs.ContractorId).FirstOrDefault();

                            if (questionTPCRAs.FacilitySignatureId.HasValue)
                                questionTPCRAs.DSFacilitySignature = digitalSignature.Where(x => x.DigSignatureId == questionTPCRAs.FacilitySignatureId).FirstOrDefault();

                            if (questionTPCRAs.FacilityId.HasValue)
                                questionTPCRAs.FacilityUser = users.Where(x => x.UserId == questionTPCRAs.FacilityId).FirstOrDefault();

                            if (questionTPCRAs.InfectionControlSignatureId.HasValue)
                                questionTPCRAs.DSInfectionControlSignature = digitalSignature.Where(x => x.DigSignatureId == questionTPCRAs.InfectionControlSignatureId).FirstOrDefault();

                            if (questionTPCRAs.InfectionControlId.HasValue)
                                questionTPCRAs.InfectionControlUser = users.Where(x => x.UserId == questionTPCRAs.InfectionControlId).FirstOrDefault();


                            if (questionTPCRAs.SafetySignatureId.HasValue)
                                questionTPCRAs.DSSafetySignature = digitalSignature.Where(x => x.DigSignatureId == questionTPCRAs.SafetySignatureId).FirstOrDefault();

                            if (questionTPCRAs.SafetyId.HasValue)
                                questionTPCRAs.SafetyUser = users.Where(x => x.UserId == questionTPCRAs.SafetyId).FirstOrDefault();

                            //questionTPCRAs.TDepartmentNearConstruction = lstdepatconstruction.Where(x => x.QuesPCRAId == questionTPCRAs.TPCRAQuesId).ToList();


                            if (questionTPCRAs.RequestedBy > 0)
                                questionTPCRAs.RequestByUser = users.Where(x => x.UserId == questionTPCRAs.RequestedBy).FirstOrDefault();
                            if (questionTPCRAs.CreatedBy > 0)
                                questionTPCRAs.ApproveByUser = users.Where(x => x.UserId == questionTPCRAs.CreatedBy).FirstOrDefault();

                            if (!string.IsNullOrEmpty(questionTPCRAs.TDrawingFields))
                                questionTPCRAs.DrawingAttachments = _commonRepository.GetUploadedDrawings(questionTPCRAs.TDrawingFields);

                        }
                        else
                        {
                            questionTPCRAs = new TPCRAQuestion();
                        }

                        questionTPCRAs.TPCRAQuestionDetails = lists;
                        if (TPermitWorkFlowDetails != null && questionTPCRAs != null)
                        {
                            questionTPCRAs.TPermitWorkFlowDetails = TPermitWorkFlowDetails;
                            if (questionTPCRAs.TPermitWorkFlowDetails != null && questionTPCRAs.TPermitWorkFlowDetails.Count > 0 && digitalSignature != null && digitalSignature.Count > 0)
                            {

                                foreach (var permitworkflow in questionTPCRAs.TPermitWorkFlowDetails)
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
            }
            return questionTPCRAs;
        }

        public  List<TIcraProject> GetProjectDetails()
        {
            var projectDetails = new List<TIcraProject>();
            const string sql = Utility.StoredProcedures.Get_ProjectDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            projectDetails = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[0]);
                        }
                    }
                }
            }
            return projectDetails;
        }
        public  bool DeletePCRADrawings(int TPCRAQuesNumber, string fileIds)
        {
            bool status;
            const string sql = Utility.StoredProcedures.Delete_PCRADrawings;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TPCRAQuesNumber", TPCRAQuesNumber);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        public  int InsertQuestionTPCRA(TPCRAQuestion questionTPCRA)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_TPCRAQuestion;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TPCRAQuesId", questionTPCRA.TPCRAQuesId);
                command.Parameters.AddWithValue("@ParentTPCRAQuesId", questionTPCRA.ParentTPCRAQuesId);
                command.Parameters.AddWithValue("@ProjectId", questionTPCRA.ProjectId);
                command.Parameters.AddWithValue("@Sign1Date", questionTPCRA.Sign1Date);
                command.Parameters.AddWithValue("@Sign2Date", questionTPCRA.Sign2Date);
                command.Parameters.AddWithValue("@Sign1Name", questionTPCRA.Sign1Name);
                command.Parameters.AddWithValue("@Sign2Name", questionTPCRA.Sign2Name);
                command.Parameters.AddWithValue("@Sign1SignatureId", questionTPCRA.Sign1SignatureId);
                command.Parameters.AddWithValue("@Sign2SignatureId", questionTPCRA.Sign2SignatureId);
                command.Parameters.AddWithValue("@CreatedBy", questionTPCRA.CreatedBy);
                command.Parameters.AddWithValue("@RiskAssessmentType", questionTPCRA.RiskAssessmentType);
                command.Parameters.AddWithValue("@TicraId", questionTPCRA.TicraId);
                command.Parameters.AddWithValue("@TPCRAQuesNumber", questionTPCRA.TPCRAQuesNumber);
                command.Parameters.AddWithValue("@PCRANumber", questionTPCRA.PCRANumber);
                command.Parameters.AddWithValue("@ContractorId", questionTPCRA.ContractorId);
                command.Parameters.AddWithValue("@ContractorSignatureDate", questionTPCRA.ContractorSignatureDate);
                command.Parameters.AddWithValue("@ContractorSignatureId", questionTPCRA.ContractorSignatureId);
                command.Parameters.AddWithValue("@FacilityId", questionTPCRA.FacilityId);
                command.Parameters.AddWithValue("@FacilitySignatureId", questionTPCRA.FacilitySignatureId);
                command.Parameters.AddWithValue("@FacilitySignatureDate", questionTPCRA.FacilitySignatureDate);
                command.Parameters.AddWithValue("@InfectionControlId", questionTPCRA.InfectionControlId);
                command.Parameters.AddWithValue("@InfectionControlSignatureId", questionTPCRA.InfectionControlSignatureId);
                command.Parameters.AddWithValue("@InfectionControlSignatureDate", questionTPCRA.InfectionControlSignatureDate);
                command.Parameters.AddWithValue("@SafetyId", questionTPCRA.SafetyId);
                command.Parameters.AddWithValue("@SafetySignatureDate", questionTPCRA.SafetySignatureDate);
                command.Parameters.AddWithValue("@SafetySignatureId", questionTPCRA.SafetySignatureId);
                command.Parameters.AddWithValue("@BuildingId", questionTPCRA.BuildingId);
                command.Parameters.AddWithValue("@BuildingName", questionTPCRA.BuildingName);
                command.Parameters.AddWithValue("@FloorName", questionTPCRA.FloorName);
                command.Parameters.AddWithValue("@WorkDescription", questionTPCRA.WorkDescription);
                command.Parameters.AddWithValue("@EmailAddress", questionTPCRA.EmailAddress);
                command.Parameters.AddWithValue("@Phone", questionTPCRA.Phone);
                command.Parameters.AddWithValue("@DateSubmitted", questionTPCRA.DateSubmitted);
                command.Parameters.AddWithValue("@RequestedBy", questionTPCRA.RequestedBy);
                command.Parameters.AddWithValue("@ApprovalStatus", questionTPCRA.ApprovalStatus);
                command.Parameters.AddWithValue("@RejectionReason", questionTPCRA.RejectionReason);
                command.Parameters.AddWithValue("@TDrawingFields", questionTPCRA.TDrawingFields);
                command.Parameters.AddWithValue("@IsPCRA", questionTPCRA.IsPCRA);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");



            }
            return newId;
        }

        public  int InsertQuestionDetailsTPCRA(TPCRAQuestionDetails questionDetailsTPCRA)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_TPCRAQuestionDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TPCRAQuesDetailId", questionDetailsTPCRA.TPCRAQuesDetailId);
                command.Parameters.AddWithValue("@TPCRAQuesId ", questionDetailsTPCRA.TPCRAQuesId);
                command.Parameters.AddWithValue("@QuesPCRAId ", questionDetailsTPCRA.QuesPCRAId);
                command.Parameters.AddWithValue("@PCRAOptionId", questionDetailsTPCRA.PCRAOptionId);
                command.Parameters.AddWithValue("@Comment", questionDetailsTPCRA.Comment);
                command.Parameters.AddWithValue("@QuesStatus", questionDetailsTPCRA.QuesStatus);
                command.Parameters.AddWithValue("@CreatedBy", questionDetailsTPCRA.CreatedBy);
                command.Parameters.AddWithValue("@OptionStatus", questionDetailsTPCRA.OptionStatus);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        //public  int InsertTDepartmentNearConstruction(TDepartmentNearConstruction TDepartmentNearConstruction)
        //{
        //    int newId;
        //    const string sql = Utility.StoredProcedures.Insert_TDepartmentNearConstruction;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@Id", TDepartmentNearConstruction.Id);
        //        command.Parameters.AddWithValue("@QuesPCRAId ", TDepartmentNearConstruction.QuesPCRAId);
        //        command.Parameters.AddWithValue("@Above", TDepartmentNearConstruction.Above);
        //        command.Parameters.AddWithValue("@below", TDepartmentNearConstruction.below);
        //        command.Parameters.AddWithValue("@North", TDepartmentNearConstruction.North);
        //        command.Parameters.AddWithValue("@South", TDepartmentNearConstruction.South);
        //        command.Parameters.AddWithValue("@East", TDepartmentNearConstruction.East);
        //        command.Parameters.AddWithValue("@West", TDepartmentNearConstruction.West);
        //        command.Parameters.AddWithValue("@CreatedBy", TDepartmentNearConstruction.CreatedBy);
        //        command.Parameters.Add("@newId", SqlDbType.Int);
        //        command.Parameters["@newId"].Direction = ParameterDirection.Output;
        //        newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
        //    }
        //    return newId;
        //}
        public  bool DeleteTIcraFiles(int TicraId, string TFileIds)
        {
            bool status;
            const string sql = Utility.StoredProcedures.Delete_TIcraFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TicraId", TicraId);
                command.Parameters.AddWithValue("@TFileIds", TFileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        public  List<TPCRAQuestion> GetAllTPCRA(bool? IsPCRA)
        {
            var tPCRAQuestion = new List<TPCRAQuestion>();
            const string sql = Utility.StoredProcedures.Get_TPCRAQuestion;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IsPCRA", IsPCRA.HasValue ? IsPCRA.Value : (object)DBNull.Value);
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        tPCRAQuestion = _sqlHelper.ConvertDataTable<TPCRAQuestion>(ds.Tables[0]);
                        var tIcraProject = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[2]);
                        List<TPermitWorkFlowDetails> TPermitWorkFlowDetails = _sqlHelper.ConvertDataTable<TPermitWorkFlowDetails>(ds.Tables[7]);
                        List<DigitalSignature> digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[3]);
                        var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[4]);
                        foreach (var item in tPCRAQuestion)
                        {
                            item.Project = tIcraProject.FirstOrDefault(x => x.ProjectId == item.ProjectId);
                            if (TPermitWorkFlowDetails != null && item != null)
                            {
                                item.TPermitWorkFlowDetails = TPermitWorkFlowDetails.Where(x => x.PermitNumber == item.PCRANumber).ToList();
                                if (item.TPermitWorkFlowDetails != null && item.TPermitWorkFlowDetails.Count > 0 && digitalSignature != null && digitalSignature.Count > 0)
                                {

                                    foreach (var permitworkflow in item.TPermitWorkFlowDetails)
                                    {
                                        permitworkflow.DSPermitSignature = digitalSignature.Where(x => x.DigSignatureId == permitworkflow.LabelSignId).FirstOrDefault();
                                        permitworkflow.UserDetail = users.Where(x => x.UserId == permitworkflow.LabelValue).FirstOrDefault();
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
            return tPCRAQuestion;
        }

        
        public  List<TPCRAQuestion> GetAllTCRA()
        {
            var tPCRAQuestion = new List<TPCRAQuestion>();
            var TIcraLog = new List<TIcraLog>();
            var tIcraProject = new List<TIcraProject>();
            const string sql = Utility.StoredProcedures.Get_TPCRAQuestion;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            tPCRAQuestion = _sqlHelper.ConvertDataTable<TPCRAQuestion>(ds.Tables[0]);
                            TIcraLog = _sqlHelper.ConvertDataTable<TIcraLog>(ds.Tables[0]);
                        }
                        if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                        {
                             tIcraProject = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[2]);
                        }
                        foreach (var item in tPCRAQuestion)
                        {
                            item.Project = tIcraProject.FirstOrDefault(x => x.ProjectId == item.ProjectId);
                            item.TIcraLog = TIcraLog.Where(x => x.TicraId == item.TicraId).FirstOrDefault();
                            if (item.TIcraLog.ConstructionTypeId > 0)
                                item.TIcraLog.ConstructionType = _constructionRepository.GetConstructionType(item.TIcraLog.ConstructionTypeId);
                            if (item.TIcraLog.ConstructionClassId > 0)
                                item.TIcraLog.ConstructionClass = _constructionRepository.GetConstructionClass(item.TIcraLog.ConstructionClassId);
                            if (item.TIcraLog.ConstructionRiskId > 0)
                                item.TIcraLog.ConstructionRisk = _constructionRepository.GetConstructionRisk(item.TIcraLog.ConstructionRiskId);
                        }
                    }                    
                }
            }
            return tPCRAQuestion;
        }

        public  int CheckTPCRAdddorEdit(string projectId)
        {
            int status;
            const string sql = Utility.StoredProcedures.Check_TPCRAdddorEdit;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProjectId", projectId);
                command.Parameters.Add("@status", SqlDbType.Int);
                command.Parameters["@status"].Direction = ParameterDirection.Output;
                status = _sqlHelper.ExecuteNonQuery(command, "@status");
            }
            return status;
        }


        #endregion

        #region Question Option PCRA
        public  string AddEditQuestionOptionPCRA(QuestionOptionPCRA questionOptionPCRA)
        {
            string success;
            int questionOptionPCRAId = 0;
            const string sql = Utility.StoredProcedures.Update_QuestionOptionPCRA;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PCRAOptionId", questionOptionPCRA.PCRAOptionId);
                command.Parameters.AddWithValue("@QuesPCRAId ", questionOptionPCRA.QuesPCRAId);
                command.Parameters.AddWithValue("@Text ", questionOptionPCRA.Text);
                command.Parameters.AddWithValue("@IsReadOnly ", questionOptionPCRA.IsReadOnly);
                command.Parameters.AddWithValue("@CanComment ", questionOptionPCRA.CanComment);
                command.Parameters.AddWithValue("@IsActive ", questionOptionPCRA.IsActive);
                command.Parameters.AddWithValue("@CreatedBy ", questionOptionPCRA.CreatedBy);
                command.Parameters.Add("@QuesPCRAOptionId", SqlDbType.Int);
                command.Parameters["@QuesPCRAOptionId"].Direction = ParameterDirection.Output;
                questionOptionPCRAId = _sqlHelper.ExecuteNonQuery(command, "@QuesPCRAOptionId");
            }
            if (questionOptionPCRA.QuesPCRAId > 0)
            {
                success = String.Format("Question Option {0} successfully", questionOptionPCRA.PCRAOptionId == null ? "inserted" : "updated");
                success = questionOptionPCRAId == -1 ? "insert failed" : success;
            }
            else
            {
                success = (questionOptionPCRAId > 0) ? "" : "There is some problem";
            }
            return success;
        }

        public  List<QuestionOptionPCRA> GetQuestionOptionPCRA(int? quesPCRAId)
        {
            var questionOptionPCRAs = new List<QuestionOptionPCRA>();
            const string sql = Utility.StoredProcedures.Get_QuestionOptionPCRA;

            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@QuesPCRAId", quesPCRAId);
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        questionOptionPCRAs = _sqlHelper.ConvertDataTable<QuestionOptionPCRA>(ds.Tables[0]);
                    }
                }
            }

            return questionOptionPCRAs;
        }

        public  bool DeleteQuestionOptionPCRA(string pCRAOptionId, string quesPCRAId)
        {
            bool success = false;
            const string sql = Utility.StoredProcedures.Delete_QuestionOptionPCRA;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PCRAOptionId", pCRAOptionId);
                command.Parameters.AddWithValue("@QuesPCRAId", quesPCRAId);
                success = _sqlHelper.ExecuteNonQuery(command);
            }
            return success;
        }
        #endregion
    }
}