using System;
using System.Net;

namespace TMS.HolyName
{

    public static class Common
    {
        public static bool IsConnectionUp()
        {
            try
            {
                HolyNameTmsService.HolyNameTypeClient client = new HolyNameTmsService.HolyNameTypeClient();
                HolyNameTmsService.Type1[] lists = client.GetCodesType("WO", 11, true);
            }
            catch (Exception ex)
            {
                HCF.Utility.ErrorLog.LogError(ex);
                return false;
            }
            return true;
        }

        //public static bool IsBurkeConnectionUp()
        //{
        //    try
        //    {
        //        HolyNameTmsService.BurkeWorkOrderClient client = new HolyNameTmsService.BurkeWorkOrderClient();
        //        client.State
        //    }
        //    catch (Exception ex)
        //    {
        //        HCF.Utility.ErrorLog.LogError(ex);
        //        return false;
        //    }
        //    return true;
        //}

        public static bool IsBurkeServiceRunning
        {
            get
            {
                bool isRunning = true;
                try
                {
                    var endpoint = new HolyNameTmsService.BurkeWorkOrderClient();
                    var serviceUri = endpoint.Endpoint.Address.Uri;
                    var request = (HttpWebRequest)WebRequest.Create(serviceUri);
                    request.Timeout = 1000000;
                    var response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                        isRunning = true;
                }
                #region
                catch (Exception ex)
                {
                    HCF.Utility.ErrorLog.LogError(ex);
                    isRunning = false;
                }
                #endregion
                return isRunning;
            }
        }

    }
}
