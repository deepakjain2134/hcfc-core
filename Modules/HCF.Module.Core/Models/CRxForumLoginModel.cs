using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Core.Models
{
    public class CRxForumLoginModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public int Status { get; set; }

        public string EmailAddress { get; set; }

        public string RefreshToken { get; set; }
    }
}
