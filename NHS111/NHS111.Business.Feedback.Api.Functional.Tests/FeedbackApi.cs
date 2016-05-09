
using System.Configuration;

namespace NHS111.Business.Feedback.Api.Functional.Tests {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Script.Serialization;
    using Models.Models.Domain;
    using NUnit.Framework;

    [TestFixture]
    public class FeedbackApi {

        //private string _feedbackApiDomain = "http://nhs111businessfeedbackapi.azurewebsites.net/";
        
        private RestfulHelper _restfulHelper = new RestfulHelper();

        //expected fields: https://trello.com/c/0TxfGnbn/16-feedback-mechanism-on-individual-questions
        private Feedback _testFeedback = new Feedback {
            DateAdded = DateTime.UtcNow, //Consumers shouldn't be expected to provide this!
            EmailAddress = "example@test.com",
            JSonData = "{ id: \"" + Guid.NewGuid() + "\" }",
            PageId = "SomePage",
            Rating = 5,
            Text = "Some feedback",
            UserId = "123"
        };

        private JavaScriptSerializer _javaScriptSerializer = new JavaScriptSerializer();

        [Test]
        public void AddAndThenList_Always_ReturnsAddedData() {
            AddFeedback(_testFeedback);
            Thread.Sleep(10000);
            var list = ListFeedback();

            AssertExpectedFieldsPresent(list, _testFeedback);
        }

        private static HttpRequestMessage CreateHttpRequest(string requestContent)
        {
            var message = new HttpRequestMessage
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };

            message.Headers.Add( "Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("nhsUser:oD4rqw4Ntr")));
            return message;
        }

        private async void AddFeedback(Feedback feedback) {
            var addEndpoint = "add";
            var endpoint = ConfigurationManager.AppSettings["FeedbackUrl"] + addEndpoint;

            var request = CreateHttpRequest(_javaScriptSerializer.Serialize(feedback));
            await _restfulHelper.PostAsync(endpoint, request);
        }

        private List<Feedback> ListFeedback() {
            var listEndpoint = "list";
            var endpoint = ConfigurationManager.AppSettings["FeedbackUrl"] + listEndpoint;

            var result = _restfulHelper.GetAsync(endpoint);

            Assert.IsNotNull(result);

            return _javaScriptSerializer.Deserialize<List<Feedback>>(result.Result);
        }

        private static void AssertExpectedFieldsPresent(List<Feedback> list, Feedback testFeedback)
        {
            Assert.Greater(list.Count, 0,
                "No feedbacks were returned from the list endpoint even after invoking the add endpoint.");

            var feedback = list.FirstOrDefault(f => f.JSonData == testFeedback.JSonData);
            Assert.IsNotNull(feedback, "Feedback added via add endpoint not found in subsequent list retrieval.");

            AssertJSONDateEqual(testFeedback, feedback);
            Assert.AreEqual(testFeedback.JSonData, feedback.JSonData);
            Assert.AreEqual(testFeedback.PageId, feedback.PageId);
            Assert.AreEqual(testFeedback.Rating, feedback.Rating);
            Assert.AreEqual(testFeedback.Text, feedback.Text);
            Assert.AreEqual(testFeedback.UserId, feedback.UserId);
        }

        private static void AssertJSONDateEqual(Feedback testFeedback, Feedback feedback) {
            //JSON serialisation lacks the precision of .NET's, see https://blogs.msdn.microsoft.com/rickandy/2012/02/03/json-serialization-deserialization-of-datetime-not-equal/
            Assert.AreEqual(testFeedback.DateAdded.Year, feedback.DateAdded.Year);
            Assert.AreEqual(testFeedback.DateAdded.Month, feedback.DateAdded.Month);
            Assert.AreEqual(testFeedback.DateAdded.Day, feedback.DateAdded.Day);
            Assert.AreEqual(testFeedback.DateAdded.Hour, feedback.DateAdded.Hour);
            Assert.AreEqual(testFeedback.DateAdded.Minute, feedback.DateAdded.Minute);
            Assert.AreEqual(testFeedback.DateAdded.Second, feedback.DateAdded.Second);
        }
    }
}
