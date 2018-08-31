using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace javnov.WebDriver.CookieFriendly
{
    public class CookieFriendlyRemoteOptions : IOptions
    {
        private readonly RemoteWebDriver _driver;
        private readonly IOptions _driverOptions;

        public CookieFriendlyRemoteOptions(RemoteWebDriver driver)
        {
            _driver = driver;
            _driverOptions = driver.Manage();
        }

        public ICookieJar Cookies => new CookieFriendlyCookieJar(_driver, _driverOptions.Cookies);
        public IWindow Window => _driverOptions.Window;
        public ILogs Logs => _driverOptions.Logs;
        public ITimeouts Timeouts() => _driverOptions.Timeouts();
    }
}
