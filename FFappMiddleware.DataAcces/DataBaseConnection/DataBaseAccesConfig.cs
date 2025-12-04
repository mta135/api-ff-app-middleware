using FFappMiddleware.DataBase.EncryptionService;
using FFappMiddleware.DataBase.Logger;
using FFAppMiddleware.Model.Settings;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace FFappMiddleware.DataAcces.DataBaseConnection
{
    public class DataBaseAccesConfig
    {
        private SqlConnection _connection;

        private SqlConnection _pharmaFFconnection;

        public DataBaseAccesConfig()
        {
          
        }

        #region SpherusPharma

        public Task<SqlConnection> ConnectionAsync => OpenConnectionAsync();

        private async Task<SqlConnection> OpenConnectionAsync()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                if (_connection != null)
                    await _connection.DisposeAsync();


                var valu = Environment.GetEnvironmentVariable("Aes_Public_Key");

                string connectionString = AesEncryptionHelper.Decrypt(ConnectionStringSettings.SpherusFarmaFF, "k65gR0Q3E0nKLxNk8A1Ceg==");

                _connection = new SqlConnection(connectionString);
                await _connection.OpenAsync();
            }

            return _connection;
        }

        #endregion

        #region PharmaFFConnection

        public Task<SqlConnection> PharmaFFConnectionAsync
        {
            get
            {
                return OpenPharmaFFConnetionAsync();
            }
        }
            
        private async Task<SqlConnection> OpenPharmaFFConnetionAsync()
        {
            if (_pharmaFFconnection == null || _pharmaFFconnection.State != ConnectionState.Open)
            {
                if (_pharmaFFconnection != null)
                    await _connection.DisposeAsync();

                _pharmaFFconnection = new SqlConnection(AesEncryptionHelper.Decrypt(ConnectionStringSettings.SpherusFarmaFF, "k65gR0Q3E0nKLxNk8A1Ceg=="));

                string connStr = _pharmaFFconnection.ConnectionString;

                await _pharmaFFconnection.OpenAsync();
            }

            return _pharmaFFconnection;
        }

        #endregion

        #region Dispose

        public async Task DisposeAsync()
        {
            await DisposeConnectionAsync(_connection);

            await DisposeConnectionAsync(_pharmaFFconnection);
        }

        private async Task DisposeConnectionAsync(DbConnection connection)
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();

                await connection.DisposeAsync();
            }
        }

        public async Task DisposeObjectsAsync(params object[] items)
        {
            if (items == null || items.Length == 0) return;

            foreach (var item in items)
            {
                if (item == null) continue;

                try
                {
                    if (item is IAsyncDisposable asyncDisposable)
                    {
                        await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                    }
                    else if (item is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Eroare la Dispose: {ex.Message}");
                }
            }
        }

        #endregion
    }
}

