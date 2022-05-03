using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.DAL
{
    public interface IPermitRepository
    {
        int AddCeilingPermit(CeilingPermit ceilingpermit);
        int AddCeilingPermitDetail(CeilingPermitDetail ceilingpermitdetail);
        int AddFacilitiesMaintenanceOccurrenceAsync(TFacilitiesMaintenanceOccurrencePermit TFMC);
        int AddLifeSafetyDeviceListl(LifeSafetyDeviceList lifesafetydevicelist);
        TLifeSafetyDevicesForms AddLifeSafetyDevices(TLifeSafetyDevicesForms lifesafetydevices);
        int AddMethodofProcedure(TMOP mop);
        int AddMopFile(TFiles item);
        int AddPaperPermit(PaperPermit PaperPermit);
        int AddPermitWorkFlow(PermitSetting PermitSetting);
        int AddTMopAdditionalForms(TMopAdditionalForms tMopAdditionalForms);
        int AddTMopAdditionalFormsRedirect(TMopAdditionalForms tMopAdditionalForms);
        int AddTPermitWorkFlowDetails(TPermitWorkFlowDetails TPermitWorkFlowDetails);
        int AddTProjectContactList(TProjectContactList item);
        int AddTSystemImpactArea(TSystemImpactArea item);
        bool DeleteCeilingDrawings(int ceilingPermitId, string fileIds);
        bool DeleteCeilingPermitFiles(int ceilingPermitId, string fileIds);
        bool DeleteFMCDrawings(int TfmcId, string fileIds);
        bool DeleteFSBPDrawings(int TFSBPermitId, string fileIds);
        bool DeleteFSBPFiles(int TFSBPermitId, string fileIds);
        bool DeleteLSDDrawings(int lsdno, string fileIds);
        bool DeleteLSDPFiles(int lsdno, string fileIds);
        bool DeletePermit(int Id, int PermitType);
        bool DeletePermitWorkFlow(int id, int PermitType);
        bool DeleteTMOPDrawing(int tmopId, string fileIds);
        bool DeleteTMOPFiles(int tmopId, string fileIds);
        bool Delete_TPaperPermitFiles(int permitid, string fileIds);
        List<TFSBPBuildingDetails> GetAllBuildingFiresystem();
       List<TFacilitiesMaintenanceOccurrencePermit> GetAllFacilitiesMaintenanceOccurrence();
        List<TFireSystemByPassPermit> GetAllFSBPermit();
        List<AllPermits> GetAllPermit(int? FilterBy = 30, int? PermitType = null, string Status = null, int? ProjectID = 0);
        TFSBPBuildingDetails GetBuildingFiresystemByID(TFSBPBuildingDetails objTFSBPBuildingDetails);
        List<CeilingPermit> GetCeilingPermit();
        CeilingPermit GetCeilingPermit(int? ceilingpermitId);
        List<ClassificationType> GetClassificationType();
        TFacilitiesMaintenanceOccurrencePermit GetFacilitiesMaintenanceOccurrence(int? id);
        TFacilitiesMaintenanceOccurrencePermit GetFacilitiesMaintenanceOccurrenceAsync(int? id);
        List<FireSystemType> GetFireSystemTypeDetails();
        TFireSystemByPassPermit GetFSBPermit(int? tfsbPermitId);
        List<TLifeSafetyDevicesForms> GetLifeSafetyDevicesForms();
        List<TMOP> GetMethodofProcedure(int? id);
        List<MopAdditionalForms> GetMopAdditionalForms();
        List<PaperPermit> GetPaperPermit();
        PaperPermit GetPaperPermitById(int? id);
        List<PermitSetting> GetPermitWorkFlowSettings();
        List<AssetDevicePathSettings> GetAssetDevicePath();
        List<Shift> GetShiftsAsync();
        List<TCeilingPermitDetail> Get_PermitMappingForm();
        int InsertCPermitDetails(TCeilingPermitDetail TCeilingPermitDetail);
        int InsertFSBPBuildingDetails(TFSBPBuildingDetails objTFSBPBuildingDetails);
        int InsertFSBPermit(TFireSystemByPassPermit objTFireSystemByPassPermit);
        int InsertFSBPermitDetails(TFSBPermitDetail objTFSBPermitDetail);
        int InsertPermitMapping(TPermitMapping TPermitMapping);
        TAssetDeviceChangeForms AddAssetChangeDevices(TAssetDeviceChangeForms AssetDeviceChangeForms);
        int AddAsetChangeDeviceList(AssetDeviceList AssetDeviceList);
        List<TAssetDeviceChangeForms> GetAssetChangeDeviceForms();
        TAssetDeviceChangeForms GetAssetChangeDeviceFormsById(Guid formId);
        List<AssetDevices> GetAssetDevicesList();
        bool DeleteADCDrawings(int adcno, string fileIds);
        int AddAssetDevices(AssetDevices AssetDevices);
    }
}