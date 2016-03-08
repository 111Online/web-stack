using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private IMapper _mappingEngine;
        private IItkDispatchResponseBuilder _itkDispatchResponseBuilder;

        public ItkDispatcherController(MessageEngine itkDispatcher, IMapper mappingEngine, IItkDispatchResponseBuilder itkDispatchResponseBuilder)
        {
            _itkDispatcher = itkDispatcher;
            _mappingEngine = mappingEngine;
            _itkDispatchResponseBuilder = itkDispatchResponseBuilder;
        }

        [HttpPost]
        [Route("SendItkMessage")]
        public async Task<ITKDispatchResponse> SendItkMessage(ITKDispatchRequest request)
        {
            var submitHaSCToService = _mappingEngine.Map<ITKDispatchRequest, SubmitHaSCToService>(request);
            var response = await _itkDispatcher.SubmitHaSCToServiceAsync(submitHaSCToService);
            return _itkDispatchResponseBuilder.Build(response);
        }
    }
}
