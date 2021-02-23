
using System;

namespace MarkdownToHtml
{
    public class HtmlElement
    {
        private HtmlSnippet[] sequence;

        public bool IsTagGroup
        { get; private set; }

        public HtmlElement(
            HtmlSnippet snippet,
            bool isTagGroup
        ) {
            this.sequence = new HtmlSnippet[]
            {
                snippet
            };
            IsTagGroup = isTagGroup;
        }

        public HtmlElement(
            HtmlSnippet[] sequence,
            bool isTagGroup
        ) {
            this.sequence = sequence;
            IsTagGroup = isTagGroup;
        }

        public int Count()
        {
            return sequence.Length;
        }

        public HtmlTagName GroupDisplayType()
        {
            if (!IsTagGroup) {
                return null;
            }
            for (int i = 0; i < Count(); i++)
            {
                if (sequence[i].IsTag())
                {
                    return sequence[i].Tag.Name;
                }
            }
            return null;
        }

        public string AsHtmlString()
        {
            string html = "";
            foreach (HtmlSnippet snippet in sequence)
            {
                html = html + snippet.AsHtmlString();
            }
            return html;
        }

        public HtmlSnippet[] Contents()
        {
            return new ArraySegment<HtmlSnippet>(
                sequence,
                0,
                sequence.Length
            ).ToArray();
        }
    }
}