using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public interface ISqlHelper
    {
        List<T> ConvertDataTable<T>(DataTable dt);
        bool ExecuteNonQuery(SqlCommand msCommand);
        int ExecuteNonQuery(SqlCommand msCommand, string outParameter);
        DataSet ExecuteNonQueryReturnDS(SqlCommand msCommand);
        DataSet GetDataSet(SqlCommand command);
        DataTable GetDataTable(SqlCommand command);
        object GetScalarValue(SqlCommand command);
        Task<DataTable> GetDataTableAsync(SqlCommand msCommand);
        DataTable GetCommonDataTable(SqlCommand msCommand);
       
        bool CommonExecuteNonQuery(SqlCommand msCommand);
        int CommonExecuteNonQuery(SqlCommand msCommand, string outParameter);
        DataSet GetCommonDataSet(SqlCommand command);
        object GetCommonScalarValue(SqlCommand command);


        Task<DataSet> GetDataSetAsync(SqlCommand command);
        Task<DataTable> GetCommonDataTableAsync(SqlCommand msCommand);
        Task<DataSet> GetCommonDataSetAsync(SqlCommand command);

    }
}