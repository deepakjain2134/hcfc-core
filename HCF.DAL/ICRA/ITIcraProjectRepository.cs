using System.Collections.Generic;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.DAL
{
    public interface ITIcraProjectRepository
    {
        int CrudTIcraProject(TIcraProject item, int mode);
        bool DeleteProjectFiles(int ProjectId, string fileIds, int type);
        List<TIcraProject> GetAllActiveTIcraProject(bool? hideInActive = true);
        Task<List<TIcraProject>> GetAllTIcraProject();
        List<TIcraProject> GetCountAllActiveTIcraProject(int UserID, int? vendorID, bool? HideInActive = true);
    }
}