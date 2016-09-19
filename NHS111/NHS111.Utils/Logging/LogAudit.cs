using log4net;
using log4net.Core;

namespace NHS111.Utils.Logging
{
    public static class LogAudit
    {
        private static readonly Level audit = new Level(50000, "AUDIT");

        public static void Audit(this ILog log, string message)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, audit, message, null);
        }

        public static void Audit(this ILog log, string message, params object[] args)
        {
            var formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, audit, formattedMessage, null);
        }

    }
}
