
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownHorizontalRule : IHtmlable
    {

        static Regex regexHorizontalRule = new Regex(
            @"^-{3,}"
            + @"^\*{3,}"
            + @"^\+{3,}"
        );

        string tag = "hr";

        public const MarkdownElementType Type = MarkdownElementType.HorizontalRule;

        public MarkdownHorizontalRule() 
        {}

        public string ToHtml() {
            return $"<{tag}>";
        }

        public static bool CanParseFrom(
            ArraySegment<string> lines
        ) {
            return regexHorizontalRule.Match(lines[0]).Success;
        }

        public static ParseResult ParseFrom(
            ArraySegment<string> lines
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(lines))
            {
                return result;
            }
            lines[0] = "";
            result.Success = true;
            result.AddContent(
                new MarkdownHorizontalRule()
            );
            return result;
        }

    }
}
