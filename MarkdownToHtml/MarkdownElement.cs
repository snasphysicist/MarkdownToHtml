
using System.Collections.Generic.Dictionary;
using System.Collections.Generic.LinkedList;

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
            content = new LinkedList<Htmlable>();
            attributes = new Dictionary<string, string>();
        }

    }
}
