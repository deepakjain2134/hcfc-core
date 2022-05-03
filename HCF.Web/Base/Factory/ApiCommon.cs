using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Utility.AppConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hcf.Api.Application
{
    public class ApiCommon : IApiCommon
    {
        private readonly ICommonService _commonService;
        private readonly IExercisesService _exercisesService;
        private readonly IOrganizationService _organizationService;
        private readonly IFileUpload _fileUpload;
        private readonly IHCFSession _session;
        private readonly IAppSetting _appSetting;

        public ApiCommon(IHCFSession session, IAppSetting appSetting,IFileUpload fileUpload,ICommonService commonService, IExercisesService exercisesService, IOrganizationService organizationService)
        {
            _session = session;
            _appSetting = appSetting;
            _commonService = commonService;
            _exercisesService = exercisesService;
            _organizationService = organizationService;
            _fileUpload = fileUpload;

        }
        public Files SaveFile(string filesContent, string orgFileName, string type, string folderName, bool isCommonBucket = false)
        {
            var file = new Files();
            var fileName = orgFileName.Replace(" ", "_");
            file.Extension = Path.GetExtension(fileName);
            file.FileName = fileName;

            var uploadFiles = _fileUpload.SaveFileUsingContent(filesContent, orgFileName, type, folderName, isCommonBucket);
            if (uploadFiles != null)
                file.FilePath = uploadFiles.FilePath;
            return file;
        }

        public async Task<bool> DeleteFile(string key, string filepath)
        {
            var Filepath = filepath;
            var fileName = key;
            var BucketName = GetBucketName();
            bool result = await _fileUpload.DeleteFile(fileName,BucketName);            
            return result;
        }

        public  string GetBucketName()
        {
            return _appSetting.BucketName;
            //return HcfSession.ClientNo >0 ? Convert.ToString(HcfSession.ClientNo) : "";
        }

        public  string GetBaseFolderName()
        {
            // return HCF.Utility.AppSettings.BucketName;
            return !string.IsNullOrEmpty(_session.ClientNo) ? Convert.ToString(_session.ClientNo) : "";
        }

        public string HostingEnvAppPhysicalPath()
        {
            return string.Empty;
        }


        public  string GetCommonBucketName()
        {
            return "crximages";

        }
        public  Organization GetOrganization(int clientNo)
        {
            return _organizationService.GetOrganizations().FirstOrDefault(x => x.ClientNo == clientNo);
        }
        public  Guid GetActivityId()
        {
            var newActivityId = Guid.NewGuid();
            return newActivityId;
        }

        public  List<TFiles> GetTFiles(string files)
        {
            var tfiles = new List<TFiles>();
            foreach (var item in files.Split(','))
            {
                int fileId = Convert.ToInt32(item.Split('_')[0]);
                //string tblName = item.Split('_')[1];
                string tblName = "";
                if (item.Split('_').Length > 1)
                {
                    tblName = item.Split('_')[1];
                }
                var tfile = _commonService.GetFile(fileId, tblName);
                if (tfile != null)
                    tfiles.Add(tfile);
            }
            return tfiles;
        }
        public  void SaveExercisesFiles(IEnumerable<TExerciseFiles> files, int exerciseId)
        {
            foreach (var file in files)
            {
                if (!string.IsNullOrEmpty(file.TFileIds) && Convert.ToInt32(file.TFileIds) > 0)
                {
                    var tfile = _commonService.GetFile(Convert.ToInt32(file.TFileIds), "");
                    file.TExerciseId = exerciseId;
                    file.FileName = tfile.FileName;
                    file.FilePath = tfile.FilePath;
                    file.IsActive = true;
                    _exercisesService.InsertTExerciseFiles(file);
                }
                if (!string.IsNullOrEmpty(file.FilesContent))
                {
                    var newFile = SaveFile(file.FilesContent, file.FileName, "exercise", null);
                    file.TExerciseId = exerciseId;
                    file.FileName = newFile.FileName;
                    file.FilePath = newFile.FilePath;
                    file.IsActive = true;
                    _exercisesService.InsertTExerciseFiles(file);
                }
            }
        }
    }
}