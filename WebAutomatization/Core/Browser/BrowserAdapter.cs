using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using WebAutomatization.Core.JS;
using WebAutomatization.Core.Page;
using WebAutomatization.Properties;

namespace WebAutomatization.Core {
    public sealed class BrowserAdapter<T> : IBrowser<T> where T : IWebDriver {
        private sealed class PageAdapter : IPage {
            private readonly BrowserAdapter<T> browser;
            private readonly T driver;
            public ITag Tag { get; private set; }

            public PageAdapter(BrowserAdapter<T> browser) {
                this.browser = browser;
                this.driver = browser.Driver;
                Tag = new PageTag<T>(browser);
            }

            public void GoToUrl(Uri url) {
                driver.Navigate().GoToUrl(url);
            }
            public IEnumerable<IWebElement> FindElements(By selector) {
                return driver.FindElements(selector);
            }
            public void NavigateBack() {
                driver.Navigate().Back();
            }

            public void Refresh() {
                driver.Navigate().Refresh();
            }

            
        }

        private sealed class Keybord {
            private readonly T driver;
            public Keybord(BrowserAdapter<T> browser) {
                driver = browser.Driver;
            }

            public void KeyDown(string key) {
                new Actions(driver).KeyDown(key);
            }
            public void KeyUp(string key) {
                new Actions(driver).KeyUp(key);
            }
        }

        private string mainWindowHandler;
        private readonly PageAdapter page;
        private readonly JavaScriptAdapter<T> javaScript;
        private readonly Keybord keybord;

        public BrowserAdapter(T driver, Browsers type) {
            Type = type;
            Driver = driver;
            page = new PageAdapter(this);
            javaScript = new JavaScriptAdapter<T>(this);
            keybord = new Keybord(this);
        }

        public Browsers Type { get; private set; }
        public T Driver { get; private set; }
        public IPage Page { get { return page; } }
        public IJavaScript JavaScript { get { return javaScript; } }

        public void Initialize() {
            Driver.Manage().Window.Maximize();
            mainWindowHandler = Driver.CurrentWindowHandle;
        }
        public void Quit() {
            Driver.Quit();
        }
        public void ResizeWindow(int width, int height) {
            javaScript.Execute(string.Format("window.resizeTo({0}, {1});", width, height));
        }
        public void AlertAccept() {
            Thread.Sleep(2000);
            Driver.SwitchTo().Alert().Accept();
            Driver.SwitchTo().DefaultContent();
        }
        public void SwitchToFrame(IWebElement inlineFrame) {
            Driver.SwitchTo().Frame(inlineFrame);
        }

        public void SwitchToPopupWindow() {
            foreach (var handle in Driver.WindowHandles.Where(handle => handle != mainWindowHandler)) // TODO:
            {
                Driver.SwitchTo().Window(handle);
            }
        }
        public void SwitchToMainWindow() {
            Driver.SwitchTo().Window(mainWindowHandler);
        }
        public void SwitchToDefaultContent() {
            Driver.SwitchTo().DefaultContent();
        }
        public static void AcceptAlert() {
//            var accept = Executor.MakeTry(() => WebDriver.SwitchTo().Alert().Accept());
//            Executor.SpinWait(accept, TimeSpan.FromSeconds(5));
        }

        public Screenshot GetScreenshot() {
            javaScript.WaitReadyState();
            return ((ITakesScreenshot)Driver).GetScreenshot();
        }

        public void SaveScreenshot(string path) {
            GetScreenshot().SaveAsFile(path, ImageFormat.Jpeg);
        }

        public void DragAndDrop(IWebElement source, IWebElement destination) {
            (new Actions(Driver)).DragAndDrop(source, destination).Build().Perform();
        }
        public Uri Url {
            get { javaScript.WaitAjax(); return new Uri(Driver.Url); }
        }

        public string Title {
            get {
                javaScript.WaitAjax();
                return string.Format("{0} - {1}", Driver.Title, Type);
            }
        }

        public string PageSource {
            get { javaScript.WaitAjax(); return Driver.PageSource; }
        }
    }
}

