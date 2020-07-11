
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class Quote : IMarkdownParser
    {
        private Regex regexQuoteOpening = new Regex(
            @"^(\s*)(>)(\s*?.*)"
        );

        private int indentationLevel;

        public Quote(
            int indentationLevel
        ) {
            this.indentationLevel = indentationLevel;
        }

        public bool CanParseFrom(
            ParseInput input
        ) {
            return (
                regexQuoteOpening.Match(input[0].Text).Success
                && (input[0].IndentationLevel() == indentationLevel)
            );
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                return result;
            }
            int endQuoteSection = FindEndOfQuoteSection(
                input
            );
            string[] truncatedLines = new string[endQuoteSection];
            // Remove quote arrows and spaces, if needed
            for (int i = 0; i < endQuoteSection; i++)
            {
                string truncated = input[i].Text;
                if (regexQuoteOpening.Match(input[i].Text).Success)
                {
                    truncated = regexQuoteOpening.Replace(
                        input[i].Text,
                        "$3"
                    );
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
                    truncatedLines[i] = truncated;
                }
                // Remove original line
                input[i].WasParsed();
            }
            /* 
             * The truncated lines should be parsed as any other line group
             * and wrapped in a blockquote element
             */
            MarkdownParser parser = new MarkdownParser(
                truncatedLines
            );
            Element element = new ElementFactory().New(
                ElementType.Quote,
                parser.ContentAsArray()
            );
            result.Success = true;
            result.AddContent(
                element
            );
            return result;
        }

        private static int FindEndOfQuoteSection(
            ParseInput input
        ) {
            return Utils.FindEndOfSection(
                input,
                ">"
            );
        }
    }
}