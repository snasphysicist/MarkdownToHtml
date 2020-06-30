
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownParser
    {
        private static IMarkdownParser[] multilineElementParsers
            = new IMarkdownParser[]
            {
                new PreformattedCodeBlock(),
                new DoubleLineHeading(),
                new SingleLineHeading(),
                new HorizontalRule(),
                new Quote(
                    0
                ),
                new CodeBlock(),
                new List(
                    0
                ),
                new Paragraph()
            };

        private static IMarkdownParser[] innerTextParsers
            = new IMarkdownParser[]
            {
                new Strong(),
                new Strikethrough(),
                new Emphasis(),
                new InlineCode(),
                new Link(),
                new Image()
            };

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
            ParseReferencedUrls(
                lines
            );
            // Create new parse input to pass to parsers
            ParseInput input = new ParseInput(
                Urls.ToArray(),
                lines,
                0,
                lines.Length
            );
            // Parsing printed content
            while (input.Count > 0) {
                // Parse some lines
                ParseLineGroup(
                    input
                );
                // Move to the next non-empty line
                while (
                    (input.Count > 0)
                    && (input[0].HasBeenParsed())
                ) {
                    input.NextLine();
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
            ParseResult result = new ParseResult();
            foreach (IMarkdownParser parser in multilineElementParsers)
            {
                if (
                    parser.CanParseFrom(
                        input
                    )
                ) {
                    result = parser.ParseFrom(
                        input
                    );
                    if (result.Success)
                    {
                        break;
                    }
                }
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
            while (!input[0].HasBeenParsed())
            {
                ParseResult result = new ParseResult();
                foreach (IMarkdownParser parser in innerTextParsers)
                {
                    if (
                        parser.CanParseFrom(
                            input
                        )
                    ) {
                        result = parser.ParseFrom(
                            input
                        );
                        if (result.Success)
                        {
                            break;
                        }
                    }
                }
                if (!result.Success)
                {
                    result = MarkdownText.ParseFrom(
                        input,
                        false
                    );
                    if (!result.Success)
                    {
                        // Ensure at least one character gets parsed
                        result = MarkdownText.ParseFrom(
                            input,
                            true
                        );
                    }
                }
                if (input[0].Text.Length == 0)
                {
                    input[0].WasParsed();
                }
                // Extract parsed content
                foreach (IHtmlable entry in result.GetContent())
                {
                    content.AddLast(entry);
                }
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
