using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using WebAutomatization.Core;

namespace WebAutomatization.Domain.Registration {
    public interface IWebDataRegistration {
        Uri SiteUrl { get; }
        IEnumerable<IStep> Steps { get; }
    }

    public class ActionStep : IStep {
        private readonly ActionTag tag;
        public ActionStep(ActionTag tag) { this.tag = tag; }

        public void Action(IBrowser browser) {
            browser.Page.Tag.By(tag.Tag.Attribute, tag.Tag.Key)
                .AsActionElement()
                .Action(tag.Action);
        }
    }
    public class InputStep : IStep {
        private readonly InputTag tag;
        public InputStep(InputTag tag) { this.tag = tag; }

        public void Action(IBrowser browser) {
            browser.Page.Tag.By(tag.Tag.Attribute, tag.Tag.Key)
                .AsInputElement()
                .Text = tag.Data;
        }
    }

    public interface IStep {
        void Action(IBrowser browser);
    }

}
