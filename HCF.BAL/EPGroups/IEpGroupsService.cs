using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IEpGroupsService
    {
        int AddEPGroupsName(EPGroups ePGroups);
        int AssignEPsGroup(int epdetailId, int epGruopId, bool status);
        List<EPGroups> AssignEpsList(int? epgroupId);
        EPGroups GetEPGroupName(int epgroupId);
        List<EPGroups> GetEPGroupNameList();
        List<EPGroups> GetEPGroups(int? epgroupId);
        List<EPGroups> GetVendorEPGroups(int vendorId);
        int RemoveEP(int? epgroupId, int? epdetailid);
        int SaveVendorEPGroups(int vendorId, int groupId, bool status);
    }
}