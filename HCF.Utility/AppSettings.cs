//using Microsoft.Extensions.Configuration;

//namespace HCF.Utility
//{

//    //public interface IConfigManager
//    //{
//    //    string NorthwindConnection { get; }

//    //    string EmailID { get; }

//    //    string AccountKey { get; }

//    //    string BucketName { get; }

//    //    string GetConnectionString(string connectionName);

//    //    IConfigurationSection GetConfigurationSection(string Key);
//    //}

//    //public class ConfigManager : IConfigManager
//    //{
//    //    private readonly IConfiguration _configuration;

//    //    public ConfigManager(IConfiguration configuration)
//    //    {
//    //        _configuration = configuration;
//    //    }

//    //    public string NorthwindConnection
//    //    {
//    //        get
//    //        {
//    //            return this._configuration["ConnectionStrings:NorthwindDatabase"];
//    //        }
//    //    }

//    //    public string GetConnectionString(string connectionName)
//    //    {
//    //        return this._configuration.GetConnectionString(connectionName);
//    //    }

//    //    public string EmailID
//    //    {
//    //        get
//    //        {
//    //            return this._configuration["AppSeettings:EmailID"];
//    //        }
//    //    }

//    //    public string BucketName
//    //    {
//    //        get
//    //        {
//    //            return this._configuration["AppSeettings:EmailID"];
//    //        }
//    //    }

//    //    public string AccountKey
//    //    {
//    //        get
//    //        {
//    //            return this._configuration["AppSeettings:AccountKey"];
//    //        }
//    //    }

//    //    public IConfigurationSection GetConfigurationSection(string key)
//    //    {
//    //        return this._configuration.GetSection(key);
//    //    }
//    //}


//    public static class AppSettings
//    {
//        //public static string SampleFilePath => System.Configuration.ConfigurationManager.AppSettings["SampleFilePath"];

//        //public static string ShowCRxUsers => System.Configuration.ConfigurationManager.AppSettings["ShowCRxUsersHospitals"];

//        //public static string CommonDB => System.Configuration.ConfigurationManager.AppSettings["CommonDB"];

//        //public static string WebUrlPath => System.Configuration.ConfigurationManager.AppSettings["WebUrlPath"];

//        //public static string ImageBasePath => System.Configuration.ConfigurationManager.AppSettings["imageBasePath"];

//        //public static string CommonFileBasePath => System.Configuration.ConfigurationManager.AppSettings["CommonFileBasePath"];

//        //public static string BuildVersion => System.Configuration.ConfigurationManager.AppSettings["BuildVersion"];

//        //public static string GeoLocationApi => System.Configuration.ConfigurationManager.AppSettings["geolocationapi"];

//        //public static string CommonApiUrl => System.Configuration.ConfigurationManager.AppSettings["CommonApiUrl"];

//        //public static string ApiPath => System.Configuration.ConfigurationManager.AppSettings["APIPath"];

//        //public static string BucketName => System.Configuration.ConfigurationManager.AppSettings["BucketName"];

//        //public static string S3FileBasePath => System.Configuration.ConfigurationManager.AppSettings["S3FileBasePath"];

//        //public static string UserDefaultImage => System.Configuration.ConfigurationManager.AppSettings["UserDefaultImage"];

//        //public static string SenderMailFrom => System.Configuration.ConfigurationManager.AppSettings["SenderMailFrom"];

//        //public static string DueWithInDays => System.Configuration.ConfigurationManager.AppSettings["DueWithInDays"];

//        //public static string WebBaseUrl => System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"];

//        //public static string PageSize => System.Configuration.ConfigurationManager.AppSettings["PageSize"];

//        //public static string BrowseFilePath => System.Configuration.ConfigurationManager.AppSettings["BrowseFilePath"];

//        //public static string LogCreate => System.Configuration.ConfigurationManager.AppSettings["logCreate"];

//        //public static string ErrorlogFilePath => System.Configuration.ConfigurationManager.AppSettings["ErrorlogFilePath"];

//        //public static string BarCodePage => System.Configuration.ConfigurationManager.AppSettings["BarCodePage"];

//        //public static string BarCodeImage => System.Configuration.ConfigurationManager.AppSettings["BarCodeImage"];

//        //public static bool EnableOptimizations => Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableOptimizations"]);

//        //public static string CommonHospitalMessage => System.Configuration.ConfigurationManager.AppSettings["CommonHospitalMessage"];

//        //public static string TipType => System.Configuration.ConfigurationManager.AppSettings["TipType"];

//        //public static string TipRouteUrls => System.Configuration.ConfigurationManager.AppSettings["TipRouteUrls"];

//        //public static string FileBasePath => WebConfigurationManager.AppSettings["FileBasePath"];

//        //public static string CRxAppId => System.Configuration.ConfigurationManager.AppSettings["CRxAppId"];

//        //public static string CRxAppSecret => System.Configuration.ConfigurationManager.AppSettings["CRxAppSecret"];

//        //public static string AWSAccessKey => System.Configuration.ConfigurationManager.AppSettings["AWSAccessKey"];

//        //public static string AWSSecretKey => System.Configuration.ConfigurationManager.AppSettings["AWSSecretKey"];

//        //public static string AWSBaseurl => System.Configuration.ConfigurationManager.AppSettings["AWSBaseurl"];

//        //public static string Fetchmsgcount => System.Configuration.ConfigurationManager.AppSettings["fetchmsgcount"];
//    }
//}
