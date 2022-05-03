using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface ITFilesRepository
    {
        TFiles GetFile(int fileId, string tbName);
        int Save(TFiles item);
        List<TFiles> GetTFiles(string files);
    }
}