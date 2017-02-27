using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Builders;

namespace NHS111.Web.Controllers
{
    using System;
    using System.Runtime.CompilerServices;
    using Models.Models.Domain;
    using Models.Models.Web.Validators;
    using Utils.Parser;

    [LogHandleErrorForMVC]
    public class JustToBeSafeController : Controller
    {
        private readonly IJustToBeSafeFirstViewModelBuilder _justToBeSafeFirstViewModelBuilder;
        private readonly IJustToBeSafeViewModelBuilder _justToBeSafeViewModelBuilder;


        public JustToBeSafeController(IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder, IJustToBeSafeViewModelBuilder justToBeSafeViewModelBuilder)
        {
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
            _justToBeSafeViewModelBuilder = justToBeSafeViewModelBuilder;
        }

        [HttpPost]
        public async Task<ActionResult> JustToBeSafeFirst(JustToBeSafeViewModel model)
        {
            var viewData = await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(model);
            return View(viewData.Item1, viewData.Item2);
        }

        [HttpPost]
        public async Task<ActionResult> JustToBeSafeNext(JustToBeSafeViewModel model)
        {
            ModelState.Clear();
            var next = await _justToBeSafeViewModelBuilder.JustToBeSafeNextBuilder(model);
            return View(next.Item1, next.Item2);
        }

        [HttpGet]
        [Route("{pathwayId}/start")]
        public async Task<ActionResult> PathwayStart(string pathwayId, int age, string digitalTitle) {
            string pathwayNumber;
            Gender gender;
            AgeCategory ageCategory;
            if (!PathwayIdParser.TryParse(pathwayId, out pathwayNumber, out gender, out ageCategory))
                throw new ArgumentException("Unable to parse age and gender from Pathway id " + pathwayId);

            var model = new JustToBeSafeViewModel {
                PathwayNo = pathwayNumber,
                DigitalTitle = digitalTitle,
                UserInfo = new UserInfo {
                    Demography = new AgeGenderViewModel {
                        Age = age,
                        Gender = gender.Value
                    }
                }
            };

            return await JustToBeSafeFirst(model);
        }
    }
}