using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IPermitService
    {
        int AddCeilingPermit(CeilingPermit objCeilingpermit);
        int AddCeilingPermitDetail(CeilingPermitDetail ceilingpermitdetail);
        int AddFacilitiesMaintenanceOccurrenceAsync(TFacilitiesMaintenanceOccurrencePermit TFMC);
        int AddLifeSafetyDevice(LifeSafetyDeviceList lifesafetydevicelist);
        TLifeSafetyDevicesForms AddLifeSafetyDevices(TLifeSafetyDevicesForms lifesafetydevices);
        int AddMethodofProcedure(TMOP mop);
        int AddMopFile(TFiles item);
        int AddPaperPermit(PaperPermit PaperPermit);
        int AddPermitWorkFlow(PermitSetting PermitSetting);
        int AddTMopAdditionalForms(TMopAdditionalForms item);
        int AddTMopAdditionalFormsRedirect(TMopAdditionalForms item);
        int AddTPermitWorkFlowDetails(TPermitWorkFlowDetails TPermitWorkFlowDetails, string bucketname);
        int AddTProjectContactList(TProjectContactList item);
        int AddTSystemImpactArea(TSystemImpactArea item);
        bool DeleteCeilingDrawings(int ceilingPermitId, string fileIds);
        bool DeleteCeilingPermitFiles(int ceilingPermitId, string fileIds);
        bool DeleteFMCDrawings(int fmcid, string fileIds);
        bool DeleteFSBPDrawings(int TFSBPermitId, string fileIds);
        bool DeleteFSBPFiles(int TFSBPermitId, string fileIds);
        bool DeleteLSDDrawings(int lsdno, string fileIds);
        bool DeletePermit(int Id, int PermitType);
        bool DeletePermitWorkFlow(int id, int PermitType);
        bool DeleteTMOPDrawing(int tmopId, string fileIds);
        bool DeleteTMOPFiles(int tmopId, string fileIds);
        bool Delete_TPaperPermitFiles(int permitid, string fileIds);
        List<TFSBPBuildingDetails> GetAllBuildingFiresystem();
        List<TFacilitiesMaintenanceOccurrencePermit> GetAllFacilitiesMaintenanceOccurrence();
        List<TFireSystemByPassPermit> GetAllFSBPermit();
        List<AllPermits> GetAllPermit(int? FilterBy = 30, int? PermitType = null, string Status = null, int? ProjectID = 0);
        List<CeilingPermit> GetCeilingPermit();
        CeilingPermit GetCeilingPermit(int ceilingpermitId);
        List<ClassificationType> GetClassificationType();
        TFacilitiesMaintenanceOccurrencePermit GetFacilitiesMaintenanceOccurrence(int id);
        TFacilitiesMaintenanceOccurrencePermit GetFacilitiesMaintenanceOccurrenceAsync(int id);
        TFireSystemByPassPermit GetFSBPermit(int? tfsbPermitId);
        TLifeSafetyDevicesForms GetLifeSafetyForm(Guid formId);
        TLifeSafetyDevicesForms GetLifeSafetyFormByPemitNo(int permitId);
        List<TMOP> GetMethodofProcedure();
        TMOP GetMethodofProcedure(int id);
        List<MopAdditionalForms> GetMopAdditionalForms();
        List<PaperPermit> GetPaperPermit();
        PaperPermit GetPaperPermitById(int? PermitId);
        List<PermitSetting> GetPermitWorkFlowSettings();
        List<AssetDevicePathSettings> GetAssetDevicePath();
        List<Shift> GetShiftsAsync();
        List<TCeilingPermitDetail> Get_PermitMappingForm();
        int InsertCPermitDetails(TCeilingPermitDetail TCeilingPermitDetail);
        int InsertFSBPBuildingDetails(TFSBPBuildingDetails item);
        int InsertFSBPermit(TFireSystemByPassPermit objTFireSystemByPassPermit);
        int InsertFSBPermitDetails(TFSBPermitDetail objTFSBPermitDetail);
        int InsertPermitMapping(TPermitMapping TPermitMapping);
        List<TLifeSafetyDevicesForms> LifeSafetyDevicesForms();

        TAssetDeviceChangeForms AddAssetChangeDevices(TAssetDeviceChangeForms TAssetDeviceChangeForms);
        int AddAsetChangeDeviceList(AssetDeviceList AssetDeviceList);
        TAssetDeviceChangeForms GetAssetDeviceChangeFormsByPemitNo(int permitId);
        TAssetDeviceChangeForms GetAssetDeviceForm(Guid formId);
        TAssetDeviceChangeForms GetAssetChangeDeviceFormsById(Guid formId);
        bool DeleteADCDrawings(int lsdno, string fileIds);
        List<TAssetDeviceChangeForms> AssetDeviceChangeForms();
        int AddAssetDevices(AssetDevices AssetDevices);
        List<AssetDevices> GetAssetDevicesList();
    }
}