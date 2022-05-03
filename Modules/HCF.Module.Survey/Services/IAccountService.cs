using HCF.Module.Survey.Model;
using System.Threading.Tasks;

namespace HCF.Module.Survey.Services
{
    public interface IAccountService
    {
        User User { get; }
        Task<bool> LoginAsync(LoginModel model);
        Task<bool> LogoutAsync();
        Task<User> GetCurrentUser();
    }
}
