using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IPCRAService
    {
        string AddEditQuestionOptionPCRA(QuestionOptionPCRA questionOptionPCRA);
        int CheckTPCRAdddorEdit(string projectId);
        bool DeletePCRADrawings(int TPCRAId, string fileIds);
        bool DeleteQuestionOptionPCRA(string pCRAOptionId, string quesPCRAId);
        bool DeleteTIcraFiles(int TicraId, string TFileIds);
        List<TPCRAQuestion> GetAllTCRA();
        List<TPCRAQuestion> GetAllTPCRA(bool? IsPCRA = false);
        List<TIcraProject> GetProjectDetails();
        List<QuestionOptionPCRA> GetQuestionOptionPCRA(int? quesPCRAId);
        List<QuestionPCRA> GetQuestionPCRA();
        TPCRAQuestion GetQuestionTPCRA(int? projectId, int? tPCRAQuesId, int? icraId = null, bool? IsPCRA = false);
        int InsertQuestionDetailsTPCRA(TPCRAQuestionDetails questionTPCRAs);
        int InsertQuestionTPCRA(TPCRAQuestion questionTPCRAs);
        int SaveQuestionPCRA(QuestionPCRA newQuestionPCRA);
    }
}