using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using KafkaNet;
using KafkaNet.Model;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Cache;
using NHS111.Utils.Configuration;
using NHS111.Utils.Converters;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Utils.IoC
{
    public class UtilsRegistry : Registry
    {
        public UtilsRegistry()
        {
            //For<Producer>().Use(new Producer(new BrokerRouter(new KafkaOptions(new Uri("net.tcp://kafka.dev.medplus.steinhauer.technology:9092")))));
            For<ISqliteConfiguration>().Use<SqliteConfiguration>().Singleton();
            For<IConnectionManager>().Use<SqliteConnectionManager>().Singleton();
            For(typeof(IDataConverter<Feedback>)).Use(typeof(FeedbackConverter));
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}