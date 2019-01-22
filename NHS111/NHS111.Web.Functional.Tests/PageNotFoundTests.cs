using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class PathwayNotFoundTests : BaseTests
    {
        [Test]
        public void PathwayNotFound_Displays()
        {
            
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Mental Health Problems", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            questionPage.Verify();
            questionPage.VerifyRationale();

            //TestScenarioPart.HomePage(Driver);


            //var pageNotFound = TestScenarioPart.PageNotFound(Driver);
            
            //pageNotFound.Verify();
        }
    }
}
