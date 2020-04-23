
namespace NHS111.Web.Presentation.Builders
{
    using NHS111.Models.Models.Domain;
    using NHS111.Models.Models.Web;

    public interface ISymptomDiscriminatorCollector
    {
        void Collect(QuestionWithAnswers quesionWithAnswers, JourneyViewModel exitingJourneyModel);
        void Collect(Answer answer, JourneyViewModel exitingJourneyModel);
    }
}