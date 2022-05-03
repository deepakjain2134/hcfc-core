using HCF.BDO.NBIH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiLocationWS;

namespace TMS.NBIH
{
    public static class Locations
    {

        public static List<TMSNBISLocationResults> GetTmsLocations()
        {
            var authHeader = SetNbiHeader();
            var client = new LocationWSSoapClient(LocationWSSoapClient.EndpointConfiguration.LocationWSSoap);
            var Locationlists = client.GetCodesAsync(authHeader, 51, true).Result;
            List<TMSNBISLocationResults> TMS_Results = ConverttoResult(Locationlists);
            return TMS_Results;
        }
        private static List<TMSNBISLocationResults> ConverttoResult(NbiLocationWS.Location[] Locationlists)
        {
            List<TMSNBISLocationResults> lists = new List<TMSNBISLocationResults>();
            foreach (var item in Locationlists)
            {
                TMSNBISLocationResults obj = new TMSNBISLocationResults();
                obj.ID = item.ID;
                obj.Code = item.Code;
                obj.SiteCode = item.SiteCode;
                obj.SiteDescription = item.SiteDescription;
                obj.BuildingCode = item.BuildingCode;
                obj.BuildingDescription = item.BuildingDescription;
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
