
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class Emphasis : IMarkdownParser
    {
        private static Regex regexParseable = new Regex(
            @"^\*(.*?[^\\])\*"
            + @"|^_(.*?[^\\])_"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            return regexParseable.Match(input[0].Text).Success;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            string line = input[0].Text;
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                // Return a failed result if cannot parse from this line
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
            Element element = new ElementFactory().New(
                ElementType.Emphasis,
                MarkdownParser.ParseInnerText(
                    new ParseInput(
                        input,
                        content
                    )
                )
            );
            result.AddContent(element);
            input[0].Text = regexParseable.Replace(
                line,
                ""
            );
            result.Success = true;
            return result;
        }
    }
}