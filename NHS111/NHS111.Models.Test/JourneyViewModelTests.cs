using NHS111.Models.Models.Web;
using NUnit.Framework;

namespace NHS111.Models.Test
{
    [TestFixture]
    public class JourneyViewModelTests
    {
        [Test]
        public void Rationale_NewLines()
        {
            const string textInput = @"Normal body temperature is 36C - 37C (96.8F - 98.6F).
Even if you have a high temperature, your hands and feet may still feel cool, or you may feel cold and shivery. 
Feeling the chest, abdomen or back with the hand is a reliable way of deciding whether someone has a high temperature.";
            const string expectedOutput = @"Normal body temperature is 36C - 37C (96.8F - 98.6F).<br/>Even if you have a high temperature, your hands and feet may still feel cool, or you may feel cold and shivery. <br/>Feeling the chest, abdomen or back with the hand is a reliable way of deciding whether someone has a high temperature.";

            JourneyViewModel jvm = new JourneyViewModel { Rationale = textInput };

            string result = jvm.RationaleHtml;

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void Rationale_NoNewLines()
        {
            const string textInput = @"Normal body temperature is 36C - 37C (96.8F - 98.6F).Even if you have a high temperature, your hands and feet may still feel cool, or you may feel cold and shivery. Feeling the chest, abdomen or back with the hand is a reliable way of deciding whether someone has a high temperature.";
            const string expectedOutput = @"Normal body temperature is 36C - 37C (96.8F - 98.6F).Even if you have a high temperature, your hands and feet may still feel cool, or you may feel cold and shivery. Feeling the chest, abdomen or back with the hand is a reliable way of deciding whether someone has a high temperature.";

            JourneyViewModel jvm = new JourneyViewModel { Rationale = textInput };

            string result = jvm.RationaleHtml;

            Assert.AreEqual(expectedOutput, result);
        }
    }
}