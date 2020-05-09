
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class HtmlProperties
    {

        LinkedList<string> requiredAttributes;

        string Tag
        { get; set; }

        HtmlProperties (
            string tag
        ) {
            Tag = tag;
            requiredAttributes = new LinkedList<string>();
        }

    }
}
