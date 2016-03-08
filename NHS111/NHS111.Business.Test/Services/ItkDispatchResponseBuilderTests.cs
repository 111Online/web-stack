using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Business.ITKDispatcher.Api.ITKDispatcherSOAPService;
using NHS111.Business.ITKDispatcher.Api.Mappings;
using NHS111.Models.Models.Web.ITK;
using NUnit.Framework;

namespace NHS111.Business.Test.Services
{
    public class ItkDispatchResponseBuilderTests
    {
        ItkDispatchResponseBuilder _responseBuilder = new ItkDispatchResponseBuilder();
        [Test]
        public void InitialisedResponse_Returns_Status_Unseent()
        {
            var response = new ITKDispatchResponse();
            Assert.AreEqual(response.SendSuccess, SentStatus.Unsent);
        }

        [Test]
        public void SubmitHaSCToServiceResponse_Success_Builds_ITKDispatchResponse_Success()
        {
            var submitHaSCToServiceResponse = new SubmitHaSCToServiceResponse()
            {
                SubmitEncounterToServiceResponse = new SubmitEncounterToServiceResponse()
                {
                    OverallStatus = submitEncounterToServiceResponseOverallStatus.Successful_call_to_gp_webservice,
                    RepeatCallerStatus = repeatCallerStatus.Undetermined 
                }
            };

            var convertedResponse = _responseBuilder.Build(submitHaSCToServiceResponse);
            Assert.AreEqual(convertedResponse.SendSuccess, SentStatus.Success);
        }

        [Test]
        public void SubmitHaSCToServiceResponse_Failure_Builds_ITKDispatchResponse_Failure()
        {
            var submitHaSCToServiceResponse = new SubmitHaSCToServiceResponse()
            {
                SubmitEncounterToServiceResponse = new SubmitEncounterToServiceResponse()
                {
                    OverallStatus = submitEncounterToServiceResponseOverallStatus.Failed_call_to_gp_webservice,
                    RepeatCallerStatus = repeatCallerStatus.Undetermined
                }
            };

            var convertedResponse = _responseBuilder.Build(submitHaSCToServiceResponse);
            Assert.AreEqual(convertedResponse.SendSuccess, SentStatus.Failure);
        }
    }
}
