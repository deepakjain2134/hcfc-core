using HCF.BDO;
using HCF.DAL;
using HCF.Infrastructure.Data;
using HCF.Infrastructure.Helpers;
using HCF.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Module.Core.Extensions
{
    public class WorkContext : IWorkContext
    {
        private const string UserGuidCookiesName = "CRxUserGuid";
        private const string OrgGuidCookiesName = "_crxtaean56aa44t";

        private UserProfile _currentUser;
        private readonly HttpContext _httpContext;

        private readonly IRepository<Organization> _orgRepository;

        // private UserManager<UserProfile> _userManager;

        public WorkContext(IHttpContextAccessor contextAccessor
          //  , UserManager<UserProfile> userManager
          , IRepository<Organization> orgRepository
            )
        {
            _httpContext = contextAccessor.HttpContext;
            _orgRepository = orgRepository;
            // _userManager = userManager;
        }

        public async Task<UserProfile> GetCurrentUser()
        {
            if (_currentUser != null)
            {
                return _currentUser;
            }

            var contextUser = _httpContext.User; //await _userManager.GetUserAsync(_httpContext.User); // 
            if (contextUser != null)
            {
                _currentUser = new UserProfile
                {
                    UserId = Convert.ToInt32(_httpContext.User.GetUserId()),
                    ClientNo = Convert.ToInt32(_httpContext.User.GetClientNo())
                };
            }
            return await Task.FromResult(_currentUser);
        }

        private Guid? GetGuidFromCookies(string key)
        {
            if (_httpContext.Request.Cookies.ContainsKey(key))
            {
                return Guid.Parse(_httpContext.Request.Cookies[key]);
            }
            return null;
        }

        private void SetUserGuidCookies(string key, string value)
        {
            _httpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddYears(5),
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict
            });
        }

        public async Task<Organization> GetCurrnetOrg()
        {
            var org = new Organization();
            if (_httpContext.Request.Cookies.ContainsKey(OrgGuidCookiesName))
            {
                string org_details = _httpContext.Request.Cookies[OrgGuidCookiesName];
                if (!string.IsNullOrEmpty(org_details))
                {
                    org_details = org_details.Decrypt(OrgGuidCookiesName);
                    org.ClientNo = Convert.ToInt32(org_details.Split('%')[0]);
                    org.DbName = org_details.Split('%')[1];
                    org.Orgkey = Guid.Parse(org_details.Split('%')[2]);
                }
            }
            org = await _orgRepository.Query()
                .Where(x => x.Orgkey == org.Orgkey)
                                     .Select(m => new Organization()
                                     {
                                         DbName = m.DbName,
                                         ClientNo = m.ClientNo,
                                         Name = m.Name,
                                         Orgkey = m.Orgkey
                                     })
                                     .FirstOrDefaultAsync();
            return await Task.FromResult(org);
        }

        public async Task<bool> SetCurrentOrg(Organization org)
        {
            string value = string.Format("{0}%{1}%{2}", org.ClientNo, org.DbName, org.Orgkey);
            SetUserGuidCookies(OrgGuidCookiesName, value.Encrypt(OrgGuidCookiesName));
            return await Task.FromResult(true);
        }

        public async Task<bool> SetLoginToken(string token)
        {
            SetUserGuidCookies("OrgGuidCookiesNameToken", token);
            return await Task.FromResult(true);
        }

        public async Task<string> GetLoginToken()
        {
            var token = "";
            if (_httpContext.Request.Cookies.ContainsKey("OrgGuidCookiesNameToken"))
            {
                token = _httpContext.Request.Cookies["OrgGuidCookiesNameToken"];
            }
            return await Task.FromResult(token);
        }
    }
}
