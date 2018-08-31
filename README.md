# selenium-driver-cookie-friendly
Give to selenium a WebDriver that is friendly with marketing cookies

## How to use it

Install from nuget:
```powershell
PM> Install-Package javnov.WebDriver.CookieFriendly
```

Import this namespace:
```csharp
using javnov.WebDriver.CookieFriendly;
```

Just pass your driver when creating a new instance of CookieFriendlyRemoteWebDriver

```csharp
CookieFriendlyRemoteWebDriver cookieFriendlyWebDriver = new CookieFriendlyRemoteWebDriver(new ChromeDriver());
```

