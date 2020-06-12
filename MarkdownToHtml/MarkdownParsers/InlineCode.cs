
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class InlineCode : IMarkdownParser
    {
        private static Regex regexParseable = new Regex(
            @"^`.*([^`])`.*"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            return regexParseable.Match(input.FirstLine).Success;
        }

        // Shared code for parsing emphasis sections
        public ParseResult ParseFrom(
            ParseInput input
        ) {
            string line = input.FirstLine;
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                // Fail immediately if this string cannot be parsed
                result.Line = line;
                return result;
            }
            int j = 1;
            // Find closing `
            while (
                (j < line.Length)
                && !(
                    (line[j] == '`')
                    && (line[j-1] != '\\')
                )
            ) {
                j++;
            }
            if (j >= line.Length)
            {
                // If we cannot parse, then return line as is
                result.Line = line;
                return result;
            }
            // Parse everything inside the backticks
            Element element = new ElementFactory().New(
                ElementType.CodeInline,
                MarkdownParser.ParseInnerText(
                    new ParseInput(
                        input,
                        line.Substring(1, j - 1)
                    )
                )
            );
            result.AddContent(element);
            input.FirstLine = line.Substring(j + 1);
            result.Success = true;
            return result;
        }
    }
}