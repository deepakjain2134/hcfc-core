using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL
{
    public class QuestionnairesService : IQuestionnairesService
    {
        private readonly IQuestionnairesRepository _questionnairesRepository;
        public QuestionnairesService(IQuestionnairesRepository questionnairesRepository)
        {
            _questionnairesRepository = questionnairesRepository;
        }

        #region MainGoal/Questionnaires

     
        public  List<Questionnaire> GetQuestionnaires()
        {
            return _questionnairesRepository.GetQuestionnaires(null);
        }


        public  List<Questionnaire> GetActiveQuestionnaires()
        {
            return _questionnairesRepository.GetActiveQuestionnaires(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  Questionnaire GetQuestionnaire(int questionnaireId)
        {
            return _questionnairesRepository.GetQuestionnaires(questionnaireId).FirstOrDefault();
        }

        public  int AddQuestionnaire(Questionnaire questionnaire)
        {
            return _questionnairesRepository.AddQuestionnaire(questionnaire);
        }      

        public  int AddQuestionnaireSteps(QuestionnaireSteps questionnairesteps)
        {
            return _questionnairesRepository.AddQuestionnaireSteps(questionnairesteps);
        }

        public  int AddQuestionnaireStepsDetails(QuestionnaireStepDetail questionnairestepdetail)
        {
            return _questionnairesRepository.AddQuestionnaireStepsDetails(questionnairestepdetail);
        }

        #endregion

        #region QuestionnairesTypes

        public  List<QuestionnaireTypes> GetAllQuestionnairesTypes()
        {
            return _questionnairesRepository.GetAllQuestionnairesTypes();
        }

        public  List<QuestionnaireHeaderTypes> QuestionnaireHeaderTypes()
        {
            return _questionnairesRepository.QuestionnaireHeaderTypes();
        }

        public  bool UpdateQuestionnaireSequence(string seqIds)
        {
            return _questionnairesRepository.UpdateQuestionnaireSequence(seqIds);
        }

        public  bool UpdateQuestionnaireStepSequence(string seqIds, string quesstepId, string quesnnaireid)
        {
            return _questionnairesRepository.UpdateQuestionnaireStepSequence(seqIds, quesstepId, quesnnaireid);
        }

        public  bool UpdateQuestionnaireSteps(QuestionnaireSteps objQuestionnaireSteps)
        {
            return _questionnairesRepository.UpdateQuestionnaireSteps(objQuestionnaireSteps);
        }

        #endregion
    }
}
