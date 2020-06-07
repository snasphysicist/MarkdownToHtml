
namespace MarkdownToHtml
{
    public abstract class MarkdownElementBase : IHtmlable
    {
        public MarkdownElementType Type
        { get; protected set; }

        protected string Tag {
            get {
                return Type.Tag();
            }
        }

        public virtual string ToHtml() {
            return $"<{Tag}>";
        }
    }
}
