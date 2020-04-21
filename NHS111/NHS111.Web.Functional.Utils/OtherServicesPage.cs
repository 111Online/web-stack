using OpenQA.Selenium;
using System;

namespace NHS111.Web.Functional.Utils
{
    public class OtherServicesPage : DispositionPage<OtherServicesPage>
    {

        public OtherServicesPage(IWebDriver driver) : base(driver)
        {
        }

        public override OtherServicesPage EnterPostCodeAndSubmit(string postcode)
        {
            throw new NotImplementedException();
        }

        public OtherServicesPage CompareAndVerify(string uniqueId)
        {
            return base.CompareAndVerify(this, uniqueId);
        }
    }
}
