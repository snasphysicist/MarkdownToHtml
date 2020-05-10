
namespace MarkdownToHtml
{
    public class MarkdownLinebreak : IHtmlable
    {

        public const MarkdownElementType Type = MarkdownElementType.Linebreak;

        const string html = "<br/>";

        public MarkdownLinebreak() 
        {}

        public string ToHtml() 
        {
            return html;
        }

    }
}
