using OpenQA.Selenium;
using OpenQA.Selenium.Html5;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace javnov.WebDriver.CookieFriendly
{
    public class CookieFriendlyRemoteWebDriver : IWebDriver, ISearchContext, IJavaScriptExecutor, IFindsById, IFindsByClassName, IFindsByLinkText, IFindsByName, IFindsByTagName, IFindsByXPath, IFindsByPartialLinkText, IFindsByCssSelector, ITakesScreenshot, IHasInputDevices, IHasCapabilities, IHasWebStorage, IHasLocationContext, IHasApplicationCache, IAllowsFileDetection, IHasSessionId, IWrapsDriver
    {
        private readonly RemoteWebDriver _webDriver;

        public CookieFriendlyRemoteWebDriver(RemoteWebDriver remoteWebDriver)
        {
            _webDriver = remoteWebDriver ?? throw new ArgumentNullException(nameof(remoteWebDriver));
        }

        public string Url
        {
            get => _webDriver.Url;
            set => _webDriver.Url = value;
        }

        public string Title => _webDriver.Title;

        public string PageSource => _webDriver.PageSource;

        public string CurrentWindowHandle => _webDriver.CurrentWindowHandle;

        public ReadOnlyCollection<string> WindowHandles => _webDriver.WindowHandles;

        public IKeyboard Keyboard => _webDriver.Keyboard;

        public IMouse Mouse => _webDriver.Mouse;

        public ICapabilities Capabilities => _webDriver.Capabilities;

        public bool HasWebStorage => _webDriver.HasWebStorage;

        public IWebStorage WebStorage => _webDriver.WebStorage;

        public bool HasLocationContext => _webDriver.HasLocationContext;

        public ILocationContext LocationContext => _webDriver.LocationContext;

        public bool HasApplicationCache => _webDriver.HasApplicationCache;

        public IApplicationCache ApplicationCache => _webDriver.ApplicationCache;

        public IFileDetector FileDetector
        {
            get => _webDriver.FileDetector;
            set => _webDriver.FileDetector = value;
        }

        public SessionId SessionId => _webDriver.SessionId;

        public IWebDriver WrappedDriver => _webDriver;

        public void Close() => _webDriver.Close();

        public void Dispose() => _webDriver.Dispose();

        public object ExecuteAsyncScript(string script, params object[] args) => _webDriver.ExecuteAsyncScript(script, args);

        public object ExecuteScript(string script, params object[] args) => _webDriver.ExecuteScript(script, args);

        public IWebElement FindElement(By by) => _webDriver.FindElement(by);

        public IWebElement FindElementByClassName(string className) => _webDriver.FindElementByClassName(className);

        public IWebElement FindElementByCssSelector(string cssSelector) => _webDriver.FindElementByCssSelector(cssSelector);

        public IWebElement FindElementById(string id) => _webDriver.FindElementById(id);

        public IWebElement FindElementByLinkText(string linkText) => _webDriver.FindElementByLinkText(linkText);

        public IWebElement FindElementByName(string name) => _webDriver.FindElementByName(name);

        public IWebElement FindElementByPartialLinkText(string partialLinkText) => _webDriver.FindElementByPartialLinkText(partialLinkText);

        public IWebElement FindElementByTagName(string tagName) => _webDriver.FindElementByTagName(tagName);

        public IWebElement FindElementByXPath(string xpath) => _webDriver.FindElementByXPath(xpath);

        public ReadOnlyCollection<IWebElement> FindElements(By by) => _webDriver.FindElements(by);

        public ReadOnlyCollection<IWebElement> FindElementsByClassName(string className) => _webDriver.FindElementsByClassName(className);

        public ReadOnlyCollection<IWebElement> FindElementsByCssSelector(string cssSelector) => _webDriver.FindElementsByCssSelector(cssSelector);

        public ReadOnlyCollection<IWebElement> FindElementsById(string id) => _webDriver.FindElementsById(id);

        public ReadOnlyCollection<IWebElement> FindElementsByLinkText(string linkText) => _webDriver.FindElementsByLinkText(linkText);

        public ReadOnlyCollection<IWebElement> FindElementsByName(string name) => _webDriver.FindElementsByName(name);

        public ReadOnlyCollection<IWebElement> FindElementsByPartialLinkText(string partialLinkText) => _webDriver.FindElementsByPartialLinkText(partialLinkText);

        public ReadOnlyCollection<IWebElement> FindElementsByTagName(string tagName) => _webDriver.FindElementsByTagName(tagName);

        public ReadOnlyCollection<IWebElement> FindElementsByXPath(string xpath) => _webDriver.FindElementsByXPath(xpath);

        public Screenshot GetScreenshot() => _webDriver.GetScreenshot();

        public IOptions Manage()
        {
            return new CookieFriendlyRemoteOptions(_webDriver);
        }

        public INavigation Navigate() => _webDriver.Navigate();

        public void Quit() => _webDriver.Quit();

        public ITargetLocator SwitchTo() => _webDriver.SwitchTo();
    }
}
