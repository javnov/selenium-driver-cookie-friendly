using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace javnov.WebDriver.CookieFriendly.IntegrationTest
{
    [TestClass]
    public class IntegrationTest
    {
        private IWebDriver _webDriver;
        private const string TargetTestUrl = "https://www.carnival.com/";

        [TestInitialize]
        public void Initialize()
        {
            _webDriver = new CookieFriendlyRemoteWebDriver(new ChromeDriver());
            _webDriver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromMinutes(3));
            _webDriver.Manage().Window.Maximize();
        }

        [TestCleanup]
        public virtual void TearDown()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TestAndCheckCookiesWithoutName()
        {
            _webDriver.Navigate().GoToUrl(TargetTestUrl);

            var allCookies = _webDriver.Manage().Cookies.AllCookies;

            Assert.IsTrue(allCookies.Count(x => string.IsNullOrEmpty(x.Name)) > 0);
        }
    }
}
