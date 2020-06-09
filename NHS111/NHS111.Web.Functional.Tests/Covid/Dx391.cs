using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests.Covid
{
    [TestFixture]
    class Dx391 : BaseTests
    {
        [Test]
        public void NavigateToDispositionDx391()
        {
            var questionPage = TestScenerios.LaunchWithCovidLink(Driver, TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Coldorflusymptoms");

            var outcomePage = questionPage.Answer(2) // No - breathless
                .Answer(1) // Yes - cough
                .Answer(3) // No - breathing harder
                .Answer(3) // No - so ill
                .Answer(3) // No - sharp pain
                .Answer(3) // No - choked
                .Answer(3) // No - breathed toxic
                .Answer(3) // No - coughed blood
                .Answer(3) // No - confused
                .Answer(3) // No - doctor told you
                .Answer(3) // No - NHS Letter
                .Answer(3) // No - diabetes
                .Answer(3) // Not sure - last month
                .Answer(4) // No - temperature
                .Answer<OutcomePage>(6); // No - doctor said
            outcomePage.VerifyHiddenField("Id", "Dx391");
        }
    }
}
