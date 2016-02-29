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
    public class DOSController : Controller
    {
        private readonly IDOSBuilder _dosBuilder;

        public DOSController(IDOSBuilder dosBuilder)
        {
            _dosBuilder = dosBuilder;
        }

        [HttpGet]
        public ActionResult PersonalDetails()
        {
            var model = new DosViewModel()
            {
                Id = "Dx38",
                Age = "38",
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
                Gender = "Female",
                SymptomGroup = "1203",
                SymptomDiscriminator = "4003"
            };
            
            return View("DosPersonalDetails", model);
        }

        //[HttpPost]
        //public async Task<ActionResult> PersonalDetails(OutcomeViewModel model)
        //{
        //    model = await _outcomeViewModelBuilder.PersonalDetailsBuilder(model);
        //    return View("PersonalDetails", model);
        //}


        [HttpPost]
        public async Task<ActionResult> FillServiceDetails(DosViewModel model)
        {
            return View("../DOS/Confirmation", await _dosBuilder.FillServiceDetailsBuilder(model));
        }
    }
}