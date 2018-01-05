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

            Mapper.CreateMap<JourneyViewModel, OutcomeViewModel>()
                .ForMember(s => s.SelectedServiceId, o => o.Ignore())
                .ForMember(s => s.DosCheckCapacitySummaryResult, o => o.Ignore())
                .ForMember(s => s.SurgeryViewModel, o => o.Ignore())
                .ForMember(s => s.CareAdviceMarkers, o => o.Ignore())
                .ForMember(s => s.CareAdvices, o => o.Ignore())
                .ForMember(s => s.SymptomGroup, o => o.Ignore())
                .ForMember(s => s.Urgency, o => o.Ignore())
                .ForMember(s => s.AddressInfoViewModel, o => o.Ignore())
                .ForMember(s => s.ItkSendSuccess, o => o.Ignore())
                .ForMember(s => s.ItkDuplicate, o => o.Ignore())
                .ForMember(s => s.WorseningCareAdvice, o => o.Ignore())
                .ForMember(s => s.SymptomDiscriminator, o => o.Ignore())
                .ForMember(s => s.CurrentView, o => o.Ignore())
                .ForMember(s => s.SurveyLink, o => o.Ignore())
                .ForMember(s => s.Informant, opt => opt.Ignore())
                .ForMember(s => s.UnavailableSelectedService, o => o.Ignore())
                .ForMember(s => s.GroupedDosServices, o => o.Ignore());

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
                .ForMember(dest => dest.Timestamp, opt => opt.Ignore())
                .ForMember(dest => dest.TIMESTAMP, opt => opt.Ignore())
                .ForMember(dest => dest.ETag, opt => opt.Ignore())
                .ForMember(dest => dest.PartitionKey, opt => opt.Ignore())
                .ForMember(dest => dest.RowKey, opt => opt.Ignore())
                .ForMember(dest => dest.DxCode, opt => opt.Ignore())
                .ForMember(dest => dest.EventData, opt => opt.Ignore())
                .ForMember(dest => dest.DosRequest, opt => opt.Ignore())
                .ForMember(dest => dest.DosResponse, opt => opt.Ignore())
                .ForMember(dest => dest.ItkRequest, opt => opt.Ignore())
                .ForMember(dest => dest.ItkResponse, opt => opt.Ignore())
                .ForMember(dest => dest.CampaignSource, opt => opt.Ignore())
                .ForMember(dest => dest.Campaign, opt => opt.Ignore());
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

            var questionAndBullets = questionWithAnswers.Question.TitleWithBullets();
            journeyViewModel.TitleWithoutBullets = questionAndBullets.Item1;
            journeyViewModel.Bullets = questionAndBullets.Item2;

            journeyViewModel.Answers = questionWithAnswers.Answers ?? Enumerable.Empty<Answer>().ToList();
            journeyViewModel.NodeType = (NodeType)Enum.Parse(typeof(NodeType), questionWithAnswers.Labels.FirstOrDefault());
            journeyViewModel.QuestionNo = questionWithAnswers.Question.QuestionNo;
            journeyViewModel.Rationale = questionWithAnswers.Question.Rationale;

            if (questionWithAnswers.Group != null)
            {
                journeyViewModel.OutcomeGroup = questionWithAnswers.Group;
                //this needs to be mapped better, ultimately this should be data driven from data layers so the above line is all that's needed.
                var outcomeGroup = OutcomeGroup.OutcomeGroups[questionWithAnswers.Group.Id];
                journeyViewModel.OutcomeGroup.Label = outcomeGroup.Label;
                journeyViewModel.OutcomeGroup.ITK = outcomeGroup.ITK;
            }

            if (questionWithAnswers.State != null)
            {
                journeyViewModel.State = questionWithAnswers.State;
                journeyViewModel.StateJson = JsonConvert.SerializeObject(questionWithAnswers.State);
            }
            return journeyViewModel;
        }
    }

}