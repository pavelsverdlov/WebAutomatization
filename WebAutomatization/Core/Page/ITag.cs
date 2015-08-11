namespace WebAutomatization.Core.Page
{
    public interface ITag {
        ITagSearchBy By(TagAttribute by, string key);
    }
}