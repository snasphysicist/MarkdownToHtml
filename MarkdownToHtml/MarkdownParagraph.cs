
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class MarkdownParagraph : IHtmlable
    {

        IHtmlable[] content;

        const string tag = "p";

        public const MarkdownElementType Type = MarkdownElementType.Paragraph;

        public MarkdownParagraph(
            IHtmlable[] innerContent
        ) {
            content = innerContent;
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