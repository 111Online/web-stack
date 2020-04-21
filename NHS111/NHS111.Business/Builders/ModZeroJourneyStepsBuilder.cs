using Newtonsoft.Json;
using NHS111.Models.Models.Azure;
using NHS111.Models.Models.Business.Question;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Business.Builders
{
    public class ModZeroJourneyStepsBuilder : IModZeroJourneyStepsBuilder
    {
        private readonly IStorageService _storageService;

        public ModZeroJourneyStepsBuilder(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public PathwayJourney GetModZeroJourney(string gender, int age, string pathwayType)
        {
            var modZeroJourneyEntity = _storageService.GetEntity<ModZeroJourney>(e => e.PartitionKey == "ModZeroJourney" && e.Gender == gender && age >= e.MinimumAge && age < e.MaximumAge && e.Type == pathwayType && e.Party == 1);
            if (modZeroJourneyEntity == null)
                throw new InvalidOperationException(string.Format("Missing mod zero config for {0} {1} of type {2} and party {3}", gender, new AgeCategory(age).Value, pathwayType, 1));

            var modZeroJourneyStepEntities = _storageService.GetAllEntities<ModZeroJourneyStep>(e => e.PartitionKey == modZeroJourneyEntity.RowKey);

            return new PathwayJourney()
            {
                PathwayId = modZeroJourneyEntity.PathwayId,
                DispositionId = modZeroJourneyEntity.DispositionId,
                Steps = modZeroJourneyStepEntities.OrderBy(e => int.Parse(e.RowKey)).Select(e =>
                    new JourneyStep { QuestionId = e.QuestionId, Answer = new Answer { Order = e.AnswerOrder } }),
                State = JsonConvert.DeserializeObject<IDictionary<string, string>>(modZeroJourneyEntity.State)
            };
        }
    }

    public interface IModZeroJourneyStepsBuilder
    {
        PathwayJourney GetModZeroJourney(string gender, int age, string pathwayType);
    }
}
