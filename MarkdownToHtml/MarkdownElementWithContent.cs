
namespace MarkdownToHtml
{
    public abstract class MarkdownElementWithContent : MarkdownElementBase, IHtmlable
    {

        protected IHtmlable[] content;

        public override string ToHtml() {
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
