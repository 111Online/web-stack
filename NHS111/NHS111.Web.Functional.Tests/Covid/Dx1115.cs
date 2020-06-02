using NHS111.Web.Functional.Utils;
using NUnit.Framework;


namespace NHS111.Web.Functional.Tests.Covid
{
    [TestFixture]
    class Dx1115 : BaseTests
    {
        [Test]
        public void NavigateToDispositionDx1115_6Hours()
        {
            var questionPage = TestScenerios.LaunchWithCovidLink(Driver, TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Coldandflusymptoms");

            var outcomePage = questionPage.Answer(2) // No - breathless
                .Answer(1) // Yes - cough
                .Answer(3) // No - breathing harder
                .Answer(3) // No - so ill
                .Answer(1) // Yes - sharp pain
                .Answer(3) // No - coughed blood
                .Answer(3) // No - confused
                .Answer(3) // No - doctor told you
                .Answer(3) // No - NHS Letter
                .Answer<OutcomePage>(3); // No - diabetes
            outcomePage.VerifyHiddenField("Id", "Dx1115");
        }

        [Test]
        public void NavigateToDispositionDx1115_LossOfTasteAndSmell()
        {
            var questionPage = TestScenerios.LaunchWithCovidLink(Driver, TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste");

            var outcomePage = questionPage.Answer(1) // Yes - loss of smell
                .Answer(3) // No - hurt head
                .Answer(3) // No - breathless
                .Answer(1) // Yes - continuous cough
                .Answer(3) // No - breathing harder
                .Answer(1) // Yes - so ill
                .Answer(3) // No - bruises
                .Answer(4) // No - meningitis
                .Answer(3) // No - confused
                .Answer(3) // No - sharp pain
                .Answer(3) // No - choked
                .Answer(3) // No - breathed toxic
                .Answer(3) // No - coughed blood
                .Answer(3) // No - doctor told you
                .Answer(3) // No - nhs letter
                .Answer(1) // No - diabetes
                .Answer(1) // Yes- sugar levels
                .Answer<OutcomePage>(3); // No - take insulin
            outcomePage.VerifyHiddenField("Id", "Dx1115");
        }

    }
}
