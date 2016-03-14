
namespace NHS111.Business.ITKDispatcher.Api.Monitoring {

    using System.Reflection;
    using System.Threading.Tasks;
    using Utils.Monitoring;

    public class Monitor
        : BaseMonitor {

        public override string Metrics() {
            return "Metrics";
        }

        public override async Task<bool> Health() {
            return false;
        }

        public override string Version() {
            return Assembly.GetCallingAssembly().GetName().Version.ToString();
        }
    }
}