using System;
using System.Data.Services.Client;
using log4net.Appender;
using log4net.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.Logging;

namespace NHS111.Utils.Logging
{
    public class AzureTableStorageAppender : AppenderSkeleton
    {
        public string TableStorageAccountName { get; set; }

        public string TableStorageAccountKey { get; set; }

        public string TableStorageName { get; set; }

        private LogServiceContext _logServiceContext;

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            _logServiceContext = new LogServiceContext(TableStorageAccountName, TableStorageAccountKey, TableStorageName);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            Action doWriteToLog = () => {
                try
                {
                    var logEntry = JsonConvert.DeserializeObject<LogEntry>(loggingEvent.RenderedMessage);
                    _logServiceContext.Log(logEntry);
                }
                catch (DataServiceRequestException e)
                {
                    ErrorHandler.Error(string.Format("{0}: Could not write log entry to {1}: {2}", GetType().AssemblyQualifiedName, TableStorageName, e.Message));
                }
            };
            doWriteToLog.BeginInvoke(null, null);
        }
    }
}
