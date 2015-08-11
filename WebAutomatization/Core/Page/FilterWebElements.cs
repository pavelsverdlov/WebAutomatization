using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace WebAutomatization.Core.Page {
    internal class TextSearchData {
        public string Text { get; set; }
        public bool ExactMatch { get; set; }
    }
    internal static class FilterWebElements {
        public static IEnumerable<IWebElement> FilterByVisibility(this IEnumerable<IWebElement> result, bool includeHidden) {
            return !includeHidden ? result.Where(item => item.Displayed) : result;
        }

        public static IEnumerable<IWebElement> FilterByTagNames(this IEnumerable<IWebElement> elements, IList<TagName> searchTags) {
            return searchTags.Aggregate(elements, (current, tag) => current.Where(item => item.TagName == tag));
        }

        public static IEnumerable<IWebElement> FilterByText(this IEnumerable<IWebElement> result, TextSearchData _textSearchData) {
            if (_textSearchData != null) {
                result = _textSearchData.ExactMatch
                    ? result.Where(item => item.Text == _textSearchData.Text)
                    : result.Where(item => item.Text.Contains(_textSearchData.Text, StringComparison.InvariantCultureIgnoreCase));
            }

            return result;
        }

        public static IEnumerable<IWebElement> FilterByTagAttributes(this IEnumerable<IWebElement> elements, IList<SearchProperty> searchProperties) {
            return searchProperties.Aggregate(elements, FilterByTagAttribute);
        }

        private static IEnumerable<IWebElement> FilterByTagAttribute(IEnumerable<IWebElement> elements, SearchProperty searchProperty) {
            return searchProperty.ExactMatch ?
                elements.Where(item => item.GetAttribute(searchProperty.AttributeName) != null && item.GetAttribute(searchProperty.AttributeName).Equals(searchProperty.AttributeValue)) :
                elements.Where(item => item.GetAttribute(searchProperty.AttributeName) != null && item.GetAttribute(searchProperty.AttributeName).Contains(searchProperty.AttributeValue));
        }
        public static bool Contains(this string source, string target, StringComparison stringComparison) {
            return source.IndexOf(target, stringComparison) >= 0;
        }
    }

}
