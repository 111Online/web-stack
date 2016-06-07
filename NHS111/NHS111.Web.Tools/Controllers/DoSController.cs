using NHS111.Models.Models.Web;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Builders;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Web.Tools.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Presentation.Configuration;

    [LogHandleErrorForMVC]
    public class DoSController : Controller
    {
        private readonly IDOSBuilder _dosBuilder;
        private readonly ISurgeryBuilder _surgeryBuilder;
        private readonly IConfiguration _configuration;

        public DoSController(IDOSBuilder dosBuilder, ISurgeryBuilder surgeryBuilder, IConfiguration configuration)
        {
            _dosBuilder = dosBuilder;
            _surgeryBuilder = surgeryBuilder;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult> Search()
        {
            var model = new DosViewModel();
            model.Dispositions = await ListDispositions();
            return View(model);
        }

        private async Task<IEnumerable<OutcomeViewModel>> ListDispositions() {
            var url = _configuration.GetBusinessApiListOutcomesUrl();
            HttpResponseMessage response;
            using (var http = new HttpClient()) {
                try {
                    response = await http.GetAsync(url);
                } catch (Exception e) {
                    throw new Exception(string.Format("An error occured requesting {0}. See inner exception for details.", url), e);
                }
            }
            return JsonConvert.DeserializeObject<IEnumerable<OutcomeViewModel>>(await response.Content.ReadAsStringAsync());

        }

        [HttpPost]
        public async Task<ActionResult> FillServiceDetails(DosViewModel model)
        {
            model.SymptomDiscriminatorList = new[] { model.SymptomDiscriminator };
            var dosView = new DosViewModel
            {
                DosCheckCapacitySummaryResult = (await _dosBuilder.FillCheckCapacitySummaryResult(model)),
                DosServicesByClinicalTermResult = (await _dosBuilder.FillDosServicesByClinicalTermResult(model))
            };

            return PartialView("_DoSComparisionResultsView", dosView);
        }

        [HttpPost]
        public async Task<JsonResult> SearchSurgery(string input)
        {
            return Json((await _surgeryBuilder.SearchSurgeryBuilder(input)));
        }
    }
}