
namespace MarkdownToHtml
{
    public abstract class MarkdownElement : IHtmlable
    {
        public MarkdownElementType Type
        { get; protected set; }

        protected IHtmlable[] content;

        protected string Tag {
            get {
                return Type.Tag();
            }
        }

        public string ToHtml() {
            string html = $"<{Tag}>";
            foreach (IHtmlable htmlable in content)
            {
                html += htmlable.ToHtml();
            }
            html += $"</{Tag}>";
            return html;
        }
    }
}
