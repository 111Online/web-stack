using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using NHS111.Business.ITKDispatcher.Api.ITKDispatcherSOAPService;


namespace NHS111.Business.ITKDispatcher.Api.Controllers
{
    public class ItkDispatcherController : ApiController
    {
        private MessageEngine _itkDispatcher;
        public ItkDispatcherController(MessageEngine itkDispatcher)
        {
            _itkDispatcher = itkDispatcher;
        }

        //[HttpPost]
        //[Route("SendItkMessage")]
        //public async Task<CheckCapacitySummaryResponse> CheckCapacitySummary(ITKDispatchRequest request)
        //{
        //    _itkDispatcher.SubmitHaSCToServiceAsync(SubmitHaSCToService)
        //}
    }
}
