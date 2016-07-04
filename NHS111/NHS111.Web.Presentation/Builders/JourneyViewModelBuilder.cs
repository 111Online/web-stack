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
            ISymptomDiscriminatorCollector symptomDiscriminatorCollector, IKeywordCollector keywordCollector, IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder) {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _mappingEngine = mappingEngine;
            _symptomDiscriminatorCollector = symptomDiscriminatorCollector;
            _keywordCollector = keywordCollector;
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
        }

        public async Task<JourneyViewModel> Build(JourneyViewModel model, QuestionWithAnswers nextNode) {

            model.ProgressState();

            AddLastStepToJourney(model);

            var answer = JsonConvert.DeserializeObject<Answer>(model.SelectedAnswer);

            _symptomDiscriminatorCollector.Collect(nextNode, model);
            model = _keywordCollector.Collect(answer, model);

            model = _mappingEngine.Mapper.Map(nextNode, model);

            switch (model.NodeType) {
                case NodeType.Outcome:
                    var outcome = _mappingEngine.Mapper.Map<OutcomeViewModel>(model);
                    return await _outcomeViewModelBuilder.DispositionBuilder(outcome);
                case NodeType.Pathway:
                    var jtbs = _mappingEngine.Mapper.Map<JustToBeSafeViewModel>(model);
                    return (await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(jtbs)).Item2; //todo refactor tuple away
            }

            return model;
        }

        private static void AddLastStepToJourney(JourneyViewModel model) {
            model.Journey.Steps.Add(model.ToStep());
            model.JourneyJson = JsonConvert.SerializeObject(model.Journey);
        }

        public JourneyViewModel BuildPreviousQuestion(QuestionWithAnswers lastStep, JourneyViewModel model) {

            model.RemoveLastStep();

            model.CollectedKeywords = _keywordCollector.CollectKeywordsFromPreviousQuestion(model.CollectedKeywords,
                model.Journey.Steps);

            return _mappingEngine.Mapper.Map(lastStep, model);
        }

        private readonly IOutcomeViewModelBuilder _outcomeViewModelBuilder;
        private readonly IMappingEngine _mappingEngine;
        private readonly ISymptomDiscriminatorCollector _symptomDiscriminatorCollector;
        private readonly IKeywordCollector _keywordCollector;
        private readonly IJustToBeSafeFirstViewModelBuilder _justToBeSafeFirstViewModelBuilder;
    }

    public interface IJourneyViewModelBuilder {
        Task<JourneyViewModel> Build(JourneyViewModel model, QuestionWithAnswers nextNode);
        JourneyViewModel BuildPreviousQuestion(QuestionWithAnswers lastStep, JourneyViewModel model);
    }
}