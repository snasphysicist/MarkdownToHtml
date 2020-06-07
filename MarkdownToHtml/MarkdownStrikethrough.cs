
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownStrikethrough : MarkdownElementWithContent, IHtmlable
    {

        private static Regex regexParseable = new Regex(
            @"^~{2}.+~{2}.*"
        );

        public MarkdownStrikethrough(
            IHtmlable[] content
        ) {
            Type = MarkdownElementType.Strikethrough;
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
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                // Fail immediately if we cannot parse this text as strikethrough
                result.Line = line;
                return result;
            }
            int j = 2;
            // Find closing tildes
            while (
                (j < line.Length)
                && !(
                    (line.Substring(j-1, 2) == "~~")
                    && (line[j-2] != '\\')
                )
            ) {
                j++;
            }
            if (j >= line.Length)
            {
                // Fail if we cannot find the closing squiggles
                result.Line = line;
                return result;
            }
            // Parse everything inside the stars
            MarkdownStrikethrough element = new MarkdownStrikethrough(
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
            return result;
        }

    }
}
