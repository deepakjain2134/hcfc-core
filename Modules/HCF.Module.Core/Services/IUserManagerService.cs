using HCF.BDO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HCF.Module.Core.Services
{
    public interface IUserManagerService
    {
        Task SignInAsync(
              HttpContext httpContext,
              UserProfile users, Organization org, 
              string refreshToken,
              bool isPersistent = false);

        Task SignOutAsync(HttpContext httpContext);

        Task<UserLogin> GetBrowserInfo(HttpContext httpContext);

        Task AddClaims();

    }
}
