using HCF.BDO.NBIH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiAccountWS;

namespace TMS.NBIH
{
    public static class Accounts
    {
        public static List<HCF.BDO.NBIH.TMSNBIAccountResults> GetTmsAccounts()
        {

            var authHeader = SetNbiHeader();
            var client = new AccountWSSoapClient(AccountWSSoapClient.EndpointConfiguration.AccountWSSoap);
            var Accountlists = client.GetCodesAsync(authHeader, 51, true).Result;
            List<HCF.BDO.NBIH.TMSNBIAccountResults> TMS_Results = ConverttoResult(Accountlists);
            return TMS_Results;
        }


        private static List<TMSNBIAccountResults> ConverttoResult(NbiAccountWS.Account[] Accountlists)
        {
            List<TMSNBIAccountResults> lists = new List<TMSNBIAccountResults>();
            foreach (var item in Accountlists)
            {
                HCF.BDO.NBIH.TMSNBIAccountResults obj = new TMSNBIAccountResults();
                obj.ID = item.ID;
                obj.Code = item.Code;
                obj.Description = item.Description;
                obj.SegmentID = item.SegmentID;
                obj.Show = item.Show;
                obj.DepartmentCode = item.DepartmentCode;
                obj.DepartmentDescription = item.DepartmentDescription;
                obj.TypeCode = item.TypeCode;
                obj.TypeDescription = item.TypeDescription;
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
