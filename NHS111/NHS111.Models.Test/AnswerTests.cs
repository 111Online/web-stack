using NHS111.Models.Models.Domain;
using NUnit.Framework;

namespace NHS111.Models.Test
{
    [TestFixture]
    public class AnswerTests
    {
        [Test]
        public void AnswerSupportingInfo_NewLines()
        {
            const string textInput = @"Select this option if you have a query about the menopause. 
Common symptoms of the menopause include night sweats, hot flushes, vaginal dryness and mood swings.";
            const string expectedOutput = @"Select this option if you have a query about the menopause. <br/>Common symptoms of the menopause include night sweats, hot flushes, vaginal dryness and mood swings.";

            Answer answer = new Answer { SupportingInformation = textInput };

            string result = answer.SupportingInformationHtml;

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void AnswerSupportingInfo_NoNewLines()
        {
            const string textInput = @"Select this option if you have a query about the menopause. Common symptoms of the menopause include night sweats, hot flushes, vaginal dryness and mood swings.";
            const string expectedOutput = @"Select this option if you have a query about the menopause. Common symptoms of the menopause include night sweats, hot flushes, vaginal dryness and mood swings.";

            Answer answer = new Answer { SupportingInformation = textInput };

            string result = answer.SupportingInformationHtml;

            Assert.AreEqual(expectedOutput, result);
        }
    }
}