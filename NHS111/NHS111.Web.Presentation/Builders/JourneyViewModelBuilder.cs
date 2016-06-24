namespace NHS111.Web.Presentation.Builders {
    using System.Threading.Tasks;
    using AutoMapper;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Domain;
    using NHS111.Models.Models.Web;
    using NHS111.Models.Models.Web.Enums;
    using NHS111.Models.Models.Web.FromExternalServices;

    public class JourneyViewModelBuilder
        : IJourneyViewModelBuilder {

        public JourneyViewModelBuilder(IOutcomeViewModelBuilder outcomeViewModelBuilder, IMappingEngine mappingEngine,
            ISymptomDicriminatorCollector symptomDicriminatorCollector, IKeywordCollector keywordCollector, IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder) {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _mappingEngine = mappingEngine;
            _symptomDicriminatorCollector = symptomDicriminatorCollector;
            _keywordCollector = keywordCollector;
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
        }

        public async Task<JourneyViewModel> Build(JourneyViewModel model, QuestionWithAnswers nextNode) {

            model.ProgressState();

            AddLastStepToJourney(model);

            var answer = JsonConvert.DeserializeObject<Answer>(model.SelectedAnswer);

            _symptomDicriminatorCollector.Collect(nextNode, model);
            model = _keywordCollector.Collect(answer, model);

            model = _mappingEngine.Mapper.Map(nextNode, model);

            switch (model.NodeType) {
                case NodeType.Outcome:
                    return await _outcomeViewModelBuilder.DispositionBuilder(model as OutcomeViewModel);
                case NodeType.Pathway:
                    return (await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(model as JustToBeSafeViewModel)).Item2; //todo refactor tuple away
            }

            return model;
        }

        private static void AddLastStepToJourney(JourneyViewModel model) {
            var journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson);
            journey.Steps.Add(model.ToStep());
            model.JourneyJson = JsonConvert.SerializeObject(journey);
        }

        public JourneyViewModel BuildPreviousQuestion(QuestionWithAnswers lastStep, JourneyViewModel model) {

            var journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson);

            journey.RemoveLastStep();

            model.CollectedKeywords = _keywordCollector.CollectKeywordsFromPreviousQuestion(model.CollectedKeywords,
                journey.Steps);

            model.RewindState(journey);

            return _mappingEngine.Mapper.Map(lastStep, model);
        }

        private readonly IOutcomeViewModelBuilder _outcomeViewModelBuilder;
        private readonly IMappingEngine _mappingEngine;
        private readonly ISymptomDicriminatorCollector _symptomDicriminatorCollector;
        private readonly IKeywordCollector _keywordCollector;
        private readonly IJustToBeSafeFirstViewModelBuilder _justToBeSafeFirstViewModelBuilder;
    }

    public interface IJourneyViewModelBuilder {
        Task<JourneyViewModel> Build(JourneyViewModel model, QuestionWithAnswers nextNode);
        JourneyViewModel BuildPreviousQuestion(QuestionWithAnswers lastStep, JourneyViewModel model);
    }
}