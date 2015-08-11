namespace WebAutomatization.Core {
    public struct TagAttribute {
        public readonly string Attribute;
        public TagAttribute(string attribute) : this() { Attribute = attribute; }

        public override string ToString() { return Attribute; }

        public static implicit operator string(TagAttribute x) {
            return x.Attribute;
        }


        public readonly static TagAttribute Id = new TagAttribute("id");
        public readonly static TagAttribute Name = new TagAttribute("name");
        public readonly static TagAttribute Class = new TagAttribute("class");
        public readonly static TagAttribute Value = new TagAttribute("value");
        public readonly static TagAttribute OnClick = new TagAttribute("onclick");
        public readonly static TagAttribute Src = new TagAttribute("src");
        public readonly static TagAttribute Title = new TagAttribute("title");
        public readonly static TagAttribute Href = new TagAttribute("href");
        public readonly static TagAttribute Type = new TagAttribute("type");
        public readonly static TagAttribute Style = new TagAttribute("style");
        public readonly static TagAttribute Rel = new TagAttribute("rel");
    }
}
