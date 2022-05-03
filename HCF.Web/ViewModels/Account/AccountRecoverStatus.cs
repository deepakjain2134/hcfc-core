using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewModels.Account
{
    public class AccountRecoverStatus
    {
        public int StatusCode { get; set; }

        public string ErrorMsg { get; set; }

        public bool IsSuccess { get; set; }

        public string PasswordMode { get; set; }
    }
}
