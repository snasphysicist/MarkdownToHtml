
namespace MarkdownToHtml
{
    public class MarkdownLinebreak : IHtmlable
    {

        public const MarkdownElementType Type = MarkdownElementType.Linebreak;

        public MarkdownLinebreak() 
        {}

        public string ToHtml() 
        {
            return $"{Type.TagFor()}";
        }

    }
}
