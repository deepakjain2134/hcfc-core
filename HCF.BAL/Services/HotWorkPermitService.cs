using HCF.BDO;
using System.Collections.Generic;
using HCF.BAL.Interfaces.Services;
using HCF.DAL;

namespace HCF.BAL
{
    public class HotWorkPermitService : IHotWorkPermitService
    {

        private readonly ICommonService _commonService;
        private readonly IHotWorkPermitRepository _hotWorkPermitRepository;
        public HotWorkPermitService(ICommonService commonService, IHotWorkPermitRepository hotWorkPermitRepository)
        {
            _commonService = commonService;
            _hotWorkPermitRepository = hotWorkPermitRepository;
        }

        #region Hot Work Permit


        public List<THotWorkPermits> GetAllHotWorksPermit()
        {

            return _hotWorkPermitRepository.GetAllHotWorksPermit();
        }

        public THotWorkPermits GetTHotWorksPermit(int? tHotWorkPermitId)
        {

            return _hotWorkPermitRepository.GetTHotWorksPermit(tHotWorkPermitId);
        }
        public int InsertTHotWorkPermits(THotWorkPermits thotWorkPermits)
        {
            if (!string.IsNullOrEmpty(thotWorkPermits.DSSign1Signature.FileContent) || thotWorkPermits.DSSign1Signature.DigSignatureId > 0)
                thotWorkPermits.PermitRequestorSignatureId = _commonService.SaveDigitalFile(thotWorkPermits.DSSign1Signature, thotWorkPermits.ProjectId.ToString());

            if (!string.IsNullOrEmpty(thotWorkPermits.DSSign2Signature.FileContent) || thotWorkPermits.DSSign2Signature.DigSignatureId > 0)
                thotWorkPermits.PermitAuthorizedSignatureId = _commonService.SaveDigitalFile(thotWorkPermits.DSSign2Signature, thotWorkPermits.ProjectId.ToString());
            return _hotWorkPermitRepository.InsertTHotWorkPermits(thotWorkPermits);
        }

        public int UpdateTHotWorkPermits(THotWorkPermits thotWorkPermits)
        {
            if (!string.IsNullOrEmpty(thotWorkPermits.DSSign1Signature.FileContent) || thotWorkPermits.DSSign1Signature.DigSignatureId > 0)
                thotWorkPermits.PermitRequestorSignatureId = _commonService.SaveDigitalFile(thotWorkPermits.DSSign1Signature, thotWorkPermits.ProjectId.ToString());

            if (!string.IsNullOrEmpty(thotWorkPermits.DSSign2Signature.FileContent) || thotWorkPermits.DSSign2Signature.DigSignatureId > 0)
                thotWorkPermits.PermitAuthorizedSignatureId = _commonService.SaveDigitalFile(thotWorkPermits.DSSign2Signature, thotWorkPermits.ProjectId.ToString());
            return _hotWorkPermitRepository.UpdateTHotWorkPermits(thotWorkPermits);
        }
        public int InsertTHotWorkItems(THotWorkItems tobjhotworkitem)
        {
            return _hotWorkPermitRepository.Insert_THotWorkItems(tobjhotworkitem);
        }
        public bool DeleteHWPDrawings(int HotWorkPermitId, string fileIds)
        {
            return _hotWorkPermitRepository.DeleteHWPDrawings(HotWorkPermitId, fileIds);
        }
        #endregion
    }
}
