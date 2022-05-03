using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface ITypeRepository
    {
        List<TaggedMaster> CheckExistingTag(string tagId);
        List<TaggedMaster> GetTaggedList(int userId, Guid? tagId, int? tagtype);
        int RemoveTagID(string TagId, string activityID, int UserId, int Tagtype, string PermitNo);
        int Save(Types type);
        int SaveTaggedUser(TaggedMaster tMaster);
    }
}