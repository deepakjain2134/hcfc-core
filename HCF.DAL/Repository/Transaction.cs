using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HCF.Utility;

namespace HCF.DAL
{
    public class Transaction : ITransaction
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IBinderRepository _binderRepository;
        private readonly IStepsRepository _stepsRepository;
        private readonly IEpRepository _epRepository;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IFloorAssetRepository _floorAssetRepository;
        private readonly IAssetsRepository _assetsRepository;
        private readonly IUsersRepository _usersRepository;      

        public Transaction(IAssetsRepository assetsRepository, IUsersRepository usersRepository,ISqlHelper sqlHelper, IFloorAssetRepository floorAssetRepository, IDocumentTypeRepository documentTypeRepository, IEpRepository epRepository, IBinderRepository binderRepository, IStepsRepository stepsRepository)
        {
            _assetsRepository = assetsRepository;
            _usersRepository = usersRepository;
            _floorAssetRepository = floorAssetRepository;
            _documentTypeRepository = documentTypeRepository;
            _epRepository = epRepository;
            _stepsRepository = stepsRepository;
            _sqlHelper = sqlHelper;
            _binderRepository = binderRepository;
        }

        #region Inspection 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectiondetails"></param>
        /// <returns></returns>
        public int AddInspectionDetails(TInspectionDetail inspectiondetails, string steps = "")
        {
            int newId;
            const string sql = StoredProcedures.Insert_InspectionDetail;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectionDetailId", inspectiondetails.InspectionDetailId);
                command.Parameters.AddWithValue("@ActivityId", inspectiondetails.ActivityId);
                command.Parameters.AddWithValue("@MainGoalId", inspectiondetails.MainGoalId);
                command.Parameters.AddWithValue("@steps", steps);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionfiles"></param>
        /// <returns></returns>
        public int AddInspectionFiles(TInspectionFiles inspectionfiles)
        {
            int newId;
            const string sql = StoredProcedures.Insert_InspectionFiles; // "Insert_InspectionFiles";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ActivityId", inspectionfiles.ActivityId);
                command.Parameters.AddWithValue("@FileName", inspectionfiles.FileName);
                command.Parameters.AddWithValue("@FilePath", inspectionfiles.FilePath);
                command.Parameters.AddWithValue("@Comment", inspectionfiles.Comment);
                command.Parameters.AddWithValue("@FileType", inspectionfiles.FileType);
                command.Parameters.AddWithValue("@StepsId", inspectionfiles.StepsId > 0 ? inspectionfiles.StepsId : null);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionDocs"></param>
        /// <returns></returns>
        public int AddInspectionDocs(InspectionEPDocs inspectionDocs)
        {

            int newId;
            string sql = StoredProcedures.Insert_InspectionDocs;// "Insert_InspectionDocs";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ActivityId", inspectionDocs.ActivityId);
                command.Parameters.AddWithValue("@UploadDocTypeId", inspectionDocs.UploadDocTypeId);
                //command.Parameters.AddWithValue("@AttachmentId", inspectionDocs.AttachmentId);
                command.Parameters.AddWithValue("@DocumentName", inspectionDocs.DocumentName);
                //command.Parameters.AddWithValue("@DocumentId", inspectionDocs.DocumentId);
                command.Parameters.AddWithValue("@FileId", inspectionDocs.FileId);
                command.Parameters.AddWithValue("@Path", inspectionDocs.Path);
                command.Parameters.AddWithValue("@ActivityType", inspectionDocs.ActivityType);
                command.Parameters.AddWithValue("@DoctypeId", inspectionDocs.DocTypeId > 0 ? inspectionDocs.DocTypeId : null);
                if (inspectionDocs.EffectiveDate != null)
                    command.Parameters.AddWithValue("@EffectiveDate", TimeZoneInfo.ConvertTimeToUtc(Conversion.ConvertToDateTime(inspectionDocs.EffectiveDate.Value)));
                if (inspectionDocs.InspectionId > 0 || inspectionDocs.InspectionId != null)
                    command.Parameters.AddWithValue("@InspectionId", inspectionDocs.InspectionId > 0 ? inspectionDocs.InspectionId : null);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Steps> GetStepsTransactionalRecords(int mode, int ePdetailId, int? floorAssetId, int? mainGoalId, int? inspectionDetailId, int? frequencyId)
        {
            List<Steps> lists;

            const string sql = StoredProcedures.Get_StepsTransactionalRecords; // "dbo.Get_StepsTransactionalRecords";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Mode", mode);
                command.Parameters.AddWithValue("@epdetailId", ePdetailId);
                command.Parameters.AddWithValue("@MainGoalId", mainGoalId);
                command.Parameters.AddWithValue("@FloorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@InspectionDetailId", inspectionDetailId);
                DataTable dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<Steps>(dt);
                if (mode == Convert.ToInt32(BDO.Enums.Mode.ASSET))
                {
                    foreach (var step in lists)
                        step.inspectionsteps = GetTinspectionStepsByFrequency(frequencyId, floorAssetId, step.StepsId, mode);
                }
            }
            return lists;
        }

        private List<InspectionSteps> GetTinspectionStepsByFrequency(int? frequencyId, int? floorAssetId, int? stepsId, int? activityType)
        {
            var steps = new List<InspectionSteps>();
            const string sql = StoredProcedures.Get_TinspectionStepsByFrequency;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@frequencyId", frequencyId);
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@stepsId", stepsId);
                command.Parameters.AddWithValue("@activityType", activityType);
                var dt = _sqlHelper.GetDataTable(command);
                steps = _sqlHelper.ConvertDataTable<InspectionSteps>(dt);
                foreach (InspectionSteps step in steps)
                    step.Steps = _stepsRepository.GetStep(step.StepsId);
            }
            return steps;
        }

        public List<Steps> GetStepsTransactionalRecords(int? inspectionDetailId)
        {
            List<Steps> lists;

            const string sql = StoredProcedures.Get_TranSteps; // "dbo.Get_StepsTransactionalRecords";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectionDetailId", inspectionDetailId);
                DataTable dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                {
                    lists = _sqlHelper.ConvertDataTable<Steps>(dt);
                    List<InspectionSteps> steps = _sqlHelper.ConvertDataTable<InspectionSteps>(dt);
                    foreach (var item in lists)
                    {
                        item.inspectionsteps = steps.Where(x => x.StepsId == item.StepsId).ToList();
                    }
                }

                lists = _sqlHelper.ConvertDataTable<Steps>(dt);

            }
            return lists;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="activityId"></param>
        /// <param name="subStatus"></param>
        /// <returns></returns>
        public bool UpdateInspectionSubstatus(int inspectionId, Guid activityId, string subStatus)
        {
            bool status;
            const string sql = StoredProcedures.Update_InspectionSubstatus; //"Insert_InspectionSteps";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inspectionId", inspectionId);
                command.Parameters.AddWithValue("@activityId", activityId);
                command.Parameters.AddWithValue("@subStatus", subStatus);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        #endregion

        #region TInspection EPSteps

        //public List<EPSteps> GetAllEpDocs(int userId, int epDetailId)
        //{
        //    List<EPSteps> objEpDocs = new List<EPSteps>();
        //    string sql = StoredProcedures.Get_UserDashboardData;// "Get_UserDashboardData";

        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@userId", userId);
        //        command.Parameters.AddWithValue("@epdetailId", epDetailId > 0 ? epDetailId : (object)null);
        //        var dt = _sqlHelper.GetDataTable(command);
        //        foreach (DataRow rows in dt.Rows)
        //        {
        //            EPSteps epdoc = new EPSteps
        //            {
        //                EPDetailId = Convert.ToInt32(rows["EPDetailId"].ToString()),
        //                Status = Convert.ToInt32(rows["Status"].ToString()),
        //                ScoreId = Convert.ToInt32(rows["ScoreId"].ToString()),
        //                InspectionId = Convert.ToInt32(rows["InspectionId"].ToString()),
        //                Score = rows["ScoreName"].ToString(),
        //                ScoreName = rows["ScoreName"].ToString(),
        //                SampleDocPath = rows["SampleDocPath"].ToString(),
        //                DocName = rows["DocName"].ToString(),
        //                StandardEP = rows["StandardEP"].ToString(),
        //                InspectionGroupId = Convert.ToInt32(rows["InspectionGroupId"].ToString()),
        //                DocInspectionGroupId = Convert.ToInt32(rows["DocInspectionGroupId"].ToString()),
        //                DoctypeId = rows["DoctypeId"].ToString() == null || rows["DoctypeId"].ToString() == "" ? default(int) : Convert.ToInt32(rows["DoctypeId"].ToString()),
        //                DOCStatus = Convert.ToInt32(rows["DOC"].ToString()),
        //                Description = rows["Description"].ToString(),
        //                IsDocRequired = Convert.ToBoolean(rows["IsDocRequired"].ToString())
        //            };
        //            if (epdoc.InspectionId > 0)
        //            {
        //                epdoc.EpTransStatus = InspectionRepository.EpTransStatus(epdoc.EPDetailId);
        //                epdoc.EpTranStatus = InspectionRepository.EpTransStatusInt(epdoc.EPDetailId);
        //            }
        //            else
        //            {
        //                epdoc.EpTransStatus = "P";
        //                epdoc.EpTranStatus = -1;
        //            }
        //            objEpDocs.Add(epdoc);
        //        }
        //    }
        //    return objEpDocs;
        //    //return objEpDocs.Where(x => x.IsDocRequired).ToList();
        //}

        #endregion

        #region TInspectionFiles

        private List<TInspectionFiles> GetInspectionFiles(Guid? activityId, int? floorAssetId, int? inspectionId)
        {
            return GetAllInspectionFiles(activityId, floorAssetId, inspectionId).Where(x => x.IsCurrent).ToList();
        }

        private List<TInspectionFiles> GetIlsmInspectionFiles(Guid? activityId, int? floorAssetId, int? inspectionId)
        {
            return GetAllInspectionFiles(activityId, floorAssetId, inspectionId).ToList();
        }

        private List<TInspectionFiles> GetAllInspectionFiles(Guid? activityId, int? floorAssetId, int? inspectionId)
        {
            List<TInspectionFiles> lists = new List<TInspectionFiles>();
            const string sql = Utility.StoredProcedures.Get_InspectionFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ActivityId", activityId);
                command.Parameters.AddWithValue("@FloorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@InspectionId", inspectionId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    lists = _sqlHelper.ConvertDataTable<TInspectionFiles>(dt);
            }
            return lists.ToList();
        }

        //public List<TInspectionFiles> GetInspectionFiles()
        //{
        //    return GetInspectionFiles(null, null, null);
        //}

        //public List<TInspectionFiles> GetInspectionFiles(int inspectionId)
        //{
        //    return GetInspectionFiles(null, null, inspectionId);
        //}

        public List<TInspectionFiles> GetInspectionFiles(Guid activityId)
        {
            return GetInspectionFiles(activityId, null, null);//.Where(x => x.ActivityId == ActivityId).ToList();
        }

        public List<TInspectionFiles> GetIlsmInspectionFiles(Guid activityId)
        {
            return GetIlsmInspectionFiles(activityId, null, null);//.Where(x => x.ActivityId == ActivityId).ToList();
        }

        public List<TInspectionFiles> GetInspectionAssetFiles(int floorAssetId, int inspectionId)
        {
            List<TInspectionFiles> lists = GetInspectionFiles(null, floorAssetId, inspectionId);
            return lists;//.Where(x => x.FloorAssetId == floorAssetId && x.IsCurrent == true).ToList();
        }

        public List<TInspectionFiles> GetInspectionEpFiles(int inspectionId)
        {
            List<TInspectionFiles> lists = GetInspectionFiles(null, null, inspectionId);
            return lists;//.Where(x => x.FloorAssetId == null && x.IsCurrent == true).ToList();
        }

        #endregion

        #region TInspectionEPDocs

        private List<InspectionEPDocs> GetInspectionsEPDocs(int? inspectionId, int? epDetailId)
        {
            var lists = new List<InspectionEPDocs>();
            string sql = StoredProcedures.Get_TInspectionEPDocs;// "dbo.DocumentReport";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inspectionId", inspectionId > 0 ? inspectionId : (object)null);
                command.Parameters.AddWithValue("@epDetailId", epDetailId > 0 ? epDetailId : (object)null);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var insDoc = new InspectionEPDocs
                        {
                            TInsDocs = Convert.ToInt32(row["TInsDocs"].ToString()),
                            ActivityId = Guid.Parse(row["ActivityId"].ToString()),
                            // DocReferenceId = row["DocReferenceId"].ToString()
                        };
                        if (!string.IsNullOrEmpty(row["DocTypeId"].ToString()))
                            insDoc.DocTypeId = Convert.ToInt32(row["DocTypeId"].ToString());
                        if (!string.IsNullOrEmpty(row["BinderId"].ToString()))
                            insDoc.BinderId = Convert.ToInt32(row["BinderId"].ToString());

                        if (!string.IsNullOrEmpty(row["DtEffectiveDate"].ToString()))
                            insDoc.DtEffectiveDate = Convert.ToDateTime(row["DtEffectiveDate"].ToString());
                        insDoc.CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString());
                        //insDoc.Version = row["Version"].ToString();
                        insDoc.DocumentName = row["DocumentName"].ToString();
                        if (!string.IsNullOrEmpty(row["ActivityType"].ToString()))
                            insDoc.ActivityType = Convert.ToInt32(row["ActivityType"].ToString());
                        insDoc.Path = row["Path"].ToString();

                        //if (!string.IsNullOrEmpty(row["AttachmentId"].ToString()))
                        //    insDoc.AttachmentId = Convert.ToInt32(row["AttachmentId"].ToString());

                        //if (!string.IsNullOrEmpty(row["DocumentId"].ToString()))
                        //    insDoc.DocumentId = Convert.ToInt32(row["DocumentId"].ToString());

                        if (!string.IsNullOrEmpty(row["FileId"].ToString()))
                            insDoc.FileId = Convert.ToInt32(row["FileId"].ToString());

                        if (!string.IsNullOrEmpty(row["IsDeleted"].ToString()))
                            insDoc.IsDeleted = Convert.ToBoolean(row["IsDeleted"].ToString());

                        if (insDoc.DocTypeId.HasValue)
                        {
                            insDoc.DocumentType = new DocumentType();
                            insDoc.DocTypeId = insDoc.DocTypeId.Value;
                            insDoc.DocumentType.Name = row["Name"].ToString();
                            insDoc.DocumentType.Path = row["SampleDocPath"].ToString();
                        }
                        insDoc.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                        if (!string.IsNullOrEmpty(row["ExpiredDate"].ToString()))
                        {
                            insDoc.ExpiredDate = Convert.ToDateTime(row["ExpiredDate"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["UploadDocTypeId"].ToString()))
                            insDoc.UploadDocTypeId = Convert.ToInt32(row["UploadDocTypeId"].ToString());

                        insDoc.UserProfile = new UserProfile
                        {
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            UserId = Convert.ToInt32(row["CreatedBy"].ToString()),
                        };
                        lists.Add(insDoc);
                    }
                }
            }
            return lists;
        }

        public List<InspectionEPDocs> GetInspectionEPDocs(int inspectionId, int epDetailId)
        {
            return GetInspectionsEPDocs(inspectionId, epDetailId);
        }

        public List<InspectionEPDocs> GetEpInspectionDoc(int epDetIlId)
        {
            return GetInspectionsEPDocs(null, epDetIlId);
        }

        //public List<InspectionEPDocs> GetInspectionDocs(int inspectionId)
        //{
        //    return GetInspectionsEPDocs(inspectionId, null);
        //}

        public List<InspectionEPDocs> GetInspectionEPDocs()
        {
            return GetInspectionsEPDocs(null, null);
        }

        public List<InspectionEPDocs> GetDocumentReport(int userId)
        {
            return GetUserInsDoc(userId);
        }

        public List<InspectionEPDocs> GetRelationEpInspectionDoc(int epDetIlId)
        {
            return GetRelationalEpInspectionDoc(epDetIlId);
        }

        public List<InspectionEPDocs> GetDocumentHistory(int fileId)
        {
            List<InspectionEPDocs> lists = new List<InspectionEPDocs>();
            string sql = StoredProcedures.Get_DocumentReport;// "dbo.DocumentReport";
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@fileId", fileId);
                DataTable dt = _sqlHelper.GetDataTable(command);
                List<UserProfile> users = _sqlHelper.ConvertDataTable<UserProfile>(dt);
                foreach (DataRow row in dt.Rows)
                {
                    InspectionEPDocs list =
                        new InspectionEPDocs
                        {
                            CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                            CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                            DtEffectiveDate = Convert.ToDateTime(row["DtEffectiveDate"].ToString()),
                            ExpiredDate = Convert.ToDateTime(row["DueDate"].ToString()),
                            ActivityId = Guid.Parse(row["ActivityId"].ToString())
                        };

                    if (!string.IsNullOrEmpty(row["UploadDocTypeId"].ToString()))
                    {
                        list.UploadDocTypeId = Convert.ToInt32(row["UploadDocTypeId"].ToString());
                    }

                    //if (!string.IsNullOrEmpty(row["DtEffectiveDate"].ToString()))
                    //{
                    //    list.DtEffectiveDateTimeSpan = Conversion.ConvertToTimeSpan(Convert.ToDateTime(row["DtEffectiveDate"].ToString()));
                    //}
                    //if (!string.IsNullOrEmpty(row["DueDate"].ToString()))
                    //{
                    //    list.ExpiredDateTimeSpan = Conversion.ConvertToTimeSpan(Convert.ToDateTime(row["DueDate"].ToString()));
                    //}
                    list.UserProfile = users.Where(x => x.UserId == list.CreatedBy).ToList().FirstOrDefault();
                    // list.EPDetail = EpRepository.GetEpByActivtyId(list.ActivityId);
                    list.EPDetail = _epRepository.GetEpDescription(Convert.ToInt32(row["EPDetailId"].ToString()));

                    if (!string.IsNullOrEmpty(row["docTypeId"].ToString()))
                    {
                        list.DocTypeId = Convert.ToInt32(row["docTypeId"].ToString());
                        list.DocumentType = _documentTypeRepository.GetDocumentType(list.DocTypeId.Value);
                    }
                    lists.Add(list);
                }
            }
            return lists.Distinct().ToList();
        }

        public List<EPDetails> GetDeficientEPs(int userId)
        {
            var lists = new List<EPDetails>();
            const string sql = StoredProcedures.Get_DeficientEPs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                var inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]);
                var inspectionActivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[6]);
                var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[6]);
                var tInspectionDetail = _sqlHelper.ConvertDataTable<TInspectionDetail>(ds.Tables[1]);
                var mainGoals = _sqlHelper.ConvertDataTable<MainGoal>(ds.Tables[1]);
                var steps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[4]);
                var tsteps = _sqlHelper.ConvertDataTable<InspectionSteps>(ds.Tables[4]);
                var workOrder = _sqlHelper.ConvertDataTable<WorkOrder>(ds.Tables[5]);

                var distinctEps = inspections.GroupBy(x => x.EPDetailId).Select(y => y.First());
                foreach (var item in distinctEps)
                {
                    var epDetail = _epRepository.GetEpDescription(item.EPDetailId);
                    var epInspections = inspections.Where(x => x.EPDetailId == item.EPDetailId).ToList();
                    foreach (var inspection in epInspections)
                    {
                        inspection.TInspectionActivity = inspectionActivity.Where(x => x.InspectionId == inspection.InspectionId).ToList();
                        foreach (var activity in inspection.TInspectionActivity)
                        {
                            activity.UserProfile = users.FirstOrDefault(x => x.UserId == activity.CreatedBy);
                            if (activity.FloorAssetId.HasValue)
                                activity.TFloorAssets = _floorAssetRepository.GetFloorAsset(activity.FloorAssetId.Value);
                            activity.TInspectionDetail = tInspectionDetail.Where(x => x.ActivityId == activity.ActivityId).ToList();
                            foreach (var inspectionDetail in activity.TInspectionDetail)
                            {
                                inspectionDetail.InspectionSteps = tsteps.Where(x => x.InspectionDetailId == inspectionDetail.InspectionDetailId).ToList();
                                inspectionDetail.MainGoal = new MainGoal();
                                inspectionDetail.MainGoal = mainGoals.FirstOrDefault(x => x.MainGoalId == inspectionDetail.MainGoalId);
                                foreach (var inspectionStep in inspectionDetail.InspectionSteps)
                                {
                                    inspectionStep.Steps = new Steps();
                                    inspectionStep.Steps = steps.FirstOrDefault(x => x.StepsId == inspectionStep.StepsId);
                                    if (inspectionStep.IssueId.HasValue)
                                    {
                                        inspectionStep.WorkOrders = workOrder.Where(x => x.IssueId == inspectionStep.IssueId).ToList();
                                    }
                                }
                            }
                        }
                        inspection.TInspectionFiles = GetInspectionEpFiles(inspection.InspectionId);
                    }
                    epDetail.Inspections = new List<Inspection>();
                    epDetail.Inspections = epInspections;
                    lists.Add(epDetail);
                }
            }
            return lists;
        }

        public List<InspectionEPDocs> GetUserInsDoc(int userId)
        {
            List<InspectionEPDocs> lists = new List<InspectionEPDocs>();
            string sql = StoredProcedures.Get_RepositoryDashboard;// "dbo.RepositoryDashboard";
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                DataTable dt = _sqlHelper.GetDataTable(command);
                var users = _sqlHelper.ConvertDataTable<UserProfile>(dt);
                foreach (DataRow row in dt.Rows)
                {
                    var list =
                        new InspectionEPDocs
                        {
                            CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                            CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                            DtEffectiveDate = Convert.ToDateTime(row["DtEffectiveDate"].ToString()),
                            ExpiredDate = Convert.ToDateTime(row["DueDate"].ToString()),
                            ActivityId = Guid.Parse(row["ActivityId"].ToString()),
                            Path = row["Path"].ToString(),
                            DocumentName = row["DocumentName"].ToString()
                        };

                    if (!string.IsNullOrEmpty(row["UploadDocTypeId"].ToString()))
                    {
                        list.UploadDocTypeId = Convert.ToInt32(row["UploadDocTypeId"].ToString());
                    }

                    //if (!string.IsNullOrEmpty(row["DtEffectiveDate"].ToString()))
                    //{
                    //    list.DtEffectiveDateTimeSpan = Conversion.ConvertToTimeSpan(Convert.ToDateTime(row["DtEffectiveDate"].ToString()));
                    //}
                    //if (!string.IsNullOrEmpty(row["DueDate"].ToString()))
                    //{
                    //    list.ExpiredDateTimeSpan = Conversion.ConvertToTimeSpan(Convert.ToDateTime(row["DueDate"].ToString()));
                    //}
                    list.UserProfile = users.Where(x => x.UserId == list.CreatedBy).ToList().FirstOrDefault();
                    if (!string.IsNullOrEmpty(row["EPDetailId"].ToString()))
                        list.EPDetail = _epRepository.GetEpDescription(Convert.ToInt32(row["EPDetailId"].ToString()));

                    if (!string.IsNullOrEmpty(row["AssetsId"].ToString()))
                        list.Assets = _assetsRepository.GetAsset(Convert.ToInt32(row["AssetsId"].ToString()));

                    if (!string.IsNullOrEmpty(row["docTypeId"].ToString()))
                    {
                        list.DocTypeId = Convert.ToInt32(row["docTypeId"].ToString());
                        list.DocumentType = _documentTypeRepository.GetDocumentType(list.DocTypeId.Value);
                    }
                    lists.Add(list);
                }
            }
            return lists.Distinct().ToList();
        }

        public List<InspectionEPDocs> GetEpDocByBinder(int binderId)
        {
            List<InspectionEPDocs> lists = new List<InspectionEPDocs>();
            List<EPsDocument> binderDocument = _binderRepository.GetBinderDocument(binderId,"",0);
            foreach (EPsDocument binder in binderDocument)
            {
                InspectionEPDocs doc = new InspectionEPDocs
                {
                    ActivityId = binder.ActivityId,
                    CreatedDate = binder.CreatedDate,
                    DtEffectiveDate = binder.DtEffectiveDate,
                    UserProfile = new UserProfile
                    {
                        UserId = binder.CreatedBy,
                        FirstName = binder.FirstName,
                        LastName = binder.LastName
                    },
                    EPDetail = new EPDetails
                    {
                        //StandardEp = binder.StandardEp,
                        EPDetailId = binder.EpdetailId,
                        EPNumber = binder.StandardEp.Split(',')[1],
                        Standard = new Standard { TJCStandard = binder.StandardEp.Split(',')[0] }

                    }
                };
                if (binder.BinderId.HasValue)
                {
                    doc.Binder = new Binders
                    {
                        BinderName = binder.BinderName,
                        BinderId = binder.BinderId.Value
                    };
                }
                if (binder.DocTypeId.HasValue)
                {
                    doc.DocumentType = new DocumentType
                    {
                        DocTypeId = binder.DocTypeId.Value,
                        Name = binder.Name
                    };
                }
                lists.Add(doc);
            }

            return lists.Distinct().ToList();
        }

        public List<InspectionEPDocs> GetInspectionDocs(Guid activityId)
        {
            return GetInspectionEPDocs().Where(x => x.ActivityId == activityId).ToList();
        }

        public List<EPsDocument> GetInspectionDocuments(int userId)
        {
            List<EPsDocument> lists = new List<EPsDocument>();
            const string sql = Utility.StoredProcedures.Get_InspectionDocuments;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<EPsDocument>(ds.Tables[0]);

            }
            return lists;
        }


        public List<DocumentType> GetPolicyBinders(int userId, int selectedUser, int? epdetailId)
        {
            List<DocumentType> lists = new List<DocumentType>();
            const string sql = StoredProcedures.Get_PolicyBinders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@selectedUser", selectedUser);
                using (var ds = _sqlHelper.GetDataSet(command))
                {
                    var epAssigness = _epRepository.GetEpAssignees();
                    if (ds != null)
                    {
                        lists = _sqlHelper.ConvertDataTable<DocumentType>(ds.Tables[0]);
                        var epDocuments = _sqlHelper.ConvertDataTable<EpDocuments>(ds.Tables[1]);
                        var standardEps = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[1]);
                        var inspectionEpDocs = _sqlHelper.ConvertDataTable<InspectionEPDocs>(ds.Tables[2]);
                        var users = _usersRepository.ConvertToUser(ds.Tables[2]); //_sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]);
                        var tInspectionActivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[2]);
                        //  var tDocInspections = _sqlHelper.ConvertDataTable<TDocInspections>(ds.Tables[3]);

                        // var standardEpswithOutDoc = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[4]);


                        foreach (var inspectionActivity in tInspectionActivity)
                        {
                            inspectionActivity.UserProfile = users.FirstOrDefault(x => x.UserId == inspectionActivity.CreatedBy);
                            inspectionActivity.InspectionEPDocs = inspectionEpDocs.FirstOrDefault(x => x.ActivityId == inspectionActivity.ActivityId);
                        }

                        foreach (var item in epDocuments)
                        {
                            //item.DateEffective = System.DateTime.UtcNow;
                            item.StandardEPs = standardEps.FirstOrDefault(x => x.EPDetailId == item.EPDetailId);

                        }

                        foreach (var documentType in lists)
                        {
                            documentType.DocUserProfiles = new List<UserProfile>();
                            documentType.EPDocument = epDocuments.Where(x => x.DocTypeId == documentType.DocTypeId).ToList();
                            documentType.TInspectionActivity = tInspectionActivity.Where(x => x.DocTypeId == documentType.DocTypeId).ToList();
                            //documentType.TDocInspections = tDocInspections.Where(x => x.DocTypeId == documentType.DocTypeId).ToList();

                            foreach (var data in documentType.EPDocument)
                            {
                                var epUsers = epAssigness.FirstOrDefault(x => x.EPDetailId == data.EPDetailId)?.EPUsers;
                                if (epUsers != null)
                                    documentType.DocUserProfiles.AddRange(epUsers);
                            }
                        }
                        //if (epdetailId.HasValue && epdetailId > 0)
                        //{
                        //    foreach (var item in standardEpswithOutDoc)
                        //    {
                        //        var documentType = new DocumentType
                        //        {
                        //            DocTypeId = 0,
                        //            TInspectionActivity = new List<TInspectionActivity>(),
                        //            TDocInspections = new List<TDocInspections>()
                        //        };
                        //        var objepdocs = new List<EpDocuments>();
                        //        var epDoc = new EpDocuments
                        //        {
                        //            DocTypeId = 0, EPDetailId = item.EPDetailId, StandardEPs = item
                        //        };
                        //        //epDoc.DateEffective = System.DateTime.UtcNow;
                        //        objepdocs.Add(epDoc);
                        //        documentType.EPDocument = new List<EpDocuments>();
                        //        documentType.EPDocument = objepdocs;
                        //        lists.Add(documentType);
                        //    }
                        //}
                    }
                }
            }
            return lists;
        }

        public DocumentType PolicyBinderHistory(int docTypeId)
        {
            var lists = new DocumentType();
            const string sql = StoredProcedures.Get_PolicyBinderHistory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@docTypeId", docTypeId);
                using (var ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null)
                    {
                        lists = _sqlHelper.ConvertDataTable<DocumentType>(ds.Tables[0]).FirstOrDefault();
                        var epDocuments = _sqlHelper.ConvertDataTable<EpDocuments>(ds.Tables[1]);
                        var standardEps = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[1]);
                        var inspectionEpDocs = _sqlHelper.ConvertDataTable<InspectionEPDocs>(ds.Tables[2]);
                        var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]);
                        var tInspectionActivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[2]);
                        var tDocInspections = _sqlHelper.ConvertDataTable<TDocInspections>(ds.Tables[3]);

                        foreach (var inspectionActivity in tInspectionActivity)
                        {
                            inspectionActivity.UserProfile = users.FirstOrDefault(x => x.UserId == inspectionActivity.CreatedBy);
                            inspectionActivity.InspectionEPDocs = inspectionEpDocs.FirstOrDefault(x => x.ActivityId == inspectionActivity.ActivityId);
                            if (inspectionActivity.EPDetailId != null && inspectionActivity.EPDetailId > 0)
                                inspectionActivity.StandardEps = standardEps.FirstOrDefault(x => x.EPDetailId == inspectionActivity.EPDetailId);
                        }
                        foreach (var item in epDocuments)
                        {
                            item.StandardEPs = standardEps.FirstOrDefault(x => x.EPDetailId == item.EPDetailId);
                        }
                        if (lists != null)
                        {
                            lists.EPDocument = epDocuments.Where(x => x.DocTypeId == lists.DocTypeId).ToList();
                            lists.TInspectionActivity = tInspectionActivity.Where(x => x.DocTypeId == lists.DocTypeId).ToList();
                            lists.TDocInspections = tDocInspections.Where(x => x.DocTypeId == lists.DocTypeId).ToList();
                        }
                        //foreach (var documentType in lists)
                        //{
                        //    documentType.EPDocuments = epDocuments.Where(x => x.DocTypeId == documentType.DocTypeId).ToList();
                        //    // documentType.InspectionEPDocs = inspectionEPDocs.Where(x => x.DocTypeId == documentType.DocTypeId).OrderByDescending(x => x.CreatedDate);
                        //    documentType.InspectionActivity = tInspectionActivity.Where(x => x.DocTypeId == documentType.DocTypeId).ToList();
                        //}

                    }

                }

            }
            return lists;
        }

        //public EPDetails GetCurrentEp(int epId)
        //{
        //    DataSet ds;
        //    var epDetails = new List<EPDetails>();
        //    var sql = StoredProcedures.Get_EPDetailsCurrentStaus;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@epId", epId);
        //        ds = _sqlHelper.GetDataSet(command);
        //        if (ds != null)
        //        {
        //            var docType = _sqlHelper.ConvertDataTable<DocumentType>(ds.Tables[0]);
        //            foreach (DataRow row in ds.Tables[1].Rows)
        //            {
        //                var ep = new EPDetails
        //                {
        //                    EPNumber = row["EPNumber"].ToString(),
        //                    Description = row["Description"].ToString(),
        //                    EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
        //                    StDescID = Convert.ToInt32(row["StDescID"].ToString()),
        //                    Priority = Convert.ToInt32(row["Priority"].ToString()),
        //                    IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString()),

        //                    Standard = new Standard
        //                    {
        //                        CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
        //                        StDescID = Convert.ToInt32(row["StDescID"].ToString()),
        //                        TJCStandard = row["TJCStandard"].ToString(),
        //                        Category = new Category
        //                        {
        //                            CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
        //                            Code = row["Code"].ToString()
        //                        }
        //                    },
        //                    Scores = new Score
        //                    {
        //                        Name = row["ScoreName"].ToString()
        //                    },
        //                    IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString()),
        //                    DocStatus = Convert.ToInt32(row["DocStatus"].ToString()),
        //                    Inspection = new Inspection
        //                    {
        //                        InspectionId = Convert.ToInt32(row["InspectionId"].ToString()),
        //                        InspectionGroupId = Convert.ToInt32(row["InspectionGroupId"].ToString()),
        //                        CreatedDate = string.IsNullOrEmpty(row["StartDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["StartDate"].ToString()),
        //                        StartDate = string.IsNullOrEmpty(row["StartDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["StartDate"].ToString()),
        //                        DueDate = string.IsNullOrEmpty(row["DueDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["DueDate"].ToString()),
        //                        GraceDate = string.IsNullOrEmpty(row["GraceDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["GraceDate"].ToString()),
        //                        Status = Convert.ToInt32(row["Status"].ToString()),
        //                        SubStatus = row["SubStatus"].ToString(),
        //                    },
        //                };

        //                if (docType != null)
        //                    ep.DocumentType = docType;

        //                if (!string.IsNullOrEmpty(row["EPStatus"].ToString()))
        //                    ep.EPStatus = Convert.ToInt16(row["EPStatus"].ToString());

        //                if (!string.IsNullOrEmpty(row["InitialInspDate"].ToString()))
        //                    ep.InitialInspDate = Convert.ToDateTime(row["InitialInspDate"].ToString());

        //                if (ep.Inspection.InspectionId > 0)
        //                    ep.EpTransStatus = InspectionRepository.EpTransStatus(ep.EPDetailId, ep.Inspection);
        //                else
        //                    ep.EpTransStatus = "P";

        //                ep.EPFrequency = new List<EPFrequency>(); //EPUsers(ep.EPDetailId);


        //                // set ep frequency
        //                if (!string.IsNullOrEmpty(row["FrequencyId"].ToString()))
        //                {
        //                    var epfreq = new EPFrequency
        //                    {
        //                        Frequency = new FrequencyMaster
        //                        {
        //                            FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
        //                            DisplayName = row["FrequencyDisplayName"].ToString(),
        //                            Days = Convert.ToInt32(row["Days"].ToString()),
        //                        },
        //                        FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),

        //                    };
        //                    ep.EPFrequency.Add(epfreq);
        //                }
        //                if (!string.IsNullOrEmpty(row["IsAssetsRequired"].ToString()))
        //                {
        //                    ep.Assets = new List<Assets>();
        //                    var assetExpression = "EPDetailId = '" + ep.EPDetailId + "'";
        //                    var AssetSortOrder = "Name ASC";
        //                    var drfoundRows = ds.Tables[2].Select(assetExpression, AssetSortOrder);
        //                    for (int i = 0; i < drfoundRows.Length; i++)
        //                    {
        //                        var asset = new Assets
        //                        {
        //                            Name = Convert.ToString(drfoundRows[i]["Name"]),
        //                            AssetId = Convert.ToInt32(drfoundRows[i]["AssetId"]),
        //                            IsRouteInsp = Conversion.TryCastBoolean(drfoundRows[i]["IsRouteInsp"])
        //                        };
        //                        ep.Assets.Add(asset);
        //                    }
        //                }

        //                ep.EPUsers = new List<UserProfile>(); //EPUsers(ep.EPDetailId);
        //                string expression = "EPDetailId = '" + ep.EPDetailId + "'";
        //                string sortOrder = "FirstName ASC";
        //                var foundRows = ds.Tables[3].Select(expression, sortOrder);
        //                for (int i = 0; i < foundRows.Length; i++)
        //                {
        //                    var user = new UserProfile
        //                    {
        //                        Email = Convert.ToString(foundRows[i]["Email"]),
        //                        FirstName = Convert.ToString(foundRows[i]["FirstName"]),
        //                        LastName = Convert.ToString(foundRows[i]["LastName"]),
        //                        UserId = Convert.ToInt32(foundRows[i]["UserId"])
        //                    };
        //                    ep.EPUsers.Add(user);
        //                }

        //                // set epBinders
        //                ep.Binders = new List<Binders>();
        //                if (!string.IsNullOrEmpty(row["BinderId"].ToString()))
        //                {
        //                    var binder = new Binders
        //                    {
        //                        BinderId = Convert.ToInt32(row["BinderId"]),
        //                        BinderName = row["BinderName"].ToString()
        //                    };
        //                    ep.Binders.Add(binder);
        //                }
        //                ep.IsCustomFrequency = Convert.ToBoolean(row["IsCustomFrequency"]);

        //                if (!string.IsNullOrEmpty(row["EffectiveDate"].ToString()))
        //                    ep.EffectiveDate = Convert.ToDateTime(row["EffectiveDate"].ToString());

        //                epDetails.Add(ep);

        //            }



        //        }
        //    }

        //    return epDetails.FirstOrDefault();
        //}

        public bool DeleteTInspectionFiles(int tinsFileId)
        {
            bool status;
            const string sql = StoredProcedures.Delete_TInspectionFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tinsFileId", tinsFileId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }


        #endregion

        #region Common

        internal void GetTfloorAssetsByEp(int epdetailId, EPDetails newEpDetails)
        {
            List<TFloorAssets> floorassets = new List<TFloorAssets>();
            string sql = StoredProcedures.Get_TfloorAssetsByEP;// "Get_TfloorAssetsByEP";
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetailId", epdetailId);
                DataSet ds = _sqlHelper.GetDataSet(command);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    TFloorAssets floorAsset = _floorAssetRepository.GetFloorAsset(Convert.ToInt32(row["FloorAssetsId"].ToString()));
                    floorassets.Add(floorAsset);
                }
            }
            newEpDetails.Inspection = new Inspection();
            List<TInspectionActivity> activity = new List<TInspectionActivity>();
            for (var j = 0; j < floorassets.Count; j++)
            {
                var tInspectionActivity = new TInspectionActivity
                {
                    ActivityType = 2,
                    TFloorAssets = floorassets[j],
                    Status = -1
                };
                activity.Add(tInspectionActivity);
            }

            newEpDetails.Inspection.TInspectionActivity = new List<TInspectionActivity>();
            newEpDetails.Inspection.TInspectionActivity = activity;
        }



        #endregion

        //public List<EpConfig> GetAllEpConfig()
        //{
        //    List<EpConfig> epConfig = new List<EpConfig>();
        //    const string sql = StoredProcedures.EpConfig_CRUD;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        epConfig = _sqlHelper.ConvertDataTable<EpConfig>(_sqlHelper.GetDataTable(command));
        //    }
        //    return epConfig;
        //}

        public int InsertUpdateEpConfig(EpConfig epconfig, int mode, string dates)
        {
            int newId;
            const string sql = StoredProcedures.EpConfig_CRUD;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClientNo", epconfig.ClientNo);
                command.Parameters.AddWithValue("@EPDetailId", epconfig.EPDetailId);
                command.Parameters.AddWithValue("@IsApplicable", epconfig.IsApplicable);
                command.Parameters.AddWithValue("@IsActive", epconfig.IsActiveEp);
                command.Parameters.AddWithValue("@InspectionDate", epconfig.InspectionDate);
                command.Parameters.AddWithValue("@InspectionGroupId", epconfig.InspectionGroupId);
                command.Parameters.AddWithValue("@CreatedBy", epconfig.CreatedBy);
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@dates", dates);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }


        public bool DeleteEpDocs(Guid activityId)
        {
            bool status;
            const string sql = StoredProcedures.Delete_EpDocs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }



        private List<InspectionEPDocs> GetRelationalEpInspectionDoc(int epDetailId)
        {
            var lists = new List<InspectionEPDocs>();
            string sql = StoredProcedures.Get_RelationalTInspectionEPDocs;// "dbo.DocumentReport";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epDetailId", epDetailId > 0 ? epDetailId : (object)null);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var insDoc = new InspectionEPDocs
                        {
                            TInsDocs = Convert.ToInt32(row["TInsDocs"].ToString()),
                            ActivityId = Guid.Parse(row["ActivityId"].ToString()),
                            // DocReferenceId = row["DocReferenceId"].ToString()
                        };
                        if (!string.IsNullOrEmpty(row["DocTypeId"].ToString()))
                            insDoc.DocTypeId = Convert.ToInt32(row["DocTypeId"].ToString());
                        if (!string.IsNullOrEmpty(row["BinderId"].ToString()))
                            insDoc.BinderId = Convert.ToInt32(row["BinderId"].ToString());

                        if (!string.IsNullOrEmpty(row["DtEffectiveDate"].ToString()))
                            insDoc.DtEffectiveDate = Convert.ToDateTime(row["DtEffectiveDate"].ToString());
                        insDoc.CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString());
                        //insDoc.Version = row["Version"].ToString();
                        insDoc.DocumentName = row["DocumentName"].ToString();
                        if (!string.IsNullOrEmpty(row["ActivityType"].ToString()))
                            insDoc.ActivityType = Convert.ToInt32(row["ActivityType"].ToString());
                        insDoc.Path = row["Path"].ToString();

                        //if (!string.IsNullOrEmpty(row["AttachmentId"].ToString()))
                        //    insDoc.AttachmentId = Convert.ToInt32(row["AttachmentId"].ToString());

                        //if (!string.IsNullOrEmpty(row["DocumentId"].ToString()))
                        //    insDoc.DocumentId = Convert.ToInt32(row["DocumentId"].ToString());

                        if (!string.IsNullOrEmpty(row["FileId"].ToString()))
                            insDoc.FileId = Convert.ToInt32(row["FileId"].ToString());

                        if (!string.IsNullOrEmpty(row["IsDeleted"].ToString()))
                            insDoc.IsDeleted = Convert.ToBoolean(row["IsDeleted"].ToString());

                        if (insDoc.DocTypeId.HasValue)
                        {
                            insDoc.DocumentType = new DocumentType();
                            insDoc.DocTypeId = insDoc.DocTypeId.Value;
                            insDoc.DocumentType.Name = row["Name"].ToString();
                            insDoc.DocumentType.Path = row["SampleDocPath"].ToString();
                        }
                        insDoc.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                        if (!string.IsNullOrEmpty(row["ExpiredDate"].ToString()))
                        {
                            insDoc.ExpiredDate = Convert.ToDateTime(row["ExpiredDate"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["UploadDocTypeId"].ToString()))
                            insDoc.UploadDocTypeId = Convert.ToInt32(row["UploadDocTypeId"].ToString());

                        insDoc.UserProfile = new UserProfile
                        {
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            UserId = Convert.ToInt32(row["CreatedBy"].ToString()),
                        };
                        lists.Add(insDoc);
                    }
                }
            }
            return lists;
        }


        public int AddTinspectionCampus(Buildings campus, int Inspectionid, int Epdetailid)
        {

            int newId;
            string sql = StoredProcedures.Insert_InspectionCampus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Epdetailid", Epdetailid);
                command.Parameters.AddWithValue("@Inspectionid", Inspectionid);
                command.Parameters.AddWithValue("@SiteId", campus.SiteId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }
    }
}
