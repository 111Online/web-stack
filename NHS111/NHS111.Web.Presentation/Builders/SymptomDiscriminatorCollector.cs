namespace NHS111.Web.Presentation.Builders
{
    using NHS111.Models.Models.Domain;
    using NHS111.Models.Models.Web;
    using System;

    public class SymptomDiscriminatorCollector : ISymptomDiscriminatorCollector
    {
        public void Collect(QuestionWithAnswers quesionWithAnswers, JourneyViewModel exitingJourneyModel)
        {
            if (quesionWithAnswers.Answered == null)
                return;

            Collect(quesionWithAnswers.Answered, exitingJourneyModel);
        }

        public void Collect(Answer answer, JourneyViewModel exitingJourneyModel)
        {
            AddDiscriminatorToModel(answer.SymptomDiscriminator, exitingJourneyModel);
        }

        private void AddDiscriminatorToModel(string symptomDisciminator, JourneyViewModel model)
        {
            if (!String.IsNullOrEmpty(symptomDisciminator))
                model.SymptomDiscriminatorCode = symptomDisciminator;
        }
    }
}