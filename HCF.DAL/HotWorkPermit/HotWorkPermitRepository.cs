
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HCF.BDO;
namespace HCF.DAL
{
    public class HotWorkPermitRepository : IHotWorkPermitRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ICommonRepository _commonRepository;

        public HotWorkPermitRepository(ISqlHelper sqlHelper, ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
            _sqlHelper = sqlHelper;
        }
        public  List<THotWorkPermits>  GetAllHotWorksPermit()
        {
            var tHotWorkPermit = new List<THotWorkPermits>();
            const string sql = Utility.StoredProcedures.Get_All_THotWorkPermit;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if(ds!=null && ds.Tables.Count>0)
                    {
                        tHotWorkPermit = _sqlHelper.ConvertDataTable<THotWorkPermits>(ds.Tables[0]);
                    }
                  
                   
                }
            }
            return tHotWorkPermit;
        }
        

        public  THotWorkPermits GetTHotWorksPermit(int? tHotWorkPermitId)
        {
            THotWorkPermits tHotWorkPermits = new THotWorkPermits();
            const string sql = Utility.StoredProcedures.Get_THotWorkPermits;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@HotWorkPermitId", tHotWorkPermitId.HasValue ? tHotWorkPermitId.Value : (object)DBNull.Value);
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        tHotWorkPermits = _sqlHelper.ConvertDataTable<THotWorkPermits>(ds.Tables[0]).FirstOrDefault();
                        List<ConstructionWorkType> lstConstructionWorkType = _sqlHelper.ConvertDataTable<ConstructionWorkType>(ds.Tables[1]).ToList();
                        List<THotWorkItems> lists = _sqlHelper.ConvertDataTable<THotWorkItems>(ds.Tables[2]);
                        lists = lists.GroupBy(test => test.ItemId).Select(grp => grp.First()).Distinct().ToList();

                        var projects = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[0]);

                        List<DigitalSignature> digitalSignature = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[3]);
                        var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[4]);
                        if (tHotWorkPermitId.HasValue && tHotWorkPermitId.Value > 0 && tHotWorkPermits != null)
                        {

                            if (tHotWorkPermits.PermitRequestorSignatureId.HasValue)
                                tHotWorkPermits.DSSign1Signature = digitalSignature.Where(x => x.DigSignatureId == tHotWorkPermits.PermitRequestorSignatureId).FirstOrDefault();


                            if (tHotWorkPermits.PermitAuthorizedSignatureId.HasValue)
                                tHotWorkPermits.DSSign2Signature = digitalSignature.Where(x => x.DigSignatureId == tHotWorkPermits.PermitAuthorizedSignatureId).FirstOrDefault();


                            if (tHotWorkPermits.PermitRequestBy.HasValue)
                                tHotWorkPermits.RequestorByUser = users.Where(x => x.UserId == tHotWorkPermits.PermitRequestBy).FirstOrDefault();

                            if (tHotWorkPermits.PermitAuthorizedBy.HasValue)
                                tHotWorkPermits.AuthorizedByUser = users.Where(x => x.UserId == tHotWorkPermits.PermitAuthorizedBy).FirstOrDefault();

                            tHotWorkPermits.Project = projects.FirstOrDefault(x => x.ProjectId == tHotWorkPermits.ProjectId);

                            if (!string.IsNullOrEmpty(tHotWorkPermits.TDrawingFields))
                                tHotWorkPermits.DrawingAttachments = _commonRepository.GetUploadedDrawings(tHotWorkPermits.TDrawingFields);

                        }
                        else
                        {
                            tHotWorkPermits = new THotWorkPermits();
                        }
                        if (lstConstructionWorkType != null)
                            tHotWorkPermits.ConstructionWorkType = lstConstructionWorkType;

                        if (lists != null)
                            tHotWorkPermits.THotWorkItems = lists;
                    }
                }
            }
            return tHotWorkPermits;
        }


        public  int InsertTHotWorkPermits(THotWorkPermits thotWorkPermits)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_THotWorkPermits;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PermitNo", thotWorkPermits.PermitNo);
                command.Parameters.AddWithValue("@ProjectId", thotWorkPermits.ProjectId);
                command.Parameters.AddWithValue("@StartDate", thotWorkPermits.StartDate);
                command.Parameters.AddWithValue("@EndDate", thotWorkPermits.EndDate);
                command.Parameters.AddWithValue("@StartTime", thotWorkPermits.StartTime);
                command.Parameters.AddWithValue("@EndTime", thotWorkPermits.EndTime);
                command.Parameters.AddWithValue("@PermitRequestor", thotWorkPermits.PermitRequestor);
                command.Parameters.AddWithValue("@Company", thotWorkPermits.Company);
                command.Parameters.AddWithValue("@EmailAddress", thotWorkPermits.EmailAddress);
                command.Parameters.AddWithValue("@PhoneNumber", thotWorkPermits.PhoneNumber);
                command.Parameters.AddWithValue("@OnSiteContact", thotWorkPermits.OnSiteContact);
                command.Parameters.AddWithValue("@OnSitePhone", thotWorkPermits.@OnSitePhone);
                command.Parameters.AddWithValue("@BuildingId", thotWorkPermits.BuildingId);
                command.Parameters.AddWithValue("@FloorId", thotWorkPermits.FloorId);
                command.Parameters.AddWithValue("@Zones", thotWorkPermits.Zones);
                command.Parameters.AddWithValue("@Rooms", thotWorkPermits.Rooms);
                command.Parameters.AddWithValue("@WorkType", thotWorkPermits.WorkType);
                command.Parameters.AddWithValue("@Description", thotWorkPermits.Description);
                command.Parameters.AddWithValue("@CreatedBy", thotWorkPermits.CreatedBy);
                command.Parameters.AddWithValue("@PermitAuthorizedBy", thotWorkPermits.PermitAuthorizedBy);
                command.Parameters.AddWithValue("@PermitAuthorizedByDate", thotWorkPermits.PermitAuthorizedByDate);
                command.Parameters.AddWithValue("@PermitRequestBy", thotWorkPermits.PermitRequestBy);
                command.Parameters.AddWithValue("@PermitRequestByDate", thotWorkPermits.PermitRequestByDate);
                command.Parameters.AddWithValue("@IsVerifyHotWorkPerformed", thotWorkPermits.IsVerifyHotWorkPerformed);
                command.Parameters.AddWithValue("@IsVerifyObservedrevisited", thotWorkPermits.IsVerifyObservedrevisited);
                command.Parameters.AddWithValue("@IsVerifyAttach", thotWorkPermits.IsVerifyAttach);
                command.Parameters.AddWithValue("@PermitRequestorSignatureId", thotWorkPermits.PermitRequestorSignatureId);
                command.Parameters.AddWithValue("@PermitAuthorizedSignatureId", thotWorkPermits.PermitAuthorizedSignatureId);
                command.Parameters.AddWithValue("@Status", thotWorkPermits.Status);
                command.Parameters.AddWithValue("@Reason", thotWorkPermits.Reason);
                command.Parameters.AddWithValue("@TDrawingFields", thotWorkPermits.TDrawingFields);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public  int UpdateTHotWorkPermits(THotWorkPermits thotWorkPermits)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Update_THotWorkPermits;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@THotWorkPermitId", thotWorkPermits.THotWorkPermitId);
                command.Parameters.AddWithValue("@PermitNo", thotWorkPermits.PermitNo);
                command.Parameters.AddWithValue("@ProjectId", thotWorkPermits.ProjectId);
                command.Parameters.AddWithValue("@StartDate", thotWorkPermits.StartDate);
                command.Parameters.AddWithValue("@EndDate", thotWorkPermits.EndDate);
                command.Parameters.AddWithValue("@StartTime", thotWorkPermits.StartTime);
                command.Parameters.AddWithValue("@EndTime", thotWorkPermits.EndTime);
                command.Parameters.AddWithValue("@PermitRequestor", thotWorkPermits.PermitRequestor);
                command.Parameters.AddWithValue("@Company", thotWorkPermits.Company);
                command.Parameters.AddWithValue("@EmailAddress", thotWorkPermits.EmailAddress);
                command.Parameters.AddWithValue("@PhoneNumber", thotWorkPermits.PhoneNumber);
                command.Parameters.AddWithValue("@OnSiteContact", thotWorkPermits.OnSiteContact);
                command.Parameters.AddWithValue("@OnSitePhone", thotWorkPermits.@OnSitePhone);
                command.Parameters.AddWithValue("@BuildingId", thotWorkPermits.BuildingId);
                command.Parameters.AddWithValue("@FloorId", thotWorkPermits.FloorId);
                command.Parameters.AddWithValue("@Zones", thotWorkPermits.Zones);
                command.Parameters.AddWithValue("@Rooms", thotWorkPermits.Rooms);
                command.Parameters.AddWithValue("@WorkType", thotWorkPermits.WorkType);
                command.Parameters.AddWithValue("@Description", thotWorkPermits.Description);
                command.Parameters.AddWithValue("@UpdatedBy", thotWorkPermits.UpdatedBy);
                command.Parameters.AddWithValue("@PermitAuthorizedBy", thotWorkPermits.PermitAuthorizedBy);
                command.Parameters.AddWithValue("@PermitAuthorizedByDate", thotWorkPermits.PermitAuthorizedByDate);
                command.Parameters.AddWithValue("@PermitRequestBy", thotWorkPermits.PermitRequestBy);
                command.Parameters.AddWithValue("@PermitRequestByDate", thotWorkPermits.PermitRequestByDate);
                command.Parameters.AddWithValue("@IsVerifyHotWorkPerformed", thotWorkPermits.IsVerifyHotWorkPerformed);
                command.Parameters.AddWithValue("@IsVerifyObservedrevisited", thotWorkPermits.IsVerifyObservedrevisited);
                command.Parameters.AddWithValue("@IsVerifyAttach", thotWorkPermits.IsVerifyAttach);
                command.Parameters.AddWithValue("@PermitRequestorSignatureId", thotWorkPermits.PermitRequestorSignatureId);
                command.Parameters.AddWithValue("@PermitAuthorizedSignatureId", thotWorkPermits.PermitAuthorizedSignatureId);
                command.Parameters.AddWithValue("@Status", thotWorkPermits.Status);
                command.Parameters.AddWithValue("@Reason", thotWorkPermits.Reason);
                command.Parameters.AddWithValue("@TDrawingFields", thotWorkPermits.TDrawingFields);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public  int Insert_THotWorkItems(THotWorkItems thotWorkitems)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_THotWorkItems;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@HotWorkPermitId", thotWorkitems.HotWorkPermitId);
                command.Parameters.AddWithValue("@ItemId", thotWorkitems.ItemId);
                command.Parameters.AddWithValue("@ParentId", thotWorkitems.ParentId);
                command.Parameters.AddWithValue("@CreatedBy", thotWorkitems.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public  bool DeleteHWPDrawings(int HotWorkPermitId, string fileIds)
        {
            bool status;
            const string sql = Utility.StoredProcedures.Delete_HWPDrawings;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@HotWorkPermitId", HotWorkPermitId);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
    }
}
