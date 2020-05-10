
namespace MarkdownToHtml
{
    public class MarkdownHorizontalRule : IHtmlable
    {

        string tag = "hr";

        public const MarkdownElementType Type = MarkdownElementType.HorizontalRule;

        public MarkdownHorizontalRule() 
        {}

        public string ToHtml() {
            return $"<{tag}>";
        }

    }
}
