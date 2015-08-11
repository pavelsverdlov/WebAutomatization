using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using WebAutomatization.Core.JS;

namespace WebAutomatization.Core.Page.Tag {
    public sealed class ActionTag : IActionTag {
        private readonly IWebElement element;
        private readonly IJavaScript javaScript;

        public ActionTag(IWebElement webElement, IJavaScript javaScript) {
            this.element = webElement;
            this.javaScript = javaScript;
        }

        public void Click() {
            bool useJQuery = true;
            Contract.Assert(element.Enabled);
            if (useJQuery && element.TagName != TagName.Link) {
                FireJQueryEvent(element, JavaScriptEvent.Click);
            } else {
                try {
                    element.Click();
                } catch (InvalidOperationException e) {
                    if (e.Message.Contains("Element is not clickable")) {
                        Thread.Sleep(2000);
                        element.Click();
                    }
                }
            }
        }

        public void Action(JavaScriptEvent _event) {
            if (_event == JavaScriptEvent.Click) {
                Click();
            } else {
                throw new NotImplementedException(_event);
            }
        }


        private void FireJQueryEvent(IWebElement element, JavaScriptEvent javaScriptEvent) {
            var eventName = javaScriptEvent.Name;
            javaScript.Execute(string.Format("$(arguments[0]).{0}();", eventName), element);
        }
    }
}
