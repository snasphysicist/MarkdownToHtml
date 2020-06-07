
namespace MarkdownToHtml
{
    public class MarkdownLinebreak : MarkdownElementBase, IHtmlable
    {
        public MarkdownLinebreak() 
        {
            Type = MarkdownElementType.Linebreak;
        }
    }
}
