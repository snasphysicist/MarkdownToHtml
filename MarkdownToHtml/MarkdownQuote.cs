
namespace MarkdownToHtml
{
    public class MarkdownQuote : IHtmlable
    {

        IHtmlable[] content;

        const string tag = "blockquote";

        public const MarkdownElementType Type = MarkdownElementType.Quote;

        public MarkdownQuote(
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
