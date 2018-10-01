﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NHS111.Models.Models.Azure;

namespace NHS111.Utils.Storage
{
    public class AzureStorageService : IStorageService
    {
        private readonly CloudTable _table;

        public AzureStorageService()
        {
            // Retrieve the storage account from the connection string.
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            // Create the table client.
            var tableClient = storageAccount.CreateCloudTableClient();
            // Retrieve a reference to the table.
            _table = tableClient.GetTableReference(CloudConfigurationManager.GetSetting("StorageTableReference"));
            // Create the table if it doesn't exist.
            _table.CreateIfNotExists();
        }
        public T GetEntity<T>(Func<T, bool> lambda) where T : ITableEntity, new()
        {
            var retrievedResult = _table.CreateQuery<T>().Where(lambda);
            return retrievedResult.FirstOrDefault();
        }

        public IEnumerable<T> GetAllEntities<T>(Func<T, bool> lambda) where T : ITableEntity, new()
        {
            var retrievedResults = _table.CreateQuery<T>().Where(lambda);
            return retrievedResults;
        }
    }
}
