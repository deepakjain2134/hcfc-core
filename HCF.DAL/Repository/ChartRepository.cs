
using HCF.Utility;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HCF.DAL
{
    public  class ChartRepository :IChartRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public ChartRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public  DataTable GetAssetChart(int days)
        {
            DataSet ds = new DataSet();
            try
            {
                const string sql = StoredProcedures.Get_AssetChart;
                using (var command = new SqlCommand(sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@days", days);
                    ds = _sqlHelper.GetDataSet(command);
                }
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
          
            return ds.Tables[0];
        }

        public  DataTable GetAssetInventoryReport()
        {
            DataSet ds = new DataSet();
            const string sql = StoredProcedures.Get_AssetInventoryReport;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;              
                ds = _sqlHelper.GetDataSet(command);
            }
            return ds.Tables[0];
        }

        public  DataTable GetEpStatusChart()
        {
            DataSet ds = new DataSet();
            const string sql = StoredProcedures.Get_EpStatusChart;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;               
                ds = _sqlHelper.GetDataSet(command);
            }
            return ds.Tables[0];
        }

        public  DataSet GetAllOrganizationChart()
        {
            DataSet ds = new DataSet();
            const string sql = Utility.StoredProcedures.Get_AllOrganizationChart;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;               
                ds = _sqlHelper.GetDataSet(command);
            }
            return ds;
        }

        public  DataTable GetWorkOrdersChart(int days,int userId)
        {
            DataSet ds = new DataSet();
            const string sql = StoredProcedures.Get_AssetChart;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@days", days);
                ds = _sqlHelper.GetDataSet(command);
            }
            return ds.Tables[0];
        }
    }
}
