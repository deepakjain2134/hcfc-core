using HCF.BDO;
using HCF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Module.Core.Services
{
    public class UserLoginService : IUserLoginService
    {
        private readonly IRepository<UserLogin> _loginService;

        public UserLoginService(IRepository<UserLogin> loginService)
        {
            _loginService = loginService;
        }

        public async Task<UserLogin> GetUserLogin(Guid refreshToken)
        {
            return await _loginService.Query().FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        }

        public async Task<IEnumerable<UserLogin>> GetUserLogins(int userId)
        {
            return await _loginService.Query().Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<bool> LogoutUserAsync(Guid refreshToken, string url)
        {
            var isExistingUserLogin = await GetUserLogin(refreshToken);
            if (isExistingUserLogin != null)
            {
                isExistingUserLogin.LogOffDate = DateTime.UtcNow;
                isExistingUserLogin.IsLogOn = false;
                isExistingUserLogin.LastLoginURL = url;
                _loginService.Update(isExistingUserLogin);
                await _loginService.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> Save(UserLogin userlogin)
        {
            try
            {
                userlogin.IsNewDevice = await IsNewDeviceLogin(userlogin);
                _loginService.Add(userlogin);
                await _loginService.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        public async Task<bool> AuthorizeDevice(Guid refreshToken)
        {
            var isExistingUserLogin = await GetUserLogin(refreshToken);
            if (isExistingUserLogin != null)
            {
                isExistingUserLogin.IsNewDevice = false;
                _loginService.Update(isExistingUserLogin);
                await _loginService.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> IsNewDeviceLogin(UserLogin userlogin)
        {
            bool isNewDevice = true;
            var userLogins = await GetUserLogins(Convert.ToInt32(userlogin.UserId));
            if (userLogins.Any())
                isNewDevice = !userLogins.Any(x => (x.ip == userlogin.ip || x.DeviceId == userlogin.DeviceId) && !x.IsNewDevice);
            return isNewDevice;
        }
    }
}
