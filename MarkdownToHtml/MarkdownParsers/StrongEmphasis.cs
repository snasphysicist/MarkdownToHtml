
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class StrongEmphasis : IMarkdownParser
    {
        private static Regex regexStrongEmphasisText = new Regex(
            @"^\*{3}(.*?[^\\])\*{3}"
            + @"|^_{3}(.*?[^\\])_{3}"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            return regexStrongEmphasisText.Match(input[0].Text).Success;
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
            Match contentMatch = regexStrongEmphasisText.Match(
                line
            );
            string innerText = contentMatch.Groups[1].Value + contentMatch.Groups[2].Value;
            Element strong = new ElementFactory().New(
                ElementType.Strong,
                new ElementFactory().New(
                    ElementType.Emphasis,
                    MarkdownParser.ParseInnerText(
                        new ParseInput(
                            input,
                            innerText
                        )
                    )
                )
            );
            result.AddContent(
                strong
            );
            input[0].Text = regexStrongEmphasisText.Replace(
                line,
                ""
            );
            result.Success = true;
            return result;
        }
    }
}