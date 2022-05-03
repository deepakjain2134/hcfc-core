using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiSkillWS;

namespace TMS.NBIH
{
    public static class Skill
    {
        public static NbiSkillWS.Skill[] GetTmsSkill()
        {
            var authHeader = SetNbiHeader();
            var client = new NbiSkillWS.SkillWSSoapClient(SkillWSSoapClient.EndpointConfiguration.SkillWSSoap);
            var lists = client.GetCodesAsync(authHeader, 51, true).Result;
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
