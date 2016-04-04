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

            Mapper.CreateMap<JourneyViewModel, OutcomeViewModel>();
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

                if (string.IsNullOrEmpty(source))
                    return new JourneyViewModel();

                var questionWithAnswers = JsonConvert.DeserializeObject<QuestionWithAnswers>(source);

                if (journeyViewModel == null)
                    return new JourneyViewModel
                    {
                        Id = questionWithAnswers.Question.Id,
                        Title = questionWithAnswers.Question.Title,
                        Answers = questionWithAnswers.Answers ?? Enumerable.Empty<Answer>().ToList(),
                        NodeType = (NodeType)Enum.Parse(typeof(NodeType), questionWithAnswers.Labels.FirstOrDefault())
                    };

                journeyViewModel.Id = questionWithAnswers.Question.Id;
                journeyViewModel.Title = questionWithAnswers.Question.Title;

                var questionAndBullets = questionWithAnswers.Question.TitleWithBullets();
                journeyViewModel.TitleWithoutBullets = questionAndBullets.Item1;
                journeyViewModel.Bullets = questionAndBullets.Item2;

                journeyViewModel.Answers = questionWithAnswers.Answers ?? Enumerable.Empty<Answer>().ToList();
                journeyViewModel.NodeType = (NodeType)Enum.Parse(typeof(NodeType), questionWithAnswers.Labels.FirstOrDefault());
                journeyViewModel.QuestionNo = questionWithAnswers.Question.QuestionNo;
                journeyViewModel.Rationale = questionWithAnswers.Question.Rationale;

                if (questionWithAnswers.State != null)
                {
                    journeyViewModel.State = questionWithAnswers.State;
                    journeyViewModel.StateJson = JsonConvert.SerializeObject(questionWithAnswers.State);
                }

                return journeyViewModel;
            }
        }

        public class FromQuestionWithAnswersToJourneyViewModelConverter : ITypeConverter<QuestionWithAnswers, JourneyViewModel>
        {
            public JourneyViewModel Convert(ResolutionContext context)
            {
                var questionWithAnswers = (QuestionWithAnswers)context.SourceValue;
                var journeyViewModel = (JourneyViewModel)context.DestinationValue;

                var question = questionWithAnswers.Question;
                var answers = questionWithAnswers.Answers;

                if (journeyViewModel == null)
                    return new JourneyViewModel
                    {
                        Id = question.Id,
                        Title = question.Title,
                        Answers = answers ?? Enumerable.Empty<Answer>().ToList(),
                    };

                journeyViewModel.Id = question.Id;
                journeyViewModel.Title = question.Title;

                var questionAndBullets = question.TitleWithBullets();
                journeyViewModel.TitleWithoutBullets = questionAndBullets.Item1;
                journeyViewModel.Bullets = questionAndBullets.Item2;

                journeyViewModel.Answers = answers ?? Enumerable.Empty<Answer>().ToList();
                journeyViewModel.QuestionNo = question.QuestionNo;
                journeyViewModel.State = questionWithAnswers.State;
                return journeyViewModel;
            }
        }
    }

}