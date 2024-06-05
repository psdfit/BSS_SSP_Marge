using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;


namespace PSDF_AMSReports.Dapper
{
    public interface IDapperConfig
    {
        List<T> Query<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : class;

        Task<IEnumerable<T>> QueryAsync<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : class;


        T QueryFirst<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : class;

        Task<T> QueryFirstAsync<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : class;

        T QueryFirstOrDefault<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : class;

        Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : class;

        T QuerySingle<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : class;

        Task<T> QuerySingleAsync<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : class;

        T QuerySingleOrDefault<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : class;

        Task<T> QuerySingleOrDefaultAsync<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : class;

        T ExecuteScalar<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : IConvertible;

        Task<T> ExecuteScalarAsync<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : IConvertible;

        int ExecuteQuery(string query, object parameter = null, CommandType commandType = CommandType.Text);

        Task<int> ExecuteQueryAsync(string query, object parameter = null, CommandType commandType = CommandType.Text);



    }
}
