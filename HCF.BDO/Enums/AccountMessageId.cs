using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BDO.Enums
{
    public enum AccountMessageId
    {
        [Description("Your account has been disabled.")]
        InActiveUserMsg,
        [Description("User vendor account has been de-activated.")]
        InActiveVendorMsg,
        [Description("Email not found in our CRx system.")]
        EmailNotFound,
        Error
    }
}
