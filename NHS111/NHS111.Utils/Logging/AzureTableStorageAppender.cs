using System;
using System.Data.Services.Client;
using log4net.Appender;
using log4net.Core;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.Logging;

namespace NHS111.Utils.Logging
{
    public sealed class AzureTableStorageAppender : AppenderSkeleton
    {
        private readonly ILogServiceContext _logServiceContext;

        public AzureTableStorageAppender(ILogServiceContext logServiceContext)
        {
            _logServiceContext = logServiceContext;
        }

        public AzureTableStorageAppender()
        {
            try
            {
                _logServiceContext = new LogServiceContext(TableStorageAccountName, TableStorageAccountKey, TableStorageName);
            }
            catch (DataServiceRequestException e)
            {
                ErrorHandler.Error(string.Format("{0}: Could not write log entry to {1}: {2}", GetType().AssemblyQualifiedName, TableStorageName, e.Message));
            }
        } 

        public string TableStorageAccountName { get; set; }

        public string TableStorageAccountKey { get; set; }

        public string TableStorageName { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                var auditEntry = JsonConvert.DeserializeObject<AuditEntry>(loggingEvent.RenderedMessage);
                if (auditEntry != null && typeof(AuditEntry) == auditEntry.GetType())
                    _logServiceContext.Log(auditEntry);
            }
            catch (DataServiceRequestException e)
            {
                ErrorHandler.Error(string.Format("{0}: Could not write log entry to {1}: {2}", GetType().AssemblyQualifiedName, TableStorageName, e.Message));
            }
        }
    }
}
