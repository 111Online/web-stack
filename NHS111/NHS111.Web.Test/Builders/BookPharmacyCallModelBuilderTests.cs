namespace NHS111.Web.Presentation.Builders.Tests
{
    using NHS111.Models.Models.Web;
    using NHS111.Models.Models.Web.Enums;
    using NHS111.Models.Models.Web.FromExternalServices;
    using NUnit.Framework;
    using System.Collections.Generic;
    using NHS111.Web.Presentation.Builders;

    [TestFixture]
    public class BookPharmacyCallModelBuilderTests
    {
        private ServiceViewModel _nerService;
        private ServiceViewModel _otherService;

        private List<ServiceViewModel> _servicesWithNer;
        private List<ServiceViewModel> _servicesWithoutNer;

        private List<string> _otherDispositions = new List<string>
        {
            "Dx0112",
            "Dx32",
            "Dx38"
        };

        private List<string> _urgentDispositions = new List<string>
        {
            BookPharmacyCallModelBuilder.Dx06,
            BookPharmacyCallModelBuilder.Dx07,
            BookPharmacyCallModelBuilder.Dx08,
            BookPharmacyCallModelBuilder.Dx14,
            BookPharmacyCallModelBuilder.Dx15
        };

        private List<string> _nonUrgentDispositions = new List<string>
        {
            BookPharmacyCallModelBuilder.Dx09,
            BookPharmacyCallModelBuilder.Dx10,
            BookPharmacyCallModelBuilder.Dx16,
            BookPharmacyCallModelBuilder.Dx75
        };

        private List<string> _pharmDispositions = new List<string>
        {
            BookPharmacyCallModelBuilder.Dx28
        };

        [SetUp]
        public void Setup()
        {
            _nerService = new ServiceViewModel()
            {
                OnlineDOSServiceType = OnlineDOSServiceType.Callback,
                ServiceType = new ServiceType()
                {
                    Id = BookPharmacyCallModelBuilder.EmergencyNationalResponse_ServiceTypeId
                }
            };

            _otherService = new ServiceViewModel()
            {
                OnlineDOSServiceType = OnlineDOSServiceType.GoTo,
                ServiceType = new ServiceType()
                {
                    Id = 100
                }
            };

            _servicesWithNer = new List<ServiceViewModel>
            {
                _nerService, 
                _otherService
            };

            _servicesWithoutNer = new List<ServiceViewModel>
            {
                _otherService
            };
        }

        [TestCaseSource("_otherDispositions")]
        public void Outcome_With_NER(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, -1, _servicesWithNer, true);
            Assert.AreEqual(string.Empty, value);
        }

        [TestCaseSource("_otherDispositions")]
        public void Outcome_Without_NER(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, -1, _servicesWithoutNer, true);
            Assert.AreEqual(string.Empty, value);
        }

        [TestCaseSource("_otherDispositions")]
        public void Outcome_With_NER_Selected_Service(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, 100, _servicesWithNer, true);
            Assert.AreEqual(string.Empty, value);
        }

        [TestCaseSource("_otherDispositions")]
        public void Outcome_Without_NER_Selected_Service(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, 100, _servicesWithNer, true);
            Assert.AreEqual(string.Empty, value);
        }

        [TestCaseSource("_urgentDispositions")]
        public void Urgent_Callback_Outcome_With_NER(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, -1, _servicesWithNer, true);
            Assert.AreEqual(string.Empty, value);
        }

        [TestCaseSource("_urgentDispositions")]
        public void Urgent_Callback_Outcome_Without_NER(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, -1, _servicesWithoutNer, true);
            Assert.AreEqual(string.Empty, value);
        }

        [TestCaseSource("_nonUrgentDispositions")]
        public void Non_Urgent_Callback_Outcome_With_NER(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, -1, _servicesWithNer, true);
            Assert.AreEqual(BookPharmacyCallModelBuilder.gp_phcas_no_click, value);
        }

        [TestCaseSource("_nonUrgentDispositions")]
        public void Non_Urgent_Callback_Outcome_Without_NER(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, -1, _servicesWithoutNer, true);
            Assert.AreEqual(BookPharmacyCallModelBuilder.gp_nophcas, value);
        }

        [TestCaseSource("_pharmDispositions")]
        public void Pharm_Callback_Outcome_With_NER(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, -1, _servicesWithNer, false);
            Assert.AreEqual(BookPharmacyCallModelBuilder.ph_phcas_no_click, value);
        }

        [TestCaseSource("_pharmDispositions")]
        public void Pharm_Callback_Outcome_Without_NER(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, -1, _servicesWithoutNer, false);
            Assert.AreEqual(BookPharmacyCallModelBuilder.ph_nophcas, value);
        }

        [TestCaseSource("_urgentDispositions")]
        public void Urgent_Callback_Outcome_NoResults(string dispositionCode)
        {
            var services = new List<ServiceViewModel>() { };
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, -1, services, true);
            Assert.AreEqual(string.Empty, value);
        }

        [TestCaseSource("_nonUrgentDispositions")]
        public void Non_Urgent_Callback_Outcome_NoResults(string dispositionCode)
        {
            var services = new List<ServiceViewModel>() { };
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, -1, services, true);
            Assert.AreEqual(BookPharmacyCallModelBuilder.gp_nophcas, value);
        }

        [TestCaseSource("_pharmDispositions")]
        public void Pharm_Callback_Outcome_NoSearch(string dispositionCode)
        {
            var services = new List<ServiceViewModel> { };
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, -1, services, false);
            Assert.AreEqual(string.Empty, value);
        }

        [TestCaseSource("_urgentDispositions")]
        public void Urgent_Callback_Outcome_With_NER_Clicked(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, 138, _servicesWithNer, true);
            Assert.AreEqual(BookPharmacyCallModelBuilder.gp_phcas_click, value);
        }

        [TestCaseSource("_urgentDispositions")]
        public void Urgent_Callback_Outcome_With_NER_NotClicked(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, 100, _servicesWithNer, true);
            Assert.AreEqual(BookPharmacyCallModelBuilder.gp_phcas_no_click, value);
        }

        [TestCaseSource("_urgentDispositions")]
        public void Urgent_Callback_Outcome_Without_NER_Clicked(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, 100, _servicesWithoutNer, true);
            Assert.AreEqual(string.Empty, value);
        }

        [TestCaseSource("_nonUrgentDispositions")]
        public void Non_Urgent_Callback_Outcome_With_NER_Clicked(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, 138, _servicesWithNer, true);
            Assert.AreEqual(BookPharmacyCallModelBuilder.gp_phcas_click, value);
        }

        [TestCaseSource("_nonUrgentDispositions")]
        public void Non_Urgent_Callback_Outcome_With_NER_NotClicked(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, 100, _servicesWithNer, true);
            Assert.AreEqual(BookPharmacyCallModelBuilder.gp_phcas_no_click, value);
        }

        [TestCaseSource("_nonUrgentDispositions")]
        public void Non_Urgent_Callback_Outcome_Without_NER_Clicked(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, 100, _servicesWithoutNer, true);
            Assert.AreEqual(BookPharmacyCallModelBuilder.gp_nophcas, value);
        }

        [TestCaseSource("_pharmDispositions")]
        public void Pharm_Callback_Outcome_With_NER_Clicked(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, 138, _servicesWithNer, false);
            Assert.AreEqual(BookPharmacyCallModelBuilder.ph_phcas_click, value);
        }

        [TestCaseSource("_pharmDispositions")]
        public void Pharm_Callback_Outcome_With_NER_NotClicked(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, 100, _servicesWithNer, false);
            Assert.AreEqual(BookPharmacyCallModelBuilder.ph_phcas_no_click, value);
        }

        [TestCaseSource("_pharmDispositions")]
        public void Pharm_Callback_Outcome_Without_NER_Clicked(string dispositionCode)
        {
            var value = BookPharmacyCallModelBuilder.BookPharmacyCallValue(dispositionCode, 100, _servicesWithoutNer, false);
            Assert.AreEqual(BookPharmacyCallModelBuilder.ph_nophcas, value);
        }
    }
}
