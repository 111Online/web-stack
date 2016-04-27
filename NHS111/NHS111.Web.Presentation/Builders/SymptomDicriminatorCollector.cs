using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;

namespace NHS111.Web.Presentation.Builders
{
    public interface ISymptomDicriminatorCollector
    {
        JourneyViewModel Collect(QuestionWithAnswers quesionWithAnswers, JourneyViewModel exitingJourneyModel);
        JourneyViewModel Collect(Answer answer, JourneyViewModel exitingJourneyModel);
    }

    public class SymptomDicriminatorCollector : ISymptomDicriminatorCollector
    {
        public JourneyViewModel Collect(QuestionWithAnswers quesionWithAnswers, JourneyViewModel exitingJourneyModel)
        {
            if (quesionWithAnswers.Answered == null) return exitingJourneyModel;
            return Collect(quesionWithAnswers.Answered, exitingJourneyModel);
        }

        public JourneyViewModel Collect(Answer answer, JourneyViewModel exitingJourneyModel)
        {
            return AddDiscriminatorToModel(answer.SymptomDiscriminator, exitingJourneyModel);
        }

        private JourneyViewModel AddDiscriminatorToModel(string symptomDisciminator, JourneyViewModel model)
        {
            if (!String.IsNullOrEmpty(symptomDisciminator))
                model.SymptomDiscriminator = symptomDisciminator;

            return model;
        }
    }
}
