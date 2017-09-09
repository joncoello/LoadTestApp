using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LoadTestApp.Repositories
{
    public class SqlHelper
    {
        private readonly string _connectionName;

        public SqlHelper(string connectionName)
        {
            this._connectionName = connectionName;
        }

        public DataTable RunScriptReturnDt(string script, params SqlParameter[] parameters)
        {
            // connection
            var connectionString = ConfigurationManager.ConnectionStrings[_connectionName].ConnectionString;
            var connection = new SqlConnection(connectionString);
            connection.Open();

            // command
            var command = new SqlCommand(script, connection);

            command.Parameters.AddRange(parameters);

            // adapter
            var adapter = new SqlDataAdapter(command);

            var data = new DataTable();
            adapter.Fill(data);

            connection.Close();

            return data;

        }

        public void RunScript(string script, params SqlParameter[] parameters)
        {
            // connection
            var connectionString = ConfigurationManager.ConnectionStrings[_connectionName].ConnectionString;
            var connection = new SqlConnection(connectionString);
            connection.Open();

            // command
            var command = new SqlCommand(script, connection);

            command.Parameters.AddRange(parameters);

            command.ExecuteNonQuery();

            connection.Close();
            
        }

    }
}