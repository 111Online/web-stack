using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Converters;
using NUnit.Framework;
namespace NHS111.Utils.Converters.Tests
{

    
    [TestFixture()]
    public class FeedbackConverterTests
    {
        private const string TEST_DATE_VALUE = "2016-08-19 13:51:06";
        private const string TEST_FEEDBACK_VALUE = "Here is some feedback.";
        private const string TEST_USERID_VALUE = "Test_Id_01";
        private const string TEST_PAGEID_VALUE = "/question/next";
        private const string TEST_RATING_VALUE = "8";
        private const string TEST_JSON_VALUE = "{\"feedback\": { \"field1\":\"field1 value\",\"field2\":\"field2 value\"  }}";

        private IDataReader MockIDataReader()
        {
            var moq = new Mock<IDataReader>();
            bool readToggle = true;
            moq.Setup(x => x.Read())  
                .Returns(() => readToggle)
                .Callback(() => readToggle = false);

            moq.Setup(x => x["email"])
                .Returns("test@test.com");

            moq.Setup(x => x[FeedbackConverter.FEEDBACK_DATETIME_FIELDNAME])
                .Returns(TEST_DATE_VALUE);

            moq.Setup(x => x[FeedbackConverter.FEEDBACKTEXT_FIELDNAME])
                .Returns(TEST_FEEDBACK_VALUE);

            moq.Setup(x => x[FeedbackConverter.USERID_FIELDNAME])
                .Returns(TEST_USERID_VALUE);

            moq.Setup(x => x[FeedbackConverter.PAGE_ID_FIELDNAME])
                .Returns(TEST_PAGEID_VALUE);

            moq.Setup(x => x[FeedbackConverter.RATING_FIELDNAME])
                .Returns(TEST_RATING_VALUE);

            moq.Setup(x => x[FeedbackConverter.FEEDBACK_DATA_FIELDNAME])
                .Returns(TEST_JSON_VALUE);
            return moq.Object;
        }

        [Test()]
        public void ConvertToFeedback_empty_reader_Test()
        {
            var moq = new Mock<IDataReader>();
            bool readToggle = true;
            moq.Setup(x => x.Read())
                .Returns(() => readToggle)
                .Callback(() => readToggle = false);
            
            var converter = new FeedbackConverter();

            var result = converter.Convert(moq.Object);
            Assert.IsNotNull(result);
        }

        [Test()]
        public void ConvertToStatementParamaters_empty_feedback_Test()
        {
            var feedback = new Feedback();

            var converter = new FeedbackConverter();

            var result = converter.Convert(feedback);
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test()]
        public void ConvertToFeedbackTest()
        {
            var converter = new FeedbackConverter();
            var testReader = MockIDataReader();

            var result = converter.Convert(testReader);

            Assert.IsNotNull(result);
            Assert.AreEqual(TEST_FEEDBACK_VALUE, result.Text);
            Assert.AreEqual(TEST_PAGEID_VALUE, result.PageId);
            Assert.AreEqual(new DateTime(2016,8,19,13,51,6), result.DateAdded);
            Assert.AreEqual(8, result.Rating.Value);
            Assert.AreEqual(TEST_JSON_VALUE, result.JSonData);
            Assert.AreEqual(TEST_USERID_VALUE, result.UserId);
        }

        [Test()]
        public void ConvertToStatementParamatersTest()
        {
            var converter = new FeedbackConverter();

            var testFeedback = new Feedback()
            {
                DateAdded = new DateTime(2016, 8, 19, 13, 51, 6),
                JSonData = TEST_JSON_VALUE,
                PageId = TEST_PAGEID_VALUE,
                Rating = 8,
                Text = TEST_FEEDBACK_VALUE,
                UserId = TEST_USERID_VALUE
            };

            var result = converter.Convert(testFeedback);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);

            Assert.AreEqual(result[FeedbackConverter.USERID_FIELDNAME],TEST_USERID_VALUE);
            Assert.AreEqual(result[FeedbackConverter.FEEDBACKTEXT_FIELDNAME],TEST_FEEDBACK_VALUE);
            Assert.AreEqual(result[FeedbackConverter.PAGE_ID_FIELDNAME],TEST_PAGEID_VALUE);
            Assert.AreEqual(result[FeedbackConverter.FEEDBACK_DATETIME_FIELDNAME],TEST_DATE_VALUE);
            Assert.AreEqual(result[FeedbackConverter.RATING_FIELDNAME], TEST_RATING_VALUE);

            Assert.AreEqual(result[FeedbackConverter.FEEDBACK_DATA_FIELDNAME], TEST_JSON_VALUE);


        }
    }
}
