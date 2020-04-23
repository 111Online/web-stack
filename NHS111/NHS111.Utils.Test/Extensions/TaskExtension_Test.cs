using Newtonsoft.Json;
using NHS111.Utils.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NHS111.Utils.Test.Extensions
{
    [TestFixture]
    public class TaskExtension_Test
    {

        [Test]
        public async void should_return_json_using_task_string()
        {
            //Arrange
            Task<string> task = Task.FromResult("test");

            //Act
            Task<string> jsonTask = task.AsJson();

            //Assert
            Assert.That(JsonConvert.DeserializeObject(await jsonTask), Is.EqualTo("test"));

        }


        [Test]
        public async void should_return_json_using_task_list_string()
        {

            //Arrange
            IEnumerable<string> list = new List<string>() { "test", "test2" };
            Task<IEnumerable<string>> task = Task.FromResult(list);

            //Act
            Task<string> jsonTask = task.AsJson();

            var result = JsonConvert.DeserializeObject<List<string>>(await jsonTask);
            //Assert
            Assert.That(result[0], Is.EqualTo("test"));
            Assert.That(result[1], Is.EqualTo("test2"));

        }

        [Test]
        public async void should_return_json_using_null()
        {

            //Arrange
            IEnumerable<string> list = null;
            Task<IEnumerable<string>> task = Task.FromResult(list);

            //Act
            Task<string> jsonTask = task.AsJson();

            var result = JsonConvert.DeserializeObject<List<string>>(await jsonTask);

            //Assert
            Assert.That(result, Is.Null);

        }

        [Test]
        public async void should_return_HttpResponseMessage()
        {

            //Arrange
            Task<string> mytask = Task.FromResult("test");

            //Act
            Task<HttpResponseMessage> task = mytask.AsHttpResponse();
            var result = await task;

            //Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            HttpContent myHttpContent = result.Content;
            var myHttpContentResult = await myHttpContent.ReadAsStringAsync();
            Assert.That(myHttpContentResult, Is.EqualTo("test"));

        }


        [Test]
        public async void should_return_FirstOrDefault()
        {

            //Arrange
            IEnumerable<string> list = new List<string>() { "test", "test2" };
            Task<IEnumerable<string>> taskList = Task.FromResult(list);

            //Act
            Task<string> task = taskList.FirstOrDefault();
            var result = await task;

            //Assert
            Assert.That(result, Is.EqualTo("test"));
        }

        [Test]
        public async void should_return_FirstOrDefault_using_empty_list()
        {
            //Arrange
            IEnumerable<string> list = new List<string>();
            Task<IEnumerable<string>> taskList = Task.FromResult(list);

            //Act
            Task<string> task = taskList.FirstOrDefault();
            string result = await task;

            //Assert
            Assert.IsNull(result);
        }
    }


}
