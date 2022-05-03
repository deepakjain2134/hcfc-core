using System.Threading.Tasks;

namespace HCF.Module.Survey.Services
{
    public interface ITokenManagerService
    {
        Task<string> GetTokenAsync();
    }
}
