using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Business.ITKDispatcher.Api.ITKDispatcherSOAPService;
using NHS111.Business.ITKDispatcher.Api.Mappings;
using NHS111.Models.Models.Web.ITK;


namespace NHS111.Business.ITKDispatcher.Api.Controllers
{
    public class ItkDispatcherController : ApiController
    {
        private MessageEngine _itkDispatcher;
        private IMappingEngine _mappingEngine;
        private IItkDispatchResponseBuilder _itkDispatchResponseBuilder;


        public ItkDispatcherController(MessageEngine itkDispatcher, IMappingEngine mappingEngine, IItkDispatchResponseBuilder itkDispatchResponseBuilder)
        {
            _itkDispatcher = itkDispatcher;
            _mappingEngine = mappingEngine;
            _itkDispatchResponseBuilder = itkDispatchResponseBuilder;
        }

        [HttpPost]
        [Route("SendItkMessage")]
        public async Task<ITKDispatchResponse> SendItkMessage(ITKDispatchRequest request)
        {
            BypassCertificateError();
            var submitHaSCToService = _mappingEngine.Mapper.Map<ITKDispatchRequest, SubmitHaSCToService>(request);
            SubmitHaSCToServiceResponse response = null;
            try {
                response = await _itkDispatcher.SubmitHaSCToServiceAsync(submitHaSCToService);
            }
            catch (Exception ex) {
                return _itkDispatchResponseBuilder.Build(ex);
            }
            return _itkDispatchResponseBuilder.Build(response);
        }

        /// <summary>
        /// Temorary sssl cert validation bypass until ESB hosting has domain name
        /// </summary>
        private void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback +=

                delegate(
                    Object sender1,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }
    }
}
