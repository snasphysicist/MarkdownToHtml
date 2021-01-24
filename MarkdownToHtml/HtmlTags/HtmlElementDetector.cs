
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
            HtmlElement foundElement = InlineGroupAtStart();
            if (foundElement != null) {
                return foundElement;
            }
            foundElement = BlockGroupAtStart();
            if (foundElement != null) {
                return foundElement;
            }
            return new HtmlElement(
                toScan[0],
                toScan[0].IsTag() && toScan[0].Tag.Type == HtmlTagType.SelfClosing
            );
        }

        private HtmlElement InlineGroupAtStart()
        {   
            int current = 0;
            if (
                toScan.Length <= current
                || !toScan[current].IsTag() 
                || toScan[current].Tag.Name.Type != HtmlDisplayType.Inline 
                || toScan[current].Tag.Type != HtmlTagType.Opening
            ) {
                return null;
            }
            current++;
            while (toScan.Length > current && !toScan[current].IsTag())
            {
                current++;
            }
            if (
                toScan.Length <= current
                || !toScan[current].IsTag()
                || toScan[current].Tag.Type != HtmlTagType.Closing
                || toScan[current].Tag.Name.Type != HtmlDisplayType.Inline
            ) {
                return null;
            }
            current++;
            return new HtmlElement(
                new ArraySegment<HtmlSnippet>(
                    toScan,
                    0,
                    current
                ).ToArray(),
                true
            );
        }

        public HtmlElement BlockGroupAtStart()
        {
            int current = 0;
            if (
                toScan.Length <= current
                || !toScan[current].IsToken() 
                || toScan[current].Token.Type != HtmlTokenType.LineBreakingWhitespace
            ) {
                return null;
            }
            current++;
            if (
                toScan.Length <= current
                || !toScan[current].IsToken() 
                || toScan[current].Token.Type != HtmlTokenType.LineBreakingWhitespace
            ) {
                return null;
            }
            if (
                toScan.Length <= current
                || !toScan[current].IsTag() 
                || toScan[current].Tag.Name.Type != HtmlDisplayType.Block 
                || toScan[current].Tag.Type != HtmlTagType.Opening
            ) {
                return null;
            }
            current++;
            while (toScan.Length > current && !toScan[current].IsTag())
            {
                current++;
            }
            if (
                toScan.Length <= current
                || !toScan[current].IsTag()
                || toScan[current].Tag.Type != HtmlTagType.Closing
                || toScan[current].Tag.Name.Type != HtmlDisplayType.Block
            ) {
                return null;
            }
            current++;
            if (
                toScan.Length <= current
                || !toScan[current].IsToken() 
                || toScan[current].Token.Type != HtmlTokenType.LineBreakingWhitespace
            ) {
                return null;
            }
            current++;
            if (
                toScan.Length <= current
                || !toScan[current].IsToken() 
                || toScan[current].Token.Type != HtmlTokenType.LineBreakingWhitespace
            ) {
                return null;
            }
            return new HtmlElement(
                new ArraySegment<HtmlSnippet>(
                    toScan,
                    0,
                    current
                ).ToArray(),
                true
            );
        }
    }
}