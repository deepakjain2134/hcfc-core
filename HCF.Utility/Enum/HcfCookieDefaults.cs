using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Utility.Enum
{
    public class HcfCookieDefaults
    {
        public static string Prefix => ".CRx";

        public static string AntiforgeryCookie => ".Antiforgery";

        public static string SessionCookie => ".SessionCRx";

        public static string AuthenticationCookie => ".AuthCookie";
    }
}
