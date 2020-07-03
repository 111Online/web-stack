using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NHS111.Models.Test.Models.Web
{
    [TestFixture]
    public class RecommendedServiceViewModelTests
    {
        private RecommendedServiceViewModel _recommendedServiceViewModel;

        private string _aliasOnly;

        [SetUp]
        public void Init()
        {
            _recommendedServiceViewModel = new RecommendedServiceViewModel()
            {
                ServiceTypeAlias = "Test Service Alias",
                Name = "Test Service Name",
                AddressLines = { "1 Test Street", "Test town", "Testton" },
                ServiceType = new ServiceType { Id = 0 },
                OnlineDOSServiceType = OnlineDOSServiceType.Unknown
            };

            _aliasOnly = string.Format("<b class=\"service-details__alias\">{0}</b>", WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias));
        }

        [Test]
        public void GetServiceDisplayHtml_CallBackCAS_ContainsOnlyServiceAlias()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            Assert.AreEqual(_aliasOnly, html);
        }

        [Test]
        public void GetServiceDisplayHtml_CallBackCAS_WithPublicName_ContainsOnlyServiceAlias()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            Assert.AreEqual(_aliasOnly, html);
        }

        [Test]
        public void GetServiceDisplayHtml_CallBackGPOOH_ContainsOnlyServiceAlias()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            Assert.AreEqual(_aliasOnly, html);
        }

        [Test]
        public void GetServiceDisplayHtml_CallBackGPOOH_ContainsOnlyServiceAliasAndPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndName = _aliasOnly + string.Format("<br />{0}", _recommendedServiceViewModel.PublicNameOnly);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetServiceDisplayHtml_CallbackOther_ContainsOnlyServiceAliasAndPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 100;
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndName = _aliasOnly + string.Format("<br />{0}", _recommendedServiceViewModel.PublicNameOnly);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetServiceDisplayHtml_CallbackOther_ContainsOnlyServiceAliasAndName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 100;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndName = _aliasOnly + string.Format("<br />{0}", _recommendedServiceViewModel.Name);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetServiceDisplayHtml_GoTo_ContainsServiceAliasPublicNameAndAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 52;
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndName = _aliasOnly + string.Format("<br />{0}", _recommendedServiceViewModel.PublicNameOnly);
            var aliasAndNameAndAddress = aliasAndName + string.Format("<br />{0}", _recommendedServiceViewModel.AddressLines.Where(address => !string.IsNullOrEmpty(address)).Aggregate(string.Empty, (current, address) => current + string.Format("{0}<br />", WebUtility.HtmlDecode(address))));
            Assert.AreEqual(aliasAndNameAndAddress, html);
        }

        [Test]
        public void GetServiceDisplayHtml_GoTo_ContainsServiceAliasAndAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 100;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndAddress = _aliasOnly + string.Format("<br />{0}", _recommendedServiceViewModel.AddressLines.Where(address => !string.IsNullOrEmpty(address)).Aggregate(string.Empty, (current, address) => current + string.Format("{0}<br />", WebUtility.HtmlDecode(address))));
            Assert.AreEqual(aliasAndAddress, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_CallBackCAS_ContainsOnlyServiceAlias()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            Assert.AreEqual(_aliasOnly, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_CallBackCAS_WithPublicName_ContainsOnlyServiceAlias()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            Assert.AreEqual(_aliasOnly, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_CallBackGPOOH_ContainsOnlyServiceAlias()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            Assert.AreEqual(_aliasOnly, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_CallBackGPOOH_ContainsOnlyServiceAliasAndPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var aliasAndName = _aliasOnly + string.Format("<br />{0}", _recommendedServiceViewModel.PublicNameOnly);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_CallbackOther_ContainsServiceAliasAndPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 50;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var aliasAndName = _aliasOnly + string.Format("<br />{0}", _recommendedServiceViewModel.PublicNameOnly);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_CallbackOther_ContainsServiceAliasAndName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 50;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var aliasAndName = _aliasOnly + string.Format("<br />{0}", _recommendedServiceViewModel.Name);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_GoTo_ContainsServiceAliasAndPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 52;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var aliasAndName = _aliasOnly + string.Format("<br />{0}", _recommendedServiceViewModel.PublicNameOnly);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_GoTo_ContainsServiceAndAddressLineOne()
        {
            _recommendedServiceViewModel.ServiceType.Id = 52;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var firstLineOfAddress = _recommendedServiceViewModel.AddressLines.FirstOrDefault(a => !string.IsNullOrEmpty(a));
            var aliasAndAddressLineOne = _aliasOnly + string.Format("<br />{0}", WebUtility.HtmlDecode(firstLineOfAddress));
            Assert.AreEqual(aliasAndAddressLineOne, html);
        }
    }
}
