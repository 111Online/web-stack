using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class OutcomeController : Controller
    {
        private readonly IOutcomeViewModelBuilder _outcomeViewModelBuilder;
        private readonly IDOSBuilder _dosBuilder;

        public OutcomeController(IOutcomeViewModelBuilder outcomeViewModelBuilder, IDOSBuilder dosBuilder)
        {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _dosBuilder = dosBuilder;
        }

        [HttpPost]
        public async Task<JsonResult> SearchSurgery(string input)
        {
            return Json((await _outcomeViewModelBuilder.SearchSurgeryBuilder(input))
                .SurgeryViewModel.Surgeries);
        }

        [HttpPost]
        public ActionResult Emergency()
        {
            return View();
        }

        [HttpPost]
        [ActionName("DispositionSelection")]
        [MultiSubmit(ButtonName = "DispositionNo")]
        public async Task<ActionResult> DispositionNo(OutcomeViewModel model)
        {
            model = await _outcomeViewModelBuilder.PersonalDetailsBuilder(model);
            return View("DispositionNo", model);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceDetails(OutcomeViewModel model)
        {
            var dosViewModel = await _dosBuilder.DosResultsBuilder(model);
            model.CheckCapacitySummaryResultList = dosViewModel.CheckCapacitySummaryResultList;
            return View("ServiceDetails", model);
        }

        [HttpPost]
        public ActionResult Confirmation(OutcomeViewModel model)
        {
            //submit to ITK;
            return View();
        }
        
        [HttpGet]
        public ActionResult PersonalDetails()
        {
            var config = new Configuration();
            var model = new OutcomeViewModel()
            {
                Id = "Dx38",
                UserInfo = new UserInfo()
                {
                    Age = 38,
                    Gender = "Male"
                },
                CheckCapacitySummaryResultList = new[]
                {
                    new CheckCapacitySummaryResult()
                    {
                        AddressField = "70 blah street, blah blah",
                        IdField = 17,
                        NameField = "Test service",
                        OpenAllHoursField = true,

                    }
                },
                SelectedServiceId = "17",
                SymptomGroup = "1203",
                SymptomDiscriminator = "4003",
                AddressSearchViewModel = new AddressSearchViewModel()
                {
                    PostcodeApiAddress = config.PostcodeSearchByIdApiUrl,
                    PostcodeApiSubscriptionKey = config.PostcodeSubscriptionKey
                }
            };

            return View("PersonalDetails", model);
        }

        [HttpPost]
        public async Task<ActionResult> PersonalDetails(OutcomeViewModel model)
        {
            model = await _outcomeViewModelBuilder.PersonalDetailsBuilder(model);
            return View("PersonalDetails", model);
        }


    }
}