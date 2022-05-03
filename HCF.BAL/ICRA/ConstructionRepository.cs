//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace HCF.BAL
//{
//    public static class ConstructionRepository
//    {
//        #region Construction Type AND Construction Activity

//        public static IEnumerable<ConstructionType> GetConstructionType()
//        {
//            return DAL.ConstructionRepository.GetConstructionType();
//        }

//        public static ConstructionType GetConstructionType(int? ConstructionTypeId)
//        {
//            return DAL.ConstructionRepository.GetConstructionType(ConstructionTypeId);
//        }

//        //public static ConstructionActivity GetConstructionActivity(int? ConstActivityId)
//        //{
//        //    return DAL.ConstructionRepository.GetConstructionActivity(ConstActivityId);
//        //}

//        public static int AddConstructionType(ConstructionType modelData)
//        {
//            return DAL.ConstructionRepository.AddConstructionType(modelData);
//        }

//        public static int AddConstuctionActivity(ConstructionActivity modelData)
//        {
//            return DAL.ConstructionRepository.AddConstuctionActivity(modelData);
//        }

//        #endregion

//        #region Construction Risk 

//        public static List<ConstructionRisk> GetConstructionRisk()
//        {
//            return DAL.ConstructionRepository.GetConstructionRisk();
//        }

//        public static ConstructionRisk GetConstructionRisk(int? constructionriskid)
//        {
//            return DAL.ConstructionRepository.GetConstructionRisk(constructionriskid);
//        }

//        public static int AddConstructionRisk(ConstructionRisk modelData)
//        {
//            return DAL.ConstructionRepository.AddConstructionRisk(modelData);
//        }


//        public static int AddRiskAreaToArea(int constructionRiskId, string riskAreaIds, int CreatedBy)
//        {
//            return DAL.ConstructionRepository.AddRiskAreaToArea(constructionRiskId, riskAreaIds, CreatedBy);
//        }

//        public static List<ConstRiskDeptMap> GetConstRiskDeptMap()
//        {
//            return DAL.ConstructionRepository.GetConstRiskDeptMap();
//        }

//        public static bool DeleteRiskAreaToArea(int riskAreaId)
//        {
//            return DAL.ConstructionRepository.DeleteRiskAreaToArea(riskAreaId);
//        }


//        #endregion

//        #region Construction Class 

//        public static List<ConstructionClass> GetConstructionClass()
//        {
//            return DAL.ConstructionRepository.GetConstructionClass();
//        }

//        public static ConstructionClass GetConstructionClass(int? constructionclassid)
//        {
//            return DAL.ConstructionRepository.GetConstructionClass(constructionclassid);
//        }

//        public static int AddConstructionClass(ConstructionClass modelData)
//        {
//            return DAL.ConstructionRepository.AddConstructionClass(modelData);
//        }

//        public static List<ConstructionClassCategrory> GetConstructionClassCategory()
//        {
//            return DAL.ConstructionRepository.GetConstructionClassCategory();
//        }

//        public static int AddConstClassActivity(ConstructionClassActivity modelData)
//        {
//            return DAL.ConstructionRepository.AddConstClassActivity(modelData);
//        }
//        #endregion

//        #region ICRASteps
//        public static List<ICRASteps> GetICRASteps()
//        {
//            return DAL.ConstructionRepository.GetICRASteps();
//        }

//        public static ICRASteps GetICRASteps(int icrastepId)
//        {
//            return DAL.ConstructionRepository.GetICRASteps(icrastepId);
//        }

//        public static int AddICRASteps(ICRASteps modelData)
//        {
//            return DAL.ConstructionRepository.AddICRASteps(modelData);
//        }
//        #endregion

//        #region ICRA Inspection

//        public static List<TIcraLog> GetAllInspectionIcraSteps(int icraId, DateTime? fromdate = null, DateTime? todate = null, int? statusId = null, int? constructionrikId = null)
//        {
//            return DAL.ConstructionRepository.GetAllInspectionIcraSteps(icraId, fromdate, todate, statusId, constructionrikId);
//        }
//        public static AllPermitReport GetAllPermitCount(string from, string todate, string permittype)
//        {
//            return DAL.ConstructionRepository.GetAllPermitCount(from, todate, permittype);
//        }
//        public static TIcraLog GetInspectionIcraSteps(int? icraId)
//        {
//            return DAL.ConstructionRepository.GetInspectionIcraSteps(icraId);
//        }

//        public static int AddInspectionIcra(TIcraLog modelData)
//        {
//            return DAL.ConstructionRepository.AddInspectionIcra(modelData);
//        }

//        public static bool DeleteICRADrawings(int TicraId, string fileIds)
//        {
//            return DAL.ConstructionRepository.DeleteICRADrawings(TicraId, fileIds);
//        }
//        //public static int AddIcra(TIcraLog modelData, string permittype)
//        //{
//        //    modelData.IsICRA = modelData.TIcraSteps.Any(x => x.Comment != null);

//        //    if (!string.IsNullOrEmpty(modelData.DSPermitRequestBy.FileContent) || modelData.DSPermitRequestBy.DigSignatureId > 0)
//        //        modelData.PermitRequestBySignId = SaveDSPermitRequestBy(modelData.DSPermitRequestBy, modelData.ProjectNo);

//        //    if (!string.IsNullOrEmpty(modelData.DSPermitAuthorizedBy.FileContent) || modelData.DSPermitAuthorizedBy.DigSignatureId > 0)
//        //        modelData.PermitAuthorizedBySignId = SaveDSPermitAuthorizedBy(modelData.DSPermitAuthorizedBy, modelData.ProjectNo);

//        //    if (!string.IsNullOrEmpty(modelData.DSPermitReviewerBy.FileContent) || modelData.DSPermitReviewerBy.DigSignatureId > 0)
//        //        modelData.PermitReviewerBySignId = SaveDSPermitReviewerBy(modelData.DSPermitReviewerBy, modelData.ProjectNo);

//        //    var TicraId = AddInspectionIcra(modelData);
//        //    if (TicraId > 0)
//        //    {
//        //        foreach (var item in modelData.TIcraSteps)
//        //        {
//        //            item.TicraId = TicraId;
//        //            AddTICRASteps(item);
//        //        }
//        //        SaveAreaSurrounding(modelData.AreasSurroundings, TicraId);
//        //        // SaveTICRAFiles(modelData.TICRAFiles, TicraId);
//        //        SaveTICRAAttachments(modelData.TICRAFiles, TicraId);
//        //        if (!string.IsNullOrEmpty(modelData.LinkICRA) && !string.IsNullOrEmpty(modelData.CeilingPermitId))
//        //        {
//        //            TPermitMapping objmapping = new TPermitMapping();
//        //            objmapping.PermitId1 = modelData.CeilingPermitId;
//        //            objmapping.PermitType1 = Convert.ToInt32(Enums.PermitType.CeilingPermit);
//        //            objmapping.PermitId2 = Convert.ToString(TicraId);
//        //            objmapping.PermitType2 = Convert.ToInt32(Enums.PermitType.ICRA);
//        //            objmapping.CreatedBy = modelData.CreatedBy;

//        //            PermitRepository.InsertPermitMapping(objmapping);
//        //        }


//        //    }

//        //    if (modelData.TicraId == 0)
//        //    {
//        //        if (permittype == "2")
//        //        {
//        //            if (modelData.PermitRequestBy > 0 && modelData.IsNotifyEmailPermitReq)
//        //                Email.SendICRANotifyEmail(modelData.PermitRequestBy, modelData.PermitNo, permittype);
//        //            if (modelData.PermitAuthorizedBy.HasValue && modelData.PermitAuthorizedBy > 0 && modelData.IsNotifyEmailPermitAuth)
//        //                Email.SendICRANotifyEmail(modelData.PermitAuthorizedBy.Value, modelData.PermitNo, permittype);
//        //            if (modelData.PermitReviewerBy.HasValue && modelData.PermitReviewerBy > 0 && modelData.IsNotifyEmailPermitReviewer)
//        //                Email.SendICRANotifyEmail(modelData.PermitReviewerBy.Value, modelData.PermitNo, permittype);
//        //        }

//        //    }

//        //    return TicraId;
//        //}


//        #region Icra sign and areas

//        private static int SaveDSPermitRequestBy(DigitalSignature dSPermitRequestBy, string projectNo)
//        {
//            return SaveDigitalFile(dSPermitRequestBy, projectNo);
//        }

//        private static int SaveDSPermitAuthorizedBy(DigitalSignature dSPermitAuthorizedBy, string projectNo)
//        {
//            return SaveDigitalFile(dSPermitAuthorizedBy, projectNo);
//        }

//        private static int SaveDSPermitReviewerBy(DigitalSignature dSPermitReviewerBy, string projectNo)
//        {
//            return SaveDigitalFile(dSPermitReviewerBy, projectNo);
//        }


//        public static int SaveDigitalFile(DigitalSignature dSPermitAuthorizedBy, string projectNo)
//        {
//            //if (dSPermitAuthorizedBy != null)
//            //{
//            //    if (!string.IsNullOrEmpty(dSPermitAuthorizedBy.FileName) && !string.IsNullOrEmpty(dSPermitAuthorizedBy.FileContent))
//            //    {
//            //        dSPermitAuthorizedBy.FilePath = Utility.AmazonFileUpload.SaveFileUsingContent(dSPermitAuthorizedBy.FileContent,
//            //            dSPermitAuthorizedBy.FileName,
//            //            "DigitalSignPath", projectNo).FilePath;
//            //        dSPermitAuthorizedBy.DigSignatureId = DigitalSignRepository.Save(dSPermitAuthorizedBy);

//            //    }
//            //}
//            //return dSPermitAuthorizedBy.DigSignatureId;

//            if (dSPermitAuthorizedBy != null)
//            {
//                if (!string.IsNullOrEmpty(dSPermitAuthorizedBy.FileName) && !string.IsNullOrEmpty(dSPermitAuthorizedBy.FileContent))
//                {
//                    dSPermitAuthorizedBy.FilePath = Utility.AmazonFileUpload.SaveFileUsingContent(dSPermitAuthorizedBy.FileContent,
//                        dSPermitAuthorizedBy.FileName,
//                        "DigitalSignPath", projectNo).FilePath;
                    


//                }

//                if ((!string.IsNullOrEmpty(dSPermitAuthorizedBy.FileName) && !string.IsNullOrEmpty(dSPermitAuthorizedBy.FileContent)) && (dSPermitAuthorizedBy.DigSignatureId > 0))
//                {
//                    //donothing signature is already saved in this case at the time of save signature so remove duplicay 
//                }
//                else
//                {

//                    dSPermitAuthorizedBy.DigSignatureId = DAL.DigitalSignRepository.AddDigitalSignature(dSPermitAuthorizedBy);
//                }
//            }
//            return dSPermitAuthorizedBy.DigSignatureId;
//        }

//        private static void SaveAreaSurrounding(List<TICRAAreasNearBy> areasSurroundings, int ticraId)
//        {
//            foreach (var tICRAAreasNearBy in areasSurroundings)
//            {
//                tICRAAreasNearBy.TicraId = ticraId;
//                SaveAreaSurrounding(tICRAAreasNearBy);
//            }
//        }
//        public static void SaveTICRAAttachments(List<TICRAFiles> tICRAFiles, int tICRAid)
//        {
//            if (tICRAFiles != null)
//            {

//                foreach (var item in tICRAFiles.Where(x => x.AttachmentID > 0))
//                {

//                    item.TicraId = tICRAid;
//                    SaveTICRAFiles(item);

//                }

//            }
//        }
//        public static void SaveTICRAFiles(List<TICRAFiles> tICRAFiles, int tICRAid)
//        {
//            if (tICRAFiles != null)
//            {

//                foreach (var item in tICRAFiles.Where(x => x.AttachmentID > 0))
//                {
//                    FilesRepository file = DAL.DocumentsRepository.GetAttacheMentFile(item.AttachmentID);
//                    if (file != null)
//                    {
//                        item.FilePath = file.FilePath;
//                        item.FileName = file.FileName;
//                        item.TicraId = tICRAid;
//                        SaveTICRAFiles(item);
//                    }
//                }

//                foreach (var item in tICRAFiles.Where(x => x.AttachmentID == 0))
//                    if (!string.IsNullOrEmpty(item.FileName))
//                    {
//                        if (item.FileContent != null)
//                            item.FilePath = Utility.AmazonFileUpload.SaveFileUsingContent(item.FileContent, item.FileName, "TICRAFilesPath", tICRAid.ToString()).FilePath;
//                        item.TicraId = tICRAid;
//                        SaveTICRAFiles(item);
//                    }
//            }
//        }

//        #endregion



//        public static int AddTICRASteps(TIcraSteps modelData)
//        {
//            return DAL.ConstructionRepository.AddTICRASteps(modelData);
//        }


//        public static int SaveTICRAFiles(TICRAFiles item)
//        {
//            return DAL.ConstructionRepository.SaveTICRAFiles(item);
//        }

//        public static List<TICRAFiles> GetTICRAFiles(int icraid)
//        {
//            return DAL.ConstructionRepository.GetTICRAFiles(icraid);
//        }

//        public static List<Audit> GetIcraHistory(int icraId)
//        {
//            return DAL.ConstructionRepository.GetIcraHistory(icraId);
//        }

//        public static List<TIcraLog> GetIcras()
//        {
//            return DAL.ConstructionRepository.GetIcras(null);
//        }
//        public static List<ProjectType> GetProjectType()
//        {
//            return DAL.ConstructionRepository.GetProjectType();
//        }
//        public static TIcraLog GetIcras(int icraId)
//        {
//            return DAL.ConstructionRepository.GetIcras(icraId).FirstOrDefault();
//        }

//        public static List<TIcraLog> GetIcrasWithIlsm(string searchParam, bool? IsICRA = true)
//        {
//            return DAL.ConstructionRepository.GetIcrasWithIlsm(searchParam, IsICRA.Value);
//        }

//        #endregion

//        #region ICRA Risk Area 
//        public static List<ICRARiskArea> GetICRARiskAra()
//        {
//            return DAL.ConstructionRepository.GetICRARiskArea();
//        }

//        public static ICRARiskArea GetICRARiskAra(int riskAreaId)
//        {
//            return DAL.ConstructionRepository.GetICRARiskArea().Where(x => x.RiskAreaId == riskAreaId).FirstOrDefault();
//        }

//        public static int AddICRARiskArea(ICRARiskArea iCRARiskArea)
//        {
//            return DAL.ConstructionRepository.AddICRARiskArea(iCRARiskArea);
//        }

//        public static bool UpdateICRARiskAra(ICRARiskArea iCRARiskArea)
//        {
//            return DAL.ConstructionRepository.UpdateICRARiskArea(iCRARiskArea);
//        }


//        public static List<ICRARiskArea> GetICRARiskAraWithOutRisk()
//        {
//            return DAL.ConstructionRepository.GetICRARiskArea().Where(x => x.IsEmpty).ToList();
//        }
//        #endregion

//        #region ICRAMatixPrecautions
//        public static int AddICRAMatixPrecautions(int constructionRiskId, int constructionTypeId, string classIdss, int createdby)
//        {
//            return DAL.ConstructionRepository.AddICRAMatixPrecautions(constructionRiskId, constructionTypeId, classIdss, createdby);
//        }

//        public static List<ICRAMatixPrecautions> GetICRAMatixPrecautions()
//        {
//            return DAL.ConstructionRepository.GetICRAMatixPrecautions();
//        }

//        public static int SaveAreaSurrounding(TICRAAreasNearBy tICRAAreasNearBy)
//        {
//            return DAL.ConstructionRepository.SaveAreaSurrounding(tICRAAreasNearBy);
//        }

//        public static TIcraLog GetICRAObservationReport(int icraId)
//        {
//            return DAL.ObservationReport.GetTICRAReport(icraId);
//        }


//        public static TIcraLog GetICRAObservationReport(int icraId, int reportId)
//        {
//            return DAL.ObservationReport.GetIcraReport(icraId, reportId);
//        }

//        public static int SaveReport(TICRAReports observtionReport)
//        {
//            return DAL.ObservationReport.AddTICRAReport(observtionReport);
//        }

//        public static int SaveReportCheckPoints(TICRAReportsCheckPoints checkPoints)
//        {
//            return DAL.ObservationReport.AddTICRAReportCheckPoint(checkPoints);
//        }

//        public static int AddObservationReportCheckPoint(ICRAObsReportCheckPoints checkPoints)
//        {
//            return DAL.ObservationReport.AddObservationReportCheckPoint(checkPoints);
//        }


//        public static List<ICRAObsReportCheckPoints> GetReportCheckPoints()
//        {
//            return DAL.ObservationReport.GetReportCheckPoint();
//        }

//        public static ICRAObsReportCheckPoints GetReportCheckPoints(int checkPointId)
//        {
//            return DAL.ObservationReport.GetReportCheckPoint().Where(x => x.ICRAReportPointId == checkPointId).FirstOrDefault();
//        }

//        public static bool DeleteFile(int fileId)
//        {
//            return DAL.ObservationReport.DeleteFile(fileId);
//        }

//        #endregion

//        public static TICRAReports GetICRAProjectObservationReport(string projectIds)
//        {
//            return DAL.ObservationReport.GetICRAProjectObservationReport(projectIds);
//        }
//        public static string GetPermitNumber(int TICRAId, string permittype)
//        {
//            return DAL.ObservationReport.GetPermitNumber(TICRAId, permittype);
//        }
//        public static TICRAReports GetICRAProjectObservationReport(string projectIds, string reportId)
//        {
//            return DAL.ObservationReport.GetICRAProjectObservationReport(projectIds, reportId);
//        }

//        public static int SaveProjectReport(TICRAReports observtionReport)
//        {
//            return DAL.ObservationReport.SaveProjectReport(observtionReport);
//        }

//        public static List<TICRAReports> GetICRAProjectReport()
//        {
//            return DAL.ConstructionRepository.GetICRAProjectReport();
//        }
//    }
//}
