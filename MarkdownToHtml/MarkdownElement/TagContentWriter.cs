
namespace MarkdownToHtml
{
    public class TagContentWriter : HtmlWriterBase, IHtmlWriter
    {
        public string WriteToString(
            ElementDetails details
        ) {
            string html = $"<{details.Tag}>";
            foreach (IHtmlable htmlable in details.Content())
            {
                html += htmlable.ToHtml();
            }
            html += $"</{details.Tag}>";
            return TerminateWithNewLineIfRequired(details, html);
        }
    }
}