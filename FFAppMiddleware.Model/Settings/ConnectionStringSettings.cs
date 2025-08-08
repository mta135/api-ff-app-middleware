using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Settings
{
    public class ConnectionStringSettings
    {
        private static string _connectionString = string.Empty;

        public static string ConnectionString
        {
            get { return _connectionString; }

        }

        public static void InitializeConnectionString(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:SpherusPharma"];

            //string connStr = configuration.GetConnectionString("SpherusPharma");
            //if (string.IsNullOrEmpty(connStr))
            //{
            //    connStr = configuration["ConnectionStrings:SpherusPharma"];
            //}
            //_connectionString = connStr ?? string.Empty;
        }
    }
}
