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

        private string _serviceAliasHtml;

        [SetUp]
        public void Init()
        {
            _recommendedServiceViewModel = new RecommendedServiceViewModel()
            {
                ServiceTypeAlias = "Test Service Alias",
                Name = "Test Service Name",
                AddressLines = { "1 Test Street", "Test town", "Testton" },
                ServiceType = new ServiceType { Id = 0 },
                OnlineDOSServiceType = OnlineDOSServiceType.Callback
            };

            _serviceAliasHtml = "<b class=\"service-details__alias\">{0}</b>";  
        }

        [Test]
        public void GetServiceDisplayHtml_CallBackCAS_ContainsOnlyServiceAlias()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)), html);
        }

        [Test]
        public void GetServiceDisplayHtml_PhoneCAS_ContainsOnlyServiceName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.Name)), html);
        }

        [Test]
        public void GetServiceDisplayHtml_GoToCAS_ContainsOnlyServiceNameAndAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndAddress = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.Name)) + string.Format("<br />{0}", _recommendedServiceViewModel.AddressLines.Where(address => !string.IsNullOrEmpty(address)).Aggregate(string.Empty, (current, address) => current + string.Format("{0}<br />", WebUtility.HtmlDecode(address))));
            Assert.AreEqual(aliasAndAddress, html);
        }        

        [Test]
        public void GetServiceDisplayHtml_CallBackCAS_WithPublicName_ContainsOnlyServiceAlias()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)), html);
        }

        [Test]
        public void GetServiceDisplayHtml_PhoneCAS_WithPublicName_ContainsOnlyPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.PublicName)), html);
        }

        [Test]
        public void GetServiceDisplayHtml_GoToCAS_WithPublicName_ContainsOnlyPublicNameAndAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndAddress = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.PublicName)) + string.Format("<br />{0}", _recommendedServiceViewModel.AddressLines.Where(address => !string.IsNullOrEmpty(address)).Aggregate(string.Empty, (current, address) => current + string.Format("{0}<br />", WebUtility.HtmlDecode(address))));
            Assert.AreEqual(aliasAndAddress, html);
        }

        [Test]
        public void GetServiceDisplayHtml_CallBackGPOOH_ContainsOnlyServiceAlias()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)), html);
        }

        [Test]
        public void GetServiceDisplayHtml_PhoneGPOOH_ContainsOnlyServiceName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.Name)), html);
        }

        [Test]
        public void GetServiceDisplayHtml_GoToGPOOH_ContainsOnlyServiceNameAndAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndAddress = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.Name)) + string.Format("<br />{0}", _recommendedServiceViewModel.AddressLines.Where(address => !string.IsNullOrEmpty(address)).Aggregate(string.Empty, (current, address) => current + string.Format("{0}<br />", WebUtility.HtmlDecode(address))));
            Assert.AreEqual(aliasAndAddress, html);
        }

        [Test]
        public void GetServiceDisplayHtml_CallBackGPOOH_WithPublicName_ContainsOnlyServiceAliasAndPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndName = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", _recommendedServiceViewModel.PublicName);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetServiceDisplayHtml_PhoneGPOOH_WithPublicName_ContainsOnlyPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.PublicName)), html);
        }

        [Test]
        public void GetServiceDisplayHtml_GoToGPOOH_WithPublicName_ContainsOnlyPublicNameAndAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndAddress = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.PublicName)) + string.Format("<br />{0}", _recommendedServiceViewModel.AddressLines.Where(address => !string.IsNullOrEmpty(address)).Aggregate(string.Empty, (current, address) => current + string.Format("{0}<br />", WebUtility.HtmlDecode(address))));
            Assert.AreEqual(aliasAndAddress, html);
        }

        [Test]
        public void GetServiceDisplayHtml_CallbackOther_ContainsOnlyServiceAliasAndName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 100;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndName = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", _recommendedServiceViewModel.Name);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetServiceDisplayHtml_PhoneOther_ContainsOnlyServiceAliasAndName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 100;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndName = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", _recommendedServiceViewModel.Name);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetServiceDisplayHtml_GoToOther_ContainsOnlyServiceAliasAndAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 100;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndAddress = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", _recommendedServiceViewModel.AddressLines.Where(address => !string.IsNullOrEmpty(address)).Aggregate(string.Empty, (current, address) => current + string.Format("{0}<br />", WebUtility.HtmlDecode(address))));
            Assert.AreEqual(aliasAndAddress, html);
        }

        [Test]
        public void GetServiceDisplayHtml_CallbackOther_WithPublicName_ContainsOnlyServiceAliasAndPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 100;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndName = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", _recommendedServiceViewModel.PublicName);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetServiceDisplayHtml_PhoneOther_WithPublicName_ContainsServiceAliasAndPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 52;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndName = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", _recommendedServiceViewModel.PublicName);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetServiceDisplayHtml_GoToOther_WithPublicName_ContainsServiceAliasPublicNameAndAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 52;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetServiceDisplayHtml();
            var aliasAndName = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", _recommendedServiceViewModel.PublicNameOnly);
            var aliasAndNameAndAddress = aliasAndName + string.Format("<br />{0}", _recommendedServiceViewModel.AddressLines.Where(address => !string.IsNullOrEmpty(address)).Aggregate(string.Empty, (current, address) => current + string.Format("{0}<br />", WebUtility.HtmlDecode(address))));
            Assert.AreEqual(aliasAndNameAndAddress, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_CallBackCAS_ContainsOnlyServiceAlias()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)), html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_PhoneCAS_ContainsOnlyServiceName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.Name)), html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_GoToCAS_ContainsOnlyServiceNameAndFirstLineOfAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var firstLineOfAddress = _recommendedServiceViewModel.AddressLines.FirstOrDefault(a => !string.IsNullOrEmpty(a));
            var aliasAndAddressLineOne = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.Name)) + string.Format("<br />{0}", WebUtility.HtmlDecode(firstLineOfAddress));
            Assert.AreEqual(aliasAndAddressLineOne, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_CallBackCAS_WithPublicName_ContainsOnlyServiceAlias()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)), html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_PhoneCAS_WithPublicName_ContainsOnlyPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name"; 
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.PublicName)), html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_GoToCAS_WithPublicName_ContainsOnlyPublicNameAndFirstLineOfAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 130;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name"; var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var firstLineOfAddress = _recommendedServiceViewModel.AddressLines.FirstOrDefault(a => !string.IsNullOrEmpty(a));
            var aliasAndAddressLineOne = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.PublicName)) + string.Format("<br />{0}", WebUtility.HtmlDecode(firstLineOfAddress));
            Assert.AreEqual(aliasAndAddressLineOne, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_CallBackGPOOH_ContainsOnlyServiceAlias()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)), html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_PhoneGPOOH_ContainsOnlyServiceName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.Name)), html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_GoToGPOOH_ContainsOnlyServiceNameAndFirstLineOfAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var firstLineOfAddress = _recommendedServiceViewModel.AddressLines.FirstOrDefault(a => !string.IsNullOrEmpty(a));
            var aliasAndAddressLineOne = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.Name)) + string.Format("<br />{0}", WebUtility.HtmlDecode(firstLineOfAddress));
            Assert.AreEqual(aliasAndAddressLineOne, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_CallBackGPOOH_WithPublicName_ContainsOnlyServiceAliasAndPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var aliasAndName = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", _recommendedServiceViewModel.PublicName);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_PhoneGPOOH_WithPublicName_ContainsOnlyPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            Assert.AreEqual(string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.PublicName)), html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_GoToGPOOH_WithPublicName_ContainsOnlyPublicNameAndFirstLineOfAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 25;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var firstLineOfAddress = _recommendedServiceViewModel.AddressLines.FirstOrDefault(a => !string.IsNullOrEmpty(a));
            var aliasAndAddressLineOne = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.PublicName)) + string.Format("<br />{0}", WebUtility.HtmlDecode(firstLineOfAddress));
            Assert.AreEqual(aliasAndAddressLineOne, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_CallbackOther_ContainsServiceAliasAndName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 50;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var aliasAndName = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", _recommendedServiceViewModel.Name);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_PhoneOther_ContainsServiceAliasAndName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 50;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var aliasAndName = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", _recommendedServiceViewModel.Name);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_GoToOther_ContainsServiceAliasAndFirstLineOfAddress()
        {
            _recommendedServiceViewModel.ServiceType.Id = 52;
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var firstLineOfAddress = _recommendedServiceViewModel.AddressLines.FirstOrDefault(a => !string.IsNullOrEmpty(a));
            var aliasAndAddressLineOne = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", WebUtility.HtmlDecode(firstLineOfAddress));
            Assert.AreEqual(aliasAndAddressLineOne, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_CallbackOther_WithPublicName_ContainsServiceAliasAndPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 50;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var aliasAndName = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", _recommendedServiceViewModel.PublicName);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_PhoneOther_WithPublicName_ContainsServiceAliasAndPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 50;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone; 
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var aliasAndName = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", _recommendedServiceViewModel.PublicName);
            Assert.AreEqual(aliasAndName, html);
        }

        [Test]
        public void GetOtherServicesServiceDisplayHtml_GoToOther_WithPublicName_ContainsServiceAliasAndPublicName()
        {
            _recommendedServiceViewModel.ServiceType.Id = 52;
            _recommendedServiceViewModel.PublicName = "Test public name";
            _recommendedServiceViewModel.PublicNameOnly = "Test public name";
            _recommendedServiceViewModel.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
            var html = _recommendedServiceViewModel.GetOtherServicesServiceDisplayHtml();
            var firstLineOfAddress = _recommendedServiceViewModel.AddressLines.FirstOrDefault(a => !string.IsNullOrEmpty(a));
            var aliasAndAddressLineOne = string.Format(_serviceAliasHtml, WebUtility.HtmlDecode(_recommendedServiceViewModel.ServiceTypeAlias)) + string.Format("<br />{0}", WebUtility.HtmlDecode(firstLineOfAddress));
            Assert.AreEqual(aliasAndAddressLineOne, html);
        }

        [Test]
        public void ShouldShowOtherServicesServiceTypeDescription_EmptyServiceTypeDescription_ReturnsFalse()
        {
            Assert.IsFalse(_recommendedServiceViewModel.ShouldShowOtherServicesServiceTypeDescription(false));
        }

        [Test]
        public void ShouldShowOtherServicesServiceTypeDescription_EmptyServiceTypeDescription_OtherServices_ReturnsFalse()
        {
            Assert.IsFalse(_recommendedServiceViewModel.ShouldShowOtherServicesServiceTypeDescription(true));
        }
    }
}
