using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace NHS111.Utils.Logging
{
    public class LogServiceContext : ILogServiceContext
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

        public void Log<T>(T entity) where T : ITableEntity
        {
            Action doWriteToTable = () =>
            {
                var insertOperation = TableOperation.Insert(entity);
                _table.Execute(insertOperation);
            };
            doWriteToTable.BeginInvoke(null, null);
        }
    }

    public interface ILogServiceContext
    {
        void Log<T>(T entity) where T : ITableEntity;
    }
}
