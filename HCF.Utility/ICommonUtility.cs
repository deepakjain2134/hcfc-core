using HCF.Utility.AppConfig;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HCF.Utility
{
    public interface ICommonProvider
    {
        string CreateZipFileName(string fileName);
        string GetContentRootPath();
        string GetContentRootPath(string path);
        string FilePath(string path);
        string ConvertToAMPM(TimeSpan? timeSpan);
        string ConvertToFormatDate(DateTime dt);
        string GetIlsmStatus(int transStatus);
        string GetS3StaticImage(string path);
        string GetWebUrlPath(string filepath);
        string RemoveSpecialChars(string text);
        string GetFileName(string fileName);
        string GetFileExtenstion(string fileName);
        string CreateFileName(string fileName, string extension);
        string CreateFileName(string fileName);
    }

    public class CommonProvider : ICommonProvider
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHCFSession _httpContextAccessor;
        private readonly IAppSetting _appSetting;

        public CommonProvider(IAppSetting appSetting, IHCFSession httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _appSetting = appSetting;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetIlsmStatus(int transStatus)
        {
            string status = string.Empty;
            switch (transStatus)
            {
                case 1:
                    status = "Closed";
                    break;
                case 2:
                    status = "In Progress";
                    break;
                case 0:
                    status = "Open";
                    break;
            }
            return status;
        }
        public string ConvertToAMPM(TimeSpan? timeSpan)
        {
            if (timeSpan.HasValue)
            {
                var time = DateTime.Today.Add(timeSpan.Value);
                return time.ToString("hh:mm tt");
            }
            else { return string.Empty; }
        }
        public string ConvertToFormatDate(DateTime dt)
        {
            return dt.ToString("MMM d, yyyy");
        }       
        public string CreateZipFileName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {

                var ext = GetFileExtenstion(fileName);
                return $"{fileName.Replace("." + ext, "")}.{ext}";
            }
            else
                return string.Empty;
        }
        public string GetContentRootPath()
        {
            return _webHostEnvironment.ContentRootPath;
        }
        public string GetContentRootPath(string path)
        {
            if (!path.StartsWith("/"))
                path = $"/{path}";
            return string.Format(_webHostEnvironment.ContentRootPath + "{0}", path);
        }      
        public string FilePath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var bucketName = Convert.ToString(_httpContextAccessor.ClientNo);
                var absPath = _appSetting.ImageBasePath + bucketName + "/" + path.Replace("~/", "");
                return absPath.ToLower();
            }
            else
                return string.Empty;
        }
        public string GetS3StaticImage(string path)
        {
            var imageBasepath = _appSetting.CommonFileBasePath;
            return !string.IsNullOrEmpty(path) ? $"{imageBasepath}{path.Replace("~/", string.Empty)}" : string.Empty;
        }
        public string GetWebUrlPath(string filepath)
        {
            var imageBasepath = _appSetting.WebUrlPath;
            return !string.IsNullOrEmpty(filepath) ? $"{imageBasepath}{filepath.Replace("~/", string.Empty)}" : string.Empty;
        }
        public string RemoveSpecialChars(string text)
        {
            if (!string.IsNullOrEmpty(text))
                return Regex.Replace(text, @"[^0-9a-zA-Z]+", "_");
            else
                return "";
        }
        public string GetFileName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
                return fileName.TrimEnd('.').Remove(fileName.LastIndexOf('.') + 0);
            else
                return string.Empty;
        }
        public string GetFileExtenstion(string fileName) {

            return !string.IsNullOrEmpty(fileName) ? fileName.Split('.').Last() : string.Empty;
        }       
        public string CreateFileName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var ext = GetFileExtenstion(fileName);
                return $"{Guid.NewGuid().ToString()}.{ext}";
            }
            else
                return string.Empty;
        }
        public string CreateFileName(string fileName, string extension) {

            fileName = RemoveSpecialChars(fileName);
            return string.Format("{0}.{1}", fileName, extension);
        }
    }
}