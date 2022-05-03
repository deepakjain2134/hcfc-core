using HCF.BDO.NBIH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiSubStatusWS;

namespace TMS.NBIH
{
    public static class SubStatus
    {


        public static List<TMSNBISubStatusResults> GetTmsSubStatus(string moduleCode)
        {
            var authHeader = SetNbiHeader();
            var client = new SubStatusWSSoapClient(SubStatusWSSoapClient.EndpointConfiguration.SubStatusWSSoap);
            var SubStatuslists = client.GetCodesAsync(authHeader, moduleCode, 51, true).Result;
            List<TMSNBISubStatusResults> TMS_Results = ConverttoResult(SubStatuslists);
            return TMS_Results;
        }
        private static List<TMSNBISubStatusResults> ConverttoResult(NbiSubStatusWS.SubStatus[] SubStatuslists)
        {
            List<TMSNBISubStatusResults> lists = new List<TMSNBISubStatusResults>();
            foreach (var item in SubStatuslists)
            {
                TMSNBISubStatusResults obj = new TMSNBISubStatusResults();
                obj.ID = item.ID;
                obj.Code = item.Code;
                obj.Description = item.Description;
                obj.StatusCode = item.StatusCode;
                obj.StatusDescription = item.StatusDescription;
                obj.SegmentID = item.SegmentID;
                obj.Show = item.Show;
                obj.ShowInQuery = item.ShowInQuery;
                obj.ModuleCode = item.ModuleId.ToString();
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
