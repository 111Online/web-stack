
namespace NHS111.Web.Controllers {
    using Features;
    using Helpers;
    using RestSharp;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models.Models.Web;
    using Models.Models.Web.Enums;
    using Utils.Attributes;
    using Presentation.Builders;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using Models.Models.Business.PathwaySearch;
    using Models.Models.Domain;
    using Newtonsoft.Json;
    using Presentation.Logging;
    using Presentation.ModelBinders;
    using Utils.Filters;
    using IConfiguration = Presentation.Configuration.IConfiguration;

    [LogHandleErrorForMVC]
    public class QuestionController
        : Controller {

        public const int MAX_SEARCH_RESULTS = 10;

        public QuestionController(IJourneyViewModelBuilder journeyViewModelBuilder,
            IConfiguration configuration, IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder, IDirectLinkingFeature directLinkingFeature,
            IAuditLogger auditLogger, IUserZoomDataBuilder userZoomDataBuilder, IRestClient restClientBusinessApi, IViewRouter viewRouter, IPostcodePrefillFeature postcodePrefillFeature) {
            _journeyViewModelBuilder = journeyViewModelBuilder;
            _configuration = configuration;
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
            _directLinkingFeature = directLinkingFeature;
            _auditLogger = auditLogger;
            _userZoomDataBuilder = userZoomDataBuilder;
            _restClientBusinessApi = restClientBusinessApi;
            _viewRouter = viewRouter;
            _postcodePrefillFeature = postcodePrefillFeature;
        }

        [HttpGet]
        public ActionResult Home(bool filterServices = true)
        {
            var startOfJourney = new JourneyViewModel
            {
                SessionId = Guid.Parse(Request.AnonymousID),
                FilterServices = filterServices
            };

            _userZoomDataBuilder.SetFieldsForHome(startOfJourney);
            return View("Home", startOfJourney);
        }

        [HttpPost]
        public  ActionResult Home(JourneyViewModel model)
        {
            _userZoomDataBuilder.SetFieldsForInitialQuestion(model);
            return View("InitialQuestion", model);
        }

        [HttpPost]
        public async Task<ActionResult> Search(JourneyViewModel model)
        {
            if (!ModelState.IsValidField("UserInfo.Demography.Gender") || !ModelState.IsValidField("UserInfo.Demography.Age"))
            {
                //var genderModel = new JourneyViewModel {UserInfo = new UserInfo {Demography = model}};
                _userZoomDataBuilder.SetFieldsForDemographics(model);
                return View("Gender", model);
            }

            var topicsContainingStartingPathways = await GetAllTopics(model.UserInfo.Demography);

            var startOfJourney = new SearchJourneyViewModel
            {
                UserInfo = new UserInfo { Demography = model.UserInfo.Demography },
                AllTopics = topicsContainingStartingPathways,
                FilterServices = model.FilterServices
            };

            _userZoomDataBuilder.SetFieldsForSearch(startOfJourney);

            return View(startOfJourney);

        }

        [HttpPost]
        public async Task<ActionResult> SearchResults(string q, string gender, int age, bool filterServices) {
            var ageGroup = new AgeCategory(age);

            var ageGenderViewModel = new AgeGenderViewModel {Gender = gender, Age = age};
            var topicsContainingStartingPathways = await GetAllTopics(ageGenderViewModel);
            var model = new SearchJourneyViewModel {
                SanitisedSearchTerm = q.Trim(),
                UserInfo = new UserInfo {Demography = ageGenderViewModel},
                AllTopics = topicsContainingStartingPathways,
                EntrySearchTerm = q,
                FilterServices = filterServices
            };

            _userZoomDataBuilder.SetFieldsForSearchResults(model);

            if (string.IsNullOrEmpty(q))
                return View("search", model);

            var requestPath = _configuration.GetBusinessApiPathwaySearchUrl(gender, ageGroup.Value, true);

            var request = new RestRequest(requestPath, Method.POST);
            request.AddJsonBody(Uri.EscapeDataString(model.SanitisedSearchTerm));

            var response = await _restClientBusinessApi.ExecuteTaskAsync<List<SearchResultViewModel>>(request);

            model.Results = response.Data
                .Take(MAX_SEARCH_RESULTS)
                 .Select(r => Transform(r, model.SanitisedSearchTerm));
            return View("search", model);
        }

        private SearchResultViewModel Transform(SearchResultViewModel result, string searchTerm) {
            result.Description += ".";
            result.Description = result.Description.Replace("\\n\\n", ". ");
            result.Description = result.Description.Replace(" . ", ". ");
            result.Description = result.Description.Replace("..", ".");

            SortTitlesByRelevancy(result, searchTerm);

            return result;
        }

        private void SortTitlesByRelevancy(SearchResultViewModel result, string searchTerm) {
            if (result.DisplayTitle == null)
                return;
            var lowerTerm = searchTerm.ToLower();
            for (var i = 0; i < result.DisplayTitle.Count; i++) {
                var title = result.DisplayTitle[i];
                if (!PathwaySearchResult.StripHighlightMarkup(title).ToLower().Contains(lowerTerm))
                    continue;
                result.DisplayTitle.RemoveAt(i);
                result.DisplayTitle.Insert(0, title);
            }
        }


        [HttpPost]
        public async Task<JsonResult> AutosuggestPathways(string input, string gender, int age)
        {
 
            var response = await _restClientBusinessApi.ExecuteTaskAsync<List<GroupedPathways>>(
                     new RestRequest(_configuration.GetBusinessApiGroupedPathwaysUrl(input, gender, age, true), Method.GET));

            return Json(await Search(response.Data));
        }

        private async Task<string> Search(List<GroupedPathways> pathways)
        {
            
            return
                JsonConvert.SerializeObject(
                    pathways.Select(pathway => new {label = pathway.Group, value = pathway.PathwayNumbers}));
        }

        [HttpPost]
        public ActionResult Gender(JourneyViewModel model) {
            return View(model);
        }


        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "Question")]
        public async Task<ActionResult> Question(JourneyViewModel model) {
            ModelState.Clear();
            var nextModel = await GetNextJourneyViewModel(model);

            var viewName = _viewRouter.GetViewName(nextModel, ControllerContext);
           
            return View(viewName, nextModel);
        }

        [HttpGet]
        public async Task<ActionResult> InitialQuestion()
        {
            var model = new JourneyViewModel();
            var audit = model.ToAuditEntry(new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session));
            audit.EventData = "User directed from duplicate submission page";
            await _auditLogger.Log(audit);

            model.UserInfo = new UserInfo();
            _userZoomDataBuilder.SetFieldsForDemographics(model);
            return View("Gender", model);
        }

        [HttpPost]
        public async Task<ActionResult> InitialQuestion(JourneyViewModel model)
        {
            var audit = model.ToAuditEntry(new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session));
            audit.EventData = "User accepted module zero.";
            await _auditLogger.Log(audit);

            ModelState.Clear();
            model.UserInfo = new UserInfo();

            _userZoomDataBuilder.SetFieldsForDemographics(model);
            return View("Gender", model);
        }

        [HttpPost]
        public async Task<JsonResult> PathwaySearch(string gender, int age, string searchTerm) {
            var ageGroup = new AgeCategory(age);
            var response =
                await
                    _restClientBusinessApi.ExecuteTaskAsync(
                        new RestRequest(_configuration.GetBusinessApiPathwaySearchUrl(gender, ageGroup.Value, true),
                            Method.GET));

            return Json(response.Content);
        }


        private async Task<JourneyViewModel> GetNextJourneyViewModel(JourneyViewModel model) {
            var nextNode = await GetNextNode(model);
            return await _journeyViewModelBuilder.Build(model, nextNode);
        }

        private async Task<IEnumerable<CategoryWithPathways>> GetAllTopics(AgeGenderViewModel model)
        {
            var url = _configuration.GetBusinessApiGetCategoriesWithPathwaysGenderAge(model.Gender,
                model.Age, true);
            var response = await
                _restClientBusinessApi.ExecuteTaskAsync<List<CategoryWithPathways>>(CreateJsonRequest(url, Method.GET));


            var allTopics = response.Data;
            var topicsContainingStartingPathways =
                allTopics.Where(
                    c =>
                        c.Pathways.Any(p => p.Pathway.StartingPathway) ||
                        c.SubCategories.Any(sc => sc.Pathways.Any(p => p.Pathway.StartingPathway)));
            return topicsContainingStartingPathways;
        }

        [HttpGet]
        [Route("question/direct/{pathwayId}/{age?}/{pathwayTitle}/{answers?}")]
        public async Task<ActionResult> Direct(string pathwayId, int? age, string pathwayTitle,
            [ModelBinder(typeof (IntArrayModelBinder))] int[] answers, bool? filterServices) {

            if (!_directLinkingFeature.IsEnabled) {
                return HttpNotFound();
            }

            var resultingModel = await DeriveJourneyView(pathwayId, age, pathwayTitle, answers);
            if (resultingModel != null) {
                resultingModel.FilterServices = filterServices.HasValue ? filterServices.Value : true;

                if (resultingModel.NodeType == NodeType.Outcome) {
                    var outcomeModel = resultingModel as OutcomeViewModel;

                    bool shouldPrefillPostcode = _postcodePrefillFeature.IsEnabled &&
                                                 OutcomeGroup.DosSearchOutcomesGroups.Contains(outcomeModel.OutcomeGroup) &&
                                                 _postcodePrefillFeature.RequestIncludesPostcode(Request);
                    if (shouldPrefillPostcode) {
                        resultingModel.UserInfo.CurrentAddress.Postcode = _postcodePrefillFeature.GetPostcode(Request);
                        outcomeModel.CurrentView = _viewRouter.GetViewName(resultingModel, ControllerContext);
                        //defaulting the label to 'services' because this is normally handled by the specific outcome view
                        if (outcomeModel.OutcomeGroup.IsPostcodeFirst()) {
                            var controller = DependencyResolver.Current.GetService<PostcodeFirstController>();
                            controller.ControllerContext = new ControllerContext(ControllerContext.RequestContext,
                                controller);
                            return await controller.Outcome(outcomeModel, null, null);
                        }
                        else {
                            var controller = DependencyResolver.Current.GetService<OutcomeController>();
                            controller.ControllerContext = new ControllerContext(ControllerContext.RequestContext,
                                controller);
                            if (outcomeModel.OutcomeGroup.SearchDestination == "ServiceDetails")
                                return await controller.ServiceDetails(outcomeModel, null);
                            if (outcomeModel.OutcomeGroup.SearchDestination == "ServiceList")
                                return await controller.ServiceList(outcomeModel, null, null);
                        }
                    }
                }
            }

            var viewName = _viewRouter.GetViewName(resultingModel, ControllerContext);
            return View(viewName, resultingModel);
        }

        [HttpGet]
        [Route("question/outcomedetail/{pathwayId}/{age?}/{pathwayTitle}/{answers?}")]
        public async Task<ActionResult> OutcomeDetail(string pathwayId, int? age, string pathwayTitle,
            [ModelBinder(typeof(IntArrayModelBinder))] int[] answers, bool? filterServices)
        {

            var journeyViewModel = await DeriveJourneyView(pathwayId, age, pathwayTitle, answers);
            if(journeyViewModel != null)
                journeyViewModel.FilterServices = filterServices.HasValue ? filterServices.Value : true;

            var viewName = _viewRouter.GetViewName(journeyViewModel, ControllerContext);
            if (journeyViewModel.OutcomeGroup == null ||
                !OutcomeGroup.SignpostingOutcomesGroups.Contains(journeyViewModel.OutcomeGroup))
            {
                return HttpNotFound();
            }
            journeyViewModel.DisplayOutcomeReferenceOnly = true;
            return View(viewName, journeyViewModel);
        }

        private async Task<JourneyViewModel> DeriveJourneyView(string pathwayId, int? age, string pathwayTitle, int[] answers)
        {
            var journeyViewModel = BuildJourneyViewModel(pathwayId, age, pathwayTitle);
            var response = await 
                _restClientBusinessApi.ExecuteTaskAsync<Pathway>(
                    CreateJsonRequest(_configuration.GetBusinessApiPathwayUrl(pathwayId, true), Method.GET));
            var pathway = response.Data;
            if (pathway == null) return null;

            var derivedAge = journeyViewModel.UserInfo.Demography.Age == -1 ? pathway.MinimumAgeInclusive : journeyViewModel.UserInfo.Demography.Age;
            var newModel = new JustToBeSafeViewModel
            {
                PathwayId = pathway.Id,
                PathwayNo = pathway.PathwayNo,
                PathwayTitle = pathway.Title,
                DigitalTitle = string.IsNullOrEmpty(journeyViewModel.DigitalTitle) ? pathway.Title : journeyViewModel.DigitalTitle,
                UserInfo = new UserInfo() { Demography = new AgeGenderViewModel { Age = derivedAge, Gender = pathway.Gender } },
                JourneyJson = journeyViewModel.JourneyJson,
                SymptomDiscriminatorCode = journeyViewModel.SymptomDiscriminatorCode,
                State = JourneyViewModelStateBuilder.BuildState(pathway.Gender, derivedAge),
            };

            newModel.StateJson = JourneyViewModelStateBuilder.BuildStateJson(newModel.State);
            journeyViewModel = (await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(newModel)).Item2; //todo refactor tuple away

            var resultingModel = await AnswerQuestions(journeyViewModel, answers);
            return resultingModel;
        }

        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "PreviousQuestion")]
        public async Task<ActionResult> PreviousQuestion(JourneyViewModel model) {
            ModelState.Clear();

            var url = _configuration.GetBusinessApiQuestionByIdUrl(model.PathwayId, model.Journey.Steps.Last().QuestionId, true);
            var response = await _restClientBusinessApi.ExecuteTaskAsync<QuestionWithAnswers>(CreateJsonRequest(url, Method.GET));
            var questionWithAnswers = response.Data;

            var result = _journeyViewModelBuilder.BuildPreviousQuestion(questionWithAnswers, model);
            var viewName = _viewRouter.GetViewName(result, ControllerContext);

            return View(viewName, result);
        }

        private async Task<QuestionWithAnswers> GetNextNode(JourneyViewModel model) {
            var answer = JsonConvert.DeserializeObject<Answer>(model.SelectedAnswer);
            var serialisedState = HttpUtility.UrlEncode(model.StateJson);
            var request = CreateJsonRequest(_configuration.GetBusinessApiNextNodeUrl(model.PathwayId, model.Id, serialisedState, true), Method.POST);
            request.AddJsonBody(answer.Title);
            var response = await _restClientBusinessApi.ExecuteTaskAsync<QuestionWithAnswers>(request);
            return response.Data;
        }

        private async Task<JourneyViewModel> AnswerQuestions(JourneyViewModel model, int[] answers) {
            if (answers == null)
                return null;

            var queue = new Queue<int>(answers);
            while (queue.Any()) {
                var answer = queue.Dequeue();
                model = await AnswerQuestion(model, answer);
            }
            return model;
        }

        private async Task<JourneyViewModel> AnswerQuestion(JourneyViewModel model, int answer) {
            if (answer < 0 || answer >= model.Answers.Count)
                throw new ArgumentOutOfRangeException(
                    string.Format("The answer index '{0}' was not found within the range of answers: {1}", answer,
                        string.Join(", ", model.Answers.Select(a => a.Title))));

            model.SelectedAnswer = JsonConvert.SerializeObject(model.Answers.First(a => a.Order == answer + 1));
            var result = (ViewResult) await Question(model);

            return result.Model is OutcomeViewModel ? (OutcomeViewModel)result.Model : (JourneyViewModel)result.Model;
        }

        private static JourneyViewModel BuildJourneyViewModel(string pathwayId, int? age, string pathwayTitle) {
            return new JourneyViewModel {
                NodeType = NodeType.Pathway,
                PathwayId = pathwayId,
                PathwayTitle = pathwayTitle,
                UserInfo = new UserInfo { Demography = new AgeGenderViewModel { Age = age ?? -1 }}
            };
        }

        private RestRequest CreateJsonRequest(string url, Method method)
        {
            var request =  new RestRequest(url, method);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            return request;
        }

        private readonly IJourneyViewModelBuilder _journeyViewModelBuilder;
        private readonly IConfiguration _configuration;
        private readonly IJustToBeSafeFirstViewModelBuilder _justToBeSafeFirstViewModelBuilder;
        private readonly IDirectLinkingFeature _directLinkingFeature;
        private readonly IAuditLogger _auditLogger;
        private readonly IUserZoomDataBuilder _userZoomDataBuilder;
        private readonly IRestClient _restClientBusinessApi;
        private readonly IViewRouter _viewRouter;
        private readonly IPostcodePrefillFeature _postcodePrefillFeature;
    }
}
