using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Parser;
using NHS111.Utils.RestTools;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NHS111.Web.Presentation.Logging;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    public class JustToBeSafeFirstViewModelBuilder : BaseBuilder, IJustToBeSafeFirstViewModelBuilder
    {
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;
        private readonly ILoggingRestClient _restClient;
        private readonly IKeywordCollector _keywordCollector;
        private readonly IUserZoomDataBuilder _userZoomDataBuilder;
        private readonly IAuditLogger _auditLogger;

        public JustToBeSafeFirstViewModelBuilder(ILoggingRestClient restClient, IConfiguration configuration, IMappingEngine mappingEngine, IKeywordCollector keywordCollector, IUserZoomDataBuilder userZoomDataBuilder, IAuditLogger auditLogger)
        {
            _restClient = restClient;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
            _keywordCollector = keywordCollector;
            _userZoomDataBuilder = userZoomDataBuilder;
            _auditLogger = auditLogger;
        }

        public async Task<Tuple<string, QuestionViewModel>> JustToBeSafeFirstBuilder(JustToBeSafeViewModel model)
        {

            if (model.PathwayId != null)
                model = await DoWorkPreviouslyDoneInQuestionBuilder(model); //todo refactor away

            var identifiedModel = await BuildIdentifiedModel(model);
            var questionsWithAnswers = await _restClient.ExecuteAsync<IEnumerable<QuestionWithAnswers>>(new JsonRestRequest(_configuration.GetBusinessApiJustToBeSafePartOneUrl(identifiedModel.PathwayId), Method.GET));

            CheckResponse(questionsWithAnswers);

            if (!questionsWithAnswers.Data.Any())
            {
                var questionViewModel = new QuestionViewModel
                {
                    PathwayId = identifiedModel.PathwayId,
                    PathwayNo = identifiedModel.PathwayNo,
                    PathwayTitle = identifiedModel.PathwayTitle,
                    PathwayTraumaType = identifiedModel.PathwayTraumaType,
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
                    Source = model.Source,
                    CurrentPostcode = model.CurrentPostcode,
                    EntrySearchTerm = model.EntrySearchTerm,
                    ViaGuidedSelection = model.ViaGuidedSelection,
                    StartParameter = model.StartParameter
                };

                var question = await _restClient.ExecuteAsync<QuestionWithAnswers>(new JsonRestRequest(_configuration.GetBusinessApiFirstQuestionUrl(identifiedModel.PathwayId, identifiedModel.StateJson), Method.GET));

                CheckResponse(question);

                _mappingEngine.Mapper.Map(question.Data, questionViewModel);

                if(questionViewModel.IsViaGuidedSelection)
                    _auditLogger.LogEvent(model, EventType.GuidedSelection,questionViewModel.PathwayTitle,"/GuidedSelection");


                _userZoomDataBuilder.SetFieldsForQuestion(questionViewModel);
                if (questionViewModel.NodeType == NodeType.Page)
                {
                    // This replicates logic in ViewDeterminer so in future should ideally use that instead.
                    string viewName = "../Question/Page";
                    if (questionViewModel.PathwayNo.Equals("PC111")) viewName = "../Question/Custom/NHSUKPage";
                    //replace placeholder for covid specific logic. 
                    var covidPathways = await GetCovidPathways(model.UserInfo.Demography.Gender, new AgeCategory(model.UserInfo.Demography.Age));
                    questionViewModel.Content = PageCustomContent.ReplaceCovidPlaceHolderInPageContent(questionViewModel, covidPathways);
                    return new Tuple<string, QuestionViewModel>(viewName, questionViewModel);
                }

                return new Tuple<string, QuestionViewModel>("../Question/Question", questionViewModel);
            }
            identifiedModel.Part = 1;
            identifiedModel.JourneyId = Guid.NewGuid();
            identifiedModel.Questions = questionsWithAnswers.Data.ToList();
            identifiedModel.QuestionsJson = JsonConvert.SerializeObject(questionsWithAnswers.Data);
            identifiedModel.JourneyJson = string.IsNullOrEmpty(identifiedModel.JourneyJson) ? JsonConvert.SerializeObject(new Journey()) : identifiedModel.JourneyJson;
            identifiedModel.FilterServices = model.FilterServices;
            return new Tuple<string, QuestionViewModel>("../JustToBeSafe/JustToBeSafe", identifiedModel);

        }

        private async Task<IEnumerable<string>> GetCovidPathways(string gender, AgeCategory ageGroup)
        {
            var requestPath = _configuration.GetBusinessApiGuidedPathwaySearchUrl(gender, ageGroup.Value, true);
            var request = new RestRequest(requestPath, Method.POST);
            request.AddJsonBody(new { query = Uri.EscapeDataString(SearchReservedCovidTerms.SearchTerms.First()) });

            var response = await _restClient.ExecuteAsync<List<GuidedSearchResultViewModel>>(request).ConfigureAwait(false);
            CheckResponse(response);
            return response.Data.Select(p => p.PathwayNo);
        }

        private async Task<JustToBeSafeViewModel> DoWorkPreviouslyDoneInQuestionBuilder(JustToBeSafeViewModel model)
        {
            var businessApiPathwayUrl = _configuration.GetBusinessApiPathwayUrl(model.PathwayId);
            var response = await _restClient.ExecuteAsync<Pathway>(new JsonRestRequest(businessApiPathwayUrl, Method.GET));

            CheckResponse(response);

            if (response.Data == null) return null;

            var pathway = response.Data;
            var derivedAge = model.UserInfo.Demography.Age == -1 ? pathway.MinimumAgeInclusive : model.UserInfo.Demography.Age;
            var state = !string.IsNullOrEmpty(model.StateJson)
                ? JsonConvert.DeserializeObject<Dictionary<string, string>>(model.StateJson)
                : new Dictionary<string, string>();

            var newModel = new JustToBeSafeViewModel
            {
                PathwayId = pathway.Id,
                PathwayNo = pathway.PathwayNo,
                PathwayTitle = pathway.Title,
                PathwayTraumaType = pathway.TraumaType,
                DigitalTitle = string.IsNullOrEmpty(model.DigitalTitle) ? pathway.Title : model.DigitalTitle,
                UserInfo = new UserInfo { Demography = new AgeGenderViewModel { Age = derivedAge, Gender = pathway.Gender } },
                JourneyJson = model.JourneyJson,
                SymptomDiscriminatorCode = model.SymptomDiscriminatorCode,
                State = JourneyViewModelStateBuilder.BuildState(pathway.Gender, derivedAge, state),
                SessionId = model.SessionId,
                Campaign = model.Campaign,
                Source = model.Source,
                FilterServices = model.FilterServices,
                StartParameter = model.StartParameter
            };

            newModel.StateJson = JourneyViewModelStateBuilder.BuildStateJson(newModel.State);

            return newModel;
        }

        private async Task<JustToBeSafeViewModel> BuildIdentifiedModel(JustToBeSafeViewModel model)
        {
            var response = await _restClient.ExecuteAsync<Pathway>(new JsonRestRequest(_configuration.GetBusinessApiPathwayIdUrl(model.PathwayNo, model.UserInfo.Demography.Gender, model.UserInfo.Demography.Age), Method.GET));

            CheckResponse(response);

            if (response.Data == null) return null;

            var pathway = response.Data;
            model.PathwayId = pathway.Id;
            model.PathwayTitle = pathway.Title;
            model.PathwayNo = pathway.PathwayNo;
            model.PathwayTraumaType = pathway.TraumaType;
            model.State = JourneyViewModelStateBuilder.BuildState(model.UserInfo.Demography.Gender, model.UserInfo.Demography.Age, model.State);
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