using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using OpenQA.Selenium.Firefox;
using WebAutomatization.Core;
using WebAutomatization.Core.JS;
using WebAutomatization.Domain;
using WebAutomatization.Domain.Registration;

namespace WebAutomatization.Registration.View {
    internal class WebData : IWebDataRegistration {
        public WebData() {
            SiteUrl = new Uri("https://www.etsy.com/?ref=lgo");

            //            var steps = new List<IStep>();

            //            steps.Add(new ActionStep(new ActionTag(new Tag(TagAttribute.Id, "register"), JavaScriptEvent.Click)));
            //            steps.Add(new InputStep(new InputTag(new Tag(TagAttribute.Id, "first-name"), "First Name")));
            //            steps.Add(new InputStep(new InputTag(new Tag(TagAttribute.Id, "password"), "Password")));

            //            Steps = steps;
        }

        public Uri SiteUrl { get; set; }
        public IEnumerable<IStep> Steps { get; set; }
    }

    public enum StepTypes {
        Action,
        Input
    }
    public sealed class StepViewModel : BindableBase {
        private string status;
        private bool isInputDataEnabled;
        private bool isJSEventsEnabled;
        public ICollectionView StepType { get; set; }
        public string TagAttribute { get; set; }
        public string TagAttributeKey { get; set; }
        public string InputData { get; set; }
        public ICollectionView JavaScriptEvents { get; set; }
        public string Status { get { return status; } set { SetProperty(ref status, value); } }

        public bool IsInputDataEnabled { get { return isInputDataEnabled; } set { SetProperty(ref isInputDataEnabled, value); } }
        public bool IsJSEventsEnabled { get { return isJSEventsEnabled; } set { SetProperty(ref isJSEventsEnabled, value); } }
        public bool IsSelected { get; set; }

        private readonly ObservableCollection<StepTypes> StepTypeCollection = new ObservableCollection<StepTypes>
        {
            StepTypes.Action, StepTypes.Input
        };
        public StepViewModel(StepTypes type, JavaScriptEvent ev) {
            IsSelected = true;
            Status = null;
            StepType = CollectionViewSource.GetDefaultView(StepTypeCollection);
            JavaScriptEvents = new ListCollectionView(new[] { JavaScriptEvent.Click, JavaScriptEvent.KeyUp });

            StepType.CurrentChanged += OnStepTypeChanged;
            JavaScriptEvents.CurrentChanged += OnJavaScriptEventsChanged;

            StepType.MoveCurrentTo(type);
            JavaScriptEvents.MoveCurrentTo(ev);
        }

        private void OnJavaScriptEventsChanged(object sender, EventArgs e) {
            IsJSEventsEnabled = IsInputDataEnabled = true;
//            switch (GetCurrentStepType()) {
//                case StepTypes.Action:
//                    IsInputDataEnabled = false;
//                    break;
//                case StepTypes.Input:
//                    IsJSEventsEnabled = false;
//                    break;
//            }
        }

        private void OnStepTypeChanged(object sender, EventArgs e) {

        }


        public StepTypes GetCurrentStepType() {
            return (StepTypes)StepType.CurrentItem;
        }

        public JavaScriptEvent GetCurrentJSEvent() {
            return (JavaScriptEvent)JavaScriptEvents.CurrentItem;
        }

        public void Success() { Status = "Success"; }
        public void ClearStatus() { Status = null; }
    }
    public sealed class RegistrationViewModel : BindableBase {
        private bool _isEnabled;

        public bool IsEnabled {
            get { return _isEnabled; }
            set {
                SetProperty(ref _isEnabled, value, "IsEnabled");
            }
        }

        public string Site { get; set; }
        public ObservableCollection<StepViewModel> Steps { get; private set; }
        public ICommand Run { get; private set; }
        public ICommand AddNewStep { get; private set; }
        public ICommand RemoveSelected { get; private set; }
        public RegistrationViewModel() {
            IsEnabled = true;
            Site = "https://www.etsy.com/?ref=lgo";
            Steps = new ObservableCollection<StepViewModel>();
            Run = DelegateCommand.FromAsyncHandler(OnRun, CanSignIn);
            AddNewStep = DelegateCommand.FromAsyncHandler(OnAddNewStep, CanSignIn);
            RemoveSelected = DelegateCommand.FromAsyncHandler(OnRemoveSelected, CanSignIn);

            Steps.Add(new StepViewModel(StepTypes.Action, JavaScriptEvent.Click) {
                TagAttribute = "id", TagAttributeKey = "register"
            });
            Steps.Add(new StepViewModel(StepTypes.Input, new JavaScriptEvent()) {
                TagAttribute = "id", TagAttributeKey = "first-name", InputData = "First Name"
            });
            Steps.Add(new StepViewModel(StepTypes.Input, new JavaScriptEvent()) {
                TagAttribute = "id", TagAttributeKey = "password", InputData = "Password"
            });
        }

        private Task OnRemoveSelected() {
            foreach (var step in Steps.ToList()) {
                if (step.IsSelected) {
                    Steps.Remove(step);
                }
            }
            return Task.FromResult(0);
        }

        private Task OnAddNewStep() {
            return Dispatcher.CurrentDispatcher.BeginInvoke(
                new Action(() => Steps.Add(new StepViewModel(StepTypes.Action, JavaScriptEvent.Click))))
                .Task;
        }

        private bool CanSignIn() { return true; }

        private Task OnRun() {
            foreach (var step in Steps) {
                step.ClearStatus();
            }
            return Task.Run(() => {
                IsEnabled = false;
                var reg = new Registrator<FirefoxDriver>(new BrowserFactory());
                var data = new WebData();
                data.SiteUrl = new Uri(Site);
                var s = new List<IStep>();
                foreach (var step in Steps) {
                    if (!step.IsSelected) { continue; }
                    var tag = new Tag(new TagAttribute(step.TagAttribute), step.TagAttributeKey);
                    switch (step.GetCurrentStepType()) {
                        case StepTypes.Action:
                            s.Add(new ActionStep(new ActionTag(tag, step.GetCurrentJSEvent())));
                            break;
                        case StepTypes.Input:
                            s.Add(new InputStep(new InputTag(tag, step.InputData)));
                            break;
                    }
                }
                data.Steps = s;
                reg.TryRegister(data, StepProcesed);
            });
        }

        private void StepProcesed(int index) {
            Steps[index].Success();
        }
    }
}
