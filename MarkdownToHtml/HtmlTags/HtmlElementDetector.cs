
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
            return new HtmlElement(
                toScan[0],
                false
            );
        }
    }
}