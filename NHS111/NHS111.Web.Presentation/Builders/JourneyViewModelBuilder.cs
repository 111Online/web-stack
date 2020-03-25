using System;
using System.Collections.Generic;
using NHS111.Models.Models.Web.Validators;
using NHS111.Utils.Parser;

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
            IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder,
            IPostCodeAllowedValidator postCodeAllowedValidator)
        {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _mappingEngine = mappingEngine;
            _symptomDiscriminatorCollector = symptomDiscriminatorCollector;
            _keywordCollector = keywordCollector;
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
            _postCodeAllowedValidator = postCodeAllowedValidator;
        }

        public NodeDetailsViewModel BuildNodeDetails(QuestionWithAnswers nextNode)
        {
            var outcomeGroup = _mappingEngine.Mapper.Map<OutcomeGroup>(nextNode);
            var nodeType = _mappingEngine.Mapper.Map<NodeType>(nextNode);
            return new NodeDetailsViewModel() {OutcomeGroup = outcomeGroup, NodeType = nodeType};
        }


        public async Task<JourneyViewModel> Build(QuestionViewModel model, QuestionWithAnswers nextNode)
        {

            var journeyViewModel = MapJourneyViewModel(model, nextNode);

            switch (journeyViewModel.NodeType)
            {
                case NodeType.Outcome:
                    if (journeyViewModel.OutcomeGroup.Id.Equals("111_Search_Jump"))
                    {
                        return _mappingEngine.Mapper.Map<SearchJourneyViewModel>(journeyViewModel);
                    }

                    var outcome = _mappingEngine.Mapper.Map<OutcomeViewModel>(journeyViewModel);
                    var postcodeValidatorRepsonse = _postCodeAllowedValidator.IsAllowedPostcode(outcome.CurrentPostcode);
                    outcome.UserInfo.CurrentAddress.IsInPilotArea = postcodeValidatorRepsonse.IsInPilotAreaForOutcome(model.OutcomeGroup);
                    return await _outcomeViewModelBuilder.DispositionBuilder(outcome);
                case NodeType.Pathway:
                    var jtbs = _mappingEngine.Mapper.Map<JustToBeSafeViewModel>(journeyViewModel);
                    return (await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(jtbs))
                        .Item2; //todo refactor tuple away
                case NodeType.DeadEndJump:
                    var deadEndJump = _mappingEngine.Mapper.Map<OutcomeViewModel>(journeyViewModel);
                    return await _outcomeViewModelBuilder.DeadEndJumpBuilder(deadEndJump);
                case NodeType.PathwaySelectionJump:
                    var pathwaySelectionJump = _mappingEngine.Mapper.Map<OutcomeViewModel>(journeyViewModel);
                    return await _outcomeViewModelBuilder.PathwaySelectionJumpBuilder(pathwaySelectionJump);
            }

            return model;
        }

        public JourneyViewModel MapJourneyViewModel(QuestionViewModel model, QuestionWithAnswers nextNode)
        {
            model.ProgressState();

            model.Journey.Steps.Add(model.ToStep());
            model.ResetAnswerInputValue();
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
            if (nextNode.Labels.First() == "Outcome") {
                answer.Keywords += "|" + nextNode.NonQuestionKeywords;
                answer.ExcludeKeywords += "|" + nextNode.NonQuestionExcludeKeywords;
            }
            _symptomDiscriminatorCollector.Collect(nextNode, model);
            var journeyViewModel = _keywordCollector.Collect(answer, model);

            journeyViewModel = _mappingEngine.Mapper.Map(nextNode, journeyViewModel);
            return journeyViewModel;
        }

        public JourneyViewModel BuildPreviousQuestion(QuestionWithAnswers lastStep, QuestionViewModel model)
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
        private readonly IPostCodeAllowedValidator _postCodeAllowedValidator;
    }

    public interface IJourneyViewModelBuilder
    {
        Task<JourneyViewModel> Build(QuestionViewModel model, QuestionWithAnswers nextNode);
        JourneyViewModel BuildPreviousQuestion(QuestionWithAnswers lastStep, QuestionViewModel model);
        NodeDetailsViewModel BuildNodeDetails(QuestionWithAnswers nextNode);
    }
}