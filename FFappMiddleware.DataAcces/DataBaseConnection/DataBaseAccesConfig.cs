using FFAppMiddleware.Model.Settings;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFappMiddleware.DataAcces.DataBaseConnection
{
    public class DataBaseAccesConfig
    {
        private SqlConnection connection;

        private readonly Lazy<Task<SqlConnection>> _lazyAsyncConnection;

        public DataBaseAccesConfig()
        {
            _lazyAsyncConnection = new Lazy<Task<SqlConnection>>(InitializeConnectionAsync);
        }

        public SqlConnection Connection
        {
            get
            {
                SetConnection();
                return this.connection;
            }
        }

        public Task<SqlConnection> ConnectionAsync
        {
            get
            {
                return _lazyAsyncConnection.Value;
            }
        }

        private async Task<SqlConnection> InitializeConnectionAsync()
        {
            SqlConnection connection = new SqlConnection(ConnectionStringSettings.ConnectionString);
            await connection.OpenAsync();
            return connection;
        }

        private void SetConnection()
        {
            connection = new SqlConnection(ConnectionStringSettings.ConnectionString);
            connection.Open();
        }

        public void Dispose()
        {
            if (connection?.State == ConnectionState.Open)
                connection.Close();
            connection?.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_lazyAsyncConnection.IsValueCreated)
            {
                SqlConnection connection = await _lazyAsyncConnection.Value;

                if (connection?.State == ConnectionState.Open)
                    await connection.CloseAsync();

                if (connection != null)
                    await connection.DisposeAsync();
            }

            connection?.Dispose();
        }
    }
}

