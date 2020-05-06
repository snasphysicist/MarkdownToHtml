
using System.Collections.Generic.Dictionary;

namespace MarkdownToHtml
{
    public class MarkdownElement
    {

        MarkdownElementType Type
        { get; set; }

        IHtmlable[] Content
        { get; private set; }

        Dictionary Attributes
        { get; private set; }

    }
}