using Microsoft.Extensions.Configuration;

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
        }
    }
}
