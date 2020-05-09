
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class MarkdownElement
    {

        MarkdownElementType Type
        { get; set; }

        LinkedList<IHtmlable> content;

        Dictionary<string, string> attributes;

        MarkdownElement(
            MarkdownElementType type
        ) {
            Type = type;
            content = new LinkedList<IHtmlable>();
            attributes = new Dictionary<string, string>();
        }

    }
}
