using HCF.BDO.ModuleTraining;
using HCF.Module.Core.Models;
using System.Collections.Generic;

namespace HCF.Module.Core.Extensions
{
    public interface INavigationSession
    {
        void AddCurrentPage(string screenName, int pageIndex, string displayName);
        UserNavigation AddToBreadCrumbList(string screenName, int pageIndex, string displayName);
        UserNavigation AddToList(string screenName, int pageIndex, string displayName);
        void ClearBreadCrumb();
        void ClearSession();
        string CurrentPage();
        List<UserNavigation> GetAll();
        string GetBackBreadCrumb(int pageIndex, string screenName);
        string GetBackPage(string screenName);
        string GetBackPagebyPageUrl(string pageUrl, string screenName);
        string GetBackPageInspection(string screenName);
        List<UserNavigation> GetBreadCrumbs();
        string GetPreviousPage(string screenName);
        bool IskeyExist(string screenName);      
    }
}