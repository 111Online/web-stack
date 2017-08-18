using System.Collections.Generic;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Controllers;
using NUnit.Framework;

namespace NHS111.Web.Presentation.Test.Controllers {

    [TestFixture]
    public class OutcomeControllerTests {

        [Test]
        public void AutoSelectFirstItkService_NoServices_SelectedServiceNull()
        {
            var model = new OutcomeViewModel();
            model.DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
            model.DosCheckCapacitySummaryResult.Success = new SuccessObject<ServiceViewModel>();
            model.DosCheckCapacitySummaryResult.Success.Services = new List<ServiceViewModel>();

            OutcomeController.AutoSelectFirstItkService(model);
            Assert.Null(model.SelectedService);
        }

        [Test]
        public void AutoSelectFirstItkService_OneServicesNotITK_SelectedServiceNull()
        {
            var model = new OutcomeViewModel();
            model.DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
            model.DosCheckCapacitySummaryResult.Success = new SuccessObject<ServiceViewModel>();
            model.DosCheckCapacitySummaryResult.Success.Services = new List<ServiceViewModel>();
            ServiceViewModel svm = new ServiceViewModel {Id = 123456, CallbackEnabled = false};
            model.DosCheckCapacitySummaryResult.Success.Services.Add(svm);
            
            OutcomeController.AutoSelectFirstItkService(model);
            Assert.Null(model.SelectedService);
        }

        [Test]
        public void AutoSelectFirstItkService_OneServicesITK_SelectedServicePopulated()
        {
            var model = new OutcomeViewModel();
            model.DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
            model.DosCheckCapacitySummaryResult.Success = new SuccessObject<ServiceViewModel>();
            model.DosCheckCapacitySummaryResult.Success.Services = new List<ServiceViewModel>();
            ServiceViewModel svm = new ServiceViewModel { Id = 123456, CallbackEnabled = true };
            model.DosCheckCapacitySummaryResult.Success.Services.Add(svm);

            OutcomeController.AutoSelectFirstItkService(model);
            Assert.AreEqual(123456, model.SelectedService.Id);
        }

        [Test]
        public void AutoSelectFirstItkService_OneServiceNonITKOneServiceITK_SelectedServicePopulated()
        {
            var model = new OutcomeViewModel();
            model.DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
            model.DosCheckCapacitySummaryResult.Success = new SuccessObject<ServiceViewModel>();
            model.DosCheckCapacitySummaryResult.Success.Services = new List<ServiceViewModel>();
            ServiceViewModel svm1 = new ServiceViewModel { Id = 987654, CallbackEnabled = false };
            ServiceViewModel svm2 = new ServiceViewModel { Id = 123456, CallbackEnabled = true };
            model.DosCheckCapacitySummaryResult.Success.Services.Add(svm1);
            model.DosCheckCapacitySummaryResult.Success.Services.Add(svm2);
            
            OutcomeController.AutoSelectFirstItkService(model);
            Assert.AreEqual(123456, model.SelectedService.Id);
        }

        [Test]
        public void AutoSelectFirstItkService_TwoServicesITK_SelectedServicePopulatedWithFirstService()
        {
            var model = new OutcomeViewModel();
            model.DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
            model.DosCheckCapacitySummaryResult.Success = new SuccessObject<ServiceViewModel>();
            model.DosCheckCapacitySummaryResult.Success.Services = new List<ServiceViewModel>();
            ServiceViewModel svm1 = new ServiceViewModel { Id = 987654, CallbackEnabled = true };
            ServiceViewModel svm2 = new ServiceViewModel { Id = 123456, CallbackEnabled = true };
            model.DosCheckCapacitySummaryResult.Success.Services.Add(svm1);
            model.DosCheckCapacitySummaryResult.Success.Services.Add(svm2);

            OutcomeController.AutoSelectFirstItkService(model);
            Assert.AreEqual(987654, model.SelectedService.Id);
        }
    }
}