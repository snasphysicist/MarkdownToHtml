
namespace MarkdownToHtml
{
    public class MarkdownText : IHtmlable
    {
        string content;

        public MarkdownElementType Type
        { get; private set; }

        public MarkdownText(
            string content
        ) {
            this.content = content;
        }

        public string ToHtml() {
            return content;
        }

    }
}