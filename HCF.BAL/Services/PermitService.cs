using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.BAL
{
   public  class PermitService : IPermitService
    {
        private readonly ICommonService _commonService;
        private readonly IPermitRepository _permitRepository;
        public PermitService(ICommonService commonService, IPermitRepository permitRepository)
        {
            _commonService = commonService;
            _permitRepository = permitRepository;
        }

        #region Ceiling Permit
        public  List<CeilingPermit> GetCeilingPermit()
        {
            return _permitRepository.GetCeilingPermit();
        }

        public  CeilingPermit GetCeilingPermit(int ceilingpermitId)
        {
            return _permitRepository.GetCeilingPermit(ceilingpermitId);
        }
        public  List<TCeilingPermitDetail> Get_PermitMappingForm()
        {
            return _permitRepository.Get_PermitMappingForm();
        }
        public  int AddCeilingPermit(CeilingPermit objCeilingpermit)
        {
            if (!string.IsNullOrEmpty(objCeilingpermit.DSRequesterSign.FileContent) || objCeilingpermit.DSRequesterSign.DigSignatureId > 0)
                objCeilingpermit.RequesterSignId = _commonService.SaveDigitalFile(objCeilingpermit.DSRequesterSign, "Ceilingpermit".ToString());

            if (!string.IsNullOrEmpty(objCeilingpermit.DSApproverSign.FileContent) || objCeilingpermit.DSApproverSign.DigSignatureId > 0)
                objCeilingpermit.ApproversignId = _commonService.SaveDigitalFile(objCeilingpermit.DSApproverSign, "Ceilingpermit".ToString());
            return _permitRepository.AddCeilingPermit(objCeilingpermit);
        }
        public  int InsertCPermitDetails(TCeilingPermitDetail TCeilingPermitDetail)
        {
            return _permitRepository.InsertCPermitDetails(TCeilingPermitDetail);
        }
        public  int AddCeilingPermitDetail(CeilingPermitDetail ceilingpermitdetail)
        {
            return _permitRepository.AddCeilingPermitDetail(ceilingpermitdetail);
        }

        public  bool DeleteCeilingPermitFiles(int ceilingPermitId, string fileIds)
        {
            return _permitRepository.DeleteCeilingPermitFiles(ceilingPermitId, fileIds);
        }
        public  bool DeleteCeilingDrawings(int ceilingPermitId, string fileIds)
        {
            return _permitRepository.DeleteCeilingDrawings(ceilingPermitId, fileIds);
        }
        #endregion

        #region LifeSafetyDevices

        public  TLifeSafetyDevicesForms AddLifeSafetyDevices(TLifeSafetyDevicesForms lifesafetydevices)
        {
            if (!string.IsNullOrEmpty(lifesafetydevices.PermitAuthorizedSignature.FileContent) || lifesafetydevices.PermitAuthorizedSignature.DigSignatureId > 0)
                lifesafetydevices.PermitAuthorizedSignatureId = _commonService.SaveDigitalFile(lifesafetydevices.PermitAuthorizedSignature, "DigitalSign".ToString());
            return _permitRepository.AddLifeSafetyDevices(lifesafetydevices);
        }

        public  int AddLifeSafetyDevice(LifeSafetyDeviceList lifesafetydevicelist)
        {
            return _permitRepository.AddLifeSafetyDeviceListl(lifesafetydevicelist);
        }

        public  List<TLifeSafetyDevicesForms> LifeSafetyDevicesForms()
        {
            return _permitRepository.GetLifeSafetyDevicesForms();
        }

        public  TLifeSafetyDevicesForms GetLifeSafetyFormByPemitNo(int permitId)
        {
            return LifeSafetyDevicesForms().Where(x => x.LsdFormNo == permitId).FirstOrDefault();
        }

        public  TLifeSafetyDevicesForms GetLifeSafetyForm(Guid formId)
        { return LifeSafetyDevicesForms().Where(x => x.LsdFormId == formId).FirstOrDefault(); }


        public  bool DeleteLSDDrawings(int lsdno, string fileIds)
        {
            return _permitRepository.DeleteLSDDrawings(lsdno, fileIds);
        }
        #endregion

        #region Method of Procedure [MOP]

        public  List<TMOP> GetMethodofProcedure()
        {
            return _permitRepository.GetMethodofProcedure(null);
        }

        public  TMOP GetMethodofProcedure(int id)
        {
            return _permitRepository.GetMethodofProcedure(id).FirstOrDefault();
        }

        public  List<MopAdditionalForms> GetMopAdditionalForms()
        {
            return _permitRepository.GetMopAdditionalForms();
        }

        public  int AddMethodofProcedure(TMOP mop)
        {

            if (!string.IsNullOrEmpty(mop.DSSign1Signature.FileContent) || mop.DSSign1Signature.DigSignatureId > 0)
                mop.ApproverSignatureId = _commonService.SaveDigitalFile(mop.DSSign1Signature, mop.ProjectId.ToString());
            if (!string.IsNullOrEmpty(mop.DSSign2Signature.FileContent) || mop.DSSign2Signature.DigSignatureId > 0)
                mop.RequesterSignatureId = _commonService.SaveDigitalFile(mop.DSSign2Signature, mop.ProjectId.ToString());



            return _permitRepository.AddMethodofProcedure(mop);
        }

        public  int AddTMopAdditionalForms(TMopAdditionalForms item)
        {
            return _permitRepository.AddTMopAdditionalForms(item);
        }
        public  int AddTMopAdditionalFormsRedirect(TMopAdditionalForms item)
        {
            return _permitRepository.AddTMopAdditionalFormsRedirect(item);
        }

        public  int AddTSystemImpactArea(TSystemImpactArea item)
        {
            return _permitRepository.AddTSystemImpactArea(item);
        }

        public  int AddTProjectContactList(TProjectContactList item)
        {
            return _permitRepository.AddTProjectContactList(item);
        }

        public  bool DeleteTMOPFiles(int tmopId, string fileIds)
        {
            return _permitRepository.DeleteTMOPFiles(tmopId, fileIds);
        }
        public  bool DeleteTMOPDrawing(int tmopId, string fileIds)
        {
            return _permitRepository.DeleteTMOPDrawing(tmopId, fileIds);
        }
     
        #endregion

        #region Fire System ByPass Permit [FSBP]


        public  List<TFireSystemByPassPermit> GetAllFSBPermit()
        {
            return _permitRepository.GetAllFSBPermit();
        }
        public  TFireSystemByPassPermit GetFSBPermit(int? tfsbPermitId)
        {
            return _permitRepository.GetFSBPermit(tfsbPermitId);
        }

        public  List<TFSBPBuildingDetails> GetAllBuildingFiresystem()
        {
            return _permitRepository.GetAllBuildingFiresystem();
        }
        public  int InsertFSBPermit(TFireSystemByPassPermit objTFireSystemByPassPermit)
        {
            if (!string.IsNullOrEmpty(objTFireSystemByPassPermit.DSFSBPApproverSign.FileContent) || objTFireSystemByPassPermit.DSFSBPApproverSign.DigSignatureId>0)
                objTFireSystemByPassPermit.ApproverSignId = _commonService.SaveDigitalFile(objTFireSystemByPassPermit.DSFSBPApproverSign, "FBSP");

            if (!string.IsNullOrEmpty(objTFireSystemByPassPermit.DSBypassEngActionSign.FileContent) || objTFireSystemByPassPermit.DSBypassEngActionSign.DigSignatureId>0)
                objTFireSystemByPassPermit.BypassEngActionSignId = _commonService.SaveDigitalFile(objTFireSystemByPassPermit.DSBypassEngActionSign, "FBSP");

            if (!string.IsNullOrEmpty(objTFireSystemByPassPermit.DSBypassReturnEngActionSign.FileContent) || objTFireSystemByPassPermit.DSBypassReturnEngActionSign.DigSignatureId>0)
                objTFireSystemByPassPermit.BypassReturnEngActionSignId = _commonService.SaveDigitalFile(objTFireSystemByPassPermit.DSBypassReturnEngActionSign, "FBSP");

            return _permitRepository.InsertFSBPermit(objTFireSystemByPassPermit);
        }

        public  int InsertFSBPermitDetails(TFSBPermitDetail objTFSBPermitDetail)
        {
            return _permitRepository.InsertFSBPermitDetails(objTFSBPermitDetail);
        }

        public  int InsertFSBPBuildingDetails(TFSBPBuildingDetails item)
        {
            return _permitRepository.InsertFSBPBuildingDetails(item);
        }
        public  bool DeleteFSBPDrawings(int TFSBPermitId, string fileIds)
        {
            return _permitRepository.DeleteFSBPDrawings(TFSBPermitId, fileIds);
        }
        public  bool DeleteFSBPFiles(int TFSBPermitId, string fileIds)
        {
            return _permitRepository.DeleteFSBPFiles(TFSBPermitId, fileIds);
        }
        
        #endregion

        #region Facilities Maintenance Occurrence [FMC]  

        public  List<TFacilitiesMaintenanceOccurrencePermit> GetAllFacilitiesMaintenanceOccurrence()
        {
            return _permitRepository.GetAllFacilitiesMaintenanceOccurrence();
      
        }

        public  List<Shift> GetShiftsAsync()
        {
            return  _permitRepository.GetShiftsAsync();

        }
        public  TFacilitiesMaintenanceOccurrencePermit GetFacilitiesMaintenanceOccurrence(int id)
        {
            return  _permitRepository.GetFacilitiesMaintenanceOccurrence(id);
        }

        public  List<ClassificationType> GetClassificationType()
        {
            return  _permitRepository.GetClassificationType();
        }
        public  TFacilitiesMaintenanceOccurrencePermit GetFacilitiesMaintenanceOccurrenceAsync(int id)
        {
            return  _permitRepository.GetFacilitiesMaintenanceOccurrenceAsync(id);
        }
        public   int AddFacilitiesMaintenanceOccurrenceAsync(TFacilitiesMaintenanceOccurrencePermit TFMC)
        {
            //if (!string.IsNullOrEmpty(TFMC.DSSign1Signature.FileContent))
            //    TFMC.RequesterSignId = CommonRepository.SaveDigitalFile(TFMC.DSSign1Signature, "FacilitiesMaintenanceOccurrencePermit".ToString());

            //if (!string.IsNullOrEmpty(TFMC.DSSign2Signature.FileContent))
            //    TFMC.ApproverSignId = CommonRepository.SaveDigitalFile(TFMC.DSSign2Signature, "FacilitiesMaintenanceOccurrencePermit".ToString());

            return  _permitRepository.AddFacilitiesMaintenanceOccurrenceAsync(TFMC);
        }

        public  bool DeletePermit(int Id,int PermitType)
        {
           
            return _permitRepository.DeletePermit(Id, PermitType);
        }

        public  bool DeleteFMCDrawings(int fmcid, string fileIds)
        {
            return _permitRepository.DeleteFMCDrawings(fmcid, fileIds);
        }
        #endregion

        #region PaperPermit
        public  List<PaperPermit> GetPaperPermit()
        {
            return _permitRepository.GetPaperPermit();
        }
        public  PaperPermit GetPaperPermitById(int? PermitId)
        {
            return _permitRepository.GetPaperPermitById(PermitId);
        }
        public  int AddPaperPermit(PaperPermit PaperPermit)
        {
           
            return _permitRepository.AddPaperPermit(PaperPermit);
        }
        public  bool Delete_TPaperPermitFiles(int permitid, string fileIds)
        {
            return _permitRepository.Delete_TPaperPermitFiles(permitid, fileIds);
        }
        #endregion

        #region AllPermit
        public  List<AllPermits> GetAllPermit(int? FilterBy = 30, int? PermitType=null,string Status=null,int? ProjectID=0)
        {
            return _permitRepository.GetAllPermit(FilterBy, PermitType,Status, ProjectID);
        }
        public  int InsertPermitMapping(TPermitMapping TPermitMapping)
        {
            return _permitRepository.InsertPermitMapping(TPermitMapping);
        }
        public  int AddMopFile(TFiles item)
        {
            return _permitRepository.AddMopFile(item);
        }
        #endregion

        #region Permit Settings
        public  List<PermitSetting> GetPermitWorkFlowSettings()
        {
            return _permitRepository.GetPermitWorkFlowSettings();
        }

        public  int AddPermitWorkFlow(PermitSetting PermitSetting)
        {
           
            return _permitRepository.AddPermitWorkFlow(PermitSetting);
        }
        public  int AddTPermitWorkFlowDetails(TPermitWorkFlowDetails TPermitWorkFlowDetails,string bucketname)
        {
            if (!string.IsNullOrEmpty(TPermitWorkFlowDetails.DSPermitSignature.FileContent) || TPermitWorkFlowDetails.DSPermitSignature.DigSignatureId > 0)
                TPermitWorkFlowDetails.LabelSignId = _commonService.SaveDigitalFile(TPermitWorkFlowDetails.DSPermitSignature, bucketname.ToString());

           

            return _permitRepository.AddTPermitWorkFlowDetails(TPermitWorkFlowDetails);
        }
        public  bool DeletePermitWorkFlow(int id, int PermitType)
        {
            return _permitRepository.DeletePermitWorkFlow(id, PermitType);
        }
        #endregion
        #region Asset Change Devices
        public List<AssetDevicePathSettings> GetAssetDevicePath()
        {
            return _permitRepository.GetAssetDevicePath();
        }
        public TAssetDeviceChangeForms AddAssetChangeDevices(TAssetDeviceChangeForms TAssetDeviceChangeForms)
        {
            
            return _permitRepository.AddAssetChangeDevices(TAssetDeviceChangeForms);
        }

        public int AddAsetChangeDeviceList(AssetDeviceList AssetDeviceList)
        {
            return _permitRepository.AddAsetChangeDeviceList(AssetDeviceList);
        }

        public List<TAssetDeviceChangeForms> AssetDeviceChangeForms()
        {
            return _permitRepository.GetAssetChangeDeviceForms();
        }
        public List<AssetDevices> GetAssetDevicesList()
        {
            return _permitRepository.GetAssetDevicesList();
        }
    
        public TAssetDeviceChangeForms GetAssetDeviceChangeFormsByPemitNo(int permitId)
        {
            return AssetDeviceChangeForms().Where(x => x.AdcFormNo == permitId).FirstOrDefault();
        }

        public TAssetDeviceChangeForms GetAssetDeviceForm(Guid formId)
        { return AssetDeviceChangeForms().Where(x => x.AdcFormId == formId).FirstOrDefault(); }
        public TAssetDeviceChangeForms GetAssetChangeDeviceFormsById(Guid formId)
        { return _permitRepository.GetAssetChangeDeviceFormsById(formId); }


        public bool DeleteADCDrawings(int lsdno, string fileIds)
        {
            return _permitRepository.DeleteADCDrawings(lsdno, fileIds);
        }
        public int AddAssetDevices(AssetDevices AssetDevices)
        {
            return _permitRepository.AddAssetDevices(AssetDevices);
        }
        #endregion
    }
}
