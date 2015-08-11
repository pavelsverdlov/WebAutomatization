namespace WebAutomatization.Core.Page
{
    public interface ITagSearchBy : ITag {
        IInputTag AsInputElement();
        IActionTag AsActionElement();
    }
}