using HCF.BDO;
using HCF.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace HCF.BAL.Extensions
{
    public class WorkContext : IWorkContext
    {
        private const string UserGuidCookiesName = "CRxUserGuid";
        private UserProfile _currentUser;
        private HttpContext _httpContext;
        private readonly IConfiguration _configuration;

        public WorkContext(IHttpContextAccessor contextAccessor)
        {
            _httpContext = contextAccessor.HttpContext;
        }

        public async Task<UserProfile> GetCurrentUser()
        {
            if (_currentUser != null)
            {
                return _currentUser;
            }

            var contextUser = _httpContext.User;

            if (contextUser != null)
            {
                _currentUser = new UserProfile();
                _currentUser.UserId = Convert.ToInt32(contextUser.GetUserId());
            }

            return _currentUser;
        }

        private Guid? GetUserGuidFromCookies()
        {
            if (_httpContext.Request.Cookies.ContainsKey(UserGuidCookiesName))
            {
                return Guid.Parse(_httpContext.Request.Cookies[UserGuidCookiesName]);
            }

            return null;
        }

        private void SetUserGuidCookies()
        {
            _httpContext.Response.Cookies.Append(UserGuidCookiesName, _currentUser.UserId.ToString(), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddYears(5),
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict
            });
        }
    }
}
