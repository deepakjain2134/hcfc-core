using System;
using HCF.Utility;
using HCF.Utility.AppConfig;
using Microsoft.AspNetCore.Hosting;

namespace HCF.Module.Core.Utility
{
    public class CommonCoreFactory : ICommonCoreFactory
    {
        #region ctor

        private readonly IAppSetting _appSettings;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHCFSession _session;

        public CommonCoreFactory(IAppSetting appSettings, IWebHostEnvironment webHostEnvironment, IHCFSession session)
        {
            _appSettings = appSettings;
            _webHostEnvironment = webHostEnvironment;
            _session = session;

        }

        #endregion

        #region Files and File Paths
        public string FilePath(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(_session.ClientNo))
                return string.Empty;

            var bucketName = Convert.ToString(_session.ClientNo);
            var absPath = _appSettings.ImageBasePath + bucketName + "/" + path.Replace("~/", "");
            return absPath.ToLower();
        }

        #endregion
    }
}