using log4net;
using log4net.Config;

namespace NHS111.Utils.Logging
{
    public static class Log4Net
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Log4Net));

        static Log4Net()
        {
            XmlConfigurator.Configure();
        }

        public static void Debug(string msg)
        {
            Logger.Debug(msg);
        }

        public static void Info(string msg)
        {
            Logger.Info(msg);
        }

        public static void Error(string msg)
        {
            Logger.Error(msg);
        }
    }
}
