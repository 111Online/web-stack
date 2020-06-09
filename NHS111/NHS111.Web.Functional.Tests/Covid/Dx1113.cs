﻿using NHS111.Web.Functional.Utils;
using NUnit.Framework;


namespace NHS111.Web.Functional.Tests.Covid
{
    [TestFixture]
    class Dx1113 : BaseTests
    {
        [Test]
        public void NavigateToDispositionDx1113()
        {
            var questionPage = TestScenerios.LaunchWithCovidLink(Driver, TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste");

            var outcomePage = questionPage.Answer(1) // Yes - Smell
                .Answer(3) // no - hurt head
                .Answer(3) // no - breathless
                .Answer(1) // yes - cough
                .Answer(3) // no - breathing harder
                .Answer(3) // no - so ill
                .Answer(1) // yes - sharp pain
                .Answer(3) // no - cough blood
                .Answer(3) // no - confused
                .Answer(3) // no - doctor told you
                .Answer<OutcomePage>(1); // yes - nhs letter
            outcomePage.VerifyHiddenField("Id", "Dx1113");
            outcomePage.VerifyHasButton(buttonName: "PersonalDetails", buttonValue: "Book a call");
        }

        [Test]
        public void NavigateToDispositionDx1113Journey2()
        {
            var questionPage = TestScenerios.LaunchWithCovidLink(Driver, TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste");

            var outcomePage = questionPage.Answer(1) // Yes - Smell
                .Answer(3) // no - hurt head
                .Answer(3) // no - breathless
                .Answer(1) // yes - cough
                .Answer(3) // no - breathing harder
                .Answer(1) // yes - so ill ***
                .Answer(3) // no - bruise
                .Answer(3) // no - meningitis
                .Answer(3) // no - confused
                .Answer(1) // yes - sharp pain
                .Answer<OutcomePage>(3); // no - cough blood
            outcomePage.VerifyHiddenField("Id", "Dx1113");
            outcomePage.VerifyHasButton(buttonName: "PersonalDetails", buttonValue: "Book a call");

        }
    }
}
