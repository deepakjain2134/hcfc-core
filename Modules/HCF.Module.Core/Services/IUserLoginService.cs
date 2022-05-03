using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Core.Services
{
    public interface IUserLoginService
    {
        Task<IEnumerable<UserLogin>> GetUserLogins(int userId);

        Task<UserLogin> GetUserLogin(Guid refreshToken);

        Task<bool> LogoutUserAsync(Guid refreshToken, string url);

        Task<bool> Save(UserLogin userlogin);

        Task<bool> AuthorizeDevice(Guid refreshToken);

        Task<bool> IsNewDeviceLogin(UserLogin userlogin);
    }
}
