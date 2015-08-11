using WebAutomatization.Core;

namespace WebAutomatization.Domain {
    public struct Tag {
        public readonly TagAttribute Attribute;
        public readonly string Key;
        public Tag(TagAttribute attribute, string key)
            : this() {
            Attribute = attribute;
            Key = key;
        }
    }
}