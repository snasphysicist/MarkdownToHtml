
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class MarkdownParagraph : IHtmlable
    {

        IHtmlable[] content;

        const string tag = "p";

        public const MarkdownElementType Type = MarkdownElementType.Paragraph;

        public MarkdownParagraph(
            LinkedList<IHtmlable> innerContent
        ) {
            this.content = new IHtmlable[innerContent.Count];
            innerContent.CopyTo(
                this.content, 
                0
            );
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