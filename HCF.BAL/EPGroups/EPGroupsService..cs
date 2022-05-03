using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;

namespace HCF.BAL
{
    public class EpGroupsService : IEpGroupsService
    {
        private readonly ILoggingService _loggingService;
        private readonly IEPGroupsRepsitory _epGroupsRepsitory;


        public EpGroupsService(ILoggingService loggingService, IEPGroupsRepsitory epGroupsRepsitory)
        {
            _epGroupsRepsitory = epGroupsRepsitory;
            _loggingService = loggingService;
        }
        public  List<EPGroups> GetEPGroups(int? epgroupId)
        {
            return _epGroupsRepsitory.GetEPGroups(epgroupId);
        }

        public  List<EPGroups> GetVendorEPGroups(int vendorId)
        {
            return _epGroupsRepsitory.GetVendorEPGroups(vendorId);
        }

        public  int SaveVendorEPGroups(int vendorId, int groupId, bool status)
        {
            return _epGroupsRepsitory.SaveVendorEPGroups(vendorId, groupId, status);
        }

        public  List<EPGroups> GetEPGroupNameList()
        {
            return _epGroupsRepsitory.GetEPGroupNameList();
        }

        public  EPGroups GetEPGroupName(int epgroupId)
        {
            return _epGroupsRepsitory.GetEPGroupName(epgroupId);
        }

        public  int AddEPGroupsName(EPGroups ePGroups)
        {
            return _epGroupsRepsitory.AddEPGroupsName(ePGroups);
        }

        public  int RemoveEP(int? epgroupId, int? epdetailid)
        {
            return _epGroupsRepsitory.RemoveEP(epgroupId, epdetailid);
        }

        public  List<EPGroups> AssignEpsList(int? epgroupId)
        {
            return _epGroupsRepsitory.AssignEpsList(epgroupId);
        }

        public  int AssignEPsGroup(int epdetailId, int epGruopId, bool status)
        {
            return _epGroupsRepsitory.AssignEPsGroup(epdetailId, epGruopId, status);
        }

     
    }
}
