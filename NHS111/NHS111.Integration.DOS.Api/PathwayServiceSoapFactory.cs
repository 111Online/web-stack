
namespace NHS111.Integration.DOS.Api
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Web;
    using DOSService;

    public interface IPathwayServiceSoapFactory {
        PathWayServiceSoap Create(HttpRequestMessage request);
    }

    public class PathwayServiceSoapFactory : IPathwayServiceSoapFactory {
        public PathWayServiceSoap Create(HttpRequestMessage request) {
            PathWayServiceSoapClient client;

            var values = HttpUtility.ParseQueryString(request.RequestUri.Query);
            if (values["endpoint"] == null || values["endpoint"] == "Unspecified")
                client = new PathWayServiceSoapClient();
            else {
                var endpoint = values["endpoint"] == "Live"
                    ? ConfigurationManager.AppSettings["dos-live-endpoint"]
                    : ConfigurationManager.AppSettings["dos-uat-endpoint"];
                var uri = new Uri(endpoint);
                var binding = new CustomBinding();
                var httpsBindingElement = new HttpsTransportBindingElement {
                    MaxReceivedMessageSize = 2000000000
                };
                binding.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12, Encoding.UTF8));
                binding.Elements.Add(httpsBindingElement);
                var endPointAddress = new EndpointAddress(uri);
                client = new PathWayServiceSoapClient(binding, endPointAddress);
            }

            client.Endpoint.Behaviors.Add(new HeaderInspectionBehavior());
            return client;
        }
    }
}