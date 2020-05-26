using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NHS111.Features.Test
{
    [TestFixture]
    public class FeatureRouterTest
    {
        private NameValueCollection _queryString;

        [SetUp]
        public void Init()
        {
            ConfigurationManager.AppSettings["CovidSearchRedirectFlag"] = null;
            _queryString = new NameValueCollection();
        }

        [Test]
        public void CovidSearchRedirectIsNotEnabled()
        {
            ConfigurationManager.AppSettings["CovidSearchRedirectFlag"] = "false";
            Assert.False(FeatureRouter.CovidSearchRedirect(_queryString));
        }

        [Test]
        public void CovidSearchRedirectIsNullNotEnabled()
        {
            Assert.False(FeatureRouter.CovidSearchRedirect(_queryString));
        }

        [Test]
        public void CovidSearchRedirectIsEnabled()
        {
            ConfigurationManager.AppSettings["CovidSearchRedirectFlag"] = "true";
            Assert.True(FeatureRouter.CovidSearchRedirect(_queryString));
        }

        //[Test]
        //public void CovidSearchRedirectIsEnabledByQueryString()
        //{
        //    _queryString["on2165"] = "true";
        //    Assert.True(FeatureRouter.CovidSearchRedirect(_queryString));
        //}

        [Test]
        public void CovidSearchRedirectIsNotEnabledByQueryString()
        {
            _queryString["on2165"] = "false";
            Assert.False(FeatureRouter.CovidSearchRedirect(_queryString));
        }
    }
}
