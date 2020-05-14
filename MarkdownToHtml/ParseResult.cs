
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class ParseResult
    {

        private LinkedList<IHtmlable> content;

        public string Line
        { get; set; }

        public bool Success
        { get; set; }

        public ParseResult()
        {
            content = new LinkedList<IHtmlable>();
            Line = "";
            Success = false;
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