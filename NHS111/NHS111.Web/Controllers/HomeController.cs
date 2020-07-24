using Newtonsoft.Json;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Logging;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class HomeController : Controller
    {
        private readonly IAuditLogger _auditLogger;

        public HomeController(IAuditLogger auditLogger)
        {
            _auditLogger = auditLogger;
        }

        [HttpGet]
        [Route("portsmouth")]
        public ActionResult StartWithParam(JourneyViewModel model)
        {
            model.StartParameter = "portsmouth";
            return StartPortsmouthJourney(model);
        }

        [HttpGet]
        [Route("COVID-19")]
        [Route("service/COVID-19")]
        //Special route for Covid direct link from other services to tidy up..
        public ActionResult StartCovidJourney(JourneyViewModel model)
        {
            return View("AboutCovid", model);
        }

        public ActionResult StartPortsmouthJourney(JourneyViewModel model)
        { 
            _auditLogger.LogEvent(model, EventType.CustomStart, model.StartParameter, string.Format("../Home/{0}", model.StartParameter)); 
            return View("../Location/Home", model);
        }

        [HttpGet]
        [Route("{pathwayNumber}/{sessionId}/{digitalTitle}/about")] // Old link, kept so it doesn't 404
        public ActionResult RedirectSessionCovidJourney()
        {
            return RedirectPermanent("/service/COVID-19");
        }

        [HttpGet]
        [Route("COVID-19/stayathome")]
        public ActionResult StayAtHomeHub()
        {
            return RedirectPermanent("https://www.nhs.uk/conditions/coronavirus-covid-19/what-to-do-if-you-or-someone-you-live-with-has-coronavirus-symptoms/");
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
            state.Add("SYMPTOMS_STARTED_DAYS_AGO", "-1");

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