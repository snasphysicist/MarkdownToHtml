
namespace MarkdownToHtml
{
    public class MarkdownHeading : IHtmlable
    {

        IHtmlable[] content;

        string tag;

        public MarkdownElementType Type
        { get; private set; }
        
        public MarkdownHeading(
            int level,
            IHtmlable[] content
        ) {
            this.content = content;
            if ((level > 0) && (level < 7))
            {
                this.tag = $"h{level}";
            } else {
                // If not a valid heading level, fall back to paragraph
                this.tag = "p";
            }
            switch (level)
            {
                case 1:
                    Type = MarkdownElementType.Heading1;
                    break;
                case 2:
                    Type = MarkdownElementType.Heading2;
                    break;
                case 3:
                    Type = MarkdownElementType.Heading3;
                    break;
                case 4:
                    Type = MarkdownElementType.Heading4;
                    break;
                case 5:
                    Type = MarkdownElementType.Heading5;
                    break;
                case 6:
                    Type = MarkdownElementType.Heading6;
                    break;
                default:
                    Type = MarkdownElementType.Paragraph;
                    break;
            }
        }

        public string ToHtml() 
        {
            string html = $"<{tag}>";
            foreach (IHtmlable htmlable in content)
            {
                html += htmlable.ToHtml();
            }
            html += $"</{tag}>";
            return html;
        }

    }
}
