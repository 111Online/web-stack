using System.Collections.Generic;

namespace NHS111.Web.Presentation.Builders
{
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

            AddLastStepToJourney(model);

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

        public JourneyViewModel BuildInitialQuestion()
        {
            JourneyViewModel model = new JourneyViewModel();

            Answer crush = new Answer
            {
                Title = "Central crushing pain",
                SupportingInformation = "A feeling of crushing pressure like a heavy weight pushing down on your chest.",
                Order = 1
            };

            Answer stroke = new Answer
            {
                Title = "You think it's a stroke",
                SupportingInformation =
                    "Signs of a stroke include being unable to raise both arms and keep them there, difficulty speaking or a lopsided face.",
                Order = 2
            };

            Answer choke = new Answer
            {
                Title = "Severe difficulty breathing, choking or turning blue",
                SupportingInformation =
                    "This means you are unable to finish a sentence without stopping several times to take a breath. You may be gasping, wheezing or putting a lot of effort into breathing.",
                Order = 3
            };

            Answer bleed = new Answer
            {
                Title = "Bleeding very heavily",
                SupportingInformation = "This means heavy bleeding that is continuing despite attempts to stop it.",
                Order = 4
            };

            Answer injury = new Answer
            {
                Title = "A severe injury",
                SupportingInformation =
                    "This includes deep wounds, or injuries caused by falls, assault or road traffic accidents and will require an immediate medical assessment.",
                Order = 5
            };

            Answer seizure = new Answer
            {
                Title = "The person you are enquiring about is unconscious or having a seizure (fit)",
                SupportingInformation =
                    @"Unconscious - Not awake and totally unaware of what is going on around you. As if you are asleep, but with no response if someone tries to wake you. 
                    
                    Seizure - Uncontrolled electrical activity in the brain, which can lead to loss of consciousness and/or loss of bladder and bowel control. Seizures are often known as fits.",
                Order = 6
            };

            model.Answers = new List<Answer> { crush, stroke, choke, bleed, injury, seizure };

            return model;
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
        JourneyViewModel BuildInitialQuestion();
    }
}