using HCF.BDO;
using HCF.DAL.Contexts;
using HCF.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.DAL.Interfaces
{
    public interface IUserLoginRepository //: IGenericRepositoryAsync<UserLogin>
    {
        Task<IEnumerable<UserLogin>> GetUserLogins(int userId);

        Task<UserLogin> GetUserLogin(Guid refreshToken);
    }


}
