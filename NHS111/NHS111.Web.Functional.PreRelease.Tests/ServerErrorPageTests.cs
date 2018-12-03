﻿using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.PreRelease.Tests
{
    [TestFixture]
    public class ServerErrorPageTests : BaseTests
    {
        [Test]
        public void ServerErrorPage_Displays()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            var serverErrorPage = searchPage.TypeErrorSearch();
            serverErrorPage.Verify();
        }
    }
}
