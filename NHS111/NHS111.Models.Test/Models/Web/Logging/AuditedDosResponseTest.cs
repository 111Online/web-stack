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
        [Test]
        public void Dosresult_containing_itk_offerring_return_true()
        {
            var response = new AuditedDosResponse()
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
            Assert.IsTrue(response.DosResultsContainItkOfferring);
        }

        [Test]
        public void Dosresult_containing_no_itk_offerring_return_false()
        {
            var response = new AuditedDosResponse()
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
            Assert.IsFalse(response.DosResultsContainItkOfferring);
        }

        [Test]
        public void Dosresult_containing_at_least_one_itk_offerring_return_true()
        {
            var response = new AuditedDosResponse()
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
            Assert.IsTrue(response.DosResultsContainItkOfferring);
        }

        [Test]
        public void Dosresult_containing_multiple_itk_offerring_return_true()
        {
            var response = new AuditedDosResponse()
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
            Assert.IsTrue(response.DosResultsContainItkOfferring);
        }

        [Test]
        public void Dosresult_containing_multiple_not_itk_offerring_return_false()
        {
            var response = new AuditedDosResponse()
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
            Assert.IsFalse(response.DosResultsContainItkOfferring);
        }

        [Test]
        public void Dosresult_containing_error_return_false()
        {
            var response = new AuditedDosResponse()
            {
                Error = new ErrorObject()
            };
            Assert.IsFalse(response.DosResultsContainItkOfferring);
        }
    }
}
