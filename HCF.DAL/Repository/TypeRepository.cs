using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class TypeRepository : ITypeRepository
    {
        #region Ctor
        private readonly ISqlHelper _sqlHelper;

        private readonly IUsersRepository _usersRepository;

        public TypeRepository(ISqlHelper sqlHelper, IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
            _sqlHelper = sqlHelper;
        }

        #endregion

        #region Tagged User

        public int Save(Types type)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Type;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TypeId", type.TypeId);
                command.Parameters.AddWithValue("@Name", type.Name);
                command.Parameters.AddWithValue("@Code", type.Code);
                command.Parameters.AddWithValue("@ModuleCode", type.ModuleCode);
                command.Parameters.AddWithValue("@Description", type.Description);
                command.Parameters.AddWithValue("@IsActive", type.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int SaveTaggedUser(TaggedMaster tMaster)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TaggedMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TaggedId", tMaster.TaggedId);
                command.Parameters.AddWithValue("@ActionTaken", tMaster.ActionTaken);
                command.Parameters.AddWithValue("@Notify", tMaster.Notify);
                command.Parameters.AddWithValue("@Comment", tMaster.Comment);
                command.Parameters.AddWithValue("@IsRequired", tMaster.IsRequired);
                command.Parameters.AddWithValue("@ActivityId", tMaster.ActivityId);
                command.Parameters.AddWithValue("@TaggedType", tMaster.TaggedType);
                command.Parameters.AddWithValue("@Createdby", tMaster.Createdby);
                command.Parameters.AddWithValue("@ActionToRemoveUser", tMaster.ActionToRemoveUser);
                command.Parameters.AddWithValue("@NotifyToRemoveUser", tMaster.NotifyToRemoveUser);
                command.Parameters.AddWithValue("@Deficiencies", tMaster.selectedDeficiencies);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int RemoveTagID(string TagId, string activityID, int UserId, int Tagtype, string PermitNo)
        {
            int newId;
            const string sql = StoredProcedures.Remove_TagID;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TagId", TagId);
                command.Parameters.AddWithValue("@activityID", activityID);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@Tagtype", Tagtype);
                command.Parameters.AddWithValue("@PermitNo", PermitNo);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public List<TaggedMaster> CheckExistingTag(string tagId)
        {
            List<TaggedMaster> list = new();
            const string sql = StoredProcedures.Check_ExistingTagId;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Tagid", tagId);
                var ds = _sqlHelper.GetDataSet(command);
                list = _sqlHelper.ConvertDataTable<TaggedMaster>(ds.Tables[0]);
            }
            return list;
        }

        public List<TaggedMaster> GetTaggedList(int userId, Guid? tagId, int? tagtype)
        {
            List<TaggedMaster> list = new();
            List<TaggedResource> taggedResources = new();
            const string sql = StoredProcedures.Get_TaggedList;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Tagid", tagId);
                command.Parameters.AddWithValue("@TagType", tagtype);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    list = _sqlHelper.ConvertDataTable<TaggedMaster>(ds.Tables[0]);
                    taggedResources = _sqlHelper.ConvertDataTable<TaggedResource>(ds.Tables[1]);
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        var taggedResource = new TaggedResource();
                        taggedResource.ActivityId = Conversion.ConvertToGuidOrNull(item["ActivityId"].ToString());
                        taggedResource.Email = item["Email"].ToString();
                        taggedResource.IsActive = Conversion.ConvertToBoolean(item["IsActive"].ToString());
                        taggedResource.TaggedId = Conversion.ConvertToGuid(item["TaggedId"].ToString());
                        if (!string.IsNullOrEmpty(item["TaggedResourceId"].ToString()))
                            taggedResource.TaggedResourceId = Convert.ToInt32(item["TaggedResourceId"].ToString());
                        if (!string.IsNullOrEmpty(item["TaggedType"].ToString()))
                            taggedResource.TaggedType = Convert.ToInt32(item["TaggedType"].ToString());
                        if (!string.IsNullOrEmpty(item["UserId"].ToString()))
                            taggedResource.UserId = Convert.ToInt32(item["UserId"].ToString());
                        if (!string.IsNullOrEmpty(item["VendorId"].ToString()))
                            taggedResource.VendorId = Convert.ToInt32(item["VendorId"].ToString());
                        taggedResources.Add(taggedResource);
                    }

                    var taggedMasterUser = _usersRepository.ConvertToUser(ds.Tables[0]);
                    var taggedResourcesUser = _usersRepository.ConvertToUser(ds.Tables[1]);
                    foreach (var item in list)
                    {
                        item.CreatedByUser = taggedMasterUser.FirstOrDefault(x => x.UserId == item.Createdby);
                        item.Resource = taggedResources.Where(x => x.TaggedId == item.TaggedId).ToList();
                        foreach (var resource in item.Resource)
                            resource.TaggedUser = taggedResourcesUser.FirstOrDefault(x => x.UserId == resource.UserId);
                    }
                }
            }
            return list;
        }

        #endregion
    }
}