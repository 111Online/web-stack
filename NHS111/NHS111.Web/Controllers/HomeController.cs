using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using NHS111.Models.Models.Web;
using NHS111.Utils.Attributes;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("COVID-19")]
        [Route("service/COVID-19")]
        //Special route for Covid direct link from other services to tidy up..
        public ActionResult StartCovidJourney(JourneyViewModel model)
        {
            return View("AboutCovid", model);
        }

        [HttpGet]
        [Route("{pathwayNumber}/{sessionId}/{digitalTitle}/about")] // Old link, kept so it doesn't 404
        public ActionResult RedirectSessionCovidJourney()
        {
            return RedirectPermanent("/service/COVID-19");
        }

        [HttpGet]
        [Route("COVID-19/stayathome")]
        public ActionResult StayAtHomeHub(JourneyViewModel model)
        {
            var firstModel = BuildModel("PC111", model.SessionId);
            return View("StayAtHomeHub", firstModel);
        }

        [HttpPost]
        public ActionResult StayAtHome(OutcomeViewModel model)
        {
            ModelState.Clear();
            return View("StayAtHomeHub", model);
        }

        private static QuestionInfoViewModel BuildModel(string pathwayNumber, Guid sessionId)
        {
            var state = new Dictionary<string, string>();
            state.Add("PATIENT_GENDER", "\"M\"");
            state.Add("PATIENT_AGE", "-1");

            var model = new QuestionInfoViewModel
            {
                SessionId = sessionId,
                PathwayNo = pathwayNumber,
                EntrySearchTerm = string.Format("External get to {0}", pathwayNumber),
                State = state,
                StateJson = JsonConvert.SerializeObject(state),
                UserInfo = new UserInfo
                {
                    Demography = new AgeGenderViewModel
                    {
                        Age = 111,
                        Gender = "Male"
                    }
                }
            };
            return model;
        }
    }
}