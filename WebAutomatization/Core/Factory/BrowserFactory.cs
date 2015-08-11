using System;
using System.IO;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace WebAutomatization.Core
{
    public sealed class BrowserFactory :
        ABrowserFactory,
        IBrowserWebDriver<FirefoxDriver>,
        IBrowserWebDriver<InternetExplorerDriver> {
        IBrowser<FirefoxDriver> IBrowserWebDriver<FirefoxDriver>.Create() {
            var firefoxProfile = new FirefoxProfile {
                AcceptUntrustedCertificates = true,
                EnableNativeEvents = true
            };

            return new BrowserAdapter<FirefoxDriver>(new FirefoxDriver(firefoxProfile), Browsers.Firefox);

        }

        private static ChromeDriver StartChrome() {
            var chromeOptions = new ChromeOptions();
            var defaultDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\..\Local\Google\Chrome\User Data\Default";

            if (Directory.Exists(defaultDataFolder)) {
                // Executor.Try(() => DirectoryExtension.ForceDelete(defaultDataFolder));
            }

            return new ChromeDriver(Directory.GetCurrentDirectory(), chromeOptions);
        }

        IBrowser<InternetExplorerDriver> IBrowserWebDriver<InternetExplorerDriver>.Create() {
            var internetExplorerOptions = new InternetExplorerOptions {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                InitialBrowserUrl = "about:blank",
                EnableNativeEvents = true
            };

            return new BrowserAdapter<InternetExplorerDriver>(new InternetExplorerDriver(Directory.GetCurrentDirectory(), internetExplorerOptions), Browsers.InternetExplorer);
        }
        }
}