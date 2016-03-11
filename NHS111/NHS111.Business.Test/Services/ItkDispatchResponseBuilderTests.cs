
namespace NHS111.Business.Test.Services {
    using System.Net;
    using ITKDispatcher.Api.ITKDispatcherSOAPService;
    using ITKDispatcher.Api.Mappings;
    using NUnit.Framework;

    public class ItkDispatchResponseBuilderTests {
        ItkDispatchResponseBuilder _responseBuilder = new ItkDispatchResponseBuilder();

        [Test]
        public void Build_WithSuccessResponse_BuildsSuccessResponse() {

            var submitHaSCToServiceResponse = new SubmitHaSCToServiceResponse {
                SubmitEncounterToServiceResponse = new SubmitEncounterToServiceResponse {
                    OverallStatus = submitEncounterToServiceResponseOverallStatus.Successful_call_to_gp_webservice
                }
            };

            var convertedResponse = _responseBuilder.Build(submitHaSCToServiceResponse);
            Assert.AreEqual(HttpStatusCode.OK, convertedResponse.StatusCode);
        }

        [Test]
        public void Build_WithFailedResponse_BuildsErrorResponse() {

            var submitHaSCToServiceResponse = new SubmitHaSCToServiceResponse {
                SubmitEncounterToServiceResponse = new SubmitEncounterToServiceResponse {
                    OverallStatus = submitEncounterToServiceResponseOverallStatus.Failed_call_to_gp_webservice
                }
            };

            var convertedResponse = _responseBuilder.Build(submitHaSCToServiceResponse);
            Assert.AreEqual(HttpStatusCode.InternalServerError, convertedResponse.StatusCode);
        }
    }
}
