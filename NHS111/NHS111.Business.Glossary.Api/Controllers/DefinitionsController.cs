using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using NHS111.Business.Glossary.Api.Services;
using NHS111.Utils.Extensions;

namespace NHS111.Business.Glossary.Api.Controllers
{
    public class DefinitionsController : ApiController
    {

        private ITermsService _termsService;

        public DefinitionsController(ITermsService termsService)
        {
            _termsService = termsService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<HttpResponseMessage> ListFeedback()
        {
      
            var result = await _termsService.ListDefinedTerms().AsJson().AsHttpResponse();
            return result;
        }

        [HttpPost]
        [Route("findterms")]
        public async Task<HttpResponseMessage> FindTerms([FromBody]string text)
        {
            var result = await _termsService.FindContainedTerms(text).AsJson().AsHttpResponse();
            return result;
        }

    }
}
