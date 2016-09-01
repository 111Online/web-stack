using System;
using NHS111.Utils.Helpers;

namespace NHS111.Web.Presentation.Builders
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Domain;
    using NHS111.Models.Models.Web;
    using NHS111.Models.Models.Web.Enums;
    using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

    public class JourneyViewModelBuilder
        : IJourneyViewModelBuilder
    {

        public JourneyViewModelBuilder(IOutcomeViewModelBuilder outcomeViewModelBuilder, IMappingEngine mappingEngine,
            ISymptomDiscriminatorCollector symptomDiscriminatorCollector, IKeywordCollector keywordCollector,
            IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder, IConfiguration configuration)
        {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _mappingEngine = mappingEngine;
            _symptomDiscriminatorCollector = symptomDiscriminatorCollector;
            _keywordCollector = keywordCollector;
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
            _configuration = configuration;
        }

        public async Task<JourneyViewModel> Build(JourneyViewModel model, QuestionWithAnswers nextNode)
        {

            model.ProgressState();

            AddLastStepToJourney(model);

            var answer = JsonConvert.DeserializeObject<Answer>(model.SelectedAnswer);

            _symptomDiscriminatorCollector.Collect(nextNode, model);
            model = _keywordCollector.Collect(answer, model);

            if (!string.IsNullOrEmpty(model.Id))
            {
                model.SymptomGroup = await GetSymptomGroup(ConvertQuestionIdToPathwayId(model.Id));
            }

            model = _mappingEngine.Mapper.Map(nextNode, model);

            switch (model.NodeType)
            {
                case NodeType.Outcome:
                    var outcome = _mappingEngine.Mapper.Map<OutcomeViewModel>(model);
                    return await _outcomeViewModelBuilder.DispositionBuilder(outcome);
                case NodeType.Pathway:
                    var jtbs = _mappingEngine.Mapper.Map<JustToBeSafeViewModel>(model);
                    return (await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(jtbs)).Item2; //todo refactor tuple away
            }

            return model;
        }

        private static void AddLastStepToJourney(JourneyViewModel model)
        {
            model.Journey.Steps.Add(model.ToStep());
            model.JourneyJson = JsonConvert.SerializeObject(model.Journey);
        }

        public JourneyViewModel BuildPreviousQuestion(QuestionWithAnswers lastStep, JourneyViewModel model)
        {

            model.RemoveLastStep();

            model.CollectedKeywords = _keywordCollector.CollectKeywordsFromPreviousQuestion(model.CollectedKeywords,
                model.Journey.Steps);

            return _mappingEngine.Mapper.Map(lastStep, model);
        }

        private static string ConvertQuestionIdToPathwayId(string questionId)
        {
            var array = questionId.Split('.');
            return array.Length > 0 ? array[0] : string.Empty;
        }

        private async Task<string> GetSymptomGroup(string pathway)
        {
            RestfulHelper restfulHelper = new RestfulHelper();

            var symptomGroupResponse = await
                restfulHelper.GetResponseAsync(string.Format(_configuration.GetBusinessApiPathwaySymptomGroupUrl(pathway)));
            if (!symptomGroupResponse.IsSuccessStatusCode)
                throw new Exception(string.Format("A problem occured getting the symptom group for {0}.", pathway));

            return
                await symptomGroupResponse.Content.ReadAsStringAsync();
        }

        private readonly IOutcomeViewModelBuilder _outcomeViewModelBuilder;
        private readonly IMappingEngine _mappingEngine;
        private readonly ISymptomDiscriminatorCollector _symptomDiscriminatorCollector;
        private readonly IKeywordCollector _keywordCollector;
        private readonly IJustToBeSafeFirstViewModelBuilder _justToBeSafeFirstViewModelBuilder;
        private readonly IConfiguration _configuration;
    }

    public interface IJourneyViewModelBuilder
    {
        Task<JourneyViewModel> Build(JourneyViewModel model, QuestionWithAnswers nextNode);
        JourneyViewModel BuildPreviousQuestion(QuestionWithAnswers lastStep, JourneyViewModel model);
    }
}