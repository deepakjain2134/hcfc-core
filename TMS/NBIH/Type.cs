using HCF.BDO.NBIH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiTypeWS;

namespace TMS.NBIH
{
    public static class Type
    {


        public static List<TMSNBITypeResults> GetTmsTypes(string modulecode)
        {
            var authHeader = SetNbiHeader();
            var client = new TypeWSSoapClient(TypeWSSoapClient.EndpointConfiguration.TypeWSSoap);
            var Typelists = client.GetCodesAsync(authHeader, modulecode, 51, true).Result;
            List<TMSNBITypeResults> TMS_Results = ConverttoResult(Typelists);
            return TMS_Results;
        }

        private static List<TMSNBITypeResults> ConverttoResult(NbiTypeWS.Type[] Typelists)
        {
            List<TMSNBITypeResults> lists = new List<TMSNBITypeResults>();
            foreach (var item in Typelists)
            {
                TMSNBITypeResults obj = new TMSNBITypeResults();
                obj.ID = item.ID;
                obj.Code = item.Code;
                obj.Description = item.Description;
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
