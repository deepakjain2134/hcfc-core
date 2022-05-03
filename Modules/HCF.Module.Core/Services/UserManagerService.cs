using HCF.BDO;
using HCF.Utility.Enum;
using HCF.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Shyjus.BrowserDetection;
using HCF.Utility.AppConfig;
using HCF.DAL;

namespace HCF.Module.Core.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly IBrowserDetector _browserDetector;
        private readonly IAppSetting _appSetting;

        public UserManagerService(IBrowserDetector browserDetector, IAppSetting appSetting)
        {
            _browserDetector = browserDetector;
            _appSetting = appSetting;
        }

        public async Task SignInAsync(HttpContext httpContext, UserProfile users, Organization org, string refreshToken, bool isPersistent = false)
        {
            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(users.UserId)),
                        new Claim(ClaimTypes.Name, users.FullName),
                        new Claim(ClaimTypes.Email, users.Email),
                        new Claim(ClaimTypes.Role, string.Join(",", users.UserGroups.Select(x=>x.Name).ToList())),
                        new Claim(ClaimTypes.NameIdentifier,Convert.ToString(users.UserId)),
                        new Claim("FirstName",users.FirstName),
                        new Claim("LastName",users.LastName),
                        new Claim("VendorId",(users.VendorId.HasValue ? Convert.ToString(users.VendorId.Value):"0")),
                        new Claim(SessionKey.OrgName,org.DbName),
                        new Claim(SessionKey.UserOrgName,org.Name),
                        new Claim(SessionKey.ClientNo,Convert.ToString(org.ClientNo)),
                        new Claim(SessionKey.LogOnToken,Convert.ToString(refreshToken)),
                        new Claim("IsPowerUser",Convert.ToString(users.IsPowerUser)),
                        new Claim("IsCRxUser",Convert.ToString(users.IsCRxUser)),
                        new Claim("IsSystemUser",Convert.ToString(users.IsSystemUser)),
                        new Claim("RoleIds", string.Join(",", users.UserGroups.Select(x=>x.GroupId).ToList())),
                        new Claim("Roles",string.Join(",", users.Roles.Select(x=>x.RoleName).ToList())),
                        new Claim("PhoneNumber",(!string.IsNullOrEmpty(users.PhoneNumber))?Convert.ToString(users.PhoneNumber):""),
                        new Claim("UserProfileId",Convert.ToString(users.UserGuid)),
                        new Claim("Orgkey",Convert.ToString(org.Orgkey)),
                        new Claim("UserAdditionalRoleIds",string.Join(",", users.AdditionalRoles.Select(x=>x.RoleId).ToList()))};

            var identity = new ClaimsIdentity(claims, AuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync(AuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = isPersistent
            });
        }

        public async Task SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(AuthenticationDefaults.AuthenticationScheme);
        }

        public Task<UserLogin> GetBrowserInfo(HttpContext httpContext)
        {
            var ipAddress = httpContext.Connection.RemoteIpAddress.ToString();
            var loginInfo = new UserLogin
            {
                RefreshToken = Guid.NewGuid(),
                ip = ipAddress,
                OsName = _browserDetector.Browser.OS,
                BrowserName = string.Format("{0} ({1})", _browserDetector.Browser.Name, _browserDetector.Browser.Version),
                BuildVersion = _appSetting.BuildVersion,
                DeviceType = _browserDetector.Browser.DeviceType,
                DeviceTypeId=3
            };
            return Task.FromResult(loginInfo);
        }

        public Task AddClaims()
        {
            throw new NotImplementedException();
        }
    }
}
