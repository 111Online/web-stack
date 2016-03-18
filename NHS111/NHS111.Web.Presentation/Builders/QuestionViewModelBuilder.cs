using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    public class QuestionViewModelBuilder : IQuestionViewModelBuilder
    {
        private readonly IOutcomeViewModelBuilder _outcomeViewModelBuilder;
        private readonly IJustToBeSafeFirstViewModelBuilder _justToBeSafeFirstViewModelBuilder;
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;

        public QuestionViewModelBuilder(IOutcomeViewModelBuilder outcomeViewModelBuilder, IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder, IRestfulHelper restfulHelper,
            IConfiguration configuration, IMappingEngine mappingEngine)
        {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
            _restfulHelper = restfulHelper;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
        }

        public JourneyViewModel BuildGender(JourneyViewModel model)
        {
            return model;
        }

        public async Task<JourneyViewModel> BuildGender(string pathwayTitle)
        {
            // do we have a symptom that is in the white list
            // return a list of pathways numbers
            var pathwayNo = await _restfulHelper.GetAsync(string.Format(_configuration.BusinessApiPathwayNumbersUrl, pathwayTitle));
            return new JourneyViewModel() { PathwayNo = pathwayNo };
        }

        public async Task<JourneyViewModel> BuildSlider(JourneyViewModel model)
        {
            var pathway = JsonConvert.DeserializeObject<Pathway>(await _restfulHelper.GetAsync(string.Format(_configuration.BusinessApiPathwayIdUrl, model.PathwayNo, model.UserInfo.Gender, model.UserInfo.Age)));

            if (pathway == null) return null;

            model.PathwayId = pathway.Id;
            model.PathwayTitle = pathway.Title;
            model.PathwayNo = pathway.PathwayNo;
            model.State.Add("PATIENT_AGE", model.UserInfo.Age.ToString());
            model.State.Add("PATIENT_GENDER", string.Format("\"{0}\"", model.UserInfo.Gender.First().ToString().ToUpper()));
            model.State.Add("PATIENT_PARTY", "1");
            model.StateJson = JsonConvert.SerializeObject(model.State);
            return model;
        }

        public async Task<JourneyViewModel> BuildSlider(string pathwayTitle, string gender, int age)
        {
            var pathway = JsonConvert.DeserializeObject<Pathway>(await _restfulHelper.GetAsync(string.Format(_configuration.BusinessApiPathwayIdFromTitleUrl, pathwayTitle, gender, age)));

            if (pathway == null) return null;

            var model = new JourneyViewModel()
            {
                PathwayId = pathway.Id,
                PathwayTitle = pathway.Title,
                PathwayNo = pathway.PathwayNo,
                UserInfo = new UserInfo() { Age = age, Gender = gender },
                State = BuildState(gender, age),
                StateJson = BuildStateJson(gender, age)
            };
            
            return model;
        }

        //TO DO COPY ABOVE AND CREATE CONFIG

        public async Task<Tuple<string, JourneyViewModel>> BuildQuestion(JourneyViewModel model)
        {
            model.PreviousTitle = model.Title;
            model.PreviousStateJson = model.StateJson;
            var answer = JsonConvert.DeserializeObject<Answer>(model.SelectedAnswer);
            var journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson);
            journey.Steps.Add(new JourneyStep { QuestionNo = model.QuestionNo, QuestionTitle = model.Title, Answer = answer, QuestionId = model.Id });
            model.JourneyJson = JsonConvert.SerializeObject(journey);
            model.State = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.StateJson);

            var response = await _restfulHelper.GetAsync(string.Format(_configuration.BusinessApiNextNodeUrl, model.PathwayId,
                model.Id, answer.Title, HttpUtility.UrlEncode(JsonConvert.SerializeObject(model.State))));

            model = _mappingEngine.Map(response, model);
            model = _mappingEngine.Map(answer, model);

            return await ActionSelection(model);

        }

        public async Task<JourneyViewModel> BuildPreviousQuestion(JourneyViewModel model)
        {
            var journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson);
            var response = await _restfulHelper.GetAsync(string.Format(_configuration.BusinessApiQuestionByIdUrl, model.PathwayId, journey.Steps.Last().QuestionId));
            if (journey.Steps.Count == 1)
            {
                model.PreviousTitle = string.Empty;
                journey.Steps.RemoveAt(journey.Steps.Count - 1);
            }
            else
            {
                journey.Steps.RemoveAt(journey.Steps.Count - 1);
                model.PreviousTitle = journey.Steps.Last().IsJustToBeSafe == false ? journey.Steps.Last().QuestionTitle : string.Empty;
            }
    
            model.StateJson = model.PreviousStateJson;
            model.JourneyJson = JsonConvert.SerializeObject(journey);
            model.State = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.StateJson);
            return _mappingEngine.Map(response, model);
        }

        public async Task<string> BuildSearch(string input)
        {
            var response = await _restfulHelper.GetAsync(string.Format(_configuration.BusinessApiGroupedPathwaysUrl, input));
            var pathways = JsonConvert.DeserializeObject<List<GroupedPathways>>(response);
            return JsonConvert.SerializeObject(pathways.Select(pathway => new { label = pathway.Group, value = pathway.PathwayNumbers }));
        }

        public OutcomeViewModel AddressSearchViewBuilder(OutcomeViewModel model)
        {
            model.AddressSearchViewModel = new AddressSearchViewModel()
            {
                PostcodeApiAddress = _configuration.PostcodeSearchByIdApiUrl,
                PostcodeApiSubscriptionKey = _configuration.PostcodeSubscriptionKey
            };
            return model;
        }

        public async Task<Tuple<string, JourneyViewModel>> ActionSelection(JourneyViewModel model)
        {
            var nonOutcome = new[] { "Dx011", "Dx012", "Dx013", "Dx016", };

            if (nonOutcome.Contains(model.Id))
            {
                var newModel = _mappingEngine.Map<OutcomeViewModel>(model);
                return new Tuple<string, JourneyViewModel>("../Outcome/Emergency", await _outcomeViewModelBuilder.DispositionBuilder(newModel));
            }

            switch (model.NodeType)
            {
                case NodeType.Outcome:
                    {
                        var newModel = _mappingEngine.Map<OutcomeViewModel>(model);
                        newModel = AddressSearchViewBuilder(newModel);
                        newModel.CareAdviceMarkers = model.State.Keys.Where(key => key.StartsWith("Cx"));
                        var disposition2 = new[] { "Dx02", "Dx25", "Dx75", "Dx30", "Dx03", "Dx16", "Dx94", "Dx09" };
                        return disposition2.Contains(model.Id)
                            ? new Tuple<string, JourneyViewModel>("../Outcome/Disposition2", await _outcomeViewModelBuilder.DispositionBuilder(newModel))
                            : new Tuple<string, JourneyViewModel>("../Outcome/Disposition", await _outcomeViewModelBuilder.DispositionBuilder(newModel));
                    }

                case NodeType.Pathway:
                    {
                        var pathway = JsonConvert.DeserializeObject<Pathway>(await _restfulHelper.GetAsync(string.Format(_configuration.BusinessApiPathwayUrl, model.PathwayId)));
                        if (pathway == null) return null;

                        var derivedAge = model.UserInfo.Age == -1 ? pathway.MinimumAgeInclusive : model.UserInfo.Age;
                        var newModel = new JustToBeSafeViewModel
                        {
                            PathwayId = pathway.Id,
                            PathwayNo = pathway.PathwayNo,
                            PathwayTitle = pathway.Title,
                            UserInfo = new UserInfo() { Age = derivedAge, Gender = pathway.Gender },
                            JourneyJson = model.JourneyJson,
                            SymptomDiscriminator = model.SymptomDiscriminator,
                            State = BuildState(pathway.Gender, derivedAge),
                            StateJson = BuildStateJson(pathway.Gender, derivedAge)
                        };
                        return await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(newModel);
                    }
                case NodeType.DeadEndJump:
                    return new Tuple<string, JourneyViewModel>("../Question/DeadEndJump", model);
                case NodeType.Question:
                default:
                    return new Tuple<string, JourneyViewModel>("../Question/Question", model);

            }
        }

        private static IDictionary<string, string> BuildState(string gender, int age)
        {
            return new Dictionary<string, string>()
            {
                {"PATIENT_AGE", age.ToString()},
                {"PATIENT_GENDER", string.Format("\"{0}\"", gender.ToUpper())},
                {"PATIENT_PARTY", "1"}
            };
        }

        private static string BuildStateJson(string gender, int age)
        {
            return JsonConvert.SerializeObject(BuildState(gender, age));
        }
    }

    public interface IQuestionViewModelBuilder
    {
        JourneyViewModel BuildGender(JourneyViewModel model);
        Task<JourneyViewModel> BuildGender(string pathwayTitle);
        Task<JourneyViewModel> BuildSlider(JourneyViewModel model);
        Task<JourneyViewModel> BuildSlider(string pathwayTitle, string gender, int age);
        Task<Tuple<string, JourneyViewModel>> BuildQuestion(JourneyViewModel model);
        Task<JourneyViewModel> BuildPreviousQuestion(JourneyViewModel model);
        Task<string> BuildSearch(string input);
        Task<Tuple<string, JourneyViewModel>> ActionSelection(JourneyViewModel model);
    }
}