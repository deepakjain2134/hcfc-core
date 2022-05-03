using Microsoft.Extensions.Configuration;

namespace HCF.Utility.AppConfig
{
    public interface IAppSetting
    {
        string ToErrorEmail { get; }

        string HCFConnection { get; }

        string HcfHospitalConnection { get; }

        string EmailID { get; }

        string AccountKey { get; }

        string BucketName { get; }

        string GetConnectionString(string connectionName);

        IConfigurationSection GetConfigurationSection(string Key);

        string SampleFilePath { get; }

        string ShowCRxUsers { get; }

        string CommonDB { get; }

        string WebUrlPath { get; }

        string ImageBasePath { get; }

        string CommonFileBasePath { get; }

        string BuildVersion { get; }

        string GeoLocationApi { get; }

        string CommonApiUrl { get; }

        string ApiPath { get; }

        string S3FileBasePath { get; }

        string UserDefaultImage { get; }

        string SenderMailFrom { get; }

        string DueWithInDays { get; }

        string WebBaseUrl { get; }

        string PageSize { get; }

        string BrowseFilePath { get; }

        string LogCreate { get; }

        string ErrorlogFilePath { get; }

        string BarCodePage { get; }

        string BarCodeImage { get; }

        // bool EnableOptimizations { get; }

        string CommonHospitalMessage { get; }

        string TipType { get; }

        string TipRouteUrls { get; }

        string FileBasePath { get; }

        string CRxAppId { get; }

        string CRxAppSecret { get; }

        string AWSAccessKey { get; }

        string AWSSecretKey { get; }

        string AWSBaseurl { get; }

        string Fetchmsgcount { get; }
    }
}
