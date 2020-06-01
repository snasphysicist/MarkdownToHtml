
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownStrong : MarkdownElement, IHtmlable
    {

        private static Regex regexParseable = new Regex(
            @"^\*{2}.+\*{2}.*"
            + @"|^_{2}.+_{2}.*"
        );

        public MarkdownStrong(
            IHtmlable[] content
        ) {
            Type = MarkdownElementType.Strong;
            this.content = content;
        }

        public static bool CanParseFrom(
            ParseInput input
        ) {
            return regexParseable.Match(input.FirstLine).Success;
        }

        public static ParseResult ParseFrom(
            ParseInput input
        ) {
            string line = input.FirstLine;
            if (!CanParseFrom(input))
            {
                // Return a failed result if this cannot be parsed
                ParseResult result = new ParseResult();
                result.Line = line;
                return result;
            }
            // Otherwise, parse and return result
            if (line.StartsWith("**"))
            {
                return ParseStrongSection(
                    input,
                    "**"
                );
            } else {
                return ParseStrongSection(
                    input,
                    "__"
                );
            }
        }

        // Shared code for parsing strong sections
        private static ParseResult ParseStrongSection(
            ParseInput input,
            string delimiter
        ) {
            string line = input.FirstLine;
            ParseResult result = new ParseResult();
            int j = 2;
            // Find closing two characters
            while (
                (j < line.Length)
                && !(
                    (line.Substring(j-1, 2) == delimiter)
                    && (line[j-2] != '\\')
                )
            ) {
                j++;
            }
            if (j >= line.Length)
            {
                // Fail if closing characters cannot be found
                result.Line = line;
                return result;
            }
            // Parse everything inside the strong section delimiters
            MarkdownStrong element = new MarkdownStrong(
                MarkdownParser.ParseInnerText(
                    new ParseInput(
                        input,
                        line.Substring(2, j - 3)
                    )
                )
            );
            result.AddContent(element);
            result.Line = line.Substring(j + 1);
            result.Success = true;
            // Return the line string minus the content we parsed
            return result;
        }

    }
}
