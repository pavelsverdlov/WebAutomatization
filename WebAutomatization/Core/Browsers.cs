using System;
using System.ComponentModel;

namespace WebAutomatization.Core
{
    [Serializable]
    public enum Browsers {
        [Description("Windows Internet Explorer")]
        InternetExplorer,

        [Description("Mozilla Firefox")]
        Firefox,

        [Description("Google Chrome")]
        Chrome
    }
}