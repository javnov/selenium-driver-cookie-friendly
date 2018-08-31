using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace javnov.WebDriver.CookieFriendly
{
    /// <summary>
    /// Defines an interface allowing the user to manipulate cookies on the current page.
    /// </summary>
    public class CookieFriendlyCookieJar : ICookieJar
    {
        private static readonly MethodInfo _driverInternalExecuteMethodInfo = GetInernalExecuteFromRemoteWebDriver();
        private readonly RemoteWebDriver _driver;
        private readonly ICookieJar _cookieJar;

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieFriendlyCookieJar"/> class.
        /// </summary>
        /// <param name="driver">The driver that is currently in use</param>
        public CookieFriendlyCookieJar(RemoteWebDriver driver, ICookieJar cookieJar)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _cookieJar = cookieJar ?? throw new ArgumentNullException(nameof(cookieJar));
        }

        /// <summary>
        /// Gets all cookies defined for the current page.
        /// </summary>
        public ReadOnlyCollection<Cookie> AllCookies => GetAllCookies();

        /// <summary>
        /// Method for creating a cookie in the browser
        /// </summary>
        /// <param name="cookie"><see cref="Cookie"/> that represents a cookie in the browser</param>
        public void AddCookie(Cookie cookie)
        {
            _cookieJar.AddCookie(cookie);
        }

        /// <summary>
        /// Delete All Cookies that are present in the browser
        /// </summary>
        public void DeleteAllCookies()
        {
            _cookieJar.DeleteAllCookies();
        }

        /// <summary>
        /// Delete a cookie in the browser by passing in a copy of a cookie
        /// </summary>
        /// <param name="cookie">An object that represents a copy of the cookie that needs to be deleted</param>
        public void DeleteCookie(Cookie cookie)
        {
            _cookieJar.DeleteCookie(cookie);
        }

        /// <summary>
        /// Delete the cookie by passing in the name of the cookie
        /// </summary>
        /// <param name="name">The name of the cookie that is in the browser</param>
        public void DeleteCookieNamed(string name)
        {
            _cookieJar.DeleteCookieNamed(name);
        }

        /// <summary>
        /// Method for returning a getting a cookie by name
        /// </summary>
        /// <param name="name">name of the cookie that needs to be returned</param>
        /// <returns>A Cookie from the name</returns>
        public Cookie GetCookieNamed(string name)
        {
            Cookie cookieToReturn = null;
            if (name != null)
            {
                ReadOnlyCollection<Cookie> allCookies = AllCookies;
                foreach (Cookie currentCookie in allCookies)
                {
                    if (name.Equals(currentCookie.Name))
                    {
                        cookieToReturn = currentCookie;
                        break;
                    }
                }
            }

            return cookieToReturn;
        }

        /// <summary>
        /// Method for getting a Collection of Cookies that are present in the browser
        /// </summary>
        /// <returns>ReadOnlyCollection of Cookies in the browser</returns>
        private ReadOnlyCollection<Cookie> GetAllCookies()
        {
            List<Cookie> toReturn = new List<Cookie>();
            object returned = ExecuteOnDriver(_driver, DriverCommand.GetAllCookies, new Dictionary<string, object>()).Value;

            try
            {
                object[] cookies = returned as object[];
                if (cookies != null)
                {
                    foreach (object rawCookie in cookies)
                    {
                        Dictionary<string, object> cookieDictionary = rawCookie as Dictionary<string, object>;
                        if (rawCookie != null)
                        {
                            toReturn.Add(CookieFriendly.FromDictionary(cookieDictionary));
                        }
                    }
                }

                return new ReadOnlyCollection<Cookie>(toReturn);
            }
            catch (Exception e)
            {
                throw new WebDriverException("Unexpected problem getting cookies", e);
            }
        }

        /// <summary>
        /// Call the internal method "InternalExecute" of RemoteWebDriver class
        /// </summary>
        /// <param name="driver">RemoteWebDriver to use</param>
        /// <param name="driverCommandToExecute">Command that needs executing</param>
        /// <param name="parameters">Parameters needed for the command</param>
        /// <returns>WebDriver Response</returns>
        private static Response ExecuteOnDriver(RemoteWebDriver driver, string driverCommandToExecute, Dictionary<string, object> parameters)
        {
            Response response = _driverInternalExecuteMethodInfo.Invoke(driver, new object[] { driverCommandToExecute, parameters }) as Response;

            if (response == null)
                throw new InvalidOperationException("Cannot execute command on driver");

            return response;
        }

        /// <summary>
        /// Get the reflection referece to InternalExecute method of RemoteWebDriver class
        /// </summary>
        /// <returns></returns>
        private static MethodInfo GetInernalExecuteFromRemoteWebDriver()
        {
            var runtimeMethodInfo = typeof(RemoteWebDriver).GetRuntimeMethods().FirstOrDefault(rm => rm.Name == "InternalExecute");
            if (runtimeMethodInfo == null)
            {
                return typeof(RemoteWebDriver).GetMethod("InternalExecute", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            return runtimeMethodInfo;
        }
    }
}
