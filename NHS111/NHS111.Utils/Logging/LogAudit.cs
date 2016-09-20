

namespace NHS111.Utils.Logging
{
    using Models.Models.Web.Logging;
    using log4net;
    using log4net.Core;

    public static class LogAudit
    {
        public static readonly Level AuditLevel = new Level(50000, "AUDIT");

        public static void Audit(this ILog log, string message)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, AuditLevel, message, null);
        }

        public static void Audit(this ILog log, string message, params object[] args)
        {
            var formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, AuditLevel, formattedMessage, null);
        }

        public static void Audit(this ILog log, AuditEntry message)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, AuditLevel, message, null);
        }

    }
}
