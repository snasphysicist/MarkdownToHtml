
using System;

namespace MarkdownToHtml
{
    public class MarkdownQuote : MarkdownElementWithContent, IHtmlable
    {
        public MarkdownQuote(
            IHtmlable[] content
        ) {
            Type = MarkdownElementType.Quote;
            this.content = content;
        }

        public static bool CanParseFrom(
            ParseInput input
        ) {
            return input.FirstLine.StartsWith(">");
        }

        public static ParseResult ParseFrom(
            ParseInput input
        ) {
            ArraySegment<string> lines = input.Lines();
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                return result;
            }
            int endQuoteSection = FindEndOfQuoteSection(
                lines
            );
            string[] truncatedLines = new string[endQuoteSection];
            // Remove quote arrows and spaces, if needed
            for (int i = 0; i < endQuoteSection; i++)
            {
                string truncated = lines[i];
                if (lines[i].StartsWith(">"))
                {
                    truncated = truncated.Substring(1);
                    int spaces = 0;
                    // Count spaces
                    while(
                        (spaces < truncated.Length)
                        && (truncated[spaces] == ' ')
                    ) {
                        spaces++;
                    }
                    // If there are fewer than 5 spaces, remove all
                    if (spaces < 5)
                    {
                        truncatedLines[i] = truncated.Substring(spaces);
                    } else {
                        // More than five, just remove one space
                        truncatedLines[i] = truncated.Substring(1);
                    }
                } else {
                    truncatedLines[i] = lines[i];
                }
                // Remove original line
                lines[i] = "";
            }
            /* 
             * The truncated lines should be parsed as any other line group
             * and wrapped in a blockquote element
             */
            MarkdownParser parser = new MarkdownParser(
                truncatedLines
            );

            MarkdownQuote quoteElement = new MarkdownQuote(
                parser.ContentAsArray()
            );
            result.Success = true;
            result.AddContent(
                quoteElement
            );
            return result;
        }

        private static int FindEndOfQuoteSection(
            ArraySegment<string> lines
        ) {
            return Utils.FindEndOfSection(
                lines,
                ">"
            );
        }
    }
}
