
namespace MarkdownToHtml
{
    public class MarkdownCodeBlock : IHtmlable
    {

        IHtmlable[] content;

        const string tag = "code";

        public const MarkdownElementType Type = MarkdownElementType.CodeInline;

        public MarkdownCodeBlock(
            IHtmlable[] content
        ) {
            this.content = content;
        }

        public string ToHtml() 
        {
            string html = $"<{tag}>";
            foreach (IHtmlable htmlable in content)
            {
                html += htmlable.ToHtml();
            }
            html += $"</{tag}>";
            return html;
        }

    }
}
