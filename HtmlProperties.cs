
using System.Collections.Generic.LinkedList;

namespace MarkdownToHtml
{
    public class HtmlProperties
    {

        LinkedList<String> requiredAttributes;

        string Tag
        { get; set; }

        HtmlProperties (
            string tag
        ) {
            Tag = tag;
            requiredAttributes = new LinkedList<String>();
        }

    }
}
