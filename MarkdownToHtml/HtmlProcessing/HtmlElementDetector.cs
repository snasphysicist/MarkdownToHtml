
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public enum LineBreaksAroundBlocks {
        Required,
        NotRequired
    }

    public class HtmlElementDetector
    {
        private HtmlSnippet[] toScan;

        private LineBreaksAroundBlocks lineBreaks;

        public HtmlElementDetector(
            HtmlSnippet[] toScan,
            LineBreaksAroundBlocks lineBreaks
        ) {
            this.toScan = toScan;
            this.lineBreaks = lineBreaks;
        }

        public static HtmlElement[] ElementsFromTags(
            HtmlSnippet[] snippets,
            LineBreaksAroundBlocks lineBreaks
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
                    ).ToArray(),
                    lineBreaks
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
            int depth = 0;
            if (
                toScan.Length <= current
                || !toScan[current].IsTag() 
                || toScan[current].Tag.Name.Type != HtmlDisplayType.Inline 
                || toScan[current].Tag.Type != HtmlTagType.Opening
            ) {
                return null;
            }
            HtmlTagName groupName = toScan[current].Tag.Name;
            depth++;
            current++;
            while (depth != 0 && toScan.Length > current) {
                if (toScan[current].IsTag() && toScan[current].Tag.Name.Name == groupName.Name)
                {
                    if (toScan[current].Tag.Type == HtmlTagType.Opening)
                    {
                        depth++;
                    }
                    if (toScan[current].Tag.Type == HtmlTagType.Closing)
                    {
                        depth--;
                    }
                }
                if (depth != 0)
                {
                    current++;
                }
            }
            if (current == toScan.Length)
            {
                return null;
            }
            HtmlElement[] contents = ElementsFromTags(
                new ArraySegment<HtmlSnippet>(
                    toScan,
                    1,
                    current - 1
                ).ToArray(),
                lineBreaks
            );
            foreach (HtmlElement contained in contents)
            {
                if (contained.IsTagGroup && contained.GroupDisplayType().Type == HtmlDisplayType.Block)
                {
                    return null;
                }
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
            int depth = 0;
            if (lineBreaks == LineBreaksAroundBlocks.Required) 
            {
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
                current++;
            }
            if (
                toScan.Length <= current
                || !toScan[current].IsTag() 
                || toScan[current].Tag.Name.Type != HtmlDisplayType.Block 
                || toScan[current].Tag.Type != HtmlTagType.Opening
            ) {
                return null;
            }
            HtmlTagName groupName = toScan[current].Tag.Name;
            current++;
            depth++;
            while (depth != 0 && toScan.Length > current) {
                if (toScan[current].IsTag() && toScan[current].Tag.Name.Name == groupName.Name)
                {
                    if (toScan[current].Tag.Type == HtmlTagType.Opening)
                    {
                        depth++;
                    }
                    if (toScan[current].Tag.Type == HtmlTagType.Closing)
                    {
                        depth--;
                    }
                }
                if (depth != 0)
                {
                    current++;
                }
            }
            if (current == toScan.Length) {
                return null;
            }
            current++;
            if (lineBreaks == LineBreaksAroundBlocks.Required)
            {
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
                current++;
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