using OpenQA.Selenium;

namespace WebAutomatization.Core
{
    public interface IBrowserWebDriver<T> where T : IWebDriver {
        IBrowser<T> Create();
    }
}