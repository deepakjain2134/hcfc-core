using HCF.BDO;
using System.Threading.Tasks;

namespace HCF.Module.Core.Extensions
{
    public interface IWorkContext
    {
        Task<UserProfile> GetCurrentUser();

        Task<Organization> GetCurrnetOrg();

        Task<bool> SetCurrentOrg(Organization org);

        Task<string> GetLoginToken();

        Task<bool> SetLoginToken(string token);
    }
}
