using OpenQA.Selenium;
using WebAutomatization.Core.JS;

namespace WebAutomatization.Core {
    public interface IBrowser {
        IPage Page { get; }
    }
    public interface IBrowser<out T> : IBrowser where T : IWebDriver {
        Browsers Type { get; }
        T Driver { get; }
        IJavaScript JavaScript { get; }
    }
}