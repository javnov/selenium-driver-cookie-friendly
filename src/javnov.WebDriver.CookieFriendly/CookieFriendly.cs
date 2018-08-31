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
    public class CookieFriendly : ReturnedCookie
    {
        private static readonly FieldInfo cookieNameFieldInfo = GetCookieNameFieldInfo();

        public CookieFriendly(string name, string value, string domain, string path, DateTime? expiry, bool isSecure, bool isHttpOnly) : base("friendly", value, domain, path, expiry, isSecure, isHttpOnly)
        {
            OverrideName(this, name);
        }

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

        private static FieldInfo GetCookieNameFieldInfo()
        {
            return typeof(Cookie).GetRuntimeFields().FirstOrDefault(f => f.Name == "cookieName");
        }
    }
}
