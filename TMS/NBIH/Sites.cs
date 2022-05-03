using HCF.BDO.NBIH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiSitesWS;

namespace TMS.NBIH
{
    public static class Sites
    {
        public static List<TMSNBISitesResults> GetTmsSites()
        {
            var authHeader = SetNbiHeader();
            var client = new SiteWSSoapClient(SiteWSSoapClient.EndpointConfiguration.SiteWSSoap);
            var Siteslists = client.GetCodesAsync(authHeader, 51, true).Result;
            List<HCF.BDO.NBIH.TMSNBISitesResults> TMS_Results = ConverttoResult(Siteslists);
            return TMS_Results;
        }
        private static List<TMSNBISitesResults> ConverttoResult(NbiSitesWS.Site[] Siteslists)
        {
            List<TMSNBISitesResults> lists = new List<TMSNBISitesResults>();
            foreach (var item in Siteslists)
            {
                HCF.BDO.NBIH.TMSNBISitesResults obj = new TMSNBISitesResults();
                obj.ID = item.ID;
                obj.Code = item.Code;
                obj.Description = item.Description;
                obj.SegmentID = item.SegmentID;
                obj.Show = item.Show;
                obj.ShowInQuery = item.ShowInQuery;
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
