using HCF.BDO.NBIH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiPriorityWS;

namespace TMS.NBIH
{
    public static class Priority
    {
        public static List<TMSNBIPriorityResults> GetTmsPriorities(string modulecode)
        {
            var authHeader = SetNbiHeader();
            var client = new NbiPriorityWS.PriorityWSSoapClient(PriorityWSSoapClient.EndpointConfiguration.PriorityWSSoap);
            var Prioritylist = client.GetCodesAsync(authHeader, modulecode, 51, true).Result;
            List<HCF.BDO.NBIH.TMSNBIPriorityResults> TMS_Results = ConverttoResult(Prioritylist);
            return TMS_Results;
        }

        private static List<TMSNBIPriorityResults> ConverttoResult(NbiPriorityWS.Priority[] Prioritylist)
        {
            List<TMSNBIPriorityResults> lists = new List<TMSNBIPriorityResults>();
            foreach (var item in Prioritylist)
            {
                HCF.BDO.NBIH.TMSNBIPriorityResults obj = new TMSNBIPriorityResults();
                obj.ID = item.ID;
                obj.Code = item.Code;
                obj.Description = item.Description;
                obj.SegmentID = item.SegmentID;
                obj.Show = item.Show;
                obj.ShowInQuery = item.ShowInQuery;
                obj.ModuleId = Convert.ToInt32(item.ModuleId);
                obj.ModuleCode = ((NbiPriorityWS.MODULES)item.ModuleId).ToString();
                lists.Add(obj);
            }
            return lists;
        }

        private static AuthenticationHeader SetNbiHeader()
        {
            AuthenticationHeader authHeader = new AuthenticationHeader();
            authHeader = NbihAuthenticationHeader.GetHeader<AuthenticationHeader>(authHeader);
            return authHeader;
        }
    }
}
