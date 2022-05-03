using System.Data;
using System.IO;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using HCF.BDO.Enums;
using HCF.PDI;

namespace HCF.Utility
{

    public static class CommonUtility
    {

        //public static string FilePath(string path)
        //{
        //    if (!string.IsNullOrEmpty(path))
        //    {
        //        var bucketName = Convert.ToString(HttpContext.Current.Session[SessionKey.ClientNo]);//Convert.ToString(UserSession.CurrentOrg.ClientNo);
        //        var absPath = AppSettings.ImageBasePath + bucketName + "/" + path.Replace("~/", "");
        //        return absPath.ToLower();
        //    }
        //    else
        //        return "";
        //}



        #region FileName / Extension

        public static string GetFileExtension(string fileName)
        {
            return !string.IsNullOrEmpty(fileName) ? fileName.Split('.').Last() : string.Empty;
        }

        public static string CreateFileName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {

                var ext = GetFileExtension(fileName);
                return $"{Guid.NewGuid().ToString()}.{ext}";
            }
            else
                return string.Empty;
        }
        public static string CreateZipFileName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {

                var ext = GetFileExtension(fileName);
                return $"{fileName.Replace("." + ext, "")}.{ext}";
            }
            else
                return string.Empty;
        }
        #endregion

        public static string ToStringWithSpace(this string input)
        {
            return input.Replace("_", " ");
        }
        public static string GetDaySuffix(int? day)
        {
            if (!day.HasValue)
                return "";

            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        private static DateTime Next(this DateTime date, DayOfWeek dayOfWeek)
        {
            return date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);
        }

        private static DateTime GetNthWeekofMonth(this DateTime date, int nthWeek, DayOfWeek dayOfWeek)
        {
            return date.Next(dayOfWeek).AddDays((nthWeek - 1) * 7);
        }

        public static DateTime GetNextInspScheduleDate(DateTime startDate, int startDay = 1, int currentFrequencyDays = 1, int dayNumber = 0, int weekNumber = 0, int monthNumber = 0, bool isCustomFerquecy = false, bool isInspected = false)
        {
            var NextDate = DateTime.UtcNow;
            if (isInspected)
            {
                startDate = startDate.AddDays(currentFrequencyDays);
                isInspected = false;
            }
            if (isCustomFerquecy)
            {
                int monthNo = monthNumber;
                if (currentFrequencyDays < 365 && currentFrequencyDays > 30)
                {
                    monthNo = GetInspMonthNumber(startDate, currentFrequencyDays, monthNumber);
                }
                else
                {
                    startDate.AddDays(currentFrequencyDays);
                }
                if (monthNumber != 0)
                {
                    var dateC = new DateTime(startDate.Year, monthNo, 01);
                    if (weekNumber > 4)
                    {
                        int daysInMonth = DateTime.DaysInMonth(dateC.Year, dateC.Month);
                        int weeksInMonth = (int)Math.Ceiling((dateC.Day + daysInMonth) / 7.0);
                        NextDate = new DateTime(startDate.Year, startDate.Month, 01).GetNthWeekofMonth(weeksInMonth, (DayOfWeek)dayNumber - 1);
                    }
                    else
                    {
                        NextDate = new DateTime(startDate.Year, monthNo, 01).GetNthWeekofMonth(weekNumber, (DayOfWeek)dayNumber - 1);
                    }
                }
                else
                {
                    NextDate = new DateTime(startDate.Year, startDate.Month, 01).GetNthWeekofMonth(weekNumber, (DayOfWeek)dayNumber - 1);
                }
            }
            else
            {
                var startDateDay = startDay;
                if (startDate.Date >= DateTime.UtcNow.Date && !isCustomFerquecy)
                {
                    return startDate;
                }

                NextDate = startDate.AddDays(currentFrequencyDays <= 30 ? 1 : currentFrequencyDays);
                NextDate = new DateTime(NextDate.Year, NextDate.Month, startDateDay);
            }
            while (DateTime.UtcNow.Date > NextDate.Date)
            {
                startDate = startDate.AddDays(currentFrequencyDays <= 30 ? 1 : currentFrequencyDays);
                NextDate = GetNextInspScheduleDate(startDate, startDay, currentFrequencyDays, dayNumber, weekNumber, monthNumber, isCustomFerquecy);
            }
            return NextDate;
        }

        public static DateTime GetNextInspScheduleDateNew(DateTime startDate, (string Type, int Value) frequecyTypVal, int startDay = 1, int dayNumber = 0, int weekNumber = 0, int monthNumber = 0, bool isCustomFerquecy = false, bool isInspected = false)

        {
            dynamic NextDate = null;
            if (!isCustomFerquecy)
            {
                if (startDate.Date >= DateTime.UtcNow.Date)
                {
                    return startDate;
                }
                NextDate = AddFrequencyInterval(startDate, frequecyTypVal);
                while (DateTime.UtcNow.Date > NextDate.Date)
                {
                    NextDate = AddFrequencyInterval(NextDate, frequecyTypVal);
                }
                var currentFrequencyDays = CalcDaysInFrequecy(frequecyTypVal);
                if (currentFrequencyDays >= 30)
                {
                    NextDate = new DateTime(NextDate.Year, NextDate.Month, startDay);
                }


            }
            else
            {
                int monthNo = monthNumber;
                var currentFrequencyDays = CalcDaysInFrequecy(frequecyTypVal);
                if (currentFrequencyDays < 365 && currentFrequencyDays > 30)
                {
                    monthNo = GetInspMonthNumber(startDate, currentFrequencyDays, monthNumber);
                }
                else
                {
                    startDate.AddDays(currentFrequencyDays);
                }
                if (monthNumber != 0)
                {
                    if (weekNumber > 4) // case of last dayofweek of month
                    {
                        int weeksInMonth = NumberOfParticularDaysInMonth(startDate.Year, startDate.Month, (DayOfWeek)dayNumber - 1);
                        NextDate = new DateTime(startDate.Year, startDate.Month, 01).GetNthWeekofMonth(weeksInMonth, (DayOfWeek)dayNumber - 1);
                    }
                    else
                    {
                        NextDate = new DateTime(startDate.Year, monthNo, 01).GetNthWeekofMonth(weekNumber, (DayOfWeek)dayNumber - 1);
                    }
                }
                else
                {
                    var dateC = new DateTime(startDate.Year, startDate.Month, 01);
                    if (weekNumber > 4)
                    {
                        int weeksInMonth = NumberOfParticularDaysInMonth(startDate.Year, startDate.Month, (DayOfWeek)dayNumber - 1);
                        NextDate = new DateTime(startDate.Year, startDate.Month, 01).GetNthWeekofMonth(weeksInMonth, (DayOfWeek)dayNumber - 1);
                    }
                    else
                    {
                        NextDate = new DateTime(startDate.Year, startDate.Month, 01).GetNthWeekofMonth(weekNumber, (DayOfWeek)dayNumber - 1);
                    }
                    if (currentFrequencyDays == 7)
                    {
                        NextDate = new DateTime(startDate.Year, startDate.Month, startDate.Day).Next((DayOfWeek)dayNumber - 1);
                    }
                }

                while (DateTime.UtcNow.Date > NextDate.Date)
                {
                    startDate = startDate.AddDays(currentFrequencyDays <= 30 ? 1 : currentFrequencyDays);
                    NextDate = GetNextInspScheduleDateNew(startDate, frequecyTypVal, startDay, dayNumber, weekNumber, monthNumber, isCustomFerquecy, isInspected);
                }

            }
            return NextDate;
        }

        private static DateTime AddFrequencyInterval(DateTime startDate, (string Type, int Value) frequecyTypVal)
        {
            switch (frequecyTypVal.Type)
            {
                case "D":
                    startDate = startDate.AddDays(frequecyTypVal.Value);
                    break;
                case "Y":
                    startDate = startDate.AddYears(frequecyTypVal.Value);
                    break;
                case "M":
                    startDate = startDate.AddMonths(frequecyTypVal.Value);
                    break;
                default:
                    startDate = startDate.AddDays(1);
                    break;
            }
            return startDate;
        }
        private static int CalcDaysInFrequecy((string Type, int Value) frequecyTypVal)
        {
            int days = 0;
            switch (frequecyTypVal.Type)
            {
                case "D":
                    days = frequecyTypVal.Value;
                    break;
                case "Y":
                    days = frequecyTypVal.Value * 365;
                    break;
                case "M":
                    days = frequecyTypVal.Value * 30;
                    break;
                default:
                    days = 0;
                    break;
            }
            return days;
        }

        public static int NumberOfParticularDaysInMonth(int year, int month, DayOfWeek dayOfWeek)
        {
            DateTime startDate = new DateTime(year, month, 1);
            int totalDays = startDate.AddMonths(1).Subtract(startDate).Days;

            int answer = Enumerable.Range(1, totalDays)
                .Select(item => new DateTime(year, month, item))
                .Where(date => date.DayOfWeek == dayOfWeek)
                .Count();

            return answer;
        }

        private static int GetInspMonthNumber(DateTime startDate, int currentFrequencyDays, int monthNumber)
        {
            if (currentFrequencyDays < 365)
            {
                var slot = 365 / currentFrequencyDays;
                var monthPerSlot = 12 / slot;
                var startDateMonth = startDate.Month;
                var currentSlot = (startDateMonth % monthPerSlot == 0) ? (startDateMonth / monthPerSlot) - 1 : startDateMonth / monthPerSlot;
                monthNumber = currentSlot * monthPerSlot + monthNumber;
            }
            else
            {

            }
            return monthNumber;
        }

        public static void DeleteDirectory(string path)
        {
            var di = new DirectoryInfo(path);

            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }
            di.Delete(true);
        }

        public static string GetlucenePath()
        {
            return "";
            //return Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "lucene_index" + "/" + HcfSession.ClientNo);
        }


        public static string GetMasterLucenePath()
        {
            return "";
            //return Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "lucene_index");
        }

        //public static byte[] GenerateBarCode(string data)
        //{
        //    BarcodeLib.Barcode b = new BarcodeLib.Barcode
        //    {
        //        IncludeLabel = true,
        //        LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER
        //    };
        //    //b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
        //    BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128; //BarcodeLib.TYPE.Codabar;
        //    var barcodeImage = b.Encode(type, data);
        //    //return barcodeImage;
        //    return ImageToByteArray(barcodeImage);
        //}

        //private static byte[] ImageToByteArray(Image imageIn)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        //        return ms.ToArray();
        //    }
        //}

        public static DataTable ConvertCsVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        //public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        //{
        //    OleDbConnection oledbConn = new OleDbConnection(connString);
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        oledbConn.Open();
        //        using (var cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
        //        {
        //            OleDbDataAdapter oleda = new OleDbDataAdapter { SelectCommand = cmd };
        //            DataSet ds = new DataSet();
        //            oleda.Fill(ds);
        //            dt = ds.Tables[0];
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {

        //        oledbConn.Close();
        //    }
        //    return dt;
        //}



        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static int SetActivityType(string mode)
        {
            var activityType = 0;

            if (mode ==Mode.ASSET.ToString())
                activityType = Convert.ToInt32(Mode.ASSET);
            else if (mode == Mode.EP.ToString())
                activityType = Convert.ToInt32(Mode.EP);
            else if (mode == Mode.DOC.ToString())
                activityType = Convert.ToInt32(Mode.DOC);
            return activityType;
        }

        //public static string CallGetMethod(string uri, string contentType, string Accept, List<Tuple<string, string>> headers)
        //{
        //    string Url = uri;
        //    string result = string.Empty;
        //    var request = (HttpWebRequest)WebRequest.Create(Url);
        //    foreach (var (item1, item2) in headers)
        //    {
        //        request.Headers.Add(item1, item2);
        //    }
        //    request.ContentType = contentType;
        //    request.Accept = Accept;
        //    request.AutomaticDecompression = DecompressionMethods.GZip;
        //    request.UseDefaultCredentials = false;
        //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //    using (Stream stream = response.GetResponseStream())
        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        result = reader.ReadToEnd();
        //    }
        //    return result;
        //}


        public static string CallGetMethod1(string uri, string contentType, string Accept, List<Tuple<string, string>> headers)
        {
            string Url = uri;
            string result = string.Empty;
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = Accept;
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:53.0) Gecko/20100101 Firefox/53.0");
                    foreach (var (item1, item2) in headers)
                    {
                        client.Headers.Add(item1, item2);
                    }

                    client.UseDefaultCredentials = false;
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    result = client.DownloadString(Url);
                }
            }
            catch (Exception ex)
            {
                //Utility.ErrorLog.LogMsg(ex.InnerException.ToString());
            }

            return result;
        }

        public static string CallPostMethod(string postData, string uri, string contentType, string Accept, List<Tuple<string, string>> headers)
        {
            string response = "";
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = Accept;
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:53.0) Gecko/20100101 Firefox/53.0");
                    client.UseDefaultCredentials = true;
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    foreach (var (item1, item2) in headers)
                    {
                        client.Headers.Add(item1, item2);
                    }
                    var reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("input_data", postData);
                    byte[] responsebytes = client.UploadValues(uri, "POST", reqparm);
                    response = Encoding.UTF8.GetString(responsebytes);
                }
            }
            catch (Exception e)
            {
                //ErrorLog.LogMsg(e.Message);
            }

            return response;
        }
        public static string CallPutMethod(string postData, string uri, string contentType, string Accept, List<Tuple<string, string>> headers)
        {
            string response = "";
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = Accept;
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:53.0) Gecko/20100101 Firefox/53.0");
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    foreach (var item in headers)
                    {
                        client.Headers.Add(item.Item1, item.Item2);
                    }
                    var reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("input_data", postData);
                    byte[] responsebytes = client.UploadValues(uri, "PUT", reqparm);
                    response = Encoding.UTF8.GetString(responsebytes);
                }
            }
            catch (Exception e)
            {
               // ErrorLog.LogMsg(e.Message);
            }
            return response;
        }

        public static string HTTPPost(string postData, string uri, string contentType, string Accept, string Method, List<Tuple<string, string>> headers)
        {
            var result = "";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Item1, item.Item2);
                }
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:53.0) Gecko/20100101 Firefox/53.0";
                request.Method = Method;
                //request.ContentType = contentType;
                var bytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = bytes.Length;

                var requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);

                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);

                result = reader.ReadToEnd();
                stream.Dispose();
                reader.Dispose();
            }
            catch (Exception ex)
            {
               // ErrorLog.LogMsg(ex.Message);
            }


            return result;
        }

        public static string TruncateLongString(string str, int maxLength)
        {
            return string.IsNullOrEmpty(str) ? str : str.Substring(0, Math.Min(str.Length, maxLength));
        }
        public static string ConvertDataTableToXML(DataTable dtRecords, string parentNodeName, string childNodeName, bool inAttributeFormat)
        {
            XElement xElement = new XElement(parentNodeName);
            if (dtRecords != null && dtRecords.Rows.Count > 0)
            {
                if (inAttributeFormat)
                {
                    foreach (DataRow dr in dtRecords.Rows)
                    {
                        XElement childElement = new XElement(childNodeName);
                        foreach (DataColumn col in dtRecords.Columns)
                        {
                            string val = Convert.ToString(dr[col.ColumnName]).Replace("\"", "").Replace("\r\n", "").Replace("\n", ""),
                                   column = col.ColumnName.Replace("\"", "").Replace("\r\n", "").Replace("\n", "");
                            XAttribute attr = new XAttribute(column, val);
                            childElement.Add(attr);
                        }
                        xElement.Add(childElement);
                    }
                }
            }
            return xElement.ToString();
        }

        //public static string CastToNA(this string text, string value = "NA")
        //{
        //    return !string.IsNullOrEmpty(text) ? text : value;
        //}

        #region MaintConnection Get/Post

        public static string MaintConnectionGet(string apiUrl, int skipNo, string mcCnKey, string password, string filterexp = null)
        {
            var url = string.Empty;
            const string contentType = "application/json";
            url = skipNo >= 0 ? apiUrl + "?$top=500&$skip=" + skipNo : apiUrl;

            if (!string.IsNullOrEmpty(filterexp))
                url = $"{url}{filterexp}&TypeDetails.value='PM'";
            var result = string.Empty;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var request = (HttpWebRequest)WebRequest.Create(url);
            var authInfo = mcCnKey + ":" + password;
            request.PreAuthenticate = true;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Timeout = Timeout.Infinite;
            request.KeepAlive = true;
            request.Headers.Add("Authorization", "Basic " + authInfo);
            request.Accept = request.ContentType = contentType;
            request.AutomaticDecompression = DecompressionMethods.GZip;
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (WebException exe)
            {
                if (exe.Status == WebExceptionStatus.ProtocolError)
                {
                    if (exe.Response is HttpWebResponse response)
                    {
                       // ErrorLog.LogMsg(exe.Message);
                    }
                }
            }
            return result;
        }

        public static string MaintConnectionPost(string apiUrl, string mcCnKey, string password, string postData, string MethodType = "POST")
        {
            const string contentType = "application/json";
            var url = apiUrl;
            var result = string.Empty;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            var authInfo = mcCnKey + ":" + password;
            webRequest.PreAuthenticate = true;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            webRequest.Timeout = Timeout.Infinite;
            webRequest.KeepAlive = true;
            webRequest.Method = MethodType;
            webRequest.Headers.Add("Authorization", "Basic " + authInfo);
            webRequest.Accept = webRequest.ContentType = contentType;
            webRequest.AutomaticDecompression = DecompressionMethods.GZip;
            try
            {
                using (var requestWriter2 = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter2.Write(postData);
                }
                using (var responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    postData = responseReader.ReadToEnd();
                }
            }
            catch (WebException exe)
            {
                if (exe.Status == WebExceptionStatus.ProtocolError)
                {
                    if (exe.Response is HttpWebResponse response)
                    {

                       // ErrorLog.LogMsg("main connection post api" + url + " Post Data" + " " + postData);

                    }
                }
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
            return postData;
        }

        #endregion


        #region Extension Method

        public static string ConvertToFormatDate(DateTime dt)
        {
            return dt.ToString("MMM d, yyyy");
        }

        //public static string FilePath(string path)
        //{
        //    if (!string.IsNullOrEmpty(path))
        //    {
        //        var bucketName = Convert.ToString(HttpContext.Current.Session[SessionKey.ClientNo]);//Convert.ToString(UserSession.CurrentOrg.ClientNo);
        //        var absPath = AppSettings.ImageBasePath + bucketName + "/" + path.Replace("~/", "");
        //        return absPath.ToLower();
        //    }
        //    else
        //        return "";
        //}

        #endregion

        public static DateTime GetScheduleFixedDate(int frequencyId, DateTime startDate, int sequence)
        {
            var recurDates = new DateTimeCollection();
            var rRecur = new Recurrence
            {
                StartDateTime = startDate,
                RecurUntil = RecurUntil(frequencyId, startDate)
            };
            switch (frequencyId)
            {
                case (int)Frequency.Daily:
                    rRecur.RecurDaily(1);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddDays(5));
                    break;
                case (int)Frequency.ThreeDays:
                    rRecur.RecurDaily(3);
                    rRecur.RecurUntil = DateTime.Today.AddDays(15);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddDays(15));
                    break;
                case (int)Frequency.Weekly:
                    rRecur.RecurWeekly(1, DaysOfWeek.Monday);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(1));
                    break;
                case (int)Frequency.Monthly:
                    rRecur.RecurMonthly(startDate.Day, 1);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(3));
                    break;
                case (int)Frequency.TwoMonths:
                    rRecur.RecurMonthly(startDate.Day, 2);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(6));
                    break;
                case (int)Frequency.Quarterly:
                    rRecur.RecurMonthly(startDate.Day, 3);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(6));
                    break;
                case (int)Frequency.ThreeXQuarterly:
                    rRecur.RecurMonthly(startDate.Day, 9);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(20));
                    break;
                case (int)Frequency.SemiAnnually:
                    rRecur.RecurMonthly(startDate.Day, 6);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(12));
                    break;
                case (int)Frequency.Annually:
                    rRecur.RecurYearly(startDate.Month, startDate.Day, 1);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddYears(3));
                    break;
                case (int)Frequency.Threeyears:
                    rRecur.RecurYearly(startDate.Month, startDate.Day, 3);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddYears(5));
                    break;
                case (int)Frequency.Fiveyears:
                    rRecur.RecurYearly(startDate.Month, startDate.Day, 5);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddYears(7));
                    break;
                case (int)Frequency.Sixyears:
                    rRecur.RecurYearly(startDate.Month, startDate.Day, 6);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddYears(8));
                    break;
            }
            if (recurDates.Count(x => x >= DateTime.Today) > 0)
            {
                var nearestDate = recurDates.Where(x => x >= DateTime.Today).ToList()[sequence];
                if (nearestDate != null)
                    return nearestDate;
                else
                    return DateTime.Today;
            }
            else
                return DateTime.Today;
        }

        private static DateTime RecurUntil(int frequencyId, DateTime startDate)
        {
            var recurUntil = DateTime.Today;
            switch (frequencyId)
            {
                case (int)Frequency.Daily:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddDays(5) : DateTime.Today.AddDays(5);
                    break;
                case (int)Frequency.ThreeDays:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddDays(15) : DateTime.Today.AddDays(15);
                    break;
                case (int)Frequency.Weekly:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddMonths(1) : DateTime.Today.AddMonths(1);
                    break;
                case (int)Frequency.Monthly:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddMonths(3) : DateTime.Today.AddMonths(3);
                    break;
                case (int)Frequency.TwoMonths:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddMonths(6) : DateTime.Today.AddMonths(6);
                    break;
                case (int)Frequency.Quarterly:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddMonths(9) : DateTime.Today.AddMonths(9);
                    break;
                case (int)Frequency.ThreeXQuarterly:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddMonths(9) : DateTime.Today.AddMonths(9);
                    break;
                case (int)Frequency.SemiAnnually:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddMonths(12) : DateTime.Today.AddMonths(12);
                    break;
                case (int)Frequency.Annually:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddYears(3) : DateTime.Today.AddYears(3);
                    break;
                case (int)Frequency.Threeyears:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddYears(5) : DateTime.Today.AddYears(5);
                    break;
                case (int)Frequency.Fiveyears:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddYears(7) : DateTime.Today.AddYears(7);
                    break;
                case (int)Frequency.Sixyears:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddYears(8) : DateTime.Today.AddYears(8);
                    break;
            }
            return recurUntil;
        }

    }
}
