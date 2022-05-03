using HCF.Utility;
using Microsoft.AspNetCore.Http;
using System;

namespace HCF.Web.Base
{
    public class CookieUtilFactory : ICookieUtilFactory
    {
        private readonly HttpContext _httpContext;
      
        public CookieUtilFactory(IHttpContextAccessor contextAccessor)
        {
            _httpContext = contextAccessor.HttpContext;
        }

        public string GetCookieValue(string cookieName)
        {
            if (_httpContext.Request.Cookies.ContainsKey(cookieName))
            {
                var cookieValue = Convert.ToString(_httpContext.Request.Cookies[cookieName]);
                if (!string.IsNullOrEmpty(cookieValue))
                    return Conversion.Decrypt(cookieValue, cookieName);
            }
            return string.Empty;

        }
        public void CreateCookie(string cookieName, string value, int? expirationDays)
        {
            if (CookieExists(cookieName))
                DeleteCookie(cookieName);

            var cookieCalue = Conversion.Encrypt(value, cookieName);

            _httpContext.Response.Cookies.Append(cookieName, cookieCalue, new CookieOptions
            {
                Expires = (expirationDays.HasValue) ? DateTime.UtcNow.AddDays(expirationDays.Value) : DateTime.UtcNow.AddYears(5),
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict
            });

        }
        public void DeleteCookie(string cookieName)
        {
            if (_httpContext.Request.Cookies.ContainsKey(cookieName))
            {
                _httpContext.Response.Cookies.Delete(cookieName);
            }
        }
        public bool CookieExists(string cookieName)
        {
            if (_httpContext.Request.Cookies.ContainsKey(cookieName))
                return true;
            else
                return false;
        }
        public void DeleteAllCookies()
        {
            var cookies = _httpContext.Request.Cookies.Keys;
            foreach (var cook in cookies)
            {
                DeleteCookie(cook);
            }
        }
    }
}