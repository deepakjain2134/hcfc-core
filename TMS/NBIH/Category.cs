using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiCategoryWS;

namespace TMS.NBIH
{
    public static class Category
    {
        public static List<HCF.BDO.Category> GetTmsCategory(string modulecode)
        {
            var categories = new List<HCF.BDO.Category>();
            var authHeader = SetNbiHeader();
            var client = new NbiCategoryWS.CategoryWSSoapClient(CategoryWSSoapClient.EndpointConfiguration.CategoryWSSoap);
            var lists = client.GetCodesAsync(authHeader, modulecode, 51, true).Result;
            return categories;
        }

        private static AuthenticationHeader SetNbiHeader()
        {
            AuthenticationHeader authHeader = new AuthenticationHeader();
            authHeader = NbihAuthenticationHeader.GetHeader<AuthenticationHeader>(authHeader);
            return authHeader;
        }
    }
}
