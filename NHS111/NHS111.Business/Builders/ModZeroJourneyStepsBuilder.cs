using System;
using System.Configuration;
using System.Linq;
using NHS111.Models.Models.Business.Question;
using NHS111.Models.Models.Configuration;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Business.Builders
{
    public class ModZeroJourneyStepsBuilder : IModZeroJourneyStepsBuilder
    {
        private const string Section = "moduleZeroJourneys";

        public ModZeroJourney GetModZeroJourney(string gender, int age, string pathwayType)
        {
            var section = ConfigurationManager.GetSection(Section) as ModZeroJourneysSection;
            if (section == null)
                throw new InvalidOperationException(string.Format("Missing section name {0}", "moduleZeroTriage"));

            var modZeroJourneyElement = section.ModuleZeroJourneys.GetModZeroJourneyElement(gender, new AgeCategory(age).Value, pathwayType == "Trauma");

            return new ModZeroJourney
            {
                PathwayId = modZeroJourneyElement.PathwayId,
                DispositionId = modZeroJourneyElement.DispositionId,
                Steps = modZeroJourneyElement.JourneySteps.Select(j =>
                    new JourneyStep {QuestionId = j.Id, Answer = new Answer {Order = j.Order}})
            };
        }
    }

    public interface IModZeroJourneyStepsBuilder
    {
        ModZeroJourney GetModZeroJourney(string gender, int age, string pathwayType);
    }
}
