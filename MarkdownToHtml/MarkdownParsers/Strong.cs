
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class Strong : IMarkdownParser
    {
        private static Regex regexStrongText = new Regex(
            @"^\*{2}(.+)\*{2}"
            + @"|^_{2}(.+)_{2}"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            return regexStrongText.Match(input[0].Text).Success;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            string line = input[0].Text;
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                // Fail immediately if we cannot parse this text as strong
                return result;
            }
            Match contentMatch = regexStrongText.Match(
                line
            );
            string innerText = contentMatch.Groups[1].Value + contentMatch.Groups[2].Value;
            Element strong = new ElementFactory().New(
                ElementType.Strong,
                MarkdownParser.ParseInnerText(
                    new ParseInput(
                        input,
                        innerText
                    )
                )
            );
            result.AddContent(
                strong
            );
            input[0].Text = regexStrongText.Replace(
                line,
                ""
            );
            result.Success = true;
            return result;
        }
    }
}