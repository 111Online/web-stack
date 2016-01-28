using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NHS111.Utils.Configuration
{
    public class SqliteConfiguration : ISqliteConfiguration
    {
        public string GetSqliteConnectionString()
        {
            return ConfigurationManager.AppSettings["SqliteDbConnectionString"];
        }
    }

    public interface ISqliteConfiguration
    {
        string GetSqliteConnectionString();
    }
}