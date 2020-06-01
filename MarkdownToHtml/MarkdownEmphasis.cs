
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownEmphasis : MarkdownElement, IHtmlable
    {

        private static Regex regexParseable = new Regex(
            @"^\*(.*?[^\\])\*"
            + @"|^_(.*?[^\\])_"
        );

        public MarkdownEmphasis(
            IHtmlable[] content
        ) {
            Type = MarkdownElementType.Emphasis;
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
                // Return a failed result if cannot parse from this line
                result.Line = line;
                return result;
            }
            // Otherwise parse and return result
            Match contentMatch = regexParseable.Match(line);
            string content;
            if (contentMatch.Groups[1].Value.Length != 0)
            {
                content = contentMatch.Groups[1].Value;
            } else {
                content = contentMatch.Groups[2].Value;
            }
            // Parse everything inside the stars
            MarkdownEmphasis element = new MarkdownEmphasis(
                MarkdownParser.ParseInnerText(
                    new ParseInput(
                        input,
                        content
                    )
                )
            );
            result.AddContent(element);
            result.Line = line.Substring(
                content.Length + 2
            );
            result.Success = true;
            return result;
        }

    }
}