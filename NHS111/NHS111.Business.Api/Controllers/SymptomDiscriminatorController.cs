using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NHS111.Business.Services;
using NHS111.Utils.Cache;
using NHS111.Utils.Extensions;

namespace NHS111.Business.Api.Controllers
{
    public class SymptomDiscriminatorController : ApiController
    {
        private readonly ISymptomDisciminatorService _symptomDisciminatorService;
        private readonly ICacheManager<string, string> _cacheManager;

        public SymptomDiscriminatorController(ISymptomDisciminatorService symptomDisciminatorService, ICacheManager<string, string> cacheManager)
        {
            _symptomDisciminatorService = symptomDisciminatorService;
            _cacheManager = cacheManager;
        }

       [Route("symptomdiscriminator/{symptomDisciminatorCode}")]
        public async Task<HttpResponseMessage> GetSymptomDisciminator(string symptomDisciminatorCode, string cacheKey = null)
        {
            #if !DEBUG
                cacheKey = cacheKey ?? string.Format("SymptomDisciminator-{0}", symptomDisciminatorCode);

                var cacheValue = await _cacheManager.Read(cacheKey);
                if (cacheValue != null)
                {
                    return cacheValue.AsHttpResponse();
                }
            #endif

            return await _symptomDisciminatorService.GetSymptomDisciminator(symptomDisciminatorCode).AsHttpResponse();
        }
    }
}
