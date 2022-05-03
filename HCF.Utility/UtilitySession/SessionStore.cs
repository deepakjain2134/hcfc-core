using Microsoft.AspNetCore.Http;
using System;


namespace HCF.Utility
{
    public class SessionStore : IHCFSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        //private ISession _session => _httpContextAccessor.HttpContext.Session;

        public SessionStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Exception Exception
        {
            get
            {
                var clientNo = Get<Exception>(SessionKey.Exception);
                if (clientNo != null)
                    return clientNo;
                else
                    return null;
            }
        }

        public string ClientNo
        {
            get
            {

                //var clientClientNo = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == SessionKey.ClientNo);
                //if (clientClientNo != null)
                //    return clientClientNo.Value;
                //else
                //    return string.Empty;

                var clientNo = Get<string>(SessionKey.ClientNo);
                if (clientNo != null)
                    return clientNo;
                else
                    return string.Empty;

                //var contextUser = _httpContextAccessor.HttpContext.User;
                //string clientNo = contextUser.GetClientNo();
                //if (!string.IsNullOrEmpty(clientNo))
                //{ return clientNo; }
                //else
                //    return string.Empty;
            }
        }


        public string IsExistTagDeficiency
        {
            get
            {
                var data = Get<string>(SessionKey.IsExistTagDeficiency);
                if (data != null)
                    return data;
                else
                    return string.Empty;
            }
        }

        public string IsGuestUser
        {
            get
            {
                var data = Get<string>(SessionKey.IsGuestUser);
                if (data != null)
                    return data;
                else
                    return string.Empty;
            }
        }
        public string combinevaluearr
        {
            get
            {
                var data = Get<string>(SessionKey.combinevaluearr);
                if (data != null)
                    return data;
                else
                    return string.Empty;
            }
        }

        public string ClientDbName
        {
            get
            {

                //var currentUser = new UserProfile();
                //var contextUser = ServiceLocator.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.User;
                //if (contextUser.Identity.IsAuthenticated)
                //{
                //    currentUser.UserId = Convert.ToInt32(contextUser.GetUserId());
                //    currentUser.Email = contextUser.GetUserEmail();
                //    currentUser.FirstName = contextUser.GetFirstName();
                //    currentUser.LastName = contextUser.GetLastName();
                //    currentUser.VendorId = Convert.ToInt32(contextUser.GetVendorId());
                //}
                //var contextUser = _httpContextAccessor.HttpContext.User;
                // string dbName=contextUser.GetCurrentDb();
                //var clientDBName = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == SessionKey.ClientDBName);
                //if (clientDBName != null)
                //    return clientDBName.Value;
                //else
                //    return string.Empty;

                var clientDBName = Get<string>(SessionKey.ClientDBName);
                if (clientDBName != null)
                    return clientDBName;
                else
                    return string.Empty;
            }
        }

        public void clear()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        public T Get<T>(string key)
        {
            var data = _httpContextAccessor.HttpContext.Session.Get<T>(key);
            return (T)data;
        }

        public void Set<T>(string key, T entry)
        {
            _httpContextAccessor.HttpContext.Session.Set<T>(key, entry);
        }
    }
}