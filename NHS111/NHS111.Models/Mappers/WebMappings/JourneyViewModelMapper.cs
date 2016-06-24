using System;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;

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
                .ForMember(s => s.Urgency, o => o.Ignore())
                .ForMember(s => s.SymptomGroup, o => o.Ignore())
                .ForMember(s => s.AddressSearchViewModel, o => o.Ignore())
                .ForMember(s => s.ItkSendSuccess, o => o.Ignore())
                .ForMember(s => s.WorseningCareAdvice, o => o.Ignore());
        }

        public class FromAnswerToJourneyViewModelConverter : ITypeConverter<Answer, JourneyViewModel>
        {
            public JourneyViewModel Convert(ResolutionContext context)
            {
                var answer = (Answer)context.SourceValue;
                var journeyViewModel = (JourneyViewModel)context.DestinationValue;

                if (!string.IsNullOrEmpty(answer.SymptomDiscriminator))
                    journeyViewModel.SymptomDiscriminator = answer.SymptomDiscriminator;

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