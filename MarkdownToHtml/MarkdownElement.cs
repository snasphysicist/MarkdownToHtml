
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public abstract class MarkdownElement
    {
        MarkdownElementType Type
        { get; set; }

        IHtmlable[] content;

        private string tag;
    }
}
