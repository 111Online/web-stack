using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using NHS111.Models.Models.Web.Logging;

namespace NHS111.Utils.Logging
{
    internal class LogServiceContext
    {
        private readonly CloudTable _table;
        public LogServiceContext(string accountName, string accountKey, string storageTable)
            : this(new StorageCredentials(accountName, accountKey), storageTable)
        {
        }

        public LogServiceContext(StorageCredentials credentials, string storageTable)
        {
            var account = new CloudStorageAccount(credentials, true);

            var client = account.CreateCloudTableClient();

            _table = client.GetTableReference(storageTable);
            _table.CreateIfNotExists();
        }

        internal void Log(LogEntry logEntry)
        {
            var insertOperation = TableOperation.Insert(logEntry);
            _table.Execute(insertOperation);
        }
    }
}
