using System;
using System.Diagnostics.Contracts;
using OpenQA.Selenium;

namespace WebAutomatization.Core.JS {
    internal sealed class JavaScriptAdapter<T> : IJavaScript where T : IWebDriver {
        private readonly BrowserAdapter<T> browser;
        private readonly T driver;
        private readonly IJavaScriptExecutor js;
        public JavaScriptAdapter(BrowserAdapter<T> browser) {
            this.browser = browser;
            driver = browser.Driver;
            js = (IJavaScriptExecutor)driver;
        }
        public object Execute(string javaScript, params object[] args) {
            return js.ExecuteScript(javaScript, args);
        }
        public void WaitReadyState() {
            Contract.Assume(browser != null);
            var ready = new Func<bool>(() => (bool)Execute("return document.readyState == 'complete'"));
            Contract.Assert(WaitHelper.SpinWait(ready, TimeSpan.FromSeconds(60), TimeSpan.FromMilliseconds(100)));
        }

        public void WaitAjax() {
            Contract.Assume(browser != null);
            var ready = new Func<bool>(() => (bool)Execute("return (typeof($) === 'undefined') ? true : !$.active;"));
            Contract.Assert(WaitHelper.SpinWait(ready, TimeSpan.FromSeconds(60), TimeSpan.FromMilliseconds(100)));
        }
        public void FireJQueryEvent(IWebElement element, JavaScriptEvent javaScriptEvent) {
            var eventName = javaScriptEvent.Name;
            Execute(string.Format("$(arguments[0]).{0}();", eventName), element);
        }

    }

    public interface IJavaScript {
        object Execute(string javaScript, params object[] args);
        void WaitAjax();
        void WaitReadyState();
        void FireJQueryEvent(IWebElement element, JavaScriptEvent javaScriptEvent);
    }
}