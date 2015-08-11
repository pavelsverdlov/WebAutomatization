using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace WebAutomatization.Core.Page {
    public class SeleniumWebElementWrapper<T> where T : IWebDriver {
        private readonly IBrowser<T> Browser;
        private int _index;
        private IList<IWebElement> _searchCache;
        private readonly IList<TagName> _searchTags = new List<TagName>();
        // private readonly IList<SearchProperty> _searchProperties = new List<SearchProperty>();
        private TextSearchData _textSearchData;
        private string _xPath;

        public SeleniumWebElementWrapper(IBrowser<T> Browser) {
            this.Browser = Browser;
        }

        public IWebElement FindSingle(SearchData searchData) {
            return TryFindSingle(searchData);
        }

        private IWebElement TryFindSingle(SearchData searchData) {
            try {
                return FindSingleIWebElement(searchData);
            } catch (StaleElementReferenceException) {
                //ClearSearchResultCache();

                return FindSingleIWebElement(searchData);
            } catch (InvalidSelectorException) {
                throw;
            } catch (WebDriverException) {
                throw;
            } catch (WebElementNotFoundException) {
                throw;
            } catch {
                throw WebElementNotFoundException(searchData);
            }
        }

        private IWebElement FindSingleIWebElement(SearchData searchData) {
            var elements = FindIWebElements(searchData);

            if (!elements.Any()) {
                throw WebElementNotFoundException(searchData);
            }

            var element = elements.Count() == 1
                ? elements.Single()
                : _index == -1
                    ? elements.Last()
                    : elements.ElementAt(_index);
            // ReSharper disable UnusedVariable
            var elementAccess = element.Enabled;
            // ReSharper restore UnusedVariable

            return element;
        }

        private IList<IWebElement> FindIWebElements(SearchData searchData) {
            if (_searchCache != null) {
                return _searchCache;
            }

            Browser.JavaScript.WaitReadyState();
            Browser.JavaScript.WaitAjax();

            var resultEnumerable = Browser.Driver.FindElements(searchData.FirstSelector);

            try {
                var resultList = resultEnumerable
                    .FilterByVisibility(false)
                    .FilterByTagNames(_searchTags)
                    .FilterByText(_textSearchData)
                    .FilterByTagAttributes(searchData.SearchProperties).ToList();

                return resultList;
            } catch (Exception e) {
                Console.WriteLine(e);

                return new List<IWebElement>();
            }
        }

        private string SearchCriteriaToString(SearchData searchData) {

            var result = searchData.SearchProperties.Select(searchProperty =>
                string.Format("{0}: {1} ({2})",
                    searchProperty.AttributeName,
                    searchProperty.AttributeValue,
                    searchProperty.ExactMatch ? "exact" : "contains")).ToList();

            result.AddRange(_searchTags.Select(searchTag =>
                string.Format("tag: {0}", searchTag)));

            if (_xPath != null) {
                result.Add(string.Format("XPath: {0}", _xPath));
            }

            if (_textSearchData != null) {
                result.Add(string.Format("text: {0} ({1})",
                    _textSearchData.Text,
                    _textSearchData.ExactMatch ? "exact" : "contains"));
            }

            return string.Join(", ", result);

        }

        private WebElementNotFoundException WebElementNotFoundException(SearchData searchData) {
            CheckConnectionFailure();
            return new WebElementNotFoundException(string.Format("Can't find single element with given search criteria: {0}.",
                SearchCriteriaToString(searchData)));
        }

        private void CheckConnectionFailure() {
            const string connectionFailure = "connectionFailure";

            //            Contract.Assert(!Browser.PageSource.Contains(connectionFailure),
            //                "Connection can't be established.");
        }
    }
}