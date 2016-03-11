
namespace NHS111.Business.Test.Services {
    using System;
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

        [Test]
        public void Build_WithSuccessResponse_BuildsResponseWithSuccessText() {

            var submitHaSCToServiceResponse = new SubmitHaSCToServiceResponse {
                SubmitEncounterToServiceResponse = new SubmitEncounterToServiceResponse {
                    OverallStatus = submitEncounterToServiceResponseOverallStatus.Successful_call_to_gp_webservice
                }
            };

            var result = _responseBuilder.Build(submitHaSCToServiceResponse);

            Assert.AreEqual(submitEncounterToServiceResponseOverallStatus.Successful_call_to_gp_webservice.ToString(), result.Body);
        }

        [Test]
        public void Build_WithFailedResponse_BuildsResponseWithFailureText() {

            var submitHaSCToServiceResponse = new SubmitHaSCToServiceResponse {
                SubmitEncounterToServiceResponse = new SubmitEncounterToServiceResponse {
                    OverallStatus = submitEncounterToServiceResponseOverallStatus.Failed_call_to_gp_webservice
                }
            };

            var result = _responseBuilder.Build(submitHaSCToServiceResponse);

            Assert.AreEqual(submitEncounterToServiceResponseOverallStatus.Failed_call_to_gp_webservice.ToString(), result.Body);
        }

        [Test]
        public void Build_WithException_BuildsFaultResponse() {

            var result = _responseBuilder.Build(new Exception());

            Assert.AreEqual("An error has occured processing the request.", result.Body);
        }
    }
}
