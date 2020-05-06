
using System.Collections.Generic.Dictionary;
using System.Collections.Generic.LinkedList;

namespace MarkdownToHtml
{
    public class MarkdownElement
    {

        MarkdownElementType Type
        { get; set; }

        LinkedList<IHtmlable> Content
        { get; private set; }

        Dictionary<String, String> Attributes
        { get; private set; }

        MarkdownElement(
            MarkdownElementType type
        ) {
            Type = type;
            Content = new LinkedList<Htmlable>();
            Attributes = new Dictionary<String, String>();
        }

    }
}
