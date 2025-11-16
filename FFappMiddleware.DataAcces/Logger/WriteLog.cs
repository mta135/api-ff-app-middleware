using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FFappMiddleware.DataBase.Logger
{
    public class WriteLog
    {
        public static ILog DB { get; private set; }
        public static ILog Web { get; private set; }

        public static void InitLoggers()
        {
            #region log4net Static

            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());

            var fileInfo = new FileInfo(@"log4net.config");

            log4net.Config.XmlConfigurator.Configure(repository, fileInfo);

            #endregion 

            Web = LogManager.GetLogger("WebAppender");
            DB = LogManager.GetLogger("DatabaseAppender");
        }
    }
}
