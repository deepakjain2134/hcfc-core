using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;

namespace HCF.BAL
{
    public  class PCRAService : IPCRAService
    {
        private readonly ICommonService _commonService;
        private readonly IEmailService _emailService;

        private readonly IPCRARepository _pCRARepository;

        public PCRAService(ICommonService commonService, IEmailService emailService, IPCRARepository pCRARepository)
        {
            _commonService = commonService;
            _emailService = emailService;
            _pCRARepository = pCRARepository;

        }
        #region Question PCRA

        public  List<QuestionPCRA> GetQuestionPCRA()
        {
            return _pCRARepository.GetQuestionPCRA();
        }

        public  int SaveQuestionPCRA(QuestionPCRA newQuestionPCRA)
        {
            return _pCRARepository.SaveQuestionPCRA(newQuestionPCRA);
        }

        #endregion

        #region TPCRA Question

        public  TPCRAQuestion GetQuestionTPCRA(int? projectId, int? tPCRAQuesId,int? icraId=null,bool? IsPCRA=false)
        {
            return _pCRARepository.GetQuestionTPCRA(projectId, tPCRAQuesId, icraId, IsPCRA);
        }
        public  bool DeletePCRADrawings(int TPCRAId, string fileIds)
        {
            return _pCRARepository.DeletePCRADrawings(TPCRAId, fileIds);
        }
        public  List<TIcraProject> GetProjectDetails()
        {
            return _pCRARepository.GetProjectDetails();
        }

        public  int InsertQuestionTPCRA(TPCRAQuestion questionTPCRAs)
        {
            int result = 0;
            //if (!string.IsNullOrEmpty(questionTPCRAs.DSSign1Signature.FileContent) || questionTPCRAs.DSSign1Signature.DigSignatureId > 0)
            //    questionTPCRAs.Sign1SignatureId = CommonRepository.SaveDigitalFile(questionTPCRAs.DSSign1Signature, questionTPCRAs.ProjectId.ToString());

            //if (!string.IsNullOrEmpty(questionTPCRAs.DSSign2Signature.FileContent) || questionTPCRAs.DSSign2Signature.DigSignatureId > 0)
            //    questionTPCRAs.Sign2SignatureId = CommonRepository.SaveDigitalFile(questionTPCRAs.DSSign2Signature, questionTPCRAs.ProjectId.ToString());

            if (!string.IsNullOrEmpty(questionTPCRAs.DSContractorSignature.FileContent) || questionTPCRAs.DSContractorSignature.DigSignatureId > 0)
                questionTPCRAs.ContractorSignatureId = _commonService.SaveDigitalFile(questionTPCRAs.DSContractorSignature, questionTPCRAs.ProjectId.ToString());

            if (!string.IsNullOrEmpty(questionTPCRAs.DSFacilitySignature.FileContent) || questionTPCRAs.DSFacilitySignature.DigSignatureId > 0)
                questionTPCRAs.FacilitySignatureId = _commonService.SaveDigitalFile(questionTPCRAs.DSFacilitySignature, questionTPCRAs.ProjectId.ToString());

            if (!string.IsNullOrEmpty(questionTPCRAs.DSInfectionControlSignature.FileContent) || questionTPCRAs.DSInfectionControlSignature.DigSignatureId > 0)
                questionTPCRAs.InfectionControlSignatureId = _commonService.SaveDigitalFile(questionTPCRAs.DSInfectionControlSignature, questionTPCRAs.ProjectId.ToString());

            if (!string.IsNullOrEmpty(questionTPCRAs.DSSafetySignature.FileContent) || questionTPCRAs.DSSafetySignature.DigSignatureId > 0)
                questionTPCRAs.SafetySignatureId = _commonService.SaveDigitalFile(questionTPCRAs.DSSafetySignature, questionTPCRAs.ProjectId.ToString());

            result = _pCRARepository.InsertQuestionTPCRA(questionTPCRAs);

            //if(questionTPCRAs.TPCRAQuesId==0)
            //{
            //    if (questionTPCRAs.TIcraLog.PermitRequestBy.HasValue && questionTPCRAs.TIcraLog.PermitRequestBy > 0 && questionTPCRAs.TIcraLog.IsNotifyEmailPermitReq)
            //        Email.SendPCRANotifyEmail(questionTPCRAs.TIcraLog.PermitRequestBy.Value, questionTPCRAs.TIcraLog.PermitNo, "1", result, questionTPCRAs.ProjectId);
            //    if (questionTPCRAs.TIcraLog.PermitAuthorizedBy.HasValue && questionTPCRAs.TIcraLog.PermitAuthorizedBy > 0 && questionTPCRAs.TIcraLog.IsNotifyEmailPermitAuth)
            //        Email.SendPCRANotifyEmail(questionTPCRAs.TIcraLog.PermitAuthorizedBy.Value, questionTPCRAs.TIcraLog.PermitNo, "1", result, questionTPCRAs.ProjectId);
            //    if (questionTPCRAs.TIcraLog.PermitReviewerBy.HasValue && questionTPCRAs.TIcraLog.PermitReviewerBy > 0 && questionTPCRAs.TIcraLog.IsNotifyEmailPermitReviewer)
            //        Email.SendPCRANotifyEmail(questionTPCRAs.TIcraLog.PermitReviewerBy.Value, questionTPCRAs.TIcraLog.PermitNo, "1", result, questionTPCRAs.ProjectId);
            //}
            
            return result;

        }

        public  int InsertQuestionDetailsTPCRA(TPCRAQuestionDetails questionTPCRAs)
        {
            return _pCRARepository.InsertQuestionDetailsTPCRA(questionTPCRAs);
        }
        //public  int InsertTDepartmentNearConstruction(TDepartmentNearConstruction TDepartmentNearConstruction)
        //{
        //    return _pCRARepository.InsertTDepartmentNearConstruction(TDepartmentNearConstruction);
        //}

        public  List<TPCRAQuestion> GetAllTPCRA(bool? IsPCRA=false)
        {
            return _pCRARepository.GetAllTPCRA(IsPCRA);
        }
        public  List<TPCRAQuestion> GetAllTCRA()
        {
            return _pCRARepository.GetAllTCRA();
        }
        //public  List<TPCRAQuestionDetails> GetAllTPCRAQuestionDetails()
        //{
        //    return _pCRARepository.GetAllTPCRAQuestionDetails();
        //}
        public  int CheckTPCRAdddorEdit(string projectId)
        {
            return _pCRARepository.CheckTPCRAdddorEdit(projectId);
        }
        public  bool DeleteTIcraFiles(int TicraId, string TFileIds)
        {
            return _pCRARepository.DeleteTIcraFiles(TicraId, TFileIds);
        }
        #endregion

        #region Question Option PCRA
        public  string AddEditQuestionOptionPCRA(QuestionOptionPCRA questionOptionPCRA)
        {
            return _pCRARepository.AddEditQuestionOptionPCRA(questionOptionPCRA);
        }

        public  List<QuestionOptionPCRA> GetQuestionOptionPCRA(int? quesPCRAId)
        {
            return _pCRARepository.GetQuestionOptionPCRA(quesPCRAId);
        }

        public  bool DeleteQuestionOptionPCRA(string pCRAOptionId, string quesPCRAId)
        {
            return _pCRARepository.DeleteQuestionOptionPCRA(pCRAOptionId, quesPCRAId);
        }
        #endregion
    }
}
