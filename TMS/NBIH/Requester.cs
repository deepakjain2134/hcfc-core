using HCF.BDO.NBIH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiRequesterWS;

namespace TMS.NBIH
{
    public static class Requester
    {

        public static List<TMSNBISRequesterResults> GetTmsRequester()
        {
            var authHeader = SetNbiHeader();
            var client = new RequesterWSClient(RequesterWSClient.EndpointConfiguration.RequesterWS);
            var requesterCriteria = new RequesterCriteria[1];
            DateTime createdDate = DateTime.Now;
            var c1 = new RequesterCriteria
            {
                Field = FIELDS.DATE_CREATED,
                FieldValue = createdDate.AddDays(-4000),
                Operator = OPERATORS.GREATER_OR_EQUAL
            };
            requesterCriteria[0] = c1;
            var Requesterlists = client.QueryWithSegmentAsync(authHeader, requesterCriteria, 51).Result;
            List<HCF.BDO.NBIH.TMSNBISRequesterResults> TMS_Results = ConverttoResult(Requesterlists);
            return TMS_Results;
        }
        private static List<TMSNBISRequesterResults> ConverttoResult(NbiRequesterWS.Requester[] Requesterlists)
        {
            List<TMSNBISRequesterResults> lists = new List<TMSNBISRequesterResults>();
            foreach (var item in Requesterlists)
            {
                HCF.BDO.NBIH.TMSNBISRequesterResults obj = new TMSNBISRequesterResults();

                obj.AccountCode = item.AccountCode;
                obj.AccountDescription = item.AccountDescription;
                obj.BuildingCode = item.BuildingCode;
                obj.BuildingName = item.BuildingName;
                obj.DateCreated = Convert.ToDateTime(item.DateCreated);
                obj.DateUpdated = Convert.ToDateTime(item.DateUpdated);
                obj.LocationCode = item.LocationCode;
                obj.LocationDescription = item.LocationDescription;
                obj.PagerNumber = item.PagerNumber;
                obj.PhoneNumber = item.PhoneNumber;
                obj.RequesterID = item.RequesterID;
                obj.RequesterName = item.RequesterName;
                obj.SegmentID = item.SegmentID;
                obj.SiteCode = item.SiteCode;
                obj.SiteName = item.SiteName;
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

