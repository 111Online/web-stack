using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static NHS111.Models.Models.Web.RecommendedServiceViewModelExtensions;

namespace NHS111.Models.Test.Models.Web
{
    [TestFixture]
    public class OtherServiceViewModelTests
    {
        [Test]
        public void OnlyServiceIsPharmacyCAS_Returns_False_When_Empty()
        {
            var otherServices = Enumerable.Empty<RecommendedServiceViewModel>();
            Assert.IsFalse(otherServices.OnlyServiceIsPharmacyCAS());
        }

        [Test]
        public void OnlyServiceIsPharmacyCAS_Returns_False_When_Not_PharmacyCAS()
        {
            var otherServices = new[] { new RecommendedServiceViewModel { ServiceType = new ServiceType { Id = -1 } } };
            Assert.IsFalse(otherServices.OnlyServiceIsPharmacyCAS());
        }

        [Test]
        public void OnlyServiceIsPharmacyCAS_138_Returns_True_When_PharmacyCAS()
        {
            var otherServices = new[] { new RecommendedServiceViewModel { ServiceType = new ServiceType { Id = 138 } } };
            Assert.IsTrue(otherServices.OnlyServiceIsPharmacyCAS());
        }

        [Test]
        public void OnlyServiceIsPharmacyCAS_137_Returns_True_When_PharmacyCAS()
        {
            var otherServices = new[] { new RecommendedServiceViewModel { ServiceType = new ServiceType { Id = 137 } } };
            Assert.IsTrue(otherServices.OnlyServiceIsPharmacyCAS());
        }

        [Test]
        public void OnlyServiceIsPharmacyCAS_Returns_False_When_GreaterThanOneService()
        {
            var otherServices = new[] 
            { 
                new RecommendedServiceViewModel { ServiceType = new ServiceType { Id = -1 } },
                new RecommendedServiceViewModel { ServiceType = new ServiceType { Id = 138 } }
            };
            Assert.IsFalse(otherServices.OnlyServiceIsPharmacyCAS());
        }

        [Test]
        public void GetServiceTypeOffered_Returns_None_When_NoServices()
        {
            var otherServices = Enumerable.Empty<RecommendedServiceViewModel>();
            var serviceTypeOfferred = otherServices.GetServiceTypeOffered();
            Assert.AreEqual(ServiceTypeOffered.None, serviceTypeOfferred);
        }

        [Test]
        public void GetServiceTypeOffered_Returns_None_When_NoPharmaCASOrITKService()
        {
            var otherServices = new[]
            {
                new RecommendedServiceViewModel { ServiceType = new ServiceType { Id = -1 } }
            };
            var serviceTypeOfferred = otherServices.GetServiceTypeOffered();
            Assert.AreEqual(ServiceTypeOffered.None, serviceTypeOfferred);
        }

        [Test]
        public void GetServiceTypeOffered_Returns_PharmaCAS_When_ContainsPharmaCASService()
        {
            var otherServices = new[]
            {
                new RecommendedServiceViewModel 
                {
                    OnlineDOSServiceType = OnlineDOSServiceType.Callback,
                    ServiceType = new ServiceType { Id = 138 } 
                }
            };
            var serviceTypeOfferred = otherServices.GetServiceTypeOffered();
            Assert.AreEqual(ServiceTypeOffered.PharmaCAS, serviceTypeOfferred);
        }

        [Test]
        public void GetServiceTypeOffered_Returns_ITK_ReferRingAndGo_When_ContainsITKService()
        {
            var otherServices = new[]
            {
                new RecommendedServiceViewModel 
                { 
                    OnlineDOSServiceType = OnlineDOSServiceType.ReferRingAndGo,
                    ServiceType = new ServiceType { Id = 44 }
                }
            };
            var serviceTypeOfferred = otherServices.GetServiceTypeOffered();
            Assert.AreEqual(ServiceTypeOffered.ITK, serviceTypeOfferred);
        }

        [Test]
        public void GetServiceTypeOffered_Returns_ITK_Callback_When_ContainsITKService()
        {
            var otherServices = new[]
            {
                new RecommendedServiceViewModel
                {
                    OnlineDOSServiceType = OnlineDOSServiceType.Callback,
                    ServiceType = new ServiceType { Id = 44 }
                }
            };
            var serviceTypeOfferred = otherServices.GetServiceTypeOffered();
            Assert.AreEqual(ServiceTypeOffered.ITK, serviceTypeOfferred);
        }

        [Test]
        public void GetServiceTypeOffered_Returns_Both_When_ContainsITKAndPharmaCASService()
        {
            var otherServices = new[]
            {
                new RecommendedServiceViewModel
                {
                    OnlineDOSServiceType = OnlineDOSServiceType.Callback,
                    ServiceType = new ServiceType { Id = 44 }
                },
                new RecommendedServiceViewModel
                {
                    OnlineDOSServiceType = OnlineDOSServiceType.Callback,
                    ServiceType = new ServiceType { Id = 138 }
                }
            };
            var serviceTypeOfferred = otherServices.GetServiceTypeOffered();
            Assert.AreEqual(ServiceTypeOffered.Both, serviceTypeOfferred);
        }
    }
}
