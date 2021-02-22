
namespace MarkdownToHtml
{
    public class TagOnlyWriter : HtmlWriterBase, IHtmlWriter
    {
        public string WriteToString(
            ElementDetails details
        ) {
            return TerminateWithNewLineIfRequired(details, $"<{details.Tag}>");
        }
    }
}