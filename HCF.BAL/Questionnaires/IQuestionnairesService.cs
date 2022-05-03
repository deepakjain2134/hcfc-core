using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IQuestionnairesService
    {
        int AddQuestionnaire(Questionnaire questionnaire);
        int AddQuestionnaireSteps(QuestionnaireSteps questionnairesteps);
        int AddQuestionnaireStepsDetails(QuestionnaireStepDetail questionnairestepdetail);
        List<Questionnaire> GetActiveQuestionnaires();
        List<QuestionnaireTypes> GetAllQuestionnairesTypes();
        Questionnaire GetQuestionnaire(int questionnaireId);
        List<Questionnaire> GetQuestionnaires();
        List<QuestionnaireHeaderTypes> QuestionnaireHeaderTypes();
        bool UpdateQuestionnaireSequence(string seqIds);
        bool UpdateQuestionnaireSteps(QuestionnaireSteps objQuestionnaireSteps);
        bool UpdateQuestionnaireStepSequence(string seqIds, string quesstepId, string quesnnaireid);
    }
}