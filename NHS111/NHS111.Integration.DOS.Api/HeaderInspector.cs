
namespace NHS111.Integration.DOS.Api {
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    public class HeaderInspectionBehavior : IEndpointBehavior {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) {
            clientRuntime.MessageInspectors.Add(new HeaderInspector());
        }

        public void Validate(ServiceEndpoint endpoint) { }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) { }
    }

    public class HeaderInspector : IClientMessageInspector {
        public void AfterReceiveReply(ref Message reply, object correlationState) { }

        public object BeforeSendRequest(ref Message request, IClientChannel channel) {
            request.Headers.RemoveAll("serviceVersion", "https://nww.pathwaysdos.nhs.uk/app/api/webservices");
            var header = new MessageHeader<string>("1.3");
            var untyped = header.GetUntypedHeader("serviceVersion", string.Empty);
            request.Headers.Add(untyped);
            return null;
        }
    }
}