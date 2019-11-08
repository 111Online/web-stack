using System;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.Logging;

namespace NHS111.Models.Mappers.WebMappings
{
    public class JourneyViewModelMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Answer, JourneyViewModel>()
                .ConvertUsing<FromAnswerToJourneyViewModelConverter>();

            Mapper.CreateMap<string, JourneyViewModel>()
                .ConvertUsing<FromQuestionToJourneyViewModelConverter>();

            Mapper.CreateMap<QuestionWithAnswers, JourneyViewModel>()
               .ConvertUsing<FromQuestionWithAnswersToJourneyViewModelConverter>();


            Mapper.CreateMap<QuestionWithAnswers, OutcomeGroup>()
                .ConvertUsing<FromQuestionWithAnswersToOutcomeGroupModelConverter>();

            Mapper.CreateMap<QuestionWithAnswers, NodeType>()
                .ConvertUsing<FromQuestionWithAnswersToNodeTypeModelConverter>();


            Mapper.CreateMap<JourneyViewModel, OutcomeViewModel>()
                .ForMember(s => s.SelectedServiceId, o => o.Ignore())
                .ForMember(s => s.DosCheckCapacitySummaryResult, o => o.Ignore())
                .ForMember(s => s.SurgeryViewModel, o => o.Ignore())
                .ForMember(s => s.CareAdviceMarkers, o => o.Ignore())
                .ForMember(s => s.CareAdvices, o => o.Ignore())
                .ForMember(s => s.SymptomGroup, o => o.Ignore())
                .ForMember(s => s.Urgency, o => o.Ignore())
                .ForMember(s => s.WorseningCareAdvice, o => o.Ignore())
                .ForMember(s => s.SymptomDiscriminator, o => o.Ignore())
                .ForMember(s => s.CurrentView, o => o.Ignore())
                .ForMember(s => s.SurveyLink, o => o.Ignore())
                .ForMember(s => s.Informant, opt => opt.Ignore())
                .ForMember(s => s.UnavailableSelectedService, o => o.Ignore())
                .ForMember(s => s.GroupedDosServices, o => o.Ignore())
                .ForMember(s => s.HasAcceptedCallbackOffer, o => o.Ignore())
                .ForMember(s => s.RecommendedService, opt => opt.Ignore());

            Mapper.CreateMap<JourneyViewModel, AuditEntry>()
                .ForMember(dest => dest.AnswerTitle, opt => opt.Ignore())
                .ForMember(dest => dest.AnswerOrder, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionTitle, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionNo, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionId, opt => opt.Ignore())
                .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.SessionId))
                .ForMember(dest => dest.PathwayId, opt => opt.MapFrom(src => src.PathwayId))
                .ForMember(dest => dest.PathwayTitle, opt => opt.MapFrom(src => src.PathwayTitle))
                .ForMember(dest => dest.Journey, opt => opt.MapFrom(src => src.JourneyJson))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.StateJson))
                .ForMember(dest => dest.TIMESTAMP, opt => opt.Ignore())
                .ForMember(dest => dest.DxCode, opt => opt.Ignore())
                .ForMember(dest => dest.EventData, opt => opt.Ignore())
                .ForMember(dest => dest.DosRequest, opt => opt.Ignore())
                .ForMember(dest => dest.DosResponse, opt => opt.Ignore())
                .ForMember(dest => dest.ItkRequest, opt => opt.Ignore())
                .ForMember(dest => dest.ItkResponse, opt => opt.Ignore())
                .ForMember(dest => dest.CampaignSource, opt => opt.Ignore())
                .ForMember(dest => dest.Campaign, opt => opt.Ignore())
				.ForMember(dest => dest.Page, opt => opt.Ignore())
                .ForMember(dest => dest.Age, opt => opt.Ignore())
                .ForMember(dest => dest.Gender, opt => opt.Ignore())
                .ForMember(dest => dest.Search, opt => opt.Ignore());
        }

        public class FromAnswerToJourneyViewModelConverter : ITypeConverter<Answer, JourneyViewModel>
        {
            public JourneyViewModel Convert(ResolutionContext context)
            {
                var answer = (Answer)context.SourceValue;
                var journeyViewModel = (JourneyViewModel)context.DestinationValue;

                if (!string.IsNullOrEmpty(answer.SymptomDiscriminator))
                    journeyViewModel.SymptomDiscriminatorCode = answer.SymptomDiscriminator;

                return journeyViewModel;
            }
        }

        public class FromQuestionToJourneyViewModelConverter : ITypeConverter<string, JourneyViewModel>
        {
            public JourneyViewModel Convert(ResolutionContext context)
            {
                var source = context.SourceValue.ToString();
                var journeyViewModel = (JourneyViewModel)context.DestinationValue;

                var questionWithAnswers = JsonConvert.DeserializeObject<QuestionWithAnswers>(source);

                journeyViewModel = BuildJourneyViewModel(journeyViewModel, questionWithAnswers);

                return journeyViewModel;
            }
        }

        public class FromQuestionWithAnswersToJourneyViewModelConverter : ITypeConverter<QuestionWithAnswers, JourneyViewModel>
        {
            public JourneyViewModel Convert(ResolutionContext context)
            {
                var questionWithAnswers = (QuestionWithAnswers)context.SourceValue;
                var journeyViewModel = (JourneyViewModel)context.DestinationValue;

                journeyViewModel = BuildJourneyViewModel(journeyViewModel, questionWithAnswers);
                return journeyViewModel;
            }
        }

        public class FromQuestionWithAnswersToOutcomeGroupModelConverter : ITypeConverter<QuestionWithAnswers, OutcomeGroup>
        {
            public OutcomeGroup Convert(ResolutionContext context)
            {
                var questionWithAnswers = (QuestionWithAnswers)context.SourceValue;
                return BuildOutcomeGroup(questionWithAnswers);
            }
        }


        public class FromQuestionWithAnswersToNodeTypeModelConverter : ITypeConverter<QuestionWithAnswers, NodeType>
        {
            public NodeType Convert(ResolutionContext context)
            {
                var questionWithAnswers = (QuestionWithAnswers)context.SourceValue;
                return BuildNodeType(questionWithAnswers);
            }
        }

        private static JourneyViewModel BuildJourneyViewModel(JourneyViewModel modelToPopulate, QuestionWithAnswers questionWithAnswers)
        {
            var journeyViewModel = modelToPopulate;
            if (journeyViewModel == null)
                journeyViewModel = new JourneyViewModel();

            if (questionWithAnswers == null) return journeyViewModel;

            journeyViewModel.Id = questionWithAnswers.Question.Id;
            journeyViewModel.Title = questionWithAnswers.Question.Title;
            journeyViewModel.TimeFrameText = questionWithAnswers.Question.TimeFrameText;
            journeyViewModel.TimeFrameMinutes = questionWithAnswers.Question.TimeFrame;
            journeyViewModel.WaitTimeText = questionWithAnswers.Question.WaitTimeText;
            journeyViewModel.DispositionUrgencyText = questionWithAnswers.Question.DispositionUrgencyText;
            journeyViewModel.Content = questionWithAnswers.Question.Content;
            journeyViewModel.NextButtonText = string.IsNullOrEmpty(questionWithAnswers.Question.NextButtonText) ? "Next question" : questionWithAnswers.Question.NextButtonText;
            var questionAndBullets = questionWithAnswers.Question.TitleWithBullets();
            journeyViewModel.TitleWithoutBullets = questionAndBullets.Item1;
            journeyViewModel.Bullets = questionAndBullets.Item2;
            journeyViewModel.QuestionType = questionWithAnswers.Question.QuestionType;

            journeyViewModel.Answers = questionWithAnswers.Answers ?? Enumerable.Empty<Answer>().ToList();
            journeyViewModel.NodeType = BuildNodeType(questionWithAnswers);
            journeyViewModel.QuestionNo = questionWithAnswers.Question.QuestionNo;
            journeyViewModel.Rationale = questionWithAnswers.Question.Rationale;

            journeyViewModel.OutcomeGroup = BuildOutcomeGroup(questionWithAnswers);

            if (questionWithAnswers.State != null)
            {
                journeyViewModel.State = questionWithAnswers.State;
                journeyViewModel.StateJson = JsonConvert.SerializeObject(questionWithAnswers.State);
            }
            return journeyViewModel;
        }

        private static NodeType BuildNodeType(QuestionWithAnswers questionWithAnswers)
        {
            return (NodeType)Enum.Parse(typeof(NodeType), questionWithAnswers.Labels.FirstOrDefault());
        }
        private static OutcomeGroup BuildOutcomeGroup(QuestionWithAnswers questionWithAnswers)
        {
            var mappedOutcomeGroup = new OutcomeGroup();
            if (questionWithAnswers.Group != null)
            {
                mappedOutcomeGroup = questionWithAnswers.Group;
                //this needs to be mapped better, ultimately this should be data driven from data layers so the above line is all that's needed.
                var outcomeGroup = OutcomeGroup.OutcomeGroups[questionWithAnswers.Group.Id];
                mappedOutcomeGroup.Label = outcomeGroup.Label;
                mappedOutcomeGroup.ITK = outcomeGroup.ITK;

            }
            return mappedOutcomeGroup;
        }
    }

}
