
namespace MarkdownToHtml
{
    public class TagOnlySelfClosingWriter : IHtmlWriter
    {
        public string WriteToString(
            ElementDetails details
        ) {
            return $"<{details.Tag} />";
        }
    }
}