using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface ITypeService
    {
        List<TaggedMaster> CheckExistingTag(string Tagid);
        List<TaggedMaster> GetTaggedList(int userId, Guid? tagId, int? tagtype);
        int RemoveTagID(string TagId, string activityID, int UserId, int Tagtype, string PermitNo);
        int Save(Types type);
        int SaveTaggedUser(TaggedMaster taggedMaster);
    }
}