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
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Vaginal Swellings", TestScenerioSex.Female, 20);

            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .Answer(1)
                .Answer<OutcomePage>("Yes");

            outcomePage.VerifyCareAdvice(new [] { "Abdominal pain", "Medication, pain and/or fever, abdominal/urinary", "Fever" });
        }
    }
}
