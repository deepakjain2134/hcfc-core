using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;
using DataAdapterAsync = AsyncDataAdapter;

namespace HCF.DAL
{
    public partial class SqlHelper : ISqlHelper
    {
        private readonly ISqlConnection _sqlConnection;     
        private readonly ILogger<SqlHelper> _logger;

        public SqlHelper(ISqlConnection sqlConnection, ILogger<SqlHelper> logger)
        {
            _sqlConnection = sqlConnection;
            _logger = logger;
        }

        public async Task<DataTable> GetCommonDataTableAsync(SqlCommand msCommand)
        {
            var dt = new DataTable();
            try
            {
                if (msCommand != null)
                {
                    using (var connection = new SqlConnection(_sqlConnection.CommonConnectionString()))
                    {
                        connection.Open();
                        msCommand.Connection = connection;
                        var reader = await msCommand.ExecuteReaderAsync();
                        dt.Load(reader);

                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                _logger.LogError(exception + " "  + msCommand?.CommandText);
            }
            return dt;
        }

        public async Task<DataTable> GetDataTableAsync(SqlCommand msCommand)
        {
            var dt = new DataTable();
            try
            {
                if (msCommand != null)
                {
                    using (var connection = new SqlConnection(_sqlConnection.ConnectionString()))
                    {
                        connection.Open();
                        msCommand.Connection = connection;
                        var reader = await msCommand.ExecuteReaderAsync();
                        dt.Load(reader);

                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                _logger.LogError(exception + " " + msCommand?.CommandText);

            }

            return dt;
        }

        public bool ExecuteNonQuery(SqlCommand msCommand)
        {
            try
            {
                if (msCommand != null)
                {
                    using (var msConnection = new SqlConnection(_sqlConnection.ConnectionString()))
                    {
                        msCommand.Connection = msConnection;
                        msConnection.Open();
                        msCommand.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SqlException exception)
            {
                _logger.LogError(exception.Message);
                _logger.LogError(exception + " " + msCommand?.CommandText);
            }
            catch (Exception exception)
            {

                _logger.LogError(exception.Message);
                _logger.LogError(exception + " " + msCommand?.CommandText);
            }

            return false;
        }

        public int ExecuteNonQuery(SqlCommand msCommand, string outParameter)
        {
            try
            {
                if (msCommand != null)
                {
                    using (var msConnection = new SqlConnection(_sqlConnection.ConnectionString()))
                    {
                        msCommand.Connection = msConnection;
                        msConnection.Open();
                        msCommand.ExecuteNonQuery();
                        int outId = Convert.ToInt32(msCommand.Parameters[outParameter].Value);
                        return outId;
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , msCommand?.CommandText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , msCommand?.CommandText);
                return -1;
            }

            return 0;
        }

        public DataSet ExecuteNonQueryReturnDS(SqlCommand msCommand)
        {
            try
            {
                if (msCommand != null)
                {
                    using (SqlConnection msConnection = new SqlConnection(_sqlConnection.ConnectionString()))
                    {
                        msCommand.Connection = msConnection;
                        msConnection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(msCommand);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        return ds;
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , msCommand?.CommandText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , msCommand?.CommandText);
                return null;
            }

            return null;
        }

        public object GetScalarValue(SqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    using (SqlConnection connection = new SqlConnection(_sqlConnection.ConnectionString()))
                    {
                        command.Connection = connection;
                        connection.Open();
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , command?.CommandText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , command?.CommandText);
            }

            return null;
        }

        public DataTable GetDataTable(SqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    using (var connection = new SqlConnection(_sqlConnection.ConnectionString()))
                    {
                        command.Connection = connection;
                        using (var dataAdapter = new SqlDataAdapter(command))
                        {
                            using (var dataTable = new DataTable())
                            {
                                dataTable.Locale = CultureInfo.InvariantCulture;
                                dataAdapter.Fill(dataTable);
                                return dataTable;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , command?.CommandText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , command?.CommandText);
            }

            return null;
        }

        public DataSet GetDataSet(SqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    using (SqlConnection connection = new SqlConnection(_sqlConnection.ConnectionString()))
                    {
                        command.Connection = connection;
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                        {
                            using DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            dataSet.Locale = CultureInfo.CurrentUICulture;
                            return dataSet;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , command?.CommandText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex, command?.CommandText);
            }
            return null;
        }

        public List<T> ConvertDataTable<T>(DataTable dt)
        {

            List<T> data = new List<T>();
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row, dt.TableName);
                    data.Add(item);
                }
            }
            catch (SqlException ex)
            {
                //Utility.ErrorLog.LogError(ex);
            }
            catch (Exception ex)
            {
                //Utility.ErrorLog.LogError(ex);
            }
            return data;
        }

        private T GetItem<T>(DataRow dr, string tableName)
        {
            var temp = typeof(T);
            var obj = Activator.CreateInstance<T>();
            string ColumnName = string.Empty;
            try
            {
                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (var pro in temp.GetProperties())
                    {
                        ColumnName = column.ColumnName;
                        if (pro.Name == column.ColumnName)
                            if (pro.PropertyType == typeof(DateTime?))
                            {
                                pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? null : dr[column.ColumnName], null);
                            }
                            else if (pro.PropertyType == typeof(int?))
                            {
                                pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? null : dr[column.ColumnName], null);
                            }
                            else if (pro.PropertyType == typeof(long?))
                            {
                                pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? null : dr[column.ColumnName], null);
                            }
                            else if (pro.PropertyType == typeof(Guid?))
                            {
                                pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? null : dr[column.ColumnName], null);
                            }
                            else if (pro.PropertyType == typeof(DateTime))
                            {
                                pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? null : dr[column.ColumnName], null);
                            }
                            else if (pro.PropertyType == typeof(decimal?))
                            {
                                pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? null : dr[column.ColumnName], null);
                            }
                            else if (pro.PropertyType == typeof(TimeSpan?))
                            {
                                pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? null : dr[column.ColumnName], null);
                            }
                            else if (pro.PropertyType == typeof(int[]))
                            {
                                pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? null : dr[column.ColumnName].ToString().Split(','), null);
                            }
                            //else if (
                            //    (pro.PropertyType == typeof(int) || pro.PropertyType == typeof(bool))
                            //    && string.IsNullOrEmpty(dr[column.ColumnName].ToString()))
                            //{

                            //}
                            else
                            {
                                pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? "" : dr[column.ColumnName], null);
                            }
                        else
                            continue;
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ColumnName);
            }
            return obj;
        }

        private int GetExecuteScalarValue(SqlCommand msCommand)
        {
            int newId = 0;
            try
            {

                if (msCommand != null)
                {
                    using (var msConnection = new SqlConnection(_sqlConnection.ConnectionString()))
                    {
                        msCommand.Connection = msConnection;
                        msConnection.Open();
                        newId = (int)msCommand.ExecuteScalar();
                        return newId;
                    }
                }
            }
            catch (SqlException ex)
{
                _logger.LogError(ex.Message);
                _logger.LogError(ex, msCommand?.CommandText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex, msCommand?.CommandText);
            }
            return newId;
        }

        private SqlCommand ExecuteScalar(SqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    using (var connection = new SqlConnection(_sqlConnection.ConnectionString()))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteScalar();
                        return command;
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.Message);
             
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
               
            }

            return null;
        }

        private SqlDataReader GetDataReader(SqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    var dataReader = default(SqlDataReader);
                    using (var connection = new SqlConnection(_sqlConnection.ConnectionString()))
                    {
                        command.Connection = connection;
                        command.Connection.Open();
                        dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                        return dataReader;
                    }
                }
            }
            catch (SqlException ex)
            {
                
                _logger.LogError(ex, command?.CommandText);
            }
            catch (Exception ex)
            {
              
                _logger.LogError(ex, command?.CommandText);
            }

            return null;
        }

        private DataView GetDataView(SqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    using (var dataView = new DataView(GetDataTable(command)))
                    {
                        return dataView;
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.Message);
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);              
            }

            return null;
        }

        private SqlDataAdapter GetDataAdapter(SqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    using (SqlConnection connection = new SqlConnection(_sqlConnection.ConnectionString()))
                    {
                        command.Connection = connection;
                        using (var dataAdapter = new SqlDataAdapter(command))
                        {
                            return dataAdapter;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.Message);
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);             
            }

            return null;
        }

        public bool CommonExecuteNonQuery(SqlCommand msCommand)
        {
            try
            {
                if (msCommand != null)
                {
                    using (var msConnection = new SqlConnection(_sqlConnection.CommonConnectionString()))
                    {
                        msCommand.Connection = msConnection;
                        msConnection.Open();
                        msCommand.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , msCommand?.CommandText);
            }

            return false;
        }

        public int CommonExecuteNonQuery(SqlCommand msCommand, string outParameter)
        {
            try
            {
                if (msCommand != null)
                {
                    using (SqlConnection msConnection = new SqlConnection(_sqlConnection.CommonConnectionString()))
                    {
                        msCommand.Connection = msConnection;
                        msConnection.Open();
                        msCommand.ExecuteNonQuery();
                        int outId = Convert.ToInt32(msCommand.Parameters[outParameter].Value);
                        return outId;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , msCommand?.CommandText);
                return -1;
            }
            return 0;
        }

        public DataTable GetCommonDataTable(SqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    using (var connection = new SqlConnection(_sqlConnection.CommonConnectionString()))
                    {
                        command.Connection = connection;
                        using (var dataAdapter = new SqlDataAdapter(command))
                        {
                            using (var dataTable = new DataTable())
                            {
                                dataTable.Locale = CultureInfo.InvariantCulture;
                                dataAdapter.Fill(dataTable);
                                return dataTable;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , command?.CommandText);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , command?.CommandText);
            }

            return null;
        }

        public DataSet GetCommonDataSet(SqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    using (SqlConnection connection = new SqlConnection(_sqlConnection.CommonConnectionString()))
                    {
                        command.Connection = connection;
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                        {
                            using (DataSet dataSet = new DataSet())
                            {
                                dataAdapter.Fill(dataSet);
                                dataSet.Locale = CultureInfo.CurrentUICulture;
                                return dataSet;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , command?.CommandText);
            }

            return null;
        }

        public object GetCommonScalarValue(SqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    using (SqlConnection connection = new SqlConnection(_sqlConnection.CommonConnectionString()))
                    {
                        command.Connection = connection;
                        connection.Open();
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex , command?.CommandText);
            }
            return null;
        }

        public async Task<DataSet> GetDataSetAsync(SqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    using (var connection = new SqlConnection(_sqlConnection.ConnectionString()))
                    {
                        command.Connection = connection;
                        using (var dataAdapter = new DataAdapterAsync.SqlDataAdapter(command))
                        {
                            using (var dataSet = new DataSet())
                            {

                                await dataAdapter.FillAsync(dataSet);
                                return dataSet;
                                //await Task.Run(() => dataAdapter.Fill(dataSet));
                                //dataSet.Locale = CultureInfo.CurrentUICulture;
                                //return dataSet;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
               
                _logger.LogError(ex , command?.CommandText);
            }
            catch (Exception ex)
            {
               
                _logger.LogError(ex , command?.CommandText);
            }

            return null;
        }

        public async Task<DataSet> GetCommonDataSetAsync(SqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    using (var connection = new SqlConnection(_sqlConnection.CommonConnectionString()))
                    {
                        command.Connection = connection;
                        using (var dataAdapter = new DataAdapterAsync.SqlDataAdapter(command))
                        {
                            using (var dataSet = new DataSet())
                            {

                                await dataAdapter.FillAsync(dataSet);
                                return dataSet;
                                //await Task.Run(() => dataAdapter.Fill(dataSet));
                                //dataSet.Locale = CultureInfo.CurrentUICulture;
                                //return dataSet;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, command?.CommandText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , command?.CommandText);
            }

            return null;
        }
    }
}
