using Microsoft.Extensions.Configuration;

namespace FFAppMiddleware.Model.Settings
{
    public class ConnectionStringSettings
    {
        private static string _spherusPharmaConnectionString = string.Empty;

        private static string _spherusMainConnectionString = string.Empty;

        private static string _spherusPharmaFFConnectionString = string.Empty;

        public static string SpherusPharma
        {
            get { return _spherusPharmaConnectionString; }
        }

        public static string SpherusMain
        {
            get { return _spherusMainConnectionString; }
        }

        public static string SpherusFarmaFF
        {
            get { return _spherusPharmaFFConnectionString; }
        }

        public static void InitializeConnectionString(IConfiguration configuration)
        {
            _spherusPharmaConnectionString = configuration.GetConnectionString("SpherusPharma");

            _spherusMainConnectionString = configuration.GetConnectionString("SpherusMain");
            _spherusPharmaFFConnectionString = configuration.GetConnectionString("SpherusPharmaFF");
        }
    }
}
