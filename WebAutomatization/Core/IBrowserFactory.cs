using OpenQA.Selenium;

namespace WebAutomatization.Core
{
    public interface IBrowserFactory {
        IBrowser Create<T>() where T : IWebDriver;
    }
}