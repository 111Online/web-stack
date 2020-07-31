using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using AutoMapper;
using FakeItEasy;
using Newtonsoft.Json;
using NHS111.Features;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;
using NUnit.Framework;
using RestSharp;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture]
    public class SurveyLinkViewModelBuilderTests
    {
        private SurveyLinkViewModelBuilder _sut;
        private OutcomeViewModel _outcomeViewModel;
        private List<ServiceViewModel> _services;
        private ISurveyLinkViewModelBuilder _surveyViewLinkModelBuilder;
        private OutcomeViewModel _model;
        private OutcomeViewModel _modelNoService;
        private OutcomeViewModel _modelNoGroupedServices;

        [SetUp]
        public void Setup()
        {
            Mapper.Initialize(m => m.AddProfile<FromServiceViewModelToRecommendedServiceViewModelMapper>());

            var _fakeConfiguration = A.Fake<IConfiguration>();
            var _fakeLoggingRestClient = A.Fake<ILoggingRestClient>();
            var _fakeSurveyLinkFeature = A.Fake<ISurveyLinkFeature>();

            var _fakeRestResponse = A.Fake<IRestResponse<Pathway>>();

            var journeyModel = new Journey
            {
                Steps = new List<JourneyStep> {
                    new JourneyStep {
                        Answer = new Answer{
                            Title = "SomeTitle" }, QuestionNo = "DxC112", QuestionType = QuestionType.String, QuestionId = "", NodeType = NodeType.Question, AnswerInputValue= "123456"}
                }
            };
            _outcomeViewModel = new OutcomeViewModel
            {
                Id = "TestId",
                OutcomeGroup = OutcomeGroup.ItkPrimaryCare,
                JourneyJson = JsonConvert.SerializeObject(journeyModel),
                UserInfo = new UserInfo { Demography = new AgeGenderViewModel { Gender = "Male", Age = 30 } },
                SelectedServiceId = "1",
                DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult
                {
                    Success = new SuccessObject<ServiceViewModel>
                    {
                        Services = new[] 
                        {
                            new ServiceViewModel
                            {
                                Id = 1,
                                ServiceTypeAlias = "Test alias name",
                                ServiceType = new ServiceType
                                {
                                    Id = 1,
                                }
                            }
                        }.ToList()
                    }
                }
            };

            A.CallTo(() => _fakeRestResponse.IsSuccessful).Returns(true);
            A.CallTo(() => _fakeRestResponse.Data).Returns(new Pathway());
            A.CallTo(() => _fakeLoggingRestClient.ExecuteAsync<Pathway>(A<JsonRestRequest>.Ignored)).Returns(_fakeRestResponse);
            A.CallTo(() => _fakeSurveyLinkFeature.PharmacySurveyId).Returns(string.Empty);
            A.CallTo(() => _fakeSurveyLinkFeature.SurveyId).Returns(string.Empty);

            _sut = new SurveyLinkViewModelBuilder(_fakeLoggingRestClient, _fakeConfiguration, _fakeSurveyLinkFeature);

            _services = new List<ServiceViewModel>()
            {
                {
                    new ServiceViewModel()
                    {
                        OnlineDOSServiceType = OnlineDOSServiceType.Callback,
                        ServiceType = new ServiceType()
                        {
                            Id = BookPharmacyCallModelBuilder.EmergencyNationalResponse_ServiceTypeId
                        }
                    }
                }
            };

            _model = new OutcomeViewModel()
            {
                Id = "Dx09",
                GroupedDosServices = new List<GroupedDOSServices>
                {
                    new GroupedDOSServices(OnlineDOSServiceType.Callback, _services)
                },
                OutcomePage = OutcomePage.Outcome,
                OutcomeGroup = OutcomeGroup.ItkPrimaryCare
            };

            _modelNoService = new OutcomeViewModel()
            {
                Id = "Dx09",
                OutcomePage = OutcomePage.Outcome,
                OutcomeGroup = OutcomeGroup.ItkPrimaryCare
            };

            _modelNoGroupedServices = new OutcomeViewModel()
            {
                Id = "Dx09",
                DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult
                {
                    Success = new SuccessObject<ServiceViewModel>
                    {
                        Services = _services
                    }
                },
                OutcomePage = OutcomePage.Outcome,
                OutcomeGroup = OutcomeGroup.ItkPrimaryCare
            };
        }

        [Test]
        public void GuidedSelection_has_correct_value_when_user_has_visited_guided_selection_and_user_selects_none_of_these()
        {
            _outcomeViewModel.ViaGuidedSelection = false;
            var result = _sut.SurveyLinkBuilder(_outcomeViewModel).Result;
            Assert.AreEqual("false", result.GuidedSelection);
        }

        [Test]
        public void GuidedSelection_has_correct_value_when_user_has_visited_guided_selection_and_user_selects_one_symptom()
        {
            _outcomeViewModel.ViaGuidedSelection = true;
            var result = _sut.SurveyLinkBuilder(_outcomeViewModel).Result;
            Assert.AreEqual("true", result.GuidedSelection);
        }

        [Test]
        public void GuidedSelection_has_correct_value_when_user_has_not_visited_guid4ed_selection()
        {
            _outcomeViewModel.ViaGuidedSelection = null;
            var result = _sut.SurveyLinkBuilder(_outcomeViewModel).Result;
            Assert.AreEqual(string.Empty, result.GuidedSelection);
        }

        [Test]
        public void Pharmacy_Services()
        {
            var surveyLinkViewModel = new SurveyLinkViewModel();
            _sut.AddServiceInformation(_model, surveyLinkViewModel);

            Assert.AreEqual(surveyLinkViewModel.BookPharmacyCall, BookPharmacyCallModelBuilder.gp_phcas_no_click);
        }

        [Test]
        public void No_Pharmacy_Services()
        {
            var surveyLinkViewModel = new SurveyLinkViewModel();
            _sut.AddServiceInformation(_modelNoService, surveyLinkViewModel);

            Assert.AreEqual(surveyLinkViewModel.BookPharmacyCall, BookPharmacyCallModelBuilder.gp_nophcas);
        }

        [Test]
        public void No_Grouped_Services()
        {
            var surveyLinkViewModel = new SurveyLinkViewModel();
            _sut.AddServiceInformation(_modelNoGroupedServices, surveyLinkViewModel);

            Assert.AreEqual(BookPharmacyCallModelBuilder.gp_phcas_click, surveyLinkViewModel.BookPharmacyCall);
        }

        [Test]
        public void No_Grouped_DoS_But_Services_Exist()
        {
            var surveyLinkViewModel = new SurveyLinkViewModel();
            _sut.AddServiceInformation(_modelNoGroupedServices, surveyLinkViewModel);

            Assert.AreEqual("Callback", surveyLinkViewModel.ServiceOptions);
            Assert.AreEqual(1, surveyLinkViewModel.ServiceCount);
        }

        [Test]
        public void Grouped_DoS_But_No_Services_Exist()
        {
            var surveyLinkViewModel = new SurveyLinkViewModel();
            _sut.AddServiceInformation(_model, surveyLinkViewModel);

            Assert.AreEqual("Callback", surveyLinkViewModel.ServiceOptions);
            Assert.AreEqual(1, surveyLinkViewModel.ServiceCount);
        }

        [Test]
        public void No_Services_Offered()
        {
            var surveyLinkViewModel = new SurveyLinkViewModel();
            _sut.AddServiceInformation(_modelNoService, surveyLinkViewModel);

            Assert.AreEqual(string.Empty, surveyLinkViewModel.ServiceOptions);
            Assert.AreEqual(0, surveyLinkViewModel.ServiceCount);
        }

        [Test]
        public void ServiceTypeAlias_has_correct_value_when_outcome_is_not_service_first()
        {
            var result = _sut.SurveyLinkBuilder(_outcomeViewModel).Result;
            Assert.AreEqual(string.Empty, result.RecommendedServiceTypeAlias);
        }

        [Test]
        public void ServiceTypeAlias_has_correct_value_when_outcome_service_first()
        {
            _outcomeViewModel.OutcomeGroup = OutcomeGroup.ServiceFirst;
            var result = _sut.SurveyLinkBuilder(_outcomeViewModel).Result;
            Assert.AreEqual("Test alias name", result.RecommendedServiceTypeAlias);
        }

        [Test]
        public void ServiceTypeAlias_has_correct_value_when_outcome_service_first_and_no_results()
        {
            _outcomeViewModel.OutcomeGroup = OutcomeGroup.ServiceFirst;
            _outcomeViewModel.DosCheckCapacitySummaryResult.Success.Services.RemoveAll(s => s.Id == 1);
            var result = _sut.SurveyLinkBuilder(_outcomeViewModel).Result;
            Assert.AreEqual("no-results", result.RecommendedServiceTypeAlias);
        }

        [Test]
        public void ServiceTypeAlias_has_correct_value_when_outcome_service_first_and_dos_errors()
        {
            _outcomeViewModel.OutcomeGroup = OutcomeGroup.ServiceFirst;
            _outcomeViewModel.DosCheckCapacitySummaryResult.Success = null;
            _outcomeViewModel.DosCheckCapacitySummaryResult.Error = new ErrorObject();
            var result = _sut.SurveyLinkBuilder(_outcomeViewModel).Result;
            Assert.AreEqual("no-results", result.RecommendedServiceTypeAlias);
        }

        [Test]
        public void ServiceTypeAlias_has_correct_value_when_ooh_service_first_and_not_callback()
        {
            _outcomeViewModel.OutcomeGroup = OutcomeGroup.ServiceFirst;
            _outcomeViewModel.SelectedService.ServiceType.Id = 25;
            _outcomeViewModel.SelectedService.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            var result = _sut.SurveyLinkBuilder(_outcomeViewModel).Result;
            Assert.AreEqual(string.Empty, result.RecommendedServiceTypeAlias);
        }

        [Test]
        public void ServiceTypeAlias_has_correct_value_when_cas_service_first_and_not_callback()
        {
            _outcomeViewModel.OutcomeGroup = OutcomeGroup.ServiceFirst;
            _outcomeViewModel.SelectedService.ServiceType.Id = 130;
            _outcomeViewModel.SelectedService.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            var result = _sut.SurveyLinkBuilder(_outcomeViewModel).Result;
            Assert.AreEqual(string.Empty, result.RecommendedServiceTypeAlias);
        }
    }
}
