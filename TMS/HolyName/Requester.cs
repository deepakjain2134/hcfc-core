using System.Collections.Generic;
using TMS.HolyNameTmsService;

namespace TMS
{
    public static class Requester
    {
        public static HCF.BDO.UserProfile LoadRequester(string requesterName)
        {
            HCF.BDO.UserProfile user = new HCF.BDO.UserProfile();
             HolyNameRequesterClient requestClient = new HolyNameRequesterClient();
            requestClient.LoadRequester(requesterName);
                                                                                                            
            return user;
        }

        public static List<HCF.BDO.UserProfile> Query()
        {
            List<HCF.BDO.UserProfile> users = new List<HCF.BDO.UserProfile>();
            RequesterCriteria[] criteria = new RequesterCriteria[1];
            RequesterCriteria c1 = new RequesterCriteria();
            c1.fieldField = FIELDS4.SEGMENT;
            c1.fieldValueField = "11";
            c1.operatorField = OPERATORS4.EQUAL;
          //  c1.logicalOperatorField = LOGICAL_OPERATORS2.AND;
            criteria[0] = c1;
            HolyNameRequesterClient requestClient = new HolyNameRequesterClient();
            HolyNameTmsService.Requester[] requesters = requestClient.QueryRequester(criteria);

            foreach (var requester in requesters)
            {
                HCF.BDO.UserProfile user = new HCF.BDO.UserProfile();
                users.Add(user);
            }
            return users;
        }
    }
}
