
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownParser
    {
        
        Regex regexOrderedListItem = new Regex(
            @"^\s*\d+\."        // Format 1. (ordered list)
        );

        Regex regexUnorderedListItem = new Regex(
            @"^\s*\+\s+|"       // Format +  (unordered list)
            + @"^\s*\*\s+|"     // Format *  (unordered list)
            + @"^\s*-\s+"       // Format -  (unordered list)
        );

        public bool Success
        { get; private set; }

        public LinkedList<IHtmlable> Content
        { get; private set; }

        public IHtmlable[] ContentAsArray()
        {
            IHtmlable[] contentArray = new IHtmlable[Content.Count];
            Content.CopyTo(
                contentArray,
                0
            );
            return contentArray;
        }

        // TODO replace with MarkdownDocument object
        public string ToHtml()
        {
            string html = "";
            foreach (IHtmlable entry in Content)
            {
                html += entry.ToHtml();
            }
            return html;
        }

        public MarkdownParser(
            string[] lines
        ) {
            // Assume success
            Success = true;
            // Store parsed content as we go
            Content = new LinkedList<IHtmlable>();
            int currentIndex = 0;
            while (currentIndex < lines.Length) {
                // Parse some lines
                ParseLineGroup(
                    new ArraySegment<string>(
                        lines,
                        currentIndex,
                        lines.Length - currentIndex
                    )
                );
                // Move to the next non-empty line
                while (
                    (currentIndex < lines.Length)
                    && ContainsOnlyWhitespace(lines[currentIndex])
                ) {
                    currentIndex++;
                }
            }
        }

        // Check whether a line contains only whitespace (or is empty)
        private bool ContainsOnlyWhitespace(
            string line
        ) {
            return line.Replace(
                " ",
                ""
            ).Length == 0;
        }

        private void ParseLineGroup(
            ArraySegment<string> lines
        ) {
            ParseResult result;
            if (MarkdownHeading.CanParseFrom(lines))
            {
                result = MarkdownHeading.ParseFrom(lines);
            } else if (MarkdownHorizontalRule.CanParseFrom(lines))
            {
                result = MarkdownHorizontalRule.ParseFrom(lines);
            } else if (MarkdownQuote.CanParseFrom(lines))
            {
                result = MarkdownQuote.ParseFrom(lines);
            } else if (MarkdownCodeBlock.CanParseFrom(lines))
            {
                result = MarkdownCodeBlock.ParseFrom(lines);
            } else {
                result = MarkdownParagraph.ParseFrom(lines);
            }
            foreach (IHtmlable entry in result.GetContent())
            {
                Content.AddLast(entry);
            }
        }

        // Given a single line of text, parse this, including special (emph, etc...) sections
        public static IHtmlable[] ParseInnerText(
            string line
        ) {
            // Store parsed content as we go
            LinkedList<IHtmlable> content = new LinkedList<IHtmlable>();
            // Until the whole string has been consumed
            while (line.Length > 0)
            {
                ParseResult result;
                if (MarkdownStrong.CanParseFrom(line))
                {
                    result = MarkdownStrong.ParseFrom(line);
                } else if (MarkdownStrikethrough.CanParseFrom(line))
                {
                    result = MarkdownStrikethrough.ParseFrom(line);
                } else if (MarkdownEmphasis.CanParseFrom(line))
                {
                    result = MarkdownEmphasis.ParseFrom(line);
                } else if (MarkdownCodeInline.CanParseFrom(line))
                {
                    result = MarkdownCodeInline.ParseFrom(line);
                } else {
                    result = MarkdownText.ParseFrom(
                        line,
                        false
                    );
                }
                /*
                 * If no parsing method suceeded
                 * for once character to be parsed as text
                 */
                if (!result.Success)
                {
                    result = MarkdownText.ParseFrom(
                        line,
                        true
                    );
                }
                // Extract parsed content
                foreach (IHtmlable entry in result.GetContent())
                {
                    content.AddLast(entry);
                }
                // Update text to be parsed
                line = result.Line;
            }
            IHtmlable[] contentArray = new IHtmlable[content.Count];
            content.CopyTo(
                contentArray, 
                0
            );
            return contentArray;
        }

        private T[] LinkedListToArray<T>(
            LinkedList<T> linkedList
        ) {
            T[] array = new T[linkedList.Count];
            linkedList.CopyTo(array, 0);
            return array;
        }

    }
}
