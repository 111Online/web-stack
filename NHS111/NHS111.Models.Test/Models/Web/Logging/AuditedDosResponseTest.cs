using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Logging;
using NUnit.Framework;
using System.Collections.Generic;

namespace NHS111.Models.Test.Models.Web.Logging
{

    [TestFixture]
    public class AuditedDosResponseTest
    {
        [TestFixtureSetUp]
        public void InitializeJourneyViewModelMapper()
        {
            Mapper.Initialize(m => m.AddProfile<NHS111.Models.Mappers.WebMappings.AuditedModelMappers>());
        }

        [Test]
        public void AuditedModelMappers_Configuration_IsValid_Test()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void Dosresult_containing_itk_offerring_return_true()
        {
            var result = new DosCheckCapacitySummaryResult
            {
                Success = new SuccessObject<ServiceViewModel>
                {
                    Services = new List<ServiceViewModel>()
                    {
                        new ServiceViewModel()
                        {
                            OnlineDOSServiceType = OnlineDOSServiceType.Callback
                        }
                    }
                }
            };
            var audit = Mapper.Map<DosCheckCapacitySummaryResult, AuditedDosResponse>(result);
            Assert.IsTrue(audit.DosResultsContainItkOfferring);
        }

        [Test]
        public void Dosresult_containing_no_itk_offerring_return_false()
        {
            var result = new DosCheckCapacitySummaryResult
            {
                Success = new SuccessObject<ServiceViewModel>
                {
                    Services = new List<ServiceViewModel>()
                    {
                        new ServiceViewModel()
                        {
                            OnlineDOSServiceType = OnlineDOSServiceType.GoTo
                        }
                    }
                }
            };
            var audit = Mapper.Map<DosCheckCapacitySummaryResult, AuditedDosResponse>(result);
            Assert.IsFalse(audit.DosResultsContainItkOfferring);
        }

        [Test]
        public void Dosresult_containing_at_least_one_itk_offerring_return_true()
        {
            var result = new DosCheckCapacitySummaryResult
            {
                Success = new SuccessObject<ServiceViewModel>
                {
                    Services = new List<ServiceViewModel>()
                    {
                        new ServiceViewModel()
                        {
                            OnlineDOSServiceType = OnlineDOSServiceType.GoTo
                        },
                        new ServiceViewModel()
                        {
                            OnlineDOSServiceType = OnlineDOSServiceType.Callback
                        },
                        new ServiceViewModel()
                        {
                            OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone
                        }
                    }
                }
            };
            var audit = Mapper.Map<DosCheckCapacitySummaryResult, AuditedDosResponse>(result);
            Assert.IsTrue(audit.DosResultsContainItkOfferring);
        }

        [Test]
        public void Dosresult_containing_multiple_itk_offerring_return_true()
        {
            var result = new DosCheckCapacitySummaryResult
            {
                Success = new SuccessObject<ServiceViewModel>
                {
                    Services = new List<ServiceViewModel>()
                    {
                        new ServiceViewModel()
                        {
                            OnlineDOSServiceType = OnlineDOSServiceType.Callback
                        },
                        new ServiceViewModel()
                        {
                            OnlineDOSServiceType = OnlineDOSServiceType.Callback
                        },
                        new ServiceViewModel()
                        {
                            OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone
                        }
                    }
                }
            };
            var audit = Mapper.Map<DosCheckCapacitySummaryResult, AuditedDosResponse>(result);
            Assert.IsTrue(audit.DosResultsContainItkOfferring);
        }

        [Test]
        public void Dosresult_containing_multiple_not_itk_offerring_return_false()
        {
            var result = new DosCheckCapacitySummaryResult
            {
                Success = new SuccessObject<ServiceViewModel>
                {
                    Services = new List<ServiceViewModel>()
                    {
                        new ServiceViewModel()
                        {
                            OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone
                        },
                        new ServiceViewModel()
                        {
                            OnlineDOSServiceType = OnlineDOSServiceType.GoTo
                        },
                        new ServiceViewModel()
                        {
                            OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone
                        }
                    }
                }
            };
            var audit = Mapper.Map<DosCheckCapacitySummaryResult, AuditedDosResponse>(result);
            Assert.IsFalse(audit.DosResultsContainItkOfferring);
        }

        [Test]
        public void Dosresult_containing_error_return_false()
        {
            var result = new DosCheckCapacitySummaryResult
            {
                Error = new ErrorObject()
            };
            var audit = Mapper.Map<DosCheckCapacitySummaryResult, AuditedDosResponse>(result);
            Assert.IsFalse(audit.DosResultsContainItkOfferring);
        }

        [Test]
        public void Dosresult_containing_success_return_success_code()
        {
            var result = new DosCheckCapacitySummaryResult
            {
                Success = new SuccessObject<ServiceViewModel>
                {
                    Code = 200
                }
            };
            var audit = Mapper.Map<DosCheckCapacitySummaryResult, AuditedDosResponse>(result);
            Assert.AreEqual(200, audit.Success.Code);
        }

        [Test]
        public void Dosresult_containing_error_return_error_code()
        {
            var result = new DosCheckCapacitySummaryResult
            {
                Error = new ErrorObject()
                {
                    Code = 404
                }
            };
            var audit = Mapper.Map<DosCheckCapacitySummaryResult, AuditedDosResponse>(result);
            Assert.AreEqual(404, audit.Error.Code);
        }
    }
}
