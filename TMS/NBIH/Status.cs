using HCF.BDO.NBIH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiStatusWS;

namespace TMS.NBIH
{
    public static class Status
    {
        public static List<TMSNBIStatusResults> GetTmsStatus(string modulecode)
        {
            //List<HCF.BDO.WoStatus> statuslist = new List<HCF.BDO.WoStatus>();
            var authHeader = SetNbiHeader();
            var client = new NbiStatusWS.StatusWSSoapClient(StatusWSSoapClient.EndpointConfiguration.StatusWSSoap);
            var Statuslist = client.GetCodesAsync(authHeader, modulecode, 51, true).Result;
            List<HCF.BDO.NBIH.TMSNBIStatusResults> TMS_Results = ConverttoResult(Statuslist);
            return TMS_Results;
        }

        private static List<TMSNBIStatusResults> ConverttoResult(NbiStatusWS.Status[] statuslist)
        {
            List<TMSNBIStatusResults> lists = new List<TMSNBIStatusResults>();
            foreach (var item in statuslist)
            {
                HCF.BDO.NBIH.TMSNBIStatusResults obj = new TMSNBIStatusResults();
                obj.ID = item.ID;
                obj.Code = item.Code;
                obj.Description = item.Description;
                obj.SegmentID = item.SegmentID;
                obj.Show = item.Show;
                obj.ShowInQuery = item.ShowInQuery;
                obj.ModuleId = Convert.ToInt32(item.ModuleId);
                obj.ModuleCode = ((NbiStatusWS.MODULES)item.ModuleId).ToString();
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
