using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using WebAutomatization.Core.Page.Tag;

namespace WebAutomatization.Core.Page {
    public struct SearchProperty {
        public string AttributeName;
        public string AttributeValue;
        public bool ExactMatch;
    }
    public struct SearchData {
        public readonly List<SearchProperty> SearchProperties;
        public readonly By FirstSelector;
        public SearchData(List<SearchProperty> searchProperties, By firstSelector)
            : this() {
            SearchProperties = searchProperties;
            FirstSelector = firstSelector;
        }
    }

    public sealed class TagSearchBy<T> : PageTag<T>, ITagSearchBy where T : IWebDriver {
        public By FirstSelector { get; private set; }
        internal List<SearchProperty> SearchProperties { get; private set; }

        public TagSearchBy(IBrowser<T> browser)
            : base(browser) {
            SearchProperties = new List<SearchProperty>();
        }

        public IInputTag AsInputElement() {
            return new InputTag(GetWebElement(), browser.JavaScript);
        }
        public IActionTag AsActionElement() {
            return new ActionTag(GetWebElement(), browser.JavaScript);
        }

        private IWebElement GetWebElement() {
            var selenium = new SeleniumWebElementWrapper<T>(browser);
            var webElement = selenium.FindSingle(new SearchData(
                SearchProperties,
                FirstSelector
                ));
            return webElement;
        }

        public void SetFirstSelector(By selector) {
            if (FirstSelector != null) {
                return;
            }
            FirstSelector = selector;
        }

        protected override TagSearchBy<T> CreateSearchTag() {
            return this;
        }
    }

    public class PageTag<T> : ITag where T : IWebDriver {
        protected readonly IBrowser<T> browser;
        public PageTag(IBrowser<T> browser) { this.browser = browser; }

        public ITagSearchBy By(TagAttribute by, string key) {
            return ByAttribute(TagAttribute.Id, key, true);
        }
        public ITagSearchBy ByAttribute(TagAttribute tagAttribute, string attributeValue, bool exactMatch = true) {
            return ByAttribute(tagAttribute.Attribute, attributeValue, exactMatch);
        }
        private ITagSearchBy ByAttribute(string tagAttribute, string attributeValue, bool exactMatch = true) {
            var tag = CreateSearchTag();
            var xPath = exactMatch ?
                        string.Format("//*[@{0}=\"{1}\"]", tagAttribute, attributeValue) :
                        string.Format("//*[contains(@{0}, \"{1}\")]", tagAttribute, attributeValue);
            var selector = OpenQA.Selenium.By.XPath(xPath);
            tag.SetFirstSelector(selector);
            tag.SearchProperties.Add(new SearchProperty {
                AttributeName = tagAttribute,
                AttributeValue = attributeValue,
                ExactMatch = exactMatch
            });

            return tag;
        }

        protected virtual TagSearchBy<T> CreateSearchTag() {
            var tag = new TagSearchBy<T>(browser);
            return tag;
        }
    }
}
