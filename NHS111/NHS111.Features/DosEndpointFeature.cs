
namespace NHS111.Features
{
    using Defaults;
    using System.Linq;
    using System.Web;

    public class DosEndpointFeature : BaseFeature, IDosEndpointFeature
    {

        public DosEndpointFeature()
        {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }

        public bool RequestIncludesEndpoint(HttpRequestBase request)
        {
            return request.QueryString.AllKeys.Contains(_endpointKeyname);
        }

        public string GetEndpoint(HttpRequestBase request)
        {
            return request.QueryString[_endpointKeyname];
        }

        private readonly string _endpointKeyname = "dos";
    }

    public interface IDosEndpointFeature : IFeature
    {
        bool RequestIncludesEndpoint(HttpRequestBase request);
        string GetEndpoint(HttpRequestBase request);
    }

}