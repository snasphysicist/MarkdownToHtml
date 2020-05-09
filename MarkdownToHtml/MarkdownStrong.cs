
namespace MarkdownToHtml
{
    public class MarkdownStrong : IHtmlable
    {

        IHtmlable[] content;

        const string tag = "strong";

        public const MarkdownElementType Type = MarkdownElementType.Strong;

        public MarkdownStrong(
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
