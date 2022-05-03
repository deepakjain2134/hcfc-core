//using System.Linq;
//using System.Text;
//using System.Data;
//using System.IO;
//using System.Web;

//namespace HCF.BAL
//{
//    public static class CSVRepository
//    {
//        #region Read CSV File
//        /// <summary>
//        /// Export CSV file data.
//        /// </summary>
//        /// <param name="postedFile"></param>
//        /// <returns></returns>
//        public static DataTable ReadCSVFile(HttpPostedFileBase postedFile)
//        {
//            string Fulltext; DataTable dtCsv = new DataTable();
//            using (StreamReader reader = new StreamReader(postedFile.InputStream))
//            {
//                while (!reader.EndOfStream)
//                {
//                    Fulltext = reader.ReadToEnd().ToString();
//                    string[] rows = Fulltext.Split('\n');
//                    for (int i = 0; i < rows.Count() - 1; i++)
//                    {
//                        string[] rowValues = rows[i].Split(',');
//                        {
//                            if (i == 0)
//                            {
//                                for (int j = 0; j < rowValues.Count(); j++) dtCsv.Columns.Add(rowValues[j]); //add headers  
//                            }
//                            else
//                            {
//                                DataRow dr = dtCsv.NewRow();
//                                for (int k = 0; k < rowValues.Count(); k++) dr[k] = rowValues[k].ToString();
//                                dtCsv.Rows.Add(dr); //add other rows  
//                            }
//                        }
//                    }
//                }
//            }
//            return dtCsv;
//        }
//        #endregion

//        #region Get Assets Data
//        public static DataTable AssestsCSVMatch()
//        {
//            return DAL.AssetsRepository.GetAssestsDataCSVMatch();
//        }
//        #endregion

//        #region Filter Not Match Data
//        //public static DataSet CSVFilterRecord(DataTable dtExistData, DataTable dtSelectCSVData, List<string> lstChkColumnList)
//        //{
//        //    DataSet dsFilterData = new DataSet();
//        //    if (lstChkColumnList.Count() > 0)
//        //    {
//        //        if (dtSelectCSVData != null && dtSelectCSVData.Rows.Count > 0)
//        //        {
//        //            DataTable dtNotMatchData = dtSelectCSVData.Clone();
//        //            dtNotMatchData.TableName = "NotMatchData";
//        //            DataTable dtMatchData = dtSelectCSVData.Clone();
//        //            dtMatchData.TableName = "MatchData";

//        //            //foreach (DataRow dr in dtSelectCSVData.Rows)
//        //            //{
//        //            //    //List<string> lstcondtn = new List<string>();
//        //            //    //foreach (string chkcol in lstChkColumnList)
//        //            //    //{
//        //            //    //    string c = string.Format("{0}='{1}'", chkcol, Convert.ToString(dr[chkcol]));
//        //            //    //    lstcondtn.Add(c);
//        //            //    //}
//        //            //    //string condition = (lstChkColumnList.Count > 1) ? string.Join(" and ", lstcondtn): lstcondtn[0];
//        //            //    //string[] cols = lstChkColumnList.ToArray();
//        //            //    //DataTable dtDistinctExistData = new DataTable();
//        //            //    //dtDistinctExistData = dtExistData.DefaultView.ToTable(true, cols);
//        //            //    //DataRow[] existresult = dtDistinctExistData.Select(condition);
//        //            //    //if (existresult.Count() == 0) dtNotMatchData.ImportRow(dr);
//        //            //    //else dtMatchData.ImportRow(dr);
//        //            //    foreach (string chkcol in lstChkColumnList)
//        //            //    {
//        //            //        if(string.IsNullOrEmpty(Convert.ToString(dr[chkcol]).Trim()))
//        //            //        {
                                
//        //            //        }
//        //            //    }
//        //            //}

//        //            dsFilterData.Tables.Add(GetDistinctRecord(dtNotMatchData));
//        //            dsFilterData.Tables.Add(GetDistinctRecord(dtMatchData));
//        //            return dsFilterData;
//        //        }
//        //        else return null;
//        //    }
//        //    else return null;
//        //}
//        #endregion

//        #region Distinct Row DataTable
//        public static DataTable GetDistinctRecord(DataTable dtRecords)
//        {
//            DataTable dtnew = new DataTable();
//            if (dtRecords != null && dtRecords.Rows.Count > 0)
//            {
//                string[] cols = (from dc in dtRecords.Columns.Cast<DataColumn>() select dc.ColumnName).ToArray();
//                dtnew = dtRecords.DefaultView.ToTable(true, cols);
//            }
//            else dtnew = dtRecords.Clone();
//            return dtnew;
//        }
//        #endregion

//        #region Save New Record
//        public static DataSet SaveCSVFilterNewAssets(string xmlRecord,int UserId)
//        {
//            DataSet ds = new DataSet();
//            if(!string.IsNullOrEmpty(xmlRecord))
//            {
//                ds = DAL.AssetsRepository.SaveCSVFilterNewAssets(xmlRecord, UserId);
//            }
//            return ds;
//        }
//        #endregion

//        #region Update Exist Record
//        public static bool UpdateExistedAssets(string xmlRecord, int UserId)
//        {
//            bool result = false;
//            if (!string.IsNullOrEmpty(xmlRecord))
//            {
//                result = DAL.AssetsRepository.UpdateExistAssetsFromCSV(xmlRecord, UserId);
//            }
//            return result;
//        }
//        #endregion

//        #region Download record in CSV.
//        /// <summary>
//        /// Download Record in CSV.
//        /// </summary>
//        //public static void DownloadRecordInCSV(DataTable dt,string fileName)
//        //{
//        //    string header = string.Empty, rows = string.Empty;
//        //    string[] cols = (from dc in dt.Columns.Cast<DataColumn>() select dc.ColumnName).ToArray();
//        //    header = string.Join(",", cols);
//        //    if (dt != null && dt.Rows.Count > 0)
//        //    {
//        //        foreach (DataRow dr in dt.Rows)
//        //        {
//        //            string[] vals = dr.ItemArray.Select(x => x.ToString()).ToArray();
//        //            rows += string.Join(",", vals);
//        //        }
//        //    }
//        //    string csvdata = header + rows;
//        //    DownloadCSV(csvdata, string.Empty, fileName);
//        //}
//        #endregion

//        #region Download CSV file
//        /// <summary>
//        /// Download CSV file.
//        /// </summary>
//        /// <param name="csvdata"></param>
//        /// <param name="filetype"></param>
//        /// <param name="fileName"></param>
//        public static void DownloadCSV(string csvdata, string filetype, string fileName)
//        {
//            fileName = filetype + fileName + ".csv";
//            var response = System.Web.HttpContext.Current.Response;
//            response.BufferOutput = true;
//            response.Clear();
//            response.ClearHeaders();
//            response.ContentEncoding = Encoding.Unicode;
//            response.AddHeader("content-disposition", "attachment;filename=" + fileName);
//            response.ContentType = "text/plain";
//            response.Write(csvdata);
//            response.End();
//        }
//        #endregion
//    }
//}
