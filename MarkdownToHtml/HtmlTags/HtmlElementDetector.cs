
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class HtmlElementDetector
    {
        private HtmlSnippet[] toScan;

        public HtmlElementDetector(
            HtmlSnippet[] toScan
        ) {
            this.toScan = toScan;
        }

        public static HtmlElement[] ElementsFromTags(
            HtmlSnippet[] snippets
        ) {
            if (snippets.Length == 0) {
                return new HtmlElement[0];
            }
            int currentToken = 0;
            LinkedList<HtmlElement> elements = new LinkedList<HtmlElement>();
            while (currentToken < snippets.Length)
            {
                HtmlElementDetector detector = new HtmlElementDetector(
                    new ArraySegment<HtmlSnippet>(
                        snippets,
                        currentToken,
                        snippets.Length - currentToken
                    ).ToArray()
                );
                HtmlElement element = detector.Detect();
                elements.AddLast(element);
                currentToken = currentToken + element.Count();
            }
            return elements.ToArray();
        }

        public HtmlElement Detect()
        {
            HtmlElement inlineGroup = InlineGroupAtStart();
            if (inlineGroup != null) {
                return inlineGroup;
            }
            return new HtmlElement(
                toScan[0],
                toScan[0].IsTag() && toScan[0].Tag.Type == HtmlTagType.SelfClosing
            );
        }

        private HtmlElement InlineGroupAtStart()
        {
            if (
                toScan.Length < 1
                || !toScan[0].IsTag() 
                || toScan[0].Tag.Name.Type != HtmlDisplayType.Inline 
                || toScan[0].Tag.Type != HtmlTagType.Opening
            ) {
                return null;
            }
            if (
                toScan.Length < 2
                || !toScan[1].IsTag()
                || toScan[1].Tag.Type != HtmlTagType.Closing
                || toScan[1].Tag.Name.Type != HtmlDisplayType.Inline
            ) {
                return null;
            }
            return new HtmlElement(
                new ArraySegment<HtmlSnippet>(
                    toScan,
                    0,
                    2
                ).ToArray(),
                true
            );
        }
    }
}