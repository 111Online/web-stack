//TODO fix tests
//using System;
//using NUnit.Framework;
//using System.Web;
//using System.Web.SessionState;
//using System.IO;
//using System.Reflection;

//namespace NHS111.Utils.Test.Session
//{

//    [TestFixture]
//    public class SessionWrapper_Test
//    {

//        [SetUp]
//        public void TestSetup()
//        {
//            // We need to setup the Current HTTP Context as follows:            

//            // Step 1: Setup the HTTP Request
//            var httpRequest = new HttpRequest("", "http://localhost/", "");

//            // Step 2: Setup the HTTP Response
//            var httpResponce = new HttpResponse(new StringWriter());

//            // Step 3: Setup the Http Context
//            var httpContext = new HttpContext(httpRequest, httpResponce);
//            var sessionContainer =
//                new HttpSessionStateContainer("id",
//                                               new SessionStateItemCollection(),
//                                               new HttpStaticObjectsCollection(),
//                                               10,
//                                               true,
//                                               HttpCookieMode.AutoDetect,
//                                               SessionStateMode.InProc,
//                                               false);
//            httpContext.Items["AspSession"] =
//                typeof(HttpSessionState)
//                .GetConstructor(
//                                    BindingFlags.NonPublic | BindingFlags.Instance,
//                                    null,
//                                    CallingConventions.Standard,
//                                    new[] { typeof(HttpSessionStateContainer) },
//                                    null)
//                .Invoke(new object[] { sessionContainer });

//            // Step 4: Assign the Context
//            HttpContext.Current = httpContext;    

//        }

//        [Test]
//        public void should_return_NHSUserGuid_value()
//        {
//            //Arrange
//            var testGuid = Guid.NewGuid().ToString();


//            SessionWrapper.NHSUserGuid = testGuid;

//            //Act
//            string myString = SessionWrapper.NHSUserGuid;

//            //Assert
//            Assert.That(myString, Is.EqualTo(testGuid));
//        }

//        [Test]
//        public void should_return_empty_string_using_null()
//        {
//            //Arrange
//            string testGuid = null;


//            SessionWrapper.NHSUserGuid = testGuid;

//            //Act
//            string myString = SessionWrapper.NHSUserGuid;

//            //Assert
//            Assert.That(myString, Is.EqualTo(""));
//        }

//        [Test]
//        public void should_return_empty_string_using_empty_string()
//        {
//            //Arrange
//            string testGuid = "";


//            SessionWrapper.NHSUserGuid = testGuid;

//            //Act
//            string myString = SessionWrapper.NHSUserGuid;

//            //Assert
//            Assert.That(myString, Is.EqualTo(""));
//        }
//    }
//}
