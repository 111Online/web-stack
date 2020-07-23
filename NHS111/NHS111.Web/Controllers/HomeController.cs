using AutoMapper;
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
        private readonly Presentation.Configuration.IConfiguration _configuration;

        public HomeController(IAuditLogger auditLogger, Presentation.Configuration.IConfiguration configuration)
        {
            _auditLogger = auditLogger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("{param}")]
        public ActionResult StartWithParam(JourneyViewModel model, string param)
        {
            switch (param.ToLower())
            {
                case "covid-19":
                    return StartCovidJourney(model);
                case "emergency-prescription":
                    return StartEmergencyPrescriptionJourney(model);
                case "location":
                    return StartLocationJourney(model);
                case "map":
                    return StartServiceMap();
                case "portsmouth":
                    return StartPortsmouthJourney(model, param);
                default:
                    return View("../Location/Home", model);
            }
        }

        public ActionResult StartServiceMap()
        {
            var model = new OutcomeMapViewModel()
            {
                MapsApiKey = _configuration.MapsApiKey
            };
            return View("~\\Views\\Shared\\_GoogleMap.cshtml", model);
        }

        [HttpGet]
        [Route("service/COVID-19")]
        //Special route for Covid direct link from other services to tidy up..
        public ActionResult StartCovidJourney(JourneyViewModel model)
        {
            return View("AboutCovid", model);
        }

        public ActionResult StartEmergencyPrescriptionJourney(JourneyViewModel model)
        {
            var locationModel = Mapper.Map<LocationViewModel>(model);
            locationModel.PathwayNo = "PW1827";
            return View("../Location/Location", locationModel);
        }

        public ActionResult StartLocationJourney(JourneyViewModel model)
        {
            var locationModel = Mapper.Map<LocationViewModel>(model);
            return View("../Location/Location", locationModel);
        }

        public ActionResult StartPortsmouthJourney(JourneyViewModel model, string param)
        {
            model.StartParameter = param;
            _auditLogger.LogEvent(model, EventType.CustomStart, param, string.Format("../Home/{0}", param)); 
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