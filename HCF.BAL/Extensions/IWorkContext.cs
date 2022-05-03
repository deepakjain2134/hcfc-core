using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL.Extensions
{
    public interface IWorkContext
    {
        Task<UserProfile> GetCurrentUser();
    }
}
