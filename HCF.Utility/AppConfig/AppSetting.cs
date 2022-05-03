using Microsoft.Extensions.Configuration;
using System;

namespace HCF.Utility.AppConfig
{
    public class AppSetting : IAppSetting
    {
        private readonly IConfiguration _configuration;

        public AppSetting(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string HCFConnection
        {
            get
            {
                return this._configuration["ConnectionStrings:HCFConnection"];
            }
        }

        public string HcfHospitalConnection
        {
            get
            {
                return this._configuration["ConnectionStrings:HCFConnection"];
            }
        }

        public string GetConnectionString(string connectionName)
        {
            return this._configuration.GetConnectionString(connectionName);
        }

        public string EmailID
        {
            get
            {
                return this._configuration["AppSeettings:EmailID"];
            }
        }

        public string ToErrorEmail
        {
            get
            {
                return this._configuration["AppSeettings:ToErrorEmail"];
            }
        }

       

        public string BucketName
        {
            get
            {
                return this._configuration["AppSeettings:BucketName"];
            }
        }

        public string AccountKey
        {
            get
            {
                return this._configuration["AppSeettings:AccountKey"];
            }
        }

        public string SampleFilePath
        {
            get
            {
                return this._configuration["AppSeettings:SampleFilePath"];
            }
        }

        public string ShowCRxUsers
        {
            get
            {
                return this._configuration["AppSeettings:ShowCRxUsers"];
            }
        }

        public string CommonDB
        {
            get
            {
                return this._configuration["AppSeettings:CommonDB"];
            }
        }

        public string WebUrlPath
        {
            get
            {
                return this._configuration["AppSeettings:WebUrlPath"];
            }
        }

        public string ImageBasePath
        {
            get
            {
                return this._configuration["AppSeettings:ImageBasePath"];
            }
        }

        public string CommonFileBasePath
        {
            get
            {
                return this._configuration["AppSeettings:CommonFileBasePath"];
            }
        }

        public string BuildVersion
        {
            get
            {
                return this._configuration["AppSeettings:BuildVersion"];
            }
        }

        public string GeoLocationApi
        {
            get
            {
                return this._configuration["AppSeettings:GeoLocationApi"];
            }
        }

        public string CommonApiUrl
        {
            get
            {
                return this._configuration["AppSeettings:CommonApiUrl"];
            }
        }

        public string ApiPath
        {
            get
            {
                return this._configuration["AppSeettings:ApiPath"];
            }
        }

        public string S3FileBasePath
        {
            get
            {
                return this._configuration["AppSeettings:S3FileBasePath"];
            }
        }

        public string UserDefaultImage
        {
            get
            {
                return this._configuration["AppSeettings:UserDefaultImage"];
            }
        }

        public string SenderMailFrom
        {
            get
            {
                return this._configuration["AppSeettings:SenderMailFrom"];
            }
        }

        public string DueWithInDays
        {
            get
            {
                return this._configuration["AppSeettings:DueWithInDays"];
            }
        }

        public string WebBaseUrl
        {
            get
            {
                return this._configuration["AppSeettings:WebBaseUrl"];
            }
        }

        public string PageSize
        {
            get
            {
                return this._configuration["AppSeettings:PageSize"];
            }
        }

        public string BrowseFilePath
        {
            get
            {
                return this._configuration["AppSeettings:BrowseFilePath"];
            }
        }

        public string LogCreate
        {
            get
            {
                return this._configuration["AppSeettings:LogCreate"];
            }
        }

        public string ErrorlogFilePath
        {
            get
            {
                return this._configuration["AppSeettings:ErrorlogFilePath"];
            }
        }

        public string BarCodePage
        {
            get
            {
                return this._configuration["AppSeettings:BarCodePage"];
            }
        }

        public string BarCodeImage
        {
            get
            {
                return this._configuration["AppSeettings:BarCodeImage"];
            }
        }

        //public bool EnableOptimizations
        //{
        //    get
        //    {
        //        return Convert.ToBoolean(this._configuration["AppSeettings:EnableOptimizations"]);
        //    }
        //}

        public string CommonHospitalMessage
        {
            get
            {
                return this._configuration["AppSeettings:CommonHospitalMessage"];
            }
        }

        public string TipType
        {
            get
            {
                return this._configuration["AppSeettings:TipType"];
            }
        }

        public string TipRouteUrls
        {
            get
            {
                return this._configuration["AppSeettings:TipRouteUrls"];
            }
        }

        public string FileBasePath
        {
            get
            {
                return this._configuration["AppSeettings:FileBasePath"];
            }
        }

        public string CRxAppId
        {
            get
            {
                return this._configuration["AppSeettings:CRxAppId"];
            }
        }

        public string CRxAppSecret
        {
            get
            {
                return this._configuration["AppSeettings:CRxAppSecret"];
            }
        }

        public string AWSAccessKey
        {
            get
            {
                return this._configuration["AppSeettings:AWSAccessKey"];
            }
        }

        public string AWSSecretKey
        {
            get
            {
                return this._configuration["AppSeettings:AWSSecretKey"];
            }
        }

        public string AWSBaseurl
        {
            get
            {
                return this._configuration["AppSeettings:AWSBaseurl"];
            }
        }

        public string Fetchmsgcount
        {
            get
            {
                return this._configuration["AppSeettings:Fetchmsgcount"];
            }
        }

        public IConfigurationSection GetConfigurationSection(string key)
        {
            return this._configuration.GetSection(key);
        }
    }
}
