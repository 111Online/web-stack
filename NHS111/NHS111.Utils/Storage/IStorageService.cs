using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace NHS111.Utils.Storage
{
    public interface IStorageService
    {
        T GetEntity<T>(Func<T, bool> lambda) where T : ITableEntity, new();

        IEnumerable<T> GetAllEntities<T>(Func<T, bool> lambda) where T : ITableEntity, new();
    }
}
