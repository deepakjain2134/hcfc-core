using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCF.BDO;

using HCF.Utility;

namespace HCF.DAL
{
    public  class PermitRepository : IPermitRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ITFilesRepository _tFilesRepository;
        private readonly ICommonRepository _commonRepository;
        public PermitRepository(ISqlHelper sqlHelper, ITFilesRepository tFilesRepository, ICommonRepository commonRepository)
        {
            _sqlHelper = sqlHelper;
            _tFilesRepository = tFilesRepository;
            _commonRepository = commonRepository;
        }

        #region Ceiling Permit
        public  int AddCeilingPermit(CeilingPermit ceilingpermit)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Ceilingpermit;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CeilingPermitId", ceilingpermit.CeilingPermitId);
                command.Parameters.AddWithValue("@EffectiveDate", ceilingpermit.EffectiveDate);
                command.Parameters.AddWithValue("@Number", ceilingpermit.Number);
                command.Parameters.AddWithValue("@Pages", ceilingpermit.Pages);
                command.Parameters.AddWithValue("@WorkArea", ceilingpermit.WorkArea);
                command.Parameters.AddWithValue("@Scope", ceilingpermit.Scope);
                command.Parameters.AddWithValue("@StartDate", ceilingpermit.StartDate);
                command.Parameters.AddWithValue("@CompletionDate", ceilingpermit.CompletionDate);
                command.Parameters.AddWithValue("@Responsible", ceilingpermit.Responsible);
                command.Parameters.AddWithValue("@TypeofSealant", ceilingpermit.TypeofSealant);
                command.Parameters.AddWithValue("@ULApproved", ceilingpermit.ULApproved);
                command.Parameters.AddWithValue("@Requestedby", ceilingpermit.Requestedby);
                command.Parameters.AddWithValue("@RequestedDept", ceilingpermit.RequestedDept);
                command.Parameters.AddWithValue("@RequesterSignId", ceilingpermit.RequesterSignId);
                command.Parameters.AddWithValue("@PhoneNo", ceilingpermit.PhoneNo);
                command.Parameters.AddWithValue("@RequestedDate", ceilingpermit.RequestedDate);
                command.Parameters.AddWithValue("@AdditionalItems", ceilingpermit.AdditionalItems);
                command.Parameters.AddWithValue("@ApproverId", ceilingpermit.ApproverId);
                command.Parameters.AddWithValue("@ApproverSignId", ceilingpermit.ApproversignId);
                command.Parameters.AddWithValue("@ApprovedDate", ceilingpermit.ApprovedDate);
                command.Parameters.AddWithValue("@Status", ceilingpermit.Status);
                command.Parameters.AddWithValue("@Isverified", ceilingpermit.IsVerified);
                command.Parameters.AddWithValue("@FinalInspectionBy", ceilingpermit.FinalInspectionBy);
                command.Parameters.AddWithValue("@InspectionDate", ceilingpermit.InspectionDate);
                command.Parameters.AddWithValue("@CreatedBy", ceilingpermit.CreatedBy);
                command.Parameters.AddWithValue("@IsPenetrationStructure", ceilingpermit.IsPenetrationStructure);
                command.Parameters.AddWithValue("@IsPenetrationsVerified", ceilingpermit.IsPenetrationsVerified);
                command.Parameters.AddWithValue("@Reason", ceilingpermit.Reason);
                command.Parameters.AddWithValue("@TFileIds", ceilingpermit.TFileIds);
                command.Parameters.AddWithValue("@EmailAddress", ceilingpermit.Email);
                command.Parameters.AddWithValue("@BuildingId", ceilingpermit.BuildingId);
                command.Parameters.AddWithValue("@BuildingName", ceilingpermit.BuildingName);
                command.Parameters.AddWithValue("@FloorId", ceilingpermit.FloorId);
                command.Parameters.AddWithValue("@FloorName", ceilingpermit.FloorName);
                command.Parameters.AddWithValue("@Zones", ceilingpermit.Zones);
                command.Parameters.AddWithValue("@ProjectId", ceilingpermit.ProjectId);
                command.Parameters.AddWithValue("@TDrawingFields", ceilingpermit.TDrawingFields);
                command.Parameters.AddWithValue("@StartTime", ceilingpermit.StartTime);
                command.Parameters.AddWithValue("@EndTime", ceilingpermit.EndTime);
                command.Parameters.AddWithValue("@Comment", ceilingpermit.Comment);
                //command.Parameters.AddWithValue("@TDrawingViewerId", ceilingpermit.TDrawingViewerId.TrimEnd(','));
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int AddCeilingPermitDetail(CeilingPermitDetail ceilingpermitdetail)
        {
            int newId;
            const string sql = StoredProcedures.Insert_CeilingPermitDetail;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CeilingPermitDetailId", ceilingpermitdetail.CeilingPermitDetailId);
                command.Parameters.AddWithValue("@CeilingPermitId", ceilingpermitdetail.CeilingPermitId);
                command.Parameters.AddWithValue("@RevisedDate", ceilingpermitdetail.RevisedDate);
                command.Parameters.AddWithValue("@ReviewedDate", ceilingpermitdetail.ReviewedDate);
                command.Parameters.AddWithValue("@PermitReviewBy", ceilingpermitdetail.PermitReviewBy);
                command.Parameters.AddWithValue("@PermitReviewBySignId", ceilingpermitdetail.PermitReviewBySignId);
                command.Parameters.AddWithValue("@UpdatedBy", ceilingpermitdetail.UpdatedBy);
                command.Parameters.AddWithValue("@UpdatedDate", ceilingpermitdetail.UpdatedDate);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public List<CeilingPermit> GetCeilingPermit()
        {
            List<CeilingPermit> ceilingPermit = new List<CeilingPermit>();
            var projects = new List<TIcraProject>();
            const string sql = Utility.StoredProcedures.Get_Ceilingpermit;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count>0)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ceilingPermit = _sqlHelper.ConvertDataTable<CeilingPermit>(ds.Tables[0]);
                    }
                    if (ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
                    {
                         projects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[7]);
                    }

                    List<TPermitWorkFlowDetails> TPermitWorkFlowDetails = new List<TPermitWorkFlowDetails>();
                    if (ds.Tables[9] != null && ds.Tables[9].Rows.Count > 0)
                    {
                        TPermitWorkFlowDetails= _sqlHelper.ConvertDataTable<TPermitWorkFlowDetails>(ds.Tables[9]);
                    }
                        foreach (var item in ceilingPermit)
                    {
                        item.Project = projects.FirstOrDefault(x => x.ProjectId == item.ProjectId);

                        if (TPermitWorkFlowDetails != null && item != null)
                        {
                            item.TPermitWorkFlowDetails = TPermitWorkFlowDetails.Where(x => x.PermitNumber == item.PermitNo).ToList();

                        }
                    }

                }

            }
            //using (var command = new SqlCommand(sql))
            //{
            //    command.Parameters.AddWithValue("@ceilingpermitId", ceilingpermitId);
            //    command.CommandType = CommandType.StoredProcedure;
            //    using (var ds = _sqlHelper.GetDataSet(command))
            //    {
            //        ceilingPermit = _sqlHelper.ConvertDataTable<CeilingPermit>(ds.Tables[0]);
            //        var data = _sqlHelper.ConvertDataTable<CeilingPermitDetail>(ds.Tables[1]);
            //        foreach (var item in ceilingPermit)
            //            item.CeilingPermitDetail = data.Where(x => x.CeilingPermitId == item.CeilingPermitId).ToList();
            //    }
            //}


            return ceilingPermit;
        }

        public CeilingPermit GetCeilingPermit(int? ceilingpermitId)
        {
            CeilingPermit ceilingPermit = new CeilingPermit();
            const string sql = Utility.StoredProcedures.Get_Ceilingpermit;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ceilingpermitId", ceilingpermitId);
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            ceilingPermit = _sqlHelper.ConvertDataTable<CeilingPermit>(ds.Tables[0]).FirstOrDefault();
                        }
                        List<DigitalSignature> digitalSignature = new List<DigitalSignature>();
                        if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                        {
                            digitalSignature=_sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[2]);
                        }
                        var objCeilingPermitDetail = new List<TCeilingPermitDetail>();
                        var objTPermitLinkForms = new List<TPermitLinkForms>();
                        if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                        {
                            objCeilingPermitDetail = _sqlHelper.ConvertDataTable<TCeilingPermitDetail>(ds.Tables[4]);
                             objTPermitLinkForms = _sqlHelper.ConvertDataTable<TPermitLinkForms>(ds.Tables[4]);
                        }
                        foreach (var item in objCeilingPermitDetail)
                            item.TPermitLinkForms = objTPermitLinkForms.FirstOrDefault(x => x.Type == item.CeilingFormId);

                        var users = new List<UserProfile>();
                        if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                        {
                            users=_sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[3]);
                        }
                        var objMappingPermit = new List<TPermitMapping>();
                        if (ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                        {
                            objMappingPermit = _sqlHelper.ConvertDataTable<TPermitMapping>(ds.Tables[5]);
                        }
                        var TIcraLog=new List<TIcraLog>();
                        if (ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
                        {
                            TIcraLog = _sqlHelper.ConvertDataTable<TIcraLog>(ds.Tables[6]);
                        }
                        var projects = new List<TIcraProject>();
                        if (ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
                        {
                            projects= _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[7]);
                        }
                        List<TPermitWorkFlowDetails> TPermitWorkFlowDetails = new List<TPermitWorkFlowDetails>();
                        if (ds.Tables[8] != null && ds.Tables[8].Rows.Count > 0)
                        {
                            TPermitWorkFlowDetails=_sqlHelper.ConvertDataTable<TPermitWorkFlowDetails>(ds.Tables[8]);
                        }

                        var permitsettings = new List<PermitSetting>();
                        if (ds.Tables[10] != null && ds.Tables[10].Rows.Count > 0)
                        {
                            permitsettings= _sqlHelper.ConvertDataTable<PermitSetting>(ds.Tables[10]);

                        }
                        if (ceilingPermit != null && ceilingPermit.CeilingPermitId > 0)
                        {
                            if (!string.IsNullOrEmpty(ceilingPermit.TFileIds))
                                ceilingPermit.Attachments = _tFilesRepository.GetTFiles(ceilingPermit.TFileIds);

                            if (!string.IsNullOrEmpty(ceilingPermit.TDrawingFields))
                                ceilingPermit.DrawingAttachments = _commonRepository.GetUploadedDrawings(ceilingPermit.TDrawingFields);

                            if (ceilingPermit.Requestedby.HasValue)
                                ceilingPermit.RequestedbyUser = users.Where(x => x.UserId == ceilingPermit.Requestedby).FirstOrDefault();

                            if (ceilingPermit.FinalInspectionBy.HasValue)
                                ceilingPermit.FinalInspectionByUser = users.Where(x => x.UserId == ceilingPermit.FinalInspectionBy).FirstOrDefault();

                            if (ceilingPermit.RequesterSignId.HasValue)
                                ceilingPermit.DSRequesterSign = digitalSignature.Where(x => x.DigSignatureId == ceilingPermit.RequesterSignId).FirstOrDefault();

                            if (ceilingPermit.ApproversignId.HasValue)
                                ceilingPermit.DSApproverSign = digitalSignature.Where(x => x.DigSignatureId == ceilingPermit.ApproversignId).FirstOrDefault();

                            if (ceilingPermit.ProjectId > 0)
                            {
                                ceilingPermit.Project = projects.FirstOrDefault(x => x.ProjectId == ceilingPermit.ProjectId);
                            }

                            ceilingPermit.TCeilingPermitDetail = objCeilingPermitDetail.Where(x => x.TPermitLinkForms.Type == 11).ToList();
                            if (ceilingPermit.TCeilingPermitDetail != null)
                            {
                                foreach (var item in ceilingPermit.TCeilingPermitDetail)
                                {
                                    TPermitMapping obj = new TPermitMapping();
                                    if (item.CeilingPermitId > 0)
                                    {
                                        foreach (var item1 in objMappingPermit)
                                        {
                                            if (item1.PermitId1.Trim() == Convert.ToString(ceilingPermit.CeilingPermitId))
                                            {
                                                obj = item1;
                                                // var icra = ConstructionRepository.GetAllInspectionIcraSteps(-1, null, null, null, null).ToList();
                                                obj.PermitNumber = TIcraLog.ToList().Where(x => x.TicraId == Convert.ToInt32(obj.PermitId2)).FirstOrDefault().PermitNo;
                                                item.TPermitMapping.Add(obj);
                                            }

                                        }
                                        // obj = objMappingPermit.Where(x => (x.PermitType1 == Convert.ToInt32(Enums.PermitType.CeilingPermit) && x.PermitId1 == Convert.ToString(ceilingPermit.CeilingPermitId))).ToList().FirstOrDefault();

                                    }

                                }
                            }

                            if (TPermitWorkFlowDetails != null && ceilingPermit != null)
                            {
                                ceilingPermit.TPermitWorkFlowDetails = TPermitWorkFlowDetails;
                                
                                if (ceilingPermit.TPermitWorkFlowDetails != null && ceilingPermit.TPermitWorkFlowDetails.Count > 0)
                                {

                                    foreach (var permitworkflow in ceilingPermit.TPermitWorkFlowDetails)
                                    {
                                        if(permitsettings!=null && permitsettings.Count>0)
                                        {
                                            permitworkflow.PermitSetting = permitsettings.Where(x=>x.Id==permitworkflow.FormSettingId).FirstOrDefault();
                                            if(permitworkflow!=null && permitworkflow.PermitSetting != null)
                                            {
                                                permitworkflow.PermitSetting.Id = permitworkflow.FormSettingId;
                                            }
                                          
                                        }
                                       
                                        permitworkflow.DSPermitSignature = digitalSignature.Where(x => x.DigSignatureId == permitworkflow.LabelSignId).FirstOrDefault();
                                        permitworkflow.UserDetail = users.Where(x => x.UserId == permitworkflow.LabelValue).FirstOrDefault();
                                    }
                                }
                            }
                        }

                    }
                }
            }
            return ceilingPermit;
        }
        public  List<TCeilingPermitDetail> Get_PermitMappingForm()
        {
            List<TCeilingPermitDetail> TCeilingPermitDetail = new List<TCeilingPermitDetail>();
            const string sql = Utility.StoredProcedures.Get_PermitMappingForm;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        var objTPermitLinkForms = _sqlHelper.ConvertDataTable<TPermitLinkForms>(ds.Tables[0]);
                        TCeilingPermitDetail = _sqlHelper.ConvertDataTable<TCeilingPermitDetail>(ds.Tables[0]);
                        foreach (var item in TCeilingPermitDetail)
                        {

                            item.TPermitLinkForms = objTPermitLinkForms.FirstOrDefault(x => x.Type == item.CeilingFormId);
                        }

                        TCeilingPermitDetail = TCeilingPermitDetail.Where(x => x.TPermitLinkForms.Type == 11).ToList();

                    }

                }

            }
            return TCeilingPermitDetail;
        }
        public  bool DeleteCeilingPermitFiles(int ceilingPermitId, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_CeilingPermitFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ceilingPermitId", ceilingPermitId);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        public  bool DeleteCeilingDrawings(int ceilingPermitId, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_CeilingDrawings;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ceilingPermitId", ceilingPermitId);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        public  int InsertCPermitDetails(TCeilingPermitDetail TCeilingPermitDetail)
        {
            int newId;
            const string sql = StoredProcedures.Insert_CeilingPermitDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TCeilingPermitDetailId", TCeilingPermitDetail.TCeilingPermitDetailId);
                command.Parameters.AddWithValue("@CeilingPermitId", TCeilingPermitDetail.CeilingPermitId);
                command.Parameters.AddWithValue("@CeilingFormId", TCeilingPermitDetail.CeilingFormId);
                command.Parameters.AddWithValue("@RespondStatus", TCeilingPermitDetail.RespondStatus);
                command.Parameters.AddWithValue("@CreatedBy", TCeilingPermitDetail.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        #endregion

        #region Life Safety Devices

        public  TLifeSafetyDevicesForms AddLifeSafetyDevices(TLifeSafetyDevicesForms lifesafetydevices)
        {

            const string sql = StoredProcedures.Insert_LifeSafetyDevicesForms;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FormType", lifesafetydevices.FormType);
                command.Parameters.AddWithValue("@LsdFormId", lifesafetydevices.LsdFormId);
                command.Parameters.AddWithValue("@LsdFormNo", lifesafetydevices.LsdFormNo);
                command.Parameters.AddWithValue("@IsMOPSubmission", lifesafetydevices.IsMOPSubmission);
                command.Parameters.AddWithValue("@IsFinalSubmission", lifesafetydevices.IsFinalSubmission);
                command.Parameters.AddWithValue("@ProjectId", lifesafetydevices.ProjectId);
                command.Parameters.AddWithValue("@Requestor", lifesafetydevices.Requestor);
                command.Parameters.AddWithValue("@PhoneNumber", lifesafetydevices.PhoneNumber);
                command.Parameters.AddWithValue("@Contractor", lifesafetydevices.Contractor);
                command.Parameters.AddWithValue("@EmailAddress", lifesafetydevices.EmailAddress);
                command.Parameters.AddWithValue("@Description", lifesafetydevices.Description);
                command.Parameters.AddWithValue("@PrintName", lifesafetydevices.PrintName);
                command.Parameters.AddWithValue("@Signature", lifesafetydevices.Signature);
                command.Parameters.AddWithValue("@SignDate", lifesafetydevices.SignDate);
                command.Parameters.AddWithValue("@IsCurrent", lifesafetydevices.IsCurrent);
                command.Parameters.AddWithValue("@ParentLsdFormId", lifesafetydevices.ParentLsdFormId);
                command.Parameters.AddWithValue("@CreatedBy", lifesafetydevices.CreatedBy);
                command.Parameters.AddWithValue("@DateOfRequest", lifesafetydevices.DateOfRequest);
                command.Parameters.AddWithValue("@Status", lifesafetydevices.Status);
                command.Parameters.AddWithValue("@PermitAuthorizedSignatureId", lifesafetydevices.PermitAuthorizedSignatureId);
                command.Parameters.AddWithValue("@ApprovedBy", lifesafetydevices.ApprovedBy);
                command.Parameters.AddWithValue("@Reason", lifesafetydevices.Reason);
                command.Parameters.AddWithValue("@PermitNo", lifesafetydevices.PermitNo);
                command.Parameters.AddWithValue("@BuildingId", lifesafetydevices.BuildingId);
                command.Parameters.AddWithValue("@BuildingName", lifesafetydevices.BuildingName);
                command.Parameters.AddWithValue("@FloorId", lifesafetydevices.FloorId);
                command.Parameters.AddWithValue("@FloorName", lifesafetydevices.FloorName);
                command.Parameters.AddWithValue("@Zones", lifesafetydevices.Zones);
                command.Parameters.AddWithValue("@TDrawingFields", lifesafetydevices.TDrawingFields);
                command.Parameters.AddWithValue("@DeviceType", lifesafetydevices.DeviceType);
                command.Parameters.AddWithValue("@TFileIds", lifesafetydevices.TFileIds);
                
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                lifesafetydevices.LsdFormNo = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return lifesafetydevices;
        }

        public  int AddLifeSafetyDeviceListl(LifeSafetyDeviceList lifesafetydevicelist)
        {
            int newId;
            const string sql = StoredProcedures.Insert_LifeSafetyDeviceList;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LsdFormId", lifesafetydevicelist.LsdFormId);
                command.Parameters.AddWithValue("@DeviceNumber", lifesafetydevicelist.DeviceNumber);
                command.Parameters.AddWithValue("@Building", lifesafetydevicelist.Building);
                command.Parameters.AddWithValue("@Location", lifesafetydevicelist.Location);
                command.Parameters.AddWithValue("@AssetType", lifesafetydevicelist.AssetType);
                command.Parameters.AddWithValue("@Make", lifesafetydevicelist.Make);
                command.Parameters.AddWithValue("@DateAdded", lifesafetydevicelist.DateAdded);
                command.Parameters.AddWithValue("@DateRemoved", lifesafetydevicelist.DateRemoved);
                command.Parameters.AddWithValue("@BuildingId", lifesafetydevicelist.BuildingId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public  List<TLifeSafetyDevicesForms> GetLifeSafetyDevicesForms()
        {
            List<TLifeSafetyDevicesForms> lists = new List<TLifeSafetyDevicesForms>();
            const string sql = StoredProcedures.Get_LifeSafetyDevicesForms;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    
                    lists = _sqlHelper.ConvertDataTable<TLifeSafetyDevicesForms>(ds.Tables[0]);
                    var projects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[0]);
                    var digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[0]);
                    var data = _sqlHelper.ConvertDataTable<LifeSafetyDeviceList>(ds.Tables[1]);
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]);
                    //if (FromDate != null && ToDate != null)
                    //{
                    //    lists = lists.Where(a => a.DateOfRequest >= FromDate && a.DateOfRequest <= ToDate).ToList();
                    //}
                    foreach (var item in lists)
                    {

                        if (item.Requestor.HasValue)
                            item.RequestorUser = users.Where(x => x.UserId == item.Requestor).FirstOrDefault();
                        if (item.ApprovedBy.HasValue)
                            item.ApprovedByUser = users.Where(x => x.UserId == item.ApprovedBy).FirstOrDefault();

                        if (item.PermitAuthorizedSignatureId.HasValue)
                            item.PermitAuthorizedSignature = digitalSignature.FirstOrDefault(x => x.DigSignatureId == item.PermitAuthorizedSignatureId);
                        item.Project = projects.FirstOrDefault(x => x.ProjectId == item.ProjectId);
                        item.LifeSafetyDeviceList = data.Where(x => x.LsdFormId == item.LsdFormId).ToList();

                        if (!string.IsNullOrEmpty(item.TDrawingFields))
                            item.DrawingAttachments = _commonRepository.GetUploadedDrawings(item.TDrawingFields);

                        if (!string.IsNullOrEmpty(item.TFileIds))
                            item.Attachments = _tFilesRepository.GetTFiles(item.TFileIds);


                        if (item.IsSignDeleted)
                            item.PermitAuthorizedSignature.IsDeleted = item.PermitAuthorizedSignature != null ? item.IsSignDeleted : false;

                        if (item.IsCurrentDeleted)
                            item.PermitAuthorizedSignature.IsCurrent = item.PermitAuthorizedSignature != null ? item.IsCurrentDeleted : false;
                    }
                    
                   

                }
            }
            return lists;
        }

        public  bool DeleteLSDDrawings(int lsdno, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_LSDDrawings;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LsdFormNo", lsdno);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public  bool DeleteLSDPFiles(int lsdno, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_LSDFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@lsdno", lsdno);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        #endregion

        #region Method of Procedure [MOP]

        public  List<TMOP> GetMethodofProcedure(int? id)
        {
            List<TMOP> lists = new List<TMOP>();
            const string sql = StoredProcedures.Get_GetMethodofProcedure;

            List<TMopAdditionalForms> tAdditionalForms = new List<TMopAdditionalForms>();
            List<MopAdditionalForms> tMasterForms = new List<MopAdditionalForms>();
            List<TProjectContactList> tProjectContactList = new List<TProjectContactList>();
            List<TSystemImpactArea> tSystemImpactArea = new List<TSystemImpactArea>();
            List<DigitalSignature> digitalsign = new List<DigitalSignature>();
            List<TFiles> files = new List<TFiles>();
            List<UserProfile> users = new List<UserProfile>();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    lists = _sqlHelper.ConvertDataTable<TMOP>(ds.Tables[0]);
                    var projects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[0]);
                    var digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[0]);
                    if(ds.Tables[1]!=null && ds.Tables[1].Rows.Count>0)
                    {
                        tAdditionalForms = _sqlHelper.ConvertDataTable<TMopAdditionalForms>(ds.Tables[1]);
                    }
                   
                    if (id.HasValue)
                    {
                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            tAdditionalForms = _sqlHelper.ConvertDataTable<TMopAdditionalForms>(ds.Tables[1]);
                            tMasterForms = _sqlHelper.ConvertDataTable<MopAdditionalForms>(ds.Tables[1]);
                            foreach (var item in tAdditionalForms)
                                item.AdditionalForms = tMasterForms.FirstOrDefault(x => x.Id == item.FormId);
                        }
                        if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                        {
                            tSystemImpactArea = _sqlHelper.ConvertDataTable<TSystemImpactArea>(ds.Tables[2]);
                        }
                        if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                        {
                            tProjectContactList = _sqlHelper.ConvertDataTable<TProjectContactList>(ds.Tables[3]);
                        }

                        if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                        {
                            digitalsign = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[4]);
                        }
                        if (ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                        {
                            files = _sqlHelper.ConvertDataTable<TFiles>(ds.Tables[5]);
                        }
                        if (ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
                        {
                            users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[6]);
                        }

                    }

                    foreach (var item in lists)
                    {
                        if (item.ApproverSignatureId.HasValue)
                            item.DSSign1Signature = digitalsign.FirstOrDefault(x => x.DigSignatureId == item.ApproverSignatureId);

                        if (item.RequesterSignatureId.HasValue)
                            item.DSSign2Signature = digitalsign.FirstOrDefault(x => x.DigSignatureId == item.RequesterSignatureId);

                        item.Project = projects.FirstOrDefault(x => x.ProjectId == item.ProjectId);

                        if (id.HasValue)
                        {
                            item.SystemImpactArea = tSystemImpactArea.Where(x => x.TmopId == item.TmopId).ToList();
                            item.ProjectContactList = tProjectContactList.Where(x => x.TmopId == item.TmopId).ToList();
                            item.SystemsImpacted = tAdditionalForms.Where(x => x.AdditionalForms.Type == 2).ToList();
                            item.AdditionalForms = tAdditionalForms.Where(x => x.AdditionalForms.Type == 3).ToList();
                            //item.Attachments= files.ToList();

                            if (!string.IsNullOrEmpty(item.TFileIds))
                                item.Attachments = _tFilesRepository.GetTFiles(item.TFileIds);

                            if (!string.IsNullOrEmpty(item.TDrawingFields))
                                item.DrawingAttachments = _commonRepository.GetUploadedDrawings(item.TDrawingFields);
                        }
                        if (item.ApproveBy.HasValue)
                            item.ApprovedByUser = users.Where(x => x.UserId == item.ApproveBy).FirstOrDefault();

                        if (item.RequestBy.HasValue)
                            item.RequestByUser = users.Where(x => x.UserId == item.RequestBy).FirstOrDefault();
                        if (!id.HasValue)
                        {
                            if (tAdditionalForms != null && tAdditionalForms.Count > 0)
                                item.AdditionalForms = tAdditionalForms.Where(x => x.TMopId == item.TmopId).ToList();
                        }

                    }
                }
            }
            return lists;
        }

        public  List<MopAdditionalForms> GetMopAdditionalForms()
        {
            List<MopAdditionalForms> lists = new List<MopAdditionalForms>();
            const string sql = StoredProcedures.Get_MopMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0 )
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        lists = _sqlHelper.ConvertDataTable<MopAdditionalForms>(ds.Tables[0]);
                    }
                }
                   
            }
            return lists;
        }

        public  int AddMethodofProcedure(TMOP mop)
        {
            int newId;
            const string sql = StoredProcedures.Insert_MethodofProcedure;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TmopId", mop.TmopId);
                command.Parameters.AddWithValue("@ProjectId", mop.ProjectId);
                command.Parameters.AddWithValue("@Date", mop.Date);
                command.Parameters.AddWithValue("@StartDate", mop.StartDate);
                command.Parameters.AddWithValue("@StartTime", mop.StartTime);
                command.Parameters.AddWithValue("@EndDate", mop.EndDate);
                command.Parameters.AddWithValue("@EndTime", mop.EndTime);
                command.Parameters.AddWithValue("@Contractor", mop.Contractor);
                command.Parameters.AddWithValue("@Supervisor", mop.Supervisor);
                command.Parameters.AddWithValue("@BuildingId", mop.BuildingId);
                command.Parameters.AddWithValue("@BuildingName", mop.BuildingName);
                command.Parameters.AddWithValue("@FloorId", mop.FloorId);
                command.Parameters.AddWithValue("@FloorName", mop.FloorName);
                command.Parameters.AddWithValue("@Zones", mop.Zones);
                command.Parameters.AddWithValue("@Description", mop.Description);
                command.Parameters.AddWithValue("@ProcedureSteps", mop.ProcedureSteps);
                command.Parameters.AddWithValue("@RequiredFollowUp", mop.RequiredFollowUp);
                command.Parameters.AddWithValue("@NotificationsNecessary", mop.NotificationsNecessary);
                command.Parameters.AddWithValue("@AdditionalComments", mop.AdditionalComments);
                command.Parameters.AddWithValue("@CreatedBy", mop.CreatedBy);
                command.Parameters.AddWithValue("@Status", mop.Status);
                command.Parameters.AddWithValue("@ApproveBy", mop.ApproveBy);
                command.Parameters.AddWithValue("@ApproveDate", mop.ApproveDate);
                command.Parameters.AddWithValue("@ApproverSignatureId", mop.ApproverSignatureId);
                command.Parameters.AddWithValue("@HasAttachment", mop.HasAttachment);
                command.Parameters.AddWithValue("@TFileId", mop.TFileIds);
                command.Parameters.AddWithValue("@UpdatedBy", mop.UpdatedBy);
                command.Parameters.AddWithValue("@ApproverName", mop.ApproverName);
                command.Parameters.AddWithValue("@RejectReason", mop.RejectReason);
                command.Parameters.AddWithValue("@RequestBy", mop.RequestBy);
                command.Parameters.AddWithValue("@RequesterSignatureId", mop.RequesterSignatureId);
                command.Parameters.AddWithValue("@EmailAddress", mop.EmailAddress);
                command.Parameters.AddWithValue("@AttachDrawingFiles", mop.TDrawingFields);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int AddTMopAdditionalForms(TMopAdditionalForms tMopAdditionalForms)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TMopAdditionalForms;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TmopId", tMopAdditionalForms.TMopId);
                command.Parameters.AddWithValue("@FormId", tMopAdditionalForms.FormId);
                command.Parameters.AddWithValue("@RespondStatus", tMopAdditionalForms.RespondStatus);
                command.Parameters.AddWithValue("@HasResponded", tMopAdditionalForms.HasResponded);
                command.Parameters.AddWithValue("@UpdatedBy", tMopAdditionalForms.UpdatedBy);
                command.Parameters.AddWithValue("@CreatedBy", tMopAdditionalForms.CreatedBy);
                command.Parameters.AddWithValue("@AdditionalType", tMopAdditionalForms.AdditionalType);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int AddTMopAdditionalFormsRedirect(TMopAdditionalForms tMopAdditionalForms)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TmopAdditionalFormsRedirect;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TmopId", tMopAdditionalForms.TMopId);
                command.Parameters.AddWithValue("@FormId", tMopAdditionalForms.FormId);
                command.Parameters.AddWithValue("@HasCompleted", tMopAdditionalForms.HasCompleted);
                command.Parameters.AddWithValue("@UpdatedBy", tMopAdditionalForms.UpdatedBy);
                command.Parameters.AddWithValue("@CreatedBy", tMopAdditionalForms.CreatedBy);
                command.Parameters.AddWithValue("@PermitId", tMopAdditionalForms.PermitId);
                command.Parameters.AddWithValue("@PermitNo", tMopAdditionalForms.PermitNo);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int AddTProjectContactList(TProjectContactList item)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TProjectContactList;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TmopId", item.TmopId);
                command.Parameters.AddWithValue("@Id", item.Id);
                command.Parameters.AddWithValue("@EmailAddress", item.EmailAddress);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Phone", item.Phone);
                command.Parameters.AddWithValue("@Sequence", item.Sequence);
                command.Parameters.AddWithValue("@UpdatedBy", item.UpdatedBy);
                command.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int AddTSystemImpactArea(TSystemImpactArea item)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TSystemImpactArea;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TmopId", item.TmopId);
                command.Parameters.AddWithValue("@Id", item.Id);
                command.Parameters.AddWithValue("@AreaName", item.AreaName);
                command.Parameters.AddWithValue("@Sequence", item.Sequence);
                command.Parameters.AddWithValue("@UpdatedBy", item.UpdatedBy);
                command.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int AddMopFile(TFiles item)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TFiles;
            int filetypeid = (int)item.FileType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TFileId", item.TFileId);
                command.Parameters.AddWithValue("@FileName", item.FileName);
                command.Parameters.AddWithValue("@FilePath", item.FilePath);
                command.Parameters.AddWithValue("@IsDeleted", item.IsDeleted);
                command.Parameters.AddWithValue("@FileType", filetypeid);
                command.Parameters.AddWithValue("@Comment", item.Comment);
                command.Parameters.AddWithValue("@UploadedBy", item.UploadedBy);
                command.Parameters.AddWithValue("@UploadedDate", item.UploadedDate);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  bool DeleteTMOPFiles(int tmopId, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_TMOPFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tmopId", tmopId);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public  bool DeleteTMOPDrawing(int tmopId, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_TMOPDrawing;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tmopId", tmopId);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        #endregion

        #region Fire System ByPass Permit [FSBP]

        public  List<TFireSystemByPassPermit> GetAllFSBPermit()
        {
            List<TFireSystemByPassPermit> lstTFireSystemByPassPermit = new List<TFireSystemByPassPermit>();
            var projects = new List<TIcraProject>();
            var users = new List<UserProfile>();
            const string sql = StoredProcedures.Get_FSBPermit;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            lstTFireSystemByPassPermit = _sqlHelper.ConvertDataTable<TFireSystemByPassPermit>(ds.Tables[0]);
                            projects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[0]);
                        }
                        if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                        {
                            users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[4]);
                        }
                        foreach (var item in lstTFireSystemByPassPermit)
                        {
                            if (item.RequestorBy.HasValue)
                                item.RequestorByUser = users.Where(x => x.UserId == item.RequestorBy).FirstOrDefault();
                            item.Project = projects.FirstOrDefault(x => x.ProjectId == item.ProjectId);
                        }
                    }
                }
            }
            return lstTFireSystemByPassPermit;
        }

        public  TFireSystemByPassPermit GetFSBPermit(int? tfsbPermitId)
        {
            TFireSystemByPassPermit objTFireSystemByPassPermit = new TFireSystemByPassPermit();
            const string sql = StoredProcedures.Get_FSBPermit;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tfsbPermitId", tfsbPermitId);
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        var objTFireSystemByPassPermits = new List<TFireSystemByPassPermit>();
                        var projects = new List<TIcraProject>();
                        var objTFBSPermitDetail = new List<TFSBPermitDetail>();
                        var objFBSPForms = new List<FSBPForms>();
                        var objTFBSBuildingDetails=new List<TFSBPBuildingDetails>();
                        var objBuildings = new List<Buildings>();
                        var digitalSignature = new List<DigitalSignature>();
                        var users = new List<UserProfile>();
                        var objAllTFBSBuildingDetails = new List<TFSBPBuildingDetails>();
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            objTFireSystemByPassPermits = _sqlHelper.ConvertDataTable<TFireSystemByPassPermit>(ds.Tables[0]);
                            projects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[0]);
                        }
                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                             objTFBSPermitDetail = _sqlHelper.ConvertDataTable<TFSBPermitDetail>(ds.Tables[1]);
                             objFBSPForms = _sqlHelper.ConvertDataTable<FSBPForms>(ds.Tables[1]);
                        }
                        if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                        {
                             objTFBSBuildingDetails = _sqlHelper.ConvertDataTable<TFSBPBuildingDetails>(ds.Tables[2]);
                             objBuildings = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[2]);
                        }

                        if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                        {
                             digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[3]);
                        }

                        if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                        {
                             users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[4]);
                        }
                        if (ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                        {
                             objAllTFBSBuildingDetails = _sqlHelper.ConvertDataTable<TFSBPBuildingDetails>(ds.Tables[5]);
                        }
                     
                        var lstFireSystemType = GetFireSystemTypeDetails();
                        foreach (var item in objTFBSPermitDetail)
                            item.FSBPForms = objFBSPForms.FirstOrDefault(x => x.FSBPFormId == item.FSBPFormId);

                        foreach (TFSBPBuildingDetails build in objTFBSBuildingDetails)
                        {
                            build.Buildings = objBuildings.FirstOrDefault(x => x.BuildingId == build.BuildingId);
                            if (build.fireSystemTypes == null)
                                build.fireSystemTypes = new List<FireSystemType>();

                            if (tfsbPermitId.HasValue && tfsbPermitId.Value > 0)
                            {
                                build.fireSystemTypes = new List<FireSystemType>();
                                foreach (FireSystemType firesystem in lstFireSystemType)
                                {
                                    string firesystemid = build.TFireSystemId.Trim(',');
                                    FireSystemType objfiresystem = new FireSystemType()
                                    {
                                        ID = firesystem.ID,
                                        SiteId = firesystem.SiteId,
                                        Name = firesystem.Name

                                    };

                                    if (!string.IsNullOrEmpty(firesystemid))
                                    {
                                        firesystemid = "," + build.TFireSystemId + ",";
                                        if (firesystemid.Contains("," + firesystem.ID + ","))
                                            objfiresystem.CheckVal = 1;
                                        else
                                            objfiresystem.CheckVal = 0;

                                    }
                                    else
                                        objfiresystem.CheckVal = 0;

                                    string firesystemname = firesystem.SiteId.Trim(',');
                                    firesystemname = "," + firesystemname + ",";
                                    if (firesystemname.Contains("," + build.Buildings.SiteId + ","))
                                        build.fireSystemTypes.Add(objfiresystem);
                                }
                            }


                        }

                        if (tfsbPermitId.HasValue && tfsbPermitId.Value > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            objTFireSystemByPassPermit = objTFireSystemByPassPermits.FirstOrDefault(x => x.TFSBPermitId == tfsbPermitId.Value);
                            if (objTFireSystemByPassPermit.RequestorBy.HasValue)
                                objTFireSystemByPassPermit.RequestorByUser = users.Where(x => x.UserId == objTFireSystemByPassPermit.RequestorBy).FirstOrDefault();

                            if (objTFireSystemByPassPermit.ApprovedBy.HasValue)
                                objTFireSystemByPassPermit.ApprovedByUser = users.Where(x => x.UserId == objTFireSystemByPassPermit.ApprovedBy).FirstOrDefault();

                            if (objTFireSystemByPassPermit.ApproverSignId.HasValue)
                                objTFireSystemByPassPermit.DSFSBPApproverSign = digitalSignature.Where(x => x.DigSignatureId == objTFireSystemByPassPermit.ApproverSignId).FirstOrDefault();
                            if (objTFireSystemByPassPermit.BypassEngActionSignId.HasValue)
                                objTFireSystemByPassPermit.DSBypassEngActionSign = digitalSignature.Where(x => x.DigSignatureId == objTFireSystemByPassPermit.BypassEngActionSignId).FirstOrDefault();
                            if (objTFireSystemByPassPermit.BypassReturnEngActionSignId.HasValue)
                                objTFireSystemByPassPermit.DSBypassReturnEngActionSign = digitalSignature.Where(x => x.DigSignatureId == objTFireSystemByPassPermit.BypassReturnEngActionSignId).FirstOrDefault();

                            if (!string.IsNullOrEmpty(objTFireSystemByPassPermit.TFileIds))
                                objTFireSystemByPassPermit.Attachments = _tFilesRepository.GetTFiles(objTFireSystemByPassPermit.TFileIds);

                            if (!string.IsNullOrEmpty(objTFireSystemByPassPermit.TDrawingFields))
                                objTFireSystemByPassPermit.DrawingAttachments = _commonRepository.GetUploadedDrawings(objTFireSystemByPassPermit.TDrawingFields);
                        }
                        //else
                        //{
                        //    objTFireSystemByPassPermit = new TFireSystemByPassPermit();
                        //    objTFBSPermitDetail = new List<TFSBPermitDetail>();
                        //    foreach (var fsbpform in objFBSPForms)
                        //    {
                        //        TFSBPermitDetail obj = new TFSBPermitDetail();
                        //        obj.FSBPFormId = fsbpform.FSBPFormId;
                        //        obj.FSBPForms = fsbpform;
                        //        objTFBSPermitDetail.Add(obj);
                        //    }
                        //}
                        objTFireSystemByPassPermit.Project = projects.FirstOrDefault(x => x.ProjectId == objTFireSystemByPassPermit.ProjectId);
                        objTFireSystemByPassPermit.TFSBPBuildingDetails = objTFBSBuildingDetails;
                        objTFireSystemByPassPermit.ILSMRequiredChecklist = objTFBSPermitDetail.Where(x => x.FSBPForms.Type == 2).ToList();
                        objTFireSystemByPassPermit.BypassEngineeringActions = objTFBSPermitDetail.Where(x => x.FSBPForms.Type == 4).ToList();
                        objTFireSystemByPassPermit.BypassReturnEngineeringActions = objTFBSPermitDetail.Where(x => x.FSBPForms.Type == 5).ToList();


                    }
                }
            }
            return objTFireSystemByPassPermit;
        }

        public  List<FireSystemType> GetFireSystemTypeDetails()
        {
            List<FireSystemType> FireSystemType = new List<FireSystemType>();
            string sql = StoredProcedures.Get_FireSystemType;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null && dt.Rows.Count>0)
                    FireSystemType = _sqlHelper.ConvertDataTable<FireSystemType>(dt);
            }
            return FireSystemType;
        }


        public  List<TFSBPBuildingDetails> GetAllBuildingFiresystem()
        {
            List<TFSBPBuildingDetails> tFSBPBuildingDetails = new List<TFSBPBuildingDetails>();
            var objBuildings = new List<Buildings>();
             var objTFBSBuildingDetails = new List<TFSBPBuildingDetails>();
            const string sql = StoredProcedures.Get_FSBPermit;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                        {
                             objBuildings = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[2]);
                        }
                        if (ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                        {
                             objTFBSBuildingDetails = _sqlHelper.ConvertDataTable<TFSBPBuildingDetails>(ds.Tables[5]);
                        }
                        var lstFireSystemType = GetFireSystemTypeDetails();
                        foreach (TFSBPBuildingDetails build in objTFBSBuildingDetails)
                        {
                            build.Buildings = objBuildings.FirstOrDefault(x => x.BuildingId == build.BuildingId);
                            if (build.fireSystemTypes == null)
                                build.fireSystemTypes = new List<FireSystemType>();


                            build.fireSystemTypes = new List<FireSystemType>();
                            if (build.Buildings != null)
                            {
                                foreach (FireSystemType firesystem in lstFireSystemType)
                                {
                                    string firesystemid = build.TFireSystemId.Trim(',');
                                    FireSystemType objfiresystem = new FireSystemType()
                                    {
                                        ID = firesystem.ID,
                                        SiteId = firesystem.SiteId,
                                        Name = firesystem.Name

                                    };

                                    if (!string.IsNullOrEmpty(firesystemid))
                                    {
                                        firesystemid = "," + build.TFireSystemId + ",";
                                        if (firesystemid.Contains("," + firesystem.ID + ","))
                                            objfiresystem.CheckVal = 1;
                                        else
                                            objfiresystem.CheckVal = 0;

                                    }
                                    else
                                        objfiresystem.CheckVal = 0;

                                    string firesystemname = firesystem.SiteId.Trim(',');
                                    firesystemname = "," + firesystemname + ",";
                                    if (firesystemname.Contains("," + build.Buildings.SiteId + ","))
                                        build.fireSystemTypes.Add(objfiresystem);
                                }
                            }
                        }
                        tFSBPBuildingDetails = objTFBSBuildingDetails.ToList();
                    }

                }
            }
            return tFSBPBuildingDetails;
        }


        public  TFSBPBuildingDetails GetBuildingFiresystemByID(TFSBPBuildingDetails objTFSBPBuildingDetails)
        {
            List<FireSystemType> lstfiresystemtype = new List<FireSystemType>();
            lstfiresystemtype = GetFireSystemTypeDetails();

            foreach (var firesystem in lstfiresystemtype)
            {

                string firesystemname = firesystem.SiteId.Trim(',');
                firesystemname = "," + firesystemname + ",";
                if (firesystem.SiteId.Contains("," + objTFSBPBuildingDetails.Buildings.SiteId + ","))
                {

                    objTFSBPBuildingDetails.fireSystemTypes.Add(firesystem);
                }
            }

            return objTFSBPBuildingDetails;
        }
        public  int InsertFSBPermit(TFireSystemByPassPermit objTFireSystemByPassPermit)
        {
            int newId;
            const string sql = StoredProcedures.Insert_FSBPermit;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TFSBPermitId", objTFireSystemByPassPermit.TFSBPermitId);
                command.Parameters.AddWithValue("@ProjectId", objTFireSystemByPassPermit.ProjectId);
                command.Parameters.AddWithValue("@RequestedDate", objTFireSystemByPassPermit.RequestedDate);
                command.Parameters.AddWithValue("@RequestorBy", objTFireSystemByPassPermit.RequestorBy);
                command.Parameters.AddWithValue("@PhoneNo", objTFireSystemByPassPermit.PhoneNo);
                command.Parameters.AddWithValue("@Company", objTFireSystemByPassPermit.Company);
                command.Parameters.AddWithValue("@Email", objTFireSystemByPassPermit.Email);
                command.Parameters.AddWithValue("@OnSiteContact", objTFireSystemByPassPermit.OnSiteContact);
                command.Parameters.AddWithValue("@OnSitePhone", objTFireSystemByPassPermit.OnSitePhone);
                command.Parameters.AddWithValue("@StartDate", objTFireSystemByPassPermit.StartDate);
                command.Parameters.AddWithValue("@StartTime", objTFireSystemByPassPermit.StartTime);
                command.Parameters.AddWithValue("@EndDate", objTFireSystemByPassPermit.EndDate);
                command.Parameters.AddWithValue("@EndTime", objTFireSystemByPassPermit.EndTime);
                command.Parameters.AddWithValue("@Description", objTFireSystemByPassPermit.Description);
                command.Parameters.AddWithValue("@ApprovalStatus", objTFireSystemByPassPermit.ApprovalStatus);
                command.Parameters.AddWithValue("@ScheduledDate", objTFireSystemByPassPermit.ScheduledDate);
                command.Parameters.AddWithValue("@IsSystemReprogrammingRequired", objTFireSystemByPassPermit.IsSystemReprogrammingRequired);
                command.Parameters.AddWithValue("@ApprovedDate", objTFireSystemByPassPermit.ApprovedDate);
                command.Parameters.AddWithValue("@ApprovedBy", objTFireSystemByPassPermit.ApprovedBy);
                command.Parameters.AddWithValue("@ApproverSignId", objTFireSystemByPassPermit.ApproverSignId);
                command.Parameters.AddWithValue("@DevicePointsAffected", objTFireSystemByPassPermit.DevicePointsAffected);
                command.Parameters.AddWithValue("@DepartmentZonesAffected", objTFireSystemByPassPermit.DepartmentZonesAffected);
                command.Parameters.AddWithValue("@BypassEngActionDate", objTFireSystemByPassPermit.BypassEngActionDate);
                command.Parameters.AddWithValue("@BypassEngActionTime", objTFireSystemByPassPermit.BypassEngActionTime);
                command.Parameters.AddWithValue("@BypassReturnEngActionDate", objTFireSystemByPassPermit.BypassReturnEngActionDate);
                command.Parameters.AddWithValue("@BypassReturnEngActionTime", objTFireSystemByPassPermit.BypassReturnEngActionTime);
                command.Parameters.AddWithValue("@BypassEngActionSignId", objTFireSystemByPassPermit.BypassEngActionSignId);
                command.Parameters.AddWithValue("@BypassReturnEngActionSignId", objTFireSystemByPassPermit.BypassReturnEngActionSignId);
                command.Parameters.AddWithValue("@CreatedBy", objTFireSystemByPassPermit.CreatedBy);
                command.Parameters.AddWithValue("@Reason", objTFireSystemByPassPermit.Reason);
                command.Parameters.AddWithValue("@TDrawingFields", objTFireSystemByPassPermit.TDrawingFields);
                command.Parameters.AddWithValue("@TFileIds", objTFireSystemByPassPermit.TFileIds);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int InsertFSBPermitDetails(TFSBPermitDetail objTFSBPermitDetail)
        {
            int newId;
            const string sql = StoredProcedures.Insert_FSBPermitDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TFSBPermitDetailId", objTFSBPermitDetail.TFSBPermitDetailId);
                command.Parameters.AddWithValue("@TFSBPermitId", objTFSBPermitDetail.TFSBPermitId);
                command.Parameters.AddWithValue("@FSBPFormId", objTFSBPermitDetail.FSBPFormId);
                command.Parameters.AddWithValue("@RespondStatus", objTFSBPermitDetail.RespondStatus);
                command.Parameters.AddWithValue("@TimeinbyPass", objTFSBPermitDetail.TimeinbyPass);
                command.Parameters.AddWithValue("@CreatedBy", objTFSBPermitDetail.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int InsertFSBPBuildingDetails(TFSBPBuildingDetails objTFSBPBuildingDetails)
        {
            int newId;
            const string sql = StoredProcedures.Insert_FSBPBuildingDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TFSBPBuildingDetailId", objTFSBPBuildingDetails.TFSBPBuildingDetailId);
                command.Parameters.AddWithValue("@TFSBPermitId", objTFSBPBuildingDetails.TFSBPermitId);
                command.Parameters.AddWithValue("@BuildingId", objTFSBPBuildingDetails.BuildingId);
                command.Parameters.AddWithValue("@BuildingName", objTFSBPBuildingDetails.BuildingName);
                command.Parameters.AddWithValue("@CreatedBy", objTFSBPBuildingDetails.CreatedBy);
                command.Parameters.AddWithValue("@TFireSystemId", objTFSBPBuildingDetails.TFireSystemId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public  bool DeleteFSBPDrawings(int TFSBPermitId, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_FSBPDrawings;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TFSBPermitId", @TFSBPermitId);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        public  bool DeleteFSBPFiles(int TFSBPermitId, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_FSBPFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TFSBPermitId", @TFSBPermitId);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        #endregion


        #region Facilities Maintenance Occurrence [FMC]  

        public  List<TFacilitiesMaintenanceOccurrencePermit> GetAllFacilitiesMaintenanceOccurrence()
        {

           var lists = new List<TFacilitiesMaintenanceOccurrencePermit>();
            const string sql = StoredProcedures.Get_All_FacilitiesMaintenanceOccurrence;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        lists = _sqlHelper.ConvertDataTable<TFacilitiesMaintenanceOccurrencePermit>(ds.Tables[0]);

                    }
                }
            }
            return lists.ToList();
        }

        public  List<Shift> GetShiftsAsync()
        {
            List<Shift> lists = new List<Shift>();
            const string sql = StoredProcedures.Get_Shift;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        lists = _sqlHelper.ConvertDataTable<Shift>(ds.Tables[0]);

                    }
                }
            }

            return lists.ToList();
        }
        public  TFacilitiesMaintenanceOccurrencePermit GetFacilitiesMaintenanceOccurrence(int? id)
        {
            TFacilitiesMaintenanceOccurrencePermit TFMC = new TFacilitiesMaintenanceOccurrencePermit();
            const string sql = StoredProcedures.Get_FacilitiesMaintenanceOccurrence;
            List<DigitalSignature> digitalsign = new List<DigitalSignature>();
            List<ClassificationType> ClassificationType = new List<ClassificationType>();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        TFMC = _sqlHelper.ConvertDataTable<TFacilitiesMaintenanceOccurrencePermit>(ds.Tables[0]).FirstOrDefault();
                        ClassificationType = _sqlHelper.ConvertDataTable<ClassificationType>(ds.Tables[1]);
                        var digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[0]);
                        var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count == 0)
                        {
                            TFMC = new TFacilitiesMaintenanceOccurrencePermit();
                            TFMC.lstClassificationType = ClassificationType.OrderBy(x => x.Sequence).ToList();
                        }
                        if (id > 0)
                        {
                            digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[2]);
                            users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[3]);

                            if (TFMC.RequesterSignId.HasValue)
                                TFMC.DSSign1Signature = digitalSignature.FirstOrDefault(x => x.DigSignatureId == TFMC.RequesterSignId);

                            if (TFMC.ApproverSignId.HasValue)
                                TFMC.DSSign2Signature = digitalSignature.FirstOrDefault(x => x.DigSignatureId == TFMC.ApproverSignId);

                            TFMC.lstClassificationType = ClassificationType.OrderBy(x => x.Sequence).ToList();


                            if (TFMC.RequestedBy.HasValue)
                                TFMC.PermitRequestUser = users.Where(x => x.UserId == TFMC.RequestedBy.Value).FirstOrDefault();

                            if (TFMC.ApprovedBy.HasValue)
                                TFMC.AuthorizedByUser = users.Where(x => x.UserId == TFMC.ApprovedBy.Value).FirstOrDefault();

                            if (!string.IsNullOrEmpty(TFMC.TDrawingFields))
                                TFMC.DrawingAttachments = _commonRepository.GetUploadedDrawings(TFMC.TDrawingFields);
                        }

                    }
                }

            }
            return TFMC;
        }

        public  List<ClassificationType> GetClassificationType()
        {

            const string sql = StoredProcedures.Get_FacilitiesMaintenanceOccurrence;
            List<DigitalSignature> digitalsign = new List<DigitalSignature>();
            List<ClassificationType> ClassificationType = new List<ClassificationType>();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", 0);
                using (DataSet ds =  _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        ClassificationType = _sqlHelper.ConvertDataTable<ClassificationType>(ds.Tables[1]);

                    }
                }
            }
            return ClassificationType;
        }


        public   TFacilitiesMaintenanceOccurrencePermit GetFacilitiesMaintenanceOccurrenceAsync(int? id)
        {
            TFacilitiesMaintenanceOccurrencePermit TFMC = new TFacilitiesMaintenanceOccurrencePermit();
            const string sql = StoredProcedures.Get_FacilitiesMaintenanceOccurrence;
            List<DigitalSignature> digitalsign = new List<DigitalSignature>();
            List<ClassificationType> ClassificationType = new List<ClassificationType>();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                using (DataSet ds =  _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        TFMC = _sqlHelper.ConvertDataTable<TFacilitiesMaintenanceOccurrencePermit>(ds.Tables[0]).FirstOrDefault();
                        ClassificationType = _sqlHelper.ConvertDataTable<ClassificationType>(ds.Tables[1]);
                        var digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[0]);
                        var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count == 0)
                        {
                            TFMC = new TFacilitiesMaintenanceOccurrencePermit();
                            TFMC.lstClassificationType = ClassificationType.OrderBy(x => x.Sequence).ToList();
                        }
                        if (id > 0)
                        {
                            digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[2]);
                            users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[3]);

                            if (TFMC.RequesterSignId.HasValue)
                                TFMC.DSSign1Signature = digitalSignature.FirstOrDefault(x => x.DigSignatureId == TFMC.RequesterSignId);

                            if (TFMC.ApproverSignId.HasValue)
                                TFMC.DSSign2Signature = digitalSignature.FirstOrDefault(x => x.DigSignatureId == TFMC.ApproverSignId);

                            TFMC.lstClassificationType = ClassificationType.OrderBy(x => x.Sequence).ToList();

                            if (TFMC.RequestedBy.HasValue)
                                TFMC.PermitRequestUser = users.Where(x => x.UserId == TFMC.RequestedBy).FirstOrDefault();

                            if (TFMC.ApprovedBy.HasValue)
                                TFMC.AuthorizedByUser = users.Where(x => x.UserId == TFMC.ApprovedBy).FirstOrDefault();

                            if (!string.IsNullOrEmpty(TFMC.TDrawingFields))
                                TFMC.DrawingAttachments = _commonRepository.GetUploadedDrawings(TFMC.TDrawingFields);

                        }

                    }
                }

            }
            return TFMC;
        }

        public  int AddFacilitiesMaintenanceOccurrenceAsync(TFacilitiesMaintenanceOccurrencePermit TFMC)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TFacilitiesMaintenanceOccurrence;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TfmcId", TFMC.TfmcId);
                command.Parameters.AddWithValue("@Name", TFMC.Name);
                command.Parameters.AddWithValue("@AssetId", TFMC.AssetId);
                command.Parameters.AddWithValue("@Position", TFMC.Position);
                command.Parameters.AddWithValue("@DepartmentId", TFMC.DepartmentId);
                command.Parameters.AddWithValue("@PhoneNo", TFMC.PhoneNo);
                command.Parameters.AddWithValue("@Shift", TFMC.Shift);
                command.Parameters.AddWithValue("@DateOfOccurence", TFMC.DateOfOccurence);
                command.Parameters.AddWithValue("@TimeOfOccurence", TFMC.TimeOfOccurence);
                command.Parameters.AddWithValue("@ReportDate", TFMC.ReportDate);
                command.Parameters.AddWithValue("@ClassificationType", TFMC.ClassificationType);
                command.Parameters.AddWithValue("@Location", TFMC.Location);
                command.Parameters.AddWithValue("@OccurenceDetails", TFMC.OccurenceDetails);
                command.Parameters.AddWithValue("@ActionTaken", TFMC.ActionTaken);
                command.Parameters.AddWithValue("@Comments", TFMC.Comments);
                command.Parameters.AddWithValue("@AddedToEOC", TFMC.AddedToEOC);
                command.Parameters.AddWithValue("@CompletelyResolved", TFMC.CompletelyResolved);
                command.Parameters.AddWithValue("@CompleteDate", TFMC.CompleteDate);
                command.Parameters.AddWithValue("@CreatedBy", TFMC.CreatedBy);
                command.Parameters.AddWithValue("@UpdatedBy", TFMC.UpdatedBy);
                command.Parameters.AddWithValue("@Status", TFMC.Status);
                command.Parameters.AddWithValue("@RequestedBy", TFMC.RequestedBy);
                command.Parameters.AddWithValue("@RequesterDate", TFMC.RequesterDate);
                command.Parameters.AddWithValue("@RequesterSignId", TFMC.RequesterSignId);
                command.Parameters.AddWithValue("@ApprovedBy", TFMC.ApprovedBy);
                command.Parameters.AddWithValue("@ApprovedDate", TFMC.ApprovedDate);
                command.Parameters.AddWithValue("@ApproverSignId", TFMC.ApproverSignId);
                command.Parameters.AddWithValue("@Reason", TFMC.Reason);
                command.Parameters.AddWithValue("@Company", TFMC.Company);
                command.Parameters.AddWithValue("@TDrawingFields", TFMC.TDrawingFields);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }

            return newId;
        }
        public  bool DeletePermit(int Id, int PermitType)
        {
            bool status;
            const string sql = StoredProcedures.Delete_Permit;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@PermitType", PermitType);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        public  bool DeleteFMCDrawings(int TfmcId, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_FMCDrawings;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TfmcId", @TfmcId);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        #endregion
        #region PaperPermit
        public  List<PaperPermit> GetPaperPermit()
        {
            List<PaperPermit> PaperPermit = new List<PaperPermit>();
            const string sql = Utility.StoredProcedures.Get_PaperPermit;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    PaperPermit = _sqlHelper.ConvertDataTable<PaperPermit>(ds.Tables[0]);
                    foreach (var perit in PaperPermit)
                    {
                        if (!string.IsNullOrEmpty(perit.TFileIds))
                            perit.Attachments = _tFilesRepository.GetTFiles(perit.TFileIds);
                    }
                }
            }

            return PaperPermit;
        }
        public  PaperPermit GetPaperPermitById(int? id)
        {
            PaperPermit PaperPermit = new PaperPermit();
            const string sql = Utility.StoredProcedures.Get_PaperPermitById;

            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    PaperPermit = _sqlHelper.ConvertDataTable<PaperPermit>(ds.Tables[0]).FirstOrDefault();
                    if (!string.IsNullOrEmpty(PaperPermit.TFileIds))
                        PaperPermit.Attachments = _tFilesRepository.GetTFiles(PaperPermit.TFileIds);
                }
            }

            return PaperPermit;
        }
        public  int AddPaperPermit(PaperPermit PaperPermit)
        {
            int newId;
            const string sql = StoredProcedures.Insert_PaperPermit;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PermitId", PaperPermit.PermitId);
                command.Parameters.AddWithValue("@PermitType", PaperPermit.PermitType);
                command.Parameters.AddWithValue("@PermitNo", PaperPermit.PermitNo);
                command.Parameters.AddWithValue("@ProjectId", PaperPermit.ProjectId);
                command.Parameters.AddWithValue("@TFileIds", PaperPermit.TFileIds);
                command.Parameters.AddWithValue("@Contractor", PaperPermit.Contractor);
                command.Parameters.AddWithValue("@CreatedBy", PaperPermit.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public  bool Delete_TPaperPermitFiles(int permitid, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_TPaperPermitFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@permitid", permitid);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        #endregion

        #region AllPermits
        public  List<AllPermits> GetAllPermit(int? FilterBy = 30, int? PermitType =null, string Status = null,int? ProjectID=0)
        {
            List<AllPermits> AllPermits = new List<AllPermits>();
            const string sql = Utility.StoredProcedures.GetAllPermits;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FilterBy", FilterBy.Value);
                command.Parameters.AddWithValue("@Status",Status);
                command.Parameters.AddWithValue("@PermitType", PermitType);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    AllPermits = _sqlHelper.ConvertDataTable<AllPermits>(ds.Tables[0]);

                    if (!string.IsNullOrEmpty(Status))
                    {

                        AllPermits = (from p in AllPermits
                                      where Status.Split(',').Contains(Convert.ToString(p.Status))
                                      select p).ToList();

                    }
                    if (ProjectID.HasValue && ProjectID>0)
                    {
                        AllPermits=AllPermits.Where(x => x.ProjectId == ProjectID).ToList();
                    }

                }
            }

            return AllPermits;
        }

        public  int InsertPermitMapping(TPermitMapping TPermitMapping)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TPermitMapping;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PermitId1", TPermitMapping.PermitId1);
                command.Parameters.AddWithValue("@PermitId2", TPermitMapping.PermitId2);
                command.Parameters.AddWithValue("@PermitType1", TPermitMapping.PermitType1);
                command.Parameters.AddWithValue("@PermitType2", TPermitMapping.PermitType2);
                command.Parameters.AddWithValue("@CreatedBy", TPermitMapping.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        #endregion
        #region PermitWorkflowSettings
        public  List<PermitSetting> GetPermitWorkFlowSettings()
        {
            List<PermitSetting> PermitSetting = new List<PermitSetting>();
            const string sql = Utility.StoredProcedures.Get_PermitWorkflowSettings;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    PermitSetting = _sqlHelper.ConvertDataTable<PermitSetting>(ds.Tables[0]);
                   
                }

            }
          
            return PermitSetting;
        }
        public  int AddPermitWorkFlow(PermitSetting PermitSetting)
        {
            int newId;
            const string sql = StoredProcedures.Insert_PermitSettingsWorkFlow;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", PermitSetting.Id);
                command.Parameters.AddWithValue("@IsDeleted", PermitSetting.IsDeleted);
                command.Parameters.AddWithValue("@LabelText", PermitSetting.LabelText);
                command.Parameters.AddWithValue("@LabelType", PermitSetting.LabelType);
                command.Parameters.AddWithValue("@PermitType", PermitSetting.PermitType);
                command.Parameters.AddWithValue("@Required", PermitSetting.RequiredType);
                command.Parameters.AddWithValue("@Sequence", PermitSetting.Sequence);
                command.Parameters.AddWithValue("@ControlType", PermitSetting.ControlType);
                command.Parameters.AddWithValue("@CreatedBy", PermitSetting.CreatedBy);
                command.Parameters.AddWithValue("@NotificationEmail", PermitSetting.NotificationEmail);
                command.Parameters.AddWithValue("@NotificationCCEmail", PermitSetting.NotificationCCEmail);
                command.Parameters.AddWithValue("@IsSendMail", PermitSetting.IsSendMailValue);
                command.Parameters.AddWithValue("@StepCode", PermitSetting.StepCode);
                command.Parameters.AddWithValue("@PathId", PermitSetting.PathId);
                command.Parameters.AddWithValue("@AdditionalDescription", PermitSetting.AdditionalDescription);
                command.Parameters.AddWithValue("@IsAssetDeviceChange", PermitSetting.IsAssetDeviceChange);
                command.Parameters.AddWithValue("@Mode", PermitSetting.Mode);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int AddTPermitWorkFlowDetails(TPermitWorkFlowDetails TPermitWorkFlowDetails)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TPermitDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", TPermitWorkFlowDetails.Id);
                command.Parameters.AddWithValue("@FormSettingId", TPermitWorkFlowDetails.FormSettingId);
                command.Parameters.AddWithValue("@LabelValue", TPermitWorkFlowDetails.@LabelValue);
                command.Parameters.AddWithValue("@LabelSignId", TPermitWorkFlowDetails.@LabelSignId);
                command.Parameters.AddWithValue("@LabelSignDate", TPermitWorkFlowDetails.LabelSignDate);
                command.Parameters.AddWithValue("@PermitNumber", TPermitWorkFlowDetails.PermitNumber);
                command.Parameters.AddWithValue("@CreatedBy", TPermitWorkFlowDetails.CreatedBy);
                 command.Parameters.AddWithValue("@IsCurrent", 1);
                command.Parameters.AddWithValue("@Comment", TPermitWorkFlowDetails.Comment);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public  bool DeletePermitWorkFlow(int id, int PermitType)
        {
            bool status;
            const string sql = StoredProcedures.Delete_PermitWorkFlow;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@PermitType", PermitType);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        #endregion

        #region Asset Change Devices
        public List<AssetDevicePathSettings> GetAssetDevicePath()
        {
            List<AssetDevicePathSettings> AssetDevicePathSettings = new List<AssetDevicePathSettings>();
            const string sql = Utility.StoredProcedures.Get_PermitWorkflowSettings;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    AssetDevicePathSettings = _sqlHelper.ConvertDataTable<AssetDevicePathSettings>(ds.Tables[1]);

                }

            }

            return AssetDevicePathSettings;
        }
        public TAssetDeviceChangeForms AddAssetChangeDevices(TAssetDeviceChangeForms AssetDeviceChangeForms)
        {

            const string sql = StoredProcedures.Insert_AssetDeviceChangeForms;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FormType", AssetDeviceChangeForms.FormType);
                command.Parameters.AddWithValue("@AdcFormId", AssetDeviceChangeForms.AdcFormId);
                command.Parameters.AddWithValue("@AdcFormNo", AssetDeviceChangeForms.AdcFormNo);
                command.Parameters.AddWithValue("@ProjectId", AssetDeviceChangeForms.ProjectId);
                command.Parameters.AddWithValue("@Requestor", AssetDeviceChangeForms.Requestor);
                command.Parameters.AddWithValue("@PhoneNumber", AssetDeviceChangeForms.PhoneNumber);
                command.Parameters.AddWithValue("@Contractor", AssetDeviceChangeForms.Contractor);
                command.Parameters.AddWithValue("@EmailAddress", AssetDeviceChangeForms.EmailAddress);
                command.Parameters.AddWithValue("@Description", AssetDeviceChangeForms.Description);
                command.Parameters.AddWithValue("@IsCurrent", AssetDeviceChangeForms.IsCurrent);
                command.Parameters.AddWithValue("@ParentAdcFormId", AssetDeviceChangeForms.ParentAdcFormId);
                command.Parameters.AddWithValue("@CreatedBy", AssetDeviceChangeForms.CreatedBy);
                command.Parameters.AddWithValue("@DateOfRequest", AssetDeviceChangeForms.DateOfRequest);
                command.Parameters.AddWithValue("@Status", AssetDeviceChangeForms.Status);
                command.Parameters.AddWithValue("@Reason", AssetDeviceChangeForms.Reason);
                command.Parameters.AddWithValue("@PermitNo", AssetDeviceChangeForms.PermitNo);
                command.Parameters.AddWithValue("@BuildingId", AssetDeviceChangeForms.BuildingId);
                command.Parameters.AddWithValue("@BuildingName", AssetDeviceChangeForms.BuildingName);
                command.Parameters.AddWithValue("@FloorId", AssetDeviceChangeForms.FloorId);
                command.Parameters.AddWithValue("@FloorName", AssetDeviceChangeForms.FloorName);
                command.Parameters.AddWithValue("@Zones", AssetDeviceChangeForms.Zones);
                command.Parameters.AddWithValue("@TDrawingFields", AssetDeviceChangeForms.TDrawingFields);
                command.Parameters.AddWithValue("@DeviceType", AssetDeviceChangeForms.DeviceType);
                command.Parameters.AddWithValue("@TFileIds", AssetDeviceChangeForms.TFileIds);
                command.Parameters.AddWithValue("@Path", AssetDeviceChangeForms.Path);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                AssetDeviceChangeForms.AdcFormNo = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return AssetDeviceChangeForms;
        }

        public int AddAsetChangeDeviceList(AssetDeviceList AssetDeviceList)
        {
            int newId;
            const string sql = StoredProcedures.Insert_AssetList;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AdcFormId", AssetDeviceList.AdcFormId);
                command.Parameters.AddWithValue("@DeviceNumber", AssetDeviceList.DeviceNumber);
                command.Parameters.AddWithValue("@SerialNumber", AssetDeviceList.SerialNumber);
                command.Parameters.AddWithValue("@Manufacturer", AssetDeviceList.Manufacturer);
                command.Parameters.AddWithValue("@CMMSAssetNumber", AssetDeviceList.CMMSAssetNumber);
                command.Parameters.AddWithValue("@Description", AssetDeviceList.Description);
                command.Parameters.AddWithValue("@FireAlarmDeviceNumber", AssetDeviceList.FireAlarmDeviceNumber);
                command.Parameters.AddWithValue("@RoomNumber", AssetDeviceList.RoomNumber);
                command.Parameters.AddWithValue("@ModelNumber", AssetDeviceList.ModelNumber);
                command.Parameters.AddWithValue("@DateAdded", AssetDeviceList.DateAdded);
                command.Parameters.AddWithValue("@DateRemoved", AssetDeviceList.DateRemoved);
                command.Parameters.AddWithValue("@DateRelocation", AssetDeviceList.DateRelocation);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public List<TAssetDeviceChangeForms> GetAssetChangeDeviceForms()
        {
            List<TAssetDeviceChangeForms> lists = new List<TAssetDeviceChangeForms>();
            const string sql = StoredProcedures.Get_AssetChangeDeviceForms;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {

                    lists = _sqlHelper.ConvertDataTable<TAssetDeviceChangeForms>(ds.Tables[0]);
                    var projects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[0]);
                    var data = _sqlHelper.ConvertDataTable<AssetDeviceList>(ds.Tables[1]);
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]);
                 
                    foreach (var item in lists)
                    {

                        if (item.Requestor.HasValue)
                            item.RequestorUser = users.Where(x => x.UserId == item.Requestor).FirstOrDefault();
                       item.Project = projects.FirstOrDefault(x => x.ProjectId == item.ProjectId);
                        item.AssetDeviceList = data.Where(x => x.AdcFormId == item.AdcFormId).ToList();

                        if (!string.IsNullOrEmpty(item.TDrawingFields))
                            item.DrawingAttachments = _commonRepository.GetUploadedDrawings(item.TDrawingFields);

                        if (!string.IsNullOrEmpty(item.TFileIds))
                            item.Attachments = _tFilesRepository.GetTFiles(item.TFileIds);

                       
                    }



                }
            }
            return lists;
        }

        public TAssetDeviceChangeForms GetAssetChangeDeviceFormsById(Guid FormId)
        {
            TAssetDeviceChangeForms objTAssetDeviceChangeForms = new TAssetDeviceChangeForms();
            List<TAssetDeviceChangeForms> lists = new List<TAssetDeviceChangeForms>();
            const string sql = StoredProcedures.Get_AssetChangeDeviceFormsById;
            using (var command = new SqlCommand(sql))
            {
                
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AdcFormId", FormId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {

                    lists = _sqlHelper.ConvertDataTable<TAssetDeviceChangeForms>(ds.Tables[0]);
                    var projects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[0]);
                    var data = _sqlHelper.ConvertDataTable<AssetDeviceList>(ds.Tables[1]);
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]);
                    List<TPermitWorkFlowDetails> TPermitWorkFlowDetails = new List<TPermitWorkFlowDetails>();
                    if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                    {
                        TPermitWorkFlowDetails = _sqlHelper.ConvertDataTable<TPermitWorkFlowDetails>(ds.Tables[4]);
                    }

                    var permitsettings = new List<PermitSetting>();
                    if (ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
                    {
                        permitsettings = _sqlHelper.ConvertDataTable<PermitSetting>(ds.Tables[6]);

                    }
                    List<DigitalSignature> digitalSignature = new List<DigitalSignature>();
                    if (ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
                    {
                        digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[7]);
                    }
                    foreach (var item in lists)
                    {

                        if (item.Requestor.HasValue)
                            item.RequestorUser = users.Where(x => x.UserId == item.Requestor).FirstOrDefault();
                        item.Project = projects.FirstOrDefault(x => x.ProjectId == item.ProjectId);
                       item.AssetDeviceList = data.Where(x => x.AdcFormId == item.AdcFormId).ToList();

                        if (!string.IsNullOrEmpty(item.TDrawingFields))
                            item.DrawingAttachments = _commonRepository.GetUploadedDrawings(item.TDrawingFields);

                        if (!string.IsNullOrEmpty(item.TFileIds))
                            item.Attachments = _tFilesRepository.GetTFiles(item.TFileIds);

                        item.AssetDevicePathSettings = _sqlHelper.ConvertDataTable<AssetDevicePathSettings>(ds.Tables[3]);
                    }
                    if(lists != null && lists.Count>0)
                    objTAssetDeviceChangeForms = lists.ToList().FirstOrDefault();

                    if (lists != null && TPermitWorkFlowDetails != null  && TPermitWorkFlowDetails.Count>0)
                    {
                        objTAssetDeviceChangeForms.TPermitWorkFlowDetails = TPermitWorkFlowDetails;

                        if (objTAssetDeviceChangeForms.TPermitWorkFlowDetails != null && objTAssetDeviceChangeForms.TPermitWorkFlowDetails.Count > 0)
                        {

                            foreach (var permitworkflow in objTAssetDeviceChangeForms.TPermitWorkFlowDetails)
                            {
                                if (permitsettings != null && permitsettings.Count > 0)
                                {
                                    permitworkflow.PermitSetting = permitsettings.Where(x => x.Id == permitworkflow.FormSettingId).FirstOrDefault();
                                    if (permitworkflow != null && permitworkflow.PermitSetting != null)
                                    {
                                        permitworkflow.PermitSetting.Id = permitworkflow.FormSettingId;
                                    }

                                }

                                permitworkflow.DSPermitSignature = digitalSignature.Where(x => x.DigSignatureId == permitworkflow.LabelSignId).FirstOrDefault();
                                permitworkflow.UserDetail = users.Where(x => x.UserId == permitworkflow.LabelValue).FirstOrDefault();
                            }
                        }
                    }


                }
            }
            return objTAssetDeviceChangeForms;
        }
        public List<AssetDevices> GetAssetDevicesList()
        {
            List<AssetDevices> lists = new List<AssetDevices>();
            const string sql = StoredProcedures.GetAssetDevicesList;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {

                    lists = _sqlHelper.ConvertDataTable<AssetDevices>(ds.Tables[0]);

                }
            }
            return lists;
        }
    
        public bool DeleteADCDrawings(int adcno, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_ADCDrawings;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LsdFormNo", adcno);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public bool DeleteADCPFiles(int adcno, string fileIds)
        {
            bool status;
            const string sql = StoredProcedures.Delete_ADCFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@lsdno", adcno);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        public int AddAssetDevices(AssetDevices AssetDevices)
        {
            int newId;
            const string sql = StoredProcedures.Insert_AssetDevices;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AssetFormId", AssetDevices.AssetFormId);
                command.Parameters.AddWithValue("@Name", AssetDevices.Name);
                command.Parameters.AddWithValue("@Description", AssetDevices.Description);
                command.Parameters.AddWithValue("@Path", AssetDevices.Path);
                command.Parameters.AddWithValue("@DeviceType", AssetDevices.DeviceType);
                command.Parameters.AddWithValue("@IsActive", AssetDevices.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", AssetDevices.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        #endregion
    }
}
