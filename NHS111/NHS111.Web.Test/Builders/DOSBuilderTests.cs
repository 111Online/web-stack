﻿using System.Collections.Generic;
using AutoMapper;
using Moq;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Cache;
using NHS111.Utils.Helpers;
using NHS111.Utils.Notifier;
using NUnit.Framework;
namespace NHS111.Web.Presentation.Builders.Tests
{
    [TestFixture()]
    public class DOSBuilderTests
    {

        private Mock<IMappingEngine> _mappingEngine;
        private Mock<ICareAdviceBuilder> _mockCareAdviceBuilder;
        private Mock<IRestfulHelper> _mockRestfulHelper;
        private Mock<Configuration.IConfiguration> _mockConfiguration;
        private Mock<ICacheManager<string, string>> _mockCacheManager;
        private Mock<INotifier<string>> _mockNotifier;
        private DOSBuilder _dosBuilder;
        private Mock<ISurgeryBuilder> _mockSurgeryBuilder;

        private string _mockPathwayURL = "PW755";

        private string _expectedBusinessApiPathwaySymptomGroupUrl;
    
        [SetUp()]
        public void Setup()
        {
            _mappingEngine = new Mock<IMappingEngine>();
            _mockCareAdviceBuilder = new Mock<ICareAdviceBuilder>();
            _mockRestfulHelper = new Mock<IRestfulHelper>();
            _mockConfiguration = new Mock<Configuration.IConfiguration>();
            _mockCacheManager = new Mock<ICacheManager<string, string>>();
            _mockNotifier = new Mock<INotifier<string>>();

            SetupMockFillCareAdviceBuilder();

            SetupMockConfiguration();

            _dosBuilder = new DOSBuilder(_mockCareAdviceBuilder.Object, 
                _mockRestfulHelper.Object, 
                _mockConfiguration.Object, 
                _mappingEngine.Object, _mockCacheManager.Object,
                _mockNotifier.Object);
        }

        private void SetupMockConfiguration()
        {
            _mockConfiguration.SetupGet(c => c.DosUsername).Returns("TestUsername");
            _mockConfiguration.SetupGet(c => c.DosPassword).Returns("TestPassword");
            _expectedBusinessApiPathwaySymptomGroupUrl = "http://Test.ApiPathwaySymptomGroupUrl.com/" + _mockPathwayURL;
            _mockConfiguration.Setup(c => c.GetBusinessApiPathwaySymptomGroupUrl(_mockPathwayURL)).Returns(_expectedBusinessApiPathwaySymptomGroupUrl);
        }

        private void SetupMockFillCareAdviceBuilder()
        {
            var mockCareAdvices = new List<CareAdvice>()
            {
                new CareAdvice() {Title = "TestAdvice", Id = "CA123", Items = new List<string>() {"Test advice text"}}
            };


            _mockCareAdviceBuilder.Setup(
                cb =>
                    cb.FillCareAdviceBuilder(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<List<string>>())).ReturnsAsync(mockCareAdvices);
        }

     

        [Test()]
        public async void BuildCheckCapacitySummaryRequest_Creates_SymptomGroup_Test()
        {
            var expectedSymptomGroup = "12345";
            var journeyJson = "{'steps':[" +
                              "{'answer':{'title':'No','titleWithoutSpaces':'No','symptomDiscriminator':'','supportingInfo':'','keywords':'','order':3},'questionTitle':'Test q 1?','questionNo':'Tx1506','questionId':'" + _mockPathwayURL + ".0','jtbs':false}," +
                              "{'answer':{'title':'No','titleWithoutSpaces':'No','symptomDiscriminator':'','supportingInfo':'','keywords':'','order':3},'questionTitle':'Test q 2?','questionNo':'Tx220054','questionId':'" + _mockPathwayURL + ".100','jtbs':false}" +
                              "]}";

            MockRestfulHelperWithExpectedUrl(expectedSymptomGroup);

            var symptomGroup = await _dosBuilder.BuildSymptomGroup(journeyJson);

             Assert.AreEqual(int.Parse(expectedSymptomGroup), symptomGroup);


        }

        private void MockRestfulHelperWithExpectedUrl(string expectedSymptomGroup)
        {
            _mockRestfulHelper.Setup(r => r.GetAsync(_expectedBusinessApiPathwaySymptomGroupUrl)).ReturnsAsync(expectedSymptomGroup);
        }

    }
}
