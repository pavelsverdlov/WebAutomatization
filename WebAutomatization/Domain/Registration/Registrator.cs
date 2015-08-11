//http://habrahabr.ru/post/180357/

using System;
using OpenQA.Selenium;
using WebAutomatization.Core;

namespace WebAutomatization.Domain.Registration {
    public sealed class Registrator<T> where T : IWebDriver {
        private readonly IBrowser browser;
        public Registrator(IBrowserFactory factory) {
            browser = factory.Create<T>();
        }

        public void TryRegister(IWebDataRegistration data, Action<int> processing) {
            browser.Page.GoToUrl(data.SiteUrl);
            var index = 0;
            foreach (var step in data.Steps) {
                step.Action(browser);
                processing(index);
                ++index;
            }


        }

    }
}
