
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class InlineCode : IMarkdownParser
    {
        private int QuotesAtStart(
            string line
        ) {
            int quotes = 0;
            while (
                quotes < line.Length
                && line.Substring(quotes, 1) == "`"
            ) {
                quotes++;
            }
            return quotes;
        }

        private bool IsCharEscapedAt(
            string line,
            int at
        ) {
            int checkForSlash = at - 1;
            bool isEscaped = false;
            while (
                checkForSlash >= 0
                && line[checkForSlash] == '\\'
            ) {
                checkForSlash--;
                isEscaped = !isEscaped;
            }
            return isEscaped;
        }

        private int FindClosingQuotes(
            string line,
            string quoteSection
        ) {
            int numberOfQuotes = quoteSection.Length;
            int location = numberOfQuotes;
            while (
                location < (line.Length - numberOfQuotes)
                && (
                    line.Substring(
                        location, 
                        numberOfQuotes
                    ) != quoteSection
                    || IsCharEscapedAt(
                        line, 
                        location
                    )
                )
            ) {
                location++;
            }
            if (
                (location == (line.Length - numberOfQuotes))
                && !line.EndsWith(
                    quoteSection
                )
            ) {
                return line.Length;
            } else {
                return location;
            }
        }

        public bool CanParseFrom(
            ParseInput input
        ) {
            int quotesAtStart = QuotesAtStart(
                input[0].Text
            );
            int locationOfClosingQuotes = FindClosingQuotes(
                input[0].Text,
                input[0].Text.Substring(0, quotesAtStart)
            );
            return (
                quotesAtStart > 0
                && locationOfClosingQuotes != input[0].Text.Length
            );
        }

        // Shared code for parsing emphasis sections
        public ParseResult ParseFrom(
            ParseInput input
        ) {
            string line = input[0].Text;
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                // Fail immediately if this string cannot be parsed
                result.Line = line;
                return result;
            }
            int quotesAtStart = QuotesAtStart(
                input[0].Text
            );
            int locationOfClosingQuotes = FindClosingQuotes(
                input[0].Text,
                input[0].Text.Substring(0, quotesAtStart)
            );
            if (locationOfClosingQuotes == input[0].Text.Length)
            {
                // If we cannot parse, then return line as is
                result.Line = line;
                return result;
            }
            // Parse everything inside the backticks
            Element element = new ElementFactory().New(
                ElementType.CodeInline,
                MarkdownText.EscapingReplacedHtml(
                    line.Substring(
                        quotesAtStart, 
                        locationOfClosingQuotes - quotesAtStart
                    ),
                    input.Replacements
                )
            );
            result.AddContent(element);
            input[0].Text = line.Substring(locationOfClosingQuotes + quotesAtStart);
            result.Success = true;
            return result;
        }
    }
}