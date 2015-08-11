namespace WebAutomatization.Domain {
    public struct InputTag {
        public readonly string Data;
        public readonly Tag Tag;
        public InputTag(Tag tag, string data)
            : this() {
            Tag = tag;
            Data = data;
        }
    }
}
