using System.Collections.Generic;
using HCF.BDO;

namespace HCF.DAL
{
    public interface IPCRARepository
    {
        string AddEditQuestionOptionPCRA(QuestionOptionPCRA questionOptionPCRA);
        int CheckTPCRAdddorEdit(string projectId);
        bool DeletePCRADrawings(int TPCRAQuesNumber, string fileIds);
        bool DeleteQuestionOptionPCRA(string pCRAOptionId, string quesPCRAId);
        bool DeleteTIcraFiles(int TicraId, string TFileIds);
        List<TPCRAQuestion> GetAllTCRA();
        List<TPCRAQuestion> GetAllTPCRA(bool? IsPCRA);
        List<TIcraProject> GetProjectDetails();
        List<QuestionOptionPCRA> GetQuestionOptionPCRA(int? quesPCRAId);
        List<QuestionPCRA> GetQuestionPCRA();
        TPCRAQuestion GetQuestionTPCRA(int? projectId, int? tPCRAQuesId, int? icraId, bool? IsPCRA);
        int InsertQuestionDetailsTPCRA(TPCRAQuestionDetails questionDetailsTPCRA);
        int InsertQuestionTPCRA(TPCRAQuestion questionTPCRA);
        int SaveQuestionPCRA(QuestionPCRA newQuestionPCRA);
    }
}