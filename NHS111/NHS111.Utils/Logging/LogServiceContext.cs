using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace NHS111.Utils.Logging
{
    public class LogServiceContext : ILogServiceContext
    {
        private List<CloudTable> _tables = new List<CloudTable>();
        private string _defaultStorageTableName;
        private readonly CloudStorageAccount _storageAccount;
        public LogServiceContext(string accountName, string accountKey, string storageTable)
            : this(new StorageCredentials(accountName, accountKey), storageTable)
        {
        }

        public LogServiceContext(StorageCredentials credentials, string storageTable)
        {
            _storageAccount = new CloudStorageAccount(credentials, true);
            _defaultStorageTableName = storageTable;
            SetTableStorage(storageTable);
        }

        private CloudTable SetTableStorage(string storageTable)
        {
            var client = _storageAccount.CreateCloudTableClient();
            
            var table = client.GetTableReference(storageTable);
            table.CreateIfNotExists();
            _tables.Add(table);
            return table;
        }

        private CloudTable GetTable(string storageTable)
        {
            if (_tables.Exists(t => t.Name == storageTable)) return _tables.Where(t => t.Name == storageTable).First();
            return SetTableStorage(storageTable);
        }

        public void Log<T>(T entity, string tableName) where T : ITableEntity
        {
            Action doWriteToTable = () =>
            {
                var insertOperation = TableOperation.Insert(entity);
                GetTable(tableName).Execute(insertOperation);
            };
            doWriteToTable.BeginInvoke(null, null);

        }

        public void Log<T>(T entity) where T : ITableEntity
        {
            Log<T>(entity, _defaultStorageTableName);
        }
    }

    public interface ILogServiceContext
    {
        void Log<T>(T entity) where T : ITableEntity;
        void Log<T>(T entity, string tableName) where T : ITableEntity;
    }
}
