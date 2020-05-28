
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public abstract class MarkdownElement
    {
        public MarkdownElementType Type
        { get; protected set; }

        protected IHtmlable[] content;

        protected string tag;
    }
}
