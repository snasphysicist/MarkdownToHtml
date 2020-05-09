

namespace MarkdownToHtml
{
    public class MarkdownStrikethrough : IHtmlable
    {

        IHtmlable[] content;

        const string tag = "s";

        public const MarkdownElementType Type = MarkdownElementType.Strikethrough;

        public MarkdownStrikethrough(
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
