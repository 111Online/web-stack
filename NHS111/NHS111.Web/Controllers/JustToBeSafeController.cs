using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Utils.Attributes;
using NHS111.Web.Helpers;
using NHS111.Web.Presentation.Builders;

namespace NHS111.Web.Controllers
{
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
        [Route("COVID-19")]
        //Special route for Covid direct link to tidy up..
        public async Task<ActionResult> StartNonDemographicSpecificQuestion(string pathwayNumber, string digitalTitle)
        {
            var model = BuildModel("PWCorona", Guid.NewGuid().ToString(), digitalTitle);
            model.State.Add("SYSTEM_COVID_NHSUK", "yes");
            return await JustToBeSafeFirst(model);
        }

        [HttpGet]
        [Route("{pathwayNumber}/{sessionId}/{digitalTitle}/about")]
        public async Task<ActionResult> StartNonDemographicSpecificQuestion(string pathwayNumber, string sessionId, string digitalTitle)
        {
            var model = BuildModel(pathwayNumber, sessionId, digitalTitle);
            return await JustToBeSafeFirst(model);
        }
    

        [HttpGet]
        [Route("{pathwayNumber}/{gender}/{age}/start")]
        public async Task<ActionResult> FirstQuestion(string pathwayNumber, string gender, int age, string args) {
            var model = BuildModel(pathwayNumber, gender, age, args);
            return await JustToBeSafeFirst(model);
        }

        [HttpPost]
        [Route("Question/First")]
        public async Task<ActionResult> FirstQuestionDeeplink(JustToBeSafeViewModel model) {
            ModelState.Clear();
            return await JustToBeSafeFirst(model);
        }

        private static QuestionInfoViewModel BuildModel(string pathwayNumber, string sessionId, string digitalTitle)
        {
            var model = new QuestionInfoViewModel
            {
                SessionId = Guid.Parse(sessionId),
                PathwayNo = pathwayNumber,
                DigitalTitle = digitalTitle,
                UserInfo = new UserInfo
                {
                    Demography = new AgeGenderViewModel
                    {
                        Age = 111,
                        Gender = "Female"
                    }
                }
            };
            return model;
        }

        private static QuestionInfoViewModel BuildModel(string pathwayNumber, string gender, int age, string args) {
            var decryptedArgs = new QueryStringEncryptor(args);
            var decryptedFilterServices = string.IsNullOrEmpty(decryptedArgs["filterServices"]) ||
                                          bool.Parse(decryptedArgs["filterServices"]);

            var model = new QuestionInfoViewModel {
                SessionId = Guid.Parse(decryptedArgs["sessionId"]),
                PathwayNo = pathwayNumber,
                DigitalTitle = decryptedArgs["digitalTitle"],
                EntrySearchTerm = decryptedArgs["searchTerm"],
                CurrentPostcode = decryptedArgs["postcode"],
                UserInfo = new UserInfo {
                    Demography = new AgeGenderViewModel {
                        Age = age,
                        Gender = gender
                    }
                },
                FilterServices = decryptedFilterServices,
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"]
            };
            return model;
        }
    }
}