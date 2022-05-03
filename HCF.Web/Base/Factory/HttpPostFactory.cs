using HCF.BAL.Interfaces.Services;
using HCF.Module.Core.Extensions;
using HCF.Utility;
using HCF.Utility.AppConfig;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Web;

namespace HCF.Web.Base
{
    public class HttpPostFactory : IHttpPostFactory
    {
        private readonly IAppSetting _appSettings;
        private readonly ILoggingService _logger;
        private readonly IWorkContext _workContext;
        private readonly IHttpClientFactory _clientFactory;

        public HttpPostFactory(IAppSetting appSettings, ILoggingService logger, IWorkContext workContext, IHttpClientFactory clientFactory)
        {
            _appSettings = appSettings;
            _logger = logger;
            _workContext = workContext;
            _clientFactory = clientFactory;
        }

        public string CallPostMethod(string postData, string apiUrl, ref int statusCode, bool isCompleteUrl = false)
        {
            string responseJson = string.Empty;
            try
            {                
                var client = _clientFactory.CreateClient("api");
                var clinetRequest = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                if (!string.IsNullOrEmpty(postData))
                    clinetRequest.Content = new StringContent(postData, Encoding.UTF8, "application/json"); //is where you pass the payload

                AddHttpClinetHeader(clinetRequest);
                var response = client.Send(clinetRequest);

                if (response.IsSuccessStatusCode)
                {
                    responseJson = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
            return responseJson;


            //var loginToken = _workContext.GetLoginToken().Result;
            //var baseUrl = _appSettings.CommonApiUrl;
            //string url = (isCompleteUrl) ? apiUrl : baseUrl + apiUrl;
            //var webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.Method = "POST";
            //webRequest.Timeout = Timeout.Infinite;
            //webRequest.KeepAlive = true;
            //webRequest.ContentType = "application/json";
            //webRequest.Headers.Add(APIkeys.LogOnToken, loginToken);
            //webRequest.Headers.Add(APIkeys.DBName, UserSession.CurrentOrg.DbName);
            //webRequest.Headers.Add(APIkeys.ClientNo, UserSession.CurrentOrg.ClientNo.ToString());
            //try
            //{
            //    using (var requestWriter2 = new StreamWriter(webRequest.GetRequestStream()))
            //    {
            //        requestWriter2.Write(postData);
            //    }
            //    using (var responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream() ?? throw new InvalidOperationException()))
            //    {
            //        postData = responseReader.ReadToEnd();
            //    }
            //}
            //catch (WebException exe)
            //{
            //    if (exe.Status == WebExceptionStatus.ProtocolError && exe.Response is HttpWebResponse response)
            //    {
            //        statusCode = (int)(response.StatusCode);
            //        _logger.Error("Post Data Url " + " " + exe.Message);
            //        _logger.Error("Post Data Url " + " " + url);
            //        _logger.Error("Login Token " + " " + loginToken);
            //        _logger.Error("Post Data" + " " + postData);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return "error:" + ex.Message;
            //}
            //return postData;
        }

        public string CallGetMethod(string url, ref int statusCode)
        {
            string responseJson = string.Empty;
            var client = _clientFactory.CreateClient("api");
            var clinetRequest = new HttpRequestMessage(HttpMethod.Get, url);
            AddHttpClinetHeader(clinetRequest);
            var response = client.Send(clinetRequest);
            if (response.IsSuccessStatusCode)
            {
                responseJson = response.Content.ReadAsStringAsync().Result;
            }
            return responseJson;
        }

        public string CallGetMethodAnonymous(string url, ref int statusCode)
        {
            string responseJson = string.Empty;
            var client = _clientFactory.CreateClient("api");
            var clinetRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = client.Send(clinetRequest);
            if (response.IsSuccessStatusCode)
            {
                responseJson = response.Content.ReadAsStringAsync().Result;
            }
            return responseJson;
        }

        #region private method

        private static void AddHttpClinetHeader(HttpRequestMessage clinetRequest)
        {
            clinetRequest.Headers.Add(APIkeys.LogOnToken, UserSession.LogOnToken);
            clinetRequest.Headers.Add(APIkeys.DBName, UserSession.CurrentOrg.DbName);
            clinetRequest.Headers.Add(APIkeys.ClientNo, UserSession.CurrentOrg.ClientNo.ToString());
        }

        #endregion
    }
}