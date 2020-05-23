
using System;

namespace MarkdownToHtml
{
    public class MarkdownQuote : IHtmlable
    {

        IHtmlable[] content;

        const string tag = "blockquote";

        public const MarkdownElementType Type = MarkdownElementType.Quote;

        public MarkdownQuote(
            IHtmlable[] content
        ) {
            this.content = content;
        }

        public string ToHtml() 
        {
            string html = $"<{tag}>";
            foreach (IHtmlable htmlable in content)
            {
                html += htmlable.ToHtml();
            }
            html += $"</{tag}>";
            return html;
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
            int index = 1;
            bool previousLineWasWhitespace = false;
            /* 
             * Condition
             * Don't allow index to exceed the number of elements to avoid Exceptions
             * We want to break the loop when there is a whitespace line
             * (previousLineWasWhitespace)
             * followed by a non-whitespace line (!ContainsOnlyWhitespace(lines[index])) 
             * which is not a quote line !lines[index].StarsWith(">")
             */
            while (
                index < lines.Count
                && !(
                    previousLineWasWhitespace
                    && !ContainsOnlyWhitespace(lines[index])
                    && !lines[index].StartsWith(">")
                )
            ) {
                if (ContainsOnlyWhitespace(lines[index]))
                {
                    previousLineWasWhitespace = true;
                } else {
                    previousLineWasWhitespace = false;
                }
                index++;
            }
            return index;
        }

        private static bool ContainsOnlyWhitespace(
            string line
        ) {
            return line.Replace(
                " ",
                ""
            ).Length == 0;
        }

    }
}
