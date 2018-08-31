using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace javnov.WebDriver.CookieFriendly
{
    /// <summary>
    /// Provides a mechanism for setting options needed for the driver during the test.
    /// </summary>
    public class CookieFriendlyRemoteOptions : IOptions
    {
        private readonly RemoteWebDriver _driver;
        private readonly IOptions _driverOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieFriendlyRemoteOptions"/> class
        /// </summary>
        /// <param name="driver">Instance of the driver currently in use</param>
        public CookieFriendlyRemoteOptions(RemoteWebDriver driver)
        {
            _driver = driver;
            _driverOptions = driver.Manage();
        }

        /// <summary>
        /// Gets an object allowing the user to manipulate cookies on the page.
        /// </summary>
        public ICookieJar Cookies => new CookieFriendlyCookieJar(_driver, _driverOptions.Cookies);

        /// <summary>
        /// Gets an object allowing the user to manipulate the currently-focused browser window.
        /// </summary>
        /// <remarks>"Currently-focused" is defined as the browser window having the window handle
        /// returned when IWebDriver.CurrentWindowHandle is called.</remarks>
        public IWindow Window => _driverOptions.Window;

        /// <summary>
        /// Gets an object allowing the user to examine the logs of the current driver instance.
        /// </summary>
        public ILogs Logs => _driverOptions.Logs;

        /// <summary>
        /// Provides access to the timeouts defined for this driver.
        /// </summary>
        /// <returns>An object implementing the <see cref="ITimeouts"/> interface.</returns>
        public ITimeouts Timeouts() => _driverOptions.Timeouts();
    }
}
