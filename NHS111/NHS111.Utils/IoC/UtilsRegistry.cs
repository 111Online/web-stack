using NHS111.Utils.Helpers;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Utils.IoC
{
    public class UtilsRegistry : Registry
    {
        public UtilsRegistry()
        {
            //For<Producer>().Use(new Producer(new BrokerRouter(new KafkaOptions(new Uri("net.tcp://kafka.dev.medplus.steinhauer.technology:9092")))));
            For<IRestfulHelper>().Use<RestfulHelper>().SelectConstructor(() => new RestfulHelper());
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}