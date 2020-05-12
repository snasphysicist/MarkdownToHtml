
using System.Collections.Generic;

namespace MarkdownToHtml
{
    class ParseResult
    {

        private LinkedList<IHtmlable> content;

        public string Line
        { get; set; }

        public string Success
        { get; set; }

        public ParseResult()
        {
            content = new LinkedList<IHtmlable>();
        }

        public void AddContent(
            IHtmlable element
        ) {
            content.AddLast(element);
        }

        public IHtmlable[] GetContent()
        {
            IHtmlable[] contentArray = new IHtmlable[content.Count];
            content.CopyTo(contentArray, 0);
            return contentArray;
        }

    }
}