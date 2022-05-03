using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IEmailSync
    {
        string RenameFileName(string FileName);
        void SaveAttachments(int DocumentRepoId, List<EmailFiles> files, string DbName);
    }
}