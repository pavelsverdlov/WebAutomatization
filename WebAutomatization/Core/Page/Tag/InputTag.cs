using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using WebAutomatization.Core.JS;

namespace WebAutomatization.Core.Page.Tag {
    public sealed class InputTag : IInputTag {
        private readonly IWebElement element;
        private readonly IJavaScript javaScript;

        public InputTag(IWebElement element, IJavaScript javaScript)
        {
            this.element = element;
            this.javaScript = javaScript;
        }

        public string Text {
            get {
                return GetText();
            }
            set {
                SetText(value);
            }
        }

        private string GetText() {
            return !string.IsNullOrEmpty(element.Text) ? element.Text : element.GetAttribute(TagAttribute.Value);
        }

        private void SetText(string value) {
            if (TagName.IsInputTag(element)) {
                element.Clear();
            } else {
                element.SendKeys(Keys.LeftControl + "a");
                element.SendKeys(Keys.Delete);
            }

            if (string.IsNullOrEmpty(value)) return;

            javaScript.Execute(string.Format("arguments[0].value = \"{0}\";", value), element);

            WaitHelper.Try(() => javaScript.FireJQueryEvent(element,JavaScriptEvent.KeyUp));
        }
    }
}
