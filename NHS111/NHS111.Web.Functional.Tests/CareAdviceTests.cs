using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    public class CareAdviceTests : BaseTests
    {
        [Test]
        //Ensures that Medication, pain and/or fever care advice is not additionally included in results
        public void CareAdvice_Is_Correctly_Excluded()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Headache", TestScenarioSex.Female, 20);

            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 2)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 2)
                .Answer(5)
                .AnswerSuccessiveByOrder(3, 8)
                .Answer(1)
                .Answer(3)
                .Answer(4)
                .Answer<OutcomePage>("I've had it for 3 days or more")
                .OpenCareAdvice();

            outcomePage.VerifyCareAdvice(new[] { "Medication, pain and/or fever", "Fever", "Coronavirus (COVID-19)"});
        }
        [Test]
        //Ensures that Care advice stem removal is no longer in place. CX221005. Text no longer appearing needs to have individual asertions.
        public void CareAdvice_stem_is_no_longer_removed()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Vaginal Discharge", TestScenarioSex.Female, 30);

            var outcomePage = questionPage
                .Answer(3)
                .Answer(3)
                .Answer(1)
                .Answer(3)
                .Answer(3)
                .Answer<OutcomePage>("No")
                .OpenCareAdvice();

            outcomePage.VerifyCareAdvice(new[] { "Genital discharge/irritation", "Medication, pain and/or fever", "Fever" });
            var sexualhealthAdviceExists = outcomePage.CareAdviceExists(new[] { "If you are sexually active:", "Don't have sex until the problem is sorted out." });
            var sexualhealth1AdviceDoesNotExists = outcomePage.CareAdviceExists(new[] { "Don't have sex until the problem is sorted out.", "Wear loose-fitting cotton clothes and underwear, as they allow the air to circulate.", "If you need to be seen by a GP or out of hours service, take a wee sample in a clean jar.", "If you're sexually active:", "Don't have sex until the problem has cleared up." });
            var sexualhealth2AdviceDoesNotExists = outcomePage.CareAdviceExists(new[] { "Wear loose-fitting cotton clothes and underwear, as they allow the air to circulate.", "If you need to be seen by a GP or out of hours service, take a wee sample in a clean jar.", "If you're sexually active:", "Don't have sex until the problem has cleared up." });
            var sexualhealth3AdviceDoesNotExists = outcomePage.CareAdviceExists(new[] { "If you need to be seen by a GP or out of hours service, take a wee sample in a clean jar.", "If you're sexually active:", "Don't have sex until the problem has cleared up." });
            var sexualhealth4AdviceDoesNotExists = outcomePage.CareAdviceExists(new[] { "If you're sexually active:", "Don't have sex until the problem has cleared up." });

            Assert.IsTrue(sexualhealthAdviceExists);
            Assert.IsFalse(sexualhealth1AdviceDoesNotExists);
            Assert.IsFalse(sexualhealth2AdviceDoesNotExists);
            Assert.IsFalse(sexualhealth3AdviceDoesNotExists);
            Assert.IsFalse(sexualhealth4AdviceDoesNotExists);

        }
    }
}
