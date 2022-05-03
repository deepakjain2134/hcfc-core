namespace HCF.Web.Base
{
    public interface ICookieUtilFactory
    {
        bool CookieExists(string cookieName);
        void CreateCookie(string cookieName, string value, int? expirationDays);
        void DeleteAllCookies();
        void DeleteCookie(string cookieName);
        string GetCookieValue(string cookieName);
    }
}