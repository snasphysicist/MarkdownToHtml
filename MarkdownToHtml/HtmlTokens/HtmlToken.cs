
namespace MarkdownToHtml
{
    public class HtmlToken
    {
        public HtmlTokenType Type
        { get; private set; }

        public string Content
        { get; private set; }

        public HtmlToken(
            HtmlTokenType type,
            string content
        ) {
            Type = type;
            Content = content;
        }
    }
}