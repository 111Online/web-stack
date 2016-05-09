using System.Threading.Tasks;
using System.Web.Mvc;
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

        public OutcomeController(IOutcomeViewModelBuilder outcomeViewModelBuilder, IDOSBuilder dosBuilder, IConfiguration config)
        {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _dosBuilder = dosBuilder;
            _config = config;
        }

        [HttpPost]
        public async Task<JsonResult> SearchSurgery(string input)
        {
            return Json((await _outcomeViewModelBuilder.SearchSurgeryBuilder(input))
                .SurgeryViewModel.Surgeries);
        }

        [HttpGet]
        public ActionResult Disposition(int? age, string gender = null, string dxCode = null, string symptomGroup = null, string symptomDiscriminator = null) {
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
            var dosViewModel = await _dosBuilder.DosResultsBuilder(model);
            model.CheckCapacitySummaryResultList = dosViewModel.CheckCapacitySummaryResultList;
            return View("ServiceList", model);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceDetails(OutcomeViewModel model)
        {
            var dosViewModel = await _dosBuilder.DosResultsBuilder(model);
            model.CheckCapacitySummaryResultList = dosViewModel.CheckCapacitySummaryResultList;
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

        //[HttpPost]
        //[ActionName("DispositionSelection")]
        //[MultiSubmit(ButtonName = "DispositionNo")]
        //public async Task<ActionResult> DispositionNo(OutcomeViewModel model)
        //{
        //    model = await _outcomeViewModelBuilder.PersonalDetailsBuilder(model);
        //    return View("DispositionNo", model);
        //}
    }
}