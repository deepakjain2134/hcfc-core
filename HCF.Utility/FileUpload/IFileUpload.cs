using HCF.Utility.FileUpload;
using System.IO;
using System.Threading.Tasks;

namespace HCF.Utility
{
    public interface IFileUpload
    {
        Task<Stream> GetFile(string key, string bucketName);
        Task<bool> DeleteFile(string key, string bucketName);
        Task<bool> UploadFile(string bucketName, Stream inputStream, string fileName);
        bool CopyingObject(string sourceBucket, string destinationBucket, string objectKey, string destObjectKey);
        UploadedFiles SaveFile(UploadedFiles file, string bucketName, string base64String, string filePath, string orgFileName, string clientNo = "");
        UploadedFiles SaveFileUsingContent(string filesContent, string orgFileName, string type, string folderName, bool isCommonBucket = false);
        void DownloadFiles(string filePath, string fileName, string bucketName);
        Task<long> GetFileSize(string key);
        string ComputeHashOfFile(string base64String);
    }
}