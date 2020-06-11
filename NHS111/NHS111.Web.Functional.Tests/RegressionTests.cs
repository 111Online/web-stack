﻿using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class RegressionTests : BaseTests
    {
        [Test]
        public void PathwayNotFound()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver,
                "Wound Problems, Plaster Casts, Tubes and Metal Appliances", TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Is the problem to do with any of these?");
            var outcomePage = questionPage

                .Answer<OutcomePage>("A tube or drain");

            outcomePage.VerifyPathwayNotFound();
        }

    }
}

