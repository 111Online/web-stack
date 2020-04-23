using NHS111.Business.DOS.Service;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;
using System.Collections.Generic;

namespace NHS111.Business.DOS.Test.ServiceType
{
    [TestFixture]
    public class OnlineServiceTypeFilterTest
    {
        [Test]
        public void OnlineServiceTypeFilter_AllKnownTypes_ReturnAll()
        {
            var dosResult1 = new Models.Models.Business.DosService
            {
                OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone
            };
            var dosResult2 = new Models.Models.Business.DosService
            {
                OnlineDOSServiceType = OnlineDOSServiceType.GoTo
            };
            var dosResult3 = new Models.Models.Business.DosService
            {
                OnlineDOSServiceType = OnlineDOSServiceType.Callback
            };

            var dosResultsList = new List<Models.Models.Business.DosService> { dosResult1, dosResult2, dosResult3 };

            //Act
            var sut = new OnlineServiceTypeFilter();
            var result = sut.FilterUnknownTypes(dosResultsList);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, result[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.Callback, result[2].OnlineDOSServiceType);
        }

        [Test]
        public void OnlineServiceTypeFilter_UnknownTypes_NoUnknownTypesReturn()
        {
            var dosResult1 = new Models.Models.Business.DosService
            {
                OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone
            };
            var dosResult2 = new Models.Models.Business.DosService
            {
                OnlineDOSServiceType = OnlineDOSServiceType.GoTo
            };
            var dosResult3 = new Models.Models.Business.DosService
            {
                OnlineDOSServiceType = OnlineDOSServiceType.Callback
            };
            var dosResult4 = new Models.Models.Business.DosService
            {
                OnlineDOSServiceType = OnlineDOSServiceType.Unknown
            };

            var dosResultsList = new List<Models.Models.Business.DosService> { dosResult1, dosResult2, dosResult3, dosResult4 };

            //Act
            var sut = new OnlineServiceTypeFilter();
            var result = sut.FilterUnknownTypes(dosResultsList);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, result[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.Callback, result[2].OnlineDOSServiceType);
        }

        [Test]
        public void OnlineServiceTypeFilter_Empty_EmptyListReturn()
        {
            var dosResultsList = new List<Models.Models.Business.DosService>();

            //Act
            var sut = new OnlineServiceTypeFilter();
            var result = sut.FilterUnknownTypes(dosResultsList);

            Assert.AreEqual(0, result.Count);
        }
    }
}
