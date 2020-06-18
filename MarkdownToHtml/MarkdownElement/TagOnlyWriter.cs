
namespace MarkdownToHtml
{
    public class TagOnlyWriter : IHtmlWriter
    {
        public string WriteToString(
            ElementDetails details
        ) {
            return $"<{details.Tag}>";
        }
    }
}