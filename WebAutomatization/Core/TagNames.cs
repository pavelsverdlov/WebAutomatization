using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using OpenQA.Selenium;

namespace WebAutomatization.Core {
    public struct TagName {
        public readonly string Name;
        public TagName(string name) : this() { Name = name; }

        public static bool IsInputTag(IWebElement element) {
            return element.TagName == TagName.Input || element.TagName == TagName.TextArea;
        }

        public static bool operator ==(TagName x, string y) { return x.Name == y; }
        public static bool operator !=(TagName x, string y) { return !(x == y); }
        public static bool operator ==(string x, TagName y) { return x == y.Name; }
        public static bool operator !=(string x, TagName y) { return !(x == y); }

        public readonly static TagName TextArea = new TagName("textarea");

        public readonly static TagName Input = new TagName("input");

        public readonly static TagName Link = new TagName("a");

        public readonly static TagName Span = new TagName("span");

        public readonly static TagName InlineFrame = new TagName("iframe");

        public readonly static TagName Div = new TagName("div");

        public readonly static TagName Image = new TagName("img");
    }
}
