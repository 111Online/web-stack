using System;
using System.Configuration;
using KafkaNet;
using KafkaNet.Model;
using NHS111.Utils.Cache;
using NHS111.Utils.Configuration;
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
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}