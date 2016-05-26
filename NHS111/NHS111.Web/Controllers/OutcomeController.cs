using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;

namespace NHS111.Web.Controllers
{
    using Models.Models.Domain;

    [LogHandleErrorForMVC]
    public class OutcomeController : Controller
    {
        private readonly IOutcomeViewModelBuilder _outcomeViewModelBuilder;
        private readonly IDOSBuilder _dosBuilder;
        private readonly IConfiguration _config;
        private readonly ISurgeryBuilder _surgeryBuilder;

        public OutcomeController(IOutcomeViewModelBuilder outcomeViewModelBuilder, IDOSBuilder dosBuilder, IConfiguration config, ISurgeryBuilder surgeryBuilder)
        {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _dosBuilder = dosBuilder;
            _config = config;
            _surgeryBuilder = surgeryBuilder;
        }

        [HttpPost]
        public async Task<JsonResult> SearchSurgery(string input)
        {
            return Json((await _surgeryBuilder.SearchSurgeryBuilder(input)));
        }

        [HttpGet]
        [Route("outcome/disposition/{age?}/{gender?}/{dxCode?}/{symptomGroup?}/{symptomDiscriminator?}")]
        public ActionResult Disposition(int? age, string gender, string dxCode, string symptomGroup, string symptomDiscriminator) {
            var DxCode = new DispositionCode(dxCode ?? "Dx38");
            var Gender = new Gender(gender ?? "Male");

            var model = new OutcomeViewModel {
                Id = DxCode.Value,
                UserInfo = new UserInfo {
                    Age = age ?? 38,
                    Gender = Gender.Value
                },
                SymptomGroup = symptomGroup ?? "1203",
                SymptomDiscriminator = symptomDiscriminator ?? "4003",
                AddressSearchViewModel = new AddressSearchViewModel {
                    PostcodeApiAddress = _config.PostcodeSearchByIdApiUrl,
                    PostcodeApiSubscriptionKey = _config.PostcodeSubscriptionKey
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceList(OutcomeViewModel model)
        {
            var dosCase = AutoMapper.Mapper.Map<DosViewModel>(model);
            model.CheckCapacitySummaryResultList = await _dosBuilder.FillCheckCapacitySummaryResult(dosCase);
            return View("ServiceList", model);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceDetails(OutcomeViewModel model)
        {
            var dosCase = AutoMapper.Mapper.Map<DosViewModel>(model);
            model.CheckCapacitySummaryResultList = await _dosBuilder.FillCheckCapacitySummaryResult(dosCase);
            return View("ServiceDetails", model);
        }

        [HttpPost]
        public async Task<ActionResult> PersonalDetails(OutcomeViewModel model)
        {
            model = await _outcomeViewModelBuilder.PersonalDetailsBuilder(model);
            return View("PersonalDetails", model);
        }

        [HttpPost]
        public async Task<ActionResult> Confirmation(OutcomeViewModel model)
        {
            model = await _outcomeViewModelBuilder.ItkResponseBuilder(model);
            if (model.ItkSendSuccess.HasValue && model.ItkSendSuccess.Value)
                 return View(model);
            return View("ServiceBookingFailure", model);
        }

        [HttpPost]
        public ActionResult Emergency()
        {
            return View();
        }
    }
}