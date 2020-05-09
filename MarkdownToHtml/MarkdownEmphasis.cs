
namespace MarkdownToHtml
{
    public class MarkdownEmphasis : IHtmlable
    {

        IHtmlable[] content;

        const string tag = "emph";

        public const MarkdownElementType Type = MarkdownElementType.Emphasis;

        public MarkdownEmphasis(
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