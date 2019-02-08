using System.Reflection;
using System.Threading.Tasks;
using NHS111.Utils.Monitoring;

namespace NHS111.Logging.Api.Monitoring
{
    public class Monitor : BaseMonitor
    {
        public override string Metrics()
        {
            return "Metrics";
        }

        public override async Task<bool> Health()
        {
            return true;
        }

        public override string Version() {
            return Assembly.GetCallingAssembly().GetName().Version.ToString();
        }
    }
}