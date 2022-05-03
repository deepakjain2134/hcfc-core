using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiSubCategoryWS;

namespace TMS.NBIH
{
    public static class SubCategory
    {
        public static NbiSubCategoryWS.SubCategory[] GetTmsSubCategory(string moduleCode, int segmentID, bool includeCommon)
        {
            var authHeader = SetNbiHeader();
            var client = new SubCategoryWSSoapClient(SubCategoryWSSoapClient.EndpointConfiguration.SubCategoryWSSoap);
            var list = client.GetCodesAsync(authHeader, "", 51, true).Result;
            return list;
        }

        private static AuthenticationHeader SetNbiHeader()
        {
            AuthenticationHeader authHeader = new AuthenticationHeader();
            authHeader = NbihAuthenticationHeader.GetHeader<AuthenticationHeader>(authHeader);
            return authHeader;
        }
    }
}
