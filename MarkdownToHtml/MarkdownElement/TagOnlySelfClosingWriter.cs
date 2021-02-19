
namespace MarkdownToHtml
{
    public class TagOnlySelfClosingWriter : HtmlWriterBase, IHtmlWriter
    {
        public string WriteToString(
            ElementDetails details
        ) {
            return TerminateWithNewLineIfRequired(details, $"<{details.Tag} />");
        }
    }
}