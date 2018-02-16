using System.Collections.Generic;
using Moq;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Validators;
using NHS111.Web.Controllers;
using NHS111.Web.Helpers;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Logging;
using NUnit.Framework;

namespace NHS111.Web.Presentation.Test.Controllers {

    [TestFixture]
    public class OutcomeControllerTests
    {
        private OutcomeController _outcomeController;

        private Mock<IOutcomeViewModelBuilder> _outcomeViewModelBuilder;
        private Mock<IDOSBuilder> _dosBuilder;
        private Mock<ISurgeryBuilder> _surgeryBuilder;
        private Mock<ILocationResultBuilder> _locationResultBuilder;
        private Mock<IAuditLogger> _auditLogger;
        private Mock<Configuration.IConfiguration> _configuration;
        private Mock<IPostCodeAllowedValidator> _postCodeAllowedValidator;
        private Mock<IViewRouter> _viewRouter;

        [SetUp]
        public void Setup()
        {
            _outcomeViewModelBuilder = new Mock<IOutcomeViewModelBuilder>();
            _dosBuilder = new Mock<IDOSBuilder>();
            _surgeryBuilder = new Mock<ISurgeryBuilder>();
            _locationResultBuilder = new Mock<ILocationResultBuilder>();
            _auditLogger = new Mock<IAuditLogger>();
            _configuration = new Mock<Configuration.IConfiguration>();
            _postCodeAllowedValidator = new Mock<IPostCodeAllowedValidator>();
            _viewRouter = new Mock<IViewRouter>();
            _outcomeController = new OutcomeController(_outcomeViewModelBuilder.Object, _dosBuilder.Object, _surgeryBuilder.Object, _locationResultBuilder.Object, _auditLogger.Object, _configuration.Object, _postCodeAllowedValidator.Object, _viewRouter.Object);
        }

        [Test]
        public void AutoSelectFirstItkService_NoServices_SelectedServiceNull()
        {
            var model = new OutcomeViewModel();
            model.DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
            model.DosCheckCapacitySummaryResult.Success = new SuccessObject<ServiceViewModel>();
            model.DosCheckCapacitySummaryResult.Success.Services = new List<ServiceViewModel>();

            _outcomeController.AutoSelectFirstItkService(model);
            Assert.Null(model.SelectedService);
        }

        [Test]
        public void AutoSelectFirstItkService_OneServicesNotITK_SelectedServiceNull()
        {
            var model = new OutcomeViewModel();
            model.DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
            model.DosCheckCapacitySummaryResult.Success = new SuccessObject<ServiceViewModel>();
            model.DosCheckCapacitySummaryResult.Success.Services = new List<ServiceViewModel>();
            ServiceViewModel svm = new ServiceViewModel {Id = 123456, OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone};
            model.DosCheckCapacitySummaryResult.Success.Services.Add(svm);

            _outcomeController.AutoSelectFirstItkService(model);
            Assert.Null(model.SelectedService);
        }

        [Test]
        public void AutoSelectFirstItkService_OneServicesITK_SelectedServicePopulated()
        {
            var model = new OutcomeViewModel();
            model.DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
            model.DosCheckCapacitySummaryResult.Success = new SuccessObject<ServiceViewModel>();
            model.DosCheckCapacitySummaryResult.Success.Services = new List<ServiceViewModel>();
            ServiceViewModel svm = new ServiceViewModel { Id = 123456, OnlineDOSServiceType = OnlineDOSServiceType.Callback };
            model.DosCheckCapacitySummaryResult.Success.Services.Add(svm);

            _outcomeController.AutoSelectFirstItkService(model);
            Assert.AreEqual(123456, model.SelectedService.Id);
        }

        [Test]
        public void AutoSelectFirstItkService_OneServiceNonITKOneServiceITK_SelectedServicePopulated()
        {
            var model = new OutcomeViewModel();
            model.DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
            model.DosCheckCapacitySummaryResult.Success = new SuccessObject<ServiceViewModel>();
            model.DosCheckCapacitySummaryResult.Success.Services = new List<ServiceViewModel>();
            ServiceViewModel svm1 = new ServiceViewModel { Id = 987654, OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone };
            ServiceViewModel svm2 = new ServiceViewModel { Id = 123456, OnlineDOSServiceType = OnlineDOSServiceType.Callback };
            model.DosCheckCapacitySummaryResult.Success.Services.Add(svm1);
            model.DosCheckCapacitySummaryResult.Success.Services.Add(svm2);

            _outcomeController.AutoSelectFirstItkService(model);
            Assert.AreEqual(123456, model.SelectedService.Id);
        }

        [Test]
        public void AutoSelectFirstItkService_TwoServicesITK_SelectedServicePopulatedWithFirstService()
        {
            var model = new OutcomeViewModel();
            model.DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
            model.DosCheckCapacitySummaryResult.Success = new SuccessObject<ServiceViewModel>();
            model.DosCheckCapacitySummaryResult.Success.Services = new List<ServiceViewModel>();
            ServiceViewModel svm1 = new ServiceViewModel { Id = 987654, OnlineDOSServiceType = OnlineDOSServiceType.Callback };
            ServiceViewModel svm2 = new ServiceViewModel { Id = 123456, OnlineDOSServiceType = OnlineDOSServiceType.Callback };
            model.DosCheckCapacitySummaryResult.Success.Services.Add(svm1);
            model.DosCheckCapacitySummaryResult.Success.Services.Add(svm2);

            _outcomeController.AutoSelectFirstItkService(model);
            Assert.AreEqual(987654, model.SelectedService.Id);
        }
    }
}