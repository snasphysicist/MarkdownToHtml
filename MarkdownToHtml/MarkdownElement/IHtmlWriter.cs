
namespace MarkdownToHtml
{
    public interface IHtmlWriter
    {
        public string WriteToString(
            ElementDetails details
        );
    }
}