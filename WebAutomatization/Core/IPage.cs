using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using WebAutomatization.Core.Page;

namespace WebAutomatization.Core
{
    public interface IPage {
        void GoToUrl(Uri url);
        IEnumerable<IWebElement> FindElements(By selector);
        void NavigateBack();
        void Refresh();

        ITag Tag { get; }
    }
}