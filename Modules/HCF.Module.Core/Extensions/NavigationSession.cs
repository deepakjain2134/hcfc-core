using HCF.Module.Core.Models;
using HCF.Utility;
using HCF.Utility.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCF.Module.Core.Extensions
{
    public class NavigationMngSession : INavigationSession
    {
        #region ctor
        private readonly IWorkContext _workContext;

        public NavigationMngSession(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        #endregion

        #region Back Buttons

        public const string C_SESSION_KEY_CRUMB = "C_SESSION_KEY_CRUMB";
        public const string C_SESSION_KEY_BREADCRUMB = "C_SESSION_KEY_BREADCRUMB";

        private List<UserNavigation> NavigationSession
        {
            get
            {
                //var navigationSession = (List<UserNavigation>)HttpContext.Current.Session[C_SESSION_KEY_CRUMB];
                var sessionObj = ServiceLocator.ServiceProvider.GetService<IHCFSession>();
                var navigationSession = sessionObj.Get<List<UserNavigation>>(C_SESSION_KEY_CRUMB);
                if (navigationSession != null)
                    return navigationSession;

                navigationSession = new List<UserNavigation>();
                sessionObj.Set(C_SESSION_KEY_CRUMB, navigationSession);
                //HttpContext.Current.Session[C_SESSION_KEY_CRUMB] = navigationSession;
                return navigationSession;
            }
        }
        private Uri GetUrls()
        {
            var request = ServiceLocator.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.Request;
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString(),

            };
            return uriBuilder.Uri;
        }
        private string GetReffUrl()
        {
            var request = ServiceLocator.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.Request;
            string referer = request.Headers["Referer"].ToString();
            return referer;
        }
        public void AddCurrentPage(string screenName, int pageIndex, string displayName)
        {
            var pageUrl = GetUrls().AbsoluteUri; //HttpContext.Current.Request.Url.AbsoluteUri;
            if (!string.IsNullOrEmpty(pageUrl))
            {
                AddToList(screenName, pageIndex, displayName);
                AddToBreadCrumbList(screenName, pageIndex, displayName);
            }
        }
        public UserNavigation AddToList(string screenName, int pageIndex, string displayName)
        {

            var loalNavigationSession = NavigationSession;
            var urlReferrer = GetReffUrl();
            //if (HttpContext.Current.Request.UrlReferrer != null)
            //    urlReferrer = HttpContext.Current.Request.UrlReferrer.AbsoluteUri;

            var pageUrl = GetUrls().AbsoluteUri;

            var userNaviagtion = new UserNavigation();
            var lastUrl = loalNavigationSession.LastOrDefault(x => x.UrlReferrer == urlReferrer && x.Screen == screenName);
            if (!pageUrl.Contains("Isback"))
            {
                var _pageIndex = 1;
                if (loalNavigationSession.ToList().Count > 0)
                    _pageIndex = loalNavigationSession.ToList().Max(x => x.PageIndex) + 1;

                userNaviagtion.PageIndex = _pageIndex;
                userNaviagtion.Screen = screenName;
                userNaviagtion.PageUrl = pageUrl;
                userNaviagtion.UrlReferrer = urlReferrer;
                userNaviagtion.CreatedDate = DateTime.Now;
                userNaviagtion.DisplayName = displayName;
                userNaviagtion.UserId = _workContext.GetCurrentUser().Result.UserId;
                if (lastUrl == null && !string.IsNullOrEmpty(urlReferrer))
                {
                    loalNavigationSession.Add(userNaviagtion);
                    var sessionObj = ServiceLocator.ServiceProvider.GetService<IHCFSession>();
                    sessionObj.Set(C_SESSION_KEY_CRUMB, loalNavigationSession);
                }

            }
            return userNaviagtion;
        }
        public string GetBackPage(string screenName)
        {
            var redirectPage = NavigationSession.Where(x => x.Screen == screenName).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (redirectPage != null)
            {
                NavigationSession.RemoveAll(x => x.PageIndex > redirectPage.PageIndex);
                if (!redirectPage.UrlReferrer.Contains("Isback"))
                {
                    if (redirectPage.UrlReferrer.Contains("?"))
                        return redirectPage.UrlReferrer + "&Isback=1";
                    else
                        return redirectPage.UrlReferrer + "?Isback=1";
                }
                else
                    return redirectPage.UrlReferrer;
            }
            else
                return "/dashboard";
        }
        public string GetBackPagebyPageUrl(string pageUrl, string screenName)
        {
            var redirectPage = NavigationSession.Where(x => x.Screen.ToLower() == screenName.ToLower()).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (!string.IsNullOrEmpty(screenName))
            {
                if (redirectPage != null)
                {
                    NavigationSession.RemoveAll(x => x.PageIndex > redirectPage.PageIndex);
                    if (!redirectPage.UrlReferrer.Contains("Isback"))
                    {
                        if (redirectPage.UrlReferrer.Contains("?"))
                            return redirectPage.UrlReferrer + "&Isback=1";
                        else
                            return redirectPage.UrlReferrer + "?Isback=1";
                    }
                    else
                        return redirectPage.UrlReferrer;
                }
                else
                {
                    NavigationSession.Clear();
                    return "/dashboard";
                }
            }
            else
            {
                NavigationSession.Clear();
                return "/dashboard";
            }
        }
        public void ClearSession()
        {
            NavigationSession?.Clear();
        }
        public List<UserNavigation> GetAll()
        {
            var list = new List<UserNavigation>();
            if (NavigationSession != null)
                list = NavigationSession.ToList();
            return list;
        }

        #endregion

        #region Breadcrumb

        private List<UserNavigation> BreadCrumbSession
        {
            get
            {
                //var breadCrumbSession = (List<UserNavigation>)HttpContext.Current.Session[C_SESSION_KEY_BREADCRUMB];
                var breadCrumbSession = ServiceLocator.ServiceProvider.GetService<IHCFSession>().Get<List<UserNavigation>>(C_SESSION_KEY_BREADCRUMB);
                if (breadCrumbSession == null)
                {
                    breadCrumbSession = new List<UserNavigation>();
                    ServiceLocator.ServiceProvider.GetService<IHCFSession>().Set(C_SESSION_KEY_BREADCRUMB, breadCrumbSession);
                    //HttpContext.Current.Session[C_SESSION_KEY_BREADCRUMB] = breadCrumbSession;
                }
                return breadCrumbSession;
            }
        }
        public UserNavigation AddToBreadCrumbList(string screenName, int pageIndex, string displayName)
        {
            var localBreadCrumbSession = BreadCrumbSession;
            var urlReferrer = string.Empty;
            var absolutePath = string.Empty;

            if (localBreadCrumbSession.ToList().Count > 0)
            {
                if (localBreadCrumbSession[0].DisplayName == "General")
                {
                    localBreadCrumbSession.RemoveAll(x => x.DisplayName != "General");
                }
            }

            //BreadCrumbSession?.Clear();
            // if (HttpContext.Current.Request.UrlReferrer != null)
            if (!string.IsNullOrEmpty(GetReffUrl()))
            {
                urlReferrer = GetUrls().AbsoluteUri; ///HttpContext.Current.Request.UrlReferrer.AbsoluteUri;
                absolutePath = GetUrls().AbsolutePath; //HttpContext.Current.Request.Url.AbsolutePath;
            }

            var pageUrl = GetUrls().AbsoluteUri; //HttpContext.Current.Request.Url.AbsoluteUri;

            var redirectPage = localBreadCrumbSession.Where(x => x.Screen.ToLower() == screenName.ToLower()).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (redirectPage != null)
                localBreadCrumbSession.RemoveAll(x => x.PageIndex > redirectPage.PageIndex);
            int _pageIndex = 1;
            if (localBreadCrumbSession.ToList().Count > 0)
                _pageIndex = localBreadCrumbSession.ToList().Max(x => x.PageIndex) + 1;

            var userNaviagtion = new UserNavigation
            {
                PageIndex = _pageIndex,
                Screen = screenName,
                PageUrl = pageUrl,
                UrlReferrer = urlReferrer,
                CreatedDate = DateTime.Now,
                DisplayName = displayName,
                UserId = _workContext.GetCurrentUser().Result.UserId, //UserSession.CurrentUser.UserId,
                CurrentAbsolutePath = absolutePath
            };
            var lasturl = new UserNavigation();
            if (localBreadCrumbSession.Count > 0)
                lasturl = localBreadCrumbSession.OrderByDescending(x => x.CreatedDate).First();
            if (lasturl == null)
                localBreadCrumbSession.Add(userNaviagtion);
            else
            {
                if (lasturl.Screen != screenName)
                    localBreadCrumbSession.Add(userNaviagtion);
                else
                {
                    localBreadCrumbSession.Remove(lasturl);
                    localBreadCrumbSession.Add(userNaviagtion);
                }
            }

            var sessionObj = ServiceLocator.ServiceProvider.GetService<IHCFSession>();
            sessionObj.Set(C_SESSION_KEY_BREADCRUMB, localBreadCrumbSession);
            return userNaviagtion;
        }
        public void ClearBreadCrumb()
        {
            BreadCrumbSession?.Clear();
        }
        public string GetBackBreadCrumb(int pageIndex, string screenName)
        {
            var redirectPage = BreadCrumbSession.Where(x => x.Screen.ToLower() == screenName.ToLower()).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (redirectPage != null)
            {
                BreadCrumbSession.RemoveAll(x => x.PageIndex >= pageIndex);
                if (!redirectPage.PageUrl.Contains("Isback"))
                {
                    if (redirectPage.PageUrl.Contains("?"))
                        return redirectPage.PageUrl + "&Isback=1";
                    else
                        return redirectPage.PageUrl + "?Isback=1";
                }
                else
                    return redirectPage.PageUrl;
            }
            else
                return "/dashboard";
        }
        public string CurrentPage()
        {
            if (BreadCrumbSession.Count > 0)
            {
                return BreadCrumbSession.OrderByDescending(x => x.CreatedDate).FirstOrDefault().CurrentAbsolutePath;
            }
            else
                return string.Empty;

        }
        public List<UserNavigation> GetBreadCrumbs()
        {
            return BreadCrumbSession.ToList();
        }
        public string GetBackPageInspection(string screenName)
        {
            var redirectPage = BreadCrumbSession.Where(x => x.Screen == screenName).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (redirectPage == null)
                BreadCrumbSession.Where(x => x.Screen == "assets_AssetEps").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            else
                return redirectPage.PageUrl;

            return "/dashboard";
        }
        public bool IskeyExist(string screenName)
        {
            return BreadCrumbSession.Any(x => x.Screen == screenName);
        }
        public string GetPreviousPage(string screenName)
        {
            var redirectPage = NavigationSession.Where(x => x.Screen == screenName).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (redirectPage != null)
            {
                NavigationSession.RemoveAll(x => x.PageIndex > redirectPage.PageIndex);
                if (!redirectPage.UrlReferrer.Contains("Isback"))
                {
                    if (redirectPage.UrlReferrer.Contains("?"))
                        return redirectPage.PageUrl + "&Isback=1";
                    else
                        return redirectPage.PageUrl + "?Isback=1";
                }
                else
                    return redirectPage.PageUrl;
            }
            else
                return "/dashboard";
        }
        #endregion
    }
}