using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace javnov.WebDriver.CookieFriendly
{
    /// <summary>
    /// Represents a cookie returned to the driver by the browser that could have an empty name.
    /// </summary>
    public class CookieFriendly : ReturnedCookie
    {
        private static readonly FieldInfo cookieNameFieldInfo = GetCookieNameFieldInfo();

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieFriendly"/> class with a specific name,
        /// value, domain, path and expiration date.
        /// </summary>
        /// <param name="name">The name of the cookie.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <param name="domain">The domain of the cookie.</param>
        /// <param name="path">The path of the cookie.</param>
        /// <param name="expiry">The expiration date of the cookie.</param>
        /// <param name="isSecure"><see langword="true"/> if the cookie is secure; otherwise <see langword="false"/></param>
        /// <param name="isHttpOnly"><see langword="true"/> if the cookie is an HTTP-only cookie; otherwise <see langword="false"/></param>
        /// <exception cref="ArgumentException">If the name is <see langword="null"/> or an empty string,
        /// or if it contains a semi-colon.</exception>
        /// <exception cref="ArgumentNullException">If the value or currentUrl is <see langword="null"/>.</exception>
        public CookieFriendly(string name, string value, string domain, string path, DateTime? expiry, bool isSecure, bool isHttpOnly) : base("friendly", value, domain, path, expiry, isSecure, isHttpOnly)
        {
            OverrideName(this, name);
        }

        /// <summary>
        /// Converts a Dictionary to a Cookie.
        /// </summary>
        /// <param name="rawCookie">The Dictionary object containing the cookie parameters.</param>
        /// <returns>A <see cref="Cookie"/> object with the proper parameters set.</returns>
        public new static Cookie FromDictionary(Dictionary<string, object> rawCookie)
        {
            if (rawCookie == null)
            {
                throw new ArgumentNullException("rawCookie", "Dictionary cannot be null");
            }

            string name = rawCookie["name"].ToString();
            string value = rawCookie["value"].ToString();

            string path = "/";
            if (rawCookie.ContainsKey("path") && rawCookie["path"] != null)
            {
                path = rawCookie["path"].ToString();
            }

            string domain = string.Empty;
            if (rawCookie.ContainsKey("domain") && rawCookie["domain"] != null)
            {
                domain = rawCookie["domain"].ToString();
            }

            DateTime? expires = null;
            if (rawCookie.ContainsKey("expiry") && rawCookie["expiry"] != null)
            {
                double seconds = 0;
                if (double.TryParse(rawCookie["expiry"].ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out seconds))
                {
                    try
                    {
                        expires = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds).ToLocalTime();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        expires = DateTime.MaxValue.ToLocalTime();
                    }
                }
            }

            bool secure = false;
            if (rawCookie.ContainsKey("secure") && rawCookie["secure"] != null)
            {
                secure = bool.Parse(rawCookie["secure"].ToString());
            }

            bool isHttpOnly = false;
            if (rawCookie.ContainsKey("httpOnly") && rawCookie["httpOnly"] != null)
            {
                isHttpOnly = bool.Parse(rawCookie["httpOnly"].ToString());
            }

            return new CookieFriendly(name, value, domain, path, expires, secure, isHttpOnly);
        }

        /// <summary>
        /// Override cookie name to bypass name empty null validations
        /// </summary>
        /// <param name="cookie">Object to be modified</param>
        /// <param name="name">name of the cookie</param>
        private static void OverrideName(Cookie cookie, string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(name))
            {
                cookieNameFieldInfo?.SetValue(cookie, "");
                return;
            }

            if (name.IndexOf(';') != -1)
                throw new ArgumentException("Cookie names cannot contain a ';': " + name, nameof(name));

            cookieNameFieldInfo?.SetValue(cookie, name);
        }

        /// <summary>
        /// Get cookieName reflection reference
        /// </summary>
        /// <returns></returns>
        private static FieldInfo GetCookieNameFieldInfo()
        {
            return typeof(Cookie).GetRuntimeFields().FirstOrDefault(f => f.Name == "cookieName");
        }
    }
}
