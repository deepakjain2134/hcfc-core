using System.Collections.Generic;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.BAL
{
    public interface ITIcraProjectService
    {
        int CrudTIcraProject(TIcraProject item, int mode);
        bool DeleteProjectFiles(int ProjectId, string fileIds, int type);
        List<TIcraProject> GetAllActiveTIcraProject(bool? HideInActive = true);
        Task<List<TIcraProject>> GetAllTIcraProject();
        List<TIcraProject> GetCountAllActiveTIcraProject(int UserID, int? vendorID, bool? HideInActive = true);
        Task<TIcraProject> GetTIcraProject(int ProjectId);
    }
}