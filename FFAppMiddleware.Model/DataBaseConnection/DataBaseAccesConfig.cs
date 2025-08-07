using FFAppMiddleware.Model.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.DataBaseConnection
{
    public class DataBaseAccesConfig
    {
        private SqlConnection connection;

        public SqlConnection Connection
        {
            get
            {
                SetConnection();
                return this.connection;
            }
        }

        private void SetConnection()
        {
            connection = new SqlConnection(ConnectionStringSettings.ConnectionString);
            connection.Open();
        }

        public void Dispose()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

    }
}
}
