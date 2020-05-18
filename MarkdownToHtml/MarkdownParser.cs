
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

        private LinkedList<ReferencedUrl> Urls
        { get; set; }

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
            // Urls potentially referenced by anchors/images
            Urls = new LinkedList<ReferencedUrl>();
            // Extract all 'footnote' style urls
            ParseReferencedUrls(lines);
            // Create new parse input to pass to parsers
            ParseInput input = new ParseInput(
                Utils.LinkedListToArray(Urls),
                lines,
                0,
                lines.Length
            );
            // Parsing printed content
            int currentIndex = 0;
            while (currentIndex < lines.Length) {
                // Parse some lines
                ParseLineGroup(
                    input.Slice(
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
            ParseInput input
        ) {
            ParseResult result;
            if (MarkdownHeading.CanParseFrom(input))
            {
                result = MarkdownHeading.ParseFrom(input);
            } else if (MarkdownHorizontalRule.CanParseFrom(input))
            {
                result = MarkdownHorizontalRule.ParseFrom(input);
            } else if (MarkdownQuote.CanParseFrom(input))
            {
                result = MarkdownQuote.ParseFrom(input);
            } else {
                result = MarkdownParagraph.ParseFrom(input);
            }
            foreach (IHtmlable entry in result.GetContent())
            {
                Content.AddLast(entry);
            }
        }

        // Given a single line of text, parse this, including special (emph, etc...) sections
        public static IHtmlable[] ParseInnerText(
            ParseInput input
        ) {
            // Store parsed content as we go
            LinkedList<IHtmlable> content = new LinkedList<IHtmlable>();
            // Until the whole string has been consumed
            while (input.FirstLine.Length > 0)
            {
                ParseResult result;
                if (MarkdownStrong.CanParseFrom(input))
                {
                    result = MarkdownStrong.ParseFrom(input);
                } else if (MarkdownStrikethrough.CanParseFrom(input))
                {
                    result = MarkdownStrikethrough.ParseFrom(input);
                } else if (MarkdownEmphasis.CanParseFrom(input))
                {
                    result = MarkdownEmphasis.ParseFrom(input);
                } else if (MarkdownCodeInline.CanParseFrom(input))
                {
                    result = MarkdownCodeInline.ParseFrom(input);
                } else if (MarkdownLink.CanParseFrom(input))
                {
                    result = MarkdownLink.ParseFrom(input);
                } else {
                    result = MarkdownText.ParseFrom(
                        input,
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
                        input,
                        true
                    );
                }
                // Extract parsed content
                foreach (IHtmlable entry in result.GetContent())
                {
                    content.AddLast(entry);
                }
                // Update text to be parsed
                input.FirstLine = result.Line;
            }
            IHtmlable[] contentArray = new IHtmlable[content.Count];
            content.CopyTo(
                contentArray, 
                0
            );
            return contentArray;
        }

        private void ParseReferencedUrls(
            string[] lines
        ) {
            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                ArraySegment<string> line = new ArraySegment<string>(
                    lines,
                    lineNumber,
                    1
                );
                if (ReferencedUrl.CanParseFrom(line))
                {
                    Urls.AddLast(
                        ReferencedUrl.ParseFrom(
                            line
                        )
                    );
                }
            }
        }
        
    }
}
