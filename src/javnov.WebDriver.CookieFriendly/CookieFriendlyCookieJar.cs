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
    public class CookieFriendlyCookieJar : ICookieJar
    {
        private static readonly MethodInfo _driverInternalExecuteMethodInfo = GetInernalExecuteFromRemoteWebDriver();
        private readonly RemoteWebDriver _driver;
        private readonly ICookieJar _cookieJar;        

        public CookieFriendlyCookieJar(RemoteWebDriver driver, ICookieJar cookieJar)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _cookieJar = cookieJar ?? throw new ArgumentNullException(nameof(cookieJar));
        }

        public ReadOnlyCollection<Cookie> AllCookies => GetAllCookies();

        public void AddCookie(Cookie cookie)
        {
            _cookieJar.AddCookie(cookie);
        }

        public void DeleteAllCookies()
        {
            _cookieJar.DeleteAllCookies();
        }

        public void DeleteCookie(Cookie cookie)
        {
            _cookieJar.DeleteCookie(cookie);
        }

        public void DeleteCookieNamed(string name)
        {
            _cookieJar.DeleteCookieNamed(name);
        }

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

        private static Response ExecuteOnDriver(RemoteWebDriver driver, string driverCommandToExecute, Dictionary<string, object> parameters)
        {
            Response response = _driverInternalExecuteMethodInfo.Invoke(driver, new object[] { driverCommandToExecute, parameters }) as Response;

            if (response == null)
                throw new InvalidOperationException("Cannot execute command on driver");

            return response;
        }

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
