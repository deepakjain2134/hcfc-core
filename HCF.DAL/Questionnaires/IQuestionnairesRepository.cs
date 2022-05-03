using System.Collections.Generic;
using HCF.BDO;

namespace HCF.DAL
{
    public interface IQuestionnairesRepository
    {
        int AddQuestionnaire(Questionnaire questionnaire);
        int AddQuestionnaireSteps(QuestionnaireSteps questionnairesteps);
        int AddQuestionnaireStepsDetails(QuestionnaireStepDetail questionnairestepdetail);
        List<Questionnaire> GetActiveQuestionnaires(int? questionnaireId);
        List<QuestionnaireTypes> GetAllQuestionnairesTypes();
        List<Questionnaire> GetQuestionnaires(int? questionnaireId);
        List<QuestionnaireHeaderTypes> QuestionnaireHeaderTypes();
        bool UpdateQuestionnaireSequence(string seqIds);
        bool UpdateQuestionnaireSteps(QuestionnaireSteps objQuestionnaireSteps);
        bool UpdateQuestionnaireStepSequence(string seqIds, string quesstepId, string quesnnaireid);
    }
}