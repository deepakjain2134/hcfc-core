using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using HCF.Utility.AppConfig;
using HCF.Utility.FileUpload;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HCF.Utility
{
    public class AmazonFileUpload : IFileUpload
    {
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.USEast1;
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<AmazonFileUpload> _logger;
        private readonly ICommonProvider _commonProvider;
        private readonly IAppSetting _appSetting;
        private readonly IHCFSession _session;
        private static string GetCommonBucketName = "crximages";

        public AmazonFileUpload(IAmazonS3 s3Client, ILogger<AmazonFileUpload> logger, ICommonProvider commonProvider, IAppSetting appSetting, IHCFSession session)
        {
            _s3Client = s3Client;
            _logger = logger;
            _commonProvider = commonProvider;
            _appSetting = appSetting;
            _session = session;
        }

        public static bool CreateBucket(string bucketName)
        {
            return true;

        }

        public UploadedFiles SaveFile(UploadedFiles file, string bucketName, string base64String, string filePath, string orgFileName, string clientNo = "")
        {
            filePath = filePath.Replace("~/", string.Empty);
            if (!string.IsNullOrEmpty(filePath) && filePath.StartsWith("/") && filePath.Length > 1)
                filePath = filePath.Substring(1);

            if (string.IsNullOrEmpty(clientNo))
                filePath = string.Format("{0}/{1}", GetBaseFolderName(), filePath);
            else
                filePath = string.Format("{0}/{1}", clientNo, filePath);
            try
            {
                byte[] bytes = Convert.FromBase64String(base64String);               
                var request = new PutObjectRequest
                {
                    MD5Digest = file.MD5Digest,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead,
                    Key = filePath.ToLower()
                };

                using var ms = new MemoryStream(bytes);
                request.InputStream = ms;
                PutObjectResponse responseObj = _s3Client.PutObjectAsync(request).Result;
                file.ETag = responseObj.ETag;

            }
            catch (AmazonS3Exception exe)
            {
                _logger.LogError(exe.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return file;
        }

        public bool CopyingObject(string sourceBucket, string destinationBucket, string objectKey, string destObjectKey)
        {            
            bool status = true;           
            try
            {
                var request = new CopyObjectRequest
                {
                    SourceBucket = sourceBucket,
                    SourceKey = objectKey.Replace("~/", string.Empty).ToLower(),
                    DestinationBucket = destinationBucket,
                    DestinationKey = destObjectKey.Replace("~/", string.Empty).ToLower(),
                    CannedACL = S3CannedACL.PublicRead,
                };
                _s3Client.CopyObjectAsync(request);
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex.Message);
                status = false;
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                status = false;               
            }
            return status;
        }

        public UploadedFiles SaveFileUsingContent(string filesContent, string orgFileName, string type, string folderName, bool isCommonBucket = false)
        {
            string fileName = GetNewFileName(orgFileName);
            var file = new UploadedFiles();
            string fileSavePath;
            if (!string.IsNullOrEmpty(type) && type == "ZipFile")
            {
                if (!string.IsNullOrEmpty(folderName))
                    fileSavePath = FileSavePath(type) + folderName + "/" + _commonProvider.CreateZipFileName(fileName);
                else
                    fileSavePath = FileSavePath(type) + _commonProvider.CreateZipFileName(fileName);
            }
            else
            {
                if (!string.IsNullOrEmpty(folderName))
                    fileSavePath = string.Format("{0}{1}/{2}/{3}", FileSavePath(type), folderName, Guid.NewGuid().ToString().Replace("-", ""), fileName);
                else
                    fileSavePath = string.Format("{0}{1}/{2}", FileSavePath(type), Guid.NewGuid().ToString().Replace("-", ""), fileName);
            }

            file.FilePath = fileSavePath.ToLower();
            var bucketName = GetBucketName();
            if (isCommonBucket)
                bucketName =GetCommonBucketName;

            file.MD5Digest = ComputeHashOfFile(filesContent);
            var status = CreateBucket(bucketName);
            if (status)
                file = SaveFile(file, bucketName, filesContent, file.FilePath, orgFileName);

            return file;
        }

        private string GetNewFileName(string orgFileName)
        {
            var fileExtenstion = _commonProvider.GetFileExtenstion(orgFileName);
            var newFileName= _commonProvider.GetFileName(orgFileName);
            var fileName = _commonProvider.CreateFileName(newFileName, fileExtenstion);
            return fileName;
        }

        public async Task<Stream> GetFile(string key, string bucketName)
        {
            var response = await _s3Client.GetObjectAsync(bucketName, key);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                return response.ResponseStream;
            else
                return null;
        }

        public async Task<long> GetFileSize(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var bucketName = GetBucketName();
                key = string.Format("{0}/{1}", _session.ClientNo, key).Replace("~/","");
                try
                {
                    var response = await _s3Client.GetObjectAsync(bucketName, key.ToLower());
                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        return response.ContentLength;
                }
                catch (AmazonS3Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
                    
            }
            return 0;
        }


        public void DownloadFiles(string filePath, string fileName, string bucketName)
        {
            TransferUtility fileTransferUtility = new TransferUtility(new AmazonS3Client("AKIA5D4XCJRBPYQZML5H", "xDdEAiTIyjxx1yXZLJf5ORFZVv4QZDWZqKEBB/NG", Amazon.RegionEndpoint.USEast1));

            // Note the 'fileName' is the 'key' of the object in S3 (which is usually just the file name)
            fileTransferUtility.Download(filePath, bucketName, fileName);
        }

        public async Task<bool> DeleteFile(string key, string bucketName)
        {
            try
            {
                DeleteObjectResponse response = await _s3Client.DeleteObjectAsync(bucketName, key);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return false;
        }

        public async Task<bool> UploadFile(string bucketName, Stream inputStream, string fileName)
        {
            try
            {
                var request = new PutObjectRequest()
                {
                    InputStream = inputStream,
                    BucketName = bucketName,
                    Key = fileName
                };
                var response = await _s3Client.PutObjectAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return false;
        }

        #region Private methods

        private string GetBucketName()
        {
            return _appSetting.BucketName;
        }

        private string GetBaseFolderName()
        {
            return (!string.IsNullOrEmpty(_session.ClientNo)) ? Convert.ToString(_session.ClientNo) : "";
        }             


        private string FileSavePath(string type)
        {
            return _appSetting.FileBasePath.ToLower() + "" + type.ToLower() + "/";
        }

        public string ComputeHashOfFile(string base64String)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64String);
                using var md5 = MD5.Create();
                var hash = md5.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return string.Empty;
        }

        #endregion
    }
}