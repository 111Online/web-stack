using Newtonsoft.Json;
using NHS111.Models.Models.Web;
using NHS111.Utils.Attributes;
using NHS111.Web.Helpers;
using NHS111.Web.Presentation.Builders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

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
            ModelState.Clear();
            var viewData = await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(model).ConfigureAwait(false);
            return View(viewData.Item1, viewData.Item2);
        }

        [HttpPost]
        public async Task<ActionResult> JustToBeSafeNext(JustToBeSafeViewModel model)
        {
            ModelState.Clear();
            var next = await _justToBeSafeViewModelBuilder.JustToBeSafeNextBuilder(model).ConfigureAwait(false);
            return View(next.Item1, next.Item2);
        }

        [HttpGet]
        [Route("Covid-19/SMS")]
        public async Task<ActionResult> SmsFirstQuestion(JustToBeSafeViewModel model)
        {
            var firstModel = BuildModel("PC111", model);
            return await JustToBeSafeFirst(firstModel).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{pathwayNumber}/{gender}/{age}/start")]
        public async Task<ActionResult> FirstQuestion(string pathwayNumber, string gender, int age, string args)
        {
            var model = BuildModel(pathwayNumber, gender, age, args);
            return await JustToBeSafeFirst(model).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("Question/First")]
        public async Task<ActionResult> FirstQuestionDeeplink(JustToBeSafeViewModel model)
        {
            ModelState.Clear();
            return await JustToBeSafeFirst(model).ConfigureAwait(false);
        }

        private static QuestionInfoViewModel BuildModel(string pathwayNumber, JustToBeSafeViewModel jtbsModel)
        {
            var userInfo = jtbsModel.UserInfo;
            var demogs = userInfo != null ? jtbsModel.UserInfo.Demography : null;
            var age = userInfo != null &&
                      demogs != null && demogs.Age > 0
                ? demogs.Age
                : 111;

            var gender = userInfo != null &&
                      demogs != null && !string.IsNullOrEmpty(demogs.Gender)
                ? demogs.Gender
                : "Male";

            var state = new Dictionary<string, string>();
            state.Add("PATIENT_GENDER", string.Format("\"{0}\"", gender.Substring(0, 1)));
            state.Add("SYMPTOMS_STARTED_DAYS_AGO", "-1");
            if (demogs != null && demogs.Age > 0)
                state.Add("PATIENT_AGE", age.ToString());
            else
                state.Add("PATIENT_AGE", "-1");


            var model = new QuestionInfoViewModel
            {
                SessionId = jtbsModel.SessionId,
                PathwayNo = pathwayNumber,
                EntrySearchTerm = string.Format("External get to {0}", pathwayNumber),
                State = state,
                StateJson = JsonConvert.SerializeObject(state),
                UserInfo = new UserInfo
                {
                    Demography = new AgeGenderViewModel
                    {
                        Age = age,
                        Gender = gender
                    }
                },
                CurrentPostcode = jtbsModel.CurrentPostcode
            };
            return model;
        }

        private static QuestionInfoViewModel BuildModel(string pathwayNumber, string gender, int age, string args)
        {
            var decryptedArgs = new QueryStringEncryptor(args);
            var decryptedFilterServices = !decryptedArgs.ContainsKey("filterServices") || string.IsNullOrEmpty(decryptedArgs["filterServices"]) ||
                                          bool.Parse(decryptedArgs["filterServices"]);

            var model = new QuestionInfoViewModel
            {
                SessionId = Guid.Parse(decryptedArgs["sessionId"]),
                PathwayNo = pathwayNumber,
                DigitalTitle = decryptedArgs["digitalTitle"],
                EntrySearchTerm = decryptedArgs["searchTerm"],
                CurrentPostcode = decryptedArgs["postcode"],
                UserInfo = new UserInfo
                {
                    Demography = new AgeGenderViewModel
                    {
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