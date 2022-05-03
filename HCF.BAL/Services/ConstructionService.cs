using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL
{
    public class ConstructionService : IConstructionService
    {

        private readonly IEmailService _emailService;
        private readonly IPermitService _permitService;
        private readonly IObservationReport _IObservationReport;
        private readonly IConstructionRepository _constructionRepository;
        private readonly IDigitalSignRepository _digitalSignRepository;
        private readonly IDocumentsRepository _documentsRepository; private readonly IFileUpload _fileUpload;

        public ConstructionService(
            IFileUpload fileUpload,
            IDocumentsRepository documentsRepository,
           IObservationReport IObservationReport,
           IEmailService emailService,
           IPermitService permitService,
           IDigitalSignRepository digitalSignRepository,
           IConstructionRepository constructionRepository)
        {
            _fileUpload = fileUpload;
            _documentsRepository = documentsRepository;
            _constructionRepository = constructionRepository;
            _IObservationReport = IObservationReport;
            _emailService = emailService;
            _permitService = permitService;
            _digitalSignRepository = digitalSignRepository;
        }
        #region Construction Type AND Construction Activity

        public IEnumerable<ConstructionType> GetConstructionType()
        {
            return _constructionRepository.GetConstructionType();
        }

        public ConstructionType GetConstructionType(int? ConstructionTypeId)
        {
            return _constructionRepository.GetConstructionType(ConstructionTypeId);
        }

        //public  ConstructionActivity GetConstructionActivity(int? ConstActivityId)
        //{
        //    return _constructionRepository.GetConstructionActivity(ConstActivityId);
        //}

        public int AddConstructionType(ConstructionType modelData)
        {
            return _constructionRepository.AddConstructionType(modelData);
        }

        public int AddConstuctionActivity(ConstructionActivity modelData)
        {
            return _constructionRepository.AddConstuctionActivity(modelData);
        }

        #endregion

        #region Construction Risk 

        public List<ConstructionRisk> GetConstructionRisk()
        {
            return _constructionRepository.GetConstructionRisk();
        }

        public ConstructionRisk GetConstructionRisk(int? constructionriskid)
        {
            return _constructionRepository.GetConstructionRisk(constructionriskid);
        }

        public int AddConstructionRisk(ConstructionRisk modelData)
        {
            return _constructionRepository.AddConstructionRisk(modelData);
        }


        public int AddRiskAreaToArea(int constructionRiskId, string riskAreaIds, int CreatedBy)
        {
            return _constructionRepository.AddRiskAreaToArea(constructionRiskId, riskAreaIds, CreatedBy);
        }

        public List<ConstRiskDeptMap> GetConstRiskDeptMap()
        {
            return _constructionRepository.GetConstRiskDeptMap();
        }

        public bool DeleteRiskAreaToArea(int riskAreaId)
        {
            return _constructionRepository.DeleteRiskAreaToArea(riskAreaId);
        }


        #endregion

        #region Construction Class 

        public List<ConstructionClass> GetConstructionClass()
        {
            return _constructionRepository.GetConstructionClass();
        }

        public ConstructionClass GetConstructionClass(int? constructionclassid)
        {
            return _constructionRepository.GetConstructionClass(constructionclassid);
        }

        public int AddConstructionClass(ConstructionClass modelData)
        {
            return _constructionRepository.AddConstructionClass(modelData);
        }

        public List<ConstructionClassCategrory> GetConstructionClassCategory()
        {
            return _constructionRepository.GetConstructionClassCategory();
        }

        public int AddConstClassActivity(ConstructionClassActivity modelData)
        {
            return _constructionRepository.AddConstClassActivity(modelData);
        }
        #endregion

        #region ICRASteps
        public List<ICRASteps> GetICRASteps()
        {
            return _constructionRepository.GetICRASteps();
        }

        public ICRASteps GetICRASteps(int icrastepId)
        {
            return _constructionRepository.GetICRASteps(icrastepId);
        }

        public int AddICRASteps(ICRASteps modelData)
        {
            return _constructionRepository.AddICRASteps(modelData);
        }
        #endregion

        #region ICRA Inspection

        public List<TIcraLog> GetAllInspectionIcraSteps(int icraId, DateTime? fromdate = null, DateTime? todate = null, int? statusId = null, int? constructionrikId = null, int? version = null)
        {
            return _constructionRepository.GetAllInspectionIcraSteps(icraId, fromdate, todate, statusId, constructionrikId, version);
        }
        public AllPermitReport GetAllPermitCount(string from, string todate, string permittype)
        {
            return _constructionRepository.GetAllPermitCount(from, todate, permittype);
        }
        public TIcraLog GetInspectionIcraSteps(int? icraId)
        {
            return _constructionRepository.GetInspectionIcraSteps(icraId);
        }

        public int AddInspectionIcra(TIcraLog modelData)
        {
            return _constructionRepository.AddInspectionIcra(modelData);
        }

        public bool DeleteICRADrawings(int TicraId, string fileIds)
        {
            return _constructionRepository.DeleteICRADrawings(TicraId, fileIds);
        }
        public int AddIcra(TIcraLog modelData, string permittype)
        {
            modelData.IsICRA = modelData.TIcraSteps.Any(x => x.Comment != null);

            if (!string.IsNullOrEmpty(modelData.DSPermitRequestBy.FileContent) || modelData.DSPermitRequestBy.DigSignatureId > 0)
                modelData.PermitRequestBySignId = SaveDSPermitRequestBy(modelData.DSPermitRequestBy, modelData.ProjectNo);

            if (!string.IsNullOrEmpty(modelData.DSPermitAuthorizedBy.FileContent) || modelData.DSPermitAuthorizedBy.DigSignatureId > 0)
                modelData.PermitAuthorizedBySignId = SaveDSPermitAuthorizedBy(modelData.DSPermitAuthorizedBy, modelData.ProjectNo);

            if (!string.IsNullOrEmpty(modelData.DSPermitReviewerBy.FileContent) || modelData.DSPermitReviewerBy.DigSignatureId > 0)
                modelData.PermitReviewerBySignId = SaveDSPermitReviewerBy(modelData.DSPermitReviewerBy, modelData.ProjectNo);

            var TicraId = AddInspectionIcra(modelData);
            if (TicraId > 0)
            {
                foreach (var item in modelData.TIcraSteps)
                {
                    item.TicraId = TicraId;
                    AddTICRASteps(item);
                }
                SaveAreaSurrounding(modelData.AreasSurroundings, TicraId);
                // SaveTICRAFiles(modelData.TICRAFiles, TicraId);
                SaveTICRAAttachments(modelData.TICRAFiles, TicraId);
                if (!string.IsNullOrEmpty(modelData.LinkICRA) && !string.IsNullOrEmpty(modelData.CeilingPermitId))
                {
                    TPermitMapping objmapping = new TPermitMapping();
                    objmapping.PermitId1 = modelData.CeilingPermitId;
                    objmapping.PermitType1 = Convert.ToInt32(BDO.Enums.PermitType.CeilingPermit);
                    objmapping.PermitId2 = Convert.ToString(TicraId);
                    objmapping.PermitType2 = Convert.ToInt32(BDO.Enums.PermitType.ICRA);
                    objmapping.CreatedBy = modelData.CreatedBy;

                    _permitService.InsertPermitMapping(objmapping);
                }


            }

            //if (modelData.TicraId == 0)
            //{
            //    if (permittype == "2")
            //    {
            //        if (modelData.PermitRequestBy > 0 && modelData.IsNotifyEmailPermitReq)
            //            _emailService.SendICRANotifyEmail(modelData.PermitRequestBy, modelData.PermitNo, permittype);
            //        if (modelData.PermitAuthorizedBy.HasValue && modelData.PermitAuthorizedBy > 0 && modelData.IsNotifyEmailPermitAuth)
            //            _emailService.SendICRANotifyEmail(modelData.PermitAuthorizedBy.Value, modelData.PermitNo, permittype);
            //        if (modelData.PermitReviewerBy.HasValue && modelData.PermitReviewerBy > 0 && modelData.IsNotifyEmailPermitReviewer)
            //            _emailService.SendICRANotifyEmail(modelData.PermitReviewerBy.Value, modelData.PermitNo, permittype);
            //    }

            //}

            return TicraId;
        }


        #region Icra sign and areas

        private int SaveDSPermitRequestBy(DigitalSignature dSPermitRequestBy, string projectNo)
        {
            return SaveDigitalFile(dSPermitRequestBy, projectNo);
        }

        private int SaveDSPermitAuthorizedBy(DigitalSignature dSPermitAuthorizedBy, string projectNo)
        {
            return SaveDigitalFile(dSPermitAuthorizedBy, projectNo);
        }

        private int SaveDSPermitReviewerBy(DigitalSignature dSPermitReviewerBy, string projectNo)
        {
            return SaveDigitalFile(dSPermitReviewerBy, projectNo);
        }


        public int SaveDigitalFile(DigitalSignature dSPermitAuthorizedBy, string projectNo)
        {
            //if (dSPermitAuthorizedBy != null)
            //{
            //    if (!string.IsNullOrEmpty(dSPermitAuthorizedBy.FileName) && !string.IsNullOrEmpty(dSPermitAuthorizedBy.FileContent))
            //    {
            //        dSPermitAuthorizedBy.FilePath = Utility.AmazonFileUpload.SaveFileUsingContent(dSPermitAuthorizedBy.FileContent,
            //            dSPermitAuthorizedBy.FileName,
            //            "DigitalSignPath", projectNo).FilePath;
            //        dSPermitAuthorizedBy.DigSignatureId = DigitalSignRepository.Save(dSPermitAuthorizedBy);

            //    }
            //}
            //return dSPermitAuthorizedBy.DigSignatureId;

            if (dSPermitAuthorizedBy != null)
            {
                if (!string.IsNullOrEmpty(dSPermitAuthorizedBy.FileName) && !string.IsNullOrEmpty(dSPermitAuthorizedBy.FileContent))
                {
                    //dSPermitAuthorizedBy.FilePath = "";
                    dSPermitAuthorizedBy.FilePath = _fileUpload.SaveFileUsingContent(dSPermitAuthorizedBy.FileContent,
                        dSPermitAuthorizedBy.FileName,
                        "DigitalSignPath", projectNo).FilePath;



                }

                if ((!string.IsNullOrEmpty(dSPermitAuthorizedBy.FileName) && !string.IsNullOrEmpty(dSPermitAuthorizedBy.FileContent)) && (dSPermitAuthorizedBy.DigSignatureId > 0))
                {
                    //donothing signature is already saved in this case at the time of save signature so remove duplicay 
                }
                else
                {

                    dSPermitAuthorizedBy.DigSignatureId = _digitalSignRepository.AddDigitalSignature(dSPermitAuthorizedBy);
                }
            }
            return dSPermitAuthorizedBy.DigSignatureId;
        }

        private void SaveAreaSurrounding(List<TICRAAreasNearBy> areasSurroundings, int ticraId)
        {
            foreach (var tICRAAreasNearBy in areasSurroundings)
            {
                tICRAAreasNearBy.TicraId = ticraId;
                SaveAreaSurrounding(tICRAAreasNearBy);
            }
        }
        public void SaveTICRAAttachments(List<TICRAFiles> tICRAFiles, int tICRAid)
        {
            if (tICRAFiles != null)
            {

                foreach (var item in tICRAFiles.Where(x => x.AttachmentID > 0))
                {

                    item.TicraId = tICRAid;
                    SaveTICRAFiles(item);

                }

            }
        }
        public void SaveTICRAFiles(List<TICRAFiles> tICRAFiles, int tICRAid)
        {
            if (tICRAFiles != null)
            {

                foreach (var item in tICRAFiles.Where(x => x.AttachmentID > 0))
                {
                    FilesRepository file = _documentsRepository.GetAttacheMentFile(item.AttachmentID);
                    if (file != null)
                    {
                        item.FilePath = file.FilePath;
                        item.FileName = file.FileName;
                        item.TicraId = tICRAid;
                        SaveTICRAFiles(item);
                    }
                }

                foreach (var item in tICRAFiles.Where(x => x.AttachmentID == 0))
                    if (!string.IsNullOrEmpty(item.FileName))
                    {
                        if (item.FileContent != null)
                            item.FilePath = _fileUpload.SaveFileUsingContent(item.FileContent, item.FileName, "TICRAFilesPath", tICRAid.ToString()).FilePath;
                        item.TicraId = tICRAid;
                        SaveTICRAFiles(item);
                    }
            }
        }

        #endregion



        public int AddTICRASteps(TIcraSteps modelData)
        {
            return _constructionRepository.AddTICRASteps(modelData);
        }


        public int SaveTICRAFiles(TICRAFiles item)
        {
            return _constructionRepository.SaveTICRAFiles(item);
        }

        public List<TICRAFiles> GetTICRAFiles(int icraid)
        {
            return _constructionRepository.GetTICRAFiles(icraid);
        }

        public List<Audit> GetIcraHistory(int icraId)
        {
            return _constructionRepository.GetIcraHistory(icraId);
        }

        public List<TIcraLog> GetIcras()
        {
            return _constructionRepository.GetIcras(null);
        }
        public List<ProjectType> GetProjectType()
        {
            return _constructionRepository.GetProjectType();
        }
        public TIcraLog GetIcras(int icraId)
        {
            return _constructionRepository.GetIcras(icraId).FirstOrDefault();
        }

        public List<TIcraLog> GetIcrasWithIlsm(string searchParam, bool? IsICRA = true)
        {
            return _constructionRepository.GetIcrasWithIlsm(searchParam, IsICRA.Value);
        }

        #endregion

        #region ICRA Risk Area 
        public List<ICRARiskArea> GetICRARiskAra()
        {
            return _constructionRepository.GetICRARiskArea();
        }

        public ICRARiskArea GetICRARiskAra(int riskAreaId)
        {
            return _constructionRepository.GetICRARiskArea().Where(x => x.RiskAreaId == riskAreaId).FirstOrDefault();
        }

        public int AddICRARiskArea(ICRARiskArea iCRARiskArea)
        {
            return _constructionRepository.AddICRARiskArea(iCRARiskArea);
        }

        public bool UpdateICRARiskAra(ICRARiskArea iCRARiskArea)
        {
            return _constructionRepository.UpdateICRARiskArea(iCRARiskArea);
        }


        public List<ICRARiskArea> GetICRARiskAraWithOutRisk()
        {
            return _constructionRepository.GetICRARiskArea().Where(x => x.IsEmpty).ToList();
        }

        
        #endregion

        #region ICRAMatixPrecautions
        public int AddICRAMatixPrecautions(int constructionRiskId, int constructionTypeId, string classIdss, int createdby,int version)
        {
            return _constructionRepository.AddICRAMatixPrecautions(constructionRiskId, constructionTypeId, classIdss, createdby, version);
        }

        public List<ICRAMatixPrecautions> GetICRAMatixPrecautions()
        {
            return _constructionRepository.GetICRAMatixPrecautions();
        }

        public int SaveAreaSurrounding(TICRAAreasNearBy tICRAAreasNearBy)
        {
            return _constructionRepository.SaveAreaSurrounding(tICRAAreasNearBy);
        }

        public TIcraLog GetICRAObservationReport(int icraId)
        {
            return _IObservationReport.GetTICRAReport(icraId);
        }


        public TIcraLog GetICRAObservationReport(int icraId, int reportId)
        {
            return _IObservationReport.GetIcraReport(icraId, reportId);
        }

        public int SaveReport(TICRAReports observtionReport, int userId)
        {
            //if (!string.IsNullOrEmpty(observtionReport.DSContractorSignId.FileContent) || observtionReport.DSContractorSignId.DigSignatureId > 0)
            //    observtionReport.ContractorSignId = SaveContractorSign(observtionReport.DSContractorSignId,userId);

            //if (!string.IsNullOrEmpty(observtionReport.DSObserverSignId.FileContent) || observtionReport.DSObserverSignId.DigSignatureId > 0)
            //    observtionReport.ObserverSignId = SaveObserverSign(observtionReport.DSObserverSignId, userId);

            observtionReport.ContractorSignId = observtionReport.DSContractorSignId.DigSignatureId > 0 ? observtionReport.DSContractorSignId.DigSignatureId : null;
            observtionReport.ObserverSignId = observtionReport.DSObserverSignId.DigSignatureId > 0 ? observtionReport.DSObserverSignId.DigSignatureId : null;

            var TICRAReportId = _IObservationReport.AddTICRAReport(observtionReport);
            return TICRAReportId;
            //return _IObservationReport.AddTICRAReport(observtionReport);
        }

        public int SaveReportCheckPoints(TICRAReportsCheckPoints checkPoints)
        {
            return _IObservationReport.AddTICRAReportCheckPoint(checkPoints);
        }

        public int AddObservationReportCheckPoint(ICRAObsReportCheckPoints checkPoints)
        {
            return _IObservationReport.AddObservationReportCheckPoint(checkPoints);
        }


        public List<ICRAObsReportCheckPoints> GetReportCheckPoints()
        {
            return _IObservationReport.GetReportCheckPoint();
        }

        public ICRAObsReportCheckPoints GetReportCheckPoints(int checkPointId)
        {
            return _IObservationReport.GetReportCheckPoint().Where(x => x.ICRAReportPointId == checkPointId).FirstOrDefault();
        }

        public bool DeleteFile(int fileId)
        {
            return _IObservationReport.DeleteFile(fileId);
        }

        #endregion


        #region Observation Reports 

        public TICRAReports GetICRAProjectObservationReport(string projectIds)
        {
            return _IObservationReport.GetICRAProjectObservationReport(projectIds);
        }
        public string GetPermitNumber(int TICRAId, string permittype)
        {
            return _IObservationReport.GetPermitNumber(TICRAId, permittype);
        }
        public TICRAReports GetICRAProjectObservationReport(string projectIds, string reportId)
        {
            return _IObservationReport.GetICRAProjectObservationReport(projectIds, reportId);
        }

        public int SaveProjectReport(TICRAReports observtionReport, int userId)
        {
            //if (!string.IsNullOrEmpty(observtionReport.DSContractorSignId.FileContent) || observtionReport.DSContractorSignId.DigSignatureId > 0)
            //    observtionReport.ContractorSignId = SaveContractorSign(observtionReport.DSContractorSignId, userId);

            //if (!string.IsNullOrEmpty(observtionReport.DSObserverSignId.FileContent) || observtionReport.DSObserverSignId.DigSignatureId > 0)
            //    observtionReport.ObserverSignId = SaveObserverSign(observtionReport.DSObserverSignId, userId);
            observtionReport.ContractorSignId = observtionReport.DSContractorSignId.DigSignatureId > 0 ? observtionReport.DSContractorSignId.DigSignatureId : null;
            observtionReport.ObserverSignId = observtionReport.DSObserverSignId.DigSignatureId > 0 ? observtionReport.DSObserverSignId.DigSignatureId : null;

            var TICRAReportId = _IObservationReport.SaveProjectReport(observtionReport);
            return TICRAReportId;
        }

        public List<TICRAReports> GetICRAProjectReport()
        {
            return _constructionRepository.GetICRAProjectReport();
        }

        //private int SaveObserverSign(DigitalSignature dSContractorSignId, int userId)
        //{
        //    if (dSContractorSignId != null)
        //    {
        //        if (!string.IsNullOrEmpty(dSContractorSignId.FileName) && !string.IsNullOrEmpty(dSContractorSignId.FileContent))
        //        {
        //            // dSContractorSignId.FilePath = "";
        //            dSContractorSignId.CreatedBy = userId;
        //            dSContractorSignId.UserId = userId;
        //            dSContractorSignId.FilePath = _fileUpload.SaveFileUsingContent(dSContractorSignId.FileContent,
        //                dSContractorSignId.FileName,
        //                "DigitalSignPath", "TICRAObservationReport").FilePath;
        //        }
        //        if ((!string.IsNullOrEmpty(dSContractorSignId.FileName) && !string.IsNullOrEmpty(dSContractorSignId.FileContent)) && (dSContractorSignId.DigSignatureId > 0))
        //        {
        //            //donothing signature is already saved in this case at the time of save signature so remove duplicay 
        //        }
        //        else
        //        {
        //            dSContractorSignId.DigSignatureId = _digitalSignRepository.AddDigitalSignature(dSContractorSignId);
        //        }
        //    }
        //    return dSContractorSignId.DigSignatureId;
        //}

        //private int SaveContractorSign(DigitalSignature dSObserverSignId , int userId)
        //{
        //    if (dSObserverSignId != null)
        //    {
        //        if (!string.IsNullOrEmpty(dSObserverSignId.FileName) && !string.IsNullOrEmpty(dSObserverSignId.FileContent))
        //        {
        //            //dSObserverSignId.FilePath = "";
        //            dSObserverSignId.CreatedBy = userId;
        //            dSObserverSignId.UserId = userId;
        //            dSObserverSignId.FilePath = _fileUpload.SaveFileUsingContent(dSObserverSignId.FileContent,
        //                 dSObserverSignId.FileName,
        //                 "DigitalSignPath", "TICRAObservationReport").FilePath;
        //        }

        //        if ((!string.IsNullOrEmpty(dSObserverSignId.FileName) && !string.IsNullOrEmpty(dSObserverSignId.FileContent)) && (dSObserverSignId.DigSignatureId > 0))
        //        {
        //            //donothing signature is already saved in this case at the time of save signature so remove duplicay 
        //        }
        //        else
        //        {
        //            dSObserverSignId.DigSignatureId = _digitalSignRepository.AddDigitalSignature(dSObserverSignId);
        //        }
        //    }
        //    return dSObserverSignId.DigSignatureId;
        //}

        public bool SaveObservationReport(TICRAReports newTICRAReports)
        {
            return _IObservationReport.SaveObservationReport(newTICRAReports); ;
        }

        #endregion
    }
}
