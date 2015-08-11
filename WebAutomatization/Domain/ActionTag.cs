using WebAutomatization.Core.JS;

namespace WebAutomatization.Domain {
    public struct ActionTag {
        public readonly Tag Tag;
        public readonly JavaScriptEvent Action;
        public ActionTag(Tag tag, JavaScriptEvent action)
            : this() {
            Action = action;
            Tag = tag;
        }
    }
}