
namespace MarkdownToHtml
{
    public abstract class MarkdownElement
    {
        public MarkdownElementType Type
        { get; protected set; }

        protected IHtmlable[] content;

        protected string Tag {
            get {
                return Type.TagFor();
            }
        }
    }
}
