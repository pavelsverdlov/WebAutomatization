using OpenQA.Selenium;

namespace WebAutomatization.Core
{
    public abstract class ABrowserFactory : IBrowserFactory {
        public IBrowser Create<T>() where T : IWebDriver {
            var factoryMethod = this as IBrowserWebDriver<T>;
            if (factoryMethod != null) {
                return factoryMethod.Create();
            }
            return default(IBrowser<T>);
        }

    }
}