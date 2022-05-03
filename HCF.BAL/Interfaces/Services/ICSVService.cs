using System.Data;
using System.Web;

namespace HCF.BAL
{
    public interface ICSVService
    {
        DataTable AssestsCSVMatch();
        void DownloadCSV(string csvdata, string filetype, string fileName);
        DataTable GetDistinctRecord(DataTable dtRecords);
        //DataTable ReadCSVFile(HttpPostedFileBase postedFile);
        DataSet SaveCSVFilterNewAssets(string xmlRecord, int UserId);
        bool UpdateExistedAssets(string xmlRecord, int UserId);
    }
}