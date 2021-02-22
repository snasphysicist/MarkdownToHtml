
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class HorizontalRule : IMarkdownParser
    {
        static Regex regexHorizontalRule = new Regex(
            @"^[\s|\*]{3,}$"
            + @"|^[\s|-]{3,}$"
            + @"|^[\s|_]{3,}$"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            string line = input[0].Text;
            bool correctFormat = regexHorizontalRule.Match(line).Success;
            // Check there are enough - or * characters (3++)
            bool enoughNonWhitespace = (
                (line.Length - line.Replace("*", "").Replace("-", "").Replace("_", "").Length)
                > 2
            );
            return correctFormat && enoughNonWhitespace;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                return result;
            }
            input[0].WasParsed();
            result.Success = true;
            result.AddContent(
                new ElementFactory().New(
                    ElementType.HorizontalRule
                )
            );
            return result;
        }
    }
}