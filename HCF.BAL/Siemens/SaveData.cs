using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using HCF.BDO;
using HCF.Utility;

namespace HCF.BAL.Siemens
{
    public class SaveData
    {
        static DataTable _csvTable = new DataTable();
        
        public static int SaveRecords(string path,string dbName)
        {
            try
            {
                var dt = ReadFileFromServer(path);
              // var dt = ReadFileFromServerUsingWebClient(path, downloadPath);
                // errors = CheckForMandatoryFields(dt);
                if (dt.Rows.Count > 0)
                {
                    var list = ConvertToList(dt);

                    //foreach (var item in list.Where(x => x.TestResult.ToLower() == "failed"))
                    foreach (var item in list)
                    {
                        try
                        {
                            Save(item, dbName);
                            //HCF.BAL.DeviceTestingRepository.Save(item);
                        }
                        catch (Exception ex)
                        {
                            //HCF.Utility.ErrorLog.LogError(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //HCF.Utility.ErrorLog.LogMsg("Pop Service SaveRecords " + ex.Message + " " + path);
            }

            return 1;
        }

        private static DataTable ReadFileFromServer(string URL)
        {
            try
            {
                System.Net.HttpWebRequest WebRequest;
                ////string URL =
                ////    "https://s3.amazonaws.com/111347/files/tfilespath/637257257356349016/0b042100-7b2f-4908-8ef0-700b969edf60.csv";

                //string URL = "https://s3.amazonaws.com/111347/files/tfilespath/637255570066759588/98072e3c-77cf-4d1a-bbdb-64f8fd66cee5.csv"; //URL of the remote csv
                var CSVUri = new Uri(URL);

                WebRequest = (System.Net.HttpWebRequest)
                    System.Net.WebRequest.Create(CSVUri);
                if ((WebRequest.GetResponse().ContentLength > 0))
                {
                    var strReader = new StreamReader(WebRequest.GetResponse().GetResponseStream());

                    //create datatable with column names
                    CreateCSVTable(strReader.ReadLine());
                    string SingleLine;
                    while ((SingleLine = strReader.ReadLine()) != null)
                    {
                        AddRowCSVTable(SingleLine);
                        //adding rows to datatable
                    }

                    // grdList.DataSource = CSVTable;
                    // grdList.DataBind();

                    if (strReader != null) strReader.Close();
                }
            }
            catch (WebException)
            {
                // lblMsg.Text = "File does not exist.";
            }

            return _csvTable;
        }

        private static IEnumerable<TDeviceTesting> ConvertToList(DataTable dt)
        {
            var lists = new List<TDeviceTesting>();
            try
            {
                foreach (DataRow item in dt.Rows)
                {
                    var list = new TDeviceTesting
                    {
                        Equipment = GetColumnValue(dt, item, "Equipment"),
                        Address = GetColumnValue(dt, item, "Address"),
                        XMLType = GetColumnValue(dt, item, "XML Type"),
                        XMLUsage = GetColumnValue(dt, item, "XML Usage"),
                        Location = GetColumnValue(dt, item, "Location"),
                        NFPAClassification = GetColumnValue(dt, item, "NFPA Classification"),
                        HealthClassification = GetColumnValue(dt, item, "Health Classification"),
                        TestMethod = GetColumnValue(dt, item, "Test Method"),
                        TestResult = GetColumnValue(dt, item, "Test Result"),
                        TestDate = DateTime.Parse(GetColumnValue(dt, item, "Test Date", DateTime.Now.ToString())),
                        DevReading = GetColumnValue(dt, item, "Dev. Reading"),
                        DevReadingDate = DateTime.Parse(GetColumnValue(dt, item, "Dev. Reading Date", DateTime.Now.ToString())),
                        FactorySetting = GetColumnValue(dt, item, "Factory Setting"),
                        DPM = GetColumnValue(dt, item, "DPM"),
                        MakeModel = GetColumnValue(dt, item, "Make /Model"),
                        Note = GetColumnValue(dt, item, "Note"),
                        Comment = GetColumnValue(dt, item, "Comment"),
                        Site = GetColumnValue(dt, item, "Site"),
                        Building = GetColumnValue(dt, item, "Building"),
                        Floor = GetColumnValue(dt, item, "Floor"),
                        Barcode = GetColumnValue(dt, item, "Barcode"),
                        Panel = GetColumnValue(dt, item, "Panel"),
                        Module = GetColumnValue(dt, item, "Module"),
                        Devices = GetColumnValue(dt, item, "Devices"),
                        CreatedDate = DateTime.Parse(GetColumnValue(dt, item, "CreatedDate", DateTime.Now.ToString()))
                    };
                    lists.Add(list);
                }
            }
            catch (Exception)
            { }
            return lists;
        }

        private static DataTable ReadFileFromServerUsingWebClient(string url, string path)
        {
            var dt = new DataTable();
            try
            {
                // Downloading remote file to a given folder
                var myWebClient = new WebClient();

                var csvPath = path + "\\" + Guid.NewGuid() + ".csv";
                if (File.Exists(csvPath)) { File.Delete(csvPath); }
                myWebClient.DownloadFile(url, csvPath);

                using (var sr = new StreamReader(csvPath)) //url
                {
                    var headers = sr.ReadLine()?.Split(',');
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            dt.Columns.Add(header);
                        }

                        while (!sr.EndOfStream)
                        {
                            var rows = sr.ReadLine()?.Split(',');
                            if (rows != null && rows.Length > 1)
                            {
                                var dr = dt.NewRow();
                                for (int i = 0; i < headers.Length; i++)
                                {
                                    dr[i] = rows[i].Trim();
                                }

                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
                if (File.Exists(csvPath)) { File.Delete(csvPath); }
            }
            catch (Exception)
            {

            }
            return dt;
        }

        /// <summary>
        /// gets the value of a column in a particular record of datable
        /// </summary>
        private static string GetColumnValue(DataTable dt, DataRow dr, string columnName, string defaultReturnValue = "")
        {
            var value = "";
            if (dt.Columns.Contains(columnName) && dr[columnName] != null)
            {
                value = string.IsNullOrWhiteSpace(dr[columnName].ToString()) ? "" : dr[columnName].ToString();
                value = value.Trim();

            }
            if (string.IsNullOrWhiteSpace(value))
            {
                value = defaultReturnValue;
            }
            return value;
        }
        

        #region

        private static void CreateCSVTable(string tableColumnList)
        {

            _csvTable = new DataTable("CSVTable");
            DataColumn myDataColumn;
            string[] ColumnName = tableColumnList.Split(',');
            for (int i = 0; i < ColumnName.Length; i++)
            {
                myDataColumn = new DataColumn
                {
                    DataType =
                    Type.GetType("System.String"),
                    ColumnName = ColumnName[i]
                };
                _csvTable.Columns.Add(myDataColumn);
            }
        }

        //to add the rows to datatable


        private static void AddRowCSVTable(string rowValueList)
        {
            string[] RowValue = rowValueList.Split(',');
            DataRow myDataRow;
            myDataRow = _csvTable.NewRow();
            for (int i = 0; i < RowValue.Length; i++)
            {
                myDataRow[i] = RowValue[i];
            }
            _csvTable.Rows.Add(myDataRow);
        }

        #endregion

        #region save to Database

        private static int Save(TDeviceTesting deviceTesting,string dbName)
        {
            int newId;
            try
            {
                const string sql = StoredProcedures.Insert_TDeviceTesting;
                var sqlConnection = string.Format(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"], dbName);
                using (var conn = new SqlConnection(sqlConnection))
                {
                    using (var command = new SqlCommand(sql,conn))
                    {
                        conn.Open();    
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Equipment", deviceTesting.Equipment);
                        command.Parameters.AddWithValue("@Address", deviceTesting.Address);
                        command.Parameters.AddWithValue("@XMLType", deviceTesting.XMLType);
                        command.Parameters.AddWithValue("@XMLUsage", deviceTesting.XMLUsage);
                        command.Parameters.AddWithValue("@Location", deviceTesting.Location);
                        command.Parameters.AddWithValue("@NFPAClassification", deviceTesting.NFPAClassification);
                        command.Parameters.AddWithValue("@HealthClassification", deviceTesting.HealthClassification);
                        command.Parameters.AddWithValue("@TestMethod", deviceTesting.TestMethod);
                        command.Parameters.AddWithValue("@TestResult", deviceTesting.TestResult);
                        command.Parameters.AddWithValue("@TestDate", deviceTesting.TestDate);
                        command.Parameters.AddWithValue("@DevReading", deviceTesting.DevReading);
                        command.Parameters.AddWithValue("@DevReadingDate", deviceTesting.DevReadingDate);
                        command.Parameters.AddWithValue("@FactorySetting", deviceTesting.FactorySetting);
                        command.Parameters.AddWithValue("@DPM", deviceTesting.DPM);
                        command.Parameters.AddWithValue("@MakeModel", deviceTesting.MakeModel);
                        command.Parameters.AddWithValue("@Note", deviceTesting.Note);
                        command.Parameters.AddWithValue("@Comment", deviceTesting.Comment);
                        command.Parameters.AddWithValue("@Site", deviceTesting.Site);
                        command.Parameters.AddWithValue("@Building", deviceTesting.Building);
                        command.Parameters.AddWithValue("@Floor", deviceTesting.Floor);
                        command.Parameters.AddWithValue("@Barcode", deviceTesting.Barcode);
                        command.Parameters.AddWithValue("@Panel", deviceTesting.Panel);
                        command.Parameters.AddWithValue("@Module", deviceTesting.Module);
                        command.Parameters.AddWithValue("@Devices", deviceTesting.Devices);
                        command.Parameters.AddWithValue("@CreatedDate", deviceTesting.CreatedDate.Year == 1 ? DateTime.Now : deviceTesting.CreatedDate);
                        command.Parameters.Add("@Id", SqlDbType.Int);
                        command.Parameters["@Id"].Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        newId= deviceTesting.Id = Convert.ToInt32(command.Parameters["@Id"].Value);
                    }
                }
            }
            catch (Exception)
            {
                newId = 0;
            }
            return newId;
        }
        #endregion
    }
}