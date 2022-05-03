using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HCF.BDO;

namespace Hcf.Api.Application
{
    public interface IApiCommon
    {
        Guid GetActivityId();
        string GetBaseFolderName();
        string GetBucketName();
        string GetCommonBucketName();
        Organization GetOrganization(int clientNo);
        List<TFiles> GetTFiles(string files);
        void SaveExercisesFiles(IEnumerable<TExerciseFiles> files, int exerciseId);
        Files SaveFile(string filesContent, string orgFileName, string type, string folderName, bool isCommonBucket = false);
        string HostingEnvAppPhysicalPath();
        Task<bool> DeleteFile(string key, string filepath);
    }
}