
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
            ArraySegment<string> lines
        ) {
            return lines[0].StartsWith(">");
        }

        public static ParseResult ParseFrom(
            ArraySegment<string> lines
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(lines))
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
                parser.Content
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
            while (
                index < lines.Count
                && !(
                    previousLineWasWhitespace
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
