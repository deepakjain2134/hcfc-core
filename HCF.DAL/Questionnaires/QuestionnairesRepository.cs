using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HCF.BDO;

using HCF.Utility;

namespace HCF.DAL
{
    public  class QuestionnairesRepository : IQuestionnairesRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ICommonRepository _commonRepository;

        public QuestionnairesRepository(ISqlHelper sqlHelper, ICommonRepository commonRepository)
        {
            _sqlHelper = sqlHelper;
            _commonRepository = commonRepository;
        }
        #region MainGoal/Questionnaires

        private List<Questionnaire> GetAllQuestionnaires(int? questionnaireId)
        {
            var lists = new List<Questionnaire>();
            const string sql = StoredProcedures.Get_Questionnaires;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@questionnaireId", questionnaireId);
                using (var ds = _sqlHelper.GetDataSet(command))
                {
                    lists = _sqlHelper.ConvertDataTable<Questionnaire>(ds.Tables[0]);
                    var questionnaireSteps = _sqlHelper.ConvertDataTable<QuestionnaireSteps>(ds.Tables[1]);
                    var questionnaireOptions = _sqlHelper.ConvertDataTable<QuestionnaireOptions>(ds.Tables[2]);
                    var questionnaireHeaderTypes = _sqlHelper.ConvertDataTable<QuestionnaireHeaderTypes>(ds.Tables[2]);
                    var questionnaireStepDetail = _sqlHelper.ConvertDataTable<QuestionnaireStepDetail>(ds.Tables[3]);
                    foreach (var item in questionnaireOptions)
                    {
                        item.QuestionnaireHeaderType = questionnaireHeaderTypes.FirstOrDefault(x => x.QuestionnaireHeaderTypeId == item.QuestionnaireHeaderTypeId);
                    }

                    foreach (var item in lists)
                    {
                        item.QuestionnaireOptions = questionnaireOptions.Where(x => x.QuestionnaireId == item.QuestionnaireId).ToList();
                        item.QuestionnaireSteps = questionnaireSteps.Where(x => x.QuestionnaireId == item.QuestionnaireId).ToList();
                        item.QuestHeaderTypeIds = string.Join(",", item.QuestionnaireOptions.Select(x => x.QuestionnaireHeaderTypeId));
                        foreach (var questionnaireStep in item.QuestionnaireSteps)
                        {
                            questionnaireStep.QuestionnaireStepDetail = questionnaireStepDetail.Where(x => x.QuestionnaireStepId == questionnaireStep.QuestionnaireStepId).ToList();
                            questionnaireStep.PMLogSteps = new List<TPMLogSteps>();
                            for (int i = 0; i < item.QuestionnaireOptions.Count; i++)
                            {
                                TPMLogSteps stepLog = new TPMLogSteps
                                {
                                    QuestionnaireHeaderTypeId = item.QuestionnaireOptions[i].QuestionnaireHeaderTypeId,
                                    QuestionnaireStepId = questionnaireStep.QuestionnaireStepId
                                };
                                questionnaireStep.PMLogSteps.Add(stepLog);
                            }

                            if (item.QuestionnaireOptions.Count == 0)
                            {
                                TPMLogSteps stepLog = new TPMLogSteps
                                {
                                    QuestionnaireHeaderTypeId = 0,
                                    QuestionnaireStepId = questionnaireStep.QuestionnaireStepId

                                };
                                questionnaireStep.PMLogSteps.Add(stepLog);
                            }
                        }
                    }
                }
            }
            return lists;
        }


        public  List<Questionnaire> GetQuestionnaires(int? questionnaireId)
        {
            return GetAllQuestionnaires(questionnaireId);
        }

        public  List<Questionnaire> GetActiveQuestionnaires(int? questionnaireId)
        {
            var questionnaires = GetAllQuestionnaires(questionnaireId).Where(x => x.IsActive).ToList();
            foreach (var questionnaire in questionnaires)
            {
                questionnaire.QuestionnaireSteps = questionnaire.QuestionnaireSteps.Where(x => x.IsActive).ToList();
            }
            return questionnaires;
        }


        public  bool UpdateQuestionnaireSteps(QuestionnaireSteps objQuestionnaireSteps)
        {
            bool res;
            const string sql = StoredProcedures.Update_QuestionnaireSteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@QuestionnaireStepId", objQuestionnaireSteps.QuestionnaireStepId);
                command.Parameters.AddWithValue("@IsActive", objQuestionnaireSteps.IsActive);
                res = _sqlHelper.ExecuteNonQuery(command);
            }
            return res;
        }

        public  bool UpdateQuestionnaireSequence(string seqIds)
        {
            bool status;
            const string sql = StoredProcedures.Update_QuestionnaireSequence;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@seqIds", seqIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public  bool UpdateQuestionnaireStepSequence(string seqIds, string quesstepId, string quesnnaireid)
        {
            bool status = false;
            const string sql = StoredProcedures.Update_QuestionnaireStepSequence;
            string[] seqid = seqIds.Split(',');
            string[] quesstepid = quesstepId.Split(',');
            string[] quesnnaireId = quesnnaireid.Split(',');
            for (int i = 0; i < seqid.Length; i++)
            {
                int sequeid = Convert.ToInt32(seqid[i]);
                int quesstepID = Convert.ToInt32(quesstepid[i]);
                int quesnnaireID = Convert.ToInt32(quesnnaireId[i]);
                using (var command = new SqlCommand(sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@seqIds", sequeid);
                    command.Parameters.AddWithValue("@quesstepId", quesstepID);
                    command.Parameters.AddWithValue("@quesnnaireId", quesnnaireID);
                    status = _sqlHelper.ExecuteNonQuery(command);
                }
            }
            return status;
        }

        public  int AddQuestionnaire(Questionnaire questionnaire)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Questionnaire;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@QuestionnaireId", questionnaire.QuestionnaireId);
                command.Parameters.AddWithValue("@QuestionnaireTypeId", questionnaire.QuestionnaireTypeId);
                command.Parameters.AddWithValue("@Goal", questionnaire.Goal);
                command.Parameters.AddWithValue("@Description", questionnaire.Description);
                command.Parameters.AddWithValue("@IsActive", questionnaire.IsActive);
                command.Parameters.AddWithValue("@IsHide", questionnaire.IsHide);
                command.Parameters.AddWithValue("@QuestHeaderTypeIds ", questionnaire.QuestHeaderTypeIds);
                command.Parameters.AddWithValue("@CreatedBy  ", questionnaire.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;
        }

        #endregion


        #region Steps/Options

        public  int AddQuestionnaireSteps(QuestionnaireSteps questionnairesteps)
        {
            int newId;
            //string[] getString = questionnairesteps.Step.Split(' ');
            //string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            //var stringChars = new char[3];
            //var random = new Random();
            //for (int i = 0; i < stringChars.Length; i++)
            //{
            //    stringChars[i] = chars[random.Next(chars.Length)];
            //}
            //var finalString = new String(stringChars);
            //bool getStatus = Array.Exists(getString, element => element == "Fuel");
            //if (questionnairesteps.QuestionnaireStepDetail[0].QuestionnaireHeaderTypeId == 26 || questionnairesteps.QuestionnaireStepDetail[0].QuestionnaireHeaderTypeId == 25)
            //{
            //    questionnairesteps.StepCode = "FTG";
            //}
            //else
            //{
            //if (getString.Length == 1)
            //{
            //    questionnairesteps.StepCode = getString[0].Substring(0, 3).ToUpper();
            //}
            //else if(getString.Length == 2)
            //{
            //    questionnairesteps.StepCode = getString[0].Substring(0, 1).ToUpper() + getString[1].Substring(0,2).ToUpper();
            //}
            //else if (getString.Length == 3)
            //{
            //    questionnairesteps.StepCode = getString[0].Substring(0, 1).ToUpper() + getString[1].Substring(0,1).ToUpper() + getString[2].Substring(0,1).ToUpper();
            //}
            //else if (getString.Length == 4)
            //{
            //    questionnairesteps.StepCode = getString[0].Substring(0, 1).ToUpper() + getString[1].Substring(0, 1).ToUpper() + getString[2].Substring(0, 1).ToUpper() + getString[3].Substring(0, 1).ToUpper();
            //}
            //else
            //{
            //    questionnairesteps.StepCode = getString[0].Substring(0, 1).ToUpper() + getString[1].Substring(0, 1).ToUpper() + getString[2].Substring(0, 1).ToUpper() + getString[3].Substring(0, 2).ToUpper();
            //}
            //}            

            const string sql = StoredProcedures.Insert_QuestionnaireSteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@QuestionnaireStepId", questionnairesteps.QuestionnaireStepId);
                command.Parameters.AddWithValue("@QuestionnaireId", questionnairesteps.QuestionnaireId);
                command.Parameters.AddWithValue("@Step", questionnairesteps.Step);
                command.Parameters.AddWithValue("@StepCode", questionnairesteps.StepCode);
                command.Parameters.AddWithValue("@IsActive", questionnairesteps.IsActive);
                command.Parameters.AddWithValue("@CreatedBy  ", questionnairesteps.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;
        }


        public  int AddQuestionnaireStepsDetails(QuestionnaireStepDetail questionnairestepdetail)
        {
            int newId;
            const string sql = StoredProcedures.Insert_QuestionnaireStepsDetail;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@QuestionnaireStepDetailId", questionnairestepdetail.QuestionnaireStepDetailId);
                command.Parameters.AddWithValue("@QuestionnaireStepId", questionnairestepdetail.QuestionnaireStepId);
                command.Parameters.AddWithValue("@RecommendedValue", questionnairestepdetail.RecommendedValue);
                command.Parameters.AddWithValue("@MinValue", questionnairestepdetail.MinValue);
                command.Parameters.AddWithValue("@MaxValue", questionnairestepdetail.MaxValue);
                command.Parameters.AddWithValue("@InputType", questionnairestepdetail.InputType);
                command.Parameters.AddWithValue("@Format", questionnairestepdetail.Format);
                command.Parameters.AddWithValue("@CreatedBy", questionnairestepdetail.CreatedBy);
                command.Parameters.AddWithValue("@InputValueHeader", questionnairestepdetail.InputValueHeader);
                command.Parameters.AddWithValue("@QuestionnaireHeaderTypeId", questionnairestepdetail.QuestionnaireHeaderTypeId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;
        }


        #endregion


        #region QuestionnairesTypes
        public  List<QuestionnaireTypes> GetAllQuestionnairesTypes()
        {
            return _commonRepository.GetTableData<QuestionnaireTypes>(StoredProcedures.Get_QuestionnairesTypes);
        }

        public  List<QuestionnaireHeaderTypes> QuestionnaireHeaderTypes()
        {
            return _commonRepository.GetTableData<QuestionnaireHeaderTypes>(StoredProcedures.Get_QuestionnaireHeaderTypes);
        }

        #endregion

    }
}
