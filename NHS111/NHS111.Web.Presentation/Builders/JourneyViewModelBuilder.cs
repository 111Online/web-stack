using System.Collections.Generic;

namespace NHS111.Web.Presentation.Builders
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Domain;
    using NHS111.Models.Models.Web;
    using NHS111.Models.Models.Web.Enums;

    public class JourneyViewModelBuilder
        : IJourneyViewModelBuilder
    {

        public JourneyViewModelBuilder(IOutcomeViewModelBuilder outcomeViewModelBuilder, IMappingEngine mappingEngine,
            ISymptomDiscriminatorCollector symptomDiscriminatorCollector, IKeywordCollector keywordCollector,
            IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder)
        {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _mappingEngine = mappingEngine;
            _symptomDiscriminatorCollector = symptomDiscriminatorCollector;
            _keywordCollector = keywordCollector;
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
        }

        public async Task<JourneyViewModel> Build(JourneyViewModel model, QuestionWithAnswers nextNode)
        {

            model.ProgressState();

            model.Journey.Steps.Add(model.ToStep());

            if (!string.IsNullOrEmpty(nextNode.NonQuestionKeywords))
            {
                model.Journey.Steps.Last().Answer.Keywords += "|" + nextNode.NonQuestionKeywords;
            }
            if (!string.IsNullOrEmpty(nextNode.NonQuestionExcludeKeywords))
            {
                model.Journey.Steps.Last().Answer.ExcludeKeywords += "|" + nextNode.NonQuestionExcludeKeywords;
            }
            model.JourneyJson = JsonConvert.SerializeObject(model.Journey);

            var answer = JsonConvert.DeserializeObject<Answer>(model.SelectedAnswer);

            _symptomDiscriminatorCollector.Collect(nextNode, model);
            model = _keywordCollector.Collect(answer, model);

            model = _mappingEngine.Mapper.Map(nextNode, model);

            switch (model.NodeType)
            {
                case NodeType.Outcome:
                    var outcome = _mappingEngine.Mapper.Map<OutcomeViewModel>(model);
                    return await _outcomeViewModelBuilder.DispositionBuilder(outcome);
                case NodeType.Pathway:
                    var jtbs = _mappingEngine.Mapper.Map<JustToBeSafeViewModel>(model);
                    return (await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(jtbs)).Item2; //todo refactor tuple away
                case NodeType.DeadEndJump:
                    var deadEndJump = _mappingEngine.Mapper.Map<OutcomeViewModel>(model);
                    return await _outcomeViewModelBuilder.DeadEndJumpBuilder(deadEndJump);
                case NodeType.PathwaySelectionJump:
                    var pathwaySelectionJump = _mappingEngine.Mapper.Map<OutcomeViewModel>(model);
                    return await _outcomeViewModelBuilder.PathwaySelectionJumpBuilder(pathwaySelectionJump);
            }

            return model;
        }

        public JourneyViewModel BuildPreviousQuestion(QuestionWithAnswers lastStep, JourneyViewModel model)
        {

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
        private readonly IConfiguration _configuration;
    }

    public interface IJourneyViewModelBuilder
    {
        Task<JourneyViewModel> Build(JourneyViewModel model, QuestionWithAnswers nextNode);
        JourneyViewModel BuildPreviousQuestion(QuestionWithAnswers lastStep, JourneyViewModel model);
    }
}