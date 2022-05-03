using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Utility.Core.Http
{
    public static partial class HcfCookieDefaults
    {
        public static string Prefix => ".CRx";

        public static string AntiforgeryCookie => ".Antiforgery";
       
        public static string SessionCookie => ".Session";

        public static string AuthenticationCookie => ".Authentication";
    }
}
