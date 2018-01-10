using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using StructureMap.Query;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    public class JustToBeSafeFirstViewModelBuilder : IJustToBeSafeFirstViewModelBuilder
    {
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;
        private readonly IRestfulHelper _restfulHelper;
        private readonly IKeywordCollector _keywordCollector;
        private readonly IUserZoomDataBuilder _userZoomDataBuilder;

        public JustToBeSafeFirstViewModelBuilder(IRestfulHelper restfulHelper, IConfiguration configuration, IMappingEngine mappingEngine, IKeywordCollector keywordCollector, IUserZoomDataBuilder userZoomDataBuilder)
        {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
            _keywordCollector = keywordCollector;
            _userZoomDataBuilder = userZoomDataBuilder;
        }

        public async Task<Tuple<string, QuestionViewModel>> JustToBeSafeFirstBuilder(JustToBeSafeViewModel model) {

            if (model.PathwayId != null)
                model = await DoWorkPreviouslyDoneInQuestionBuilder(model); //todo refactor away

            var identifiedModel = await BuildIdentifiedModel(model);
            var questionsJson = await _restfulHelper.GetAsync(_configuration.GetBusinessApiJustToBeSafePartOneUrl(identifiedModel.PathwayId));
            var questionsWithAnswers = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(questionsJson);
            if (!questionsWithAnswers.Any())
            {
                var questionViewModel = new QuestionViewModel
                {
                    PathwayId = identifiedModel.PathwayId,
                    PathwayNo = identifiedModel.PathwayNo,
                    PathwayTitle = identifiedModel.PathwayTitle,
                    DigitalTitle = string.IsNullOrEmpty(identifiedModel.DigitalTitle) ? identifiedModel.PathwayTitle : identifiedModel.DigitalTitle,
                    UserInfo = identifiedModel.UserInfo,
                    JourneyJson = identifiedModel.JourneyJson,
                    State = JsonConvert.DeserializeObject<Dictionary<string, string>>(identifiedModel.StateJson),
                    StateJson = identifiedModel.StateJson,
                    CollectedKeywords = identifiedModel.CollectedKeywords,
                    Journey = JsonConvert.DeserializeObject<Journey>(identifiedModel.JourneyJson),
                    SessionId = model.SessionId,
                    JourneyId = Guid.NewGuid(),
                    FilterServices = model.FilterServices,
                    Campaign = model.Campaign,
                    Source = model.Source
                };
                var question = JsonConvert.DeserializeObject<QuestionWithAnswers>(await _restfulHelper.GetAsync(_configuration.GetBusinessApiFirstQuestionUrl(identifiedModel.PathwayId, identifiedModel.StateJson)));
                _mappingEngine.Mapper.Map(question, questionViewModel);

                _userZoomDataBuilder.SetFieldsForQuestion(questionViewModel);

                return new Tuple<string, QuestionViewModel>("../Question/Question", questionViewModel);
            }
            identifiedModel.Part = 1;
            identifiedModel.JourneyId = Guid.NewGuid();
            identifiedModel.Questions = questionsWithAnswers;
            identifiedModel.QuestionsJson = questionsJson;
            identifiedModel.JourneyJson = string.IsNullOrEmpty(identifiedModel.JourneyJson) ? JsonConvert.SerializeObject(new Journey()) : identifiedModel.JourneyJson;
            identifiedModel.FilterServices = model.FilterServices;
            return new Tuple<string, QuestionViewModel>("../JustToBeSafe/JustToBeSafe", identifiedModel);

        }

        private async Task<JustToBeSafeViewModel> DoWorkPreviouslyDoneInQuestionBuilder(JustToBeSafeViewModel model) {
            var businessApiPathwayUrl = _configuration.GetBusinessApiPathwayUrl(model.PathwayId);
            var response = await _restfulHelper.GetAsync(businessApiPathwayUrl);
            var pathway = JsonConvert.DeserializeObject<Pathway>(response);
            if (pathway == null) return null;

            var derivedAge = model.UserInfo.Demography.Age == -1 ? pathway.MinimumAgeInclusive : model.UserInfo.Demography.Age;
            var newModel = new JustToBeSafeViewModel
            {
                PathwayId = pathway.Id,
                PathwayNo = pathway.PathwayNo,
                PathwayTitle = pathway.Title,
                DigitalTitle = string.IsNullOrEmpty(model.DigitalTitle) ? pathway.Title : model.DigitalTitle,
                UserInfo = new UserInfo { Demography = new AgeGenderViewModel { Age = derivedAge, Gender = pathway.Gender } },
                JourneyJson = model.JourneyJson,
                SymptomDiscriminatorCode = model.SymptomDiscriminatorCode,
                State = JourneyViewModelStateBuilder.BuildState(pathway.Gender, derivedAge),
                SessionId = model.SessionId,
                FilterServices = model.FilterServices,
                Campaign = model.Campaign,
                Source = model.Source
            };

            newModel.StateJson = JourneyViewModelStateBuilder.BuildStateJson(newModel.State);

            return newModel;
        }

        private async Task<JustToBeSafeViewModel> BuildIdentifiedModel(JustToBeSafeViewModel model)
        {
            var pathway = JsonConvert.DeserializeObject<Pathway>(await _restfulHelper.GetAsync(_configuration.GetBusinessApiPathwayIdUrl(model.PathwayNo, model.UserInfo.Demography.Gender, model.UserInfo.Demography.Age)));

            if (pathway == null) return null;

            model.PathwayId = pathway.Id;
            model.PathwayTitle = pathway.Title;
            model.PathwayNo = pathway.PathwayNo;
            model.State = JourneyViewModelStateBuilder.BuildState(model.UserInfo.Demography.Gender,model.UserInfo.Demography.Age, model.State);
            model.StateJson = JourneyViewModelStateBuilder.BuildStateJson(model.State);
            model.CollectedKeywords = new KeywordBag(_keywordCollector.ParseKeywords(pathway.Keywords, false).ToList(), _keywordCollector.ParseKeywords(pathway.ExcludeKeywords, false).ToList());
            return model;
        }
    }

    public interface IJustToBeSafeFirstViewModelBuilder
    {
        Task<Tuple<string, QuestionViewModel>> JustToBeSafeFirstBuilder(JustToBeSafeViewModel model);
    }
}